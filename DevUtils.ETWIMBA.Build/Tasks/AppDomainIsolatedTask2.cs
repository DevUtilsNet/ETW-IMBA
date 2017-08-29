using System;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DevUtils.ETWIMBA.Build.Tasks
{
	/// <summary>
	/// Application domain isolated task 2.
	/// </summary>
	public abstract class AppDomainIsolatedTask2 : AppDomainIsolatedTask
	{
		private Tuple<string, string>[] _references;

		/// <summary>
		/// Gets or sets the items from which the compiler will import public type information.
		/// </summary>
		/// <returns>The items from which the compiler will import public type information.</returns>
		public ITaskItem[] References { get; set; }

		/// <summary>
		/// Runs the task.
		/// </summary>
		/// <returns>
		/// true if successful; otherwise, false.
		/// </returns>
		public override bool Execute()
		{
			_references = References.Select(Create).ToArray();

			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ReflectionOnlyAssemblyResolve;
			try
			{
				return Execute2();
			}
			finally 
			{
				AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ReflectionOnlyAssemblyResolve;
				AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
			}
		}

		private static Tuple<string, string> Create(ITaskItem ti)
		{
			var fn = ti.GetMetadata("FusionName");
			var ret = Tuple.Create(new AssemblyName(string.IsNullOrEmpty(fn) ? ti.GetMetadata("Filename") : fn).ToString(), ti.ItemSpec);
			return ret;
		}

		/// <summary>
		/// Executes the 2 operation.
		/// </summary>
		/// <returns>
		/// true if it succeeds, false if it fails.
		/// </returns>
		protected abstract bool Execute2();

		private string GetAssemblyFile(string assemblyName)
		{
			for (var i = 0; i < 2; ++i)
			{
				var reference = _references.FirstOrDefault(f => f.Item1.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

				if (reference != null)
				{
					return reference.Item2;
				}

				var an = new AssemblyName(assemblyName);
				assemblyName = an.Name;
			}

			return null;
		}

		Assembly AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var assemblyFile = GetAssemblyFile(args.Name);
			if (!string.IsNullOrEmpty(assemblyFile))
			{
				var ret = Assembly.UnsafeLoadFrom(assemblyFile);
				return ret;
			}
			return null;
		}

		Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		{
			var assemblyFile = GetAssemblyFile(args.Name);
			if (!string.IsNullOrEmpty(assemblyFile))
			{
				var ret = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
				return ret;
			}
			return null;
		}
	}
}
