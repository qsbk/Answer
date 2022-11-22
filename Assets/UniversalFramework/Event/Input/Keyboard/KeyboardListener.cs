using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UniversalFramework;

namespace UniversalFramework {
    /// <summary>
    /// °´¼ü¼àÌýÀà
    /// </summary>
    public class KeyboardListener : MonoBehaviour, IKeyPubSubCenter{
        private List<string> listeningKeys;
        private Dictionary<string, KeySubscriber> subscriberDic;
        private void Awake() {
            listeningKeys = new List<string>();
            subscriberDic = new Dictionary<string, KeySubscriber>();
        }
        private void Update() {
            string temp = string.Empty;
            foreach (var item in listeningKeys) {
                if (Input.GetKeyDown(item)) {
                    temp += item + @"_|";
                }else if (Input.GetKeyUp(item)) {
                    temp += item + @"^|";
                }else if (Input.GetKey(item)) {
                    temp += item + @"!|";
                } else {
                    temp += item + @"-|";
                }
            }
            for (int i = 0; i < subscriberDic.Count; i++) {
                KeyValuePair<string, KeySubscriber> kv = subscriberDic.ElementAt(i);
                
            }
            foreach (var item in subscriberDic) {
                string[] trigger = item.Value.SubscribeTag.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                bool t = true;
                for (int i = 0; i < trigger.Length; i++) {
                    if (!temp.Contains(trigger[i])) {
                        t = false;
                        break;
                    }
                }
                if (t) { item.Value.Invoke();}
                //Regex r;
                //string pattern = item.Value.SubscribeTag;
                //r = new Regex(pattern);
                //Log.Debug(r.IsMatch(temp));
                //if (r.IsMatch(temp)) {
                //    item.Value.Invoke();
                //}
            }
        }
        public bool AddSubscribe(Key[] keys, Action method, string name) {
            return AddSubscribe(new KeySubscriber(keys, method, name));
        }
        public bool AddSubscribe(Key key, Action method, string name) {
            return AddSubscribe(new KeySubscriber(new Key[] {key}, method, name));
        }

        public bool AddSubscribe(KeySubscriber keySubscriber) {
            Debug.Log("Ìí¼Ó¼üÅÌ¼àÌý" + keySubscriber.SubscribeName);
            if (subscriberDic.ContainsKey(keySubscriber.SubscribeName)){
                return false;
            }
            subscriberDic.Add(keySubscriber.SubscribeName, keySubscriber);
            foreach (var item in keySubscriber.Key) {
                listeningKeys.Add(item.KeyCode);
            }
            return true;
        }

        public List<KeySubscriber> GetSubScribeList(string keyCode) {
            List<KeySubscriber> keySubscribers = new List<KeySubscriber>();
            foreach (var item in subscriberDic) {
                if (item.Value.SubscribeTag.Equals(keyCode)) {
                    keySubscribers.Add(item.Value);
                }
            }
            return keySubscribers;
        }

        public void RemoveSubscribe(string name) {
            KeySubscriber subscriber = subscriberDic[name];
            foreach (var item in subscriber.Key) {
                listeningKeys.Remove(item.KeyCode);
            }
            subscriberDic.Remove(name);
        }
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //private static void init() {
        //    foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
        //        string sceneName = Path.GetFileNameWithoutExtension(S.path);
        //        List<MonoBehaviour> monos = UniversalObject.FindSceneObject<MonoBehaviour>(sceneName);
        //        foreach (var item in monos) {
        //            Type type = item.GetType();
        //            foreach (var method in type.GetMethods()) {

        //                foreach (var attr in Attribute.GetCustomAttributes(method)) {
        //                    //Type fieldType = Delegate.CreateDelegate()
        //                    //Key key = ((KeyListener)attr).Key;
        //                    //Singleton<KeyboardListener>.Instance.AddSubscribe(new Key[] {
        //                    //    Key.Build(KeyCode.Space,KeyState.Down)
        //                    //}, () => {
        //                    //    Log.Debug("¿Õ¸ñ¼ü°´ÏÂ");
        //                    //}, "start");
        //                    //field.SetValue(item, "");

        //                }
        //            }
        //        }
        //    }
        //}

        public void RemoveAllSubscribe() {
            listeningKeys.Clear();
            subscriberDic.Clear();
        }
    }
}