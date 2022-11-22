using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public class Danmu
	{
		public long id;
		public string msg;
        public string name;
        public Danmu(long id, string msg) {
            this.id = id;
            this.msg = msg;
        }

        public Danmu(long id, string name, string msg) {
            this.id = id;
            this.msg = msg;
            this.name = name;
        }
    }
	
}