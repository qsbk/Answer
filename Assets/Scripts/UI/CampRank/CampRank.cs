using System;
using System.Collections.Generic;
using UnityEngine;

namespace Answerquestions {
    public class CampRank : MonoBehaviour {
        const float MAX_WIDTH = 300f;
        CampRankItem[] items;
        private void Awake() {
            EventManager.AddEvent("SETTLE_END", UpRank);
            items = GetComponentsInChildren<CampRankItem>();
            
        }
        private void Start() {
            UpRank();
        }
        void UpRank() {
            CampInfo[] infos = GameDocuments.Instance.GetCampRank().ToArray();
            int len = Math.Min(infos.Length, items.Length);
            for (int i = 0; i < len; i++) {
                items[i].Set(infos[i], 0);
            }
        }
    }
}
