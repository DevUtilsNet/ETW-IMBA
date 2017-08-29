using System.IO;
using System.Reflection;

namespace DevUtils.ETWIMBA.Reflection.Extensions
{
	/// <summary> An assembly extensions. </summary>
	public static class AssemblyExtensions
	{
		/// <summary> An Assembly extension method that gets file name. </summary>
		/// <param name="assembly" type="this Assembly"> The assembly to act on. </param>
		/// <returns> The file name. </returns>
		public static string GetManifestFileName(this Assembly assembly)
		{
			var ret = assembly.ManifestModule.Name;
			return ret;
		}
	}
}
