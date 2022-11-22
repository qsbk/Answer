using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniversalFramework
{
	/// <summary>
	/// 所有Buff的基类
	/// </summary>
	public abstract class Buff
	{
		/// <summary>
		/// Buff类型ID
		/// </summary>
		public int BuffTypeId { get; set; }
		/// <summary>
		/// Buff施加者
		/// </summary>
		public GameObject Caster { get; set; }
		/// <summary>
		/// Buff挂载者
		/// </summary>
		public GameObject Parent { get; set; }
		/// <summary>
		/// 创建Buff的技能
		/// </summary>
		public GameObject Ability { get; set; }
		/// <summary>
		/// Buff层数
		/// </summary>
		public int BuffLayer { get; set; }
		/// <summary>
		/// Buff等级
		/// </summary>
		public int BuffLevel { get; set; }
		/// <summary>
		/// Buff时长
		/// </summary>
		public float BuffDuration { get; set; }
		/// <summary>
		/// Buff类型
		/// </summary>
		public BuffType BuffTag { get; set; }
		/// <summary>
		/// 免疫的Buff
		/// </summary>
		public BuffType BuffImmuneTag { get; set; }
		/// <summary>
		/// Buff上下文数据
		/// </summary>
		public string Context { get; set; }

		
	}
	
}