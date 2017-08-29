using Microsoft.Build.Tasks;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Imba rectangle.
	/// </summary>
	public sealed class IMBARc : IMBAToolTask
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
			get { return "Rc.exe"; }
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

			cmdBuilder.AppendFileNameIfNotNull(Source);

			return cmdBuilder.ToString();
		}


		#endregion
	}
}
