using System.Windows;
using System.Windows.Controls;
using Sakura.Tools.Aoe2FontGenerator.Models;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	///     GeneralSettingPage.xaml 的交互逻辑
	/// </summary>
	public partial class GeneralSettingPage : UserControl
	{
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(nameof(Data), typeof(GenerationSetting), typeof(GeneralSettingPage),
				new FrameworkPropertyMetadata(default(GenerationSetting)));

		public GeneralSettingPage()
		{
			InitializeComponent();
		}

		public GenerationSetting Data
		{
			get => (GenerationSetting) GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}
	}
}