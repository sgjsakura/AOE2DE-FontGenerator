using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// The application class.
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Get the current instance of this application.
		/// </summary>
		public new static App Current => (App)Application.Current;
	}
}
