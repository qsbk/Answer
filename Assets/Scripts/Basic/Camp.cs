using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Answerquestions {
    public class Camp {
        /// <summary>
        /// 阵营ID
        /// </summary>
        public int id;
        /// <summary>
        /// 阵营名称
        /// </summary>
        public string name;
        /// <summary>
        /// 创始人ID
        /// </summary>
        public int oid;
        /// <summary>
        /// 阵营倍率
        /// </summary>
        public float times;
        /// <summary>
        /// 背景色
        /// </summary>
        public Color bClolor;
        /// <summary>
        /// 前景色
        /// </summary>
        public Color fClolor;
    }
    public struct CampInfo {
        public Camp camp;
        public int Score;
    }
}
