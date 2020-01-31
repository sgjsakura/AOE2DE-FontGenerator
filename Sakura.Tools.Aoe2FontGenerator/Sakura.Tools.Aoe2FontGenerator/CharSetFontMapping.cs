using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Defines a charset and a specified rendering font.
	/// </summary>
	public class CharSetFontMapping : INotifyPropertyChanged
	{
		private FontSource _font;
		private CharSetSource _charSet;

		/// <summary>
		/// The mapping setting.
		/// </summary>
		public MappingSetting Setting { get; } = new MappingSetting();

		/// <summary>
		/// The charset to be drawn.
		/// </summary>
		public CharSetSource CharSet
		{
			get => _charSet;
			set
			{
				if (Equals(value, _charSet)) return;
				_charSet = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// The font used to draw all the chars.
		/// </summary>
		public FontSource Font
		{
			get => _font;
			set
			{
				if (Equals(value, _font)) return;
				_font = value;
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

	/// <summary>
	/// Represents as the additional setting for a <see cref="CharSetFontMapping"/>.
	/// </summary>
	public class MappingSetting
	{
		/// <summary>
		/// Value used to change the actual glyph size compared with the excepted glyph size.
		/// </summary>
		[Range(0, double.MaxValue)]
		public double GlyphSizeRatio { get; set; } = 1;

		/// <summary>
		/// Value used to adjust the baseline offset compared with the typeface height.
		/// </summary>
		[Range(-1.0, 1.0)]
		public double BaselineOffsetRatio { get; set; } = 0;
	}
}