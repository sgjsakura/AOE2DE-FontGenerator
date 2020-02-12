using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sakura.Tools.Aoe2FontGenerator.Data;
using Sakura.Tools.Aoe2FontGenerator.Utilities;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	///     FontSourceSelector.xaml 的交互逻辑
	/// </summary>
	public partial class FontSourceSelector : UserControl
	{
		public static readonly DependencyProperty ActiveFontSourceProperty =
			DependencyProperty.Register(nameof(ActiveFontSource), typeof(FontSource), typeof(FontSourceSelector),
				new FrameworkPropertyMetadata(null, OnActiveFontSourceChanged));

		private static readonly DependencyPropertyKey FileFontSourcePropertyKey = DependencyProperty.RegisterReadOnly(
			nameof(FileFontSource), typeof(FileFontSource), typeof(FontSourceSelector),
			new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty FileFontSourceProperty = FileFontSourcePropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey SystemFontSourcePropertyKey =
			DependencyProperty.RegisterReadOnly(nameof(SystemFontSource), typeof(SystemFontSource),
				typeof(FontSourceSelector), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty SystemFontSourceProperty =
			SystemFontSourcePropertyKey.DependencyProperty;

		public FontSourceSelector()
		{
			InitializeComponent();
			SystemFontSource = new SystemFontSource();
			FileFontSource = new FileFontSource();
		}

		public FontSource ActiveFontSource
		{
			get => (FontSource) GetValue(ActiveFontSourceProperty);
			set => SetValue(ActiveFontSourceProperty, value);
		}

		public FileFontSource FileFontSource
		{
			get => (FileFontSource) GetValue(FileFontSourceProperty);
			private set => SetValue(FileFontSourcePropertyKey, value);
		}

		public SystemFontSource SystemFontSource
		{
			get => (SystemFontSource) GetValue(SystemFontSourceProperty);
			private set => SetValue(SystemFontSourcePropertyKey, value);
		}

		private void BrowseFontFileButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("FontFilesFilter"), "*.ttf; *.otf"));
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("AllFilesFilter"), "*.*"));

			if (dialog.ShowDialog() == CommonFileDialogResult.Ok) FileFontSource.FontFilePath = dialog.FileName;
		}

		private void BrowseSystemFontButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new ColorFontDialog(showColorPicker: false);
			dialog.InitializeComponent();
			dialog.Font = SystemFontSource.FontInfo ?? FontInfo.GetControlFont(this);

			if (dialog.ShowDialog() == true) SystemFontSource.FontInfo = dialog.Font;
		}

		private static void OnActiveFontSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var oldValue = (FontSource) e.OldValue;
			var newValue = (FontSource) e.NewValue;

			if (Equals(oldValue, newValue)) return;

			var target = (FontSourceSelector) d;
			target.SyncFontSource();
		}

		/// <summary>
		///     Update any necessary information whenever <see cref="ActiveFontSource" /> is changed.
		/// </summary>
		private void SyncFontSource()
		{
			switch (ActiveFontSource)
			{
				case SystemFontSource systemFontSource:
					SystemFontSource = systemFontSource;
					SystemFontRadioButton.IsChecked = true;
					break;
				case FileFontSource fileFontSource:
					FileFontSource = fileFontSource;
					FileFontRadioButton.IsChecked = true;
					break;
				case null:
					SystemFontRadioButton.IsChecked = false;
					FileFontRadioButton.IsChecked = false;
					break;
			}
		}

		#region Event Handler

		private void SystemFontRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			ActiveFontSource = SystemFontSource;
		}


		private void FileFontRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			ActiveFontSource = FileFontSource;
		}

		#endregion
	}
}