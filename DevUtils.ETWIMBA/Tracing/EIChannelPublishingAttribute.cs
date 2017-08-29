using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Defines the logging properties for the session that the channel uses.
	/// </summary>
	/// <remarks>
	/// Defines the logging properties for the session that the channel uses. Only Debug and Analytic
	/// channels and channels that use Custom isolation can specify logging properties for their
	/// session.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EIChannelPublishingAttribute : Attribute
	{
		/// <summary>
		/// The amount of memory, in kilobytes, to allocate for each buffer. If you expect a relatively low
		/// event rate, the buffer size should be set to the memory page size. If the event rate is
		/// expected to be relatively high, you should specify a larger buffer size and increase the
		/// maximum number of buffers. The buffer size affects the rate at which buffers fill and must be
		/// flushed. Although a small buffer size requires less memory, it increases the rate at which
		/// buffers must be flushed. The default buffer size for Analytic and Debug channels is 4 KB and
		/// for Admin and Operational it is 64 KB.
		/// </summary>
		/// <value>
		/// The size of the buffer.
		/// </value>
		public ulong BufferSize { get; set; }
		/// <summary>
		/// The maximum number of times that you want the service to create a new log file when the channel
		/// is enabled (includes when the computer is restarted). If the value is 0 or 1, the service will
		/// overwrite the log file each time the channel is enabled and the previous events will be lost.
		/// If the value is greater than 1, the service will create a new log file each time the channel is
		/// enabled in order to preserve the events. The default is 1 and the maximum that you can specify
		/// is 16. The service appends a three digit decimal number between 0 and fileMax–1 to each file
		/// name. For example, filename.etl.xxx, where xxx is the three digit decimal number. The files are
		/// located in %windir%\System32\winevt\Logs.
		/// </summary>
		/// <value>
		/// The file maximum.
		/// </value>
		public uint FileMax { get; set; }
		/// <summary>
		/// A bitmask that determines the category of events that are written to the channel. If the value
		/// of keywords attribute is 0, all events that the provider writes are written to the channel;
		/// otherwise only events that have defined a keyword that is included in the keywords bitmask are
		/// written to the channel. The default is 0. Debug channels that have the controlGuid attribute
		/// set must set the keywords attribute to 0xFFFFFFFFFFFFFFFF. The session passes the keywords
		/// value to the provider when it enables the provider.
		/// </summary>
		/// <value>
		/// The keywords.
		/// </value>
		public ulong Keywords { get; set; }
		/// <summary>
		/// The time to wait before flushing the buffers, in milliseconds. If zero, ETW flushes the buffers
		/// as soon as they become full. If nonzero, ETW flushes all buffers that contain events based on
		/// the value even if the buffer is not full. Typically, you want to flush buffers only when they
		/// become full. Forcing the buffers to flush can increase the file size of the log file with
		/// unfilled buffer space. The default value is 1 second for Admin and Operational logs and 5
		/// seconds for Analytic and Debug logs.
		/// </summary>
		/// <value>
		/// The latency.
		/// </value>
		public uint Latency { get; set; }
		/// <summary>
		/// The severity level of the events to write to the channel. The service writes events to the
		/// channel that have a level value that is less than or equal to the specified value. The default
		/// is 0, which means to log events with any level value. The session passes the level value to the
		/// provider when it enables the provider.
		/// </summary>
		/// <value>
		/// The level.
		/// </value>
		public byte Level { get; set; }
		/// <summary>
		/// The maximum number of buffers to allocate for the session. Typically, this value is the minimum
		/// number of buffers plus twenty. This value must be greater than or equal to the value specified
		/// for minBuffers. The default maximum number of buffers for Analytic and Debug channels is 10 KB
		/// and for Admin and Operational it is 64 KB.
		/// </summary>
		/// <value>
		/// The maximum buffers.
		/// </value>
		public uint MaxBuffers { get; set; }
		/// <summary>
		/// The minimum number of buffers to allocate for the session. The default is zero.
		/// </summary>
		/// <value>
		/// The minimum buffers.
		/// </value>
		public uint MinBuffers { get; set; }
	}
}
