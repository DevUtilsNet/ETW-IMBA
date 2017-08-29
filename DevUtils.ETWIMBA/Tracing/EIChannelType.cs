namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Identifies the channel's type. You can specify one of the following types:
	/// Analytic and debug channels are disabled by default and should only enabled to determine the cause of an issue. 
	/// For example, you would enable the channel, run the scenario that is causing the issue, disable the channel, 
	/// and then query the events. Note that enabling the channel clears the channel of existing events. 
	/// If the analytic and debug channel uses a circular backing file, you must disable the channel to query its events.
	/// All Admin channels use the same ETW session; the same is true for Operational channels. 
	/// However, each Analytic and Debug channel uses a separate ETW session, which is another reason to only enable 
	/// these channel types when needed (there is a limited number of ETW sessions available).
	/// </summary>
	public enum EIChannelType
	{
		/// <summary>
		/// Debug type channels support events that are used solely by developers to diagnose a problem for debugging.
		/// </summary>
		Debug,
		/// <summary>
		/// Admin type channels support events that target end users, administrators, and support personnel. 
		/// Events written to the Admin channels should have a well-defined solution on which the administrator can act. 
		/// An example of an admin event is an event that occurs when an application fails to connect to a printer. 
		/// These events are either well-documented or have a message associated with them that gives the reader 
		/// direct instructions of what must be done to rectify the problem.
		/// </summary>
		Admin,
		/// <summary>
		/// Operational type channels support events that are used for analyzing and diagnosing a problem or occurrence. 
		/// They can be used to trigger tools or tasks based on the problem or occurrence. 
		/// An example of an operational event is an event that occurs when a printer is added or removed from a system.
		/// </summary>
		Operational,
		/// <summary>
		/// Analytic type channels support events that are published in high volume. 
		/// They describe program operation and indicate problems that cannot be handled by user intervention.
		/// </summary>
		Analytic
	}
}
