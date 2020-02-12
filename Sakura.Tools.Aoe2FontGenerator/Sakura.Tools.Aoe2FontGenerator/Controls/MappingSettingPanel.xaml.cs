using System.Windows;
using System.Windows.Controls;
using Sakura.Tools.Aoe2FontGenerator.Models;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	///     MappingSettingPanel.xaml 的交互逻辑
	/// </summary>
	public partial class MappingSettingPanel : UserControl
	{
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data),
			typeof(MappingSetting), typeof(MappingSettingPanel), new FrameworkPropertyMetadata(null));

		public MappingSettingPanel()
		{
			InitializeComponent();
		}

		public MappingSetting Data
		{
			get => (MappingSetting) GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}
	}
}