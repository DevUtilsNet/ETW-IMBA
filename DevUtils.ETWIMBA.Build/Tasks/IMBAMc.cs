using System.IO;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Mc.
	/// </summary>
	public sealed class IMBAMc : IMBAToolTask
	{
		#region Overrides of ToolTask

		/// <summary>
		/// Gets the name of the executable file to run.
		/// </summary>
		/// <returns>
		/// The name of the executable file to run.
		/// </returns>
		protected override string ToolName
		{
			get { return "Mc.exe"; }
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

			var outPath = new TaskItem(Path.GetDirectoryName(Source.ItemSpec));

			cmdBuilder.AppendSwitchIfNotNull("-r ", outPath);
			cmdBuilder.AppendSwitchIfNotNull("-h ", outPath);
			cmdBuilder.AppendSwitchIfNotNull("-z ", Source.GetMetadata("FileName") + ".mc");
			cmdBuilder.AppendSwitch("-b ");
			cmdBuilder.AppendFileNameIfNotNull(Source);

			var ret = cmdBuilder.ToString();
			return ret;
		}

		#endregion
	}
}
