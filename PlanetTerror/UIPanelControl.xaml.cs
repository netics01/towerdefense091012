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
	//	UIPanelControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class UIPanelControl : UserControl
	{
		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		int gold;
		int lastGold;
		double lastPowerGauge;
		int upgradeLevel = 0;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Storyboard noMoneyStory;
		Storyboard normalWarningStory;
		List<Storyboard> routeWarningStories;
		Storyboard bossWarningStory;
		Storyboard goldGainStory;
		Storyboard goldLostStory;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		bool bFirstTowerBuilt = false;
		bool bFirstLabBuilt = false;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public UIPanelControl()
		{
			this.InitializeComponent();

			noMoneyStory = Resources.FindStoryboard("NoMoney_Storyboard");
			normalWarningStory = Resources.FindStoryboard("Warning_Enemy_Storyboard");
			routeWarningStories = new List<Storyboard>();
			bossWarningStory = Resources.FindStoryboard("Warning_Boss_Storyboard");
			goldGainStory = Resources.FindStoryboard("Gold_Gain_Storyboard");
			goldLostStory = Resources.FindStoryboard("Gold_Lost_Storyboard");
	
			Loaded += new RoutedEventHandler(UIPanelControl_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void UIPanelControl_Loaded(object sender, RoutedEventArgs e)
		{
			for( int i = 0; i < Game.World.Routes.Count; ++i )
			{
				var story = Resources.FindStoryboard(string.Format("route{0}_Path_Storyboard", i + 1));
				if( story == null )
				{
					MessageBox.Show(string.Format("UI : route{0}_Path_Storyboard Not Found", i + 1));
					continue;
				}
				story.RepeatBehavior = new RepeatBehavior(2);
				routeWarningStories.Add(story);
			}
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			gold = Game.Setting.startGold;
			lastGold = gold;
			power_Progress.Maximum = Game.Setting.core.upg1Research;
			power_Progress.Minimum = 0;
			power_Progress.Value = 0;
			DisplayGold();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			if( lastGold != gold )
			{
				if( lastGold < gold )
				{
					Game.SoundMgr.Play("Sound/Money_Increase.wav");
					goldGainStory.Begin();
				}
				else
				{
					goldLostStory.Begin();
				}
				DisplayGold();
			}			
			lastPowerGauge = power_Progress.Value;
			lastGold = gold;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업그레이드 가능?
		public bool IsUpgradable(int level)
		{
			bool bUpgradable = upgradeLevel >= level;
			if( !bUpgradable )
			{
				Game.SoundMgr.Play("Sound/Money_NoMoney.wav");
				Resources.FindStoryboard("Upgrade_Not_Available_Storyboard").Begin();
			}
			return bUpgradable;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	돈 소모
		public bool SpendGold(int requiredGold)
		{
			if( requiredGold > gold )
			{
				noMoneyStory.Begin();
				Game.SoundMgr.Play("Sound/Money_NoMoney.wav");
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
			double lastValue = power_Progress.Value;
			power_Progress.Value = power_Progress.Value + power;

			if( power_Progress.Value == power_Progress.Maximum )
			{
				switch(upgradeLevel)
				{
				case 0:
					Resources.FindStoryboard("Upg1_Completed_Storyboard").Begin();
					power_Progress.Value = 0;
					power_Progress.Maximum = Game.Setting.core.upg2Research;
					upgradeLevel++;
					break;
				case 1:
					Resources.FindStoryboard("Upg2_Completed_Storyboard").Begin();
					power_Progress.Value = 0;
					power_Progress.Maximum = Game.Setting.core.upg3Research;
					upgradeLevel++;
 					break;
				case 2:
					Resources.FindStoryboard("Upg3_Completed_Storyboard").Begin();
					power_Progress.Value = 0;
					power_Progress.Maximum = Game.Setting.core.upg4Research;
					upgradeLevel++;
					break;
				case 3:
					Resources.FindStoryboard("Upg4_Completed_Storyboard").Begin();
					Game.World.core.AttackReady();
					upgradeLevel++;
					break;
				case 4:
					if( lastValue < power_Progress.Maximum )
					{
						Game.World.core.AttackReady();	
					}
					break;
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	경고 발생
		public void DisplayWarning(int route, bool bBoss)
		{
			Game.SoundMgr.Play("Sound/Wave_Alarm.wav");
			if( bBoss ) { bossWarningStory.Begin(); }
			else
			{
				normalWarningStory.Begin();
				routeWarningStories[route].Begin();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	다음 수입 예상 표시
		public void DisplayIncome(double leftTime, int gold)
		{
			gold_NextTime_Text.Text = string.Format("{0:F1}", leftTime);
			gold_Next_Text.Text = (gold >= 0 ? "+" : "-") + gold.ToString();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타워가 지어졌다.
		public void TowerBuilt()
		{
			if( !bFirstTowerBuilt )
			{
				bFirstTowerBuilt = true;
				Resources.FindStoryboard("FirstTower_Storyboard").Begin();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	연구소가 지어졌다.
		public void LabBuilt()
		{
			if( !bFirstLabBuilt )
			{
				bFirstLabBuilt = true;
				Resources.FindStoryboard("FirstLab_Storyboard").Begin();
			}
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