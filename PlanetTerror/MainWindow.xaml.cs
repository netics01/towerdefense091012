using System;
using System.Collections.Generic;
using System.Linq;
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
	//	MainWindow
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class MainWindow : Window
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static MainWindow Instance { get; protected set; }

		//===============================================================================================================================================
		//	필드
		UpdatePump pump;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public MainWindow()
		{
			InitializeComponent();

			Instance = this;

			pump = new UpdatePump();
			pump.Update += new UpdatePump.UpdateHandler(pump_Update);

			Loaded += new RoutedEventHandler(MainWindow_Loaded);
			KeyDown += new KeyEventHandler(MainWindow_KeyDown);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			//창을 화면 중앙으로 정렬
			var left = (System.Windows.SystemParameters.PrimaryScreenWidth - Width) / 2;
			var top = (System.Windows.SystemParameters.PrimaryScreenHeight - Height) / 2;
			this.SetLeftTop(left, top);

			pump.Begin();

			ui_Panel.Initialize();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.Key == Key.F8 )
			{
				ToggleDebugPanel();
			}
			if( e.Key == Key.F1 )
			{
				debug_Panel.enemy1_Button_Click(null, null);
			}
			if( e.Key == Key.F2 )
			{
				debug_Panel.enemy2_Button_Click(null, null);
			}
			if( e.Key == Key.F3 )
			{
				debug_Panel.enemy3_Button_Click(null, null);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void pump_Update(float delta)
		{
			world.Update(delta);
			debug_Panel.Update(delta);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디버그 패널 토글
		public void ToggleDebugPanel()
		{
			debug_Panel.SetVisible(!debug_Panel.GetVisible());
		}
	}
}
