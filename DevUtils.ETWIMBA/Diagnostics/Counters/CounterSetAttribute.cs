using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary>
	/// Attribute for counter set.
	/// </summary>
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
	public sealed class CounterSetAttribute : Attribute, IRequiredGuidAttribute, IRequiredUriAttribute, IRequiredDescriptionAttribute
	{
		/// <summary> A unique uniform resource identifier that lets users access the counters in the counter set from any location. </summary>
		/// <value> The URI. </value>
		public string Uri { get; set; }

		/// <summary> A GUID that uniquely identifies the counter set. 
		/// The counter set registration fails if the GUID is already registered. 
		/// To update a counter set that is registered, 
		/// you must first uninstall the counter set and then register it again. </summary>
		/// <value> The identifier of the unique. </value>
		public string Guid { get; set; }

		/// <summary> The display name of the counter set. 
		/// Must be less than 1,024 characters. 
		/// The name is case-sensitive. </summary>
		/// <value> The name. </value>
		public string Name { get; set; }

		/// <summary> Determines whether the counter set can contain multiple instances. </summary>
		/// <value> The instances. </value>
		public CounterSetInstances Instances { get; set; }

		/// <summary> A short description of the counter set. </summary>
		/// <value> The description. </value>
		public string Description { get; set; }

		/// <summary> Constructor. </summary>
		/// <param name="instances"> The instances. </param>
		public CounterSetAttribute(CounterSetInstances instances)
		{
			Instances = instances;
		}
	}
}