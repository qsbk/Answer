using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Answerquestions {
    internal class CacheEntry : ICacheEntry {
        private bool _added = false;
        private static readonly Action<object> ExpirationCallback = ExpirationTokensExpired;
        
        private IList<PostEvictionCallbackRegistration> _postEvictionCallbacks;
        internal IList<IChangeToken> _expirationTokens;
        private IList<IDisposable> _expirationTokenRegistrations;
        private IDisposable _scope;
        private readonly Action<CacheEntry> _notifyCacheOfExpiration;
        private readonly Action<CacheEntry> _notifyCacheEntryDisposed;
        private bool _isExpired;

        internal DateTimeOffset? _absoluteExpiration;
        internal TimeSpan? _absoluteExpirationRelativeToNow;
        private TimeSpan? _slidingExpiration;
        private long? _size;

        internal readonly object _lock = new object();
        internal CacheEntry(
            object key,
            Action<CacheEntry> notifyCacheEntryDisposed,
            Action<CacheEntry> notifyCacheOfExpiration) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if (notifyCacheEntryDisposed == null) {
                throw new ArgumentNullException(nameof(notifyCacheEntryDisposed));
            }

            if (notifyCacheOfExpiration == null) {
                throw new ArgumentNullException(nameof(notifyCacheOfExpiration));
            }

            Key = key;
            _notifyCacheEntryDisposed = notifyCacheEntryDisposed;
            _notifyCacheOfExpiration = notifyCacheOfExpiration;
        }

        public object Key { get; private set; }
        /// <summary>
        /// 获取或设置缓存的绝对过期时间
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration {
            get {
                return _absoluteExpiration;
            }
            set {
                _absoluteExpiration = value;
            }
        }
        /// <summary>
        /// 获取或设置绝对过期时间
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow {
            get {
                return _absoluteExpirationRelativeToNow;
            }
            set {
                if (value <= TimeSpan.Zero) {
                    throw new ArgumentOutOfRangeException(
                        nameof(AbsoluteExpirationRelativeToNow),
                        value,
                        "The relative expiration value must be positive.");
                }

                _absoluteExpirationRelativeToNow = value;
            }
        }
        /// <summary>
        /// 获取或设置非活动时间
        /// </summary>
        public TimeSpan? SlidingExpiration {
            get {
                return _slidingExpiration;
            }
            set {
                if (value <= TimeSpan.Zero) {
                    throw new ArgumentOutOfRangeException(
                        nameof(SlidingExpiration),
                        value,
                        "The sliding expiration value must be positive.");
                }
                _slidingExpiration = value;
            }
        }
        /// <summary>
        /// 获取或设置缓存优先级
        /// memory pressure triggered cleanup. The default is <see cref="CacheItemPriority.Normal"/>.
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;
        /// <summary>
        /// 缓存条目大小
        /// </summary>
        public long? Size {
            get => _size;
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be non-negative.");
                }

                _size = value;
            }
        }
        public object Value { get; set; }
        
        internal EvictionReason EvictionReason { get; private set; }
        internal DateTimeOffset LastAccessed { get; set; }

        internal void InvokeEvictionCallbacks() {
            if (_postEvictionCallbacks != null) {
                Task.Factory.StartNew(state => InvokeCallbacks((CacheEntry)state), this,
                    CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }
        }
        private static void InvokeCallbacks(CacheEntry entry) {
            var callbackRegistrations = Interlocked.Exchange(ref entry._postEvictionCallbacks, null);

            if (callbackRegistrations == null) {
                return;
            }

            for (int i = 0; i < callbackRegistrations.Count; i++) {
                var registration = callbackRegistrations[i];

                try {
                    registration.EvictionCallback?.Invoke(entry.Key, entry.Value, entry.EvictionReason, registration.State);
                } catch (Exception) {
                    // This will be invoked on a background thread, don't let it throw.
                    // TODO: LOG
                }
            }
        }

        internal void SetExpired(EvictionReason reason) {
            if (EvictionReason == EvictionReason.None) {
                EvictionReason = reason;
            }
            _isExpired = true;
            DetachTokens();
        }
        private bool CheckForExpiredTime(DateTimeOffset now) {
            if (_absoluteExpiration.HasValue && _absoluteExpiration.Value <= now) {
                SetExpired(EvictionReason.Expired);
                return true;
            }

            if (_slidingExpiration.HasValue
                && (now - LastAccessed) >= _slidingExpiration) {
                SetExpired(EvictionReason.Expired);
                return true;
            }

            return false;
        }

        internal bool CheckExpired(DateTimeOffset now) {
            return _isExpired || CheckForExpiredTime(now) || CheckForExpiredTokens();
        }


        private static void ExpirationTokensExpired(object obj) {
            // start a new thread to avoid issues with callbacks called from RegisterChangeCallback
            Task.Factory.StartNew(state => {
                var entry = (CacheEntry)state;
                entry.SetExpired(EvictionReason.TokenExpired);
                entry._notifyCacheOfExpiration(entry);
            }, obj, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        internal bool CheckForExpiredTokens() {
            if (_expirationTokens != null) {
                for (int i = 0; i < _expirationTokens.Count; i++) {
                    var expiredToken = _expirationTokens[i];
                    if (expiredToken.HasChanged) {
                        SetExpired(EvictionReason.TokenExpired);
                        return true;
                    }
                }
            }
            return false;
        }

        private void DetachTokens() {
            lock (_lock) {
                var registrations = _expirationTokenRegistrations;
                if (registrations != null) {
                    _expirationTokenRegistrations = null;
                    for (int i = 0; i < registrations.Count; i++) {
                        var registration = registrations[i];
                        registration.Dispose();
                    }
                }
            }
        }

        public void Dispose() {
            if (!_added) {
                _added = true;
                _scope.Dispose();
                _notifyCacheEntryDisposed(this);
                PropagateOptions(CacheEntryHelper.Current);
            }
        }


        internal void PropagateOptions(CacheEntry parent) {
            if (parent == null) {
                return;
            }

            // Copy expiration tokens and AbsoluteExpiration to the cache entries hierarchy.
            // We do this regardless of it gets cached because the tokens are associated with the value we'll return.
            if (_expirationTokens != null) {
                lock (_lock) {
                    lock (parent._lock) {
                        foreach (var expirationToken in _expirationTokens) {
                            //parent.AddExpirationToken(expirationToken);
                        }
                    }
                }
            }

            if (_absoluteExpiration.HasValue) {
                if (!parent._absoluteExpiration.HasValue || _absoluteExpiration < parent._absoluteExpiration) {
                    parent._absoluteExpiration = _absoluteExpiration;
                }
            }
        }



    }
}