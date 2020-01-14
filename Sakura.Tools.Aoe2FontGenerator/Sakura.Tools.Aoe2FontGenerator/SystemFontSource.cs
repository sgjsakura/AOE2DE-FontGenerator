using System;
using System.Windows;
using System.Windows.Media;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Using a font provided by the current system.
	/// </summary>
	public class SystemFontSource : FontSource
	{
		private FontInfo _fontInfo;

		public FontInfo FontInfo
		{
			get => _fontInfo;
			set
			{
				if (Equals(value, _fontInfo)) return;
				_fontInfo = value;
				OnPropertyChanged();
			}
		}

		public override GlyphTypeface GetGlyphTypeface()
		{
			var typeface = new Typeface(FontInfo.Family, FontInfo.Style, FontInfo.Weight, FontInfo.Stretch);
			if (typeface.TryGetGlyphTypeface(out var result))
			{
				return result;
			}

			throw new InvalidOperationException("Cannot get typeface from the font information. Are you using a composed font?");
		}
	}
}