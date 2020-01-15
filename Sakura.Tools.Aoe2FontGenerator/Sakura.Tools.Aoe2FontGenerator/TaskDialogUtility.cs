using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Provide
	/// </summary>
	public static class TaskDialogUtility
	{
		/// <summary>
		/// Show a simple task dialog message box.
		/// </summary>
		/// <param name="element">The element used to determine the owner window.</param>
		/// <param name="caption">The message box caption.</param>
		/// <param name="instructionText">The instruction text in the message box.</param>
		/// <param name="text">The detailed text in the message box.</param>
		/// <param name="icon">The icon in the message box.</param>
		/// <param name="buttons">The buttons in the message box.</param>
		/// <returns>The dialog result user selected.</returns>
		public static TaskDialogResult ShowMessage(this FrameworkElement element, [Localizable(true)]string caption, [Localizable(true)]string instructionText, [Localizable(true)]string text, TaskDialogStandardIcon icon, TaskDialogStandardButtons buttons)
		{
			var dialog = new TaskDialog
			{
				Caption = caption,
				InstructionText = instructionText,
				Text = text,
				Icon = icon,
				StandardButtons = buttons,
				Cancelable = true,
				OwnerWindowHandle = Window.GetWindow(element).GetWindowHandle()
			};

			using (dialog)
			{
				return dialog.Show();
			}
		}

		/// <summary>
		/// Get the Win32 window handle for a WPF window instance.
		/// </summary>
		/// <param name="window">The WPF window instance.</param>
		/// <returns>The Win32 handle of the <paramref name="window"/>.</returns>
		public static IntPtr GetWindowHandle(this Window window)
		{
			var helper = new WindowInteropHelper(window);
			return helper.Handle;
		}
	}
}
