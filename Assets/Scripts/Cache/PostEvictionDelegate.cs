using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    public delegate void PostEvictionDelegate(object key, object value, EvictionReason reason, object state);
}
