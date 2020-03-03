using System;
using System.Windows.Media;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Using a font provided by a physical file.
	/// </summary>
	[Serializable]
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
			try
			{
				return new GlyphTypeface(new Uri(FontFilePath));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(App.Current.FindResString("FileFontErrorMessage"), ex);
			}
		}
	}
}