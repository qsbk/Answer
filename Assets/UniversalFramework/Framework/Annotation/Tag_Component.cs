using System;
using UniversalFramework;

namespace UniversalFramework {
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple =false)]
    public class Tag_Component : Attribute {
        private string id;
        public Tag_Component() {}
        public Tag_Component(string id) {
            this.Id = id;

        }
        public string Id { get => id; set => id = value; }
    }
}

