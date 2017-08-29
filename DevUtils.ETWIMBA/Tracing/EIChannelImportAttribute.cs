using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Identifies a channel that has been defined by another provider or in a manifest that contains a metadata section.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EIChannelImportAttribute : Attribute
	{
		/// <summary>
		/// The name of the channel to import.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
	}
}
