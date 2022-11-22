using System;
using UnityEngine;
using UnityEngine.UI;

namespace Answerquestions {
    public class CampRankItem : MonoBehaviour {
        [SerializeField]
        Text cName, score;
        [SerializeField]
        Image bg;
        private void Start() {
            gameObject.SetActive(false);
        }
        public void Set(CampInfo info, float width) {
            cName.text = info.camp.name;
            score.text = info.Score.ToString();
            bg.color = info.camp.bClolor;
            cName.color = info.camp.fClolor;
            score.color = info.camp.fClolor;
            gameObject.SetActive(true);
        }



    }
}
