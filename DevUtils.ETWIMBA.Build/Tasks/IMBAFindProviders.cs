using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DevUtils.ETWIMBA.Diagnostics.Counters;
using DevUtils.ETWIMBA.Tracing;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Find event sources.
	/// </summary>
	sealed public class IMBAFindProviders : AppDomainIsolatedTask2
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
		[Output]
		public ITaskItem[] Providers { get; set; }

		static ITaskItem CreateManifestItem(Type type)
		{
			var ret = new TaskItem(type.FullName);

			ret.SetMetadata("Type", IsEventsProvider(type) ? "Events" : "Counters");

			return ret;
		}

		static bool IsEventsProvider(Type eventSourceType)
		{
			return 
				typeof(EIEventSource).IsAssignableFrom(eventSourceType) && 
				eventSourceType.GetCustomAttribute<EIEventSourceAttribute>() != null;
		}

		static bool IsCountersProvider(Type eventSourceType)
		{
			return 
				//typeof(CounterSource).IsAssignableFrom(eventSourceType) && 
				eventSourceType.GetCustomAttribute<CounterSourceAttribute>() != null;
		}

		static bool IsProvider(Type type)
		{
			return IsEventsProvider(type) || IsCountersProvider(type);
		}

		#region Overrides of AppDomainIsolatedTask2

		/// <summary>
		/// When overridden in a derived class, executes the task.
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false.
		/// </returns>
		protected override bool Execute2()
		{
			var assemblyPath = InputAssembly.ItemSpec;

			try
			{
				var assembly = Assembly.UnsafeLoadFrom(assemblyPath);
				Providers = assembly.GetTypes().Where(IsProvider).Select(CreateManifestItem).ToArray();
				return true;
			}
			catch (ReflectionTypeLoadException e)
			{
				Log.LogErrorFromException(e);

				foreach (var item in e.LoaderExceptions ?? Enumerable.Empty<Exception>())
				{
					Log.LogErrorFromException(item);
				}
			}
			catch (IOException ex)
			{
				Log.LogErrorFromException(ex);
			}

			return false;
		}

		#endregion
	}
}
