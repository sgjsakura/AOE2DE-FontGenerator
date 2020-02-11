using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Models
{
	public class GenerationSetting : INotifyPropertyChanged
	{
		private string _metadataFileName;
		private string _outputDirectory;
		private string _surfaceFileNameFormat;
		private bool _outputDebugFile;
		private int _textureSize;
		private int _glyphSize;
		private int _glyphSpace;
		private string _debugFileDirectory;

		/// <summary>
		/// Get or set the target output directory.
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
		/// Get or set a value that indicates whether some debug purpose files should also be generated during the generation process.
		/// </summary>
		public bool OutputDebugFile
		{
			get => _outputDebugFile;
			set
			{
				if (value == _outputDebugFile) return;
				_outputDebugFile = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Get or set the directory path should be used to save the debug file.
		/// </summary>
		public string DebugFileDirectory
		{
			get => _debugFileDirectory;
			set
			{
				if (value == _debugFileDirectory) return;
				_debugFileDirectory = value;
				OnPropertyChanged();
			}
		}

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

		#region  NPC Support

		/// <summary>
		/// Raises when property is changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
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
