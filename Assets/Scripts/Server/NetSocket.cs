using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UniversalFramework;

namespace Answerquestions {
    public class NetSocket : MonoBehaviour {
        private const int port = 9930;
        private static string IpStr = "127.0.0.1";
        private static Socket serverSocket;
        ClientSocket mClient;
        private bool isConnect;
        public void Connect() {
            IPAddress ip = IPAddress.Parse(IpStr);
            IPEndPoint ip_end_point = new IPEndPoint(ip, port);
            ///创建服务器Socket对象，并设置相关属性 
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定ip和端口  
            serverSocket.Bind(ip_end_point);
            //设置最长的连接请求队列长度  
            serverSocket.Listen(10);
            Debug.Log(string.Format("启动监听{0}成功", serverSocket.LocalEndPoint.ToString()));
            serverSocket.BeginAccept(Accept, null);
        }
        private void Accept(IAsyncResult result) {
            //当没有客户端连接时，接受连接请求
            if(mClient is null) {
                //收到连接后结束连接监听并获取当前连接的客户端Socket
                Socket TempClient = serverSocket.EndAccept(result);
                Debug.Log(string.Format("客户端{0}已连接", TempClient.RemoteEndPoint.ToString()));
                ClientSocket client = new ClientSocket(TempClient, CloseClient);

                //将连接的客户端记录下来
                mClient = client;
                Loom.QueueOnMainThread((param) => {
                    EventManager.TriggerEvent("PYTHON_CONNECT");
                }, null);
            }
            //重新开启客户端的连接监听
            serverSocket.BeginAccept(Accept, null);

        }
        /// <summary>
        ///	关闭客户端连接
        /// </summary>
        private void CloseClient(ClientSocket client) {
            if (!(mClient is null)) {
                mClient = null;
            }
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Close() {
            mClient = null;
            serverSocket.Close();
        }
        private void OnDestroy() {
            this.Close();
        }
    }
}

