using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalFramework;

namespace Answerquestions
{
	/// <summary>
	/// 排行榜UI类
	/// </summary>
	public class Rank : MonoBehaviour
	{
		private RankItem[] items;
		private List<long> users = new List<long>();
        private void Awake() {
			items = GetComponentsInChildren<RankItem>();
			EventManager.AddEvent<List<long>>("UP_RANK", UpRanke);

		}
		private void UpRanke(List<long> users) {
			int len = users.Count > items.Length ? items.Length : users.Count;
			this.users = users;
			for (int i = 0; i < len; i++) {
				items[i].SetUser(UserManager.GetUser(users[i]), i + 1);
            }
        }

		private void Clear() {
            foreach (var item in items) {
				item.gameObject.SetActive(false);
            }
        }

     }
}