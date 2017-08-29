using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Attribute for counter attribute no digit grouping. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttributeNoDigitGroupingAttribute : CounterAttributeBaseAttribute
	{
		/// <summary> Default constructor. </summary>
		public CounterAttributeNoDigitGroupingAttribute()
			: base(CounterAttributeName.NoDigitGrouping)
		{
		}
	}
}