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
	//	Projectile3
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Projectile3 : UserControl
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
		Enemy target;
		Point targetLastPos;
		Point pos;
		VSM vsm;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Projectile3(Enemy target, Point pos)
		{
			this.InitializeComponent();

			this.target = target;
			this.targetLastPos = target.Pos;
			this.pos = pos;
			vsm = new VSM(this, LayoutRoot);

			Loaded += new RoutedEventHandler(Projectile_Loaded);
			//Projectile_Boom_State.Storyboard.Completed += new EventHandler(BoomState_Complete);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Projectile_Loaded(object sender, RoutedEventArgs e)
		{
			this.SetCenter(pos);
			vsm.SetState(FIRE_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomState_Complete(object sender, EventArgs e)
		{
			IsDeleted = true;	
		}

		//===============================================================================================================================================
		//	공용
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
		//	이동
		void Move()
		{
			var disp = targetLastPos - pos;
			if( disp.LengthSquared <= Speed * Speed )
			{
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