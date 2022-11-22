using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniversalFramework;

namespace Answerquestions
{
	public class AnsPlan : MonoBehaviour
	{
		[SerializeField] GameObject ansPlan;
		[SerializeField] Text ansText;
		private void Awake() {
			EventManager.AddEvent<string>("SHOW_ANS", ShowAns);
		}
		private void ShowAns(string ans) {
			Singleton<Coroutines>.Instance.StartCoroutine(ShowAnswer(ans));

		}
		/// <summary>
		/// 显示本轮答案
		/// </summary>
		/// <param name="ans"></param>
		/// <returns></returns>
		IEnumerator ShowAnswer(string ans) {
			ansText.text = "本轮答案:" + ans;
			ansPlan.transform.DOScaleX(1, 0.2f);
			yield return new WaitForSeconds(5);
			ansPlan.transform.DOScaleX(0, 0.2f);
		}
	}
	
}