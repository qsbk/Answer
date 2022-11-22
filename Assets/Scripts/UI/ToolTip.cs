using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions {
    public class ToolTip : MonoBehaviour{
        public const float SHOW_LENGTH = 3;
        public const float SHORT = 2;
        public const float NORMAL = 3;
        public const float LONG = 5;
        [SerializeField]
        Text text;
        private float showedTime = 0;
        /// <summary>
        /// 消息队列
        /// </summary>
        Queue<string> msgQueue = new Queue<string>();
        Queue<float> timeQueue = new Queue<float>();
        /// <summary>
        /// 标识是否正在显示
        /// </summary>
        private bool isShow = false;
        private void Start() {
            gameObject.SetActive(false);
            EventManager.AddEvent<string, string, float>("TOOL_TIP", Show);
        }
        private void Update() {
            if (!isShow) {
                if (msgQueue.Count > 0) {
                    text.text = msgQueue.Dequeue();
                    showedTime = timeQueue.Dequeue();
                    gameObject.SetActive(true);
                    isShow = true;
                }
            } else {
                showedTime -= Time.deltaTime;
                if (showedTime <= 0) {
                    showedTime = 0;
                    isShow = false;
                    Hide();
                }
                
            }
        }
        public void Show(string sender, string msg, float time) {
            string s = $"{sender}: " + msg;
            msgQueue.Enqueue(s);
            timeQueue.Enqueue(time);
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
