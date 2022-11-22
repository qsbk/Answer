using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Answerquestions
{
	[Serializable]
	public class Questions
	{
		public List<Question> data;

	}
	[Serializable]
	public class Question {
		public long id;
		public string type;
		public string description;
		public List<string> answer;
		public List<string> option;
	}

}