

namespace UniversalFramework.Framework.Pool {
    /// <summary>
    /// 验证对象池内对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Validator<T> {
        /// <summary>
        /// 对象是否有效
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool isValid(T t);
        /// <summary>
        /// 将对象置于无效
        /// </summary>
        /// <param name="t"></param>
        void invalidate(T t);

    }

}