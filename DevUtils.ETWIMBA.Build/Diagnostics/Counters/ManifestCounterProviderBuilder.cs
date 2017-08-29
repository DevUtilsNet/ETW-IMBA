using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using DevUtils.ETWIMBA.Diagnostics.Counters;
using DevUtils.ETWIMBA.Extensions;
using DevUtils.ETWIMBA.Reflection.Extensions;

namespace DevUtils.ETWIMBA.Build.Diagnostics.Counters
{
	sealed class ManifestCounterProviderBuilder
	{
		private readonly XmlWriter _xmlWriter;
		private readonly Type _counterSourceType;

		public ManifestCounterProviderBuilder(XmlWriter xmlWriter, Type counterSourceType)
		{
			_xmlWriter = xmlWriter;
			_counterSourceType = counterSourceType;
		}

		public void Build()
		{
			var attribute = _counterSourceType.GetCustomAttributeT<CounterSourceAttribute>();

			_xmlWriter.WriteStartElement("provider");

			var providerName = attribute.GetName(_counterSourceType);

			_xmlWriter.WriteAttributeString("providerGuid", attribute.GetGuid(_counterSourceType).ToString("B"));
			_xmlWriter.WriteAttributeString("symbol", providerName.Replace('-', '_').Replace(' ', '_').Replace('+', '_'));
			_xmlWriter.WriteAttributeString("applicationIdentity", Path.ChangeExtension(_counterSourceType.Assembly.GetManifestFileName(), ".IM.dll"));

			if (!string.IsNullOrWhiteSpace(attribute.Name))
			{
				_xmlWriter.WriteAttributeString("providerName", providerName);
			}

			CounterSets();

			_xmlWriter.WriteEndElement();
		}

		private void CounterSets()
		{
			var sets = _counterSourceType.GetNestedTypes()
				.Select(s => new { Type = s, Attribute = s.GetCustomAttributeT<CounterSetAttribute>() })
				.Where(w => w.Attribute != null);

			foreach (var item in sets)
			{
				_xmlWriter.WriteStartElement("counterSet");

				var setName = item.Attribute.GetName(item.Type);

				_xmlWriter.WriteAttributeString("name", setName);
				_xmlWriter.WriteAttributeString("uri", item.Attribute.GetUri(item.Type));
				_xmlWriter.WriteAttributeString("description", item.Attribute.GetDescription());
				_xmlWriter.WriteAttributeString("instances", item.Attribute.Instances.GetStringValue());
				_xmlWriter.WriteAttributeString("guid", item.Attribute.GetGuid(item.Type).ToString("B"));
				_xmlWriter.WriteAttributeString("symbol", setName.Replace('-', '_').Replace(' ', '_').Replace('+', '_'));

				Counters(item.Type);

				_xmlWriter.WriteEndElement();
			}
		}

		private void Counters(Type set)
		{
			var counters = set.GetFields()
				.Select(s => new { Field = s, Attribute = s.GetCustomAttributeT<CounterAttribute>() })
				.Where(w => w.Attribute != null);

			foreach (var item in counters)
			{
				_xmlWriter.WriteStartElement("counter");

				_xmlWriter.WriteAttributeString("uri", item.Attribute.GetUri(item.Field));
				_xmlWriter.WriteAttributeString("type", item.Attribute.Type.GetStringValue());
				_xmlWriter.WriteAttributeString("id", item.Field.GetRawConstantValue().ToString());
				_xmlWriter.WriteAttributeString("detailLevel", item.Attribute.DetailLevel.GetStringValue());

				if (!string.IsNullOrWhiteSpace(item.Attribute.Name))
				{
					_xmlWriter.WriteAttributeString("name", item.Attribute.Name);
				}

				if (!string.IsNullOrWhiteSpace(item.Attribute.Description))
				{
					_xmlWriter.WriteAttributeString("description", item.Attribute.Description);
				}

				if (item.Attribute.ActualBaseId != null)
				{
					_xmlWriter.WriteAttributeString("baseID", item.Attribute.BaseId.ToString(CultureInfo.InvariantCulture));
				}

				if (item.Attribute.ActualDefaultScale != null)
				{
					_xmlWriter.WriteAttributeString("defaultScale", item.Attribute.DefaultScale.ToString(CultureInfo.InvariantCulture));
				}

				if (item.Attribute.Aggregate != CounterAggregate.None)
				{
					_xmlWriter.WriteAttributeString("aggregate", item.Attribute.Aggregate.GetStringValue());
				}

				if (item.Attribute.ActualPerfTimeId != null)
				{
					_xmlWriter.WriteAttributeString("perfTimeID", item.Attribute.PerfTimeId.ToString(CultureInfo.InvariantCulture));
				}

				if (item.Attribute.ActualPerfFreqId != null)
				{
					_xmlWriter.WriteAttributeString("perfFreqID", item.Attribute.PerfFreqId.ToString(CultureInfo.InvariantCulture));
				}

				if (item.Attribute.ActualMultiCounterId != null)
				{
					_xmlWriter.WriteAttributeString("multiCounterID", item.Attribute.MultiCounterId.ToString(CultureInfo.InvariantCulture));
				}

				var attributes = new List<CounterAttributeBaseAttribute>(5)
					{
						item.Field.GetCustomAttributeT<CounterAttributeNoDisplayAttribute>(),
						item.Field.GetCustomAttributeT<CounterAttributeReferenceAttribute>(),
						item.Field.GetCustomAttributeT<CounterAttributeDisplayAsHexAttribute>(),
						item.Field.GetCustomAttributeT<CounterAttributeDisplayAsRealAttribute>(),
						item.Field.GetCustomAttributeT<CounterAttributeNoDigitGroupingAttribute>()
					}.Where(w => w != null);

				if (attributes.Any())
				{
					_xmlWriter.WriteStartElement("counterAttributes");

					foreach (var item2 in attributes)
					{
						CounterAttribute(item2);
					}

					_xmlWriter.WriteEndElement();
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private void CounterAttribute(CounterAttributeBaseAttribute attribute)
		{
			_xmlWriter.WriteStartElement("counterAttribute");

			_xmlWriter.WriteAttributeString("name", attribute.Name.GetStringValue());

			_xmlWriter.WriteEndElement();
		}
	}
}