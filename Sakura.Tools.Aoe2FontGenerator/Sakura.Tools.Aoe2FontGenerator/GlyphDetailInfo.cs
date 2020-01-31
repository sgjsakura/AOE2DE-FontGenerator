using System.Windows.Media;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Record all layout information for a glyph.
	/// </summary>
	public class GlyphDetailInfo
	{
		/// <summary>
		/// The glyph key in the typeface.
		/// </summary>
		public ushort Key { get; set; }

		/// <summary>
		/// The code print for this glyph.
		/// </summary>
		public int CodePrint { get; set; }

		/// <summary>
		/// The left-side bearing distance.
		/// </summary>
		public double LeftSideBearing { get; set; }
		/// <summary>
		/// The right-side bearing distance.
		/// </summary>
		public double RightSideBearing { get; set; }
		/// <summary>
		/// The top-side bearing distance.
		/// </summary>
		public double TopSideBearing { get; set; }
		/// <summary>
		/// The bottom-side bearing distance.
		/// </summary>
		public double BottomSideBearing { get; set; }

		/// <summary>
		/// The advance width.
		/// </summary>
		public double AdvanceWidth { get; set; }
		/// <summary>
		/// The advance height.
		/// </summary>
		public double AdvanceHeight { get; set; }

		/// <summary>
		/// The total width of the glyph bounding box.
		/// </summary>
		public double BoxWidth => LeftSideBearing + RightSideBearing + AdvanceWidth;

		/// <summary>
		/// The total height of the glyph bounding box.
		/// </summary>
		public double BoxHeight => TopSideBearing + BottomSideBearing + AdvanceHeight;

		/// <summary>
		/// The actual size in EM unit for this glyph.
		/// </summary>
		public double EmSize { get; set; }
		
		/// <summary>
		/// The corresponding <see cref="GlyphTypeface"/> for this glyph.
		/// </summary>
		public GlyphTypeface Typeface { get; set; }
	}
}