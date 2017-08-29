using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection;
using DevUtils.ETWIMBA.Extensions;

namespace DevUtils.ETWIMBA.Tracing.Extensions
{
	static class ReflectExtensions
	{
		public static IEnumerable<Tuple<MethodInfo, EIEventTraceAttribute>> ExtractEventTraceAttributes(this IReflect eventSourceType)
		{
			var methods = eventSourceType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var item in methods)
			{
				var eventAttribute = item.GetCustomAttributeT<EIEventTraceAttribute>();
				if (eventAttribute == null)
				{
					var ea = item.GetCustomAttributeT<EventAttribute>();
					if (ea == null)
					{
						continue;
					}

					eventAttribute = new EIEventTraceAttribute(ea.EventId)
					{
						Keywords = (ulong)(long)ea.Keywords,
						Level = (byte)ea.Level,
						Message = ea.Message,
						Opcode = (byte)ea.Opcode,
						Task = (ushort)ea.Task,
						Version = ea.Version
					};
				}

				if (item.ReturnType != typeof(void))
				{
					throw new EIValidateException("Event attribute does not return 'void'.");
				}

				if (eventAttribute.EventId <= 0)
				{
					throw new EIValidateException("Event IDs must be positive integers.");
				}

				yield return Tuple.Create(item, eventAttribute);
			}
		}
	}
}