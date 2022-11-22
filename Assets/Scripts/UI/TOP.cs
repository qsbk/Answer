using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniversalFramework;
using DG.Tweening;

namespace Answerquestions
{
	/// <summary>
	/// 结算排行榜UI类
	/// </summary>
	public class TOP : MonoBehaviour
	{
		TOPItemInterface[] Items;
		[SerializeField] Text title;
		[SerializeField] GameObject ansPlan;
		[SerializeField] Text ansText;
		[SerializeField] GameObject tiPanle;
		List<UserInfo> infos = new List<UserInfo>();
        private void Awake() {
			Items = GetComponentsInChildren<TOPItemInterface>();
			EventManager.AddEvent<string>("SETTLE", OnSettle);
			EventManager.AddEvent<UserInfo>("ADD_USER_TO_TOP", AddInfo);
        }
        private void Start() {
			gameObject.SetActive(false);
        }
		private void AddInfo(UserInfo info) {
			/*if( info.tag > 0)*/ infos.Add(info);
			
        }
        private void OnSettle(string ans) {
			title.gameObject.SetActive(false);
			tiPanle.SetActive(false);
			Singleton<Coroutines>.Instance.StartCoroutine(SetUser(ans));
        }
		IEnumerator SetUser(string ans) {
			gameObject.SetActive(true);
			yield return ShowAnswer(ans);
			title.gameObject.SetActive(true);
			tiPanle.SetActive(true);
			int len = infos.Count > Items.Length ? Items.Length : infos.Count;
            for (int i = 0; i < len; i++) {
				Items[i].UpUser(infos[i], i+1);
				yield return new WaitForSeconds(0.1f);
				Items[i].Show();
            }
			yield return new WaitForSeconds(3);
			///����������
            for (int i = 0; i < len; i++) {
				Items[i].AddScore();
			}
			yield return new WaitForSeconds(2);
			Sort(infos);
			yield return new WaitForSeconds(3);
			///���ⱶ��
			for (int i = 0; i < len; i++) {
				Items[i].MultiplyScore();
			}
			infos.Clear();
			yield return new WaitForSeconds(3);
			foreach (var item in Items) {
				item.Hide();
            }
			EventManager.TriggerEvent("SETTLE_END");
			gameObject.SetActive(false);
        }
		/// <summary>
		/// 显示本轮答案
		/// </summary>
		/// <param name="ans"></param>
		/// <returns></returns>
		IEnumerator ShowAnswer(string ans) {
			ansText.text = "本轮答案:" + ans;
			ansPlan.transform.DOScaleX(1, 0.2f);
			yield return new WaitForSeconds(3);
			ansPlan.transform.DOScaleX(0, 0.2f);
		}
		/// <summary>
		/// 开始排序
		/// </summary>
		/// <param name="list"></param>
		private void Sort(List<UserInfo> list) {
			if (list == null || list.Count < 2) {
				return;
			}
			StartCoroutine(InsertionSort(list));
		}
		/// <summary>
		/// 插入排序
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		private IEnumerator InsertionSort(List<UserInfo> list) {
			UserInfo key;
			for (int i = 1; i < list.Count; i++) {
				key = list[i];
				int j = i;
				while (j > 0 && key.Score > list[j-1].Score) {
					if (j + 1 < Items.Length) {
						Items[j].UpUser(list[j-1], j + 1);
						Items[j].UpScore(list[j - 1].basicScore + list[j].extraScore);
					}
					list[j] = list[j - 1];
					yield return new WaitForSeconds(0.2f);
					j--;
				}
				if (j + 1 < Items.Length) {
					Items[j].UpUser(key, j + 1);
					Items[j].UpScore(key.basicScore + key.extraScore);
					yield return new WaitForSeconds(0.2f);
				}
				list[j] = key;
			}
		}



	}
	public struct UserInfo {
		public long id;
		public string name;
		public string face;
		public int basicScore;
		public int Score;
		public int extraScore;
		public float weight;
		public long tag;

	}
	
}