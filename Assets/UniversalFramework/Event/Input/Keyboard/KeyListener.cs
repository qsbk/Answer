using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class KeyListener : Attribute {
        private string key;
        public KeyListener(string keys) {
            this.Key = keys;
        }
        public string Key { get => key; set => key = value; }
    }
	
}