using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sakura.Tools.Aoe2FontGenerator.Data;
using Sakura.Tools.Aoe2FontGenerator.Models;
using Sakura.Tools.Aoe2FontGenerator.Properties;
using Sakura.Tools.Aoe2FontGenerator.Utilities;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// The main window of this application.
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Initialize a new instance of <see cref="MainWindow"/>.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			// Add a new initialized item
			CharSetFontMappings = new ObservableCollection<CharSetFontMapping>
			{
				new CharSetFontMapping
				{
					CharSet = new FullCharSetSource(),
					Font = new SystemFontSource(FontInfo.GetControlFont(this))
				}
			};

			// Default setting
			GenerationSetting = new GenerationSetting
			{
				TextureSize = Settings.Default.DefaultTextureSize,
				GlyphSize = Settings.Default.DefaultGlyphSize,
				GlyphSpace = Settings.Default.DefaultGlyphSpace,
				SurfaceFileNameFormat = Settings.Default.DefaultSurfaceFileFormat,
				MetadataFileName = Settings.Default.DefaultMetaFileName,
				OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				DebugFileDirectory = Settings.Default.DefaultDebugDirectory,
			};
		}

		#region Initialization Related Event Handler

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			App.Current.ProgressLogger.Completed += ProgressLogger_Completed;
		}
		private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
		{
			App.Current.ProgressLogger.Completed -= ProgressLogger_Completed;
		}

		#endregion

		#region Data Properties

		public static readonly DependencyProperty CharSetFontMappingsProperty = DependencyProperty.Register(nameof(CharSetFontMappings), typeof(ObservableCollection<CharSetFontMapping>), typeof(MainWindow), new FrameworkPropertyMetadata(null));

		public ObservableCollection<CharSetFontMapping> CharSetFontMappings
		{
			get => (ObservableCollection<CharSetFontMapping>)GetValue(CharSetFontMappingsProperty);
			set => SetValue(CharSetFontMappingsProperty, value);
		}

		public static readonly DependencyProperty GenerationSettingProperty = DependencyProperty.Register(nameof(GenerationSetting), typeof(GenerationSetting), typeof(MainWindow), new FrameworkPropertyMetadata(default(GenerationSetting)));

		public GenerationSetting GenerationSetting
		{
			get => (GenerationSetting)GetValue(GenerationSettingProperty);
			set => SetValue(GenerationSettingProperty, value);
		}

		#endregion

		#region Working Events

		private void OpenOutputDirButton_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(GenerationSetting.OutputDirectory);
			}
			catch (Exception ex)
			{
				this.ShowError(this.FindResString("OpenOutputDirectoryErrorTitle"),
					this.FindResString("OpenOutputDirectoryErrorDetail"), ex);
			}
		}

		private void StartButton_OnClick(object sender, RoutedEventArgs e)
		{
			// Error detection
			if (CharSetFontMappings.Count == 0)
			{
				this.ShowMessage(this.FindResString("AppName"), this.FormatResString("NoMappingIsSetErrorTitle"),
					this.FormatResString("NoMappingIsSetErrorDetail"), TaskDialogStandardIcon.Error, TaskDialogStandardButtons.Ok);
				return;
			}

			if (CharSetFontMappings.Any(item => item.Font == null || item.CharSet == null))
			{
				this.ShowMessage(this.FindResString("AppName"), this.FormatResString("MappingInvalidErrorTitle"),
					this.FormatResString("MappingInvalidErrorDetail"), TaskDialogStandardIcon.Error, TaskDialogStandardButtons.Ok);

				return;
			}

			this.GoToElementState(WorkingState, true);
			LogTab.IsSelected = true;

			var generator = new FontGenerator(App.Current.ProgressLogger);
			generator.GenerateAsync(CharSetFontMappings, GenerationSetting);
		}

		private void ProgressLogger_Completed(object sender, EventArgs e)
		{
			this.GoToElementState(NormalState, true);
		}

		#endregion


	}
}
