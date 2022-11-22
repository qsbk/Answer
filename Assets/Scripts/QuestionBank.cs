using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniversalFramework;
using LitJson;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Answerquestions
{
	public class QuestionBank 
	{
		private List<Question> questions;
		public int index = 0;
		public static readonly int count = 1;//每轮题库的大小
		public QuestionBank() {
			questions = GameDocuments.Instance.GetQuestions(count);
		}
		/// <summary>
		/// 随机排序
		/// </summary>
		/// <param name="data"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public Question[] Shuffle(IEnumerable<Question> data, System.Random r) {
			var arr = data.ToArray();

			for (var i = arr.Length - 1; i > 0; --i) {
				int randomIndex = r.Next(i + 1);
				Question temp = arr[i];
				arr[i] = arr[randomIndex];
				arr[randomIndex] = temp;
			}

			return arr;
		}

		public Question getQuestion(int index) {
			this.index = index;
			return questions[this.index];
		}
		/// <summary>
		/// 获取下一题
		/// </summary>
		/// <returns></returns>
		public Question getNextQuestion() {
			index++;
			if(index < questions.Count) {
				return questions[index];
            } else {
				index = 0;
				flushBank();
				return questions[0];
            }
        }
		/// <summary>
		/// 刷新题库
		/// </summary>
		/// <param name="path"></param>
		private void flushBank() {
			GameDocuments.Instance.UpQuetionsWeightToUsed(questions);
			//如果题库已经被抽完了，那么重置题库到未抽取状态
            if (!GameDocuments.Instance.isHasNotUsedQuestion()) {
				GameDocuments.Instance.ResetQuestionsToUse();
            }
			
			questions = GameDocuments.Instance.GetQuestions(count);
		}
		

	}
	
}