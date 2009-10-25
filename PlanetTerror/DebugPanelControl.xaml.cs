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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public DebugPanelControl()
		{
			this.InitializeComponent();

			test1_Button.Click += new RoutedEventHandler(test1_Button_Click);
			test2_Button.Click += new RoutedEventHandler(test2_Button_Click);
			bossTest1_Button.Click += new RoutedEventHandler(bossTest1_Button_Click);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void test1_Button_Click(object sender, RoutedEventArgs e)
		{
			WorldControl.Instance.CreateEnemy1(WorldControl.Instance.Route1);
			MainWindow.Instance.ToggleDebugPanel();			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void test2_Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.Instance.ToggleDebugPanel();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void bossTest1_Button_Click(object sender, RoutedEventArgs e)
		{
			WorldControl.Instance.testBoss1.TestState("MoveState");
		}
	}
}