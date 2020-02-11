using System.Collections.Generic;
using System.Linq;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	/// Defines the charset source from a string directly.
	/// </summary>
	public class DirectCharSetSource : CharSetSource
	{
		/// <summary>
		/// Backend field for <see cref="Chars"/> property.
		/// </summary>
		private string _chars;

		/// <summary>
		/// Get or set the string which contains all chars should be included in the source.
		/// </summary>
		public string Chars
		{
			get => _chars;
			set
			{
				if (value == _chars) return;
				_chars = value;
				OnPropertyChanged();
			}
		}

		public override IEnumerable<int> GetValidCodePrints(IEnumerable<int> fontCodePrints)
		{
			var codePrints = (Chars ?? string.Empty).Select(i => (int)i);
			return fontCodePrints.Intersect(codePrints);
		}
	}
}