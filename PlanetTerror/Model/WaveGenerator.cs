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
		public double WaveTimeLeft { get; set; }
		public double BundleTimeLeft { get; set; }

		//===============================================================================================================================================
		//	필드
		int curWaveIndex;
		int curBundleIndex;
		int curSeqIndex;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WaveGenerator()
		{
			curWaveIndex = 0;
			PrepareWave();			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			if( curWaveIndex >= Game.Setting.waves.Count ) { return; }

//			if( Time)
			var wave = Game.Setting.waves[curWaveIndex];



			
		}

		//===============================================================================================================================================
		//	전용
		void PrepareWave()
		{
			if( curWaveIndex >= Game.Setting.waves.Count ) { return; }

			curBundleIndex = 0;
			curSeqIndex = 0;
			
			var wave = Game.Setting.waves[curWaveIndex];
			WaveTimeLeft = wave.waitTime;
		}
	}
}