namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary>
	/// An ei counter source.
	/// </summary>
	public static class CounterSource
	{
		/// <summary> Creates counter set. </summary>
		/// <tparam name="T"> Generic type parameter. </tparam>
		/// <returns> The new counter set. </returns>
		public static CounterSet<T> CreateCounterSet<T>() where T : struct
		{
			var ret = new CounterSet<T>();
			return ret;
		}
	}
}