using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sakura.Tools.Aoe2FontGenerator.Properties;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Provides ability to call texconv.exe. This class is static.
	/// </summary>
	public static class TexConvUtility
	{
		/// <summary>
		/// Gets the relative path for texconv.exe tools. this field is constant.
		/// </summary>
		public const string ToolRelativePath = @"Libs\texconv.exe";

		/// <summary>
		/// Get the absoulte path for texconv.exe. 
		/// </summary>
		public static string ToolFullPath => Path.Combine(Directory.GetCurrentDirectory(), ToolRelativePath);

		public static void ConvertTexture(string sourcePath, string outputDirectory)
		{
			var processStartInfo = new ProcessStartInfo
			{
				FileName = ToolFullPath,
				Arguments = string.Format(CultureInfo.InvariantCulture, Settings.Default.TexConvCommandLine, sourcePath, outputDirectory),
				CreateNoWindow = true,
				UseShellExecute = false
			};

			var process = Process.Start(processStartInfo);

			if (process == null)
			{
				throw new InvalidOperationException();
			}

			process.WaitForExit();
		}

		private static string HandleCommandArgument(string value) => $"\"{value}\"";
	}
}
