using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class GapFilling : MonoBehaviour
	{
		[SerializeField]
		Text oText;
		string description;

		private void Awake() {
			EventManager.AddEvent<Question>("UPDATE_QUESTION", OnUpdate);
		}
		private void OnUpdate(Question question) {
			if (!question.type.Equals("填空题")) {
				gameObject.SetActive(false);
				return;
			} else {
				gameObject.SetActive(true);
				description = question.description;
				oText.text = formatDescription(description);
			}
		}
		public void showResult() {
			oText.text = description.Replace("{", "").Replace("}", "");
		}
		/// <summary>
		/// 格式化题目样式
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string formatDescription(string s) {
			int start = s.IndexOf("{");
			if (start == -1) return s;
			int end = s.IndexOf("}", start);
			if (end == -1) return s;
			int len = end - start;
			char[] cs = s.ToCharArray();

			for (int i = start; i <= end; i++) {
				//cs[i] = '\u00a0';
				cs[i] = '_';
			}
			return new string(cs);
		}
	}
	
}