using System;
using System.Linq;
using DevUtils.ETWIMBA.Extensions;

namespace DevUtils.ETWIMBA.Diagnostics.Counters
{
	/// <summary>Defines a set of logical counters.</summary>
	public sealed class CounterSet<T> : IDisposable where T : struct 
	{
		private System.Diagnostics.PerformanceData.CounterSet _innerCounterSet;

		/// <summary> Default constructor. </summary>
		public CounterSet()
		{
			Initialize(typeof (T));
		}

		private void Initialize(Type counterSetType)
		{
			var counterSourceType = counterSetType.DeclaringType;
			var setAttribute = counterSetType.GetCustomAttributeT<CounterSetAttribute>();
			if (setAttribute == null || counterSourceType == null)
			{
				throw new ArgumentException("counterSetType");
			}

			var sourceAttribute = counterSourceType.GetCustomAttributeT<CounterSourceAttribute>();
			if (sourceAttribute == null)
			{
				throw new ArgumentException("counterSetType");
			}

			_innerCounterSet = new System.Diagnostics.PerformanceData.CounterSet(
				sourceAttribute.GetGuid(counterSourceType),
				setAttribute.GetGuid(counterSetType),
				(System.Diagnostics.PerformanceData.CounterSetInstanceType)setAttribute.Instances);

			var counters = counterSetType.GetFields()
				.Select(s => new { Field = s, Attribute = s.GetCustomAttributeT<CounterAttribute>() })
				.Where(w => w.Attribute != null);

			foreach (var item in counters)
			{
				if (string.IsNullOrWhiteSpace(item.Attribute.Name))
				{
					_innerCounterSet.AddCounter((int)item.Field.GetRawConstantValue(), (System.Diagnostics.PerformanceData.CounterType)item.Attribute.Type);
				}
				else
				{
					_innerCounterSet.AddCounter((int)item.Field.GetRawConstantValue(), (System.Diagnostics.PerformanceData.CounterType)item.Attribute.Type, item.Attribute.Name);
				}
			}
		}

		/// <summary> Creates an instance. </summary>
		/// <param name="name" type="string"> The name. </param>
		/// <returns> The new instance. </returns>
		public CounterSetInstance<T>  CreateInstance(string name)
		{
			var ret = new CounterSetInstance<T>(_innerCounterSet.CreateCounterSetInstance(name));
			return ret;
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_innerCounterSet.Dispose();
		}

		#endregion
	}
}
