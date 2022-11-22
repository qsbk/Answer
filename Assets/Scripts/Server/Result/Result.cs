using System.Collections;
using System.Collections.Generic;
namespace Answerquestions {
    
    public class Result<T> {
        public string type;
        public string sys;
        public string time;
        public long uid;
        public string face;
        public int tag;
        public string name;
        public T data;

    }
    public enum SystemType {
        Bilibili,
        Douyu
    }
}


