using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// An imba unlodctr.
	/// </summary>
	public sealed class IMBAUnlodctr : ToolTask
	{
		/// <summary>
		/// Gets or sets the manifest.
		/// </summary>
		/// <value>
		/// The manifest.
		/// </value>
		public ITaskItem Manifest { get; set; }

		#region Overrides of ToolTask

		/// <summary>
		/// Gets the name of the executable file to run.
		/// </summary>
		/// <returns>
		/// The name of the executable file to run.
		/// </returns>
		protected override string ToolName
		{
			get { return "Unlodctr.exe"; }
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

			cmdBuilder.AppendSwitchIfNotNull("/m:", Manifest);

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
