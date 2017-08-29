using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DevUtils.ETWIMBA.SDILReader
{
	/// <summary>
	/// Method body reader.
	/// </summary>
	public class MethodBodyReader
	{
		readonly OpCode[] _multiByteOpCodes;
		readonly OpCode[] _singleByteOpCodes;

		#region il read methods

		private ushort ReadUInt16(IList<byte> il, ref int position)
		{
			return (ushort)((il[position++] | (il[position++] << 8)));
		}
		private int ReadInt32(IList<byte> il, ref int position)
		{
			return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
		}
		private ulong ReadInt64(IList<byte> il, ref int position)
		{
			return (ulong)(((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
		}
		private double ReadDouble(IList<byte> il, ref int position)
		{
			return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
		}
		private sbyte ReadSByte(IList<byte> il, ref int position)
		{
			return (sbyte)il[position++];
		}
		private byte ReadByte(IList<byte> il, ref int position)
		{
			return il[position++];
		}
		private Single ReadSingle(IList<byte> il, ref int position)
		{
			return ((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18);
		}
		#endregion

		/// <summary>
		/// Constructs the array of ILInstructions according to the IL byte code.
		/// </summary>
		/// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
		/// <param name="mi"> . </param>
		/// <returns>
		/// An enumerator that allows foreach to be used to process construct instructions in this
		/// collection.
		/// </returns>
		public  IEnumerable<ILInstruction> ConstructInstructions(MethodInfo mi)
		{
			if (mi.GetMethodBody() == null)
			{
				return new ILInstruction[0];
			}

			var il = mi.GetMethodBody().GetILAsByteArray();
			var module = mi.Module;

			var position = 0;
			var instructions = new List<ILInstruction>();
			while (position < il.Length)
			{
				var instruction = new ILInstruction();

				// get the operation code of the current instruction
				OpCode code;
				ushort value = il[position++];
				if (value != 0xfe)
				{
					code = _singleByteOpCodes[value];
				}
				else
				{
					value = il[position++];
					code = _multiByteOpCodes[value];
				}
				instruction.Code = code;
				instruction.Offset = position - 1;
				int metadataToken;
				// get the operand of the current operation
				switch (code.OperandType)
				{
					case OperandType.InlineBrTarget:
						metadataToken = ReadInt32(il, ref position);
						metadataToken += position;
						instruction.Operand = metadataToken;
						break;
					case OperandType.InlineField:
						metadataToken = ReadInt32(il, ref position);
						instruction.Operand = module.ResolveField(metadataToken);
						break;
					case OperandType.InlineMethod:
						metadataToken = ReadInt32(il, ref position);
						try
						{
							instruction.Operand = module.ResolveMethod(metadataToken);
						}
						catch
						{
							instruction.Operand = module.ResolveMember(metadataToken);
						}
						break;
					case OperandType.InlineSig:
						metadataToken = ReadInt32(il, ref position);
						instruction.Operand = module.ResolveSignature(metadataToken);
						break;
					case OperandType.InlineTok:
						metadataToken = ReadInt32(il, ref position);
						try
						{
							instruction.Operand = module.ResolveType(metadataToken);
						}
						// ReSharper disable EmptyGeneralCatchClause
						catch
						// ReSharper restore EmptyGeneralCatchClause
						{

						}
						// SSS : see what to do here
						break;
					case OperandType.InlineType:
						metadataToken = ReadInt32(il, ref position);
						// now we call the ResolveType always using the generic attributes type in order
						// to support decompilation of generic methods and classes

						// thanks to the guys from code project who commented on this missing feature

						instruction.Operand = module.ResolveType(metadataToken, mi.DeclaringType.GetGenericArguments(), mi.GetGenericArguments());
						break;
					case OperandType.InlineI:
						{
							instruction.Operand = ReadInt32(il, ref position);
							break;
						}
					case OperandType.InlineI8:
						{
							instruction.Operand = ReadInt64(il, ref position);
							break;
						}
					case OperandType.InlineNone:
						{
							instruction.Operand = null;
							break;
						}
					case OperandType.InlineR:
						{
							instruction.Operand = ReadDouble(il, ref position);
							break;
						}
					case OperandType.InlineString:
						{
							metadataToken = ReadInt32(il, ref position);
							instruction.Operand = module.ResolveString(metadataToken);
							break;
						}
					case OperandType.InlineSwitch:
						{
							var count = ReadInt32(il, ref position);
							var casesAddresses = new int[count];
							for (var i = 0; i < count; i++)
							{
								casesAddresses[i] = ReadInt32(il, ref position);
							}
							var cases = new int[count];
							for (var i = 0; i < count; i++)
							{
								cases[i] = position + casesAddresses[i];
							}
							break;
						}
					case OperandType.InlineVar:
						{
							instruction.Operand = ReadUInt16(il, ref position);
							break;
						}
					case OperandType.ShortInlineBrTarget:
						{
							instruction.Operand = ReadSByte(il, ref position) + position;
							break;
						}
					case OperandType.ShortInlineI:
						{
							instruction.Operand = ReadSByte(il, ref position);
							break;
						}
					case OperandType.ShortInlineR:
						{
							instruction.Operand = ReadSingle(il, ref position);
							break;
						}
					case OperandType.ShortInlineVar:
						{
							instruction.Operand = ReadByte(il, ref position);
							break;
						}
					default:
						{
							throw new Exception("Unknown operand type.");
						}
				}
				instructions.Add(instruction);
			}
			return instructions;
		}

		/// <summary>
		/// Gets refferenced operand.
		/// </summary>
		/// <param name="module">				 . </param>
		/// <param name="metadataToken"> The metadata token. </param>
		/// <returns>
		/// The refferenced operand.
		/// </returns>
		public object GetRefferencedOperand(Module module, int metadataToken)
		{
			var assemblyNames = module.Assembly.GetReferencedAssemblies();
			foreach (var t1 in assemblyNames.Select(t2 => Assembly.Load(t2).GetModules()).SelectMany(modules => modules))
			{
				try
				{
					var t = t1.ResolveType(metadataToken);
					return t;
				}
				// ReSharper disable EmptyGeneralCatchClause
				catch
				// ReSharper restore EmptyGeneralCatchClause
				{

				}
			}
			return null;
			//System.Reflection.Assembly.Load(module.Assembly.GetReferencedAssemblies()[3]).GetModules()[0].ResolveType(metadataToken)

		}
		/// <summary>
		/// Gets the IL code of the method
		/// </summary>
		/// <returns></returns>
		public static string GetBodyCode(IEnumerable<ILInstruction> instructions)
		{
			var ret = new StringBuilder();
			if (instructions != null)
			{
				foreach (var item in instructions)
				{
					ret.AppendLine(item.GetCode());
				}
			}
			return ret.ToString();

		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
		public MethodBodyReader()
		{
			_singleByteOpCodes = new OpCode[0x100];
			_multiByteOpCodes = new OpCode[0x100];
			var infoArray1 = typeof(OpCodes).GetFields();
			foreach (var info1 in infoArray1)
			{
				if (info1.FieldType == typeof(OpCode))
				{
					var code1 = (OpCode)info1.GetValue(null);
					var num2 = (ushort)code1.Value;
					if (num2 < 0x100)
					{
						_singleByteOpCodes[num2] = code1;
					}
					else
					{
						if ((num2 & 0xff00) != 0xfe00)
						{
							throw new Exception("Invalid OpCode.");
						}
						_multiByteOpCodes[num2 & 0xff] = code1;
					}
				}
			}
		}
	}
}
