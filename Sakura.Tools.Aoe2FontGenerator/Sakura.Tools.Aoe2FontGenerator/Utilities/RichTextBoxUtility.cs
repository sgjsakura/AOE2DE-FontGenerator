using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sakura.Tools.Aoe2FontGenerator.Utilities
{
	/// <summary>
	///     Provide extensions method for <see cref="RichTextBox" />. This class is static.
	/// </summary>
	public static class RichTextBoxUtility
	{
		/// <summary>
		///     Append text with specified color to the target <see cref="RichTextBox" />.
		/// </summary>
		/// <param name="richTextBox">The <see cref="RichTextBox" /> control.</param>
		/// <param name="text">The text to be append.</param>
		/// <param name="color">The color of the text.</param>
		public static void AppendColoredText(this RichTextBox richTextBox, string text, Color color)
		{
			var range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd)
			{
				Text = string.Concat(text, Environment.NewLine)
			};

			range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
		}
	}
}