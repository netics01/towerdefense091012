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
		const string BUILD_GROUP = "Build_StateGroup";
		const string NOTYETBUILT_STATE = "Build_NotYetBuilt_State";
		const string BUILT_STATE = "Build_Built_State";
		const string DISMANTLE_STATE = "Build_Dismantle_State";
		const string MENU_GROUP = "Menu_StateGroup";
		const string MENU_NOMENU_STATE = "Menu_NoMenu_State";
		const string MENU_BUILD_STATE = "Menu_Build_State";

		//===============================================================================================================================================
		//	필드
		Enemy target;
		Point towerCenter;
		double cooldownTime;
		VSM vsm;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Tower()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup(BUILD_GROUP);

			var idleStory = Resources.FindStoryboard("Idle_Storyboard");
			idleStory.RepeatBehavior = RepeatBehavior.Forever;

			Loaded += new RoutedEventHandler(Tower_Loaded);
			MouseEnter += new MouseEventHandler(Tower_MouseEnter);
			MouseLeave += new MouseEventHandler(Tower_MouseLeave);
			Menu_Build_Button.MouseLeave +=new MouseEventHandler(Menu_Build_Button_MouseLeave);
			Menu_Upgrade_Button.MouseLeave +=new MouseEventHandler(Menu_Upgrade_Button_MouseLeave);
			Menu_UpgradeBIG_Button.MouseLeave +=new MouseEventHandler(Menu_UpgradeBIG_Button_MouseLeave);
			Menu_Dismantle_Button.MouseLeave +=new MouseEventHandler(Menu_Dismantle_Button_MouseLeave);
		}
		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			vsm.SetState(NOTYETBUILT_STATE);

			towerCenter = this.GetCenter();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseEnter(object sender, MouseEventArgs e)
		{
			vsm.SetState(MENU_GROUP, MENU_BUILD_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseLeave(object sender, MouseEventArgs e)
		{
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

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			return;
			if( cooldownTime > 0 )
			{
				cooldownTime -= delta;
				return;
			}
			//타겟 설정
			if( target == null || target.IsInvalid )
			{
				target = WorldControl.Instance.FindTarget(towerCenter, Setting.Instance.tower.attackRangeSqr);
			}
			//타겟이 있으면 타겟 공격
			if( target != null )
			{
// 				var projectile = WorldControl.Instance.CreateProjectile(target, towerCenter);
// 				projectile.Damage = Setting.Instance.tower.attackDamage;
// 
// 				cooldownTime = Setting.Instance.tower.attackCooldown;
			}
		}
	}
}