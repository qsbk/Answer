using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateResourceConfig : Editor {
    //需要映射的文件类型
    private static string[] fileTypes = { "prefab", "material", "json", "png" };

    //储存上面类型对应的扩展名
    private static string[] extensionStr = { "prefab", "mat", "json", "png" };

    //显示在Unity菜单工具栏中，方便点击调用
    [MenuItem("Tools/Resource/GenerateResourceConfig")]
    public static void GenerateConfig() {
        Debug.Log("GenerateConfig");
        for (int i = 0; i < fileTypes.Length; ++i) {
            //将映射信息写入到文件中
            if (i == 0)
                File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", GenerateStrInfo(i));
            else
                File.AppendAllLines("Assets/StreamingAssets/ConfigMap.txt", GenerateStrInfo(i));
        }
        //刷新Assets
        AssetDatabase.Refresh();
    }

    public static string[] GenerateStrInfo(int type) {
        //获取GUID
        string[] resPaths = AssetDatabase.FindAssets("t:" + fileTypes[type], new string[] { "Assets/StreamingAssets" });
        for (int i = 0; i < resPaths.Length; ++i) {
            //通过GUID获取路径
            resPaths[i] = AssetDatabase.GUIDToAssetPath(resPaths[i]);
            //获取文件名
            string name = Path.GetFileNameWithoutExtension(resPaths[i]);
            //获取Resources Load时需要的路径(去掉"Assets/StreamingAssets/"和文件扩展名)
            string filePath = resPaths[i].Replace("Assets/StreamingAssets/", string.Empty).Replace("." + extensionStr[type], string.Empty);
            //通过等号分割key value，此处不宜使用空格分割，因为路径或文件名中可能包含空格
            resPaths[i] = name + "=" + filePath;
        }
        return resPaths;
    }
}

