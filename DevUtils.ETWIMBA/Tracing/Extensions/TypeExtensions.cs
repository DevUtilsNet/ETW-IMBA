using System;
using DevUtils.ETWIMBA.Extensions;

namespace DevUtils.ETWIMBA.Tracing.Extensions
{
	/// <summary> A type extensions. </summary>
	public static class TypeExtensions
	{
		/// <summary> Gets a unique identifier. </summary>
		/// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
		/// 																		 illegal values. </exception>
		/// <param name="eventSourceType" type="Type"> Type of the event source. </param>
		/// <returns> The unique identifier. </returns>
		public static Guid GetGuid(this Type eventSourceType)
		{
			var eventSourceAttribute = eventSourceType.GetCustomAttributeT<EIEventSourceAttribute>();
			if (eventSourceAttribute != null)
			{
				if (eventSourceAttribute.Guid != null)
				{
					Guid guid;
					if (Guid.TryParse(eventSourceAttribute.Guid, out guid))
					{
						return guid;
					}
				}
			}
			var name = eventSourceType.GetName();

			if (name == null)
			{
				// ReSharper disable LocalizableElement
				throw new ArgumentException("The name of the type is invalid.", "eventSourceType");
				// ReSharper restore LocalizableElement
			}
			var ret = name.ToUpperInvariant().GenerateGuid();
			return ret;
		}
		/// <summary> A Type extension method that gets a name. </summary>
		/// <param name="eventSourceType" type="this Type"> Type of the event source. </param>
		/// <returns> The name. </returns>
		public static string GetName(this Type eventSourceType)
		{
			var eventSourceAttribute = eventSourceType.GetCustomAttributeT<EIEventSourceAttribute>();
			if (eventSourceAttribute != null && eventSourceAttribute.Name != null)
			{
				return eventSourceAttribute.Name;
			}
			var ret = eventSourceType.FullName.Replace('.', '-');
			return ret;
		}
	}
}
