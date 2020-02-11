namespace Sakura.Tools.Aoe2FontGenerator.Loggers
{
	/// <summary>
	/// Define the level of the log message.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Debug message, usually are used for developers to troubleshooting.
		/// </summary>
		Debug,
		/// <summary>
		/// Non-harmful informational message.
		/// </summary>
		Info,
		/// <summary>
		/// Indicates that the operation has been successfully finished.
		/// </summary>
		Success,
		/// <summary>
		/// Indicates some non-critical problem has been detected and fixed or ignored automatically.
		/// </summary>
		Warning,
		/// <summary>
		/// Some error has occured and the excepted result may be damaged, but the process are still able to continue finishing other planned work.
		/// </summary>
		Error,
		/// <summary>
		/// Critical error has occured and the process has been terminated.
		/// </summary>
		Critical
	}
}