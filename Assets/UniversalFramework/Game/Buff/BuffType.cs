using System;


namespace UniversalFramework
{
	/// <summary>
	/// Buff的类型分类
	/// </summary>
	[Flags]
	public enum BuffType
	{
		/// <summary>
		/// 增益效果
		/// </summary>
		Buff,
		/// <summary>
		/// 负面效果
		/// </summary>
		DeBuff,
		/// <summary>
		/// 弱控制
		/// </summary>
		WeakControl,
		/// <summary>
		/// 强控制
		/// </summary>
		StrongControl
	}
	
}