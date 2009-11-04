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
	//	Tower
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Tower : UserControl
	{
		//===============================================================================================================================================
		//	상수
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string BUILD_GROUP = "Build_StateGroup";
		const string NOTYETBUILT_STATE = "Build_NotYetBuilt_State";
		const string TOWER_BUILT_STATE = "Build_Tower_Built_State";
		const string TOWER_DISMANTLE_STATE = "Build_Tower_Dismantle_State";
		const string LAB_BUILT_STATE = "Build_Lab_Built_State";
		const string LAB_DISMANTLE_STATE = "Build_Lab_Dismantle_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string MENU_GROUP = "Menu_StateGroup";
		const string MENU_NOMENU_STATE = "Menu_NoMenu_State";
		const string MENU_BUILD_STATE = "Menu_Build_State";
		const string MENU_UPGRADE_STATE = "Menu_Upgrade_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string GOLD_GROUP = "Gold_StateGroup";
		const string GOLD_GAIN_STATE = "Gold_Gain_State";
		const string GOLD_LOSE_STATE = "Gold_Lose_State";
		const string GOLD_NORMAL_STATE = "Gold_Normal_State";

		//===============================================================================================================================================
		//	스태틱
		static Tower selected;

		//===============================================================================================================================================
		//	필드
		Enemy target;
		Point towerCenter;
		double cooldownTime;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		VSM vsm;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		List<Storyboard> effectStories;
		Storyboard labStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Tower()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup(BUILD_GROUP);

			effectStories = new List<Storyboard>();
			effectStories.Add(Resources.FindStoryboard("Upg0_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg1_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg2_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg3_Tower_Storyboard"));
			for( int i = 0; i < effectStories.Count; ++i )
			{
				effectStories[i].RepeatForever();
			}

			labStory = Resources.FindStoryboard("Lab_Tower_Storyboard");
			labStory.RepeatForever();

			Loaded += new RoutedEventHandler(Tower_Loaded);
			MouseEnter += new MouseEventHandler(Tower_MouseEnter);
			MouseLeave += new MouseEventHandler(Tower_MouseLeave);
			//Menu_Build_Button.MouseLeave +=new MouseEventHandler(Menu_Build_Button_MouseLeave);
			//Menu_Upgrade_Button.MouseLeave +=new MouseEventHandler(Menu_Upgrade_Button_MouseLeave);
			//Menu_UpgradeBig_Button.MouseLeave +=new MouseEventHandler(Menu_UpgradeBIG_Button_MouseLeave);
			//Menu_Dismantle_Button.MouseLeave +=new MouseEventHandler(Menu_Dismantle_Button_MouseLeave);
			Menu_Build_Button.Click += new RoutedEventHandler(Menu_Build_Button_Click);
			Menu_Upgrade_Button.Click += new RoutedEventHandler(Menu_Upgrade_Button_Click);
			Menu_UpgradeBig_Button.Click += new RoutedEventHandler(Menu_UpgradeBIG_Button_Click);
			Menu_Dismantle_Button.Click += new RoutedEventHandler(Menu_Dismantle_Button_Click);
			Menu_Lab_Button.Click += new RoutedEventHandler(Menu_Lab_Button_Click);
			
			Gold_Gain_State.Storyboard.Completed += new EventHandler(GoldGain_Storyboard_Completed);
			Gold_Lose_State.Storyboard.Completed += new EventHandler(GoldLose_Storyboard_Completed);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			towerCenter = this.GetCenter();
			vsm.SetState(NOTYETBUILT_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseEnter(object sender, MouseEventArgs e)
		{
			Select(this);
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				vsm.SetState(MENU_GROUP, MENU_BUILD_STATE);
				break;
			case TOWER_BUILT_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(MENU_GROUP, MENU_UPGRADE_STATE);
				}
				break;
			case TOWER_DISMANTLE_STATE:
				vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
				break;
			}			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseLeave(object sender, MouseEventArgs e)
		{
			Select(null);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Build_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Tower_MouseLeave(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Upgrade_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Tower_MouseLeave(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_UpgradeBIG_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Tower_MouseLeave(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Dismantle_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Tower_MouseLeave(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Build_Button_Click(object sender, RoutedEventArgs e)
		{
			int gold = Game.Setting.tower.towerCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			vsm.SetState(TOWER_BUILT_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Upgrade_Button_Click(object sender, RoutedEventArgs e)
		{
			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_UpgradeBIG_Button_Click(object sender, RoutedEventArgs e)
		{
			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Dismantle_Button_Click(object sender, RoutedEventArgs e)
		{
			int gold = Game.Setting.tower.dismantleCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			effectStories[0].Stop();
			vsm.SetState(TOWER_DISMANTLE_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GoldGain_Storyboard_Completed(object sender, EventArgs e)
		{
			vsm.SetState(GOLD_GROUP, GOLD_NORMAL_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GoldLose_Storyboard_Completed(object sender, EventArgs e)
		{
			vsm.SetState(GOLD_GROUP, GOLD_NORMAL_STATE);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				break;
			case TOWER_BUILT_STATE:
				if( !vsm.GetStateFinished() ) { return; }
				if( vsm.GetStateJustFinished() )
				{
					effectStories[0].Begin();
				}
				if( cooldownTime > 0 )
				{
					cooldownTime -= delta;
					break;
				}
				//타겟 설정
				if( target == null ||
					target.IsInvalid ||
					target.Pos.DistanceSqaure(towerCenter) >= Game.Setting.tower.attackRangeSqr )
				{
					target = Game.World.FindTarget(towerCenter, Game.Setting.tower.attackRangeSqr);
				}
				//타겟이 있으면 타겟 공격
				if( target != null )
				{
					var projectile = Game.World.CreateProjectile(target, towerCenter);
					projectile.Damage = Game.Setting.tower.attackDamage;

					cooldownTime = Game.Setting.tower.attackCooldown;
				}
				break;
			case TOWER_DISMANTLE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NOTYETBUILT_STATE);
					effectStories[0].Stop();
				}
				break;
			case LAB_BUILT_STATE:
				if( vsm.GetStateJustFinished() )
				{
					labStory.Begin();
				}
			case LAB_DISMANTLE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NOTYETBUILT_STATE);
					labStory.Stop();
				}
				break;
			}			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	골드타임
		public void MakeGold()
		{
			int gold = 0;
			double power = 0;
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				gold = Game.Setting.gold.mineGold;
				break;
			case LAB_BUILT_STATE:
				gold = Game.Setting.gold.labGold;
				power = Game.Setting.gold.labPower;
				break;
			}
			if( gold > 0 )
			{
				vsm.SetState(GOLD_GROUP, GOLD_GAIN_STATE);
				goldGain_Text.Text = "+" + gold.ToString();
				Game.UI.GainGold(gold);
			}
			else if( gold < 0 )
			{
				vsm.SetState(GOLD_GROUP, GOLD_LOSE_STATE);
				goldLose_Text.Text = gold.ToString();
				Game.UI.GainGold(gold);
			}
			if( power > 0 )
			{
				vsm.SetState(GOLD_GROUP, GOLD_GAIN_STATE);
				goldGain_Text.Text = "+" + power.ToString();
				Game.UI.GainPower(power);
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	활성 타워
		static void Select(Tower t)
		{
			if( selected != null )
			{
				Canvas.SetZIndex(selected, (int)ELayer.Tower);
			}
			selected = t;
			if( selected != null )
			{
				Canvas.SetZIndex(selected, (int)ELayer.SelectedTower);
			}
		}
	}
}