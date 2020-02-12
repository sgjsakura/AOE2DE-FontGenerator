using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Define the font source used to generate font atlas.
	/// </summary>
	public abstract class FontSource : INotifyPropertyChanged
	{
		/// <summary>
		///     When overriden in the derived classes, get the actual typeface instance represents this font source.
		/// </summary>
		/// <returns>The actual <see cref="GlyphTypeface" /> instance.</returns>
		public abstract GlyphTypeface GetGlyphTypeface();

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