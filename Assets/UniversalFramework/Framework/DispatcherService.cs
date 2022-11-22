using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UniversalFramework;
using UniversalFramework.Framework;
using System.Reflection;

namespace UniversalFramework {
    public class DispatcherService : MonoBehaviour {
        [Tag_Component]
        public Button www;
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //private static void init() {
        //    List<MonoBehaviour> monos = UniversalObject.FindSceneObject<MonoBehaviour>("SampleScene");
        //    foreach (var item in monos) {
        //        Type type = item.GetType();
        //        foreach (var field in type.GetFields()) {
        //            Log.Debug(field.FieldType);
        //            foreach (var attr in Attribute.GetCustomAttributes(field)) {
        //                Type fieldType = field.FieldType;

        //                string name = ((Tag_Component)attr).Id;
        //                Component c = item.gameObject.GetComponent(fieldType);

        //                field.SetValue(item, c);

        //            }
        //        }
        //    }
        //}
    }
      
}