namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Determines whether the counter set can contain multiple instances. </summary>
	public enum CounterSetInstances
	{
		/// <summary> Defines a counter set where only one instance of the counters in the counter set can exist. 
		/// Specify this value if the counters provide system-wide measurements, such as physical memory. 
		/// This is the default. </summary>
		[StringValue("single")]
		Single,
		/// <summary> Defines a counter set where multiple instances of the counters in the counter set can exist. 
		/// Specify this value if the counters provide per-instance measurements, such as processor time per process. </summary>
		[StringValue("multiple")]
		Multiple = 0x2,
		/// <summary> Defines a single instance counter set where the counters in the counter set must be aggregated from various active sources. 
		/// For example, you could create a counter set that contains a counter that counts the number of disk reads for a hard disk. 
		/// If the computer has three hard disks and a consumer queries for the number of disk reads, 
		/// PERFLIB will obtain the number of reads from each disk and sum their individual values. </summary>
		[StringValue("globalAggregate")]
		GlobalAggregate = 0x4,
		/// <summary> Defines a multiple instance counter set where the counters in the counter set must be aggregated across all instances of that counter. 
		/// For example, you could create a counter set for a multi-threaded application that contains a counter that measures thread performance 
		/// (each thread would refer to an instance of the counter set). When a consumer queries the total thread execution time counter, 
		/// PERFLIB will sum the total thread execution time from each instance. </summary>
		[StringValue("multipleAggregate")]
		MultipleAggregate = 0x6,
		/// <summary> Defines a single instance counter set whose counter values are cached for the lifetime of the consumer. 
		/// Note that all counters in the counter set are cached. 
		/// To cache only specific counters, decorate those counters with the history attribute. 
		/// Using the disk read example from globalAggregate, all counter values in the counter set would be cached. 
		/// If one disk became unavailable, the last cached value for total bytes read by that disk would still be available to the consumer application. </summary>
		[StringValue("globalAggregateHistory")]
		GlobalAggregateHistory = 0xB
	}
}