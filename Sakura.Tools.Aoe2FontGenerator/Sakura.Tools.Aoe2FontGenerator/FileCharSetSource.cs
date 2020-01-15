using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sakura.Tools.Aoe2FontGenerator
{
	public class FileCharSetSource : CharSetSource
	{
		public string FilePath { get; set; }

		public override IEnumerable<int> GetCodePrints()
		{
			var str = File.ReadAllText(FilePath, Encoding.UTF8);
			return str.Select(i => (int)i);
		}
	}
}