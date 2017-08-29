using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Imba wevtutil.
	/// </summary>
	public sealed class IMBAWevtutil : IMBAToolTask
	{
		/// <summary>
		/// Gets or sets the resources file.
		/// </summary>
		/// <value>
		/// The resources file.
		/// </value>
		public ITaskItem ResourcesFile { get; set; }

		/// <summary>
		/// Gets or sets the messages file.
		/// </summary>
		/// <value>
		/// The messages file.
		/// </value>
		public ITaskItem MessagesFile { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether the uninstall.
		/// </summary>
		/// <value>
		/// true if uninstall, false if not.
		/// </value>
		public bool Uninstall { get; set; }

		#region Overrides of ToolTask

		/// <summary>
		/// Gets the name of the executable file to run.
		/// </summary>
		/// <returns>
		/// The name of the executable file to run.
		/// </returns>
		protected override string ToolName
		{
			get { return "wevtutil.exe"; }
		}

		/// <summary>
		/// Returns the fully qualified path to the executable file.
		/// </summary>
		/// <returns>
		/// The fully qualified path to the executable file.
		/// </returns>
		protected override string GenerateFullPathToTool()
		{
			return ToolName;
		}
		/// <summary>
		/// Returns a string value containing the command line arguments to pass directly to the executable
		/// file.
		/// </summary>
		/// <returns>
		/// A string value containing the command line arguments to pass directly to the executable file.
		/// </returns>
		protected override string GenerateCommandLineCommands()
		{
			var cmdBuilder = new CommandLineBuilderExtension();

			cmdBuilder.AppendSwitch(!Uninstall ? "im" : "um");

			cmdBuilder.AppendFileNameIfNotNull(Source);

			if (ResourcesFile != null)
			{
				cmdBuilder.AppendSwitchIfNotNull("/rf:", ResourcesFile.GetMetadata("FullPath"));
			}

			if (MessagesFile != null)
			{
				cmdBuilder.AppendSwitchIfNotNull("/mf:", MessagesFile.GetMetadata("FullPath"));
			}

			return cmdBuilder.ToString();
		}

		/// <summary>
		/// Handles execution errors raised by the executable file.
		/// </summary>
		/// <returns>
		/// true if the method runs successfully; otherwise, false.
		/// </returns>
		protected override bool HandleTaskExecutionErrors()
		{
			base.HandleTaskExecutionErrors();
			return true;
		}

		#endregion
	}
}
