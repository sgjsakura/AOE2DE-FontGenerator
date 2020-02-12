using System.Windows;
using System.Windows.Controls;
using Sakura.Tools.Aoe2FontGenerator.Models;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	///     CharSetMappingSettingPanel.xaml 的交互逻辑
	/// </summary>
	public partial class CharSetMappingSettingPanel : UserControl
	{
		public static readonly DependencyProperty CharSetFontMappingProperty =
			DependencyProperty.Register(nameof(CharSetFontMapping), typeof(CharSetFontMapping),
				typeof(CharSetMappingSettingPanel), new FrameworkPropertyMetadata(null));

		public CharSetMappingSettingPanel()
		{
			InitializeComponent();
		}

		public CharSetFontMapping CharSetFontMapping
		{
			get => (CharSetFontMapping) GetValue(CharSetFontMappingProperty);
			set => SetValue(CharSetFontMappingProperty, value);
		}

		private void ConfigButton_OnClick(object sender, RoutedEventArgs e)
		{
			var item = (CharSetFontMapping) ((Button) sender).Tag;

			var window = new CharSetMappingWindow
			{
				Owner = Window.GetWindow(this),
				Data = item
			};

			window.ShowDialog();
		}
	}
}