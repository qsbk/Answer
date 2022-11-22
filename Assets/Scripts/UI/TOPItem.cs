using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class TOPItem : MonoBehaviour, TOPItemInterface {
		[SerializeField] new Text name;
		[SerializeField] Image face;
		[SerializeField] Text sText;
		[SerializeField] Text eText; //�����ı�
		Color oldColor;
		UserInfo info;
		Sprite noFace;
        private void Awake() {
			oldColor = sText.color;
			noFace = face.sprite;
        }
        // Start is called before the first frame update
        void Start()
	    {
			this.Hide();
	    }
		public void UpUser(UserInfo user, int rank) {
			this.info = user;
			name.text = user.name;
			sText.text = "+" + user.basicScore;
			if (user.tag >= 15) {
				name.color = Color.red;
			} else {
				name.color = oldColor;
			}
			BiliBIliAPI.LoadSprite(user.face, (s) => {
				face.sprite = s;
			});
        }
        public void Hide() {
			gameObject.SetActive(false);
			sText.color = oldColor;
		}

        public void Show() {
			gameObject.SetActive(true);
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
			if(info.weight <= 1) { return; }
			StartCoroutine(Multiply());
		}
		private IEnumerator Multiply() {
			eText.gameObject.SetActive(true);
			eText.text = "x" + info.weight;
			yield return new WaitForSeconds(1);
			eText.gameObject.SetActive(false);
			string endvalue =  info.Score.ToString();
			sText.color = Color.red;
			sText.DOText("+" + endvalue, 0.5f);
		}

        public void UpScore(int s) {
			sText.text = "+" + s.ToString();
		}
    }
	
}