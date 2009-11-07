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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	UIPanelControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class UIPanelControl : UserControl
	{
		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		int gold;
		double lastPowerGauge;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Storyboard noMoneyStory;
		Storyboard normalWarningStory;
		Storyboard bossWarningStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public UIPanelControl()
		{
			this.InitializeComponent();

			noMoneyStory = Resources.FindStoryboard("NoMoney_Storyboard");
			normalWarningStory = Resources.FindStoryboard("Warning_Enemy_Storyboard");
			bossWarningStory = Resources.FindStoryboard("Warning_Boss_Storyboard");
			
			Loaded += new RoutedEventHandler(UIPanelControl_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void UIPanelControl_Loaded(object sender, RoutedEventArgs e)
		{
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			gold = Game.Setting.startGold;
			power_Progress.Maximum = Game.Setting.powerMax;
			power_Progress.Minimum = 0;
			power_Progress.Value = 0;
			DisplayGold();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			if( lastPowerGauge < 100 &&
				power_Progress.Value == 100 )
			{
				Game.World.core.DeadlyReady();
			}
			DisplayGold();
			lastPowerGauge = power_Progress.Value;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	돈 소모
		public bool SpendGold(int requiredGold)
		{
			if( requiredGold > gold )
			{
				noMoneyStory.Begin();
				return false;
			}

			gold -= requiredGold;
			return true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	돈 추가
		public void GainGold(int gainedGold)
		{
			gold += gainedGold;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	게이지 변화
		public void GainPower(double power)
		{
			power_Progress.Value = power_Progress.Value + power;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	경고 발생
		public void DisplayWarning(bool bBoss)
		{
			if( bBoss ) { bossWarningStory.Begin(); }
			else { normalWarningStory.Begin(); }
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	골드 표시
		void DisplayGold()
		{
			gold_Text.Text = gold.ToString();
		}
	}
}