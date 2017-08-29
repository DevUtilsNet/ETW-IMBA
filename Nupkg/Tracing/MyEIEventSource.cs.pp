using DevUtils.ETWIMBA.Tracing;

namespace $rootnamespace$.Tracing
{
	/// <summary>
	/// My ei event source.
	/// </summary>
	[EIEventSource]
	public sealed class MyEIEventSource : EIEventSource
	{
		enum Channels
		{
			[EIChannel]
			MyChannel = 16,

			[EIChannel]
			[EIChannelLogging(Retention = true)]
			MyChannel2 = 17,

			[EIChannelImport]
			Application = (int)EIEventChannel.Application
		}

		enum Levels
		{
			MyLevel = 16
		}

		enum Opcodes
		{
			MyOpcode = 11
		}

		enum Tasks
		{
			MyTask = 1
		}

		enum Keywords : ulong
		{
			MyKeyword1 = 0x100,
			MyKeyword2 = 0x200
		}
		/// <summary>
		/// Event 1.
		/// </summary>
		/// <param name="param"> The parameter. </param>
		[EIEventTrace(1, Message = "Event1 sample param {0}", Channel = (int)Channels.MyChannel)]
		public void Event1(string param)
		{
			WriteEvent(1, param);
		}
		/// <summary>
		/// Event 2.
		/// </summary>
		[EIEventTrace(2,
			Task = (ushort)Tasks.MyTask,
			Level = (byte)Levels.MyLevel,
			Opcode = (byte)Opcodes.MyOpcode,
			Channel = (int)Channels.MyChannel2,
			Keywords = ((ulong)Keywords.MyKeyword1) | ((ulong)Keywords.MyKeyword2))]
		public void Event2()
		{
			WriteEvent(2);
		}
		/// <summary>
		/// Event 3.
		/// </summary>
		[EIEventTrace(3, Channel = (int)Channels.Application, Message = "Event3")]
		public void Event3()
		{
			WriteEvent(3);
		}
	}
}
