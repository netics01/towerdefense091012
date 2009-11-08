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
		List<Projectile> projectiles;
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
			for( int i = 0; i < enemies.Count; ++i )
			{
				enemies[i].Update(delta);
			}

			UpdateTower(delta);

			//객체 제거
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
		public void AddProjectile(Projectile proj)
		{
			projectiles.Add(proj);
			LayoutRoot.Children.Add(proj);
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
		//	적 제거
// 		void DeleteEnemy(Enemy1 enemy)
// 		{
// 			LayoutRoot.Children.Remove(enemy);
// 		}

	}
}