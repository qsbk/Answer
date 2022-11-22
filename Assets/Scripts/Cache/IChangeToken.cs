using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 发生改变时通知
    /// </summary>
    public interface IChangeToken {
        /// <summary>
        /// 指示此令牌是否将主动引发回调 如果为 false，则令牌使用者必须轮询 HasChanged 以检测更改
        /// </summary>
        public bool ActiveChangeCallbacks { get; }
        /// <summary>
        /// 是否发生更改
        /// </summary>
        public bool HasChanged { get; }
        /// <summary>
        /// 更改时回调
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable RegisterChangeCallback(Action<object> callback, object state);
    }
}
