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
			const int MAX_BUFFER = 5;
			Device soundDevice;
			List<SecondaryBuffer> buffers;

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Sound(Device soundDevice, string source)
			{
				Source = source;
				this.soundDevice = soundDevice;
				buffers = new List<SecondaryBuffer>();
				buffers.Add(AllocateBuffer());
			}

			//===============================================================================================================================================
			//	공용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주한다.
			public bool Play(double volume)
			{
				int vol = (int)((1 - volume) * -10000);
				for( int i = 0; i < buffers.Count; ++i )
				{
					if( !buffers[i].Status.Playing )
					{
						PlayBuffer(buffers[i], vol);
						return true;
					}
				}
				if( buffers.Count >= MAX_BUFFER ) { return false; }
				var b = AllocateBuffer();
				PlayBuffer(b, vol);
				buffers.Add(b);

				Debug.Print("BufferCount {0}", buffers.Count);


				return true;
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			public bool Play() { return Play(1); }

			//===============================================================================================================================================
			//	전용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	버퍼를 할당한다.
			SecondaryBuffer AllocateBuffer()
			{
				var desc = new BufferDescription();
				desc.ControlVolume = true;
				return new SecondaryBuffer(Source, desc, soundDevice);
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	연주한다.
			void PlayBuffer(SecondaryBuffer buffer, int volume)
			{
				buffer.Volume = volume;
				buffer.Play(0, BufferPlayFlags.Default);
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
		Device soundDevice;
		Dictionary<string, Sound> sounds;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public SoundMgr()
		{
			musicPlayer = new MediaPlayer();
			sounds = new Dictionary<string, Sound>();

			soundDevice = new Device();
			soundDevice.SetCooperativeLevel(Game.MainWindow.GetHWND(), CooperativeLevel.Priority);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	재생
		public void Play(string soundPath, double volume)
		{
			Sound sound;
			if( sounds.TryGetValue(soundPath, out sound) )
			{
				sound.Play(volume);
			}
			else
			{
				sound = new Sound(soundDevice, soundPath);
				sounds.Add(soundPath, sound);
				sound.Play(volume);
			}
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
	}
}