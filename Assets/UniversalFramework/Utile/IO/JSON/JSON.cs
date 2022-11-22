using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace UniversalFramework
{
	public class JSON
	{
		/// <summary>
		/// 指定编码将字节码转对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vs"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static T ConvertToObject<T>(byte[] vs, Encoding encoding) {
			string str = encoding.GetString(vs);
			return ConvertToObject<T>(str);
		}
		/// <summary>
		/// 使用默认utf-8编码将字节码转对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vs"></param>
		/// <returns></returns>
		public static T ConvertToObject<T>(byte[] vs) {
			return ConvertToObject<T>(vs, Encoding.UTF8);
		}
		/// <summary>
		/// 将string转Object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T ConvertToObject<T>(string str) {
			return JsonUtility.FromJson<T>(str);
		}
		/// <summary>
		/// 对象转json
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ConverToJson(object obj) {
			return JsonUtility.ToJson(obj);
        }
	}
	
}