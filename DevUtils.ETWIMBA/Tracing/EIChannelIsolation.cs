namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// The isolation value defines the default access permissions for the channel. You can specify one of the following values:
	/// Channels that specify Application isolation use the same ETW session. The same is true for System isolation. 
	/// However, if you specify Custom isolation, the service creates a separate ETW session for the channel. 
	/// Using Custom isolation lets you control the access permissions for the channel and backing file. 
	/// Because there are only 64 ETW sessions available, you should limit your use of Custom isolation.
	/// </summary>
	public enum EIChannelIsolation
	{
		/// <summary>
		/// An enum constant representing the none option.
		/// </summary>
		None,
		/// <summary>
		/// The default permissions for Application are (shown using SDDL):
		/// "O:BAG:SYD:"
		/// "(A;;0xf0007;;;SY)"                local system               (read, write, clear)
		/// "(A;;0x7;;;BA)"                    built-in admins            (read, write, clear)
		/// "(A;;0x7;;;SO)"                    server operators           (read, write, clear)
		/// "(A;;0x3;;;IU)"                    INTERACTIVE LOGON          (read, write)
		/// "(A;;0x3;;;SU)"                    SERVICES LOGON             (read, write)
		/// "(A;;0x3;;;S-1-5-3)"               BATCH LOGON                (read, write)
		/// "(A;;0x3;;;S-1-5-33)"              write restricted service   (read, write)
		/// "(A;;0x1;;;S-1-5-32-573)";         event log readers          (read)
		/// </summary>
		Application,
		/// <summary>
		/// The default permissions for System are (shown using SDDL):
		/// "O:BAG:SYD:"
		/// "(A;;0xf0007;;;SY)"                local system             (read, write, clear)
		/// "(A;;0x7;;;BA)"                    built-in admins          (read, write, clear)
		/// "(A;;0x3;;;BO)"                    backup operators         (read, write)
		/// "(A;;0x5;;;SO)"                    server operators         (read, clear)
		/// "(A;;0x1;;;IU)"                    INTERACTIVE LOGON        (read)
		/// "(A;;0x3;;;SU)"                    SERVICES LOGON           (read, write)
		/// "(A;;0x1;;;S-1-5-3)"               BATCH LOGON              (read)
		/// "(A;;0x2;;;S-1-5-33)"              write restricted service (write)
		/// "(A;;0x1;;;S-1-5-32-573)";         event log readers        (read)
		/// </summary>
		System,
		/// <summary>
		/// The default permissions for Custom isolation is the same as Application.
		/// </summary>
		Custom
	}
}
