using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sakura.Tools.Aoe2FontGenerator.Models;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	/// CharSetMappingPage.xaml 的交互逻辑
	/// </summary>
	public partial class CharSetMappingPage : UserControl
	{
		public CharSetMappingPage()
		{
			InitializeComponent();
		}

		private void MainListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (MainListBox.SelectedValue is CharSetFontMapping item)
			{
				var window = new CharSetMappingWindow
				{
					Owner = Window.GetWindow(this),
					Data = item
				};

				window.ShowDialog();
			}
		}

		#region Data Properties

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<CharSetFontMapping>), typeof(CharSetMappingPage), new FrameworkPropertyMetadata(null));

		/// <summary>
		/// The related data of this control.
		/// </summary>
		public ObservableCollection<CharSetFontMapping> Data
		{
			get => (ObservableCollection<CharSetFontMapping>)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		#endregion

		#region Command Handlers

		private void AddCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Data != null;
		}

		private void AddCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Data?.Add(new CharSetFontMapping());
		}

		private void DeleteCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = MainListBox.SelectedIndex >= 0;
		}

		private void DeleteCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.ShowMessage(this.FindResString("AppName"), this.FormatResString("DeleteWarningTitle"),
					this.FormatResString("DeleteWarningDetail"), TaskDialogStandardIcon.Warning,
					TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No) !=
				TaskDialogResult.Yes)
			{
				return;
			}

			Data?.RemoveAt(MainListBox.SelectedIndex);
		}


		private void DeleteAllCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Data != null && Data.Count != 0;
		}

		private void DeleteAllCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.ShowMessage(this.FindResString("AppName"), this.FormatResString("ClearWarningTitle"), this.FormatResString("ClearWarningDetail"),
					TaskDialogStandardIcon.Warning, TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No) !=
				TaskDialogResult.Yes)
			{
				return;
			}

			Data?.Clear();
		}

		private void MoveUpCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = MainListBox.SelectedIndex > 0;
		}

		private void MoveUpCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var currentIndex = MainListBox.SelectedIndex;
			Data?.Move(currentIndex, currentIndex - 1);
		}

		private void MoveDownCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (Data == null)
			{
				e.CanExecute = false;
				return;
			}

			e.CanExecute = MainListBox.SelectedIndex >= 0 && MainListBox.SelectedIndex < Data.Count - 1;
		}

		private void MoveDownCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var currentIndex = MainListBox.SelectedIndex;
			Data?.Move(currentIndex, currentIndex + 1);
		}

		#endregion
	}
}
