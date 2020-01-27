using System.Collections.Generic;
using System.Windows.Media;

namespace Sakura.Tools.Aoe2FontGenerator
{
	public class TypefaceGlyphs
	{
		public GlyphTypeface Typeface { get; set; }
		public IDictionary<int, GlyphDetailInfo> GlyphInfos { get; set; }
	}
}