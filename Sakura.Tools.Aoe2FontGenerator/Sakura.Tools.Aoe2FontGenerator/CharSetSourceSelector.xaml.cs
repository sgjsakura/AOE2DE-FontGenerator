using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// Provides the UI for charset selection.
	/// </summary>
	public partial class CharSetSourceSelector : UserControl
	{
		/// <summary>
		/// Initialize a new instance of <see cref="CharSetSourceSelector"/>.
		/// </summary>
		public CharSetSourceSelector()
		{
			InitializeComponent();

			FullCharSetSource = new FullCharSetSource();
		}

		public static readonly DependencyProperty ActiveCharSetSourceProperty = DependencyProperty.Register(nameof(ActiveCharSetSource), typeof(CharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Get the active <see cref="CharSetSource"/>.
		/// </summary>
		public CharSetSource ActiveCharSetSource
		{
			get => (CharSetSource)GetValue(ActiveCharSetSourceProperty);
			set => SetValue(ActiveCharSetSourceProperty, value);
		}

		private static readonly DependencyPropertyKey FullCharSetSourcePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FullCharSetSource), typeof(FullCharSetSource), typeof(CharSetSourceSelector), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty FullCharSetSourceProperty =
			FullCharSetSourcePropertyKey.DependencyProperty;

		public FullCharSetSource FullCharSetSource
		{
			get => (FullCharSetSource) GetValue(FullCharSetSourceProperty);
			private set => SetValue(FullCharSetSourcePropertyKey, value);
		}

		private void FullCharSetRadioButton_OnChecked(object sender, RoutedEventArgs e)
		{
			ActiveCharSetSource = FullCharSetSource;
		}
	}
}
