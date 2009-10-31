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
	//	WorldControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class WorldControl : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public List<PathGeometry> Routes { get; protected set; }		

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		WaveGenerator generator;
		List<Enemy> enemies;
		List<Tower> towers;
		List<Projectile3> projectiles;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		int enemyLayer = 0;		

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WorldControl()
		{
			this.InitializeComponent();

			generator = new WaveGenerator();
			Routes = new List<PathGeometry>();

			Game.Generator = generator;

			enemies = new List<Enemy>();
			towers = new List<Tower>();
			for( int i = 0; i < LayoutRoot.Children.Count; ++i )
			{
				var tower = LayoutRoot.Children[i] as Tower;
				if( tower != null ) { towers.Add(tower); }
			}
			projectiles = new List<Projectile3>();

			PreparePath();			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			core.Initialize();			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			generator.Update(delta);
			for( int i = 0; i < projectiles.Count; ++i )
			{
				projectiles[i].Update(delta);
			}
			for( int i = 0; i < enemies.Count; ++i )
			{
				enemies[i].Update(delta);
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
			for( int i = 0; i < enemies.Count; ++i )
			{
				if( enemies[i].IsDeleted )
				{
					LayoutRoot.Children.Remove(enemies[i]);
					enemies.RemoveAt(i);
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void CreateEnemy<T>(PathGeometry path) where T : Enemy, new()
		{
			var enemy = new T();
			enemy.Initialize(path);
			Canvas.SetZIndex(enemy, enemyLayer);
			enemies.Add(enemy);
			LayoutRoot.Children.Add(enemy);

			enemyLayer--;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포탄 생성
		public Projectile3 CreateProjectile(Enemy target, Point pos)
		{
			var p = new Projectile3(target, pos);
			p.Damage = Game.Setting.tower.attackRange;
			p.Speed = Game.Setting.proj1.speed;

			Canvas.SetZIndex(p, 6);
			projectiles.Add(p);
			LayoutRoot.Children.Add(p);
			return p;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타겟 검색
		public Enemy FindTarget(Point pos, double rangeSqr)
		{
			Enemy target = null;
			double minDist = 100000000.0;
			for( int i = 0; i < enemies.Count; ++i )
			{
				if( enemies[i].IsInvalid ) { continue; }
				double distSqr = enemies[i].Pos.DistanceSqaure(pos);
				if( distSqr < rangeSqr &&
					distSqr < minDist )
				{
					target = enemies[i];
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