using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	///     Define an UI element contains both image and text.
	/// </summary>
	public class ImageItem : INotifyPropertyChanged
	{
		/// <summary>
		///     Backend field for the <see cref="Image" /> property.
		/// </summary>
		private ImageSource _image;

		/// <summary>
		///     Backend field for the <see cref="Text" /> property.
		/// </summary>
		private string _text;

		/// <summary>
		///     The image.
		/// </summary>
		public ImageSource Image
		{
			get => _image;
			set
			{
				if (Equals(value, _image)) return;
				_image = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     The text.
		/// </summary>
		[Localizability(LocalizationCategory.Title, Readability = Readability.Readable,
			Modifiability = Modifiability.Modifiable)]
		[Localizable(true)]
		public string Text
		{
			get => _text;
			set
			{
				if (value == _text) return;
				_text = value;
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