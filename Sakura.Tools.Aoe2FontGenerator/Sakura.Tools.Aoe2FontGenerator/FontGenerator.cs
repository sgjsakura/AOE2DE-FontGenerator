using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using TeximpNet;
using Brushes = System.Windows.Media.Brushes;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Core class used to calculate and generate font glyph atlas.
	/// </summary>
	public class FontGenerator
	{

		public void Generate(IEnumerable<CharSetFontMapping> mappings)
		{
			double atlasWidth = 4096, atlasHeight = 4096;
			double pixelHeight = 64;
			double space = 8;

			var visualList = new List<Visual>();
			var charBoxList = new List<CharInfo>();

			var currentVisual = new DrawingVisual();
			var drawingContext = currentVisual.RenderOpen();

			double currentX = space, currentY = space, lineHeight = 0;

			foreach (var charSetFontMapping in mappings)
			{
				var typeface = charSetFontMapping.Font.GetGlyphTypeface();

				var emSize = pixelHeight / 1.5;
				var baseline = typeface.Baseline;

				// Get valid chars
				var fontChars = typeface.CharacterToGlyphMap.Keys;
				var validChars = charSetFontMapping.CharSet.GetValidCodePrints(fontChars);

				foreach (var c in validChars)
				{
					if (c > ushort.MaxValue)
					{
						Debug.WriteLine("字符 {0} 超过允许范围，无法显示，可能会覆盖的字是 {1}。", (char)c, (char)(c % 65536));
						continue;
					}

					var index = typeface.CharacterToGlyphMap[c];

					var advancedWidth = typeface.AdvanceWidths[index];
					var advancedHeight = typeface.AdvanceHeights[index];

					var leftSideBearing = typeface.LeftSideBearings[index];
					var rightSideBearing = typeface.RightSideBearings[index];

					var topSideBearing = typeface.TopSideBearings[index];
					var bottomSizeBearing = typeface.BottomSideBearings[index];

					var geometry = typeface.GetGlyphOutline(index, emSize, 0);

					//var totalWidth = (advancedWidth - leftSideBearing - rightSideBearing) * emSize;
					//var totalHeight = (advancedHeight - topSideBearing - bottomSizeBearing) * emSize;

					var totalWidth = geometry.Bounds.Width;
					var totalHeight = geometry.Bounds.Height;

					if (geometry.Bounds.IsEmpty)
					{
						totalWidth = 0;
						totalHeight = 0;
					}

					if (totalWidth > atlasWidth || totalHeight > atlasHeight)
					{
						throw new InvalidOperationException("Glyph is larger than the atlas.");
					}

					if (currentX + space + totalWidth <= atlasWidth && currentY + space + totalHeight <= atlasHeight)
					{
						// Current line, do nothing
					}
					else if (currentY + lineHeight + space + totalHeight <= atlasHeight)
					{
						// New line
						currentX = space;
						currentY += lineHeight + space;
						lineHeight = 0;
					}
					else
					{
						// new Atlas
						currentX = space;
						currentY = space;
						lineHeight = 0;

						// Close current
						drawingContext.Close();
						visualList.Add(currentVisual);

						// Open new drawing context
						currentVisual = new DrawingVisual();
						drawingContext = currentVisual.RenderOpen();
					}

					var charInfo = new CharInfo
					{
						Atlas = visualList.Count,

						W = (float)totalWidth,
						H = (float)totalHeight,

						U = (float)(currentX / atlasWidth),
						V = (float)(currentY / atlasHeight),

						S = (float)((currentX + totalWidth) / atlasWidth),
						T = (float)((currentY + totalHeight) / atlasHeight),

						X0 = (float)(leftSideBearing * emSize),
						Y0 = (float)(geometry.Bounds.Top + baseline * emSize),

						HAdvance = (float)(advancedWidth * emSize),

						CodePrint = (ushort)c,
					};

					charBoxList.Add(charInfo);

					var transform = new TransformGroup();

					// Move to baseline
					transform.Children.Add(new TranslateTransform(-geometry.Bounds.Left, -geometry.Bounds.Top));

					// Move to start location
					transform.Children.Add(new TranslateTransform(currentX, currentY));

					geometry.Transform = transform;

					// Draw
					drawingContext.DrawGeometry(Brushes.White, new Pen(Brushes.Black, 1), geometry);

					// Advance the start location
					currentX += totalWidth + space;

					// Update line height if necessary
					if (totalHeight > lineHeight)
					{
						lineHeight = totalHeight;
					}
				}
			}

			// Close the final drawing
			drawingContext.Close();
			visualList.Add(currentVisual);

			var atlasIndex = 0;

			var dir = @"d:\test";
			Directory.CreateDirectory(dir);

			foreach (var v in visualList)
			{
				var bitmap = new RenderTargetBitmap((int)atlasWidth, (int)atlasHeight, 96, 96, PixelFormats.Pbgra32);
				bitmap.Render(v);

				var encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmap));


				using (var memoryStream = new MemoryStream())
				{
					// Save image and set to start
					encoder.Save(memoryStream);
					memoryStream.Seek(0, SeekOrigin.Begin);

					using (var surface = Surface.LoadFromStream(memoryStream))
					{
						surface.SaveToFile(ImageFormat.PNG, Path.Combine(dir, $"combined_{atlasIndex:D4}.dds"));
						surface.SaveToFile(ImageFormat.PNG, Path.Combine(dir, $"combined_{atlasIndex:D4}.png"));
					}
				}

				atlasIndex++;
			}

			using (var boxFile = File.Create(Path.Combine(dir, @"combined.box")))
			{
				WriteCharInfo(charBoxList, visualList.Count, (float)pixelHeight, boxFile);
				boxFile.Close();
			}

			using (var boxTxt = File.Create(Path.Combine(dir, "combined.txt")))
			{
				var writer = new StreamWriter(boxTxt);

				writer.WriteLine("Char Count = {0}", charBoxList.Count);
				writer.WriteLine("Total Atlas = {0}", visualList.Count);
				writer.WriteLine("Source Font Size = {0}", pixelHeight);

				foreach (var c in charBoxList)
				{
					writer.WriteLine("Char {0} - W({1}), H({2}), U({3}), V({4}), S({5}), T({6}), Atlas({7}), X0({8}), Y0({9}), HA({10})", (char)c.CodePrint, c.W, c.H, c.U, c.V, c.S, c.T, c.Atlas, c.X0, c.Y0, c.HAdvance);
				}

				boxTxt.Close();
			}
		}

		private static double AddPositive(params double[] values) => values.Where(i => i >= 0).Sum();

		private static void WriteCharInfo(IList<CharInfo> charInfos, int pages, float sourceFontPixelSize, Stream stream)
		{
			var binaryWriter = new BinaryWriter(stream);

			// Version: 0x2
			binaryWriter.Write(0x2, Endianness.LittleEndian);
			// Glyph Count
			binaryWriter.Write(charInfos.Count, Endianness.LittleEndian);
			// Page count
			binaryWriter.Write(pages, Endianness.LittleEndian);
			// Source font size
			binaryWriter.Write(sourceFontPixelSize);

			foreach (var c in charInfos)
			{
				binaryWriter.Write(c.W);
				binaryWriter.Write(c.H);
				binaryWriter.Write(c.U);
				binaryWriter.Write(c.S);
				binaryWriter.Write(c.V);
				binaryWriter.Write(c.T);
				binaryWriter.Write(c.Atlas, Endianness.LittleEndian);
				binaryWriter.Write(c.X0);
				binaryWriter.Write(c.Y0);
				binaryWriter.Write(c.HAdvance);
			}

			for (var i = 0; i < charInfos.Count; i++)
			{
				binaryWriter.Write(charInfos[i].CodePrint, Endianness.LittleEndian);
				binaryWriter.Write((ushort)i, Endianness.LittleEndian);
			}
		}
	}
}
