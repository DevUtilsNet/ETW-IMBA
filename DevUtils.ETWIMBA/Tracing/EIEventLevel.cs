namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Levels are used to group events and typically indicate the severity or verbosity of an event.
	/// </summary>
	public enum EIEventLevel
	{
		/// <summary>
		/// Identifies an abnormal exit or termination event.
		/// </summary>
		Critical = 1,
		/// <summary>
		/// Identifies a severe error event.
		/// </summary>
		Error,
		/// <summary>
		/// Identifies a warning event such as an allocation failure.
		/// </summary>
		Warning,
		/// <summary>
		/// Identifies a non-error event such as an entry or exit event.
		/// </summary>
		Informational,
		/// <summary>
		/// Identifies a detailed trace event.
		/// </summary>
		Verbose
	}
}
