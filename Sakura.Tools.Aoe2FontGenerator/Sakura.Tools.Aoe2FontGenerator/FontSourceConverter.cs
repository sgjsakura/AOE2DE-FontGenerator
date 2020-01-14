using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sakura.Tools.Aoe2FontGenerator
{
	[ValueConversion(typeof(FontSource), typeof(string))]
	public class FontSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case SystemFontSource systemFontSource when systemFontSource.FontInfo != null:
					return string.Format(CultureInfo.CurrentUICulture, "System font - {0}",
						new FontInfoConverter().Convert(systemFontSource.FontInfo, culture));
				case FileFontSource fileFontSource when fileFontSource.FontFilePath != null:
					return string.Format(CultureInfo.CurrentUICulture, "Font file - {0}", fileFontSource.FontFilePath);
				default:
					return "Not specified";
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
