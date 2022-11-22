using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    public class PostEvictionCallbackRegistration {
        public PostEvictionDelegate EvictionCallback { get; set; }
        public object State { get; set; }
    }
}
