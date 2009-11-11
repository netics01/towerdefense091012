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
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Boss1
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	데브캣 고양이
	public partial class Boss1 : Enemy
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Boss1()
		{
			this.InitializeComponent();

			Loaded += new RoutedEventHandler(Boss1_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Boss1_Loaded(object sender, RoutedEventArgs e)
		{
			LayoutRoot.Children.Remove(background_Image);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BossMoveState_Storyboard_Completed(object sender, EventArgs e)
		{

		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public override void Initialize(PathGeometry path)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			//Boss_Move_State.Storyboard.Completed += new EventHandler(BossMoveState_Storyboard_Completed);

			IsDeleted = false;
			IsDestroyed = false;
			this.SetLeftTop(0, 0);
			this.SetZIndex((int)ELayer.Boss);
			HitPoint = Game.Setting.boss.hitPoint;

			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup("Boss_StateGroup");
			vsm.SetState("Boss_Move_State");
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public override void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case "Boss_Move_State":
				var bodyCenter = body.GetCenter();
				var bodyCanvasPos = body_canvas.GetLeftTop();
				Pos = new Point(bodyCenter.X + bodyCanvasPos.X, bodyCenter.Y + bodyCanvasPos.Y);

				Debug.Print("{0}", Pos);

				if( vsm.GetStateJustFinished() )
				{
					IsDestroyed = true;		//더이상 공격받지 못하게 한다.
					if( HitPoint < 0 )
					{
						vsm.SetState("Boss_Ending_State");
					}
					else
					{
						vsm.SetState("Boss_GameOver_State");
					}
				}
				break;
			case "Boss_Ending_State":
				if( vsm.GetStateJustFinished() )
				{
					IsDeleted = true;
				}
				break;
			case "Boss_GameOver_State":
				if( vsm.GetStateFinished() )
				{
					IsDeleted = true;
					Game.World.core.TakeDamage(Game.Setting.boss.damage);
				}
				break;
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지를 입는다.
		public override void TakeDamage(double damage)
		{
			HitPoint -= damage;
		}
	}
}