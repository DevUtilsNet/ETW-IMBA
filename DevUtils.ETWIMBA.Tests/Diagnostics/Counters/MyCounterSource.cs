using DevUtils.ETWIMBA.Diagnostics.Counters;

namespace DevUtils.ETWIMBA.Tests.Diagnostics.Counters
{
	[CounterSource(Guid = "{ab8e1320-965a-4cf9-9c07-fe25378c2a23}")]
	public class MyCounterSource
	{
		#region MyLogicalDiskSet

		[CounterSet(CounterSetInstances.Single,
			Name = "My LogicalDisk",
			Instances = CounterSetInstances.Multiple,
			Guid = "{dd36a036-c923-4794-b696-70577630b5cf}",
			Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet1",
			Description = "This is a sample counter set with multiple instances.")]
		public enum MyLogicalDiskSet
		{
			[Counter(CounterType.PerfCounterRawcount,
				DefaultScale = 1,
				Name = "My Free Megabytes",
				Description = "First sample counter.",
				DetailLevel = CounterDetailLevel.Standard,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet1.MyCounter1"
				)] MyFreeMegabytes = 1,

			[CounterAttributeReference] 
			[CounterAttributeDisplayAsReal] 
			[Counter(CounterType.PerfAverageTimer,
				DefaultScale = 1,
				BaseId = (int) MyAvgDiskTransfer,
				Name = "My Avg. Disk sec/Transfer",
				Description = "Second sample counter.",
				DetailLevel = CounterDetailLevel.Advanced,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet1.MyCounter2")] MyAvgDiskSec,

			[CounterAttributeNoDisplay] 
			[Counter(CounterType.PerfAverageBase,
				DetailLevel = CounterDetailLevel.Advanced,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet1.MyCounter3")] MyAvgDiskTransfer
		}

		#endregion

		#region MySystemObjectsSet

		[CounterSet(CounterSetInstances.Single,
			Name = "My System Objects",
			Instances = CounterSetInstances.Single,
			Description = "My System Objects Help.",
			Guid = "{f72fdf55-eaa6-45ba-bf6d-4c7cb0d6ef73}",
			Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2")]
		public enum MySystemObjectsSet
		{
			[CounterAttributeDisplayAsHex]
			[CounterAttributeNoDigitGrouping]
			[Counter(CounterType.PerfCounterRawcount,
				DefaultScale = 1,
				Name = "Process Count",
				Description = "Process Count Help.",
				DetailLevel = CounterDetailLevel.Standard,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2.MyCounter1")] ProcessCount = 1,

			[Counter(CounterType.PerfCounterRawcount,
				Name = "Thread Count",
				Description = "Thread Count Help.",
				DetailLevel = CounterDetailLevel.Standard,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2.MyCounter2")] ThreadCount,

			[Counter(CounterType.PerfElapsedTime,
				DefaultScale = 1,
				PerfTimeId = (int) SystemTime,
				PerfFreqId = (int) SystemFreq,
				Name = "System Elapsed Time",
				Description = "System Elapsed Time Help.",
				DetailLevel = CounterDetailLevel.Advanced,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2.MyCounter3")] SystemElapsedTime,

			[CounterAttributeNoDisplay]
			[Counter(CounterType.PerfCounterLargeRawcount,
				DetailLevel = CounterDetailLevel.Standard,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2.MyCounter4")] SystemTime,

			[CounterAttributeNoDisplay]
			[Counter(CounterType.PerfCounterLargeRawcount,
				DetailLevel = CounterDetailLevel.Standard,
				Uri = "Microsoft.Windows.System.PerfCounters.MyCounterSet2.MyCounter5")] SystemFreq
		}

		#endregion
	}
}