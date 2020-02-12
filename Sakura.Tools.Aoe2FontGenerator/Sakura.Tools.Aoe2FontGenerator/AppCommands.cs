using System.Windows.Input;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	///     Provide additional commands for application. This class is static.
	/// </summary>
	public static class AppCommands
	{
		/// <summary>
		///     Re
		/// </summary>
		public static RoutedCommand DeleteAll { get; } = new RoutedCommand(nameof(DeleteAll), typeof(AppCommands));
	}
}