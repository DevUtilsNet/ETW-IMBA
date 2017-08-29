using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DevUtils.ETWIMBA.Build.Diagnostics.Counters;
using DevUtils.ETWIMBA.Build.Tracing;

namespace DevUtils.ETWIMBA.Build
{
	/// <summary>
	/// Manifest builder.
	/// </summary>
	class ManifestBuilder : IDisposable
	{
		private readonly XmlWriter _xmlWriter;
		private readonly IEnumerable<Type> _eventSourcesTypes;
		private readonly IEnumerable<Type> _countSourcesTypes;
		private readonly List<Tuple<string, string>> _stringTab = new List<Tuple<string, string>>();

		/// <summary>
		/// Enumerates generate in this collection.
		/// </summary>
		/// <param name="eventSourcesTypes"> Type of the event source. </param>
		/// <param name="countSourcesTypes"></param>
		/// <param name="outputFileName">		 Filename of the output file. </param>
		public ManifestBuilder(IEnumerable<Type> eventSourcesTypes, IEnumerable<Type> countSourcesTypes, string outputFileName)
		{
			if (eventSourcesTypes == null)
			{
				throw new ArgumentNullException("eventSourcesTypes");
			}

			_eventSourcesTypes = eventSourcesTypes;
			_countSourcesTypes = countSourcesTypes;
			_xmlWriter = XmlWriter.Create(outputFileName, GetSettings());
		}

		private static XmlWriterSettings GetSettings()
		{
			return new XmlWriterSettings { Indent = true, IndentChars = "\t" };
		}

		/// <summary>
		/// Builds this object.
		/// </summary>
		public void Build()
		{
			_xmlWriter.WriteStartElement("instrumentationManifest", "http://schemas.microsoft.com/win/2004/08/events");
			_xmlWriter.WriteAttributeString("xmlns", "xs", String.Empty, "http://www.w3.org/2001/XMLSchema");
			//_xmlWriter.WriteAttributeString("xmlns", "xsi", String.Empty, "http://www.w3.org/2001/XMLSchema-instance");
			_xmlWriter.WriteAttributeString("xmlns", "win", String.Empty, "http://manifests.microsoft.com/win/2004/08/windows/events");
			_xmlWriter.WriteStartElement("instrumentation");

			#region events

			if (_eventSourcesTypes.Any())
			{
				_xmlWriter.WriteStartElement("events");

				foreach (var item in _eventSourcesTypes)
				{
					var builder = new ManifestEventProviderBuilder(this, _xmlWriter, item);
					try
					{
						builder.Build();
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Event Source : '{0}'", item), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}

			#endregion


			#region counters

			if (_countSourcesTypes.Any())
			{
				_xmlWriter.WriteStartElement("counters", "http://schemas.microsoft.com/win/2005/12/counters");
				_xmlWriter.WriteAttributeString("schemaVersion", "1.1");

				foreach (var item in _countSourcesTypes)
				{
					var builder = new ManifestCounterProviderBuilder(_xmlWriter, item);
					try
					{
						builder.Build();
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Counter Source : '{0}'", item), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}

			#endregion

			_xmlWriter.WriteEndElement();

			Localization();

			_xmlWriter.WriteEndElement();

			_xmlWriter.Close();
		}


		private void Localization()
		{
			_xmlWriter.WriteStartElement("localization");

			_xmlWriter.WriteStartElement("resources");

		 // <xs:attribute name="fallbackCulture" type="string" default="en-us" use="optional" />
			_xmlWriter.WriteAttributeString("culture", "en-us");
			_xmlWriter.WriteStartElement("stringTable");

			foreach (var item in _stringTab)
			{
				_xmlWriter.WriteStartElement("string");
				_xmlWriter.WriteAttributeString("id", item.Item1);
				_xmlWriter.WriteAttributeString("value", item.Item2);
				_xmlWriter.WriteEndElement();
			}

			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteEndElement();
		}

		/// <summary>
		/// Gets string reference.
		/// </summary>
		/// <param name="text"> The text. </param>
		/// <returns>
		/// The string reference.
		/// </returns>
		public string GetStringRef(string text)
		{
			var id = "s" + (_stringTab.Count + 1).ToString("x4");

			_stringTab.Add(Tuple.Create(id, text));

			return "$(string." + id + ")";
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_xmlWriter.Dispose();
		}

		#endregion
	}
}
