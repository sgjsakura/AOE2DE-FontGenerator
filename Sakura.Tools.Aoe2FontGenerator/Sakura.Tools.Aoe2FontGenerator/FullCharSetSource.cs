using System.Collections.Generic;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Represent as a full charset source, which covers all supported unicode code print range (0-65535).
	/// </summary>
	public class FullCharSetSource : CharSetSource
	{
		/// <inheritdoc />
		public override IEnumerable<int> GetCodePrints()
		{
			for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
			{
				yield return i;
			}
		}
	}
}