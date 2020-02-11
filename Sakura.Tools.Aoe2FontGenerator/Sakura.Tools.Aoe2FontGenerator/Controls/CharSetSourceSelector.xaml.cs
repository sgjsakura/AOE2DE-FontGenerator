using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sakura.Tools.Aoe2FontGenerator.Data;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	/// Provides the UI for charset selection.
	/// </summary>
	public partial class CharSetSourceSelector : UserControl
	{
		/// <summary>
		/// Initialize a new instance of <see cref="CharSetSourceSelector"/>.
		/// </summary>
		public CharSetSourceSelector()
		{
			InitializeComponent();

			FullCharSetSource = new FullCharSetSource();
			RangeCharSetSource = new RangeCharSetSource();
			DirectCharSetSource = new DirectCharSetSource();
			FileCharSetSource = new FileCharSetSource();
		}

		public static readonly DependencyProperty ActiveCharSetSourceProperty = DependencyProperty.Register(nameof(ActiveCharSetSource), typeof(CharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(null, OnActiveCharSetSourceChanged));

		private static void OnActiveCharSetSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var oldValue = (CharSetSource)e.OldValue;
			var newValue = (CharSetSource)e.NewValue;

			if (Equals(oldValue, newValue))
			{
				return;
			}

			var target = (CharSetSourceSelector)d;
			target.SyncActiveCharSetSource();
		}

		/// <summary>
		/// Get the active <see cref="CharSetSource"/>.
		/// </summary>
		public CharSetSource ActiveCharSetSource
		{
			get => (CharSetSource)GetValue(ActiveCharSetSourceProperty);
			set => SetValue(ActiveCharSetSourceProperty, value);
		}

		#region Selection Related Data

		private static readonly DependencyPropertyKey FullCharSetSourcePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FullCharSetSource), typeof(FullCharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty FullCharSetSourceProperty =
			FullCharSetSourcePropertyKey.DependencyProperty;

		public FullCharSetSource FullCharSetSource
		{
			get => (FullCharSetSource)GetValue(FullCharSetSourceProperty);
			private set => SetValue(FullCharSetSourcePropertyKey, value);
		}

		private static readonly DependencyPropertyKey RangeCharSetSourcePropertyKey =
			DependencyProperty.RegisterReadOnly(nameof(RangeCharSetSource), typeof(RangeCharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(default(RangeCharSetSource)));

		public static readonly DependencyProperty RangeCharSetSourceProperty = RangeCharSetSourcePropertyKey.DependencyProperty;

		public RangeCharSetSource RangeCharSetSource
		{
			get => (RangeCharSetSource)GetValue(RangeCharSetSourceProperty);
			private set => SetValue(RangeCharSetSourcePropertyKey, value);
		}

		private static readonly DependencyPropertyKey DirectCharSetSourcePropertyKey =
			DependencyProperty.RegisterReadOnly(nameof(DirectCharSetSource), typeof(DirectCharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(default(DirectCharSetSource)));

		public static readonly DependencyProperty DirectCharSetSourceProperty = DirectCharSetSourcePropertyKey.DependencyProperty;

		public DirectCharSetSource DirectCharSetSource
		{
			get => (DirectCharSetSource)GetValue(DirectCharSetSourceProperty);
			private set => SetValue(DirectCharSetSourcePropertyKey, value);
		}

		private static readonly DependencyPropertyKey FileCharSetSourcePropertyKey =
			DependencyProperty.RegisterReadOnly(nameof(FileCharSetSource), typeof(FileCharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(default(FileCharSetSource)));

		public static readonly DependencyProperty FileCharSetSourceProperty = FileCharSetSourcePropertyKey.DependencyProperty;

		public FileCharSetSource FileCharSetSource
		{
			get => (FileCharSetSource)GetValue(FileCharSetSourceProperty);
			private set => SetValue(FileCharSetSourcePropertyKey, value);
		}

		#endregion

		/// <summary>
		/// Update related Control state with the <see cref="ActiveCharSetSource"/>.
		/// </summary>
		private void SyncActiveCharSetSource()
		{
			switch (ActiveCharSetSource)
			{
				case FullCharSetSource source:
					FullCharSetSource = source;
					FullCharSetRadioButton.IsChecked = true;
					break;
				case RangeCharSetSource source:
					RangeCharSetSource = source;
					RangeCharSetRadioButton.IsChecked = true;
					break;
				case DirectCharSetSource source:
					DirectCharSetSource = source;
					DirectCharSetRadioButton.IsChecked = true;
					break;
				case FileCharSetSource source:
					FileCharSetSource = source;
					FileCharSetRadioButton.IsChecked = true;
					break;
			}
		}


		private void BrowseTextFileButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog
			{
				CookieIdentifier = new Guid("{6F139568-E04A-43B5-8EF5-87B1FFD5BF6F}")
			};

			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("TextFilesFilter"), "*.txt"));
			dialog.Filters.Add(new CommonFileDialogFilter(this.FindResString("AllFilesFilter"), "*.*"));

			using (dialog)
			{
				if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					TextFilePathTextBox.Text = dialog.FileName;
				}
			}
		}

		private void CharSetSourceRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			var relatedCharSetSource = (CharSetSource)((RadioButton)sender).Tag;
			ActiveCharSetSource = relatedCharSetSource;
		}
	}
}
