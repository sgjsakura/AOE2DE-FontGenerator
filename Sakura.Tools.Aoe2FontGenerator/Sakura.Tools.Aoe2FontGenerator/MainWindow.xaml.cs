using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
	///     The main window of this application.
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		///     Initialize a new instance of <see cref="MainWindow" />.
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
				OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			};
		}

		#region Initialization Related Event Handler

		private void MainWindow_OnInitialized(object sender, EventArgs e)
		{
			App.Current.ProgressLogger.Completed += ProgressLogger_Completed;
		}

		#endregion

		#region Data Properties

		public static readonly DependencyProperty CharSetFontMappingsProperty =
			DependencyProperty.Register(nameof(CharSetFontMappings), typeof(ObservableCollection<CharSetFontMapping>),
				typeof(MainWindow), new FrameworkPropertyMetadata(null));

		public ObservableCollection<CharSetFontMapping> CharSetFontMappings
		{
			get => (ObservableCollection<CharSetFontMapping>)GetValue(CharSetFontMappingsProperty);
			set => SetValue(CharSetFontMappingsProperty, value);
		}

		public static readonly DependencyProperty GenerationSettingProperty =
			DependencyProperty.Register(nameof(GenerationSetting), typeof(GenerationSetting), typeof(MainWindow),
				new FrameworkPropertyMetadata(default(GenerationSetting)));

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
					this.FormatResString("NoMappingIsSetErrorDetail"), TaskDialogStandardIcon.Error,
					TaskDialogStandardButtons.Ok);
				return;
			}

			if (CharSetFontMappings.Any(item => item.Font == null || item.CharSet == null))
			{
				this.ShowMessage(this.FindResString("AppName"), this.FormatResString("MappingInvalidErrorTitle"),
					this.FormatResString("MappingInvalidErrorDetail"), TaskDialogStandardIcon.Error,
					TaskDialogStandardButtons.Ok);

				return;
			}

			LayoutRoot.GoToElementState(WorkingState, true);
			LogTab.IsSelected = true;

			// Start generation
			App.Current.FontGenerator.Generate(CharSetFontMappings, GenerationSetting);
		}

		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonSaveFileDialog
			{
				AlwaysAppendDefaultExtension = true,
				DefaultExtension = ".aoe2fgp"
			};
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("ProjectFilesFilter"), "*.aoe2fgp"));
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("AllFilesFilter"), "*.*"));

			if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
			{
				return;
			}

			try
			{
				using var file = File.Create(dialog.FileName);
				var formatter = new BinaryFormatter();
				formatter.Serialize(file, CharSetFontMappings);
				formatter.Serialize(file, GenerationSetting);
			}
			catch (Exception ex)
			{
				this.ShowError(this.FindResString("CannotOpenProjectFileTitle"), this.FindResString("CannotOpenProjectFileDetail"), ex);
			}

		}

		private void LoadButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("ProjectFilesFilter"), "*.aoe2fgp"));
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("AllFilesFilter"), "*.*"));

			if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
			{
				return;
			}

			try
			{
				using var file = File.OpenRead(dialog.FileName);
				var formatter = new BinaryFormatter();
				CharSetFontMappings = (ObservableCollection<CharSetFontMapping>)formatter.Deserialize(file);
				GenerationSetting = (Models.GenerationSetting)formatter.Deserialize(file);
			}
			catch (Exception ex)
			{
				this.ShowError(this.FindResString("CannotSaveProjectFileTitle"), this.FindResString("CannotSaveProjectFileDetail"), ex);
			}
		}

		private void ProgressLogger_Completed(object sender, EventArgs e)
		{
			LayoutRoot.GoToElementState(NormalState, true);
		}

		#endregion

	}
}