using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace DevUtils.ETWIMBA.Tracing
{
	enum ControllerCommand
	{
		Disable = -3,
		Enable = -2,
		SendManifest = -1,
		Update = 0,
	}

	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal class EIEventProvider : IDisposable
	{
		private enum ActivityControl : uint
		{
			EVENT_ACTIVITY_CTRL_GET_ID = 1u,
			EVENT_ACTIVITY_CTRL_SET_ID,
			EVENT_ACTIVITY_CTRL_CREATE_ID,
			EVENT_ACTIVITY_CTRL_GET_SET_ID,
			EVENT_ACTIVITY_CTRL_CREATE_SET_ID
		}

		public enum WriteEventErrorCode
		{
			NoError,
			NoFreeBuffers,
			EventTooBig
		}

		private NativeMethods.EtwEnableCallback _etwCallback;
		private long _regHandle;
		private byte _level;
		private long _anyKeywordMask;
		private long _allKeywordMask;
		private int _enabled;
		private Guid _providerId;
		private int _disposed;
		[ThreadStatic]
		private static WriteEventErrorCode _returnCode;
		private const int BasicTypeAllocationBufferSize = 16;
		private const int EtwMaxMumberArguments = 32;
		private const int EtwAPIMaxStringCount = 8;
		private const int MaxEventDataDescriptors = 128;
		private const int TraceEventMaximumSize = 65482;
		private const int TraceEventMaximumStringSize = 32724;
		internal const int ERROR_ARITHMETIC_OVERFLOW = 534;
		internal const int ERROR_NOT_ENOUGH_MEMORY = 8;
		internal const int ERROR_MORE_DATA = 234;

		protected EventLevel Level
		{
			get
			{
				return (EventLevel)_level;
			}
			set
			{
				_level = (byte)value;
			}
		}
		protected EventKeywords MatchAnyKeyword
		{

			get
			{
				return (EventKeywords)_anyKeywordMask;
			}

			set
			{
				_anyKeywordMask = (long)value;
			}
		}
		protected EventKeywords MatchAllKeyword
		{

			get
			{
				return (EventKeywords)_allKeywordMask;
			}

			set
			{
				_allKeywordMask = (long)value;
			}
		}

		[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
		protected EIEventProvider(Guid providerGuid)
		{
			_providerId = providerGuid;
			Register(providerGuid);
		}

		internal EIEventProvider()
		{
		}

		internal unsafe void Register(Guid providerGuid)
		{
			_providerId = providerGuid;
			_etwCallback = EtwEnableCallBack;
			var num = EventRegister(ref _providerId, _etwCallback);
			if (num != 0u)
			{
				throw new Win32Exception((int)num);
			}
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed == 1)
			{
				return;
			}
			if (Interlocked.Exchange(ref _disposed, 1) != 0)
			{
				return;
			}
			_enabled = 0;
			Deregister();
		}

		public virtual void Close()
		{
			Dispose();
		}
		~EIEventProvider()
		{
			Dispose(false);
		}
		protected virtual void OnControllerCommand(ControllerCommand command, IDictionary<string, string> arguments)
		{
		}
		public bool IsEnabled()
		{
			return _enabled != 0;
		}

		public bool IsEnabled(byte level, long keywords)
		{
			return _enabled != 0 && ((level <= _level || _level == 0) && (keywords == 0L || ((keywords & _anyKeywordMask) != 0L && (keywords & _allKeywordMask) == _allKeywordMask)));
		}

		public static WriteEventErrorCode GetLastWriteEventError()
		{
			return _returnCode;
		}

		[SecuritySafeCritical]
		public unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, IEnumerable<object> eventPayload)
		{
			var num1 = 0U;
			if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				var num2 = 0;
				if (eventPayload == null || eventPayload.Take(2).Count() != 2)
				{
					var str = (string)null;
					byte* dataBuffer = stackalloc byte[BasicTypeAllocationBufferSize];
					EventData eventData;
					eventData.Size = 0U;
					if (eventPayload != null && eventPayload.Any())
					{
						str = EncodeObject(eventPayload.First(), &eventData, dataBuffer);
						num2 = 1;
					}
					if (eventData.Size > TraceEventMaximumSize)
					{
						_returnCode = WriteEventErrorCode.EventTooBig;
						return false;
					}
					if (str != null)
					{
						fixed (char* chPtr = str)
						{
							eventData.Ptr = (ulong)chPtr;
							num1 = EventWrite(ref eventDescriptor, (uint)num2, &eventData);
						}
					}
					else
						num1 = num2 != 0 ? EventWrite(ref eventDescriptor, (uint)num2, &eventData) : EventWrite(ref eventDescriptor, 0U, null);
				}
				else
				{
					var length = eventPayload.Count();
					if (length > EtwMaxMumberArguments)
					{
						throw new ArgumentOutOfRangeException("eventPayload", String.Format("The total number of parameters must not exceed {0}.", EtwMaxMumberArguments));
					}
					var count = 0U;
					var index1 = 0;
					var index2 = 0;
					var numArray = new int[Math.Min(EtwAPIMaxStringCount, length)];
					var strArray = new string[Math.Min(EtwAPIMaxStringCount, length)];
					var userData = stackalloc EventData[length];
					var dataDescriptor = userData;
					byte* numPtr = stackalloc byte[BasicTypeAllocationBufferSize * length];
					var dataBuffer = numPtr;
					foreach (var item in eventPayload.Where(w => w != null))
					{
						var str = EncodeObject(item, dataDescriptor, dataBuffer);
						dataBuffer += BasicTypeAllocationBufferSize;
						count += dataDescriptor->Size;
						if (str != null)
						{
							if (index1 < EtwAPIMaxStringCount)
							{
								strArray[index1] = str;
								numArray[index1] = index2;
								++index1;
							}
							else
							{
								throw new ArgumentOutOfRangeException("eventPayload", String.Format("The number of String parameters must not exceed {0}.", EtwAPIMaxStringCount));
							}
						}
						++dataDescriptor;
						++index2;
					}
					if (count > TraceEventMaximumSize)
					{
						_returnCode = WriteEventErrorCode.EventTooBig;
						return false;
					}
					fixed (char* chPtr1 = strArray[0])
					fixed (char* chPtr2 = strArray[1])
					fixed (char* chPtr3 = strArray[2])
					fixed (char* chPtr4 = strArray[3])
					fixed (char* chPtr5 = strArray[4])
					fixed (char* chPtr6 = strArray[5])
					fixed (char* chPtr7 = strArray[6])
					fixed (char* chPtr8 = strArray[7])
					{
						var eventDataPtr = userData;
						if (strArray[0] != null)
							eventDataPtr[numArray[0]].Ptr = (ulong)chPtr1;
						if (strArray[1] != null)
							eventDataPtr[numArray[1]].Ptr = (ulong)chPtr2;
						if (strArray[2] != null)
							eventDataPtr[numArray[2]].Ptr = (ulong)chPtr3;
						if (strArray[3] != null)
							eventDataPtr[numArray[3]].Ptr = (ulong)chPtr4;
						if (strArray[4] != null)
							eventDataPtr[numArray[4]].Ptr = (ulong)chPtr5;
						if (strArray[5] != null)
							eventDataPtr[numArray[5]].Ptr = (ulong)chPtr6;
						if (strArray[6] != null)
							eventDataPtr[numArray[6]].Ptr = (ulong)chPtr7;
						if (strArray[7] != null)
							eventDataPtr[numArray[7]].Ptr = (ulong)chPtr8;
						num1 = EventWrite(ref eventDescriptor, (uint)length, userData);
					}
				}
			}
			if ((int)num1 == 0)
				return true;
			SetLastError((int)num1);
			return false;
		}

		public unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, string data)
		{
			var num = 0u;
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				if (data.Length > TraceEventMaximumStringSize)
				{
					_returnCode = WriteEventErrorCode.EventTooBig;
					return false;
				}
				EventData eventData;
				eventData.Size = (uint)((data.Length + 1) * 2);
				eventData.Reserved = 0u;
				fixed (char* ptr = data)
				{
					eventData.Ptr = (ulong)ptr;
					num = EventWrite(ref eventDescriptor, 1u, &eventData);
				}
			}
			if (num != 0u)
			{
				SetLastError((int)num);
				return false;
			}
			return true;
		}

		protected internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, int dataCount, IntPtr data)
		{
			var num = EventWrite(ref eventDescriptor, (uint)dataCount, (EventData*)((void*)data));
			if (num != 0u)
			{
				SetLastError((int)num);
				return false;
			}
			return true;
		}

		private void Deregister()
		{
			if (_regHandle != 0L)
			{
				EventUnregister();
				_regHandle = 0L;
			}
		}

		private unsafe void EtwEnableCallBack([In] ref Guid sourceId, [In] int isEnabled, [In] byte setLevel, [In] long anyKeyword, [In] long allKeyword, [In] NativeMethods.EventFilterDescriptor* filterData, [In] void* callbackContext)
		{
			_enabled = isEnabled;
			_level = setLevel;
			_anyKeywordMask = anyKeyword;
			_allKeywordMask = allKeyword;
			ControllerCommand command;
			IDictionary<string, string> dictionary = null;
			byte[] array;
			int i;
			if (GetDataFromController(filterData, out command, out array, out i))
			{
				dictionary = new Dictionary<string, string>(4);
				while (i < array.Length)
				{
					var num = FindNull(array, i);
					var num2 = num + 1;
					var num3 = FindNull(array, num2);
					if (num3 < array.Length)
					{
						var @string = Encoding.UTF8.GetString(array, i, num - i);
						var string2 = Encoding.UTF8.GetString(array, num2, num3 - num2);
						dictionary[@string] = string2;
					}
					i = num3 + 1;
				}
			}
			try
			{
				OnControllerCommand(command, dictionary);
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch
			// ReSharper restore EmptyGeneralCatchClause
			{
			}
		}
		private static int FindNull(byte[] buffer, int idx)
		{
			while (idx < buffer.Length && buffer[idx] != 0)
			{
				idx++;
			}
			return idx;
		}

		private unsafe bool GetDataFromController(NativeMethods.EventFilterDescriptor* filterData, out ControllerCommand command, out byte[] data, out int dataStart)
		{
			data = null;
			if (filterData != null)
			{
				if (filterData->Ptr != 0L && 0 < filterData->Size && filterData->Size <= 1024)
				{
					data = new byte[filterData->Size];
					Marshal.Copy((IntPtr)filterData->Ptr, data, 0, data.Length);
				}
				command = (ControllerCommand)filterData->Type;
				dataStart = 0;
				return true;
			}
			var text = "\\Microsoft\\Windows\\CurrentVersion\\Winevt\\Publishers\\{" + _providerId + "}";
			if (Marshal.SizeOf(typeof(IntPtr)) == 8)
			{
				text = "HKEY_LOCAL_MACHINE\\Software\\Wow6432Node" + text;
			}
			else
			{
				text = "HKEY_LOCAL_MACHINE\\Software" + text;
			}
			data = (Registry.GetValue(text, "ControllerData", null) as byte[]);
			if (data != null && data.Length >= 4)
			{
				command = (ControllerCommand)(data[3] << 8 + data[2] << 8 + data[1] << 8 + data[0]);
				dataStart = 4;
				return true;
			}
			dataStart = 0;
			command = ControllerCommand.Update;
			return false;
		}
		private static void SetLastError(int error)
		{
			if (error == ERROR_NOT_ENOUGH_MEMORY)
			{
				_returnCode = WriteEventErrorCode.NoFreeBuffers;
				return;
			}
			if (error != ERROR_MORE_DATA && error != ERROR_ARITHMETIC_OVERFLOW)
			{
				return;
			}
			_returnCode = WriteEventErrorCode.EventTooBig;
		}

		/// <summary>
		/// Encode object.
		/// </summary>
		/// <param name="data">					  [in,out] The data. </param>
		/// <param name="dataDescriptor"> [in,out] If non-null, information describing the data. </param>
		/// <param name="dataBuffer">		  [in,out] If non-null, buffer for data data. </param>
		/// <returns>
		/// .
		/// </returns>
		private unsafe static string EncodeObject(object data, EventData* dataDescriptor, byte* dataBuffer)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			dataDescriptor->Reserved = 0u;

			if (data is IntPtr)
			{
				dataDescriptor->Size = (uint)sizeof(IntPtr);
				*(IntPtr*)dataBuffer = (IntPtr)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is Enum)
			{
				var underlyingType = Enum.GetUnderlyingType(data.GetType());
				if (underlyingType == typeof(int))
				{
					data = ((IConvertible)data).ToInt32(null);
				}
				else if (underlyingType == typeof(uint))
				{
					data = ((IConvertible)data).ToUInt32(null);
				}
				else if (underlyingType == typeof(long))
				{
					data = ((IConvertible)data).ToInt64(null);
				}
			}
			if (data is int)
			{
				dataDescriptor->Size = 4u;
				*(int*)dataBuffer = (int)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is long)
			{
				dataDescriptor->Size = 8u;
				*(long*)dataBuffer = (long)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is uint)
			{
				dataDescriptor->Size = 4u;
				*(int*)dataBuffer = (int)((uint)data);
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is ulong)
			{
				dataDescriptor->Size = 8u;
				*(long*)dataBuffer = (long)((ulong)data);
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is char)
			{
				dataDescriptor->Size = 2u;
				*(short*)dataBuffer = (short)((char)data);
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is byte)
			{
				dataDescriptor->Size = 1u;
				*dataBuffer = (byte)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is short)
			{
				dataDescriptor->Size = 2u;
				*(short*)dataBuffer = (short)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is sbyte)
			{
				dataDescriptor->Size = 1u;
				*dataBuffer = (byte)((sbyte)data);
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is ushort)
			{
				dataDescriptor->Size = 2u;
				*(short*)dataBuffer = (short)((ushort)data);
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is float)
			{
				dataDescriptor->Size = 4u;
				*(float*)dataBuffer = (float)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is double)
			{
				dataDescriptor->Size = 8u;
				*(double*)dataBuffer = (double)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is bool)
			{
				dataDescriptor->Size = 4u;
				*dataBuffer = (bool)data ? (byte)1 : (byte)0;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is Guid)
			{
				dataDescriptor->Size = (uint)sizeof(Guid);
				*(Guid*)dataBuffer = (Guid)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}
			if (data is decimal)
			{
				dataDescriptor->Size = 16u;
				*(decimal*)dataBuffer = (decimal)data;
				dataDescriptor->Ptr = (ulong)dataBuffer;
				return null;
			}

			var text = data.ToString();

			dataDescriptor->Size = (uint)((text.Length + 1) * 2);

			return text;
		}

		private unsafe uint EventRegister(ref Guid providerId, NativeMethods.EtwEnableCallback enableCallback)
		{
			_providerId = providerId;
			_etwCallback = enableCallback;
			return NativeMethods.EventRegister(ref providerId, enableCallback, null, ref _regHandle);
		}

		private void EventUnregister()
		{
			NativeMethods.EventUnregister(_regHandle);
			_regHandle = 0L;
		}

		private unsafe uint EventWrite(ref EventDescriptor eventDescriptor, uint userDataCount, EventData* userData)
		{
			return NativeMethods.EventWrite(_regHandle, ref eventDescriptor, userDataCount, userData);
		}
	}
}
