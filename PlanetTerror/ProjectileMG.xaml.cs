﻿using System;
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
	public partial class ProjectileMG : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsDeleted { get; protected set; }
		public bool IsDestroyed { get; protected set; }
		public bool IsInvalid { get { return IsDeleted || IsDestroyed; } }
		public double Damage { get; set; }

		//===============================================================================================================================================
		//	필드
		Enemy1 target;
		Point targetLastPos;
		Point pos;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public ProjectileMG(Enemy1 target, Point pos)
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
			if( IsInvalid ) { return; }

			if( !target.IsInvalid )
			{
				targetLastPos = target.Pos;
			}

			var disp = targetLastPos - pos;
			var speed = 100;
			if( disp.LengthSquared <= speed * speed )
			{
				pos = targetLastPos;
				target.TakeDamage(Damage);
				IsDestroyed = true;
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