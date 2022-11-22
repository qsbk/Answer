using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 描述删除原因
    /// </summary>
    public enum EvictionReason {
        /// <summary>
        /// 溢出
        /// </summary>
        Capacity = 5, 
        /// <summary>
        /// 超时
        /// </summary>
        Expired = 3,

        None = 0,
        /// <summary>
        /// 手动
        /// </summary>
        Removed = 1,
        /// <summary>
        /// 覆盖
        /// </summary>
        Replaced = 2,
        /// <summary>
        /// 事件
        /// </summary>
        TokenExpired = 4

    }
}
