using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UniversalFramework;
namespace Answerquestions
{
	public class BiliBIliAPI {
		/// <summary>
		/// 从URL中加载Sprite
		/// </summary>
		/// <param name="url"></param>
		/// <param name="action"></param>
		public static void LoadSprite(string url, Action<Sprite> action) {
			Coroutine c = Singleton<Coroutines>.Instance.StartCoroutine(Load(url, action));
		
        }
		static IEnumerator Load(string url, Action<Sprite> action) {
			//byte[] bytes = GameDocuments.Instance.GetUserFaceByte(url);
			//if (bytes != null) {
			//	Debug.Log("SQL请求");
			//	action(Utils.GetSprite(bytes));
			//	yield break;
			//}
			using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)) {
				yield return uwr.SendWebRequest();
				if (uwr.result == UnityWebRequest.Result.ConnectionError) {
					Debug.LogError(uwr);
					yield return new WaitForSeconds(1);
				} else {
					Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
					if (texture != null) {
						Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
						//Byte[] vs = Utils.GetByte(sprite);
						//GameDocuments.Instance.InsertUserFace(url, vs);
						action(sprite);
					}
					
				}
			}
        }

	}
	
}