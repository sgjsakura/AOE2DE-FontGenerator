using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Provide helper methods to access resources. This class is static.
	/// </summary>
	public static class ResourceUtility
	{
		/// <summary>
		/// Find the resource object of type <typeparamref name="T"/> with the specified <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T">The type of the resource.</typeparam>
		/// <param name="element">The <see cref="FrameworkElement"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The strong-typed resource object.</returns>
		public static T FindResource<T>(this FrameworkElement element, object key) => (T)element.FindResource(key);

		/// <summary>
		/// Find the resource object of type <typeparamref name="T"/> with the specified <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T">The type of the resource.</typeparam>
		/// <param name="application">The <see cref="Application"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The strong-typed resource object.</returns>
		public static T FindResource<T>(this Application application, object key) => (T)application.FindResource(key);

		#region String Helper

		/// <summary>
		/// Find the string resource with the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="element">The <see cref="FrameworkElement"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The string resource object.</returns>
		public static string FindResString(this FrameworkElement element, object key)
			=> element.FindResource<string>(key);

		/// <summary>
		/// Find the string resource with the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="application">The <see cref="Application"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The string resource object.</returns>
		public static string FindResString(this Application application, object key) =>
			application.FindResource<string>(key);

		#endregion
	}
}
