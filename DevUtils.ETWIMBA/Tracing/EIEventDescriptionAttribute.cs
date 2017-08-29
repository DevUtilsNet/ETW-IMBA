using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Defines detailed event description
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class EIEventDescriptionAttribute : Attribute
	{
		/// <summary>
		/// Returns event description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gives a proposal what can be done to fix the defect that is denoted by this
		/// </summary>
		public string Solution { get; set; }

		/// <summary>
		/// Gives detailed instructions for executing the solution. 
		/// </summary>
		public string Procedure { get; set; }

		/// <summary>
		/// Gives instructions to verify that the failure is fixed. 
		/// </summary>
		public string Verification { get; set; }
	}
}
