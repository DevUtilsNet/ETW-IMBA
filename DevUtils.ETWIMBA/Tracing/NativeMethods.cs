using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DevUtils.ETWIMBA.Tracing
{
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	struct EventDescriptor
	{
		[FieldOffset(0)]
		private readonly ushort _id;
		[FieldOffset(2)]
		private readonly byte _version;
		[FieldOffset(3)]
		private readonly byte _channel;
		[FieldOffset(4)]
		private readonly byte _level;
		[FieldOffset(5)]
		private readonly byte _opcode;
		[FieldOffset(6)]
		private readonly ushort _task;
		[FieldOffset(8)]
		private readonly long _keywords;

		public int EventId
		{
			get
			{
				return _id;
			}
		}
		public byte Version
		{
			
			get
			{
				return _version;
			}
		}
		public byte Channel
		{
			
			get
			{
				return _channel;
			}
		}
		public byte Level
		{
			
			get
			{
				return _level;
			}
		}
		public byte Opcode
		{
			
			get
			{
				return _opcode;
			}
		}
		public int Task
		{
			
			get
			{
				return _task;
			}
		}
		public long Keywords
		{
			
			get
			{
				return _keywords;
			}
		}
		public EventDescriptor(int id, byte version, byte channel, byte level, byte opcode, int task, long keywords)
		{
			if (id < 0)
			{
				throw new ArgumentOutOfRangeException("id", "Non-negative number required.");
			}

			if (id > 65535)
			{
				throw new ArgumentOutOfRangeException("id", String.Format("The ID parameter must be in the range {0} through {1}.", 1, 65535));
			}

			_id = (ushort)id;
			_version = version;
			_channel = channel;
			_level = level;
			_opcode = opcode;
			_keywords = keywords;
			if (id < 0)
			{
				throw new ArgumentOutOfRangeException("task", "Non-negative number required.");
			}
			if (id > 65535)
			{
				throw new ArgumentOutOfRangeException("task", String.Format("The ID parameter must be in the range {0} through {1}.", 1, 65535));
			}
			_task = (ushort)task;
		}
		public override bool Equals(object obj)
		{
			return obj is EventDescriptor && Equals((EventDescriptor)obj);
		}
		public override int GetHashCode()
		{
			return _id ^ _version ^ _channel ^ _level ^ _opcode ^ _task ^ (int)_keywords;
		}
		public bool Equals(EventDescriptor other)
		{
			return _id == other._id && _version == other._version && _channel == other._channel && _level == other._level && _opcode == other._opcode && _task == other._task && _keywords == other._keywords;
		}
		public static bool operator ==(EventDescriptor event1, EventDescriptor event2)
		{
			return event1.Equals(event2);
		}
		public static bool operator !=(EventDescriptor event1, EventDescriptor event2)
		{
			return !event1.Equals(event2);
		}
	}

	/// <summary>
	/// Event data.
	/// </summary>
	public struct EventData
	{
		internal ulong Ptr;
		internal uint Size;
		internal uint Reserved;
	}

	[SecurityCritical, SuppressUnmanagedCodeSecurity]
	static class NativeMethods
	{
		public unsafe delegate void EtwEnableCallback([In] ref Guid sourceId, [In] int isEnabled, [In] byte level, [In] long matchAnyKeywords, [In] long matchAllKeywords, [In] EventFilterDescriptor* filterData, [In] void* callbackContext);

		#pragma warning disable 0649
		public struct EventFilterDescriptor
		{
			public long Ptr;
			public int Size;
			public int Type;
		}

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public unsafe static extern uint EventRegister([In] ref Guid providerId, [In] EtwEnableCallback enableCallback, [In] void* callbackContext, [In] [Out] ref long registrationHandle);
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern uint EventUnregister([In] long registrationHandle);
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public unsafe static extern uint EventWrite([In] long registrationHandle, [In] ref EventDescriptor eventDescriptor, [In] uint userDataCount, [In] EventData* userData);
	}
}
