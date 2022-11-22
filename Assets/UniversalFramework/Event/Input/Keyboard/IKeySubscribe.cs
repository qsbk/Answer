using System;

namespace UniversalFramework
{
	public interface IKeySubscribe
	{
		/// <summary>
		/// �����¼�ID
		/// </summary>
		string SubscribeName { get; set; }
		/// <summary>
		/// �����¼�
		/// </summary>
		string SubscribeTag { get;}
		/// <summary>
		/// ���ĵ�����
		/// </summary>
		Key[] Key { get; set; }
		/// <summary>
		/// ��Ӧ�����ķ���
		/// </summary>
		Action SubscribeMethod { get; set; }
		/// <summary>
		/// ���ö��ķ���
		/// </summary>
		/// <returns></returns>
		object Invoke();
	}
	
}