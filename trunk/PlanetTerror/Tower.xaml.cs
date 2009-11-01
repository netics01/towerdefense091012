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
	//	Tower
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Tower : UserControl
	{
		//===============================================================================================================================================
		//	상수
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string BUILD_GROUP = "Build_StateGroup";
		const string NOTYETBUILT_STATE = "Build_NotYetBuilt_State";
		const string BUILT_STATE = "Build_Built_State";
		const string DISMANTLE_STATE = "Build_Dismantle_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string MENU_GROUP = "Menu_StateGroup";
		const string MENU_NOMENU_STATE = "Menu_NoMenu_State";
		const string MENU_BUILD_STATE = "Menu_Build_State";
		const string MENU_UPGRADE_STATE = "Menu_Upgrade_State";

		//===============================================================================================================================================
		//	스태틱
		static Tower selected;

		//===============================================================================================================================================
		//	필드
		Enemy target;
		Point towerCenter;
		double cooldownTime;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		double miningTime;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		VSM vsm;
		Storyboard idleStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Tower()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup(BUILD_GROUP);

			idleStory = Resources.FindStoryboard("Upg0_Tower_Storyboard");
			idleStory.RepeatBehavior = RepeatBehavior.Forever;

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
			miningTime = 0;
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
			case BUILT_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(MENU_GROUP, MENU_UPGRADE_STATE);
				}
				break;
			case DISMANTLE_STATE:
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
			int gold = Game.Setting.tower.buildCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			vsm.SetState(BUILT_STATE);
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
			idleStory.Stop();
			vsm.SetState(DISMANTLE_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
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
			case BUILT_STATE:
				if( !vsm.GetStateFinished() ) { return; }
				if( vsm.GetStateJustFinished() )
				{
					idleStory.Begin();
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
			case DISMANTLE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NOTYETBUILT_STATE);
				}
				break;
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