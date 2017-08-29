using System;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Lists the unique attributes that specify how the counter data is displayed in a consumer application. </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CounterAttribute : Attribute, IRequiredUriAttribute
	{
		internal int? ActualBaseId { get; set; }
		internal int? ActualPerfFreqId { get; set; }
		internal int? ActualPerfTimeId { get; set; }
		internal int? ActualDefaultScale { get; set; }
		internal int? ActualMultiCounterId { get; set; }

		/// <summary> A unique uniform resource identifier that lets users retrieve counter values from any location. </summary>
		/// <value> The URI. </value>
		public string Uri { get; set; }

		/// <summary> The identifier of another counter within the same counter set, whose value is used to calculate this counter's value. 
		/// The following counter types require a base counter:
		/// <see cref="CounterType.PerfAverageTimer" /> Requires the <see cref="CounterType.PerfAverageBase" /> base counter. 
		/// <see cref="CounterType.PerfAverageBulk" /> Requires the <see cref="CounterType.PerfAverageBase" /> base counter.
		/// <see cref="CounterType.PerfCounterMultiTimerInv" /> Requires the <see cref="CounterType.PerfCounterMultiBase" /> base counter.
		/// <see cref="CounterType.PerfLargeRawFraction" /> Requires the <see cref="CounterType.PerfLargeRawBase" /> base counter.
		/// <see cref="CounterType.PerfPrecision100NSTimer" /> Requires the <see cref="CounterType.PerfLargeRawBase" /> base counter.
		/// <see cref="CounterType.PerfRawFraction" /> Requires the <see cref="CounterType.PerfRawBase" />PERF_RAW_BASE base counter.
		/// <see cref="CounterType.PerfSampleFraction" /> Requires the <see cref="CounterType.PerfSampleBase" /> base counter.</summary>
		///<value> The identifier of the base. </value>
		public int BaseId
		{
			get { return ActualBaseId ?? default(int); }
			set { ActualBaseId = value; }
		}

		/// <summary> The name of the counter. The name must be unique and less than 1,024 characters. 
		/// The name is case-sensitive. 
		/// You do not have to specify this attribute if the counter includes the <see cref="CounterAttributeName.NoDisplay" /> attribute. </summary>
		/// <value> The name. </value>
		public string Name { get; set; }

		/// <summary> The identifier of another counter within the same counter set, 
		/// whose frequency value is used to calculate this counter's value. 
		/// The following counter types require a frequency. 
		/// The <see cref="CounterType.PerfCounterLargeRawcount" /> counter type contains the time stamp value.
		/// <see cref="CounterType.PerfElapsedTime" />
		/// <see cref="CounterType.PerfObjTimeTimer" />
		/// <see cref="CounterType.PerfCounterObjTimeQueuelenType" />
		/// <see cref="CounterType.PerfCounterObjTimeQueuelenType" /> </summary>
		/// <value> The identifier of the performance frequency. </value>
		public int PerfFreqId
		{
			get { return ActualPerfFreqId ?? default(int); }
			set { ActualPerfFreqId = value; }
		}

		/// <summary> The identifier of another counter within the same counter set, 
		/// whose time stamp value is used to calculate this counter's value. 
		/// The following counter types require a time stamp. 
		/// The <see cref="CounterType.PerfCounterLargeRawcount" /> counter type contains the time stamp value.
		/// <see cref="CounterType.PerfElapsedTime" />
		/// <see cref="CounterType.PerfObjTimeTimer" />
		/// <see cref="CounterType.PerfPrecisionObjectTimer" />
		/// <see cref="CounterType.PerfCounterObjTimeQueuelenType" />
		/// </summary>
		/// <value> The identifier of the performance time. </value>
		public int PerfTimeId
		{
			get { return ActualPerfTimeId ?? default(int); }
			set { ActualPerfTimeId = value; }
		}

		/// <summary> A short description of the counter. 
		/// You do not have to specify this attribute if the counter includes the <see cref="CounterAttributeName.NoDisplay" /> attribute. </summary>
		/// <value> The description. </value>
		public string Description { get; set; }

		/// <summary> The scale factor to apply to the counter value (factor * counter value). 
		/// The default is zero if no scale is applied. 
		/// Valid values range from –10 to 10 (0.0000000001 to 1000000000). 
		/// If this value is zero, the scale value is 1; 
		/// if this value is 1, the scale value is 10; 
		/// if this value is –1, the scale value is .10; and so on. </summary>
		/// <value> The default scale. </value>
		public int DefaultScale
		{
			get { return ActualDefaultScale ?? default(byte); }
			set { ActualDefaultScale = value; }
		}

		/// <summary> The name of the counter type. 
		/// For possible values, see the above syntax block. 
		/// For details of each type, see Counter Types in the Windows 2003 Deployment Guide. </summary>
		/// <value> The type. </value>
		public CounterType Type { get; set; }

		/// <summary> The identifier of another counter within the same counter set, whose multiplier value is used to calculate this counter's value. 
		/// The following counter types require a multiplier value. 
		/// The referenced counter must be of type <see cref="CounterType.PerfCounterRawcount" />.
		/// <see cref="CounterType.PerfCounterMultiTimer" />
		/// <see cref="CounterType.PerfCounterMultiTimerInv" />
		/// <see cref="CounterType.Perf100NsecMultiTimer" />
		/// <see cref="CounterType.Perf100NsecMultiTimerInv" /> </summary>
		/// <value> The identifier of the multi counter. </value>
		public int MultiCounterId
		{
			get { return ActualMultiCounterId ?? default(int); }
			set { ActualMultiCounterId = value; }
		}

		/// <summary> The aggregation function to apply if the instances attribute of counterSet is <see cref="CounterSetInstances.GlobalAggregate" />,
		/// <see cref="CounterSetInstances.MultipleAggregate" />, or <see cref="CounterSetInstances.GlobalAggregateHistory" />. </summary>
		/// <value> The aggregate. </value>
		public CounterAggregate Aggregate { get; set; }

		/// <summary> Specifies the target audience for the counter details. </summary>
		/// <value> The detail level. </value>
		public CounterDetailLevel DetailLevel { get; set; }

		/// <summary> Constructor. </summary>
		/// <param name="type"> The type. </param>
		public CounterAttribute(CounterType type)
		{
			Type = type;
		}
	}
}