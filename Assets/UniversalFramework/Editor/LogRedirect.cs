using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace UniversalFramework {
    /// <summary>
    /// 日志重定向
    /// </summary>
    public class LogRedirect {
        private static readonly Regex LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");
        private const string DEBUGERFILEPATH = "Assets/UniversalFramework/Utility/Log.cs";
        UnityEngine.Object debuggerFile;
        private int m_DebugerFileInstanceId;
        private static LogRedirect m_Instance;
        public static LogRedirect GetInstacne() {
            if (m_Instance == null) {
                m_Instance = new LogRedirect();
            }
            return m_Instance;
        }

        private LogRedirect() {
            UnityEngine.Object debuggerFile = AssetDatabase.LoadAssetAtPath(DEBUGERFILEPATH, typeof(UnityEngine.Object));

            m_DebugerFileInstanceId = debuggerFile.GetInstanceID();
        }
        [OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceId, int line) {
            if (instanceId != LogRedirect.GetInstacne().m_DebugerFileInstanceId) {
                return false;
            }
            
            string selectedStackTrace = LogRedirect.GetInstacne().GetSelectedStackTrace();
            if (string.IsNullOrEmpty(selectedStackTrace)) {
                return false;
            }
            if (!selectedStackTrace.Contains("Log.cs")) {
                return false;
            }

            Match match = LogRegex.Match(selectedStackTrace);
            if (!match.Success) {
                return false;
            }
            

            // 跳过第一次匹配的堆栈
            match = match.NextMatch();
            if (!match.Success) {
                return false;
            }
            if (!selectedStackTrace.Contains("Log.cs")) {
                UnityEngine.Object codeObject = AssetDatabase.LoadAssetAtPath(match.Groups[1].Value, typeof(UnityEngine.Object));
                if (codeObject == null) {
                    return false;
                }
                EditorGUIUtility.PingObject(codeObject);
                AssetDatabase.OpenAsset(codeObject, int.Parse(match.Groups[2].Value));
                //InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath.Replace("Assets", "") + match.Groups[1].Value, int.Parse(match.Groups[2].Value)); 

                return true;
            } else {
                // 跳过第2次匹配的堆栈
                match = match.NextMatch();
                if (!match.Success) {
                    return false;
                }
                UnityEngine.Object codeObject = AssetDatabase.LoadAssetAtPath(match.Groups[1].Value, typeof(UnityEngine.Object));
                if (codeObject == null) {
                    return false;
                }
                EditorGUIUtility.PingObject(codeObject);
                AssetDatabase.OpenAsset(codeObject, int.Parse(match.Groups[2].Value));
                //InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath.Replace("Assets", "") + match.Groups[1].Value, int.Parse(match.Groups[2].Value));
                return true;
            }
            

        }

        private string GetSelectedStackTrace() {
            Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;
            if (editorWindowAssembly == null) {
                return null;
            }

            System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null) {
                return null;
            }

            FieldInfo consoleWindowFieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            if (consoleWindowFieldInfo == null) {
                return null;
            }

            EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
            if (consoleWindow == null) {
                return null;
            }

            if (consoleWindow != EditorWindow.focusedWindow) {
                return null;
            }

            FieldInfo activeTextFieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (activeTextFieldInfo == null) {
                return null;
            }

            return (string)activeTextFieldInfo.GetValue(consoleWindow);
        }
    }
}