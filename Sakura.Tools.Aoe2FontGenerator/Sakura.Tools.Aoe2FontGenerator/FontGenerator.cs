using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Core class used to calculate and generate font glyph atlas.
	/// </summary>
	public class FontGenerator
	{
		public void Generate(IEnumerable<CharSetFontMapping> mappings)
		{
			foreach (var item in mappings)
			{


				var typeface = item.Font.GetGlyphTypeface();
				var codePrints = item.CharSet.GetCodePrints();

				foreach (var c in codePrints)
				{
					if (typeface.CharacterToGlyphMap.TryGetValue(c, out var index))
					{
					}
				}
			}
		}

		private void HandleSingleMapping(CharSetFontMapping mapping)
		{
			var codePrints = new SortedSet<int>(mapping.CharSet.GetCodePrints());
			var typeface = mapping.Font.GetGlyphTypeface();
			codePrints.IntersectWith(typeface.CharacterToGlyphMap.Keys);

			var glyphInfos = codePrints.Select(i => HandleSingleChar(i, typeface));
		}

		private GlyphInfo HandleSingleChar(int codePrint, GlyphTypeface typeface, double emSize)
		{
			var key = typeface.CharacterToGlyphMap[codePrint];

			return new GlyphInfo
			{
				CodePrint = codePrint,
				Width = typeface.GetTotalWidth(key),
				Height = typeface.GetTotalHeight(key),
				XOffset = typeface.LeftSideBearings[key],
				YOffset = typeface.TopSideBearings[key],
			};
		}
	}

	public static class TypefaceUtility
	{
		public static double GetTotalWidth(this GlyphTypeface typeface, ushort key) =>
			typeface.LeftSideBearings[key] + typeface.RightSideBearings[key] + typeface.AdvanceWidths[key];

		public static double GetTotalHeight(this GlyphTypeface typeface, ushort key) =>
			typeface.TopSideBearings[key] + typeface.BottomSideBearings[key] + typeface.AdvanceHeights[key];
	}

	public class GlyphInfo
	{
		public int CodePrint { get; set; }

		public 

		public double Width { get; set; }
		public double Height { get; set; }
		public double XOffset { get; set; }
		public double YOffset { get; set; }
	}
}
