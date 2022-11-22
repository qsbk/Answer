using System;

namespace UniversalFramework
{
	public interface IKeySubscribe
	{
		/// <summary>
		/// 订阅事件ID
		/// </summary>
		string SubscribeName { get; set; }
		/// <summary>
		/// 订阅事件
		/// </summary>
		string SubscribeTag { get;}
		/// <summary>
		/// 订阅的名称
		/// </summary>
		Key[] Key { get; set; }
		/// <summary>
		/// 相应发布的方法
		/// </summary>
		Action SubscribeMethod { get; set; }
		/// <summary>
		/// 调用订阅方法
		/// </summary>
		/// <returns></returns>
		object Invoke();
	}
	
}