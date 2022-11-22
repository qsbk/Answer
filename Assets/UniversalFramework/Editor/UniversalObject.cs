using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniversalFramework{
    public class UniversalObject{

        /// <summary>
        /// 获取场景中所有目标对象(包括不激活的对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_SceneName"></param>
        /// <returns></returns>
        public static List<T> FindSceneObject<T>(string _SceneName) where T : UnityEngine.Component {
            List<T> objectsInScene = new List<T>();
            foreach (var go in Resources.FindObjectsOfTypeAll<T>()) {
                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;
                if (EditorUtility.IsPersistent(go.transform.root.gameObject))// 如果对象位于Scene中，则返回false
                    continue;
                if (_SceneName != go.gameObject.scene.name)
                    continue;
                objectsInScene.Add(go);
            }
            return objectsInScene;
        }
        

    }
}