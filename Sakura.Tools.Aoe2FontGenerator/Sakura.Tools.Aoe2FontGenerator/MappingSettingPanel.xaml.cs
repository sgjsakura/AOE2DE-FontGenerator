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
using Sakura.Tools.Aoe2FontGenerator.Properties;

namespace Sakura.Tools.Aoe2FontGenerator
{
	/// <summary>
	/// MappingSettingPanel.xaml 的交互逻辑
	/// </summary>
	public partial class MappingSettingPanel : UserControl
	{
		public MappingSettingPanel()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(MappingSetting), typeof(MappingSettingPanel), new FrameworkPropertyMetadata(null));

		public MappingSetting Data
		{
			get { return (MappingSetting)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		private void MappingSettingPanel_OnLoaded(object sender, RoutedEventArgs e)
		{
			GlyphSizeRatioSlider.Ticks = Utility.ToDoubleCollection(Settings.Default.DefaultGlyphRatioTicks);
			GlyphBaselineOffsetSlider.Ticks = Utility.ToDoubleCollection(Settings.Default.DefaultBaselineOffsetTicks);
		}
	}
}
