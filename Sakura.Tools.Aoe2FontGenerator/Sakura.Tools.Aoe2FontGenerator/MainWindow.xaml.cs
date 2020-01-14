using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// The main window of this application.
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Initialize a new instance of <see cref="MainWindow"/>.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			CharSetFontMappings = new ObservableCollection<CharSetFontMapping>
			{
				new CharSetFontMapping()
			};
		}

		public static readonly DependencyProperty CharSetFontMappingsProperty = DependencyProperty.Register(nameof(CharSetFontMappings), typeof(ObservableCollection<CharSetFontMapping>), typeof(MainWindow), new FrameworkPropertyMetadata(null));

		public ObservableCollection<CharSetFontMapping> CharSetFontMappings
		{
			get => (ObservableCollection<CharSetFontMapping>)GetValue(CharSetFontMappingsProperty);
			set => SetValue(CharSetFontMappingsProperty, value);
		}
	}
}
