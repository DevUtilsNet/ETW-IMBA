using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Imba tool task.
	/// </summary>
	public abstract class IMBAToolTask : ToolTask
	{
		/// <summary>
		/// Gets or sets source for the.
		/// </summary>
		/// <value>
		/// The source.
		/// </value>
		[Required]
		public virtual ITaskItem Source { get; set; }
	}
}
