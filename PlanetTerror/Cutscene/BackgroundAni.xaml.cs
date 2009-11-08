using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	BackgroundAni
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class BackgrouundAni : UserControl
	{
		//===============================================================================================================================================
		//	필드
		Storyboard backgroundStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public BackgrouundAni()
		{
			this.InitializeComponent();

			Loaded += new RoutedEventHandler(BackgrouundAni_Loaded);

			backgroundStory = Resources.FindStoryboard("BackgroundAni_Storyboard");
			backgroundStory.RepeatForever();
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BackgrouundAni_Loaded(object sender, RoutedEventArgs e)
		{
			backgroundStory.Begin();
		}
	}
}