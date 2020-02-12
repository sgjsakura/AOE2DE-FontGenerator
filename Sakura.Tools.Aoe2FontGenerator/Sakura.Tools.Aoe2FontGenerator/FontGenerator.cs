using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;
using Sakura.Tools.Aoe2FontGenerator.Loggers;
using Sakura.Tools.Aoe2FontGenerator.Models;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	///     Core class used to calculate and generate font glyph atlas.
	/// </summary>
	public class FontGenerator
	{
		/// <summary>
		///     Initialize a new instance of <see cref="FontGenerator" />.
		/// </summary>
		/// <param name="logger">The <see cref="ProgressLogger" /> instance used to generate logs.</param>
		public FontGenerator([NotNull] ProgressLogger logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			Logger.DoWork += Logger_DoWork;
		}

		#region Internal Service Instance

		/// <summary>
		///     The <see cref="ProgressLogger" /> instance used to generate logs and report progresses.
		/// </summary>
		private ProgressLogger Logger { get; }

		#endregion

		#region Worker Event Handler

		private void Logger_DoWork(object sender, EventArgs e)
		{
			// When DoWork is called, calling the generation core method in a separate thread.
			GenerateCore();
		}

		#endregion

		#region Internal Classes

		/// <summary>
		///     Represents as a single mapping item. This class is used for data exchanging between <see cref="ExtractChars" /> and
		///     <see cref="DrawingGlyphs" />.
		/// </summary>
		private class GlyphMappingInfo
		{
			/// <summary>
			///     The typeface of the glyph.
			/// </summary>
			public GlyphTypeface Typeface { get; set; }

			/// <summary>
			///     The mapping setting.
			/// </summary>
			public MappingSetting Setting { get; set; }

			/// <summary>
			///     The code print of the glyph.
			/// </summary>
			public int CodePrint { get; set; }
		}

		#endregion

		#region Data Properties

		/// <summary>
		///     The mapping data.
		/// </summary>
		private CharSetFontMapping[] Mappings { get; set; }

		/// <summary>
		///     The setting.
		/// </summary>
		private GenerationSetting Setting { get; set; }

		#endregion

		#region Entry Point Methods

		/// <summary>
		///     Core method used to generate glyph atlas.
		/// </summary>
		/// <param name="mappings">A collection which contains all mapping settings.</param>
		/// <param name="setting">The setting for generation.</param>
		public void Generate(IEnumerable<CharSetFontMapping> mappings, GenerationSetting setting)
		{
			if (Logger.IsWorking) throw new InvalidOperationException(App.Current.FormatResString("WorkerIsBusyError"));

			Mappings = mappings.ToArray();
			Setting = setting;

			Logger.StartAsync();
		}

		/// <summary>
		///     Core method for atlas generation.
		/// </summary>
		private void GenerateCore()
		{
			try
			{
				Logger.Log(LogLevel.Info, App.Current.FormatResString("GenerationStarted"));

				// 2 phases
				Logger.InitializeProgress(2);

				// General phase info
				Logger.SetProgressLevel(0, App.Current.FormatResString("TotalProgressTitle"), 100);

				var glyphMappings = ExtractChars();
				var (visuals, boxInfo) = DrawingGlyphs(glyphMappings);
				SaveAllFiles(visuals, boxInfo);

				Logger.Log(LogLevel.Success, App.Current.FormatResString("GenerationEnded"));
			}
			catch (Exception ex)
			{
				Logger.Log(LogLevel.Critical, App.Current.FormatResString("DoWorkFatalErrorMessage", ex.Message));
			}
		}

		#endregion

		#region Core Methods

		/// <summary>
		///     Extract all chars from the <see cref="Mappings" />.
		/// </summary>
		/// <returns>A collection which contains all glyph information.</returns>
		private IReadOnlyList<GlyphMappingInfo> ExtractChars()
		{
			// Log 
			Logger.Log(LogLevel.Debug, App.Current.FormatResString("BeginExtractChars"));

			// Advance total progress for 10%
			Logger.AdvanceProgressStage(0, 10);
			Logger.SetProgressLevel(1, App.Current.FormatResString("BeginExtractCharsTitle"), Mappings.Length);

			var result = new SortedDictionary<int, GlyphMappingInfo>();

			foreach (var mapping in Mappings)
			{
				var typeface = mapping.Font.GetGlyphTypeface();

				var typefaceChars = typeface.CharacterToGlyphMap.Keys;
				var chars = mapping.CharSet.GetValidCodePrints(typefaceChars).ToArray();
				var validChars = chars.Where(i => i <= ushort.MaxValue).ToArray();

				// Warning if invalid chars exist.
				if (validChars.Length != chars.Length)
					Logger.Log(LogLevel.Warning,
						App.Current.FormatResString("CharCodePrintOutOfRangeWarning",
							chars.Length - validChars.Length));

				// Add all chars
				foreach (var c in validChars)
					if (!result.ContainsKey(c))
						result.Add(c, new GlyphMappingInfo
						{
							CodePrint = c,
							Typeface = typeface,
							Setting = mapping.Setting
						});

				Logger.AdvanceCurrentStageProgress(1);
			}

			Logger.Log(LogLevel.Success, App.Current.FormatResString("ExtractCharsFinished", result.Count));
			return result.Values.ToArray();
		}

		/// <summary>
		///     Drawing all glyphs and return the drawn visual and char information.
		/// </summary>
		/// <param name="glyphs">The collection of all glyphs.</param>
		/// <returns>The drawn visual list and box information.</returns>
		private (IReadOnlyList<Visual> Visuals, IReadOnlyList<GlyphInfo> BoxInfo) DrawingGlyphs(
			IReadOnlyCollection<GlyphMappingInfo> glyphs)
		{
			DrawingVisual currentVisual;
			DrawingContext drawingContext;

			var visualList = new List<Visual>();
			var charInfoList = new List<GlyphInfo>();

			#region Help methods

			void CloseCurrentVisual()
			{
				drawingContext.Close();
				visualList.Add(currentVisual);
			}

			void OpenNewVisual()
			{
				currentVisual = new DrawingVisual();
				drawingContext = currentVisual.RenderOpen();
			}

			#endregion

			double atlasWidth = Setting.TextureSize;
			double atlasHeight = Setting.TextureSize;

			double space = Setting.GlyphSpace;

			double currentX = space, currentY = space, lineHeight = 0;

			// Log
			Logger.Log(LogLevel.Debug, App.Current.FormatResString("BeginDrawingGlyphs"));

			// Initialize progress
			Logger.AdvanceProgressStage(0, 40);
			Logger.SetProgressLevel(1, App.Current.FormatResString("BeginDrawingGlyphsTitle"), glyphs.Count);

			// Open visual
			OpenNewVisual();

			foreach (var g in glyphs)
			{
				var index = g.Typeface.CharacterToGlyphMap[g.CodePrint];

				// Get alignment data
				var advancedWidth = g.Typeface.AdvanceWidths[index];
				var leftSideBearing = g.Typeface.LeftSideBearings[index];

				// Calculate real size and baseline
				var emSize = Setting.GlyphSize * g.Setting.GlyphSizeRatio;
				var baseline = g.Typeface.Baseline + g.Setting.BaselineOffsetRatio * g.Typeface.Height;

				// Extract geometry and get outline size
				var geometry = g.Typeface.GetGlyphOutline(index, emSize, 0);

				var totalWidth = geometry.Bounds.Width;
				var totalHeight = geometry.Bounds.Height;

				// Handle empty (the size will be -inf by default)
				if (geometry.Bounds.IsEmpty)
				{
					totalWidth = 0;
					totalHeight = 0;
				}

				if (totalWidth > atlasWidth || totalHeight > atlasHeight)
					throw new InvalidOperationException(App.Current.FormatResString("GlyphSizeTooLargeErrorMessage",
						(char) g.CodePrint, g.Typeface.FamilyNames[CultureInfo.CurrentUICulture]));

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

					// Close and open visual
					CloseCurrentVisual();
					OpenNewVisual();
				}

				// Add char info for metadata
				var charInfo = new GlyphInfo
				{
					Atlas = visualList.Count,

					W = (float) totalWidth,
					H = (float) totalHeight,

					U = (float) (currentX / atlasWidth),
					V = (float) (currentY / atlasHeight),

					S = (float) ((currentX + totalWidth) / atlasWidth),
					T = (float) ((currentY + totalHeight) / atlasHeight),

					X0 = (float) (leftSideBearing * emSize),
					Y0 = (float) (geometry.Bounds.Top + baseline * emSize),

					HAdvance = (float) (advancedWidth * emSize),

					CodePrint = (ushort) g.CodePrint
				};

				charInfoList.Add(charInfo);

				// Apply transform and drawing chars

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
				if (totalHeight > lineHeight) lineHeight = totalHeight;

				// Advance progress
				Logger.AdvanceCurrentStageProgress(1);
			}

			// Close the final visual
			CloseCurrentVisual();

			Logger.Log(LogLevel.Success,
				App.Current.FormatResString("DrawingGlyphsFinished", visualList.Count, charInfoList.Count));
			return (visualList, charInfoList);
		}

		/// <summary>
		///     Try to save a drawn visual to the disk location.
		/// </summary>
		/// <param name="visual">The visual to be saved.</param>
		/// <param name="index">The index of the current visual.</param>
		private void SaveSurfaceImage(Visual visual, int index)
		{
			// Draw visual to BMP file and load as image source from memory
			var bitmap = new RenderTargetBitmap(Setting.TextureSize, Setting.TextureSize, 96, 96, PixelFormats.Pbgra32);
			bitmap.Render(visual);

			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bitmap));

			var fileName = string.Format(CultureInfo.InvariantCulture, Setting.SurfaceFileNameFormat, index);
			var saveFilePath = Path.Combine(Setting.OutputDirectory, Path.ChangeExtension(fileName, ".png"));

			using (var fileStream = File.Create(saveFilePath))
			{
				encoder.Save(fileStream);
				fileStream.Close();
			}

			TexConvUtility.ConvertTexture(saveFilePath, Setting.OutputDirectory);
		}

		/// <summary>
		///     Save the box file to the disk location.
		/// </summary>
		/// <param name="boxInfo">The box data.</param>
		/// <param name="pageCount">Total atlas count.</param>
		private void SaveBoxFile(IReadOnlyList<GlyphInfo> boxInfo, int pageCount)
		{
			Logger.Log(LogLevel.Debug, App.Current.FormatResString("BeginSaveMetadataFile"));

			// Update progress
			Logger.AdvanceProgressStage(0, 10);
			Logger.SetProgressLevel(1, App.Current.FormatResString("BeginSaveMetadataFileTitle"), double.NaN);

			using (var stream = File.Create(Path.Combine(Setting.OutputDirectory, Setting.MetadataFileName)))
			{
				var binaryWriter = new BinaryWriter(stream);

				// Version: 0x2
				binaryWriter.Write(0x2, Endianness.LittleEndian);
				// Glyph Count
				binaryWriter.Write(boxInfo.Count, Endianness.LittleEndian);
				// Page count
				binaryWriter.Write(pageCount, Endianness.LittleEndian);
				// Source font size
				binaryWriter.Write((float) Setting.GlyphSize);

				foreach (var c in boxInfo)
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

				for (var i = 0; i < boxInfo.Count; i++)
				{
					binaryWriter.Write(boxInfo[i].CodePrint, Endianness.LittleEndian);
					binaryWriter.Write((ushort) i, Endianness.LittleEndian);
				}
			}

			// Save debug file
			SaveDebugBoxTextFile(boxInfo, pageCount);

			Logger.Log(LogLevel.Debug, App.Current.FormatResString("SaveMetadataFileFinished"));
		}

		/// <summary>
		///     Save the debug text file for box metadata.
		/// </summary>
		/// <param name="boxInfo">The box data.</param>
		/// <param name="pageCount">Total atlas count.</param>
		private void SaveDebugBoxTextFile(IReadOnlyCollection<GlyphInfo> boxInfo, int pageCount)
		{
			using (var writer =
				File.CreateText(Path.Combine(Setting.OutputDirectory, string.Empty, $"{Setting.MetadataFileName}.txt")))
			{
				writer.WriteLine(App.Current.FormatResString("TextureSizeLineFormat", Setting.TextureSize));
				writer.WriteLine(App.Current.FormatResString("GlyphSizeLineFormat", Setting.GlyphSize));
				writer.WriteLine(App.Current.FormatResString("GlyphSpaceLineFormat", Setting.GlyphSpace));

				writer.WriteLine();

				writer.WriteLine(App.Current.FormatResString("CharCountLineFormat", boxInfo.Count));
				writer.WriteLine(App.Current.FormatResString("AtlasCountLineFormat", pageCount));

				writer.WriteLine();

				foreach (var c in boxInfo)
					writer.WriteLine(App.Current.FormatResString("CharInfoLineFormat",
						(char) c.CodePrint, c.W, c.H, c.U, c.V, c.S, c.T, c.Atlas, c.X0, c.Y0, c.HAdvance));

				writer.Close();
			}
		}

		/// <summary>
		///     Save all files.
		/// </summary>
		/// <param name="visuals">The visuals represent as DDS image.</param>
		/// <param name="boxInfo">The box data which contains all glyph information.</param>
		private void SaveAllFiles(IReadOnlyList<Visual> visuals, IReadOnlyList<GlyphInfo> boxInfo)
		{
			// Log
			Logger.Log(LogLevel.Debug, App.Current.FormatResString("BeginSaveImages", visuals.Count));

			// Set progress.
			Logger.AdvanceProgressStage(0, 40);
			Logger.SetProgressLevel(1, App.Current.FormatResString("BeginSaveImagesTitle"), visuals.Count);


			// Create directory before creating files.
			Directory.CreateDirectory(Setting.OutputDirectory);

			// Save all images
			for (var i = 0; i < visuals.Count; i++)
			{
				var v = visuals[i];
				SaveSurfaceImage(v, i);

				// Advance Stage
				Logger.AdvanceCurrentStageProgress(1);
			}

			Logger.Log(LogLevel.Success, App.Current.FormatResString("SaveImagesFinished", visuals.Count));

			SaveBoxFile(boxInfo, visuals.Count);
		}

		#endregion
	}
}