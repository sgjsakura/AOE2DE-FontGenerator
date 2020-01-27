using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Core class used to calculate and generate font glyph atlas.
	/// </summary>
	public class FontGenerator
	{
		public void Generate(IEnumerable<CharSetFontMapping> mappings)
		{
			var layouts = mappings.Select(HandleSingleMapping);

			var visual = new DrawingVisual();
			var drawingContext = visual.RenderOpen();

			var totalWidth = 2048;
			var totalHeight = 2048;

			const double defaultDpi = 96;

			var layoutContext = new AtlasGenerateContext(totalWidth, totalHeight);

			foreach (var layout in layouts)
			{
				DrawGlyphList(layout, 1000, drawingContext, layoutContext);
			}

			drawingContext.Close();

			Window w;
			PresentationSource s = PresentationSource.FromVisual(w);

			var bitmap = new RenderTargetBitmap(totalWidth, totalHeight, defaultDpi, defaultDpi, PixelFormats.Pbgra32);
			bitmap.Render(visual);

			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bitmap));

			using (var file = File.OpenWrite(@"e:\1.png"))
			{
				encoder.Save(file);
				file.Close();
			}
		}

		/// <summary>
		/// Calcualte the box infomation for a glyph.
		/// </summary>
		/// <param name="info">The information aboue the glyph.</param>
		/// <param name="emSize">The EM-size used to calcuate the real glyph size information.</param>
		/// <returns>The calculated box information.</returns>
		private GlyphBoxInfo CalculateBox(GlyphDetailInfo info, double emSize)
		{
			double AddPositive(params double[] values) => values.Where(i => i >= 0).Sum();

			var result = new GlyphBoxInfo
			{
				TotalWidth = AddPositive(info.AdvanceWidth, info.LeftSideBearing, info.RightSideBearing) * emSize,
				TotalHeight = AddPositive(info.AdvanceHeight, info.TopSideBearing, info.BottomSideBearing) * emSize,
				XOffset = info.LeftSideBearing * emSize,
				YOffset = info.TopSideBearing * emSize,
				AdvanceWdith = info.AdvanceWidth * emSize,
				AdvanceHeight = info.AdvanceHeight * emSize,
			};

			return result;
		}

		/// <summary>
		/// Calculate the drawing location.
		/// </summary>
		/// <param name="boxInfo">The box information for a </param>
		/// <param name="generateContext"></param>
		/// <returns></returns>
		private (int AtlasIndex, Point Location) CalculateDrawLocation(GlyphBoxInfo boxInfo, AtlasGenerateContext generateContext)
		{

			if (generateContext.TotalWidth < boxInfo.TotalWidth || generateContext.TotalHeight < boxInfo.TotalHeight)
			{
				throw new InvalidOperationException("The size of atlas is less than the size of the glyph.");
			}

			// Can place the current glyph in the same line.
			if (generateContext.TotalWidth - generateContext.CurrentXStart >= boxInfo.TotalWidth && generateContext.TotalHeight - generateContext.CurrentYStart >= boxInfo.TotalHeight)
			{
				// Do nothing
			}
			// put on new line
			else if (generateContext.TotalHeight - generateContext.CurrentYStart >= boxInfo.TotalHeight)
			{
				generateContext.CurrentXStart = 0;
				generateContext.CurrentYStart += generateContext.LineHeight;
				generateContext.LineHeight = 0;
			}
			// new atlas
			else
			{
				generateContext.CurrentAtlasIndex++;
				generateContext.CurrentXStart = 0;
				generateContext.CurrentYStart = 0;
			}

			// Set draw location
			var drawLocation = new Point(generateContext.CurrentXStart, generateContext.CurrentYStart);

			// Update X location
			generateContext.CurrentXStart += boxInfo.TotalWidth;

			// Update line height if necessary
			if (generateContext.LineHeight < boxInfo.TotalHeight)
			{
				generateContext.LineHeight = boxInfo.TotalHeight;
			}

			return (generateContext.CurrentAtlasIndex, drawLocation);

			//return new CharInfo
			//{
			//	Atlas = generateContext.CurrentAtlasIndex,
			//	W = (float) boxInfo.TotalWidth,
			//	H = (float) boxInfo.TotalHeight,
			//	U = (float) (drawLocation.X / generateContext.TotalWidth),
			//	V = (float) (drawLocation.Y / generateContext.TotalHeight),
			//	S = (float) ((drawLocation.X + boxInfo.TotalWidth) / generateContext.TotalWidth),
			//	T = (float) ((drawLocation.Y + boxInfo.TotalHeight) / generateContext.TotalHeight),
			//	X0 = (float)boxInfo.XOffset,
			//	Y0 = (float)boxInfo.YOffset,
			//	HAdvance = (float)boxInfo.AdvanceWdith
			//};
		}

		private void DrawGlyphList(TypefaceGlyphs typefaceGlyphs, double size, DrawingContext context, AtlasGenerateContext layoutContext)
		{
			foreach (var glyphInfo in typefaceGlyphs.GlyphInfos)
			{
				DrawGlyph(typefaceGlyphs.Typeface, glyphInfo.Value, size, context, layoutContext);
			}
		}

		private void DrawGlyph(GlyphTypeface typeface, GlyphDetailInfo info, double size, DrawingContext drawingContext, AtlasGenerateContext layoutContext)
		{
			var boxInfo = CalculateBox(info, size);
			var drawLocation = CalculateDrawLocation(boxInfo, layoutContext);

			

			var g = typeface.GetGlyphOutline(info.Key, size, 0);

			var transform = new TransformGroup();
			transform.Children.Add(new TranslateTransform(0, typeface.Baseline * size));

			g.Transform = transform;
			Debug.Print("Drawing {0}, Bound = {1}", (char)info.CodePrint, g.Bounds);
			drawingContext.DrawGeometry(Brushes.Black, null, g);
		}

		/// <summary>
		/// Reteirve all the information of the actual typeface and related glyphs for a <see cref="CharSetFontMapping"/> setting. 
		/// </summary>
		/// <param name="mapping">The <see cref="CharSetFontMapping"/> instance.</param>
		/// <returns>The generated <see cref="TypefaceGlyphs"/> instance, which contains the typeface information and all glyph typefaceGlyphs.</returns>
		private TypefaceGlyphs HandleSingleMapping(CharSetFontMapping mapping)
		{
			var codePrints = new SortedSet<int>(mapping.CharSet.GetCodePrints());
			var typeface = mapping.Font.GetGlyphTypeface();
			codePrints.IntersectWith(typeface.CharacterToGlyphMap.Keys);

			var glyphLayouts = codePrints.Take(50).ToDictionary(i => i, i => HandleSingleChar(i, typeface));

			return new TypefaceGlyphs
			{
				Typeface = typeface,
				GlyphInfos = glyphLayouts
			};
		}

		private GlyphDetailInfo HandleSingleChar(int codePrint, GlyphTypeface typeface)
		{
			var key = typeface.CharacterToGlyphMap[codePrint];

			return new GlyphDetailInfo
			{
				Key = key,
				CodePrint = codePrint,
				TopSideBearing = typeface.TopSideBearings[key],
				BottomSideBearing = typeface.BottomSideBearings[key],
				LeftSideBearing = typeface.LeftSideBearings[key],
				RightSideBearing = typeface.RightSideBearings[key],
				AdvanceWidth = typeface.AdvanceWidths[key],
				AdvanceHeight = typeface.AdvanceHeights[key]
			};
		}
	}

	/// <summary>
	/// Internal context information used when generating the font atlas.
	/// </summary>
	internal class AtlasGenerateContext
	{
		/// <summary>
		/// The total width of the canvas.
		/// </summary>
		public double TotalWidth { get; }

		/// <summary>
		/// The total height of the canvas.
		/// </summary>
		public double TotalHeight { get; }

		/// <summary>
		/// The index of the current atlas, also indicates total atlases used.
		/// </summary>

		public int CurrentAtlasIndex { get; set; }

		/// <summary>
		/// The layout start X-location for the next glyph.
		/// </summary>
		public double CurrentXStart { get; set; }
		/// <summary>
		/// The layout start Y-Location for the next glyph.
		/// </summary>
		public double CurrentYStart { get; set; }

		/// <summary>
		/// The height of the current line. Used when needs to advance to next line.
		/// </summary>
		public double LineHeight { get; set; }

		/// <summary>
		/// Initialize a new instance of <see cref="AtlasGenerateContext"/> with specified canvas size.
		/// </summary>
		/// <param name="totalWidth">The total width of the canvas.</param>
		/// <param name="totalHeight">The total height of the canvas.</param>
		public AtlasGenerateContext(double totalWidth, double totalHeight)
		{
			TotalWidth = totalWidth;
			TotalHeight = totalHeight;
		}
	}

	public class GlyphBoxInfo
	{
		public double TotalWidth { get; set; }
		public double TotalHeight { get; set; }

		public double XOffset { get; set; }

		public double YOffset { get; set; }

		public double AdvanceWdith { get; set; }
		public double AdvanceHeight { get; set; }
	}

	/// <summary>
	/// Record the necessary information for drawing an atlas.
	/// </summary>
	public class GlyphDrawInfo
	{
		/// <summary>
		/// The index of the atlas.
		/// </summary>
		public int AtlasIndex { get; set; }

		/// <summary>
		/// The location of the glyph in the atlas.
		/// </summary>
		public Point Location { get; set; }
	}
}
