using System;
using NativeWebSocket;
using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;
using UniversalFramework;

namespace Answerquestions {
    /// <summary>
    /// b站系统用于调用官方API监听消息
    /// </summary>
    public class Bilibili : MonoBehaviour {
        private WebSocketBLiveClient m_WebSocketBLiveClient;
        private const string accessKeySecret = "nmZO9sUgKzWlUicBYXhpmOo2ujyZEZ";
        private const string accessKeyId = "8491uBeuOyvR82Yg08l00raL";
        private long roomId = 24656761;
        public string code;
        public string appId;

        private async void Start() {
            //测试环境的域名现在不可用
            BApi.isTestEnv = false;
            //测试的密钥
            SignUtility.accessKeySecret = accessKeySecret;
            //测试的ID
            SignUtility.accessKeyId = accessKeyId;
            var ret = await BApi.StartInteractivePlay(code, appId);
            //打印到控制台日志
            var gameIdResObj = JsonConvert.DeserializeObject<AppStartInfo>(ret);
            if (gameIdResObj.Code != 0) {
                Debug.LogError(gameIdResObj.Message);
                return;
            }

            m_WebSocketBLiveClient = new WebSocketBLiveClient(gameIdResObj);
            m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
            m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
            m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
            m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;
            m_WebSocketBLiveClient.Connect();
            Debug.Log("连接成功");
            EventManager.TriggerEvent("SYSTEM_CONNECT");

        }

        private void WebSocketBLiveClientOnSuperChat(SuperChat superChat) {

        }

        private void WebSocketBLiveClientOnGuardBuy(Guard guard) {
        }

        private void WebSocketBLiveClientOnGift(SendGift sendGift) {
            Loom.QueueOnMainThread((param) => {
                Singleton<GiftSys>.Instance.Work(sendGift);
            }, null);
        }

        private void WebSocketBLiveClientOnDanmaku(Dm dm) {
            Loom.QueueOnMainThread((param) => {
                Singleton<DanmuSys>.Instance.Work(dm);
            }, null);
        }


        private void Update() {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (m_WebSocketBLiveClient is { ws: { State: WebSocketState.Open } }) {
                m_WebSocketBLiveClient.ws.DispatchMessageQueue();
            }
#endif
        }

        private void OnDestroy() {
            if (m_WebSocketBLiveClient == null)
                return;

            m_WebSocketBLiveClient.Dispose();
        }
    }
}