using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class Counter : MonoBehaviour
	{
        private int cont = 0;
		private int index = 0;
        private Text text;
        private void Awake() {
            text = this.GetComponentInChildren<Text>();
        }
        private void resetConter() {

        }
    }
	
}