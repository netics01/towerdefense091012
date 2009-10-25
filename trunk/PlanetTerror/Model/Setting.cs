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
	//	SettingXml
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class SettingXml
	{
		public double enemy1RouteTime = 20.0;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static SettingXml Instance { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SettingXml()
		{
	
		}
	}
}