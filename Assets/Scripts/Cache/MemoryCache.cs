using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Answerquestions
{
	public class MemoryCache
	{
		private readonly ConcurrentDictionary<object, CacheEntry> _entries;
		private long _cacheSize = 0;
		private bool _disposed;
		//在添加新的缓存项的时候触发该action
		private readonly Action<CacheEntry> _setEntry;
		private readonly Action<CacheEntry> _entryExpirationNotification;
		private DateTimeOffset _lastExpirationScan;
		private readonly MemoryCacheOptions _options;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optionsAccessor">cache的配置选项</param>
        public MemoryCache(IOptions<MemoryCacheOptions> optionsAccessor) {
            if (optionsAccessor == null) {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;

            _entries = new ConcurrentDictionary<object, CacheEntry>();
            _setEntry = SetEntry;
            _entryExpirationNotification = EntryExpired;

            _lastExpirationScan = DateTime.UtcNow;
        }
        /// <summary>
        /// Cleans up the background collection events.
        /// </summary>
        ~MemoryCache() {
            Dispose(false);
        }

        /// <summary>
        /// 获取当前缓存数量
        /// </summary>
        public int Count {
            get { return _entries.Count; }
        }
        // internal for testing
        internal long Size { get => Interlocked.Read(ref _cacheSize); }
        private ICollection<KeyValuePair<object, CacheEntry>> EntriesCollection => _entries;
        public ICacheEntry CreateEntry(object key) {
            CheckDisposed();

            ValidateCacheKey(key);

            return new CacheEntry(
                key,
                _setEntry,
                _entryExpirationNotification
            );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void SetEntry(CacheEntry entry) {
            if (_disposed) {
                //无操作而不是抛出异常，因为它在 CacheEntry.Dispose 期间被调用
                return;
            }

            if (_options.SizeLimit.HasValue && !entry.Size.HasValue) {
                throw new InvalidOperationException($"Cache entry must specify a value for {nameof(entry.Size)} when {nameof(_options.SizeLimit)} is set.");
            }

            var utcNow = DateTime.UtcNow;

            DateTimeOffset? absoluteExpiration = null;
            if (entry._absoluteExpirationRelativeToNow.HasValue) {
                absoluteExpiration = utcNow + entry._absoluteExpirationRelativeToNow;
            } else if (entry._absoluteExpiration.HasValue) {
                absoluteExpiration = entry._absoluteExpiration;
            }

            // Applying the option's absolute expiration only if it's not already smaller.
            // This can be the case if a dependent cache entry has a smaller value, and
            // it was set by cascading it to its parent.
            if (absoluteExpiration.HasValue) {
                if (!entry._absoluteExpiration.HasValue || absoluteExpiration.Value < entry._absoluteExpiration.Value) {
                    entry._absoluteExpiration = absoluteExpiration;
                }
            }

            //初始化添加条目时的最后访问时间
            entry.LastAccessed = utcNow;

            if (_entries.TryGetValue(entry.Key, out CacheEntry priorEntry)) {
                priorEntry.SetExpired(EvictionReason.Replaced);
            }

            var exceedsCapacity = UpdateCacheSizeExceedsCapacity(entry);

            if (!entry.CheckExpired(utcNow) && !exceedsCapacity) {
                var entryAdded = false;

                if (priorEntry == null) {
                    //如果不存在以前的条目，则尝试添加新条目
                    entryAdded = _entries.TryAdd(entry.Key, entry);
                } else {
                    //如果以前的条目存在，则尝试使用新条目进行更新
                    entryAdded = _entries.TryUpdate(entry.Key, entry, priorEntry);

                    if (entryAdded) {
                        if (_options.SizeLimit.HasValue) {
                            //前一个条目被删除，减少前一个条目的大小
                            Interlocked.Add(ref _cacheSize, -priorEntry.Size.Value);
                        }
                    } else {
                        // The update will fail if the previous entry was removed after retrival.
                        // Adding the new entry will succeed only if no entry has been added since.
                        // This guarantees removing an old entry does not prevent adding a new entry.
                        entryAdded = _entries.TryAdd(entry.Key, entry);
                    }
                }

                if (entryAdded) {
                    //entry.AttachTokens();
                } else {
                    if (_options.SizeLimit.HasValue) {
                        // 无法添加条目，重置缓存大小
                        Interlocked.Add(ref _cacheSize, -entry.Size.Value);
                    }
                    entry.SetExpired(EvictionReason.Replaced);
                    entry.InvokeEvictionCallbacks();
                }

                if (priorEntry != null) {
                    priorEntry.InvokeEvictionCallbacks();
                }
            } else {
                if (exceedsCapacity) {
                    // 由于容量过剩，没有添加条目
                    entry.SetExpired(EvictionReason.Capacity);

                    TriggerOvercapacityCompaction();
                }

                entry.InvokeEvictionCallbacks();
                if (priorEntry != null) {
                    RemoveEntry(priorEntry);
                }
            }

            StartScanForExpiredItems();
        }
        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGetValue(object key, out object result) {
            ValidateCacheKey(key);

            CheckDisposed();

            result = null;
            var utcNow = DateTime.UtcNow;
            var found = false;

            if (_entries.TryGetValue(key, out CacheEntry entry)) {
                // 检查是过期令牌、计时器等而过期，如果是，则将其删除。
                // 由于在 SetEntry 期间对 SetExpired 的并发调用，允许返回一个过时的 Replaced 值
                if (entry.CheckExpired(utcNow) && entry.EvictionReason != EvictionReason.Replaced) {
                    // TODO：为了提高效率，排队等待批量删除
                    RemoveEntry(entry);
                } else {
                    found = true;
                    entry.LastAccessed = utcNow;
                    result = entry.Value;

                    // When this entry is retrieved in the scope of creating another entry,
                    // that entry needs a copy of these expiration tokens.
                    entry.PropagateOptions(CacheEntryHelper.Current);
                }
            }

            StartScanForExpiredItems();

            return found;
        }
        public void Remove(object key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            CheckDisposed();
            if (_entries.TryRemove(key, out CacheEntry entry)) {
                if (_options.SizeLimit.HasValue) {
                    Interlocked.Add(ref _cacheSize, -entry.Size.Value);
                }

                entry.SetExpired(EvictionReason.Removed);
                entry.InvokeEvictionCallbacks();
            }

            StartScanForExpiredItems();
        }

        private void RemoveEntry(CacheEntry entry) {
            if (EntriesCollection.Remove(new KeyValuePair<object, CacheEntry>(entry.Key, entry))) {
                if (_options.SizeLimit.HasValue) {
                    Interlocked.Add(ref _cacheSize, -entry.Size.Value);
                }
                entry.InvokeEvictionCallbacks();
            }
        }

        private void EntryExpired(CacheEntry entry) {
            // TODO: For efficiency consider processing these expirations in batches.
            RemoveEntry(entry);
            StartScanForExpiredItems();
        }


        // 查看扫描过期时间，如果符合条件，启动扫描
        private void StartScanForExpiredItems() {
            var now = DateTime.UtcNow;
            if (_options.ExpirationScanFrequency < now - _lastExpirationScan) {
                _lastExpirationScan = now;
                Task.Factory.StartNew(state => ScanForExpiredItems((MemoryCache)state), this,
                    CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }
        }
        private static void ScanForExpiredItems(MemoryCache cache) {
            var now = DateTime.UtcNow;
            foreach (var entry in cache._entries.Values) {
                if (entry.CheckExpired(now)) {
                    cache.RemoveEntry(entry);
                }
            }
        }



        private bool UpdateCacheSizeExceedsCapacity(CacheEntry entry) {
            if (!_options.SizeLimit.HasValue) {
                return false;
            }

            var newSize = 0L;
            for (var i = 0; i < 100; i++) {
                var sizeRead = Interlocked.Read(ref _cacheSize);
                newSize = sizeRead + entry.Size.Value;

                if (newSize < 0 || newSize > _options.SizeLimit) {
                    // 发生溢出，不更新缓存大小返回true
                    return true;
                }

                if (sizeRead == Interlocked.CompareExchange(ref _cacheSize, newSize, sizeRead)) {
                    return false;
                }
            }

            return true;
        }


        private void TriggerOvercapacityCompaction() {
            // Spawn background thread for compaction
            ThreadPool.QueueUserWorkItem(s => OvercapacityCompaction((MemoryCache)s), this);
        }

        private static void OvercapacityCompaction(MemoryCache cache) {
            var currentSize = Interlocked.Read(ref cache._cacheSize);
            var lowWatermark = cache._options.SizeLimit * (1 - cache._options.CompactionPercentage);
            if (currentSize > lowWatermark) {
                cache.Compact(currentSize - (long)lowWatermark, entry => entry.Size.Value);
            }
        }
        /// Remove at least the given percentage (0.10 for 10%) of the total entries (or estimated memory?), according to the following policy:
        /// 1. Remove all expired items.
        /// 2. Bucket by CacheItemPriority.
        /// 3. Least recently used objects.
        /// ?. Items with the soonest absolute expiration.
        /// ?. Items with the soonest sliding expiration.
        /// ?. Larger objects - estimated by object graph size, inaccurate.
        public void Compact(double percentage) {
            int removalCountTarget = (int)(_entries.Count * percentage);
            Compact(removalCountTarget, _ => 1);
        }

        private void Compact(long removalSizeTarget, Func<CacheEntry, long> computeEntrySize) {
            var entriesToRemove = new List<CacheEntry>();
            var lowPriEntries = new List<CacheEntry>();
            var normalPriEntries = new List<CacheEntry>();
            var highPriEntries = new List<CacheEntry>();
            long removedSize = 0;

            // Sort items by expired & priority status
            var now = DateTime.UtcNow;
            foreach (var entry in _entries.Values) {
                if (entry.CheckExpired(now)) {
                    entriesToRemove.Add(entry);
                    removedSize += computeEntrySize(entry);
                } else {
                    switch (entry.Priority) {
                        case CacheItemPriority.Low:
                            lowPriEntries.Add(entry);
                            break;
                        case CacheItemPriority.Normal:
                            normalPriEntries.Add(entry);
                            break;
                        case CacheItemPriority.High:
                            highPriEntries.Add(entry);
                            break;
                        case CacheItemPriority.NeverRemove:
                            break;
                        default:
                            throw new NotSupportedException("Not implemented: " + entry.Priority);
                    }
                }
            }
            ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, entriesToRemove, lowPriEntries);
            ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, entriesToRemove, normalPriEntries);
            ExpirePriorityBucket(ref removedSize, removalSizeTarget, computeEntrySize, entriesToRemove, highPriEntries);
            foreach (var entry in entriesToRemove) {
                RemoveEntry(entry);
            }
        }


        /// Policy:
        /// 1. Least recently used objects.
        /// ?. Items with the soonest absolute expiration.
        /// ?. Items with the soonest sliding expiration.
        /// ?. Larger objects - estimated by object graph size, inaccurate.
        private void ExpirePriorityBucket(ref long removedSize, long removalSizeTarget, Func<CacheEntry, long> computeEntrySize, List<CacheEntry> entriesToRemove, List<CacheEntry> priorityEntries) {
            // Do we meet our quota by just removing expired entries?
            if (removalSizeTarget <= removedSize) {
                // No-op, we've met quota
                return;
            }

            // Expire enough entries to reach our goal
            // TODO: Refine policy

            // LRU
            foreach (var entry in priorityEntries.OrderBy(entry => entry.LastAccessed)) {
                entry.SetExpired(EvictionReason.Capacity);
                entriesToRemove.Add(entry);
                removedSize += computeEntrySize(entry);

                if (removalSizeTarget <= removedSize) {
                    break;
                }
            }
        }


        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    GC.SuppressFinalize(this);
                }

                _disposed = true;
            }
        }
        private void CheckDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(typeof(MemoryCache).FullName);
            }
        }
        private static void ValidateCacheKey(object key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }
        }


    }

}