using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class Option : MonoBehaviour
	{
        [SerializeField]
        private Sprite rightSprite;
        [SerializeField]
        private Sprite generalSprite;
		private Text oText;
        private Image bg;
        Color generalColor = new Color(0.27f, 0.282f, 0.552f);
        Color rightColor = Color.white;
        private void Awake() {
			oText = this.GetComponentInChildren<Text>();
            bg = this.GetComponent<Image>();
        }

        public void setText(string str) {
			oText.text = str;
        }
		public void isTrue() {
            bg.sprite = rightSprite;
            oText.color = rightColor;
        }
        public void resetOp() {
            bg.sprite = generalSprite;
            oText.color = generalColor;
        }
    }
	
}