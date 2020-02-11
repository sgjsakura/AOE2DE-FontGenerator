using System.Windows;
using System.Windows.Controls;
using Sakura.Tools.Aoe2FontGenerator.Models;
using Sakura.Tools.Aoe2FontGenerator.Properties;
using Sakura.Tools.Aoe2FontGenerator.Utilities;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
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
			get => (MappingSetting)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		private void MappingSettingPanel_OnLoaded(object sender, RoutedEventArgs e)
		{
			GlyphSizeRatioSlider.Ticks = Utility.ToDoubleCollection(Settings.Default.DefaultGlyphRatioTicks);
			GlyphBaselineOffsetRadioSlider.Ticks = Utility.ToDoubleCollection(Settings.Default.DefaultBaselineOffsetTicks);
		}
	}
}
