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
	//	WaveGenerator
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class WaveGenerator
	{
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public enum EState { Wave, Bundle, Seq, WaveOver }
		public EState State { get; protected set; }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public double WaveTimeLeft { get; set; }
		public double BundleTimeLeft { get; set; }
		public double SeqTimeLeft { get; set; }

		//===============================================================================================================================================
		//	필드
		int curWaveIndex;
		Setting.Wave curWave;
		int curBundleIndex;
		Setting.Wave.Bundle curBundle;
		int curSeqIndex;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WaveGenerator()
		{
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화. Blend 에서 디자인 타임에 생성하기 위해서 분리
		public void Initialize()
		{
			curWaveIndex = -1;
			PrepareNextWave();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			while( true )
			{
				switch( State )
				{
				case EState.Wave:
					//Debug.Print("{0}", WaveTimeLeft);
					if( WaveTimeLeft - delta <= Game.Setting.core.warningBefore &&
						Game.Setting.core.warningBefore <= WaveTimeLeft )
					{
						Game.UI.DisplayWarning(curWave.bundles[0].pathIndex, curWave.bundles[0].typeName == "Boss1");
					}
					WaveTimeLeft -= delta;
					if( WaveTimeLeft < 0 )
					{
						delta = (float)-WaveTimeLeft;
						WaveTimeLeft = 0;

						Debug.Assert( curWave.bundles.Count > 0 );
						State = EState.Bundle;
						curBundleIndex = 0;
						curBundle = curWave.bundles[curBundleIndex];
						BundleTimeLeft = curBundle.waitTime;
					}
					else { return; }
					break;
				case EState.Bundle:
					BundleTimeLeft -= delta;
					if( BundleTimeLeft < 0 )
					{
						delta = (float)-BundleTimeLeft;
						BundleTimeLeft = 0;

						Debug.Assert(curBundle.count > 0);
						State = EState.Seq;
						curSeqIndex = 0;
						SeqTimeLeft = curBundle.interval;
						Create();
					}
					else { return; }
					break;
				case EState.Seq:
					SeqTimeLeft -= delta;
					if( SeqTimeLeft < 0 )
					{
						delta = (float)-SeqTimeLeft;
						SeqTimeLeft = 0;

						curSeqIndex++;
						if( curSeqIndex < curBundle.count )
						{
							SeqTimeLeft = curBundle.interval;
							Create();
						}
						else
						{
							curBundleIndex++;
							if( curBundleIndex < curWave.bundles.Count )
							{
								State = EState.Bundle;
								curBundle = curWave.bundles[curBundleIndex];
								BundleTimeLeft = curBundle.waitTime;
							}
							else { PrepareNextWave(); }
						}						
					}
					else { return; }
					break;
				case EState.WaveOver:
					return;
				}
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	웨이브 준비
		void PrepareNextWave()
		{
			curWaveIndex++;

			if( curWaveIndex >= Game.Setting.waves.Count)
			{
				State = EState.WaveOver;
				return;
			}

			State = EState.Wave;
			curWave = Game.Setting.waves[curWaveIndex];
			WaveTimeLeft = curWave.waitTime;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	번들 준비
// 		private void PrepareNextBundle()
// 		{
// 			curBundleIndex++;
// 			var wave = Game.Setting.waves[curWaveIndex];
// 			Debug.Assert( wave.bundles.Count != 0 );
// 		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성
		private void Create()
		{
			Debug.Assert( curBundle != null );
			var pathIndex = curBundle.pathIndex < Game.World.Routes.Count ? curBundle.pathIndex : 0;
			var path = Game.World.Routes[pathIndex];
			switch( curBundle.typeName )
			{
			case "Enemy1":
				Game.World.CreateEnemy<Enemy1>(path);
				break;
			case "Enemy2":
				Game.World.CreateEnemy<Enemy2>(path);
				break;
			case "Enemy3":
				Game.World.CreateEnemy<Enemy3>(path);
				break;
			case "Boss1":
				Game.World.CreateEnemy<Boss1>(path);
				break;
			}
		}
	}
}