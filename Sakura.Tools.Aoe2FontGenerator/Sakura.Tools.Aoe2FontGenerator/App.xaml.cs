using System.Windows;
using Sakura.Tools.Aoe2FontGenerator.Loggers;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	///     The application class.
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		///     Get the current instance of this application.
		/// </summary>
		public new static App Current => (App) Application.Current;

		/// <summary>
		///     Service instance used to monitor the generation progress.
		/// </summary>
		public ProgressLogger ProgressLogger { get; private set; }

		/// <summary>
		///     Service instance used to generate fonts.
		/// </summary>
		public FontGenerator FontGenerator { get; private set; }

		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			ProgressLogger = new ProgressLogger();
			FontGenerator = new FontGenerator(ProgressLogger);
		}
	}
}