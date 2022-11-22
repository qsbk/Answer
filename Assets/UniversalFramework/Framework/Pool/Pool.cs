/// <summary>
/// 对象池接口
/// </summary>
namespace UniversalFramework.Framework.Pool {
    public interface Pool<T> : Validator<T> {
        /// <summary>
        /// 释放一个对象
        /// </summary>
        /// <param name="t">要释放的对象</param>
        void release(T t);
        /// <summary>
        /// 释放资源
        /// </summary>
        void shutdown();
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        T get();
    }

}
    
