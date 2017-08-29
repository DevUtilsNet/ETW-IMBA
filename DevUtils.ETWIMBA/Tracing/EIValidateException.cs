using System;

namespace DevUtils.ETWIMBA.Tracing
{
	/// <summary>
	/// Exception for signalling ei validate errors.
	/// </summary>
	public sealed class EIValidateException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"> The message. </param>
		public EIValidateException(string message)
			: base(message)
		{
			
		}
	}
}
