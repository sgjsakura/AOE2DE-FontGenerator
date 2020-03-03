using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Defines the charset source from a text file.
	/// </summary>
	[Serializable]
	public class FileCharSetSource : CharSetSource
	{
		/// <summary>
		///     Get or set the file path.
		/// </summary>
		public string FilePath { get; set; }

		public override IEnumerable<int> GetValidCodePrints(IEnumerable<int> fontCodePrints)
		{
			try
			{
				var text = File.ReadAllText(FilePath, Encoding.UTF8);
				var textChars = text.Select(i => (int) i);
				return fontCodePrints.Intersect(textChars);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(App.Current.FindResString("FileCharSetErrorMessage"), ex);
			}
		}
	}
}