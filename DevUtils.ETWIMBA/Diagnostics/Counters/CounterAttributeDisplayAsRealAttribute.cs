using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Attribute for counter attribute display as real. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttributeDisplayAsRealAttribute : CounterAttributeBaseAttribute
	{
		/// <summary> Default constructor. </summary>
		public CounterAttributeDisplayAsRealAttribute()
			: base(CounterAttributeName.DisplayAsReal)
		{
		}
	}
}