using System;
using System.Globalization;
using System.Windows.Data;
using Sakura.Tools.Aoe2FontGenerator.Utilities;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator.Converters
{
	/// <summary>
	/// Convert <see cref="FontInfo"/> into display string.
	/// </summary>
	[ValueConversion(typeof(FontInfo), typeof(string))]
	public class FontInfoConverter : IValueConverter
	{
		/// <summary>
		/// Core method used for conversion.
		/// </summary>
		/// <param name="value">The <see cref="FontInfo"/> to be converted.</param>
		/// <param name="cultureInfo">The culture info.</param>
		/// <returns>The converted string value.</returns>
		public static string Convert(FontInfo value, CultureInfo cultureInfo)
		{
			return App.Current.FormatResString("FontInfoConverterFormat",
				value.Family.FamilyNames[cultureInfo.ToXmlLanguage()], value.Size, value.Style.ToString(),
				value.Weight.ToString());
		}

		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is FontInfo realValue)
			{
				return Convert(realValue, culture);
			}

			return null;
		}

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
