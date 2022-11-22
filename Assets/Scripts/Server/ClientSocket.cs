
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
namespace Answerquestions {
	public class ClientSocket {
		Socket mSocket;
		byte[] mData;
		int mOffect;
		/// <summary>
		///	一次性接收数据的最大字节
		/// </summary>
		int mSize = 1024 * 1024 * 1;
		Action<ClientSocket> CloseCallBack;

		public ClientSocket(Socket varSocket, Action<ClientSocket> action) {
			CloseCallBack = action;
			mSocket = varSocket;
			//获取当前连接的客户端的IP和端口
			IPEndPoint IPEnd = mSocket.RemoteEndPoint as IPEndPoint;
			//IP地址
			string IP = IPEnd.Address.ToString();
			//Port端口
			int Port = IPEnd.Port;

			mData = new byte[mSize];
			//开启数据接口监听
			mSocket.BeginReceive(mData, mOffect, mSize - mOffect, SocketFlags.None, Receive, null);
		}
		private void Receive(IAsyncResult result) {
			
			//结束当前监听进行数据处理
			int size = mSocket.EndReceive(result, out SocketError error);
			if (error == 0) {
				//接收数据成功
				if (size != 0) {
					
					//当数据大小不为0时进行数据内容处理
					byte[] bytes = new byte[size + mOffect];
					Array.Copy(mData, 0, bytes, 0, size + mOffect);
					string message = Regex.Unescape(Encoding.UTF8.GetString(bytes));
					//MessageCenter.Work(message);
				}
				this.Send(Encoding.UTF8.GetBytes("OK"));
				//开启数据接收的异步监听
				mSocket.BeginReceive(mData, mOffect, mSize - mOffect, SocketFlags.None, Receive, null);
			} else {
				Debug.LogError("断开连接");
				this.Send(Encoding.UTF8.GetBytes("ERR"));
				//接收数据发生错误-自动断开连接
				CloseConnect();
			}

		}
		/// <summary>
		///	发送数据给服务器
		/// </summary>
		public bool Send(byte[] bytes) {
			try {
				mSocket.Send(bytes);
				return true;
			} catch (Exception exp) {
				//发生数据异常-自动断开连接
				CloseConnect();
				return false;
			}
		}
		/// <summary>
		///	关闭服务器连接
		/// </summary>
		public void CloseConnect() {
			if (mSocket.Connected) {
				CloseCallBack(this);
				mSocket.Close();
			}
		}
	}
}


