using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using Sakura.Tools.Aoe2FontGenerator.Utilities;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Using a font provided by the current system.
	/// </summary>
	[Serializable]
	public class SystemFontSource : FontSource, ISerializable
	{
		/// <summary>
		///     Backend field of the <see cref="FontInfo" /> property.
		/// </summary>
		private FontInfo _fontInfo;

		/// <summary>
		///     Initialize a new instance of <see cref="SystemFontSource" />.
		/// </summary>
		public SystemFontSource()
		{
		}

		/// <summary>
		/// Construct the new <see cref="SystemFontSource"/> from the serialization context.
		/// </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The streaming context.</param>
		protected SystemFontSource(SerializationInfo info, StreamingContext context)
		{
			var hasFontInfo = info.GetBoolean(nameof(FontInfo));

			FontInfo = hasFontInfo ? ReadFromInfo(info) : null;
		}

		/// <summary>
		///     Initialize a new instance of <see cref="SystemFontSource" /> with the specified value.
		/// </summary>
		/// <param name="fontInfo">The new <see cref="WpfColorFontDialog.FontInfo" />.</param>
		public SystemFontSource(FontInfo fontInfo)
		{
			FontInfo = fontInfo;
		}

		/// <summary>
		///     Get or set the related <see cref="WpfColorFontDialog.FontInfo" /> instance.
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

		public override GlyphTypeface GetGlyphTypeface()
		{
			if (FontInfo == null)
				throw new InvalidOperationException(App.Current.FindResString("SystemFontNullErrorMessage"));

			var typeface = new Typeface(FontInfo.Family, FontInfo.Style, FontInfo.Weight, FontInfo.Stretch);
			if (typeface.TryGetGlyphTypeface(out var result)) return result;

			throw new InvalidOperationException(App.Current.FindResString("SystemFontErrorMessage"));
		}

		#region Serialization Helper Methods

		public static void WriteFontInfo(SerializationInfo info, FontInfo value)
		{
			info.AddValueWithConverter<FontFamilyConverter, string>(nameof(value.Family), value.Family);
			info.AddValueWithConverter<FontSizeConverter, string>(nameof(value.Size), value.Size);
			info.AddValueWithConverter<FontStyleConverter, string>(nameof(value.Style), value.Style);
			info.AddValueWithConverter<FontStretchConverter, string>(nameof(value.Stretch), value.Stretch);
			info.AddValueWithConverter<FontWeightConverter, string>(nameof(value.Weight), value.Weight);
			info.AddValueWithConverter<ColorConverter, string>(nameof(value.Color), value.BrushColor.Color);
		}

		public static FontInfo ReadFromInfo(SerializationInfo info)
		{
			try
			{
				return new FontInfo(
					info.GetValueWithConverter<FontFamilyConverter, string, FontFamily>(nameof(WpfColorFontDialog.FontInfo.Family)),
					info.GetValueWithConverter<FontSizeConverter, string, double>(nameof(WpfColorFontDialog.FontInfo.Size)),
					info.GetValueWithConverter<FontStyleConverter, string, FontStyle>(nameof(WpfColorFontDialog.FontInfo.Style)),
					info.GetValueWithConverter<FontStretchConverter, string, FontStretch>(nameof(WpfColorFontDialog.FontInfo.Stretch)),
					info.GetValueWithConverter<FontWeightConverter, string, FontWeight>(nameof(WpfColorFontDialog.FontInfo.Weight)),
					new SolidColorBrush(info.GetValueWithConverter<ColorConverter, string ,Color>(nameof(WpfColorFontDialog.FontInfo.Color)))
					);
			}
			catch (Exception)
			{
				return null;
			}
		}

		#endregion

		/// <summary>
		/// Customized serialization process.
		/// </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The streaming context.</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(FontInfo), FontInfo != null);

			if (FontInfo != null)
			{
				WriteFontInfo(info, FontInfo);
			}
		}
	}
}