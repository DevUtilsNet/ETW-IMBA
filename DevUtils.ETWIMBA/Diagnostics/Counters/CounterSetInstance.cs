using System;
using System.Diagnostics.PerformanceData;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary> Creates an instance of the logical counters defined in the <see cref="CounterSet{T}" /> class. </summary>
	public sealed class CounterSetInstance<T> : IDisposable where T : struct 
	{
		private readonly CounterSetInstance _innerCounterSetInstance;

		internal CounterSetInstance(CounterSetInstance setInstance)
		{
			_innerCounterSetInstance = setInstance;
		}
		/// <summary> Gets a counter. </summary>
		/// <param name="counterId" type="T"> Identifier for the counter. </param>
		/// <returns> The counter. </returns>
		public CounterData GetCounter(int counterId)
		{
			var ret = _innerCounterSetInstance.Counters[counterId];
			return ret;
		}

		/// <summary> Indexer to get items within this collection using array index syntax. </summary>
		/// <param name="counterId" type="int"> Identifier for the counter. </param>
		/// <returns> The indexed item. </returns>
		public CounterData this[T counterId]
		{
			get
			{
				var ret = GetCounter((int)(object)counterId);
				return ret;
			}
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_innerCounterSetInstance.Dispose();
		}

		#endregion
	}
}
