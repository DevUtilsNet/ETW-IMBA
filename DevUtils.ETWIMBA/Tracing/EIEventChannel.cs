namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Defines the audience for the event (for example, administrator or developer).
	/// </summary>
	public enum EIEventChannel : byte
	{
		/// <summary>
		/// Events for Classic ETW tracing.
		/// </summary>
		TraceClassic = 0,

		/// <summary>
		/// Events for all installed system services.  This channel is secured to applications running under
		/// system service accounts or user applications running under local adminstrator privileges.
		/// </summary>
		System = 8,

		/// <summary>
		/// Events for all user-level applications.  This channel is not secured and open to any applications.
		/// Applications which log extensive information should define an application-specific channel.
		/// </summary>
		Application = 9,

		/// <summary>
		/// The Windows Audit Log.  For exclusive use of the Windows Local Security Authority.  User events
		/// may appear as audits if supported by the underlying application.
		/// </summary>
		Security = 10
	}
}
