﻿using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sakura.Tools.Aoe2FontGenerator.Models;
using Sakura.Tools.Aoe2FontGenerator.Properties;

namespace Sakura.Tools.Aoe2FontGenerator.Controls
{
	/// <summary>
	/// OutputSettingPage.xaml 的交互逻辑
	/// </summary>
	public partial class OutputSettingPage : UserControl
	{
		public OutputSettingPage()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(nameof(Data), typeof(GenerationSetting), typeof(OutputSettingPage), new FrameworkPropertyMetadata(default(GenerationSetting)));

		public GenerationSetting Data
		{
			get => (GenerationSetting) GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		private void BrowseOutputDirButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog
			{
				IsFolderPicker = true,
				CookieIdentifier = new Guid("{04818447-B495-47D9-9401-9623F88FB107}")
			};

			using (dialog)
			{
				if (dialog.ShowDialog(Window.GetWindow(this)) == CommonFileDialogResult.Ok)
				{
					OutputDirectoryTextBox.Text = dialog.FileName;
				}
			}
		}

		private void SetAsDefaultButton_OnClick(object sender, RoutedEventArgs e)
		{
			MetaDataFileNameTextBox.Text = Settings.Default.DefaultMetaFileName;
			SurfaceFileFormatTextBox.Text = Settings.Default.DefaultSurfaceFileFormat;
		}

		private void SetAsDefaultSansSerifButton_OnClick(object sender, RoutedEventArgs e)
		{
			MetaDataFileNameTextBox.Text = Settings.Default.DefaultSansSerifMetaFileName;
			SurfaceFileFormatTextBox.Text = Settings.Default.DefaultSansSerifSurfaceFileFormat;
		}
	}
}