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
using System.Windows.Shapes;

using PlanetTerror.Util;

namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Cusscene1
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Cuscene1 : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public bool Active { get; protected set; }

		//===============================================================================================================================================
		//	필드
		VSM vsm;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Cuscene1()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);

			Menu_Start.Click += new RoutedEventHandler(Menu_Start_Click);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Start_Click(object sender, RoutedEventArgs e)
		{
			Activate(false);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타이틀 실행
		public void RunTitle()
		{
			Activate(true);
			vsm.SetState("GameStart_State");
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	활성화
		void Activate(bool bActive)
		{
			Active = bActive;
			this.SetVisible(Active);
		}
	}
}