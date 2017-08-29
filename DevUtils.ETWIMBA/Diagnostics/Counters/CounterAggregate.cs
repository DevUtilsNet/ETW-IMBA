namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> A counter aggregate. </summary>
	public enum CounterAggregate
	{
		/// <summary> An enum constant representing the none option. </summary>
		None,
		/// <summary> The maximum counter value is returned. </summary>
		[StringValue("max")]
		Max,
		/// <summary> The minimum counter value is returned. </summary>
		[StringValue("min")]
		Min,
		/// <summary> The average counter value is returned. </summary>
		[StringValue("avg")]
		Avg,
		/// <summary> The sum of the counter values is returned. </summary>
		[StringValue("sum")]
		Sum,
		/// <summary> Do not aggregate this counter. </summary>
		[StringValue("undefined")]
		Undefined
	}
}