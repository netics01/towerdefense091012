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

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public MainWindow()
		{
			InitializeComponent();

			Instance = this;

			KeyDown += new KeyEventHandler(MainWindow_KeyDown);
		}

		//===============================================================================================================================================
		//	핸들러
		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.Key == Key.F8 )
			{
				ToggleDebugPanel();
			}
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
