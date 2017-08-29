using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Defines the properties of the log file that backs the channel, 
	/// such as its capacity and whether it is sequential or circular.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EIChannelLoggingAttribute : Attribute
	{
		/// <summary>
		/// Determines whether to create a new log file when the current log file reaches its maximum size. 
		/// Set to true to request that the service create a new file when the log file reaches its maximum size; otherwise, false. 
		/// You can set <see cref="AutoBackup"/> to true only if retention is set to true. The default is false.
		/// </summary>
		/// <remarks>
		/// You can specify the AutoBackup only for Admin and Operational channel types.
		/// </remarks>
		/// <value>
		/// true if automatic backup, false if not.
		/// </value>
		public bool AutoBackup { get; set; }
		/// <summary>
		/// The maximum size, in bytes, of the log file. The default (and minimum) value is 1 MB. 
		/// If the physical log size is less than the configured maximum size and additional space is required in the log to store events, 
		/// the service will allocate another block of space even if the resulting physical size of the log will be larger than the configured maximum size. 
		/// The service allocates blocks of 1 MB so the physical size could grow to up to 1 MB larger than the configured max size.
		/// </summary>
		/// <remarks>
		/// You can specify the MaxSize for any channel type.
		/// </remarks>
		/// <value>
		/// The size of the maximum.
		/// </value>
		public ulong MaxSize { get; set; }
		/// <summary>
		/// Determines whether the log file is a sequential or circular log file. 
		/// Set to true for a sequential log file and false for a circular log file. 
		/// The default is false for Admin and Operational channel types and true for Analytic and Debug channel types.
		/// To query a circular log, you must first disable the channel.
		/// </summary>
		/// <remarks>
		/// You can set the retention attribute to false (circular logging) for Admin and Operational channel types. 
		/// You can set the retention attribute to false (circular logging) for Analytic and Debug channel types but to view the events in the Windows Event Viewer, 
		/// you will need to first disable the channel. Note that when you enable the channel again, the events are removed from the channel.
		/// </remarks>
		/// <value>
		/// true if retention, false if not.
		/// </value>
		public bool Retention { get; set; }
	}
}
