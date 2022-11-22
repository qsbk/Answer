using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateResourceConfig : Editor {
    //��Ҫӳ����ļ�����
    private static string[] fileTypes = { "prefab", "material", "json", "png" };

    //�����������Ͷ�Ӧ����չ��
    private static string[] extensionStr = { "prefab", "mat", "json", "png" };

    //��ʾ��Unity�˵��������У�����������
    [MenuItem("Tools/Resource/GenerateResourceConfig")]
    public static void GenerateConfig() {
        Debug.Log("GenerateConfig");
        for (int i = 0; i < fileTypes.Length; ++i) {
            //��ӳ����Ϣд�뵽�ļ���
            if (i == 0)
                File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt", GenerateStrInfo(i));
            else
                File.AppendAllLines("Assets/StreamingAssets/ConfigMap.txt", GenerateStrInfo(i));
        }
        //ˢ��Assets
        AssetDatabase.Refresh();
    }

    public static string[] GenerateStrInfo(int type) {
        //��ȡGUID
        string[] resPaths = AssetDatabase.FindAssets("t:" + fileTypes[type], new string[] { "Assets/StreamingAssets" });
        for (int i = 0; i < resPaths.Length; ++i) {
            //ͨ��GUID��ȡ·��
            resPaths[i] = AssetDatabase.GUIDToAssetPath(resPaths[i]);
            //��ȡ�ļ���
            string name = Path.GetFileNameWithoutExtension(resPaths[i]);
            //��ȡResources Loadʱ��Ҫ��·��(ȥ��"Assets/StreamingAssets/"���ļ���չ��)
            string filePath = resPaths[i].Replace("Assets/StreamingAssets/", string.Empty).Replace("." + extensionStr[type], string.Empty);
            //ͨ���Ⱥŷָ�key value���˴�����ʹ�ÿո�ָ��Ϊ·�����ļ����п��ܰ����ո�
            resPaths[i] = name + "=" + filePath;
        }
        return resPaths;
    }
}

