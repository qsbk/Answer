using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
	public class Coroutines : MonoBehaviour
	{
		public Coroutine StartCorouting(string methodName) {
			return StartCoroutine(methodName);
        }
		public Coroutine StartCorouting(IEnumerator routine) {
			return StartCoroutine(routine);
        }
	}
	
}