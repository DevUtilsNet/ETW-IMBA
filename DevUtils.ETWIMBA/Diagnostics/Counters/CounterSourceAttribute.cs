using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary>
	/// Defines a provider and the counters that it provides.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CounterSourceAttribute : Attribute, IRequiredGuidAttribute
	{
		/// <summary>
		/// The name that is used to create the WMI Win32_PerfRawData class name. 
		/// If you do not specify a name, "Counters" is used as the name of the class.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		/// <summary>
		/// String GUID that uniquely identifies the provider in the manifest. The GUID must be unique within the manifest.
		/// You need to provide a new GUID only when the version of the application changes (if you support side-by-side installations).
		/// </summary>
		/// <value>
		/// The identifier of the unique.
		/// </value>
		public string Guid { get; set; }
	}
}