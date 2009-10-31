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
	//	DebugPanelControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class DebugPanelControl : UserControl
	{
		//===============================================================================================================================================
		//	필드
		int routeSelect;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public DebugPanelControl()
		{
			this.InitializeComponent();

			test1_Button.Click += new RoutedEventHandler(test1_Button_Click);
			test2_Button.Click += new RoutedEventHandler(test2_Button_Click);
			enemy1_Button.Click += new RoutedEventHandler(enemy1_Button_Click);
			enemy2_Button.Click += new RoutedEventHandler(enemy2_Button_Click);
			enemy3_Button.Click += new RoutedEventHandler(enemy3_Button_Click);
			bossTest1_Button.Click += new RoutedEventHandler(bossTest1_Button_Click);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void test1_Button_Click(object sender, RoutedEventArgs e)
		{
			//MainWindow.Instance.ToggleDebugPanel();			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void test2_Button_Click(object sender, RoutedEventArgs e)
		{
			//MainWindow.Instance.ToggleDebugPanel();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void enemy1_Button_Click(object sender, RoutedEventArgs e)
		{
			EnemyTest<Enemy1>();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void enemy2_Button_Click(object sender, RoutedEventArgs e)
		{
			EnemyTest<Enemy2>();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void enemy3_Button_Click(object sender, RoutedEventArgs e)
		{
			EnemyTest<Enemy3>();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void bossTest1_Button_Click(object sender, RoutedEventArgs e)
		{
			WorldControl.Instance.testBoss1.TestState("Boss_Move_State");
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void EnemyTest<T>() where T : Enemy, new()
		{
			WorldControl.Instance.CreateEnemy<T>(WorldControl.Instance.Routes[routeSelect]);
			routeSelect = routeSelect.RotateInc(WorldControl.Instance.Routes.Count);
		}
	}
}