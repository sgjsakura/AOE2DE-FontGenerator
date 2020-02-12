using System;
using System.Windows;
using System.Windows.Markup;

namespace Sakura.Tools.Aoe2FontGenerator.Converters
{
	/// <summary>
	///     Used by <see cref="TypeBasedDataTemplateSelector" /> in order to set the relationship between
	///     <see cref="System.Type" /> and <see cref="System.Windows.DataTemplate" />.
	/// </summary>
	[ContentProperty(nameof(DataTemplate))]
	public class TypeDataTemplatePair
	{
		/// <summary>
		///     Initialize an empty instance of <see cref="TypeDataTemplatePair" />.
		/// </summary>
		public TypeDataTemplatePair()
		{
		}

		/// <summary>
		///     Initialize an instance of <see cref="TypeDataTemplatePair" /> with the specified data.
		/// </summary>
		/// <param name="type">The value for <see cref="Type" />.</param>
		/// <param name="dataTemplate">The value for <see cref="DataTemplate" />.</param>
		public TypeDataTemplatePair(Type type, DataTemplate dataTemplate)
		{
			Type = type;
			DataTemplate = dataTemplate;
		}

		/// <summary>
		///     The <see cref="System.Type" /> value.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		///     The <see cref="System.Windows.DataTemplate" /> value.
		/// </summary>
		public DataTemplate DataTemplate { get; set; }
	}
}