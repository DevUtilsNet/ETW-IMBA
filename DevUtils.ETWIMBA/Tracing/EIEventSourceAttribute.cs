using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Attribute for event source.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EIEventSourceAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets an unique identifier.
		/// </summary>
		/// <value>
		/// The identifier of the unique.
		/// </value>
		public string Guid { get; set; }
	}
}
