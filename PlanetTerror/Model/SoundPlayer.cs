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
using NAudio.CoreAudioApi;
using NAudio.Wave;
using IrrKlang;

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
			const int MAX_BUFFER = 4;
			int playerIndex;
			List<IWavePlayer> wavePlayers;

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Sound(string source)
			{
				Source = source;
				playerIndex = 0;
				wavePlayers = new List<IWavePlayer>();
				for( int i = 0; i < MAX_BUFFER; ++i )
				{
					CreatePlayer();
				}
			}

			//===============================================================================================================================================
			//	공용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주한다.
			public bool Play(double volume)
			{
				Play(wavePlayers[playerIndex], (float)volume);
				playerIndex = (playerIndex + 1) % MAX_BUFFER;
				return true;
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			public bool Play() { return Play(1); }

			//===============================================================================================================================================
			//	전용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	플레이어 생성
			IWavePlayer CreatePlayer()
			{
				var p = CreateWaveOut();
				var input = CreateInputStream();
				p.Init(input);
				p.PlaybackStopped +=new EventHandler(p_PlaybackStopped);
				wavePlayers.Add(p);
				return p;
			}
			void p_PlaybackStopped(object sender, EventArgs e)
			{
				Debug.Print("Stopped");
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	스트림 생성
			WaveChannel32 CreateInputStream()
			{
				WaveStream stream = new WaveFileReader(Source);
				if( stream.WaveFormat.Encoding != WaveFormatEncoding.Pcm )
				{
					stream = WaveFormatConversionStream.CreatePcmStream(stream);
					stream = new BlockAlignReductionStream(stream);
				}
				if( stream.WaveFormat.BitsPerSample != 16 )
				{
					var format = new WaveFormat(stream.WaveFormat.SampleRate, 16, stream.WaveFormat.Channels);
					stream = new WaveFormatConversionStream(format, stream);
				}
				return new WaveChannel32(stream);
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	장치 생성
			static IWavePlayer CreateWaveOut()
			{
				return new WaveOut();
				//return new WaveOut(WaveCallbackInfo.FunctionCallback());
				//return new AsioOut();
				//return new WasapiOut(AudioClientShareMode.Exclusive, 0);
				//return new DirectSoundOut();
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주
			void Play(IWavePlayer player, float volume)
			{
				player.Stop();
				player.Volume = (float)volume;
				player.Play();
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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		MediaPlayer musicPlayer;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Dictionary<string, Sound> sounds;
		ISoundEngine engine;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundMgr()
		{
			musicPlayer = new MediaPlayer();
			musicPlayer.MediaEnded += new EventHandler(musicPlayer_MediaEnded);
			sounds = new Dictionary<string, Sound>();
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	재생
		public bool Play(string soundPath, double volume)
		{

			if( engine == null ) { engine = new ISoundEngine(); }
			engine.Play2D(soundPath, false);


			return true;

//			try
			{
				Sound sound;
				if( !sounds.TryGetValue(soundPath, out sound) )
				{
					sound = new Sound(soundPath);
					sounds.Add(soundPath, sound);
				}
				return sound.Play(volume);
			}
//			catch(Exception e) { Debug.Print("{0}", e.Message); }
			return false;
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