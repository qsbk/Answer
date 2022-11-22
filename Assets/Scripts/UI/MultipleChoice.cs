using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class MultipleChoice : MonoBehaviour
	{
		[SerializeField]
		Text oText;
		[SerializeField]
		Option optionA;
		[SerializeField]
		Option optionB;
		[SerializeField]
		Option optionC;
		[SerializeField]
		Option optionD;
        private void Awake() {
			EventManager.AddEvent<Question>("UPDATE_QUESTION", OnUpdate);
        }

        private void OnUpdate(Question question) {
            if (!question.type.Equals("MCQ")) {
				gameObject.SetActive(false);
				return;
            } else {
				gameObject.SetActive(true);
				oText.text = question.description;
				optionA.setText("A." + question.option[0]);
				optionB.setText("B." + question.option[1]);
				optionC.setText("C." + question.option[2]);
				optionD.setText("D." + question.option[3]);
            }
        }
	}
	
}