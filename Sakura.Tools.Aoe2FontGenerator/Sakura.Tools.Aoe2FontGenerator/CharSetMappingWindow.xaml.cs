using System.Windows;
using Sakura.Tools.Aoe2FontGenerator.Models;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// CharSetMappingWindow.xaml 的交互逻辑
	/// </summary>
	public partial class CharSetMappingWindow : Window
	{
		public CharSetMappingWindow()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(CharSetFontMapping), typeof(CharSetMappingWindow), new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Get or set the data related with this control.
		/// </summary>
		public CharSetFontMapping Data
		{
			get => (CharSetFontMapping)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}
	}
}
