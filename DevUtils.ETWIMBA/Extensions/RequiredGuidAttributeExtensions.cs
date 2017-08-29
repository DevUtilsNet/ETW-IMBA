using System;

namespace DevUtils.ETWIMBA.Extensions
{
	static class RequiredGuidAttributeExtensions
	{
		public static Guid GetGuid(this IRequiredGuidAttribute attribute, Type linkedType)
		{
			if (attribute.Guid != null)
			{
				Guid guid;
				if (Guid.TryParse(attribute.Guid, out guid))
				{
					return guid;
				}
			}

			var name = attribute.GetName(linkedType);

			var ret = name.ToUpperInvariant().GenerateGuid();
			return ret;
		}
	}
}