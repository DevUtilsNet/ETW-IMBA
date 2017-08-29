using System;
using System.Reflection;

namespace DevUtils.ETWIMBA.Extensions
{
	static class RequiredUriAttributeExtensions
	{
		public static string GetUri(this IRequiredUriAttribute attribute, Type linkedType)
		{
			var ret = string.IsNullOrWhiteSpace(attribute.Uri) ? linkedType.FullName : attribute.Uri;
			return ret;
		}

		public static string GetUri(this IRequiredUriAttribute attribute, FieldInfo linkedField)
		{
			var ret = string.IsNullOrWhiteSpace(attribute.Uri) ? linkedField.FieldType + "." + linkedField.Name : attribute.Uri;
			return ret;
		}
	}
}