using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 阵营的表模型类
    /// </summary>
    public class CampModel {
        private int id;
        private string name;
        private string creator_id;
        private string b_color;
        private string f_color;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Creator_id { get => creator_id; set => creator_id = value; }
        public string B_color { get => b_color; set => b_color = value; }
        public string F_color { get => f_color; set => f_color = value; }
    }
}
