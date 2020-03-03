using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Models
{
	[Serializable]
	public class GenerationSetting : INotifyPropertyChanged
	{
		private int _glyphSize;
		private int _glyphSpace;
		private string _metadataFileName;
		private string _outputDirectory;
		private string _surfaceFileNameFormat;
		private int _textureSize;

		/// <summary>
		///     Get or set the target output directory.
		/// </summary>
		public string OutputDirectory
		{
			get => _outputDirectory;
			set
			{
				if (value == _outputDirectory) return;
				_outputDirectory = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get or set the metadata file name.
		/// </summary>
		public string MetadataFileName
		{
			get => _metadataFileName;
			set
			{
				if (value == _metadataFileName) return;
				_metadataFileName = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get or set the surface file name format.
		/// </summary>
		public string SurfaceFileNameFormat
		{
			get => _surfaceFileNameFormat;
			set
			{
				if (value == _surfaceFileNameFormat) return;
				_surfaceFileNameFormat = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get or set the texture size.
		/// </summary>
		public int TextureSize
		{
			get => _textureSize;
			set
			{
				if (value == _textureSize) return;
				_textureSize = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get or set the glyph size.
		/// </summary>
		public int GlyphSize
		{
			get => _glyphSize;
			set
			{
				if (value == _glyphSize) return;
				_glyphSize = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Get or set the glyph space.
		/// </summary>
		public int GlyphSpace
		{
			get => _glyphSpace;
			set
			{
				if (value == _glyphSpace) return;
				_glyphSpace = value;
				OnPropertyChanged();
			}
		}

		#region NPC Support

		/// <summary>
		///     Raises when property is changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Raises the <see cref="PropertyChanged" /> event.
		/// </summary>
		/// <param name="propertyName">The changed property name. Default value is the caller member name.</param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}