using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public class UserManager
	{
		/// <summary>
		/// 缓存用户ID，避免多次调用SQL
		/// </summary>
		static List<int> cache = new List<int>();
		/// <summary>
		/// 添加用户
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public static void AddUser(User user) {
			
			if (Contains(user.Id)){
				
				return;
            }
			UpUser(user);
		}
		/// <summary>
		/// 通过ID获取用户数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static User GetUser(long id) {
			return GameDocuments.Instance.GetUserByID(id);
        }
		/// <summary>
		/// 更新用户
		/// </summary>
		/// <param name="user"></param>
		public static void UpUser(User user) {
			
			GameDocuments.Instance.UpUser(user);
        }
		public static bool Contains(long id) {
			return GameDocuments.Instance.IsUserExists(id);
        }
	}
	
}