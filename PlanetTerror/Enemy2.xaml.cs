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
	//	Enemy
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Enemy2 : Enemy
	{
		//===============================================================================================================================================
		//	필드

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Enemy2()
		{
			this.InitializeComponent();

			Loaded += new RoutedEventHandler(Enemy2_Loaded);
			//Enemy_Boom_State.Storyboard.Completed += new EventHandler(BoomState_Completed);
			//Enemy_Destroy_State.Storyboard.Completed +=new EventHandler(DestroyState_Completed);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Enemy2_Loaded(object sender, RoutedEventArgs e)
		{
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomState_Completed(object sender, EventArgs e)
		{
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void DestroyState_Completed(object sender, EventArgs e)
		{
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public override void Initialize(PathGeometry path)
		{
			Initialize(path, Game.Setting.enemy2, LayoutRoot, Resources);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public override void Update(float delta)
		{
			base.Update(delta);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지를 입는다.
		public override void TakeDamage(double damage)
		{
			base.TakeDamage(damage);
		}
	}
}