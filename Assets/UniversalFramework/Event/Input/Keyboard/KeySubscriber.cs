using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
    /// <summary>
    /// ¼üÅÌ¶©ÔÄÕß
    /// </summary>
    public class KeySubscriber : IKeySubscribe {
        private string subscribeTag;
        private Key[] keys;
        private Action method;
        private string name;

        public KeySubscriber(Key[] keys, Action method, string name) {
            if(keys is null || keys.Length <= 0) {throw new Exception();}
            foreach (var item in keys) {
                subscribeTag += item.KeyCode + (item.State == KeyState.Down ? @"_|" : @"^|");
            }
            this.keys = keys;
            this.method = method;
            this.name = name;
        }

        public string SubscribeTag { get => subscribeTag;}
        public Key[] Key { get => keys; set => keys = value; }
        public Action SubscribeMethod { get => method; set => method = value; }
        public string SubscribeName { get => name; set => name = value; }

        public object Invoke() {
            try {
                method.Invoke();
            } catch (Exception) {

                return null;
            }
            
            return this;
        }
    }

}