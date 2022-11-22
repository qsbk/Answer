using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public class SettlePanel : MonoBehaviour
	{
        private void Awake() {
			EventManager.AddEvent("TIME_OVER", () => {
				gameObject.SetActive(true);
				StartCoroutine(Settle());
			});
        }
        private void Start() {
			gameObject.SetActive(false);
        }
		/// <summary>
		/// 显示结算面板，5S后开始结算
		/// </summary>
		/// <returns></returns>
        private IEnumerator Settle() {
			yield return new WaitForSeconds(5);
			EventManager.TriggerEvent("START_SETTLE");
			gameObject.SetActive(false);
		}
	}
	
}