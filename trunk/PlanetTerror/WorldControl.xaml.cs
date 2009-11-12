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
		public bool NoEnemy { get { return enemies.Count == 0; } }

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		WaveGenerator generator;
		List<Enemy> enemies;
		List<Tower> towers;
		List<Projectile> projectiles;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		List<InstantProjectile> instantProjectiles;
		ClassPool<Projectile0> proj0Pool;
		ClassPool<Projectile1> proj1Pool;
		ClassPool<Projectile2> proj2Pool;
		ClassPool<Projectile3> proj3Pool;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		int enemyLayer = 0;
		RefreshTimer goldTimer;

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
			projectiles = new List<Projectile>();
			instantProjectiles = new List<InstantProjectile>();
			proj0Pool = new ClassPool<Projectile0>(30);
			proj1Pool = new ClassPool<Projectile1>(30);
			proj2Pool = new ClassPool<Projectile2>(30);
			proj3Pool = new ClassPool<Projectile3>(30);

			PreparePath();

			Loaded += new RoutedEventHandler(WorldControl_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void WorldControl_Loaded(object sender, RoutedEventArgs e)
		{
			//위에서부터 아래로 깊이 설정
			towers.Sort((a, b) => (int)(Canvas.GetTop(a) - Canvas.GetTop(b)));
			for( int i = 0; i < towers.Count; ++i )
			{
				towers[i].SetZIndex((int)ELayer.Tower + i);
			}
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			generator.Initialize();

			goldTimer = new RefreshTimer((float)Game.Setting.gold.interval);
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
			for( int i = 0; i < instantProjectiles.Count; ++i )
			{
				instantProjectiles[i].Update(delta);
			}
			for( int i = 0; i < enemies.Count; ++i )
			{
				enemies[i].Update(delta);
			}

			UpdateTower(delta);
			core.Update(delta);

			RemoveObject();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void CreateEnemy<T>(PathGeometry path) where T : Enemy, new()
		{
			var enemy = new T();
			Canvas.SetZIndex(enemy, enemyLayer);
			enemy.Initialize(path);			
			enemies.Add(enemy);
			LayoutRoot.Children.Add(enemy);

			enemyLayer--;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포탄 생성
		public void AddProjectile(Projectile proj)
		{
			projectiles.Add(proj);
			LayoutRoot.Children.Add(proj);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타격 이펙트 생성
		public void CreateInstantProjectile(Enemy target, Point firePos, double angle, int towerLevel, double damage)
		{
			//일단 데미지를 바로 입힌다.
			target.TakeDamage(damage);

			InstantProjectile beginProj, endProj;
			switch( towerLevel )
			{
			case 0:
// 				beginProj = proj0Pool.Allocate();
// 				endProj = proj0Pool.Allocate();
				beginProj = new Projectile0();
				endProj = new Projectile0();
				break;
			case 1:
// 				beginProj = proj1Pool.Allocate();
// 				endProj = proj1Pool.Allocate();
				beginProj = new Projectile1();
				endProj = new Projectile1();
				break;
			case 2:
// 				beginProj = proj2Pool.Allocate();
// 				endProj = proj2Pool.Allocate();
				beginProj = new Projectile2();
				endProj = new Projectile2();
				break;
			default:
// 				beginProj = proj3Pool.Allocate();
// 				endProj = proj3Pool.Allocate();
				beginProj = new Projectile3();
				endProj = new Projectile3();
				break;
			}
			beginProj.Initialize(target, firePos, angle, true);
			endProj.Initialize(target, target.Pos, 0, false);

			instantProjectiles.Add(beginProj);
			instantProjectiles.Add(endProj);
			LayoutRoot.Children.Add(beginProj);
			LayoutRoot.Children.Add(endProj);

			Game.SoundMgr.Play(towerLevel >= 2 ? "Sound/Tower_Attack34.wav" : "Sound/Tower_Attack12.wav");
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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	
		public bool IsThereEnemy(Point pos, double rangeSqr)
		{
			for( int i = 0; i < enemies.Count; ++i )
			{
				if( enemies[i].IsInvalid ) { continue; }
				double distSqr = enemies[i].Pos.DistanceSqaure(pos);
				if( distSqr < rangeSqr )
				{
					return true;
				}
			}
			return false;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지
		public void SweepDamage(double leftX, double rightX, double damage)
		{
			//적이 이동하는 경우 빠져나갈수가 있다. 여유를 더 준다.
			rightX += 8;
			for( int i = 0; i < enemies.Count; ++i )
			{
				var e = enemies[i];
				if( e.IsInvalid ) { continue; }
				if( leftX < e.Pos.X && e.Pos.X <= rightX )
				{
					e.TakeDamage(damage);
					
				}
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	패스를 가공한 후 삭제한다.
		private void PreparePath()
		{
			if( this.IsInDesignMode() ) { return; }
			for( int i = 0; i < LayoutRoot.Children.Count; ++i )
			{
				var path = LayoutRoot.Children[i] as Path;
				if( path != null )
				{
					PathGeometry pg = path.Data.GetFlattenedPathGeometry();
					Routes.Add(pg);
				}
			}
			LayoutRoot.RemoveType<Path>();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	수입 관련 처리
		private void UpdateTower(float delta)
		{
			bool bGoldTime = goldTimer.Refresh(delta);
			int estimatedGold = 0;
			for( int i = 0; i < towers.Count; ++i )
			{
				towers[i].Update(delta);
				estimatedGold += towers[i].EstimateGold();
				if( bGoldTime ) { towers[i].MakeGold(); }
			}
			Game.UI.DisplayIncome(goldTimer.TimeLeft, estimatedGold);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	파괴된 객체들 삭제
		private void RemoveObject()
		{
			for( int i = 0; i < projectiles.Count; ++i )
			{
				if( projectiles[i].IsDeleted )
				{
					LayoutRoot.Children.Remove(projectiles[i]);
					projectiles.RemoveAt(i);
				}
			}
			for( int i = 0; i < instantProjectiles.Count; ++i )
			{
				var proj = instantProjectiles[i];
				if( !proj.IsDeleted ) { continue; }
				
				LayoutRoot.Children.Remove(proj);
				instantProjectiles.RemoveAt(i);

// 				var proj0 = proj as Projectile0;
// 				if( proj0 != null ) { proj0Pool.Deallocate(proj0); }
// 				var proj1 = proj as Projectile1;
// 				if( proj1 != null ) { proj1Pool.Deallocate(proj1); }
// 				var proj2 = proj as Projectile2;
// 				if( proj2 != null ) { proj2Pool.Deallocate(proj2); }
// 				var proj3 = proj as Projectile3;
// 				if( proj3 != null ) { proj3Pool.Deallocate(proj3); }
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
	}
}