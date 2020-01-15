using System;
using System.Collections.Generic;
using System.Linq;

namespace Sakura.Tools.Aoe2FontGenerator
{
	public class DirectCharSetSource : CharSetSource
	{
		public string Chars { get; set; }

		public override IEnumerable<int> GetCodePrints()
		{
			return Chars.Select(c => (int) c);
		}
	}
}