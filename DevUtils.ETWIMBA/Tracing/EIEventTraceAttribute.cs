using System;
using System.Diagnostics.Tracing;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Attribute for event trace.
	/// Defines an event that your provider can write
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EIEventTraceAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the event identifier.
		/// </summary>
		/// <value>
		/// The identifier of the event.
		/// </value>
		public int EventId {  get; private set; }

		/// <summary>
		/// Gets or sets the version.
		/// Version of the event. The version indicates a revision to the event definition. 
		/// You can use this member and the Id member to uniquely identify the event within the scope of a provider.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		public byte Version { get; set; }

		/// <summary>
		/// Gets or sets the channel.
		/// Defines the audience for the event (for example, administrator or developer).
		/// An identifier that identifies the channel to where the event is written. 
		/// Specify a channel identifier of one of the channels that you defined or imported. 
		/// If the channel does not specify a channel identifier, use the channel's name. 
		/// If you do not specify a channel, the event is not written to a channel. 
		/// Typically, the only reason not to specify a channel is if you are writing events only to an ETW session. 
		/// </summary>
		/// <value>
		/// The channel.
		/// </value>
		public int Channel { get; set; }

		/// <summary>
		/// Gets or sets the level.
		/// Specifies the severity or level of detail included in the event (for example, informational or fatal).
		/// The level of verbosity to use when writing the event. 
		/// Specify the name of a level that you defined in the manifest or one of the levels defined in the \Include\Winmeta.xml file that is included in the Windows SDK. 
		/// If you do not specify a level, the event descriptor will contain a zero for level.
		/// </summary>
		/// <value>
		/// The level.
		/// </value>
		public byte Level {  get; set; }

		/// <summary>
		/// Gets or sets the opcode.
		/// Identifies a step in a sequence of operations being performed within the Task.
		/// The name of an opcode that identifies an operation within the task. 
		/// Specify the name of an opcode that you defined in the manifest or one of the opcodes defined in Winmeta.xml. 
		/// If the task that you reference contains task-specific (local) opcodes, you can specify one of its 
		/// task-specific opcodes or an opcode defined at the provider level (a global opcode). 
		/// If you specify a global opcode, the value of the global opcode cannot be the same as one of the local opcodes for the task.
		/// If you reference a local opcode, the task attribute must reference the task to which the local opcode belongs.
		/// If you do not specify an opcode, the event descriptor will contain a zero for opcode.
		/// </summary>
		/// <value>
		/// The opcode.
		/// </value>
		public byte Opcode { get; set; }

		/// <summary>
		/// Gets or sets the task.
		/// Identifies a larger unit of work within an application or component (is broader than the Opcode).
		/// The name of a task that identifies the component or subcomponent that generates this event. 
		/// If you do not specify a task, the event descriptor will contain a zero for task.
		/// </summary>
		/// <value>
		/// The task.
		/// </value>
		public ushort Task { get; set; }

		/// <summary>
		/// Gets or sets the keywords.
		/// Bitmask that specifies a logical group of related events. Each bit corresponds to one group. 
		/// An event may belong to one or more groups. 
		/// The keyword can contain one or more provider-defined keywords, standard keywords, or both.
		/// If you do not specify keywords, the event descriptor will contain a zero for keywords.
		/// </summary>
		/// <value>
		/// The keywords.
		/// </value>
		public ulong Keywords { get; set; }


		/// <summary>
		/// Gets or sets the message.
		/// The localized message for the event. The message string references a localized string in the stringTable section of the manifest.
		/// You must specify a message if the channel type to which the event is written is Admin.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get;  set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="eventId"> The identifier of the event. </param>
		public EIEventTraceAttribute(int eventId)
		{
			EventId = eventId;
			Level = (byte)EventLevel.Informational;
		}
	}
}
