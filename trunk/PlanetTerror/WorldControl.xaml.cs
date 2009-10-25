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
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	WorldControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class WorldControl : UserControl
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static WorldControl Instance { get; protected set; }

		//===============================================================================================================================================
		//	프로퍼티
		public List<PathGeometry> Routes { get; protected set; }

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		List<Enemy1> enemy1s;
		List<Tower> towers;
		List<Projectile> projectiles;
		

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WorldControl()
		{
			this.InitializeComponent();

			Instance = this;
			Routes = new List<PathGeometry>();

			enemy1s = new List<Enemy1>();
			towers = new List<Tower>();
			for( int i = 0; i < LayoutRoot.Children.Count; ++i )
			{
				var tower = LayoutRoot.Children[i] as Tower;
				if( tower != null ) { towers.Add(tower); }
			}
			projectiles = new List<Projectile>();

			PreparePath();			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			for( int i = 0; i < projectiles.Count; ++i )
			{
				projectiles[i].Update(delta);
			}
			for( int i = 0; i < enemy1s.Count; ++i )
			{
				enemy1s[i].Update(delta);
			}
			for( int i = 0; i < towers.Count; ++i  )
			{
				towers[i].Update(delta);
			}

			for( int i = 0; i < projectiles.Count; ++i )
			{
				if( projectiles[i].IsDeleted )
				{
					LayoutRoot.Children.Remove(projectiles[i]);
					projectiles.RemoveAt(i);
				}
			}
			for( int i = 0; i < enemy1s.Count; ++i )
			{
				if( enemy1s[i].IsDeleted )
				{
					LayoutRoot.Children.Remove(enemy1s[i]);
					enemy1s.RemoveAt(i);
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void CreateEnemy1(PathGeometry path)
		{
			var enemy = new Enemy1(path);
			Canvas.SetZIndex(enemy, 0);
			enemy1s.Add(enemy);
			LayoutRoot.Children.Add(enemy);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포탄 생성
		public Projectile CreateProjectile(Enemy1 target, Point pos)
		{
			var p = new Projectile(target, pos);
			Canvas.SetZIndex(p, 6);
			projectiles.Add(p);
			LayoutRoot.Children.Add(p);
			return p;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타겟 검색
		public Enemy1 FindTarget(Point pos, double rangeSqr)
		{
			Enemy1 target = null;
			double minDist = 100000000.0;
			for( int i = 0; i < enemy1s.Count; ++i )
			{
				if( enemy1s[i].IsInvalid ) { continue; }
				double distSqr = enemy1s[i].Pos.DistanceSqaure(pos);
				if( distSqr < rangeSqr &&
					distSqr < minDist )
				{
					target = enemy1s[i];
					minDist = distSqr;
				}
			}
			return target;
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	패스를 모두 찾아서 안보이게 만든다.
		private void PreparePath()
		{
			for( int i = 0; i < LayoutRoot.Children.Count; ++i )
			{
				var path = LayoutRoot.Children[i] as Path;
				if( path != null )
				{
					PathGeometry pg = path.Data.GetFlattenedPathGeometry();
					path.SetVisible(false);
					Routes.Add(pg);
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 제거
// 		void DeleteEnemy(Enemy1 enemy)
// 		{
// 			LayoutRoot.Children.Remove(enemy);
// 		}
	}
}