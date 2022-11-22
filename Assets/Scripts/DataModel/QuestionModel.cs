using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    public class QuestionModel {
        /// <summary>
        /// 主键
        /// </summary>
        private long id;
        /// <summary>
        /// 问题描述
        /// </summary>
        private string qs_describe;
        /// <summary>
        /// 问题类型：选择或者填空
        /// </summary>
        private int qs_type;
        /// <summary>
        /// 问题的答案列表
        /// </summary>
        private List<string> qs_solution;
        /// <summary>
        /// 问题的选项列表，只有选择题会有
        /// </summary>
        private List<string> qs_options;
        /// <summary>
        /// 问题的分类
        /// </summary>
        private int qs_category;
        private int display_number;
        private int answer_number;
        private int correct_number;
        private bool isBan;
    }
}
