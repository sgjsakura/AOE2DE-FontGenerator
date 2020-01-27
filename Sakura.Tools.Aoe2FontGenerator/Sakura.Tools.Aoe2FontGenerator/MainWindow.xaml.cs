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
using Microsoft.WindowsAPICodePack.Dialogs;

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

		private void StartButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (CharSetFontMappings.Count == 0)
			{
				this.ShowMessage(this.FindResString("AppName"), "No charset mapping is defined",
					"You must define at least one charset mapping in order to generate the font atlas file.", TaskDialogStandardIcon.Error, TaskDialogStandardButtons.Ok);
				return;
			}

			foreach (var item in CharSetFontMappings)
			{
				if (item.Font == null || item.CharSet == null)
				{
					this.ShowMessage(this.FindResString("AppName"), "Charset mapping setting is invalid",
						"Please set both the font source and the charset source for all mappings before starting the font generation.", TaskDialogStandardIcon.Error, TaskDialogStandardButtons.Ok);

					return;
				}
			}

			var generator = new FontGenerator();
			generator.Generate(CharSetFontMappings);

			MessageBox.Show("Ok!");

		}
	}
}
