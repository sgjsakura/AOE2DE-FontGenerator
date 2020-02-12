using System;
using System.Globalization;
using System.Windows.Data;

namespace Sakura.Tools.Aoe2FontGenerator.Converters
{
	/// <summary>
	///     Used to convert a <see cref="double" /> value to <see cref="bool" /> and indicates if the source value is
	///     <see cref="double.NaN" />.
	/// </summary>
	/// <inheritdoc />
	[ValueConversion(typeof(double), typeof(bool))]
	public class DoubleIsNaNConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is double realValue ? (object) Convert(realValue) : null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     Core method for conversion.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		/// <returns>The converted result.</returns>
		public static bool Convert(double value)
		{
			return double.IsNaN(value);
		}
	}
}