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
		//	싱글턴 인스턴스
		public static DebugPanelControl Instance;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	로그
		public static void Log(string str)
		{
			Instance.log_ListBox.Items.Insert(0, str);
			if( Instance.log_ListBox.Items.Count == 100 )
			{
				Instance.log_ListBox.Items.RemoveAt(99);
			}
		}

		//===============================================================================================================================================
		//	필드
		int routeSelect;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public DebugPanelControl()
		{
			this.InitializeComponent();

			Instance = this;

			test1_Button.Click += new RoutedEventHandler(test1_Button_Click);
			test2_Button.Click += new RoutedEventHandler(test2_Button_Click);
			enemy1_Button.Click += new RoutedEventHandler(enemy1_Button_Click);
			enemy2_Button.Click += new RoutedEventHandler(enemy2_Button_Click);
			enemy3_Button.Click += new RoutedEventHandler(enemy3_Button_Click);
			wave_Button.Click += new RoutedEventHandler(wave_Button_Click);
			bossTest1_Button.Click += new RoutedEventHandler(bossTest1_Button_Click);
			coreTest_Button.Click += new RoutedEventHandler(coreTest_Button_Click);
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
			EnemyTest<Enemy4>();
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
		public void wave_Button_Click(object sender, RoutedEventArgs e)
		{
			Game.Generator.WaveTimeLeft = 0.1;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void bossTest1_Button_Click(object sender, RoutedEventArgs e)
		{
			//Game.World.testBoss1.TestState("Boss_Move_State");
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void coreTest_Button_Click(object sender, RoutedEventArgs e)
		{
			Game.UI.GainPower(1000);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			coreHp_Text.Text = string.Format("{0:F0}", Game.World.core.HitPoint);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void EnemyTest<T>() where T : Enemy, new()
		{
			Game.World.CreateEnemy<T>(Game.World.Routes[routeSelect]);
			routeSelect = routeSelect.RotateInc(Game.World.Routes.Count);
		}
	}
}