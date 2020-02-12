namespace Sakura.Tools.Aoe2FontGenerator.Models
{
	/// <summary>
	///     Represents as a single char glyph in the atlas.
	/// </summary>
	public class GlyphInfo
	{
		/// <summary>
		///     The code print of the glyph.
		/// </summary>
		public ushort CodePrint { get; set; }

		/// <summary>
		///     Glyph width in pixel.
		/// </summary>
		public float W { get; set; }

		/// <summary>
		///     Glyph height in pixel.
		/// </summary>
		public float H { get; set; }

		/// <summary>
		///     Glyph start X position from the left of texture in ratio.
		/// </summary>
		public float U { get; set; }

		/// <summary>
		///     Glyph end X position from the left of texture in ratio.
		/// </summary>
		public float S { get; set; }

		/// <summary>
		///     Glyph start Y position from the top of texture in ratio.
		/// </summary>
		public float V { get; set; }

		/// <summary>
		///     Glyph end Y position from the top of texture in ratio.
		/// </summary>
		public float T { get; set; }

		/// <summary>
		///     The texture page number the glyph exists on.
		/// </summary>
		public int Atlas { get; set; }

		/// <summary>
		///     The amount pushing the glyph to the right to align.
		/// </summary>
		public float X0 { get; set; }

		/// <summary>
		///     The amount pushing the glyph down to align.
		/// </summary>
		public float Y0 { get; set; }

		/// <summary>
		///     The real width of the glyph i.e. room until the next glyph in pixel.
		/// </summary>
		public float HAdvance { get; set; }
	}
}