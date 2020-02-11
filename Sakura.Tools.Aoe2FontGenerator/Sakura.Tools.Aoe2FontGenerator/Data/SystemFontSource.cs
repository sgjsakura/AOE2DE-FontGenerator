using System;
using System.Windows.Media;
using Sakura.Tools.Aoe2FontGenerator.Utilities;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	/// Using a font provided by the current system.
	/// </summary>
	public class SystemFontSource : FontSource
	{
		/// <summary>
		/// Backend field of the <see cref="FontInfo"/> property.
		/// </summary>
		private FontInfo _fontInfo;

		/// <summary>
		/// Get or set the related <see cref="WpfColorFontDialog.FontInfo"/> instance.
		/// </summary>
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

		/// <summary>
		/// Initialize a new instance of <see cref="SystemFontSource"/>.
		/// </summary>
		public SystemFontSource()
		{

		}

		/// <summary>
		/// Initialize a new instance of <see cref="SystemFontSource"/> with the specified value.
		/// </summary>
		/// <param name="fontInfo">The new <see cref="WpfColorFontDialog.FontInfo"/>.</param>
		public SystemFontSource(FontInfo fontInfo)
		{
			FontInfo = fontInfo;
		}

		public override GlyphTypeface GetGlyphTypeface()
		{
			if (FontInfo == null)
			{
				throw new InvalidOperationException(App.Current.FindResString("SystemFontNullErrorMessage"));
			}

			var typeface = new Typeface(FontInfo.Family, FontInfo.Style, FontInfo.Weight, FontInfo.Stretch);
			if (typeface.TryGetGlyphTypeface(out var result))
			{
				return result;
			}

			throw new InvalidOperationException(App.Current.FindResString("SystemFontErrorMessage"));
		}
	}
}