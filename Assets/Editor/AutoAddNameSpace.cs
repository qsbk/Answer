using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AutoAddNmaeSpace : UnityEditor.AssetModificationProcessor {
    static string namespaceName = "Answerquestions";

    /// 将要创建资源时会调用这个函数
    /// 

    static void OnWillCreateAsset(string path) {
        //导入资源的路径
        string str = path.Replace(".meta", "");
        string[] splitArgs = str.Split('.');
        if (splitArgs[splitArgs.Length - 1].Equals("cs")) {
            //Debug.Log("导入的是脚本");
            string[] newSplitArgs = str.Split('/');
            bool isEditor = false;
            foreach (var item in newSplitArgs) {
                if (item.Equals("Editor")) {
                    isEditor = true;
                    break; ;
                }
            }
            if (isEditor) return;
            ParseAndChangeScript(str.Substring(6, str.Length - 6));
        }
    }

    /*
    [MenuItem("Assets/Create/NameSpace_C#",false,1)]
    public static void CreateScripts()
    {
        string sourcePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //判断是文件夹还是文件
        if (File.Exists(sourcePath))
        {
            int length = sourcePath.LastIndexOf('/');
            sourcePath = sourcePath.Substring(0, length);
        }
        //AssetDatabase
        // File.Create(sourcePath);
        //ParseAndChangeScript();
        //Debug.Log();
    }
    */

    private static void ParseAndChangeScript(string path) {
        string str = File.ReadAllText(Application.dataPath + path);
        if (string.IsNullOrEmpty(str)) {
            Debug.Log("读取出错了，Application.dataPath=" + Application.dataPath + "  path=" + path);
            return;
        }

        string newStr = "";
        //增加命名空间
        if (!str.Contains("namespace")) {
            if (!string.IsNullOrEmpty(namespaceName)) {
                int length = str.IndexOf("public");
                newStr += str.Substring(0, length);
                string extraStr = "";
                string[] extraStrs = str.Substring(length, str.Length - length).Replace("\r\n", "\n").Split('\n');
                foreach (var item in extraStrs) {
                    extraStr += "\t" + item + "\r\n";
                }


                newStr += "\r\nnamespace " + namespaceName + "\r\n{\r\n" + extraStr + "}";
                //newStr = newStr.Replace("\n", "\r\n");
                //newStr = newStr.Replace('\r', ' ');
            } else {
                newStr = str;
            }
            File.WriteAllText(Application.dataPath + path, newStr);
        }
    }

    /// <summary>
    /// 在保存资源时对资源进行初始化
    /// </summary>
    /// <param name="path"></param>
    static string[] OnWillSaveAssets(string[] paths) {
        
        //foreach (string path in paths)
        //    Debug.Log(path);
        return paths;
    }

}
