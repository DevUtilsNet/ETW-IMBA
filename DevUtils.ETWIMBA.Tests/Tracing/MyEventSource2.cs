using DevUtils.ETWIMBA.Tracing;

namespace DevUtils.ETWIMBA.Tests.Tracing
{
	//[EIEventSource(Guid = "{f861212a-7a16-542a-0116-5ead9eb9b31b}")]
	//sealed class MyEventSource2 : EIEventSource
	//{
	//	static class Channels
	//	{
	//		[EIChannelImport(Name = "Application")]
	//		public const int Application = (int)EIEventChannel.Application;

	//		//[Channel(Name = "DefUtils-IMBA-SampleProvider2/Diagnostic", Type = ChannelType.Analytic, Isolation = ChannelIsolation.Application)]
	//		//public const int Channel1 = 20;
	//	}

	//	[EIEventTrace(1, Message = "Bla bla", Channel = Channels.Application)]
	//	public void Event1()
	//	{
	//		WriteEvent(1);
	//	}

	//	[EIEventTrace(2, Message = "Bla bla blu", Channel = Channels.Application)]
	//	public void Event2WithParam(int param)
	//	{
	//		WriteEvent(2, param);
	//	}
	//}
}
