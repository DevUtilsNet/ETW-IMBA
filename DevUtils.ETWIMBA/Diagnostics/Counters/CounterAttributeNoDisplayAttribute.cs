using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Attribute for counter attribute no display. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttributeNoDisplayAttribute : CounterAttributeBaseAttribute
	{
		/// <summary> Default constructor. </summary>
		public CounterAttributeNoDisplayAttribute()
			: base(CounterAttributeName.NoDisplay)
		{
		}
	}
}