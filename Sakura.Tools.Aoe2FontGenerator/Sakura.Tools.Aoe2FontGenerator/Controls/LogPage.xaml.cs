using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sakura.Tools.Aoe2FontGenerator.Loggers;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	/// LogPage.xaml 的交互逻辑
	/// </summary>
	public partial class LogPage : UserControl
	{
		public LogPage()
		{
			InitializeComponent();
		}


		private void LogPage_OnInitialized(object sender, EventArgs e)
		{
			App.Current.ProgressLogger.LogOutputted += ProgressLogger_LogOutputted;
			App.Current.ProgressLogger.ProgressInitialized += ProgressLogger_ProgressInitialized;
		}

		private void ProgressLogger_ProgressInitialized(object sender, ProgressInitializedEventArgs e)
		{
			ProgressPanel.ItemsSource = e.LevelInfos;
		}

		private void LogPage_OnLoaded(object sender, RoutedEventArgs e)
		{
			//	Debug.WriteLine("Attached Logoutput");
			//	App.Current.ProgressLogger.LogOutputted += ProgressLogger_LogOutputted;
		}

		private void LogPage_OnUnloaded(object sender, RoutedEventArgs e)
		{
			//App.Current.ProgressLogger.LogOutputted -= ProgressLogger_LogOutputted;
		}


		private void ProgressLogger_LogOutputted(object sender, LogOutputtedEventArgs e)
		{
			AppendColorLine(e.Message, LogLevelToColor(e.Level));
		}

		private static Color LogLevelToColor(LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					return Colors.Red;
				case LogLevel.Warning:
					return Colors.Orange;
				case LogLevel.Info:
					return Colors.CornflowerBlue;
				case LogLevel.Success:
					return Colors.ForestGreen;
				case LogLevel.Debug:
					return Colors.Gray;
				default:
					return Colors.Black;
			}
		}

		/// <summary>
		/// Append a text line with specified color.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		private void AppendColorLine(string text, Color color)
		{
			LogTextBox.AppendColoredText(text, color);
			LogTextBox.ScrollToEnd();
		}
	}
}
