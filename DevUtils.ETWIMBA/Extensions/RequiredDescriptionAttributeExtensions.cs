namespace DevUtils.ETWIMBA.Extensions
{
	static class RequiredDescriptionAttributeExtensions
	{
		public static string GetDescription(this IRequiredDescriptionAttribute attribute)
		{
			var ret = string.IsNullOrWhiteSpace(attribute.Description) ? "<Empty>" : attribute.Description;
			return ret;
		}
	}
}