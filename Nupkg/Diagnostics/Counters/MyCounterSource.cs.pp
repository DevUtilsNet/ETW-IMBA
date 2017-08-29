using System.Diagnostics;
using System.Threading;
using DevUtils.ETWIMBA.Diagnostics.Counters;

namespace $rootnamespace$.Diagnostics.Counters
{
	[CounterSource(Guid = "{ab8e1320-965a-4cf9-9c07-fe25378c2a23}")]
	sealed class MyCounterSource
	{
		#region MyLogicalDiskSet

		[CounterSet(CounterSetInstances.Multiple,
			Name = "My LogicalDisk",
			Description = "This is a sample counter set with multiple instances.")]
		public enum MyLogicalDiskSet
		{
			[Counter(CounterType.PerfCounterRawcount,
				DefaultScale = 1,
				Name = "My Free Megabytes",
				Description = "First sample counter.",
				DetailLevel = CounterDetailLevel.Standard)]
			MyFreeMegabytes = 1,

			[CounterAttributeReference]
			[CounterAttributeDisplayAsReal]
			[Counter(CounterType.PerfAverageTimer,
				DefaultScale = 1,
				BaseId = (int)MyAvgDiskTransfer,
				Name = "My Avg. Disk sec/Transfer",
				Description = "Second sample counter.",
				DetailLevel = CounterDetailLevel.Advanced)]
			MyAvgDiskSec,

			[CounterAttributeNoDisplay]
			[Counter(CounterType.PerfAverageBase,
				//DefaultScale = 3,
				DetailLevel = CounterDetailLevel.Advanced)]
			MyAvgDiskTransfer,
		}

		#endregion

		#region MySystemObjectsSet

		[CounterSet(CounterSetInstances.Single,
			Name = "My System Objects",
			Description = "My System Objects Help.")]
		public enum MySystemObjectsSet
		{
			[CounterAttributeDisplayAsHex]
			[CounterAttributeNoDigitGrouping]
			[Counter(CounterType.PerfCounterRawcount,
				DefaultScale = 1,
				Name = "Process Count",
				Description = "Process Count Help.")]
			ProcessCount = 1,

			[Counter(CounterType.PerfCounterRawcount,
				Name = "Thread Count",
				Description = "Thread Count Help.")]
			ThreadCount,

			[Counter(CounterType.PerfElapsedTime,
				DefaultScale = 1,
				PerfTimeId = (int)SystemTime,
				PerfFreqId = (int)SystemFreq,
				Name = "System Elapsed Time",
				Description = "System Elapsed Time Help.",
				DetailLevel = CounterDetailLevel.Advanced)]
			SystemElapsedTime,

			[CounterAttributeNoDisplay]
			[Counter(CounterType.PerfCounterLargeRawcount)]
			SystemTime,

			[CounterAttributeNoDisplay]
			[Counter(CounterType.PerfCounterLargeRawcount)]
			SystemFreq
		}

		#endregion

		public static void Test()
		{
			using (var diskSet = new CounterSet<MyLogicalDiskSet>())
			using (var objectsSet = new CounterSet<MySystemObjectsSet>())
			using (var diskSetInst = diskSet.CreateInstance("Default"))
			using (var objectsSetInst = objectsSet.CreateInstance("Default"))
			{
				var processCount = objectsSetInst[MySystemObjectsSet.ProcessCount];
				var myAvgDiskSec = diskSetInst[MyLogicalDiskSet.MyAvgDiskSec];
				var myAvgDiskTransfer = diskSetInst[MyLogicalDiskSet.MyAvgDiskTransfer];

				processCount.Value = 2;

				for (var i = 0; i < 10; ++i)
				{
					var beginTicks = Stopwatch.GetTimestamp();
					// some works
					Thread.Sleep(1000);
					var endTicks = Stopwatch.GetTimestamp();

					myAvgDiskSec.IncrementBy(endTicks - beginTicks);
					myAvgDiskTransfer.IncrementBy(1);
				}
			}
		}
	}
}