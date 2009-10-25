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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SettingXml
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class SettingXml
	{
		public int startGold = 1000;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int tower_BuildGold = 50;
		public double tower_AttackRangeSqr = 200 * 200;
		public double tower_AttackDamageSqr = 20;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public double enemy1_RouteTime = 20.0;
		public double enemy1_HitPoint = 100;
		
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