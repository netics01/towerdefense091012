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
using System.Windows.Media.Animation;
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SoundPlayer
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SoundPlayer
	{
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		string music;
		public string Music
		{
			get { return music; }
			set { OnSetMusic(value); }
		}

		//===============================================================================================================================================
		//	필드
		MediaPlayer musicPlayer;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundPlayer()
		{
			musicPlayer = new MediaPlayer();	
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Music이 변경되면
		protected void OnSetMusic(string value)
		{
			music = value;

			musicPlayer.Stop();
			musicPlayer.Open(new Uri(music, UriKind.RelativeOrAbsolute));
			Debug.Print("{0}", musicPlayer.Source);
			musicPlayer.Play();			
		}
	}
}