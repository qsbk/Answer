using UnityEngine;
using UnityEngine.UI;
using SpeechLib;
using System.Collections;
namespace Answerquestions {
    public class Timer : MonoBehaviour {
        public bool isStart = false;
        private int sec = 20;
        private float secCont = 0;
        [SerializeField]
        private Text secText;
        SpVoice voice = new SpVoice();
        private void Awake() {
            secText.text = sec.ToString();
            voice.Rate = 3;
            EventManager.AddEvent<Question>("UPDATE_QUESTION", StartTimer);
        }
        // Update is called once per frame
        void Update() {
            if (isStart) {
                secCont += Time.deltaTime;
                secText.text = (sec - (int)secCont).ToString();
                if(int.Parse(secText.text) <= 5) {
                    //StartCoroutine(Spek(secText.text));

                }
                if (secCont >= 20) {
                    EventManager.TriggerEvent("TIME_OVER");
                    sec = 20;
                    secCont = 0;
                    isStart = false;
                }
            }
        }
        public void setSec(int sec) {
            this.sec = sec;
        }
        public void StartTimer(Question question) {
            isStart = true;
        }
        private IEnumerator Spek(string str) {
            voice.Speak(str, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            yield return new WaitForSeconds(1f);
            voice.Speak(string.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }
    }

}
  
