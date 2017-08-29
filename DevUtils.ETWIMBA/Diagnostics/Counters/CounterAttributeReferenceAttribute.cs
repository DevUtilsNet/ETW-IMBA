using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Attribute for counter attribute reference. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttributeReferenceAttribute : CounterAttributeBaseAttribute
	{
		/// <summary> Default constructor. </summary>
		public CounterAttributeReferenceAttribute()
			: base(CounterAttributeName.Reference)
		{
		}
	}
}