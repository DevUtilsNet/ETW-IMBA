using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Attribute for counter attribute display as hexadecimal. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttributeDisplayAsHexAttribute : CounterAttributeBaseAttribute
	{
		/// <summary> Default constructor. </summary>
		public CounterAttributeDisplayAsHexAttribute()
			: base(CounterAttributeName.DisplayAsHex)
		{
		}
	}
}