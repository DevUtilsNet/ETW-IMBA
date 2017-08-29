using DevUtils.ETWIMBA.Tracing;

namespace DevUtils.ETWIMBA.Sample.Tracing
{
	[EIEventSource]
	public class MyEIEventSource : EIEventSource
	{
		enum Channels
		{
			[EIChannel(Type = EIChannelType.Admin, Enabled = true)]
			Admin = 16,

			[EIChannel(Type = EIChannelType.Operational, Enabled = true)]
			Operational = 17,

			[EIChannel(Type = EIChannelType.Analytic, Enabled = true)]
			Analytic = 18,

			[EIChannel(Type = EIChannelType.Debug, Enabled = true)]
			Debug = 19,
		}

		[EIEventTrace(1, Message = "Admin {0}", Channel = (int)Channels.Admin, Level = (byte)EIEventLevel.Informational)]
		public void Admin(string param)
		{
			WriteEvent(1, param);
		}

		[EIEventTrace(2, Message = "Operational {0}", Channel = (int)Channels.Operational, Level = (byte)EIEventLevel.Informational)]
		public void Operational(string param)
		{
			WriteEvent(2, param);
		}

		[EIEventTrace(3, Message = "Analytic {0}", Channel = (int)Channels.Analytic, Level = (byte)EIEventLevel.Informational)]
		public void Analytic(string param)
		{
			WriteEvent(3, param);
		}

		[EIEventTrace(4, Message = "Debug {0}", Channel = (int)Channels.Debug, Level = (byte)EIEventLevel.Informational)]
		public void Debug(string param)
		{
			WriteEvent(4, param);
		}
	}
}
