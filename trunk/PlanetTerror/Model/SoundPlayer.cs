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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SoundPlayer
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SoundPlayer
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//	Sound
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		class Sound
		{
			//===============================================================================================================================================
			//	프로퍼티
			public bool IsPlaying { get; protected set;}
			//===============================================================================================================================================
			//	필드
			MediaPlayer player;

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Sound()
			{
				player = new MediaPlayer();
				player.ScrubbingEnabled = false;
				player.MediaEnded += new EventHandler(player_MediaEnded);
				player.MediaOpened += new EventHandler(player_MediaOpened);
				player.MediaFailed += new EventHandler<ExceptionEventArgs>(player_MediaFailed);
				IsPlaying = false;
			}

			//===============================================================================================================================================
			//	공용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주한다.
			public bool Play(string path)
			{
				if( IsPlaying ) { return false; }

				Debug.Print("access = {0}", player.CheckAccess());
				//player.Dispatcher.BeginInvoke(DispatcherPriority.Send,
				player.Dispatcher.Invoke(DispatcherPriority.Send,
					new Action(delegate {
						player.Stop();
						if( player.Source == null ||
							player.Source.OriginalString != path )
						{
							player.Open(new Uri(path, UriKind.Relative));
							Debug.Print("Open()");
						}
						player.Play();
					}));

// 				player.Stop();
// 				if( player.Source == null ||
// 					player.Source.OriginalString != path)
// 				{
// 					player.Open(new Uri(path, UriKind.Relative));
// 					Debug.Print("Open()");
// 				}				
// 				player.Play();
				IsPlaying = true;
				return true;
			}

			//===============================================================================================================================================
			//	전용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	재생이 끝났다.
			void player_MediaEnded(object sender, EventArgs e)
			{
				IsPlaying = false;
				Debug.Print("End");
// 				player.Stop();
// 				player.Play();
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	에러발생
			void player_MediaFailed(object sender, ExceptionEventArgs e)
			{
				Debug.Print("Error {0}", e.ErrorException.ToString());
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	미디어가 열렸다?
			void player_MediaOpened(object sender, EventArgs e)
			{
				Debug.Print("MediaOpened");	
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
		const int BUFFER = 10;
		MediaPlayer musicPlayer;
		List<Sound> sounds;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundPlayer()
		{
			musicPlayer = new MediaPlayer();
			sounds = new List<Sound>();
			for( int i = 0; i < BUFFER; ++i )
			{
				sounds.Add(new Sound());
			}
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	사운드 재생
		public bool Play(string soundPath)
		{
			//재생중이 아닌 플레이어가 없으면 재생 안한다.
			for( int i = 0; i < sounds.Count; ++i )
			{
				if( !sounds[i].IsPlaying )
				{
					sounds[i].Play(soundPath);
					Debug.Print("Sound {0}", i);
					return true;
				}
			}
			Debug.Print("No Sounds");
			return false;
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Music이 변경되면
		protected void OnSetMusic(string value)
		{
			music = value;

			musicPlayer.Stop();
			musicPlayer.Open(new Uri(music, UriKind.Relative));
			musicPlayer.Play();			
		}
	}
}