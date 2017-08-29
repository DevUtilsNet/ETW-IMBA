using System;
using System.Globalization;
using DevUtils.ETWIMBA.Diagnostics.Counters;
using DevUtils.ETWIMBA.Tests.Diagnostics.Counters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevUtils.ETWIMBA.Tests.Tracing
{
	[TestClass]
	public class EventSourceTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var source = new MyEIEventSource();

			source.Admin(DateTime.Now.ToString(CultureInfo.InvariantCulture));
			source.Operational(DateTime.Now.ToString(CultureInfo.InvariantCulture));
			source.Analytic(DateTime.Now.ToString(CultureInfo.InvariantCulture));
			source.Debug(DateTime.Now.ToString(CultureInfo.InvariantCulture));
		}
	}
}
