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
	//	Enemy1
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Enemy1 : UserControl
	{
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
		PathGeometry path;
		double acc;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Enemy1(PathGeometry path)
		{
			this.InitializeComponent();

			IsDeleted = false;
			Pos = new Point(-1000, -1000);
			HitPoint = Setting.Instance.enemy1.hitPoint;
			this.path = path;
			acc = 0;
			
			Loaded += new RoutedEventHandler(Enemy1_Loaded);
			Enemy_Boom_State.Storyboard.Completed += new EventHandler(BoomState_Completed);
			Enemy_Destroy_State.Storyboard.Completed +=new EventHandler(DestroyState_Completed);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Enemy1_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			var story = Resources["Move_Storyboard"] as Storyboard;
			story.Begin();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomState_Completed(object sender, EventArgs e)
		{
			IsDeleted = true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void DestroyState_Completed(object sender, EventArgs e)
		{
			IsDeleted = true;
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			if( acc >= 1.0 || IsDestroyed ) { return; }

 			acc += delta / Setting.Instance.enemy1.routeTime;
			if( acc >= 1.0 )
			{
				this.SetState("Enemy_Boom_State", true);
				return;
			}

			Point pos, tangent;
			path.GetPointAtFractionLength(acc, out pos, out tangent);
			Pos = pos;
			this.SetCenter(pos);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지를 입는다.
		public void TakeDamage(double damage)
		{
			if( IsInvalid ) { return; }

			HitPoint -= damage;
			if( HitPoint < 0 )
			{
				IsDestroyed = true;
				this.SetState("Enemy_Destroy_State", true);
			}
		}
	}
}