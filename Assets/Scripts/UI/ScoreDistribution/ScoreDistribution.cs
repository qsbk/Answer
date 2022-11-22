using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Answerquestions {
    public class ScoreDistribution : MonoBehaviour {
        [SerializeField]
        public ScoreDistributionEntry Bilibili, Douyu;
        public int BScore = 0, DScore = 0;
        public const int PLAN_WIDTH = 500;
        private void Awake() {
            if (BScore == DScore) {
                Bilibili.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PLAN_WIDTH / 2);
                Douyu.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PLAN_WIDTH / 2);
            }
        }
        private void Update() {
            if (BScore != 0 || DScore != 0) {
                float Bwidth = PLAN_WIDTH * (BScore / (float)(BScore + DScore));
                float Dwidth = PLAN_WIDTH * (DScore / (float)(BScore + DScore));
                Debug.Log(Bwidth);
                Bilibili.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Bwidth);
                Douyu.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Dwidth);
            }
            
        }
    }

}
