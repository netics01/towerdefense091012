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
using Microsoft.DirectX.DirectSound;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SoundMgr
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SoundMgr
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//	Sound
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		class Sound
		{
			//===============================================================================================================================================
			//	프로퍼티
			public string Source { get; protected set; }

			//===============================================================================================================================================
			//	필드
			const int BUFFER = 5;
			int playIndex;
			List<SoundPlayer> players;

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Sound(string source)
			{
				Source = source;
				playIndex = 0;
				players = new List<SoundPlayer>(BUFFER);
				for( int i = 0; i < BUFFER; ++i )
				{
					players.Add(new SoundPlayer(Source));
				}
			}

			//===============================================================================================================================================
			//	공용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주한다.
			public void Play()
			{
				//players[playIndex].Stop();
				players[playIndex].Play();
				playIndex = (playIndex + 1) % BUFFER;
			}
		}

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
		Dictionary<string, Sound> sounds;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundMgr()
		{
			musicPlayer = new MediaPlayer();
			sounds = new Dictionary<string, Sound>();
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	재생
		public void Play(string soundPath)
		{
			Sound sound;
			if( sounds.TryGetValue(soundPath, out sound) )
			{
				sound.Play();
			}
			else
			{
				sound = new Sound(soundPath);
				sounds.Add(soundPath, sound);
				sound.Play();
			}
		}
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
	}
}