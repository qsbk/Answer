using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public class Gift
	{
		public long id;
		public string giftName;

        public Gift(long id, string giftName) {
            this.id = id;
            this.giftName = giftName;
        }
    }
	
}