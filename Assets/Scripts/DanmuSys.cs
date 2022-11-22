using System;
using System.Collections;
using System.Collections.Generic;
using OpenBLive.Runtime.Data;
using UnityEngine;
namespace Answerquestions {
	public class DanmuSys : MonoBehaviour, ISys<Dm> {
		public bool isRun = true;
		public void Close() {
			Destroy(gameObject);
		}

		public void Run() {
			isRun = true;
		}

		public void Stop() {
			isRun = false;
		}

		public void Work(Dm data) {
			if (!isRun) { return; }

			//当接收数据成功，将用户数据序列化，并尝试添加进UserManager
			User user = new User(data.uid, data.userName, data.userFace, data.fansMedalName.Equals("百科书") ? data.fansMedalLevel : 0);
			/*if(user.Tag > 0) {
				UserManager.UpUser(user);
            }*/
			UserManager.UpUser(user);
			this.Classify(data);

		}
		/// <summary>
		/// 将弹幕消息分类触发不同的事件
		/// </summary>
		/// <param name="dm"></param>
		private void Classify(Dm dm) {
			Debug.Log(dm.msg);
			string order;
			if (dm.msg.Length >= 2) {
				order = dm.msg.Substring(0, 2);
			} else {
				order = dm.msg;
			}
			string msg = String.Empty;
			try {
				msg = dm.msg[2..];
			} catch (Exception) {

			}
			
			Loom.QueueOnMainThread((param) => {
				switch (order) {
					case "加入":

						GameDocuments.Instance.JoinCamp(dm.uid, msg.Replace(" ", ""));
						break;
					case "查分":
						User user = UserManager.GetUser(dm.uid);
						if (user != null && user.Tag == 0) {
							EventManager.TriggerEvent("MSG", "系统", $"{dm.userName}携带粉丝牌可查分哦~");
							return;
						}
						int score = GameDocuments.Instance.GetUserScore(dm.uid);
						int rank = GameDocuments.Instance.GetUserRank(dm.uid);
						EventManager.TriggerEvent("MSG", "系统", $"{dm.userName}的分数为{score}，目前排名第{rank}");
						break;
					default:
						SendMsg(dm);
						break;
				}
			}, null);

		}
		private static void SendMsg<T>(T msg) {
			SendEvent(DanmuType.Message, msg);
		}
		private static void SendEvent<T>(DanmuType type, T data) {
			EventManager.TriggerEvent(type.ToString(), data);
		}
		public enum DanmuType {
			Message,
			Gift,
			Phiz
		}

	}
}