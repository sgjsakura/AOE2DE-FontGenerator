using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Markup;
using System.Windows.Media;
using JetBrains.Annotations;

namespace Sakura.Tools.Aoe2FontGenerator.Utilities
{
	/// <summary>
	///     Provide helper methods. This class is static.
	/// </summary>
	public static class Utility
	{
		/// <summary>
		///     Convert the <see cref="CultureInfo" /> instance to its corresponding <see cref="XmlLanguage" />.
		/// </summary>
		/// <param name="cultureInfo">The <see cref="CultureInfo" /> to be converted.</param>
		/// <returns>The <see cref="XmlLanguage" /> related with the <paramref name="cultureInfo" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="cultureInfo" /> is null.</exception>
		public static XmlLanguage ToXmlLanguage([NotNull] this CultureInfo cultureInfo)
		{
			if (cultureInfo is null) throw new ArgumentNullException(nameof(cultureInfo));

			return XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag);
		}

		/// <summary>
		///     Convert the <see cref="StringCollection" /> value came from the setting file into <see cref="DoubleCollection" />.
		/// </summary>
		/// <param name="strings">The value of <see cref="StringCollection" />, usually came from a setting file.</param>
		/// <returns>The converted <see cref="DoubleCollection" />.</returns>
		/// <remarks>Any invalid value items will be ignored.</remarks>
		public static DoubleCollection ToDoubleCollection(StringCollection strings)
		{
			var items = new List<double>();

			foreach (var str in strings)
				if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
					items.Add(d);

			return new DoubleCollection(items);
		}

		/// <summary>
		///     Add a series of items to the source collection.
		/// </summary>
		/// <typeparam name="T">The element type in the <paramref name="source" />.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="items">The items should be added.</param>
		public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
		{
			foreach (var item in items) source.Add(item);
		}

		/// <summary>
		///     Create a series of items using the same creator method.
		/// </summary>
		/// <typeparam name="T">The created object type.</typeparam>
		/// <param name="creator">The method used to creating each item.</param>
		/// <param name="count">The count should be created.</param>
		/// <returns>A <see cref="IEnumerable{T}" /> which represents as the created item sequence.</returns>
		public static IEnumerable<T> Repeat<T>(Func<T> creator, int count)
		{
			for (var i = 0; i < count; i++) yield return creator();
		}

		/// <summary>
		///     Get the message from the base exception of a specified exception instance.
		/// </summary>
		/// <param name="exception">The <see cref="Exception" /> instance.</param>
		/// <returns>The message came from the <paramref name="exception" />'s final base exception.</returns>
		public static string GetBaseMessage(this Exception exception)
		{
			return exception.GetBaseException().Message;
		}


		/// <summary>
		/// Deserialize a value from a serialization stream and convert it to the actual value.
		/// </summary>
		/// <typeparam name="TConverter">The type of the converter with is used to help convert source value to store values.</typeparam>
		/// <typeparam name="TStore">The store type with can be used to store data in the serialization stream.</typeparam>
		/// <typeparam name="TResult">The actual value type.</typeparam>
		/// <param name="info">The <see cref="SerializationInfo"/> instance.</param>
		/// <param name="name">The name of the value.</param>
		/// <returns>The converted actual value.</returns>
		public static TResult GetValueWithConverter<TConverter, TStore, TResult>(this SerializationInfo info, string name)
			where TConverter : TypeConverter, new()
		{
			var value = info.GetValue(name, typeof(TStore));
			var converter = new TConverter();
			return (TResult)converter.ConvertFrom(value);
		}

		/// <summary>
		/// Serialize a value into the serialization stream with the specified type converter.
		/// </summary>
		/// <typeparam name="TConverter">The type of the converter with is used to help convert source value to store values.</typeparam>
		/// <typeparam name="TStore">The store type with can be used to store data in the serialization stream.</typeparam>
		/// <param name="info">The <see cref="SerializationInfo"/> instance.</param>
		/// <param name="name">The name of the value.</param>
		/// <param name="value">The actual value.</param>
		public static void AddValueWithConverter<TConverter, TStore>(this SerializationInfo info, string name, object value)
		where TConverter : TypeConverter, new()
		{
			var converter = new TConverter();
			var realValue = converter.ConvertTo(value, typeof(TStore));
			info.AddValue(name, realValue);
		}
	}
}