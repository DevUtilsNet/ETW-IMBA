using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.Framework;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Imba generate manifests files.
	/// </summary>
	public sealed class IMBAGenerateManifest : AppDomainIsolatedTask2
	{
		/// <summary>
		/// Gets or sets the input assembly.
		/// </summary>
		/// <value>
		/// The input assembly.
		/// </value>
		[Required]
		public ITaskItem InputAssembly { get; set; }

		/// <summary>
		/// Gets or sets the event sources.
		/// </summary>
		/// <value>
		/// The event sources.
		/// </value>
		[Required]
		public ITaskItem[] Providers { get; set; }

		/// <summary>
		/// Gets or sets the event sources.
		/// </summary>
		/// <value>
		/// The event sources.
		/// </value>
		[Required]
		public ITaskItem ManifestFile { get; set; }

		#region Overrides of AppDomainIsolatedTask

		/// <summary>
		/// Runs the task.
		/// </summary>
		/// <returns>
		/// true if successful; otherwise, false.
		/// </returns>
		protected override bool Execute2()
		{
			var assemblyPath = InputAssembly.ItemSpec;

			try
			{
				var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

				var f = Providers.Select(s => new {s.ItemSpec,Metadata = s.GetMetadata("Type")} ).ToArray();

				using (var builder = new ManifestBuilder(
					Providers.Where(w => w.GetMetadata("Type") == "Events").Select(s => assembly.GetType(s.ItemSpec, true)),
					Providers.Where(w => w.GetMetadata("Type") == "Counters").Select(s => assembly.GetType(s.ItemSpec, true)),
					ManifestFile.ItemSpec))
				{
					builder.Build();
				}
				return true;
			}
			catch (Exception ex)
			{
				if (File.Exists(ManifestFile.ItemSpec))
				{
					File.Delete(ManifestFile.ItemSpec);
				}

				var sb = new StringBuilder(1000);
				while (ex != null)
				{
					if (sb.Length > 0)
					{
						sb.Append(" ");
					}
					sb.Append(ex.Message);

					ex = ex.InnerException;
				}

				Log.LogError(sb.ToString());
			}

			return false;
		}

		#endregion
	}
}
