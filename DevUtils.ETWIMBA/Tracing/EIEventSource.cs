using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using DevUtils.ETWIMBA.Extensions;
using DevUtils.ETWIMBA.Tracing.Extensions;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Event source.
	/// </summary>
	public abstract class EIEventSource : IDisposable
	{
		private volatile EventDescriptor[] _eventData;
		private readonly bool _throwOnEventWriteErrors;
		private EIEventProvider _provider;

		/// <summary>
		/// Query if this object is enabled.
		/// </summary>
		/// <returns>
		/// true if enabled, false if not.
		/// </returns>
		public bool IsEnabled()
		{
			return _provider.IsEnabled();
		}

		/// <summary>
		/// Query if this object is enabled.
		/// </summary>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <returns>
		/// true if enabled, false if not.
		/// </returns>
		public bool IsEnabled(int eventId)
		{
			var evt = _eventData[eventId];
			return _provider.IsEnabled(evt.Level, evt.Keywords);
		}

		/// <summary>
		/// Specialised default constructor for use only by derived classes.
		/// </summary>
		protected EIEventSource() : this(false)
		{
		}

		/// <summary>
		/// Specialised default constructor for use only by derived classes.
		/// </summary>
		/// <param name="throwOnEventWriteErrors"> true to throw on event write errors. </param>
		protected EIEventSource(bool throwOnEventWriteErrors)
		{
			_eventData = CreateManifestAndDescriptors();

			_throwOnEventWriteErrors = throwOnEventWriteErrors;
			Initialize(GetType().GetGuid());
		}

		internal EIEventSource(Guid eventSourceGuid)
		{
			Initialize(eventSourceGuid);
		}

		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="args">		 A variable-length parameters list containing arguments. </param>
		protected unsafe void WriteEvent(int eventId, IEnumerable<int> args)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (args == null)
					{
						if (!_provider.WriteEvent(ref _eventData[eventId], 0, (IntPtr) 0) &&
						    _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
						return;
					}

					var arr = args.ToArray();
					var length = arr.Length;

					var ptr = stackalloc EventData[length];

					fixed (int* fixedArr = arr)
					{
						var offset1 = fixedArr;
						var offset2 = ptr;
						for (var i = 0; i < length; ++i)
						{
							offset2->Ptr = (ulong)(offset1++);
							offset2->Size = 4u;
							offset2->Reserved = 0u;
							++offset2;
						}

						if (!_provider.WriteEvent(ref _eventData[eventId], length, (IntPtr)ptr) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="args">		 A variable-length parameters list containing arguments. </param>
		protected unsafe void WriteEvent(int eventId, IEnumerable<long> args)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					var arr = args.ToArray();
					var length = arr.Length;

					var ptr = stackalloc EventData[length];

					fixed (long* fixedArr = arr)
					{
						var offset1 = fixedArr;
						var offset2 = ptr;
						for (var i = 0; i < length; ++i)
						{
							offset2->Ptr = (ulong)(offset1++);
							offset2->Size = 8u;
							offset2->Reserved = 0u;
							++offset2;
						}

						if (!_provider.WriteEvent(ref _eventData[eventId], length, (IntPtr)ptr) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = "";
					}
					fixed (char* ptr = arg1)
					{
						EventData* ptr2 = stackalloc EventData[1];
						ptr2->Ptr = (ulong) ptr;
						ptr2->Size = (uint)((arg1.Length + 1) * 2);
						ptr2->Reserved = 0u;
						if (!_provider.WriteEvent(ref _eventData[eventId], 1, (IntPtr)(ptr2)) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		/// <param name="arg2">		 The second argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1, string arg2)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = String.Empty;
					}
					if (arg2 == null)
					{
						arg2 = String.Empty;
					}
					fixed (char* ptr = arg1)
					{
						fixed (char* ptr2 = arg2)
						{
							EventData* ptr3 = stackalloc EventData[2];
							ptr3->Ptr = (ulong) ptr;
							ptr3->Size = (uint)((arg1.Length + 1) * 2);
							ptr3->Reserved = 0u;
							ptr3[1].Ptr = (ulong) ptr2;
							ptr3[1].Size = (uint)((arg2.Length + 1) * 2);
							ptr3[1].Reserved = 0u;
							if (!_provider.WriteEvent(ref _eventData[eventId], 2, (IntPtr)ptr3) && _throwOnEventWriteErrors)
							{
								throw new EventSourceException();
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		/// <param name="arg2">		 The second argument. </param>
		/// <param name="arg3">		 The third argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1, string arg2, string arg3)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = String.Empty;
					}
					if (arg2 == null)
					{
						arg2 = String.Empty;
					}
					if (arg3 == null)
					{
						arg3 = String.Empty;
					}
					fixed (char* ptr = arg1)
					{
						fixed (char* ptr2 = arg2)
						{
							fixed (char* ptr3 = arg3)
							{
								EventData* ptr4 = stackalloc EventData[3];
								ptr4->Ptr = (ulong) ptr;
								ptr4->Size = (uint)((arg1.Length + 1) * 2);
								ptr4->Reserved = 0u;
								ptr4[1].Ptr = (ulong) ptr2;
								ptr4[1].Size = (uint)((arg2.Length + 1) * 2);
								ptr4[1].Reserved = 0u;
								ptr4[2].Ptr = (ulong) ptr3;
								ptr4[2].Size = (uint)((arg3.Length + 1) * 2);
								ptr4[2].Reserved = 0u;
								if (!_provider.WriteEvent(ref _eventData[eventId], 3, (IntPtr)ptr4) && _throwOnEventWriteErrors)
								{
									throw new EventSourceException();
								}
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		/// <param name="arg2">		 The second argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1, int arg2)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = String.Empty;
					}
					fixed (char* ptr = arg1)
					{
						EventData* ptr2 = stackalloc EventData[2];
						ptr2->Ptr = (ulong) ptr;
						ptr2->Size = (uint)((arg1.Length + 1) * 2);
						ptr2->Reserved = 0u;
						ptr2[1].Ptr = (ulong) (&arg2);
						ptr2[1].Size = 4u;
						ptr2[1].Reserved = 0u;
						if (!_provider.WriteEvent(ref _eventData[eventId], 2, (IntPtr)(ptr2)) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		/// <param name="arg2">		 The second argument. </param>
		/// <param name="arg3">		 The third argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1, int arg2, int arg3)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = String.Empty;
					}
					fixed (char* ptr = arg1)
					{
						EventData* ptr2 = stackalloc EventData[3];
						ptr2->Ptr = (ulong) ptr;
						ptr2->Size = (uint)((arg1.Length + 1) * 2);
						ptr2->Reserved = 0u;
						ptr2[1].Ptr = (ulong) (&arg2);
						ptr2[1].Size = 4u;
						ptr2[1].Reserved = 0u;
						ptr2[2].Ptr = (ulong) (&arg3);
						ptr2[2].Size = 4u;
						ptr2[2].Reserved = 0u;
						if (!_provider.WriteEvent(ref _eventData[eventId], 3, (IntPtr)(ptr2)) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="arg1">		 The first argument. </param>
		/// <param name="arg2">		 The second argument. </param>
		protected unsafe void WriteEvent(int eventId, string arg1, long arg2)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (arg1 == null)
					{
						arg1 = String.Empty;
					}
					fixed (char* ptr = arg1)
					{
						EventData* ptr2 = stackalloc EventData[2];
						ptr2->Ptr = (ulong) ptr;
						ptr2->Size = (uint)((arg1.Length + 1) * 2);
						ptr2->Reserved = 0u;
						ptr2[1].Ptr = (ulong) (&arg2);
						ptr2[1].Size = 8u;
						ptr2[1].Reserved = 0u;
						if (!_provider.WriteEvent(ref _eventData[eventId], 2, (IntPtr)(ptr2)) && _throwOnEventWriteErrors)
						{
							throw new EventSourceException();
						}
					}
				}
			}
		}

		/// <summary>
		/// Writes an event core.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId">			  Identifier for the event. </param>
		/// <param name="eventDataCount"> Number of event data. </param>
		/// <param name="data">					  [in,out] If non-null, the data. </param>
		protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventData* data)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId))
				{
					if (!_provider.WriteEvent(ref _eventData[eventId], eventDataCount, (IntPtr)data) && _throwOnEventWriteErrors)
					{
						throw new EventSourceException();
					}
				}
			}
		}

		/// <summary>
		/// Writes an event.
		/// </summary>
		/// <exception cref="EventSourceException"> Thrown when an Event Source error condition occurs. </exception>
		/// <param name="eventId"> Identifier for the event. </param>
		/// <param name="args">		 A variable-length parameters list containing arguments. </param>
		protected void WriteEvent(int eventId, params object[] args)
		{
			if (_eventData != null)
			{
				if (IsEnabled(eventId) && !_provider.WriteEvent(ref _eventData[eventId], args) && _throwOnEventWriteErrors)
				{
					throw new EventSourceException();
				}
			}
		}
		
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		/// <param name="disposing"> true to release both managed and unmanaged resources; false to
		/// 												 release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _provider != null)
			{
				_provider.Dispose();
				_provider = null;
			}
		}
		
		/// <summary>
		/// Finaliser.
		/// </summary>
		~EIEventSource()
		{
			Dispose(false);
		}

		private void Initialize(Guid eventSourceGuid)
		{
			if (eventSourceGuid == Guid.Empty)
			{
				throw new ArgumentException(String.Format("The Guid of an EventSource must be non zero."));
			}

			_provider = new EIEventProvider();

			try
			{
				_provider.Register(eventSourceGuid);
			}
			catch (ArgumentException)
			{
				_provider = null;
			}
		}

		private static IEnumerable<EIEventTraceAttribute> GetEventTraceAttributes(IReflect eventSourceType)
		{
			return eventSourceType.ExtractEventTraceAttributes().Select(s => s.Item2);
		}

		private EventDescriptor[] CreateManifestAndDescriptors()
		{
			var arr = GetEventTraceAttributes(GetType()).ToArray();

			var ret = new EventDescriptor[arr.Max(m => m.EventId) + 1];

			foreach (var item in arr)
			{
				ret[item.EventId] = new EventDescriptor(item.EventId, item.Version, (byte)item.Channel, item.Level, item.Opcode, item.Task, (long)item.Keywords);
			}

			return ret;
		}
	}
}
