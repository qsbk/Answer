using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
	public interface IKeyPubSubCenter
	{
		/// <summary>
		/// ùùùù
		/// </summary>
		/// <param name="method"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		bool AddSubscribe(Key[] keys, Action method, string name);
		bool AddSubscribe(Key key, Action method, string name);
		bool AddSubscribe(KeySubscriber keySubscriber);
		List<KeySubscriber> GetSubScribeList(string keyCode);
		/// <summary>
		/// ???
		/// </summary>
		void RemoveSubscribe(string name);
		void RemoveAllSubscribe();
	}
	
}