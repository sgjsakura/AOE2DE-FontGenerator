using System;
using System.ComponentModel;

namespace Sakura.Tools.Aoe2FontGenerator.Loggers
{
	/// <summary>
	///     Provide event data for <see cref="ProgressLogger.LogOutputted" /> event.
	/// </summary>
	public class LogOutputtedEventArgs : EventArgs
	{
		/// <summary>
		///     Initialize a new instance of <see cref="LogOutputtedEventArgs" />.
		/// </summary>
		/// <param name="level">The level of the message.</param>
		/// <param name="message">The content of the message.</param>
		public LogOutputtedEventArgs(LogLevel level, [Localizable(true)] string message)
		{
			Level = level;
			Message = message;
		}

		/// <summary>
		///     Get the level of the message.
		/// </summary>
		public LogLevel Level { get; }

		/// <summary>
		///     Get the content of the message.
		/// </summary>
		[Localizable(true)]
		public string Message { get; }
	}
}