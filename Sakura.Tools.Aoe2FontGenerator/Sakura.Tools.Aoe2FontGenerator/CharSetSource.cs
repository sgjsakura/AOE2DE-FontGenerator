using System.Collections.Generic;
using System.Linq;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Define the charset which atlas should included.
	/// </summary>
	public abstract class CharSetSource
	{
		/// <summary>
		/// Get all the code prints included in the source.
		/// </summary>
		/// <returns>A collection of all code prints included in this source. If the source is empty, it returns an empty collection.</returns>
		public abstract IEnumerable<int> GetCodePrints();
	}
}