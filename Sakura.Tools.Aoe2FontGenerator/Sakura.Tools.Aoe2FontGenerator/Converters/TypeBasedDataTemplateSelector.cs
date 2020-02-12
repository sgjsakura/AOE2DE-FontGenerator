using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Sakura.Tools.Aoe2FontGenerator.Converters
{
	/// <summary>
	///     Select the <see cref="DataTemplate" /> according to source content type.
	/// </summary>
	[ContentProperty(nameof(TypeDataTemplates))]
	public class TypeBasedDataTemplateSelector : DataTemplateSelector
	{
		/// <summary>
		///     A list of <see cref="Type" /> and <see cref="DataTemplate" /> pairs used to define the relationship between data
		///     type and data template.
		/// </summary>
		public ObservableCollection<TypeDataTemplatePair> TypeDataTemplates { get; set; } =
			new ObservableCollection<TypeDataTemplatePair>();

		/// <summary>
		///     The default <see cref="DataTemplate" /> if the source content is <c>null</c> or no type is matched in the
		///     <see cref="TypeDataTemplates" />.
		/// </summary>
		public DataTemplate DefaultDataTemplate { get; set; }


		/// <inheritdoc />
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			// null handling
			if (item is null) return DefaultDataTemplate;

			var itemType = item.GetType();

			// Check valid type
			foreach (var pair in TypeDataTemplates)
			{
				if (pair.Type is null) throw new InvalidOperationException("The related type cannot be null.");

				if (pair.Type.IsAssignableFrom(itemType)) return pair.DataTemplate;
			}

			// No valid type, use default template
			return DefaultDataTemplate;
		}
	}
}