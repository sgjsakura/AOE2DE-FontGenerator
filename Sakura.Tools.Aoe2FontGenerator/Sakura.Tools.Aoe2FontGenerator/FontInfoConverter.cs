using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator
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
		/// <param name="value">The <see cref="FontInfo"/> to be convertedd.</param>
		/// <param name="cultureInfo">The culture info.</param>
		/// <returns></returns>
		public static string Convert(FontInfo value, CultureInfo cultureInfo)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} {1} {2} {3}",
				value.Family.FamilyNames[cultureInfo.ToXmlLanguage()],
				value.Size,
				value.Style.ToString(),
				value.Weight.ToString());
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is FontInfo realValue)
			{
				return Convert(realValue, culture);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
