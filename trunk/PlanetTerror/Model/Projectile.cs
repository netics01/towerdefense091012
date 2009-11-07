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
	//	Projectile
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class Projectile : UserControl
	{
		//===============================================================================================================================================
		//	상수
		const string FIRE_STATE = "Projectile_Fire_State";
		const string BOOM_STATE = "Projectile_Boom_State";
		const string NORMAL_STATE = "Projectile_Normal_State";

		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsDeleted { get; protected set; }
		public bool IsDestroyed { get; protected set; }
		public bool IsInvalid { get { return IsDeleted || IsDestroyed; } }
		public double Damage { get; set; }
		public double Speed { get; set; }

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Enemy target;
		Point targetLastPos;
		Point pos;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Canvas layoutRoot;
		ResourceDictionary resources;
		VSM vsm;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Projectile()
		{
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Projectile_Loaded(object sender, RoutedEventArgs e)
		{
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public virtual void Initialize(Enemy target, Point pos, double angle)
		{
			throw new NotImplementedException();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case FIRE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NORMAL_STATE);
				}
				break;
			case NORMAL_STATE:
				if( !target.IsInvalid )
				{
					targetLastPos = target.Pos;
				}
				Move();
				break;
			case BOOM_STATE:
				if( vsm.GetStateFinished() )
				{
					IsDeleted = true;
				}
				break;
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		protected void Initialize(Enemy target, Point pos, double angle, Canvas layoutRoot, ResourceDictionary resources)
		{
			this.target = target;
			this.targetLastPos = target.Pos;
			this.pos = pos;
			this.layoutRoot = layoutRoot;
			this.resources = resources;
			vsm = new VSM(this, layoutRoot);

			Canvas.SetZIndex(this, Math.Max(Canvas.GetZIndex(target), (int)ELayer.Projectile));
			this.SetCenter(pos);
			vsm.SetState(FIRE_STATE);

			this.RenderTransform = new RotateTransform(angle);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	이동
		void Move()
		{
			var disp = targetLastPos - pos;
			if( disp.LengthSquared <= Speed * Speed )
			{
				var angle = Math.Atan2(disp.Y, disp.X) * 180 / Math.PI;
				this.RenderTransform = new RotateTransform(angle);

				pos = targetLastPos;
				target.TakeDamage(Damage);
				IsDestroyed = true;
				vsm.SetState(BOOM_STATE);
			}
			else
			{
				disp.Normalize();
				disp = disp * Speed;
				pos += disp;
			}
			this.SetCenter(pos);
		}
	}
}