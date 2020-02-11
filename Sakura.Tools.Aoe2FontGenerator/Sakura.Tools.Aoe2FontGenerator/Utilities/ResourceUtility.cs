using System.Globalization;
using System.Windows;

namespace Sakura.Tools.Aoe2FontGenerator.Utilities
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

		/// <summary>
		/// Find the string format resource with the specified <paramref name="key"/> and use format arguments to generate the entire string.
		/// </summary>
		/// <param name="element">The <see cref="FrameworkElement"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <param name="args">Argument used to formatting the string.</param>
		/// <returns>The formatted string.</returns>
		public static string FormatResString(this FrameworkElement element, object key, params object[] args)
		{
			var format = element.FindResString(key);
			return string.Format(CultureInfo.CurrentUICulture, format, args);
		}


		/// <summary>
		/// Find the string format resource with the specified <paramref name="key"/> and use format arguments to generate the entire string.
		/// </summary>
		/// <param name="application">The <see cref="Application"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <param name="args">Argument used to formatting the string.</param>
		/// <returns>The formatted string.</returns>

		public static string FormatResString(this Application application, object key, params object[] args)
		{
			var format = application.FindResString(key);
			return string.Format(CultureInfo.CurrentUICulture, format, args);
		}

		/// <summary>
		/// Find the string format resource with the specified <paramref name="key"/>. This method does not provide additional formatting arguments.
		/// </summary>
		/// <param name="element">The <see cref="FrameworkElement"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The formatted string.</returns>
		public static string FormatResString(this FrameworkElement element, object key) => element.FindResString(key);

		/// <summary>
		/// Find the string format resource with the specified <paramref name="key"/>. This method does not provide additional formatting arguments.
		/// </summary>
		/// <param name="application">The <see cref="Application"/> object.</param>
		/// <param name="key">The key of the resource.</param>
		/// <returns>The formatted string.</returns>
		public static string FormatResString(this Application application, object key) => application.FindResString(key);

		#endregion
	}
}
