﻿using System;
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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Boss2
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	데브캣 나크
	public partial class Boss2 : UserControl
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Boss2()
		{
			this.InitializeComponent();

			background_Image.SetVisible(false);

			Loaded += new RoutedEventHandler(Boss1_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		void Boss1_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);
			//VisualStateManager.GoToState(this, "MoveState", false);			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스테이트 테스트
		public void TestState(string stateName)
		{
			VisualStateManager.GoToState(this, stateName, false);
		}

	}
}