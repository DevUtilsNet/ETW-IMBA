using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Imba link.
	/// </summary>
	public sealed class IMBALink : ToolTask
	{
		/// <summary>
		/// Gets or sets source for the.
		/// </summary>
		/// <value>
		/// The source.
		/// </value>
		[Required]
		public ITaskItem[] Files { get; set; }
		/// <summary>
		/// Gets or sets the out.
		/// </summary>
		/// <value>
		/// The out.
		/// </value>
		public ITaskItem Out { get; set; }

		#region Overrides of ToolTask

		/// <summary>
		/// Gets the name of the executable file to run.
		/// </summary>
		/// <returns>
		/// The name of the executable file to run.
		/// </returns>
		protected override string ToolName
		{
			get { return "Link.exe"; }
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

			cmdBuilder.AppendSwitch("/NOENTRY");
			cmdBuilder.AppendSwitch("/DLL");
			cmdBuilder.AppendSwitch("/MACHINE:X86");
			cmdBuilder.AppendSwitchIfNotNull("/OUT:", Out);

			cmdBuilder.AppendFileNamesIfNotNull(Files, " ");

			return cmdBuilder.ToString();
		}

		#endregion
	}
}
