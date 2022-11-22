using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace UniversalFramework
{
	public class JSON
	{
		/// <summary>
		/// ָ�����뽫�ֽ���ת����
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
		/// ʹ��Ĭ��utf-8���뽫�ֽ���ת����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vs"></param>
		/// <returns></returns>
		public static T ConvertToObject<T>(byte[] vs) {
			return ConvertToObject<T>(vs, Encoding.UTF8);
		}
		/// <summary>
		/// ��stringתObject
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str"></param>
		/// <returns></returns>
		public static T ConvertToObject<T>(string str) {
			return JsonUtility.FromJson<T>(str);
		}
		/// <summary>
		/// ����תjson
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ConverToJson(object obj) {
			return JsonUtility.ToJson(obj);
        }
	}
	
}