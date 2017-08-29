using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DevUtils.ETWIMBA.Diagnostics.Counters;
using DevUtils.ETWIMBA.Reflection.Extensions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevUtils.ETWIMBA.Tests.Diagnostics.Counters
{
	[TestClass]
	public sealed class CounterSourceTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			//var set = CounterSource.CreateCounterSet<MyCounterSource.MyLogicalDiskSet>();

			//var setInstance = set.CreateCounterSetInstance("123");

			//var counter = setInstance.GetCounter(MyCounterSource.MyCounterSet.One);

			//MyCounterSource();

			var matadata = new Dictionary<string, string>();

			matadata["Type"] = "Counters";
			matadata["FusionName"] = typeof (CounterSetAttribute).Assembly.FullName;

			//var task = new IMBAGenerateManifest
			//	{
			//		InputAssembly = new TaskItem(Assembly.GetExecutingAssembly().CodeBase),
			//		Providers = new ITaskItem[] { new TaskItem(typeof (MyCounterSource).FullName, matadata) },
			//		References = new ITaskItem[] { new TaskItem(typeof(CounterSetAttribute).Assembly.CodeBase, matadata) },
			//		ManifestFile = new TaskItem(Path.ChangeExtension(typeof(MyCounterSource).Assembly.GetManifestFileName(), ".IM.xml"))
			//	};

			//task.Execute();
		}
	}
}
