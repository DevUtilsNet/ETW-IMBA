using System;
using System.Globalization;
using System.Reflection.Emit;

namespace DevUtils.ETWIMBA.SDILReader
{
	/// <summary>
	/// Il instruction.
	/// </summary>
	public class ILInstruction
	{
		// Fields
		private OpCode _code;
		private object _operand;


		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		public OpCode Code
		{
			get { return _code; }
			set { _code = value; }
		}

		/// <summary>
		/// Gets or sets the operand.
		/// </summary>
		/// <value>
		/// The operand.
		/// </value>
		public object Operand
		{
			get { return _operand; }
			set { _operand = value; }
		}

		/// <summary>
		/// Gets or sets the information describing the operand.
		/// </summary>
		/// <value>
		/// Information describing the operand.
		/// </value>
		public byte[] OperandData { get; set; }

		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public int Offset { get; set; }

		static string ProcessSpecialTypes(string typeName)
		{
			var result = typeName;
			switch (typeName)
			{
				case "System.string":
				case "System.String":
				case "String":
					result = "string"; break;
				case "System.Int32":
				case "Int":
				case "Int32":
					result = "int"; break;
			}
			return result;
		}

		/// <summary>
		/// Returns a friendly strign representation of this instruction
		/// </summary>
		/// <returns></returns>
		public string GetCode()
		{
			var result = "";
			result += GetExpandedOffset(Offset) + " : " + _code;
			if (_operand != null)
			{
				switch (_code.OperandType)
				{
					case OperandType.InlineField:
						var fOperand = ((System.Reflection.FieldInfo)_operand);
						result += " " + ProcessSpecialTypes(fOperand.FieldType.ToString()) + " " +
								ProcessSpecialTypes(fOperand.ReflectedType.ToString()) +
								"::" + fOperand.Name + "";
						break;
					case OperandType.InlineMethod:
						try
						{
							var mOperand = (System.Reflection.MethodInfo)_operand;
							result += " ";
							if (!mOperand.IsStatic) result += "instance ";
							result += ProcessSpecialTypes(mOperand.ReturnType.ToString()) +
									" " + ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
									"::" + mOperand.Name + "()";
						}
						catch
						{
							try
							{
								var mOperand = (System.Reflection.ConstructorInfo)_operand;
								result += " ";
								if (!mOperand.IsStatic) result += "instance ";
								result += "void " +
										ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
										"::" + mOperand.Name + "()";
							}
// ReSharper disable EmptyGeneralCatchClause
							catch
// ReSharper restore EmptyGeneralCatchClause
							{
							}
						}
						break;
					case OperandType.ShortInlineBrTarget:
					case OperandType.InlineBrTarget:
						result += " " + GetExpandedOffset((int)_operand);
						break;
					case OperandType.InlineType:
						result += " " + ProcessSpecialTypes(_operand.ToString());
						break;
					case OperandType.InlineString:
						if (_operand.ToString() == "\r\n") result += " \"\\r\\n\"";
						else result += " \"" + _operand + "\"";
						break;
					case OperandType.ShortInlineVar:
						result += _operand.ToString();
						break;
					case OperandType.InlineI:
					case OperandType.InlineI8:
					case OperandType.InlineR:
					case OperandType.ShortInlineI:
					case OperandType.ShortInlineR:
						result += _operand.ToString();
						break;
					case OperandType.InlineTok:
						var type = _operand as Type;
						if (type != null)
							result += type.FullName;
						else
							result += "not supported";
						break;

					default: result += "not supported"; break;
				}
			}
			return result;

		}

		/// <summary>
		/// Add enough zeros to a number as to be represented on 4 characters
		/// </summary>
		/// <param name="offset">
		/// The number that must be represented on 4 characters
		/// </param>
		/// <returns>
		/// </returns>
		private string GetExpandedOffset(long offset)
		{
			var result = offset.ToString(CultureInfo.InvariantCulture);
			for (var i = 0; result.Length < 4; i++)
			{
				result = "0" + result;
			}
			return result;
		}
	}
}
