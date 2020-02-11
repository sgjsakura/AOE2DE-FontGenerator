using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Models
{
	/// <summary>
	/// Represents as the additional setting for a <see cref="CharSetFontMapping"/>.
	/// </summary>
	public class MappingSetting : INotifyPropertyChanged
	{
		/// <summary>
		/// Backend field for the <see cref="GlyphSizeRatio"/> property.
		/// </summary>
		private double _glyphSizeRatio = 1;

		/// <summary>
		/// Backend file for the <see cref="BaselineOffsetRatio"/> property.
		/// </summary>
		private double _baselineOffsetRatio;

		/// <summary>
		/// Value used to change the actual glyph size compared with the excepted glyph size.
		/// </summary>
		[Range(0, double.MaxValue)]
		public double GlyphSizeRatio
		{
			get => _glyphSizeRatio;
			set
			{
				if (value.Equals(_glyphSizeRatio)) return;
				_glyphSizeRatio = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Value used to adjust the baseline offset compared with the typeface height.
		/// </summary>
		[Range(-1.0, 1.0)]
		public double BaselineOffsetRatio
		{
			get => _baselineOffsetRatio;
			set
			{
				if (value.Equals(_baselineOffsetRatio)) return;
				_baselineOffsetRatio = value;
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