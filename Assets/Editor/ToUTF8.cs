using System;
using System.IO;
using System.Text;
using UnityEditor;

public static class ToUTF8 {
    /// 

    /// ��.csת��UTF-8��ʽ
    /// 

    [MenuItem("Unisvil/Convert2UTF8")]
    public static void Convert2UTF8() {
        var dir = "Assets/Scripts/";
        foreach (var f in new DirectoryInfo(dir).GetFiles("*.cs", SearchOption.AllDirectories)) {
            var s = File.ReadAllText(f.FullName, Encoding.Default);
            try {
                File.WriteAllText(f.FullName, s, Encoding.UTF8);
            } catch (Exception) {
                continue;
            }
        }
    }
}