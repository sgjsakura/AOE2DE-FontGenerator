using System.Collections.Generic;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Represent as a full charset source, which covers all supported unicode code print range (0-65535).
	/// </summary>
	public class FullCharSetSource : CharSetSource
	{
		/// <inheritdoc />
		public override IEnumerable<int> GetValidCodePrints(IEnumerable<int> fontCodePrints)
		{
			return fontCodePrints;
		}
	}
}