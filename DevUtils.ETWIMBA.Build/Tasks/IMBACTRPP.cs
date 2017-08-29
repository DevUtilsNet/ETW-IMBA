using System.IO;
using Microsoft.Build.Tasks;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// An imbactrpp.
	/// </summary>
	public sealed class IMBACTRPP : IMBAToolTask
	{
		#region Overrides of ToolTask

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
		/// Gets the name of the executable file to run.
		/// </summary>
		/// <returns>
		/// The name of the executable file to run.
		/// </returns>
		protected override string ToolName
		{
			get { return "CTRPP.exe"; }
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

			cmdBuilder.AppendSwitchIfNotNull("-rc ", Path.ChangeExtension(Source.ItemSpec, ".ctrpp.rc"));
			cmdBuilder.AppendFileNameIfNotNull(Source);

			var ret = cmdBuilder.ToString();
			return ret;
		}

		#endregion
	}
}
