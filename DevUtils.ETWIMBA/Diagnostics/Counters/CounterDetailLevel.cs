namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Values that represent CounterDetailLevel. </summary>
	public enum CounterDetailLevel
	{
		/// <summary> Display details about the counter that a typical user would understand. </summary>
		[StringValue("standard")]
		Standard,
		/// <summary> Display details about the counter that only an advanced user would understand. </summary>
		[StringValue("advanced")]
		Advanced
	}
}