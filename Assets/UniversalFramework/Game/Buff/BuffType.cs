using System;


namespace UniversalFramework
{
	/// <summary>
	/// Buff�����ͷ���
	/// </summary>
	[Flags]
	public enum BuffType
	{
		/// <summary>
		/// ����Ч��
		/// </summary>
		Buff,
		/// <summary>
		/// ����Ч��
		/// </summary>
		DeBuff,
		/// <summary>
		/// ������
		/// </summary>
		WeakControl,
		/// <summary>
		/// ǿ����
		/// </summary>
		StrongControl
	}
	
}