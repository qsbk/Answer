using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// °´¼üÀà
/// </summary>

namespace UniversalFramework {
	public class Key {
		private string keyCode;
		private KeyState state;
		private Key(){}

        public string KeyCode { get => keyCode; set => keyCode = value; }
        public KeyState State { get => state; set => state = value; }

        public static Key Build(KeyCode keyCode, KeyState state) {
			string code = keyCode.ToString().ToLower();
			return Build(code, state);
        }
		public static Key Build(string keyCode, KeyState state) {
			Key key = new Key();
			key.KeyCode = keyCode;
			key.State = state;
			return key;
		}
	}
	
}