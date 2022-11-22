using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 系统接口，所有系统将具有以下功能
    /// </summary>
    public interface ISys<T> {
        /// <summary>
        /// 启动系统
        /// </summary>
        public void Run();
        /// <summary>
        /// 暂停系统
        /// </summary>
        public void Stop();
        /// <summary>
        /// 关闭系统
        /// </summary>
        public void Close();
        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="data"></param>
        public void Work(T data);
    }
}
