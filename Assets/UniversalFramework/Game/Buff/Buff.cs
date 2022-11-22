using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
	/// <summary>
	/// ����Buff�Ļ���
	/// </summary>
	public abstract class Buff
	{
		/// <summary>
		/// Buff����ID
		/// </summary>
		public int BuffTypeId { get; set; }
		/// <summary>
		/// Buffʩ����
		/// </summary>
		public GameObject Caster { get; set; }
		/// <summary>
		/// Buff������
		/// </summary>
		public GameObject Parent { get; set; }
		/// <summary>
		/// ����Buff�ļ���
		/// </summary>
		public GameObject Ability { get; set; }
		/// <summary>
		/// Buff����
		/// </summary>
		public int BuffLayer { get; set; }
		/// <summary>
		/// Buff�ȼ�
		/// </summary>
		public int BuffLevel { get; set; }
		/// <summary>
		/// Buffʱ��
		/// </summary>
		public float BuffDuration { get; set; }
		/// <summary>
		/// Buff����
		/// </summary>
		public BuffType BuffTag { get; set; }
		/// <summary>
		/// ���ߵ�Buff
		/// </summary>
		public BuffType BuffImmuneTag { get; set; }
		/// <summary>
		/// Buff����������
		/// </summary>
		public string Context { get; set; }

		
	}
	
}