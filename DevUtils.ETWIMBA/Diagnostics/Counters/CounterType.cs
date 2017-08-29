namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Values that represent CounterType. </summary>
	public enum CounterType
	{
		/// <summary> This counter type collects the last observed value only. 
		/// The value is used as the denominator of a counter that presents a general arithmetic fraction. 
		/// This type supports the <see cref="PerfRawFraction" /> counter type.
		/// Generic type           Instantaneous.
		/// Formula                None. Uses raw data in factional calculations without displaying an output.
		/// Average                SUM (N) / x
		/// Example                This type of counter is used to calculate the Disk Queue Length counters.
		/// </summary>
		[StringValue("perf_raw_base")]
		PerfRawBase = 0x40030403,

		/// <summary> The divisor for a sample, used with the previous counter to form a
		/// sampled %.  You must check for >0 before dividing by this! This counter will directly follow the  numerator counter. 
		/// It should not be displayed to the user.
		/// </summary>
		[StringValue("perf_sample_base")]
		PerfSampleBase = 0x40030401,

		/// <summary> This counter is used as the base data (denominator) in the computation of time or count averages for the <see cref="PerfAverageBulk" /> and <see cref="PerfAverageTimer" /> counter types.
		/// This counter type collects the last observed value only.
		/// Generic type           Instantaneous.
		/// Formula                None. This counter uses raw data in factional calculations without displaying an output.
		/// Average                SUM (N) / x
		/// Example                Counters of this type can be used by the Windows Server 2003 family to calculate the Average Disk Transfer counters.
		/// </summary>
		[StringValue("perf_average_base")]
		PerfAverageBase = 0x40030402,

		/// <summary> This counter type shows how many items are processed, on average, during an operation. 
		/// Counters of this type display a ratio of the items processed (such as bytes sent) to the number of operations completed. 
		/// The ratio is calculated by comparing the number of items processed during the last interval to the number of operations completed during the last interval.
		/// Generic type           Average.
		/// Formula                (N1 - N0) / (D1 - D0), where the numerator (N) represents the number of items processed during the last sample interval, and the denominator (D) represents the number of operations completed during the last two sample intervals.
		/// Average                (Nx - N0) / (Dx - D0)
		/// Example                PhysicalDisk\ Avg. Disk Bytes/Transfer
		/// </summary>
		[StringValue("perf_average_bulk")]
		PerfAverageBulk = 0x40020500,

		///// <summary> This counter type shows a variable-length text string. 
		///// It does not display calculated values.
		///// Generic type           Text.
		///// Formula                None.
		///// Average                None.
		///// Example                There are no counters of this type installed with Windows Server 2003 family.
		///// </summary>
		//[StringValue("perf_counter_text")]
		//PerfCounterText,

		/// <summary> This counter type shows the ratio of a subset to its set as a percentage. 
		/// For example, it compares the number of bytes in use on a disk to the total number of bytes on the disk. 
		/// Counters of this type display the current percentage only, not an average over time.
		/// Generic type           Instantaneous, Percentage.
		/// Formula                (N0 / D0), where the denominator (D) represents a measured attribute, and the numerator (N) represents one component of that attribute.
		/// Average                SUM (N / D) /x
		/// Example                Paging File\% Usage Peak
		/// </summary>
		[StringValue("perf_raw_fraction")]
		PerfRawFraction = 0x20020400,

		/// <summary> This counter type shows the total time between when the component or process started and the time when this value is calculated. 
		/// The variable F represents the number of time units that elapse in one second. 
		/// The value of F is factored into the equation so that the result is displayed in seconds.
		/// Generic type           Difference.
		/// Formula                (D0 - N0) / F, where the denominator (D) represents the current time, the numerator (N) represents the time the object was started, and the variable F represents the number of time units that elapse in one second.
		/// Average                (Dx - N0) / F
		/// Example                System\System Up Time
		/// </summary>
		[StringValue("perf_elapsed_time")]
		PerfElapsedTime = 0x30240500,

		/// <summary> This counter type shows the active time of a component as a percentage of the total elapsed time of the sample interval. 
		/// It measures time in units of 100 nanoseconds. 
		/// Counters of this type are designed to measure the activity of one component at a time.
		/// Generic type           Percentage.
		/// Formula                (N1 - N0) / (D1 - D0) x 100, where the denominator (D) represents the total elapsed time of the sample interval, and the numerator (N) represents the portions of the sample interval during which the monitored components were active.
		/// Average                (Nx - N0) / (Dx - D0) x 100
		/// Example                Processor\ % User Time
		/// </summary>
		[StringValue("perf_100nsec_timer")]
		Perf100NsecTimer = 0x20510500,

		/// <summary> This counter type measures the average time it takes to complete a process or operation. 
		/// Counters of this type display a ratio of the total elapsed time of the sample interval to the number of processes or operations completed during that time. 
		/// This counter type measures time in ticks of the system clock. The variable F represents the number of ticks per second. 
		/// The value of F is factored into the equation so that the result is displayed in seconds.
		/// Generic type           Average.
		/// Formula                ((N1 - N0) / F) / (D1 - D0), where the numerator (N) represents the number of ticks counted during the last sample interval, the variable F represents the frequency of the ticks, and the denominator (D) represents the number of operations completed during the last sample interval.
		/// Average                ((Nx - N0) / F) / (Dx - D0)
		/// Example                PhysicalDisk\ Avg. Disk sec/Transfer
		/// </summary>
		[StringValue("perf_average_timer")]
		PerfAverageTimer = 0x30020400,

		/// <summary> This counter type shows the change in the measured attribute between the two most recent sample intervals.
		/// Generic type           Difference.
		/// Formula                N1 - N0
		/// Average                (Nx - N0) / x
		/// Example                There are no counters of this type installed with the Windows Server 2003 family.
		/// </summary>
		[StringValue("perf_counter_delta")]
		PerfCounterDelta = 0x400400,

		/// <summary> The following table shows the most common timer.
		/// Element                Value
		/// X                      CounterData
		/// Y                      PerfTime
		/// Time base              PerfFreq
		/// Data Size              8 Bytes
		/// Display Suffix         %
		/// Calculation            100*(X1-X0)/(Y1-Y0) </summary>
		[StringValue("perf_counter_timer")]
		PerfCounterTimer = 0x20410500,

		/// <summary> This 64-bit counter type is a timer that displays in object-specific units.
		/// Generic type           Percentage.
		/// Formula                (X1-X0) / (Y1-Y0), where the denominator (Y) represents the performance time measurement, and the numerator (X) represents counter data.
		/// Average                (X1-X0) / (Y1-Y0)
		/// </summary>
		[StringValue("perf_obj_time_timer")]
		PerfObjTimeTimer = 0x20610500,

		/// <summary> This counter type collects the last observed value. 
		/// It is the same as the <see cref="PerfRawBase" /> counter type except that it uses larger fields to accomaodate larger values.
		/// Generic type           Instantaneous.
		/// Formula                None. Uses raw data in factional calculations without displaying an output.
		/// Average                SUM (N) / x
		/// </summary>
		[StringValue("perf_large_raw_base")]
		PerfLargeRawBase = 0x40030500,

		/// <summary> This counter type shows the average ratio of hits to all operations during the last two sample intervals.
		/// Generic type           Percentage; Hits %
		/// Formula                (N1 - N0) / (D1 - D0), where the numerator (N) represents the number of successful operations during the last sample interval, and the denominator (D) represents the change in the number of all operations (of the type measured) completed during the sample interval.
		/// Average                (Nx - N0) / (Dx - D0) (See Note below)
		/// Example                Cache\Pin Read Hits %
		/// </summary>
		[StringValue("perf_sample_fraction")]
		PerfSampleFraction = 0x20C20400,

		/// <summary> This counter type shows the average number of operations completed in one second. 
		/// It measures time in units of ticks of the system performance timer. 
		/// The variable F represents the number of ticks that occur in one second. 
		/// The value of F is factored into the equation so that the result is displayed in seconds.
		/// Generic type           Average.
		/// Formula                (N1 - N0) / ((D1 - D0) / F), where the numerator (N) represents the number of operations completed, the denominator (D) represents elapsed time in units of ticks of the system performance timer, and the variable F represents the number of ticks that elapse in one second.
		/// Average                (Nx - N0) / ((Dx - D0) / F)
		/// </summary>
		[StringValue("perf_sample_counter")]
		PerfSampleCounter = 0x410400,

		/// <summary> The following table shows how PERF_COUNTER_COUNTER tracks the rate of counts. 
		/// This is the most common counter. 
		/// Element                Value
		/// X                      CounterData
		/// Y                      PerfTime
		/// Time base              PerfFreq
		/// Data Size              4 Bytes
		/// Display Suffix         /Sec
		/// Calculation            (X1-X0)/((Y1-Y0)/TB)</summary>
		[StringValue("perf_counter_counter")]
		PerfCounterCounter = 0x10410400,

		/// <summary> This counter type shows the average percentage of active time observed during the sample interval. 
		/// This is an inverse counter. Inverse counters are calculated by monitoring the percentage of time that the service was inactive and then subtracting that value from 100 percent.
		/// Generic type           Percentage.
		/// Formula                (1- ((N1 - N0) / (D1 - D0))) x 100, where the denominator (D) represents the total elapsed time of the sample interval, and the numerator (N) represents the time during the interval when the monitored components were inactive.
		/// Average                (1- ((Nx - N0) / (Dx - D0))) x 100
		/// Example                Processor\ % Processor Time
		/// </summary>
		[StringValue("perf_100nsec_timer_inv")]
		Perf100NsecTimerInv = 0x21510500,

		/// <summary> This is an inverse counter type. Inverse counters measure the time that a component is not active, and derive the active time from that measurement. 
		/// Counters of this type display the average percentage of active time observed during sample interval. 
		/// The value of these counters is calculated by monitoring the percentage of time that the service was inactive and then subtracting that value from 100 percent. 
		/// This counter type is the same as the <see cref="Perf100NsecTimerInv" /> counter type,
		/// except that it measures time in units of ticks of the system performance timer, rather than in 100 nanosecond units.
		/// Generic type           Percentage.
		/// Formula                (1- ((N1 - N0) / (D1 - D0))) x 100, where the denominator (D) represents the total elapsed time of the sample interval, and the numerator (N) represents the time during the interval when the monitored components were inactive.
		/// Average                (1- ((Nx - N0) / (Dx - D0))) x 100
		/// </summary>
		[StringValue("perf_counter_timer_inv")]
		PerfCounterTimerInv = 0x21410500,

		/// <summary> This counter type shows the ratio of a subset to its set as a percentage. 
		/// 					For example, it compares the number of bytes in use on a disk to the total number of bytes on the disk. 
		/// 					Counters of this type display the current percentage only, not an average over time. 
		/// 					It is the same as the <see cref="PerfRawFraction" /> counter type, except that it uses larger fields to accommodate larger values. </summary>
		[StringValue("perf_large_raw_fraction")]
		PerfLargeRawFraction = 0x20020500,

		/// <summary>  Number of instances to which the preceding _MULTI_..._INV counter applies.
		/// 					 Used as a factor to get the percentage.
		/// </summary>
		[StringValue("perf_counter_multi_base")]
		PerfCounterMultiBase = 0x42030500,

		/// <summary> This counter type shows the last observed value only. 
		/// Generic type           Instantaneous.
		/// Formula                None. Shows raw data as collected.
		/// Average                SUM (N) / x
		/// Example                Memory\Available Bytes
		/// </summary>
		[StringValue("perf_counter_rawcount")]
		PerfCounterRawcount = 0x10000,

		///// <summary> PerfCounterComposite
		///// Generic type           -
		///// Formula                -
		///// Average                -
		///// Example                -
		///// </summary>
		//[StringValue("perf_counter_composite")]
		//PerfCounterComposite = 0x,

		/// <summary> This counter type shows the average number of operations completed during each second of the sample interval. 
		/// Counters of this type measure time in ticks of the system clock. 
		/// The variable F represents the number of ticks per second. 
		/// The value of F is factored into the equation so that the result is displayed in seconds. 
		/// This counter type is the same as the <see cref="PerfCounterCounter" /> type, but it uses larger fields to accommodate larger values.
		/// Generic type           Difference.
		/// Formula                (N1 - N0) / ( (D1 - D0) / F, where the numerator (N) represents the number of operations performed during the last sample interval, the denominator (D) represent the number of ticks elapsed during the last sample interval, and the variable F is the frequency of the ticks.
		/// Average                (Nx - N0) / ((Dx - D0)/ F)
		/// Example                System\File Read Bytes/sec 
		/// </summary>
		[StringValue("perf_counter_bulk_count")]
		PerfCounterBulkCount = 0x10410500,

		/// <summary> This counter type shows the change in the measured attribute between the two most recent sample intervals. 
		/// It is the same as the <see cref="PerfCounterDelta" /> counter type, except that it uses larger fields to accommodate larger values.
		/// Generic type           Difference.
		/// Formula                N1 - N0
		/// Average                (Nx - N0) / x
		/// </summary>
		[StringValue("perf_counter_large_delta")]
		PerfCounterLargeDelta = 0x400500,

		/// <summary> This counter type shows the active time of one or more components as a percentage of the total time of the sample interval.
		/// It measures time in 100 nanosecond units. This counter type is a multitimer. 
		/// Multitimers are designed to monitor more than one instance of a component, such as a processor or disk.
		/// Generic type           Percentage.
		/// Formula                ((N1 - N0) / (D1 - D0)) x 100 / B, where the denominator (D) represents the total elapsed time of the sample interval, the numerator (N) represents the portions of the sample interval during which the monitored components were active, and the variable B represents the base count for the monitored components.
		/// Average                ((Nx - N0) / (Dx - D0) ) x 100 / B
		/// </summary>
		[StringValue("perf_100nsec_multi_timer")]
		Perf100NsecMultiTimer = 0x22510500,

		/// <summary> This counter type is a multitimer. Multitimers collect data from more than one instance of a component, such as a processor or disk. 
		/// Counters of this type display the active time of one or more components as a percentage of the total time of the sample interval. 
		/// Because the numerator records the active time of components operating simultaneously, the resulting percentage can exceed 100 percent.
		/// This counter type differs from <see cref="Perf100NsecMultiTimer" /> in that it measures time in units of ticks of the system performance timer, rather than in 100 nanosecond units.
		/// Generic type           Percentage.
		/// Formula                ((N1 - N0) / (D1 - D0)) x 100, where the denominator (D) represents the total elapsed time of the sample interval, and the numerator (N) represents the portions of the sample interval during which the monitored components were active. They are multiplied by 100 to display the value as a percentage.
		/// Average                (Nx - N0) / (Dx - D0) x 100
		/// </summary>
		[StringValue("perf_counter_multi_timer")]
		PerfCounterMultiTimer = 0x22410500,

		/// <summary> This counter type shows a value that consists of two counter values: 
		/// the count of the elapsed time of the event being monitored, and the "clock" time from a private timer in the same units. 
		/// It measures time in 100 nanosecond units. This counter type differs from other counter timers in that the clock tick value 
		/// accompanies the counter value eliminating any possible difference due to latency from the function call. 
		/// Precision counter types are used when standard system timers are not precise enough for accurate readings.
		/// Generic type           Percentage.
		/// Formula                N1 - N0 / D1 - D0, where the numerator (N) represents the counter value, and the denominator (D) is the value of the private timer. The private timer has the same frequency as the 100 nanosecond timer.
		/// Average                N1 - N0 / D1 - D0
		/// Example                PhysicalDisk\% Disk Time
		/// </summary>
		[StringValue("perf_precision_100ns_timer")]
		PerfPrecision100NSTimer = 0x20570500,

		/// <summary> This counter type shows a value that consists of two counter values: the count of the elapsed time of the event being monitored, 
		/// and the frequency specified in the PerfFreq field of the object header. 
		/// This counter type differs from other counter timers in that the clock tick value accompanies the counter value so as 
		/// to eliminate any possible difference due to latency from the function call. Precision counter types are used when standard system timers are not precise enough for accurate readings.
		/// Generic type           Percentage.
		/// Formula                Nx - N0 / D1 - D0, where the numerator (N) represents the counter value and the denominator (D) represents the value of the private timer.
		/// Average                Nx - N0 / D1 - D0
		/// </summary>
		[StringValue("perf_precision_object_timer")]
		PerfPrecisionObjectTimer = 0x20670500,

		/// <summary> This counter type shows the active time of one or more components as a percentage of the total time of the sample interval. 
		/// Counters of this type measure time in 100 nanosecond units. 
		/// This counter type is an inverse multitimer. Multitimers are designed to monitor more than one instance of a component, such as a processor or disk. 
		/// Inverse counters measure the time that a component is not active and derive its active time from the measurement of inactive time.
		/// Generic type           Percentage.
		/// Formula                (B - ((N1 - N0) / (D1 - D0))) x 100, where the denominator (D) represents the total elapsed time of the sample interval, the numerator (N) represents the time during the interval when monitored components were inactive, and the variable B represents the number of components being monitored.
		/// Average                (B - ((Nx - N0) / (Dx - D0))) x 100
		/// </summary>
		[StringValue("perf_100nsec_multi_timer_inv")]
		Perf100NsecMultiTimerInv = 0x23510500,

		/// <summary> This counter type shows the most recently observed value, in hexadecimal format. 
		/// Generic type           Instantaneous.
		/// Formula                None. Shows raw data as collected.
		/// Average                SUM (N) / x
		/// </summary>
		[StringValue("perf_counter_rawcount_hex")]
		PerfCounterRawcountHex = 0x0,

		/// <summary> This counter type shows the active time of one or more components as a percentage of the total time of the sample interval.
		/// This counter type is an inverse multitimer. Multitimers monitor more than one instance of a component, such as a processor or disk.
		/// Inverse counters measure the time that a component is not active, and derive the active time from that measurement.
		/// This counter differs from <see cref="Perf100NsecMultiTimerInv" /> in that it measures time in units of ticks of the system performance timer,
		/// rather than in 100 nanosecond units.
		/// Generic type           Percentage.
		/// Formula                (B- ((N1 - N0) / (D1 - D0))) x 100, where the denominator (D) represents the total elapsed time of the sample interval, the numerator (N) represents the time during the interval when monitored components were inactive, and the variable B represents the number of components being monitored.
		/// Average                (B- ((Nx - N0) / (Dx - D0))) x 100
		/// </summary>
		[StringValue("perf_counter_multi_timer_inv")]
		PerfCounterMultiTimerInv = 0x23410500,

		/// <summary> This counter is the same as <see cref="PerfCounterQueuelenType" /> but it requires the monitoring interval to be measured with 100-nanosecond precision.
		/// Generic type           Percentage.
		/// Formula                (V1 - V0) / (T1 - T0)
		/// Average                (V1 - V0) / (T1 - T0)
		/// </summary>
		[StringValue("perf_precision_system_timer")]
		PerfPrecisionSystemTimer = 0x20470500,

		/// <summary> This counter type is designed to monitor the average length of a queue to a resource over time. 
		/// It shows the difference between the queue lengths observed during the last two sample intervals 
		/// divided by the duration of the interval. The following table shows the inverse of the timer for multiple but similar items.
		/// The counter is used when the objects are not in use.
		/// Element	               Value
		/// X                      CounterData
		/// Y                      PerfTime
		/// Time base              PerfFreq
		/// Data Size              4 Bytes
		/// Display Suffix         No suffix
		/// Calculation            (X1-X0)/(Y1-Y0)
		/// </summary>
		[StringValue("perf_counter_queuelen_type")]
		PerfCounterQueuelenType = 0x450400,

		/// <summary> This counter type shows the last observed value only, not an average. 
		///It is the same as the <see cref="PerfCounterRawcount" /> counter type,
		///except that it uses larger fields to accommodate larger values.
		///Generic type           Instantaneous.
		///Formula                None. Shows raw data as collected.
		///Average                SUM (N) / x
		/// </summary>
		[StringValue("perf_counter_large_rawcount")]
		PerfCounterLargeRawcount = 0x10100,

		/// <summary> This counter type shows the last observed value, in hexadecimal format. 
		/// It is the same as the <see cref="PerfCounterRawcountHex" /> counter type, except that it uses larger fields to accommodate larger values.
		/// Generic type           Instantaneous.
		/// Formula                None. Shows raw data as collected.
		/// Average                SUM (N) / x
		/// </summary>
		[StringValue("perf_counter_large_rawcount_hex")]
		PerfCounterLargeRawcountHex = 0x100,
		/// <summary>This counter type monitors the average length of a queue to a resource over time. 
		/// Counters of this type display the difference between the queue lengths observed during the last two sample intervals, 
		/// divided by the duration of the interval. 
		/// This counter type is the same as <see cref="PerfCounterQueuelenType" /> except that it uses larger fields to accommodate larger values.
		/// Generic type            Average
		/// Formula                 (N1 - N0) / (D1 - D0), where the numerator (N) represents the number of items in a queue and the denominator (D) represents the time elapsed during the sample interval.
		/// Average                 (Nx - N0) / (Dx - D0)
		/// Example                 PhysicalDisk\ Avg. Disk Queue Length
		/// </summary>
		[StringValue("perf_counter_large_queuelen_type")]
		PerfCounterLargeQueuelenType = 0x450500,
		/// <summary> This counter type measures the queue-length space-time product using a 100-nanosecond time base.
		/// Generic type           Rate.
		/// Formula                (TB(X1-X0)) / (Y1-Y0), where TB represents the performance frequency time base, the denominator (Y) represents the performance time measurement, and the numerator (X) represents counter data.
		/// Average                (TB(X1-X0)) / (Y1-Y0)
		/// </summary>
		[StringValue("perf_counter_100ns_queuelen_type")]
		PerfCounter100NSQueuelenType = 0x550500,
		/// <summary> This counter type measures the queue-length space-time product using an object-specific time base.
		/// Generic type           Percentage.
		/// Formula                (X1-X0) / (Y1-Y0), where the denominator (Y) represents the performance time measurement, and the numerator (X) represents counter data.
		/// Average                (X1-X0) / (Y1-Y0)
		/// </summary>
		[StringValue("perf_counter_obj_time_queuelen_type")]
		PerfCounterObjTimeQueuelenType = 0x650500
	}
}
