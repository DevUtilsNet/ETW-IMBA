using System;
using System.Security.Cryptography;

namespace DevUtils.ETWIMBA.Extensions
{
	static class StringExtensions
	{
		public static Guid GenerateGuid(this string text)
		{
			var array = new byte[text.Length * 2 + 16];
			var num = 1210854834u;
			var num2 = 3281012680u;
			var num3 = 2281183765u;
			var num4 = 3217109243u;
			var num5 = 3;
			while (0 <= num5)
			{
				array[num5] = (byte)num;
				num >>= 8;
				array[num5 + 4] = (byte)num2;
				num2 >>= 8;
				array[num5 + 8] = (byte)num3;
				num3 >>= 8;
				array[num5 + 12] = (byte)num4;
				num4 >>= 8;
				num5--;
			}
			for (var i = 0; i < text.Length; i++)
			{
				array[2 * i + 16 + 1] = (byte)text[i];
				array[2 * i + 16] = (byte)(text[i] >> 8);
			}
			var sHa = SHA1.Create();
			var array2 = sHa.ComputeHash(array);
			var a = (((array2[3] << 8) + array2[2] << 8) + array2[1] << 8) + array2[0];
			var b = (short)((array2[5] << 8) + array2[4]);
			var num6 = (short)((array2[7] << 8) + array2[6]);
			num6 = (short)((num6 & 4095) | 20480);
			var result = new Guid(a, b, num6, array2[8], array2[9], array2[10], array2[11], array2[12], array2[13], array2[14], array2[15]);
			return result;
		}
	}
}
