using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UniversalFramework;
namespace Answerquestions {
    public class MainManger : MonoBehaviour {
        [SerializeField]
        GameObject option;
        [SerializeField]
        AnswerSys sys;
        [SerializeField]
        GameObject waitPlanel;
        /// <summary>
        /// 计分器
        /// </summary>
        public ScoreIndicator indicator;
        private static MainManger instance;
        public static MainManger Instance { get { return instance; }  }
        private void Awake() {
            instance = this;
            EventManager.AddEvent("SYSTEM_CONNECT", init);
            indicator = new ScoreIndicator();
            //Singleton<NetSocket>.Instance.Connect();
        }
        private void init() {
            waitPlanel.SetActive(false);
            sys.run();
        }
        private void OnDestroy() {
            GameDocuments.Instance.Close();
        }
    }
}

