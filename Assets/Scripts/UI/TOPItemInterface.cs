using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public interface TOPItemInterface
	{
		public void UpUser(UserInfo user, int rank);
		public void UpScore(int s);
		public void Hide();
		public void Show();
		public void AddScore();
		public void MultiplyScore();
	}
	
}