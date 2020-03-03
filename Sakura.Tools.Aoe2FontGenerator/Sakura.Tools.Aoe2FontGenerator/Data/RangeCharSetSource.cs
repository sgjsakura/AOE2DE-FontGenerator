using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	/// Represents as a char set source with is specified by the Unicode code print range.
	/// </summary>
	[Serializable]
	public class RangeCharSetSource : CharSetSource
	{
		/// <summary>
		///     backend field for the <see cref="Range" /> property.
		/// </summary>
		private string _range;

		/// <summary>
		///     Get or set the range setting string.
		/// </summary>
		public string Range
		{
			get => _range;
			set
			{
				if (value == _range) return;
				_range = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get the actual range value from the <paramref name="range" /> setting string.
		/// </summary>
		/// <param name="range">The range string.</param>
		/// <returns>A collection contains all valid range segments.</returns>
		/// <exception cref="FormatException">The <see cref="Range" /> string is invalid.</exception>
		private static IEnumerable<(int Min, int Max)> GetRangeValue(string range)
		{
			if (string.IsNullOrEmpty(range)) yield break;

			var items = range.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in items)
			{
				// ###-### range format
				var match = Regex.Match(item, @"^(\d+)\-(\d+)$",
					RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

				if (match.Success)
				{
					var min = int.Parse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.CurrentCulture);
					var max = int.Parse(match.Groups[2].Value, NumberStyles.Integer, CultureInfo.CurrentCulture);

					yield return (min, max);
					continue;
				}

				// ### single value format
				var value = int.Parse(item, NumberStyles.Integer, CultureInfo.CurrentCulture);
				yield return (value, value);
			}
		}

		/// <summary>
		///     Determine if a value is in the specified range.
		/// </summary>
		/// <param name="ranges">A collection contains all valid range segments.</param>
		/// <param name="value">The value to be determining.</param>
		/// <returns>
		///     If the <paramref name="value" /> is in the <paramref name="ranges" />, returns <c>true</c>; otherwise, returns
		///     <c>false</c>.
		/// </returns>
		private static bool IsInRange(IEnumerable<(int Min, int Max)> ranges, int value)
		{
			foreach (var (min, max) in ranges)
				if (value >= min && value <= max)
					return true;

			return false;
		}

		public override IEnumerable<int> GetValidCodePrints(IEnumerable<int> fontCodePrints)
		{
			try
			{
				var ranges = GetRangeValue(Range).ToArray();
				return fontCodePrints.Where(i => IsInRange(ranges, i));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(App.Current.FormatResString("RangeCharSetErrorMessage", Range), ex);
			}
		}
	}
}