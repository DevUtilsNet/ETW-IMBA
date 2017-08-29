using System.Diagnostics.PerformanceData;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> A counter set instance extensions. </summary>
	public static class CounterSetInstanceExtensions
	{
		/// <summary> A CounterSetInstance extension method that gets a counter. </summary>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <param name="setInstance"> The setInstance to act on. </param>
		/// <param name="id"> The identifier. </param>
		/// <returns> The counter. </returns>
		public static CounterData GetCounter<T>(this CounterSetInstance setInstance, T id) where T : struct
		{
			var ret = setInstance.Counters[(int)(object)id];
			return ret;
		}
	}
}
