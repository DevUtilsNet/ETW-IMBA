using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml;
using DevUtils.ETWIMBA.Extensions;
using DevUtils.ETWIMBA.Reflection.Extensions;
using DevUtils.ETWIMBA.SDILReader;
using DevUtils.ETWIMBA.Tracing;
using DevUtils.ETWIMBA.Tracing.Extensions;

namespace DevUtils.ETWIMBA.Build.Tracing
{
	sealed class ManifestEventProviderBuilder
	{
		private Dictionary<string, Type> _mapsTab;

		private readonly Type _eventSourceType;
		private readonly XmlWriter _xmlWriter;
		private readonly ManifestBuilder _owner;
		private readonly HashSet<int> _channelSet = new HashSet<int>();
		private readonly Dictionary<byte, string> _levels = new Dictionary<byte, string>();
		private readonly Dictionary<byte, string> _opcodes = new Dictionary<byte, string>();
		private readonly Dictionary<ushort, string> _tasks = new Dictionary<ushort, string>();
		private readonly Dictionary<ulong, string> _keywords = new Dictionary<ulong, string>();
		private readonly List<EIEventTraceAttribute> _eventTraceAttributes = new List<EIEventTraceAttribute>();
		private readonly Lazy<MethodBodyReader> _methodBodyReader = new Lazy<MethodBodyReader>(() => new MethodBodyReader());

		public ManifestEventProviderBuilder(ManifestBuilder owner, XmlWriter xmlWriter, Type eventSourceType)
		{
			_owner = owner;
			_xmlWriter = xmlWriter;
			_eventSourceType = eventSourceType;
		}

		private void WriteMessageAttrib(string message)
		{
			_xmlWriter.WriteAttributeString("message", _owner.GetStringRef(message));
		}

		public void Build()
		{
			var providerName = _eventSourceType.GetName();

			var fileName = Path.ChangeExtension(_eventSourceType.Assembly.GetManifestFileName(), ".IM.dll");

			_xmlWriter.WriteStartElement("provider");
			_xmlWriter.WriteAttributeString("name", providerName);
			_xmlWriter.WriteAttributeString("guid", _eventSourceType.GetGuid().ToString("B"));
			_xmlWriter.WriteAttributeString("symbol", providerName.Replace('-', '_').Replace(' ', '_'));
			_xmlWriter.WriteAttributeString("resourceFileName", fileName);
			_xmlWriter.WriteAttributeString("messageFileName", fileName);

			Channels();

			Levels();

			Tasks();

			Opcodes();

			Keywords();

			Events();

			Maps();

			_xmlWriter.WriteEndElement();
		}

		private void Channels()
		{
			var channels = _eventSourceType.GetNestedType("Channels", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (channels != null)
			{
				_xmlWriter.WriteStartElement("channels");

				var eventSourceName = _eventSourceType.GetName();

				foreach (var item in channels.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					try
					{
						var value = (int)item.GetRawConstantValue();

						var ca = item.GetCustomAttributeT<EIChannelAttribute>();
						var ica = item.GetCustomAttributeT<EIChannelImportAttribute>();

						if (ca != null && ica != null)
						{
							throw new EIValidateException("ChannelAttribute and ChannelImportAttribute attributes are mutually exclusive");
						}
						if (ca == null && ica == null)
						{
							throw new EIValidateException("Channel definition should have one attribute of them ChannelAttribute or ChannelImportAttribute");
						}

						if (ca != null)
						{
							if (value <= 10)
							{
								throw new EIValidateException("Channel values less or equals than 10 are reserved for system use.");
							}

							_xmlWriter.WriteStartElement("channel");

							var name = String.IsNullOrEmpty(ca.Name) ? eventSourceName + "/" + item.Name : ca.Name;

							ValidateChannelName(name);

							_xmlWriter.WriteAttributeString("name", name);
							_xmlWriter.WriteAttributeString("type", ca.Type.ToString());
							_xmlWriter.WriteAttributeString("chid", "c" + value);
							if (ca.ActualEnabled.HasValue)
							{
								_xmlWriter.WriteAttributeString("enabled", ca.ActualEnabled.Value.ToString().ToLower());
							}

							_xmlWriter.WriteAttributeString("value", value.ToString(CultureInfo.InvariantCulture));
							if (!string.IsNullOrEmpty(ca.Access))
							{
								_xmlWriter.WriteAttributeString("access", ca.Access);
							}
							if (ca.Isolation != EIChannelIsolation.None)
							{
								_xmlWriter.WriteAttributeString("isolation", ca.Isolation.ToString());
							}

							if (!string.IsNullOrEmpty(ca.Message))
							{
								WriteMessageAttrib(ca.Message);
							}

							var logging = item.GetCustomAttributeT<EIChannelLoggingAttribute>();

							if (logging != null)
							{
								if (logging.AutoBackup && !logging.Retention)
								{
									throw new EIValidateException("You can set 'AutoBackup' to true only if 'Retention' is set to true.");
								}

								_xmlWriter.WriteStartElement("logging");
								if (logging.AutoBackup)
								{
									_xmlWriter.WriteElementString("autoBackup", "true");
								}
								if (logging.MaxSize > 1048576)
								{
									_xmlWriter.WriteElementString("maxSize", logging.MaxSize.ToString(CultureInfo.InvariantCulture));
								}
								if (logging.Retention)
								{
									_xmlWriter.WriteElementString("retention", "true");
								}
								_xmlWriter.WriteEndElement();
							}

							var publishing = item.GetCustomAttributeT<EIChannelPublishingAttribute>();

							if (publishing != null)
							{
								if (ca.Type != EIChannelType.Debug && ca.Type != EIChannelType.Analytic && ca.Isolation != EIChannelIsolation.Custom)
								{
									throw new EIValidateException("Only Debug and Analytic channels and channels that use Custom Isolation can specify logging properties ('Publishing') for their session.");
								}

								_xmlWriter.WriteStartElement("publishing");

								if (publishing.Level != 0)
								{
									_xmlWriter.WriteElementString("level", publishing.Level.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.Keywords != 0)
								{
									_xmlWriter.WriteElementString("keywords", publishing.Keywords.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.BufferSize != 0)
								{
									_xmlWriter.WriteElementString("bufferSize", publishing.BufferSize.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.MinBuffers != 0)
								{
									_xmlWriter.WriteElementString("minBuffers", publishing.MinBuffers.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.FileMax > 1)
								{
									_xmlWriter.WriteElementString("fileMax", publishing.FileMax.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.MaxBuffers != 0)
								{
									_xmlWriter.WriteElementString("maxBuffers", publishing.MaxBuffers.ToString(CultureInfo.InvariantCulture));
								}
								if (publishing.Latency != 0)
								{
									_xmlWriter.WriteElementString("latency", publishing.Latency.ToString(CultureInfo.InvariantCulture));
								}
								_xmlWriter.WriteEndElement();
							}
							_xmlWriter.WriteEndElement();
						}
						else
						{
							_xmlWriter.WriteStartElement("importChannel");
							var name = String.IsNullOrEmpty(ica.Name) ? item.Name : ica.Name;
							_xmlWriter.WriteAttributeString("name", name);
							_xmlWriter.WriteAttributeString("chid", "c" + value);
							_xmlWriter.WriteEndElement();
						}

						if (!_channelSet.Add(value))
						{
							throw new EIValidateException(String.Format("Channel ID {0} is already in use.", value));
						}
					}
					catch (Exception ex)
					{
						throw new Exception(item.ToString(), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private void Levels()
		{
			var levels = _eventSourceType.GetNestedType("Levels", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (levels != null)
			{
				_xmlWriter.WriteStartElement("levels");

				foreach (var item in levels.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					try
					{
						var value = (int)item.GetRawConstantValue();
						if (value < 16)
						{
							throw new EIValidateException("Level values less than 16 are reserved for system use.");
						}

						_xmlWriter.WriteStartElement("level");
						_xmlWriter.WriteAttributeString("name", item.Name);
						_xmlWriter.WriteAttributeString("value", value.ToString(CultureInfo.InvariantCulture));
						_xmlWriter.WriteEndElement();

						_levels.Add((byte)value, item.Name);
					}
					catch (Exception ex)
					{
						throw new Exception(item.ToString(), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private static void ValidateChannelName(string name)
		{
			if (name.Length > 255)
			{
				throw new EIValidateException("Channel name must be less that 255 characters.");
			}

			var ch = name.FirstOrDefault(
				f =>
				f == '>' || f == '<' || f == '&' || f == '"' || f == '|' || f == '\\' || f == ':' || f == '`' || f == '?' ||
				f == '*' || f < 31);


			if (ch != default(char))
			{
				throw new EIValidateException(String.Format(
					"Invalid character '{0}' in channel name. " +
					"Channel names cannot contain the following characters: " +
					"'>', '<', '&', '\"', '|', '\\', ':', '`', '?', '*', or characters with codes less than 31.", ch));
			}
		}

		private void Tasks()
		{
			var tasks = _eventSourceType.GetNestedType("Tasks", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (tasks != null)
			{
				_xmlWriter.WriteStartElement("tasks");

				foreach (var item in tasks.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					try
					{
						var value = (int)item.GetRawConstantValue();
						if (value == 0)
						{
							throw new EIValidateException("Task with value 0 are reserved for system use.");
						}

						_xmlWriter.WriteStartElement("task");

						_xmlWriter.WriteAttributeString("name", item.Name);
						_xmlWriter.WriteAttributeString("value", value.ToString(CultureInfo.InvariantCulture));

						_xmlWriter.WriteEndElement();

						_tasks.Add((ushort)value, item.Name);
					}
					catch (Exception ex)
					{
						throw new Exception(item.ToString(), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private void Opcodes()
		{
			var opcodes = _eventSourceType.GetNestedType("Opcodes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			if (opcodes != null)
			{
				_xmlWriter.WriteStartElement("opcodes");
				foreach (var item in opcodes.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					try
					{
						var value = (int)item.GetRawConstantValue();
						if (value <= 10)
						{
							throw new EIValidateException("Opcode values less than 11 are reserved for system use.");
						}

						_xmlWriter.WriteStartElement("opcode");

						_xmlWriter.WriteAttributeString("name", item.Name);
						WriteMessageAttrib(item.Name);
						_xmlWriter.WriteAttributeString("value", value.ToString(CultureInfo.InvariantCulture));

						_xmlWriter.WriteEndElement();

						_opcodes.Add((byte)value, item.Name);
					}
					catch (Exception ex)
					{
						throw new Exception(item.ToString(), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private void Keywords()
		{
			var keywords = _eventSourceType.GetNestedType("Keywords", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			if (keywords != null)
			{
				_xmlWriter.WriteStartElement("keywords");

				foreach (var item in keywords.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					try
					{
						var value = (ulong)item.GetRawConstantValue();
						if ((value & value - 1uL) != 0uL)
						{
							throw new EIValidateException(String.Format("Value {0} needs to be a power of 2.", value.ToString("x", CultureInfo.CurrentCulture)));
						}

						_xmlWriter.WriteStartElement("keyword");

						_xmlWriter.WriteAttributeString("name", item.Name);

						WriteMessageAttrib(item.Name);

						_xmlWriter.WriteAttributeString("mask", "0x" + value.ToString("x", CultureInfo.InvariantCulture));

						_xmlWriter.WriteEndElement();

						_keywords.Add(value, item.Name);
					}
					catch (Exception ex)
					{
						throw new Exception(item.ToString(), ex);
					}
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private string GetLevelName(byte level)
		{
			if (level <= (byte)EIEventLevel.Verbose)
			{
				return "win:" + ((EIEventLevel)level);
			}

			if (level < 16)
			{
				return "win:" + level;
			}

			string result;
			if (!_levels.TryGetValue(level, out result))
			{
				throw new EIValidateException(String.Format("Use of undefined level value {0}.", level));
			}
			return result;
		}

		private string GetChannel(int channel)
		{
			if (!_channelSet.Contains(channel))
			{
				throw new EIValidateException(String.Format("Use of undefined channel value {0}.", channel));
			}
			return "c" + channel.ToString(CultureInfo.InvariantCulture);
		}

		private string GetKeywords(ulong keywords)
		{
			switch (keywords)
			{
				case (ulong)EventKeywords.AuditFailure:
					{
						return "win:AuditFailure";
					}
				case (ulong)EventKeywords.AuditSuccess:
					{
						return "win:AuditSuccess";
					}
				case (ulong)EventKeywords.EventLogClassic:
					{
						return "win:EventlogClassic";
					}
				case (ulong)EventKeywords.None:
					{
						return "win:AnyKeyword";
					}
				case (ulong)EventKeywords.Sqm:
					{
						return "win:SQM";
					}
				case (ulong)EventKeywords.WdiContext:
					{
						return "win:WDIContext";
					}
				case (ulong)EventKeywords.WdiDiagnostic:
					{
						return "win:WDIDiag";
					}
				default:
					{
						var text = "";

						for (var num = 1uL; num != 0uL; num <<= 1)
						{
							if ((keywords & num) != 0uL)
							{
								string str;
								if (!_keywords.TryGetValue(num, out str))
								{
									throw new EIValidateException(String.Format("Use of undefined keyword value {0}.", num.ToString("x", CultureInfo.CurrentCulture)));
								}
								if (text.Length != 0)
								{
									text += " ";
								}
								text += str;
							}
						}
						return text;
					}
			}
		}

		private string GetOpcodeName(byte opcode)
		{
			switch (opcode)
			{
				case (byte)EventOpcode.Info:
					return "win:Info";
				case (byte)EventOpcode.Start:
					return "win:Start";
				case (byte)EventOpcode.Stop:
					return "win:Stop";
				case (byte)EventOpcode.DataCollectionStart:
					return "win:DC_Start";
				case (byte)EventOpcode.DataCollectionStop:
					return "win:DC_Stop";
				case (byte)EventOpcode.Extension:
					return "win:Extension";
				case (byte)EventOpcode.Reply:
					return "win:Reply";
				case (byte)EventOpcode.Resume:
					return "win:Resume";
				case (byte)EventOpcode.Suspend:
					return "win:Suspend";
				case (byte)EventOpcode.Send:
					return "win:Send";
				case (byte)EventOpcode.Receive:
					return "win:Receive";
				default:
					{
						string result;
						if (!_opcodes.TryGetValue(opcode, out result))
						{
							throw new EIValidateException(String.Format("Use of undefined opcode value {0}.", opcode));
						}
						return result;
					}
			}
		}

		private string GetTaskName(ushort task)
		{
			if (task == (ushort)EventTask.None)
			{
				return String.Empty;
			}

			string result;
			if (!_tasks.TryGetValue(task, out result))
			{
				throw new EIValidateException(String.Format("Use of undefined task value {0}.", task));
			}
			return result;
		}

		private static string TranslateToManifestConvention(string eventMessage)
		{
			StringBuilder stringBuilder = null;
			var num = 0;
			var i = 0;
			while (i < eventMessage.Length)
			{
				if (eventMessage[i] == '{')
				{
					var num2 = i;
					i++;
					var num3 = 0;
					while (i < eventMessage.Length && char.IsDigit(eventMessage[i]))
					{
						num3 = num3 * 10 + eventMessage[i] - 48;
						i++;
					}
					if (i < eventMessage.Length && eventMessage[i] == '}')
					{
						i++;
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder();
						}
						stringBuilder.Append(eventMessage, num, num2 - num);
						stringBuilder.Append('%').Append(num3 + 1);
						num = i;
					}
				}
				else
				{
					i++;
				}
			}
			if (stringBuilder == null)
			{
				return eventMessage;
			}
			stringBuilder.Append(eventMessage, num, i - num);
			return stringBuilder.ToString();
		}

		private static string GetTypeName(Type type)
		{
			if (type.IsEnum)
			{
				var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				var typeName = GetTypeName(fields[0].FieldType);
				return typeName.Replace("win:Int", "win:UInt");
			}
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					return "win:Boolean";
				case TypeCode.SByte:
					return "win:Int8";
				case TypeCode.Byte:
					return "win:UInt8";
				case TypeCode.Int16:
					return "win:Int16";
				case TypeCode.UInt16:
					return "win:UInt16";
				case TypeCode.Int32:
					return "win:Int32";
				case TypeCode.UInt32:
					return "win:UInt32";
				case TypeCode.Int64:
					return "win:Int64";
				case TypeCode.UInt64:
					return "win:UInt64";
				case TypeCode.Single:
					return "win:Float";
				case TypeCode.Double:
					return "win:Double";
				case TypeCode.String:
					return "win:UnicodeString";
			}
			if (type == typeof(Guid))
			{
				return "win:GUID";
			}
			throw new EIValidateException(String.Format("Unsupported type {0} in event source.", type.Name));
		}

		private void Events()
		{
			_xmlWriter.WriteStartElement("events");

			var mparams = new List<Tuple<string, ParameterInfo[]>>();

			foreach (var item in _eventSourceType.ExtractEventTraceAttributes())
			{
				var method = item.Item1;
				var attr = item.Item2;

				try
				{
					DebugCheckEvent(method, attr);

					if (_eventTraceAttributes.Any(a => a.EventId == attr.EventId))
					{
						throw new EIValidateException(string.Format("Event ID {0} which is already in use.", attr.EventId));
					}

					_eventTraceAttributes.Add(attr);

					_xmlWriter.WriteStartElement("event");

					_xmlWriter.WriteAttributeString("value", attr.EventId.ToString(CultureInfo.InvariantCulture));
					_xmlWriter.WriteAttributeString("version", attr.Version.ToString(CultureInfo.InvariantCulture));
					_xmlWriter.WriteAttributeString("level", GetLevelName(attr.Level));

					var templateName = method.Name + "Args";

					_xmlWriter.WriteAttributeString("template", templateName);

					if (mparams.Any(a => a.Item1.Equals(templateName)))
					{
						throw new EIValidateException(String.Format("Event name {0} used more than once.  If you wish to overload a method, the overloaded method should not have a Event attribute.", method.Name));
					}

					if (attr.Message != null)
					{
						WriteMessageAttrib(TranslateToManifestConvention(attr.Message));
					}

					//if (attr.Channel > 10)
					//{
					_xmlWriter.WriteAttributeString("channel", GetChannel(attr.Channel));
					//}
					if (attr.Keywords != 0)
					{
						_xmlWriter.WriteAttributeString("keywords", GetKeywords(attr.Keywords));
					}
					if (attr.Opcode != 0)
					{
						_xmlWriter.WriteAttributeString("opcode", GetOpcodeName(attr.Opcode));
					}
					if (attr.Task != 0)
					{
						_xmlWriter.WriteAttributeString("task", GetTaskName(attr.Task));
					}

					mparams.Add(Tuple.Create(templateName, method.GetParameters()));

					_xmlWriter.WriteEndElement();
				}
				catch (Exception ex)
				{
					throw new Exception(method.Name, ex);
				}
			}

			_xmlWriter.WriteEndElement();

			_xmlWriter.WriteStartElement("templates");

			foreach (var item in mparams)
			{
				_xmlWriter.WriteStartElement("template");
				_xmlWriter.WriteAttributeString("tid", item.Item1);
				foreach (var item2 in item.Item2)
				{
					_xmlWriter.WriteStartElement("data");

					_xmlWriter.WriteAttributeString("name", item2.Name);
					_xmlWriter.WriteAttributeString("inType", GetTypeName(item2.ParameterType));
					if (item2.ParameterType.IsEnum)
					{
						_xmlWriter.WriteAttributeString("map", item2.ParameterType.Name);

						_mapsTab = _mapsTab ?? new Dictionary<string, Type>();

						if (!_mapsTab.ContainsKey(item2.ParameterType.Name))
						{
							_mapsTab.Add(item2.ParameterType.Name, item2.ParameterType);
						}
					}

					_xmlWriter.WriteEndElement();
				}

				_xmlWriter.WriteEndElement();
			}

			_xmlWriter.WriteEndElement();

		}

		private void Maps()
		{
			if (_mapsTab != null)
			{
				_xmlWriter.WriteStartElement("maps");
				foreach (var current2 in _mapsTab.Values)
				{
					var value = current2.GetCustomAttributeT<FlagsAttribute>() != null ? "bitMap" : "valueMap";
					_xmlWriter.WriteStartElement(value);
					_xmlWriter.WriteAttributeString("name", current2.Name);
					var fields = current2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
					var array = fields;
					foreach (var fieldInfo in array)
					{
						var rawConstantValue = fieldInfo.GetRawConstantValue();
						{
							string value2 = null;
							if (rawConstantValue is int)
							{
								value2 = ((int)rawConstantValue).ToString("x", CultureInfo.InvariantCulture);
							}
							else
							{
								if (rawConstantValue is long)
								{
									value2 = ((long)rawConstantValue).ToString("x", CultureInfo.InvariantCulture);
								}
							}

							if (value2 != null)
							{
								_xmlWriter.WriteStartElement("map");
								_xmlWriter.WriteAttributeString("value", "0x" + value2);
								WriteMessageAttrib(fieldInfo.Name);
								_xmlWriter.WriteEndElement();
							}
						}
					}
					_xmlWriter.WriteEndElement();
				}
				_xmlWriter.WriteEndElement();
			}
		}

		private void DebugCheckEvent(MethodInfo method, EIEventTraceAttribute eventAttribute)
		{
			var helperCallFirstArg = GetHelperCallFirstArg(method);
			if (helperCallFirstArg >= 0 && eventAttribute.EventId != helperCallFirstArg)
			{
				throw new EIValidateException(String.Format("Event ID {0} but {1} was passed to WriteEvent.", eventAttribute.EventId, helperCallFirstArg));
			}
		}

		private int GetHelperCallFirstArg(MethodInfo method)
		{
			var inst = _methodBodyReader.Value.ConstructInstructions(method);

			var stack = new Stack<int>();

			foreach (var item in inst)
			{
				if (item.Code == OpCodes.Ldc_I4_0)
				{
					stack.Push(0);
				}
				else if (item.Code == OpCodes.Ldc_I4_1)
				{
					stack.Push(1);
				}
				else if (item.Code == OpCodes.Ldc_I4_2)
				{
					stack.Push(2);
				}
				else if (item.Code == OpCodes.Ldc_I4_3)
				{
					stack.Push(3);
				}
				else if (item.Code == OpCodes.Ldc_I4_4)
				{
					stack.Push(4);
				}
				else if (item.Code == OpCodes.Ldc_I4_5)
				{
					stack.Push(5);
				}
				else if (item.Code == OpCodes.Ldc_I4_6)
				{
					stack.Push(6);
				}
				else if (item.Code == OpCodes.Ldc_I4_7)
				{
					stack.Push(7);
				}
				else if (item.Code == OpCodes.Ldc_I4_8)
				{
					stack.Push(8);
				}
				else if (item.Code == OpCodes.Ldc_I4_S)
				{
					stack.Push((sbyte)item.Operand);
				}
				else if (item.Code == OpCodes.Newarr ||
					item.Code == OpCodes.Stelem ||
					item.Code == OpCodes.Stelem_I ||
					item.Code == OpCodes.Stelem_I1 ||
					item.Code == OpCodes.Stelem_I2 ||
					item.Code == OpCodes.Stelem_I4 ||
					item.Code == OpCodes.Stelem_I8 ||
					item.Code == OpCodes.Stelem_R4 ||
					item.Code == OpCodes.Stelem_R8 ||
					item.Code == OpCodes.Stelem_Ref)
				{
					stack.Pop();
				}
			}

			if (stack.Count != 1)
			{
				throw new InvalidProgramException("Invalid argument detection");
			}

			return stack.Pop();
		}

	}
}
