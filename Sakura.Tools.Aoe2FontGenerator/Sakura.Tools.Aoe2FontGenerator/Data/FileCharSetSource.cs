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
		///     Backend field for <see cref="FilePath" /> property.
		/// </summary>
		private string _filePath;

		/// <summary>
		///     Get or set the string which contains path to text file with source chars.
		/// </summary>
		public string FilePath
		{
		    get => _filePath;
		    set
		    {
		        if (value == _filePath) return;
		        _filePath = value;
		        OnPropertyChanged();
		    }
		}

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