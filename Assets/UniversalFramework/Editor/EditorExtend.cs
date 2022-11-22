using UnityEditor;
using UnityEngine;
using UniversalFramework;

namespace UniversalFramework {
    public class EditorExtend : UnityEditor.AssetModificationProcessor {
        [MenuItem("Universal/Log Level/Debug")]
        static void SetLogLevelDebug() {
            Log.logLevel = LogLevel.Debug;
            //EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
        [MenuItem("Universal/Log Level/Info")]
        static void setLogLevelInfo() {
            Log.logLevel = LogLevel.Info;
        }


    }
}