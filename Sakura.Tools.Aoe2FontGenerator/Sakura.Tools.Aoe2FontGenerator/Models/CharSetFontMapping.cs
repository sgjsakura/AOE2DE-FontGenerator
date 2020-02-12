using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Sakura.Tools.Aoe2FontGenerator.Data;

namespace Sakura.Tools.Aoe2FontGenerator.Models
{
	/// <summary>
	///     Defines a charset and a specified rendering font.
	/// </summary>
	public class CharSetFontMapping : INotifyPropertyChanged
	{
		/// <summary>
		///     Backend field for the <see cref="CharSet" /> property.
		/// </summary>
		private CharSetSource _charSet;

		/// <summary>
		///     Backend field for the <see cref="Font" /> property.
		/// </summary>
		private FontSource _font;

		/// <summary>
		///     Backend field for the <see cref="Setting" /> property.
		/// </summary>
		private MappingSetting _setting = new MappingSetting();

		/// <summary>
		///     The mapping setting.
		/// </summary>
		public MappingSetting Setting
		{
			get => _setting;
			set
			{
				if (Equals(value, _setting)) return;
				_setting = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     The charset to be drawn.
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
		///     The font used to draw all the chars.
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