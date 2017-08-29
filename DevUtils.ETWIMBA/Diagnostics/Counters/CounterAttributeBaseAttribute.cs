using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> A counter attribute that specifies how the counter data is displayed in a consumer application. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class CounterAttributeBaseAttribute : Attribute
	{
		/// <summary> The name of the display attribute to apply. </summary>
		/// <value> The name. </value>
		public CounterAttributeName Name { get; private set; }

		/// <summary> Default constructor. </summary>
		public CounterAttributeBaseAttribute(CounterAttributeName value)
		{
			Name = value;
		}
	}
}