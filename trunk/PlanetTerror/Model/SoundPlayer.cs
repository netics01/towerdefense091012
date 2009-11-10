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
using System.Windows.Threading;
using System.Media;
using IrrKlang;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SoundMgr
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SoundMgr
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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		MediaPlayer musicPlayer;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		ISoundEngine engine;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundMgr()
		{
			musicPlayer = new MediaPlayer();
			musicPlayer.MediaEnded += new EventHandler(musicPlayer_MediaEnded);
			engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.PrintDebugInfoIntoDebugger);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	재생
		public bool Play(string soundPath, double volume)
		{
			var sound = engine.Play2D(soundPath);
			if( sound == null )
			{
				Debug.Print("No Sound File : {0}", soundPath);
				return false;
			}
			sound.Volume = (float)volume;
			return true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Play(string soundPath) { Play(soundPath, 1); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	구식 방법으로 사운드 재생
		public void PlaySystem(string soundPath)
		{
			NativeMethods.PlaySound(soundPath, new System.IntPtr(), NativeMethods.PlaySoundFlags.SND_ASYNC|NativeMethods.PlaySoundFlags.SND_FILENAME);
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Music이 변경되면
		protected void OnSetMusic(string value)
		{
			music = value;
			musicPlayer.Stop();
			if( value.Length > 0 )
			{
				musicPlayer.Open(new Uri(value, UriKind.Relative));
				musicPlayer.Play();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	배경음악이 종료되면.
		void musicPlayer_MediaEnded(object sender, EventArgs e)
		{
			//무한루핑
			musicPlayer.Stop();
			musicPlayer.Play();
		}
	}
}