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
	//	Projectile
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Projectile : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public bool IsDeleted { get; protected set; }
		public double Damage { get; set; }

		//===============================================================================================================================================
		//	필드
		Enemy1 target;
		Point targetLastPos;
		Point pos;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Projectile(Enemy1 target, Point pos)
		{
			this.InitializeComponent();

			this.target = target;
			this.targetLastPos = target.Pos;
			this.pos = pos;

			Loaded += new RoutedEventHandler(Projectile_Loaded);
			Projectile_Boom_State.Storyboard.Completed += new EventHandler(BoomState_Complete);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Projectile_Loaded(object sender, RoutedEventArgs e)
		{
			this.SetCenter(pos);
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
			if( IsDeleted ) { return; }

			if( !target.IsInvalid )
			{
				targetLastPos = target.Pos;
			}

			var disp = targetLastPos - pos;
			var speed = SettingXml.Instance.projectile_speed;
			if( disp.LengthSquared <= speed * speed )
			{
				pos = targetLastPos;
				target.TakeDamage(Damage);
				this.SetState("Projectile_Boom_State", true);
			}
			else
			{
				disp.Normalize();
				disp = disp * speed;
				pos += disp;
			}
			this.SetCenter(pos);
		}
	}
}