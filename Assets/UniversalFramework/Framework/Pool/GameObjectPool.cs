using UnityEditor;
using UnityEngine;

namespace UniversalFramework.Framework.Pool {
    public class GameObjectPool<T> : AbstractPool<T> {
        public override T get() {
            throw new System.NotImplementedException();
        }

        public override void invalidate(T t) {
            throw new System.NotImplementedException();
        }

        public override bool isValid(T t) {
            throw new System.NotImplementedException();
        }

        public override void shutdown() {
            throw new System.NotImplementedException();
        }

        protected override void handleInvalidReturn(T t) {
            throw new System.NotImplementedException();
        }

        protected override void returnToPool(T t) {
            throw new System.NotImplementedException();
        }
    }
}