using System;
using System.Windows.Media;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Using a font provided by a physical file.
	/// </summary>
	public class FileFontSource : FontSource
	{
		private string _fontFilePath;

		public string FontFilePath
		{
			get => _fontFilePath;
			set
			{
				if (value == _fontFilePath) return;
				_fontFilePath = value;
				OnPropertyChanged();
			}
		}

		public override GlyphTypeface GetGlyphTypeface()
		{
			return new GlyphTypeface(new Uri(FontFilePath));
		}
	}
}