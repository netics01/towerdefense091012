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
	public class Enemy : UserControl
	{
		//===============================================================================================================================================
		//	상수
		const string NO_STATE = "";
		const string HIT_STATE = "Enemy_Hit_State";
		const string DESTROY_STATE = "Enemy_Destroy_State";
		const string BOOM_STATE = "Enemy_Boom_State";

		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsDeleted { get; protected set; }
		public bool IsDestroyed { get; protected set; }
		public bool IsInvalid { get { return IsDeleted || IsDestroyed; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public Point Pos { get; protected set; }
		public double HitPoint { get; protected set; }

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Setting.Enemy setting;
		PathGeometry path;
		double routeTime;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Canvas layoutRoot;
		ResourceDictionary resources;
		VSM vsm;
		VisualState destroyState;
		VisualState boomState;
		Storyboard moveStory;		

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Enemy()
		{
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomState_Completed(object sender, EventArgs e)
		{
			IsDeleted = true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void DestroyState_Completed(object sender, EventArgs e)
		{
			IsDestroyed = true;
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public virtual void Initialize(PathGeometry path)
		{
			throw new NotImplementedException();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public virtual void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case NO_STATE:
				Move(delta);
				break;
			//case HIT_STATE:
			//    Move(delta);
			//    if( vsm.GetStateFinished() )
			//    {
			//        vsm.SetState(NO_STATE);
			//    }
			//    break;
			case DESTROY_STATE:
			case BOOM_STATE:
				if( vsm.GetStateFinished() )
				{
					IsDeleted = true;
				}
				break;
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지를 입는다.
		public virtual void TakeDamage(double damage)
		{
			if( IsInvalid ) { return; }

			HitPoint -= damage;
			if( HitPoint < 0 )
			{
				IsDestroyed = true;
				vsm.SetState(DESTROY_STATE);
				Game.UI.GainGold(setting.gold);
				Game.UI.GainPower(setting.powerUp);
			}
			//else { vsm.SetState(HIT_STATE);	}
		}

		//===============================================================================================================================================
		//	보호
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		protected void Initialize(PathGeometry path, Setting.Enemy setting, Canvas layoutRoot, ResourceDictionary resources)
		{
			this.setting = setting;

			IsDeleted = false;
			IsDestroyed = false;
			Pos = new Point(-1000, -1000);
			HitPoint = setting.hitPoint;
			this.path = path;
			routeTime = 0;

			this.layoutRoot = layoutRoot;
			this.resources = resources;
			vsm = new VSM(this, layoutRoot);
			vsm.SetDefaultGroup("Enemy_StateGroup");
			destroyState = layoutRoot.FindState("Enemy_Destroy_State");
			this.boomState = layoutRoot.FindState("Enemy_Boom_State");

// 			if( (this as Enemy2) != null )
// 			{
// 				this.moveStory = Application.Current.Resources["Enemy2_Move_Storyboard"] as Storyboard;
// 			}
// 			else
			{
				this.moveStory = resources.FindStoryboard("Move_Storyboard");
			}

			WPFUtil.SetImageScaleMode(layoutRoot, BitmapScalingMode.Linear);

			moveStory.Begin();
			//moveStory.Begin(layoutRoot);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	이동
		void Move(float delta)
		{
			routeTime += delta / setting.routeTime;
			//routeTime = Math.Min(1.0, routeTime);
			if( routeTime >= 1.0 )
			{
				vsm.SetState(BOOM_STATE, true);
				Game.World.core.TakeDamage(setting.damage);
				IsDestroyed = true;
				return;
			}

			Point pos, tangent;
			path.GetPointAtFractionLength(routeTime, out pos, out tangent);
			Pos = pos;
			this.SetCenter(pos);
		}
	}
}