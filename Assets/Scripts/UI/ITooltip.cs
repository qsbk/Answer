using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 提示框接口类
    /// </summary>
    public interface ITooltip {
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        public void Show(string msg);
    }
}
