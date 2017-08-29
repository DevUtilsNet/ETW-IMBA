using System;

namespace DevUtils.ETWIMBA.Extensions
{
	static class EnumExtensions
	{
		public static string GetStringValue(this Enum @enum)
		{
			var field = @enum.GetType().GetField(@enum.ToString());
			var attribute = field.GetCustomAttributeT<StringValueAttribute>();
			if (attribute != null)
			{
				var ret = attribute.Value;
				return ret;
			}
			else
			{
				var ret = @enum.ToString();
				return ret;
			}
		}
	}
}
