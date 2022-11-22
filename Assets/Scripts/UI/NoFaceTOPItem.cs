using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Answerquestions
{
	public class NoFaceTOPItem : MonoBehaviour, TOPItemInterface {
		[SerializeField] Text oText;
		[SerializeField] Text rText;
		[SerializeField] Text sText;
		[SerializeField] Text eText; //�����ı�
		[SerializeField] Image face;
		Color oldColor;
		UserInfo info;
		Sprite noFace;
        private void Awake() {
			oldColor = sText.color;
			noFace = face.sprite;
		}
        void Start() {
			this.Hide();
		}

		public void Hide() {
			this.transform.localScale = new Vector3(0,  1,  1);
			sText.color = oldColor;
		}

        public void UpUser(UserInfo user, int rank) {
			rText.text = rank.ToString();
			oText.text = user.name;
			sText.text = "+" + user.basicScore;
			info = user;
			if (user.tag > 0) {
				oText.color = Color.red;
			} else {
				oText.color = oldColor;
			}

			BiliBIliAPI.LoadSprite(user.face, (s) => {
				face.sprite = s;
			});
		}

        public void Show() {
			this.transform.DOScaleX(1, 0.2f);
		}

        public void AddScore() {
			if (info.extraScore <= 0) { return; }
			StartCoroutine(Add());
        }
		private IEnumerator Add() {
			eText.gameObject.SetActive(true);
			eText.text = "+" + info.extraScore;
			yield return new WaitForSeconds(1);
			eText.gameObject.SetActive(false);
			string endvalue = (info.basicScore + info.extraScore).ToString();
			sText.DOText("+" + endvalue, 0.5f);
		}
        public void MultiplyScore() {
			if (info.weight <= 1) { return; }
			StartCoroutine(Multiply());
		}
		private IEnumerator Multiply() {
			eText.gameObject.SetActive(true);
			eText.text = "x" + info.weight;
			yield return new WaitForSeconds(1);
			eText.gameObject.SetActive(false);
			string endvalue = info.Score.ToString();
			sText.color = Color.red;
			sText.DOText("+" + endvalue, 0.5f);
		}

        public void UpScore(int s) {
			sText.text = "+" + s.ToString();
        }
    }
	
}