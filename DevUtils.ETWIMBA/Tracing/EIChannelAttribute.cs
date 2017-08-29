using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Defines a channel to which providers can log events.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EIChannelAttribute : Attribute
	{
		private bool? _enabled;
		/// <summary>
		/// The name of the channel. The name must be unique within the list of channels that the provider uses. 
		/// The convention for naming channels is to append the channel type to the provider's name. For example. 
		/// if the provider's name is Company-Product-Component and you are defining an operational channel, 
		/// the name would be Company-Product-Component/Operational.
		/// Channel names must be less that 255 characters and cannot contain the following characters: 
		/// '>', '&lt;', '&amp;', '"', '|', '\', ':', '`', '?', '*', or characters with codes less than 31.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		/// <summary>
		/// A Security Descriptor Definition Language (SDDL) access descriptor that controls access to the log file that backs the channel. 
		/// If the isolation attribute is set to Application or System, the access descriptor controls read access to the file 
		/// (the write permissions are ignored). If the isolation attribute is set to Custom, the access descriptor controls 
		/// write access to the channel and read access to the file.
		/// </summary>
		/// <value>
		/// The Security Descriptor Definition Language (SDDL).
		/// </value>
		public string Access { get; set; }

		/// <summary>
		/// Determines whether the channel is enabled. 
		/// The default is false (logging is disabled).
		/// Because Debug and Analytic channel types are high volume channels, you should enable the channel only when 
		/// investigating an issue with a component that writes to that channel; otherwise, the channel should remain disabled.
		/// Each time you enable a Debug and Analytic channel, the service clears the events from the channel.
		/// </summary>
		/// <value>
		/// Set to true to allow logging to the channel; otherwise, false.
		/// </value>
		public bool Enabled
		{
			get { return _enabled.HasValue && _enabled.Value; }
			set { _enabled = value; }
		}

		internal bool? ActualEnabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		/// The isolation value defines the default access permissions for the channel.
		/// </summary>
		/// <value>
		/// The isolation.
		/// The default isolation is Application.
		/// </value>
		public EIChannelIsolation Isolation { get; set; }

		/// <summary>
		/// The localized display name for the channel.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public EIChannelType Type { get; set; }
	}
}
