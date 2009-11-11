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
using System.Windows.Shapes;

using PlanetTerror.Util;

namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Cutscene1
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Cutscene1 : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public bool Active { get; protected set; }

		//===============================================================================================================================================
		//	필드
		VSM vsm;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Cutscene1()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);

			Menu_Start.Click += new RoutedEventHandler(Menu_Start_Click);
			GameStart_State.Storyboard.Completed += new EventHandler(GameStart_State_Storyboard_Completed);
			GameOver_State.Storyboard.Completed +=new EventHandler(GameStart_Click_State_Storyboard_Completed);
			GameStart_Click_State.Storyboard.Completed +=new EventHandler(GameStart_Click_State_Storyboard_Completed);			
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Start_Click(object sender, RoutedEventArgs e)
		{
			Game.SoundMgr.Play("Sound/Button_Title.wav");
			vsm.SetState("GameStart_Click_State");
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GameStart_State_Storyboard_Completed(object sender, EventArgs e)
		{
			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GameStart_Click_State_Storyboard_Completed(object sender, EventArgs e)
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
			Game.SoundMgr.Music = "Sound/Music/Title.mp3";
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	게임 오버
		public void RunGameOver()
		{
			Activate(true);
			vsm.SetState("GameOver_State");
			Game.SoundMgr.Music = "Sound/Music/GameOver.mp3";
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