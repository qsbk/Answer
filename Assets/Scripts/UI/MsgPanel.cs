using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions
{
	public class MsgPanel : MonoBehaviour
	{
		/// <summary>
		/// 消息栈
		/// </summary>
		Stack<string> _stack = new Stack<string>();
		/// <summary>
		/// 面板能承载的最大消息量
		/// </summary>
		const int	MAX_MASSAGE_LENGTH = 12;
		/// <summary>
		/// 消息队列
		/// </summary>
		Queue<string> _queue = new Queue<string>();
		[SerializeField]
		Text text;
        private void Awake() {
			EventManager.AddEvent<string, string>("MSG", OnMsg);
        }
		private void OnMsg(string sender,string msg) {
			_queue.Enqueue(DateTime.Now.ToString("hh:mm:ss ")+$"[{sender}] " + msg);
			if (_queue.Count > MAX_MASSAGE_LENGTH) {
				_queue.Dequeue();
			}
			string temp = string.Empty;
            foreach (string item in _queue) {
				temp += item + '\n';
            }
			text.text = temp;
        }
    }
	
}