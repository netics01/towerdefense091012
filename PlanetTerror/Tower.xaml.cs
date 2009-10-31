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

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Tower
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Tower : UserControl
	{
		//===============================================================================================================================================
		//	필드
		Enemy1 target;
		Point towerCenter;
		double cooldownTime;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Tower()
		{
			this.InitializeComponent();

			Loaded += new RoutedEventHandler(Tower_Loaded);
			MouseEnter += new MouseEventHandler(Tower_MouseEnter);
			MouseLeave += new MouseEventHandler(Tower_MouseLeave);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);
			//this.SetState("Build_NotYetBuilt_State", true);
			this.SetState("Build_Built_State", true);

			towerCenter = this.GetCenter();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseEnter(object sender, MouseEventArgs e)
		{
			this.SetState("Menu_Build_State", true);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseLeave(object sender, MouseEventArgs e)
		{
			this.SetState("Menu_NoMenu_State", true);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
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
// 				projectile.Damage = Setting.Instance.tower_AttackDamage;
// 
// 				cooldownTime = Setting.Instance.tower_AttackCooldown;
			}
		}
	}
}