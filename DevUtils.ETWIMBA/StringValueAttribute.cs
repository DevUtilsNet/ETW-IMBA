using System;

namespace DevUtils.ETWIMBA
{
	[AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
	sealed class StringValueAttribute : Attribute
	{
		public string Value { get; private set; }

		public StringValueAttribute(string value)
		{
			Value = value;
		}
	}
}
