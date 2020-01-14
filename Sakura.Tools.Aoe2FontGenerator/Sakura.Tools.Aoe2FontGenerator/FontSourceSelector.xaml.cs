using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// FontSourceSelector.xaml 的交互逻辑
	/// </summary>
	public partial class FontSourceSelector : UserControl
	{
		public FontSourceSelector()
		{
			InitializeComponent();
			SystemFontSource = new SystemFontSource();
			FileFontSource = new FileFontSource();
		}

		private void BrowseFontFileButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				FileName = FileFontSource.FontFilePath,
				Filter = "字体文件 (*.ttf; *.otf)|*.ttf; *.otf|所有文件 (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				FileFontSource.FontFilePath = dialog.FileName;
			}
		}

		private void BrowseSystemFontButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new ColorFontDialog(showColorPicker: false);
			dialog.InitializeComponent();
			dialog.Font = SystemFontSource.FontInfo ?? FontInfo.GetControlFont(this);

			if (dialog.ShowDialog() == true)
			{
				SystemFontSource.FontInfo = dialog.Font;
			}
		}

		#region Event Handler

		private void SystemFontRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			ActiveFontSource = SystemFontSource;
			SystemFontGrid.IsEnabled = true;
		}

		private void SystemFontRadioButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			SystemFontGrid.IsEnabled = false;
		}

		private void FontFileRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			ActiveFontSource = FileFontSource;
			FontFileGrid.IsEnabled = true;
		}

		private void FontFileRadioButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			FontFileGrid.IsEnabled = false;
		}

		#endregion

		public static readonly DependencyProperty ActiveFontSourceProperty = DependencyProperty.Register(nameof(ActiveFontSource), typeof(FontSource), typeof(FontSourceSelector), new FrameworkPropertyMetadata(null, OnActiveFontSourceChanged));

		private static void OnActiveFontSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var oldValue = (FontSource)e.OldValue;
			var newValue = (FontSource)e.NewValue;

			if (Equals(oldValue, newValue))
			{
				return;
			}

			var target = (FontSourceSelector)d;
			target.SyncFontSource();
		}

		public FontSource ActiveFontSource
		{
			get => (FontSource)GetValue(ActiveFontSourceProperty);
			set => SetValue(ActiveFontSourceProperty, value);
		}

		/// <summary>
		/// Update any necessary information whenever <see cref="ActiveFontSource"/> is changed.
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
					FontFileRadioButton.IsChecked = true;
					break;
				case null:
					SystemFontRadioButton.IsChecked = false;
					FontFileRadioButton.IsChecked = false;
					break;
				default:
					throw new InvalidOperationException("Not supported font source setting.");
			}
		}

		private static readonly DependencyPropertyKey FileFontSourcePropertyKey = DependencyProperty.RegisterReadOnly(
			nameof(FileFontSource), typeof(FileFontSource), typeof(FontSourceSelector), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty FileFontSourceProperty = FileFontSourcePropertyKey.DependencyProperty;

		public FileFontSource FileFontSource
		{
			get => (FileFontSource)GetValue(FileFontSourceProperty);
			private set => SetValue(FileFontSourcePropertyKey, value);
		}

		private static readonly DependencyPropertyKey SystemFontSourcePropertyKey = DependencyProperty.RegisterReadOnly(nameof(SystemFontSource), typeof(SystemFontSource), typeof(FontSourceSelector), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty SystemFontSourceProperty =
			SystemFontSourcePropertyKey.DependencyProperty;

		public SystemFontSource SystemFontSource
		{
			get => (SystemFontSource)GetValue(SystemFontSourceProperty);
			private set => SetValue(SystemFontSourcePropertyKey, value);
		}
	}
}
