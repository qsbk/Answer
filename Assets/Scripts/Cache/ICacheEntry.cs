using System;

namespace Answerquestions {
    public interface ICacheEntry : IDisposable {
        /// <summary>
        /// 设置缓存的绝对过期时间
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        /// <summary>
        /// 获取或设置相对于当前时间的绝对到期时间
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        /// <summary>
        /// 获取缓存的键
        /// </summary>
        public object Key { get; }
        /// <summary>
        /// 获取或设置在清理过程中缓存中保留缓存中的缓存项的优先级。 默认值为 Normal
        /// </summary>
        public CacheItemPriority Priority { get; set; }
        /// <summary>
        /// 获取或设置缓存项值的大小
        /// </summary>
        public long? Size { get; set; }
        /// <summary>
        /// 获取或设置缓存项在被删除之前可以处于停用状态（例如不被访问）的时长。 这不会将项生存期延长到超过绝对到期时间（如果已设置）
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }
        /// <summary>
        /// 获取或设置缓存项的值
        /// </summary>
        public object Value { get; set; }
    }
}
/// <summary>
/// 指定在内存压力触发的清理过程中，如何优先保留项目
/// </summary>
public enum CacheItemPriority {
    High = 2,
    Low = 0,
    Normal = 1,
    NeverRemove = 3,
}