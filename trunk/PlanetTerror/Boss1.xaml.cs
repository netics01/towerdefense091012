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
			Game.SoundMgr.Play("Sound/Boss_Move.mp3");
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public override void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case "Boss_Move_State":
				var bodyPos = new Point(body.ActualWidth * 0.5, body.ActualHeight * 0.5);
				var bodyTransPos = ExtractTranslation(body.RenderTransform, 3);

				var bodyCanvasPos = body_canvas.GetLeftTop();
				var bodyCanvasTransPos = ExtractTranslation(body_canvas.RenderTransform, 3);
				Pos = Add(Add(bodyPos, bodyTransPos), Add(bodyCanvasPos, bodyCanvasTransPos));

				if( vsm.GetStateJustFinished() )
				{
					IsDestroyed = true;		//더이상 공격받지 못하게 한다.
					if( HitPoint < 0 )
					{
						vsm.SetState("Boss_Ending_State");
						Game.SoundMgr.Play("Sound/Boss_ending.mp3");
					}
					else
					{
						vsm.SetState("Boss_GameOver_State");
						Game.SoundMgr.Play("Sound/Boss_GameOver.wav");
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

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	렌더트랜스폼에서 트랜슬레이션을 가져온다.
		Point ExtractTranslation(Transform renderTransform, int i)
		{
			var transformGroup = renderTransform as TransformGroup;
			var translateTransform = transformGroup.Children[i] as TranslateTransform;
			return new Point(translateTransform.X, translateTransform.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Point 덧셈. 나 원 별걸 다 만들어야 하네
		Point Add(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}