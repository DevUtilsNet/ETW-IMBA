namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Values that represent CounterAttributeName. </summary>
	public enum CounterAttributeName
	{
		/// <summary> Retrieve the value of the counter by reference as opposed to by value. </summary>
		[StringValue("reference")]
		Reference,

		/// <summary> Do not display the counter value. 
		/// Typically, you use this attribute if the counter's data is used as input for calculating another counter's value. </summary>
		[StringValue("noDisplay")]
		NoDisplay,

		/// <summary> Consumer or monitoring applications should not use digit separators when displaying counter values. </summary>
		[StringValue("noDigitGrouping")]
		NoDigitGrouping,

		/// <summary> Consumer or monitoring applications should display the counter value as a hexadecimal, instead of the default integer value. </summary>
		[StringValue("displayAsHex")]
		DisplayAsHex,

		/// <summary> Consumer or monitoring applications should display the counter value as a real number, instead of the default integer value. </summary>
		[StringValue("displayAsReal")]
		DisplayAsReal
	}
}