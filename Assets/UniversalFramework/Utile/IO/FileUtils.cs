using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace UniversalFramework
{
	public class FileUtils
	{
		/// <summary>
		/// 加载文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="handler"></param>
		public static void LoadFile(string path, Action<byte[]> handler) {
			var url = Path.Combine(Application.streamingAssetsPath, path);
			Singleton<Coroutines>.Instance.StartCoroutine(LoadFileAsync(url,handler));
        }
	    /// <summary>
		/// 加载一个文件返回字节码
		/// </summary>
		/// <param name="path">路径</param>
		/// <param name="handler">回调</param>
		/// <returns></returns>
	    private static IEnumerator LoadFileAsync(string path, Action<byte[]> handler) {
	        using (var req = UnityWebRequest.Get(path)) {
	            yield return req.SendWebRequest();
	            var bytes = req.downloadHandler.data;
	            handler(bytes);
				
	        }
	    }
		public static Sprite GetSprite(Byte[] bytes) {
			//先创建一个Texture2D对象，用于把流数据转成Texture2D
			Texture2D texture = new Texture2D(10, 10);
			texture.LoadImage(bytes);//流数据转换成Texture2D
									 //创建一个Sprite,以Texture2D对象为基础
			Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			return sp;
		}

	}
	
}