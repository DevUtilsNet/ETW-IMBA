using System;

namespace DevUtils.ETWIMBA.Extensions
{
	static class RequiredNameAttributeExtensions
	{
		public static string GetName(this IRequiredNameAttribute attribute, Type linkedType)
		{
			if (attribute.Name != null)
			{
				return attribute.Name;
			}

			var ret = linkedType.FullName.Replace('.', '-');
			return ret;
		}
	}
}