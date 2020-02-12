using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Data
{
	/// <summary>
	///     Define the charset which atlas should included.
	/// </summary>
	public abstract class CharSetSource : INotifyPropertyChanged
	{
		/// <summary>
		///     Get all the code prints included in the source.
		/// </summary>
		/// <returns>
		///     A collection of all code prints included in this source. If the source is empty, it returns an empty
		///     collection.
		/// </returns>
		public abstract IEnumerable<int> GetValidCodePrints(IEnumerable<int> fontCodePrints);

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