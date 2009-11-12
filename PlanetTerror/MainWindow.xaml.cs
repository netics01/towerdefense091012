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
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	MainWindow
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class MainWindow : Window
	{
		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		UpdatePump pump;
		bool bFullscreen;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		bool bGameStart;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public MainWindow()
		{
			InitializeComponent();

			//창을 화면 중앙으로 정렬
			this.SetLeftTop(WPFUtil.GetScreenCenterLeftTop(Width, Height));

			Game.MainWindow = this;
			Game.UI = ui_Panel;
			Game.World = world;

			pump = new UpdatePump();
			pump.Update += new UpdatePump.UpdateHandler(pump_Update);
			bFullscreen = false;

			Loaded += new RoutedEventHandler(MainWindow_Loaded);
			KeyDown += new KeyEventHandler(MainWindow_KeyDown);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if( this.IsInDesignMode() ) { return; }

			if( !Debugger.IsAttached )
			{
				Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);
			}

			Game.SoundMgr = new SoundMgr();
			pump.Begin();

			ui_Panel.Initialize();
			world.Initialize();
			bGameStart = false;

			if( Game.Setting.title )
			{
				cutscene.RunTitle();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if( !Game.Setting.debugMode ) { return; }
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
			if( e.Key == Key.F4 )
			{
				debug_Panel.wave_Button_Click(null, null);
			}
			if( e.Key == Key.F5 )
			{
				debug_Panel.coreTest_Button_Click(null, null);
			}
			if( e.Key == Key.F6 )
			{
				Game.UI.GainGold(1000);
				Game.World.core.Invincible = !Game.World.core.Invincible;
			}
			if( e.Key == Key.F7 )
			{
				Game.World.CreateEnemy<Boss1>(null);
			}
			if( e.Key == Key.F12 )
			{
				ToggleFullscreen();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void pump_Update(float delta)
		{
			if( !bGameStart )
			{
				if( !cutscene.Active )
				{
					bGameStart = true;
					Game.SoundMgr.Music = "Sound/Music/Background.mp3";
				}
			}
			if( cutscene.Active ) { return; }

			world.Update(delta);
			ui_Panel.Update(delta);
			debug_Panel.Update(delta);

			if( Game.World.core.Boomed )
			{
				cutscene.RunGameOver();
			}
			if( Game.Generator.State == WaveGenerator.EState.WaveOver &&
				Game.World.NoEnemy )
			{
				cutscene.RunEnding();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.StackTrace, e.Exception.Message);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디버그 패널 토글
		public void ToggleDebugPanel()
		{
			debug_Panel.SetVisible(!debug_Panel.GetVisible());
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	풀스크린
		void ToggleFullscreen()
		{
			if( !bFullscreen )
			{
				this.SwitchFullscreen();
				bFullscreen = true;
			}
			else
			{
				this.SwitchWindowed(WindowStyle.SingleBorderWindow, ResizeMode.NoResize);
				bFullscreen = false;
			}
		}
	}
}
