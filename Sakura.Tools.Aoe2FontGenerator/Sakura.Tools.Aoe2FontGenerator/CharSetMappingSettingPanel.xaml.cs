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
using WpfColorFontDialog;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// CharSetMappingSettingPanel.xaml 的交互逻辑
	/// </summary>
	public partial class CharSetMappingSettingPanel : UserControl
	{
		public CharSetMappingSettingPanel()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty CharSetFontMappingProperty = DependencyProperty.Register(nameof(CharSetFontMapping), typeof(CharSetFontMapping), typeof(CharSetMappingSettingPanel), new FrameworkPropertyMetadata(null));

		public CharSetFontMapping CharSetFontMapping
		{
			get => (CharSetFontMapping) GetValue(CharSetFontMappingProperty);
			set => SetValue(CharSetFontMappingProperty, value);
		}
	}
}
