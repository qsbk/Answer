
namespace UniversalFramework.Framework.Pool {
    public interface ObjectFactory<T>{
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <returns></returns>
        T createNew();

    }
}