
namespace UniversalFramework.Framework.Pool {
    public abstract class AbstractPool<T> : Pool<T> {
        public abstract T get();
        public void release(T t) {
            if (isValid(t)) {
                returnToPool(t);
            } else {
                handleInvalidReturn(t);
            }
        }
        public abstract void shutdown();
        protected abstract void handleInvalidReturn(T t);
        protected abstract void returnToPool(T t);
        public abstract bool isValid(T t);
        public abstract void invalidate(T t);
    }
}

