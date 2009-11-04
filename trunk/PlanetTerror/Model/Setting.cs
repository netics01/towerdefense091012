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



//----레이어----
//	Background			-2001
//	Enemy				[-1000, 0]
//	Roof				5
//	Projectile			6
//	Tower				[10, 100]
//	Core				105
//	Boss				200




//----작업----



//돈이 없습니다. 표시해준다.
//타워 업그레이드
//합체
//타워 방향 돌아가기
//포탄 발사할때 이펙트
//다양한 발사체
//웨이브 기술
//보스 처리





//----이따가----
//하늘 흘러가기
//배경음악
//효과음
//에네미 풀링
//폰트 배포
//





namespace PlanetTerror
{
	//-----------------------------------------------------------------------------------------------------------------------------------------------
	//	Enum
	enum ELayer
	{
		Tower = 10,
		TowerMax = 100,
		SelectedTower = TowerMax
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Setting
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class Setting
	{
		//===============================================================================================================================================
		//	필드
		public const int VERSION = 9;

		public int version;
		public int startGold;
		public double powerMax;

		public class Gold
		{
			public double interval;
			public int mineGold;
			public int labGold;
			public double labPower;
		}
		public Gold gold;
		public class Tower
		{
			public int towerCost;
			public int labCost;
			public int dismantleCost;
			public int upgrade1Cost;
			public int upgrade2Cost;
			public int upgrade3Cost;
			public double attackRange;
			public double attackRangeSqr;
			public double attackDamage;
			public double attackCooldown;			
		}
		public Tower tower;

		public class Core
		{
			public double hitPoint;
		}
		public Core core;

		public class Projectile
		{
			public double speed;
		}
		public Projectile proj1;

		public class Enemy
		{
			public double routeTime;
			public double hitPoint;
			public double damage;
			public int gold;
			public double powerUp;
		}
		public Enemy enemy1;
		public Enemy enemy2;
		public Enemy enemy3;

		public class Wave
		{
			public double waitTime;

			public class Bundle
			{
				public double waitTime;
				public string typeName;
				public int pathIndex;
				public int count;
				public double interval;
			}
			public List<Bundle> bundles;			
		}
		public List<Wave> waves;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Setting()
		{
			gold = new Gold();
			tower = new Tower();
			core = new Core();
			proj1 = new Projectile();
			enemy1 = new Enemy();
			enemy2 = new Enemy();
			enemy3 = new Enemy();

			waves = new List<Wave>();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public static void Initialize()
		{
			Setting setting = null;
			try { setting = Helper.Deserialize<Setting>("setting.xml"); }
			catch( Exception ) {}

			if( setting == null ||
				setting.version != VERSION )
			{
				setting = new Setting();
				setting.SetDefault();
				try { Helper.Serialize("setting.xml", setting); }
				catch( Exception ) {}
			}

			setting.Adjust();
			Game.Setting = setting;
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업그레이드 비용을 얻는다.
		public int GetUpgradeCost(int level)
		{
			switch( level )
			{
			case 0:
				return 0;
			case 1:
				return tower.upgrade1Cost;
			case 2:
				return tower.upgrade2Cost;
			case 3:
				return tower.upgrade3Cost;
			default:
				throw new Exception("Can't reach here");
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본값 설정
		void SetDefault()
		{
			version = VERSION;
			startGold = 500;
			powerMax = 100;

			gold.interval = 15;
			gold.mineGold = 5;
			gold.labGold = -5;
			gold.labPower = 2.5;

			tower.towerCost = 50;
			tower.labCost = 200;
			tower.dismantleCost = -30;
			tower.upgrade1Cost = 150;
			tower.upgrade2Cost = 200;
			tower.upgrade3Cost = 250;
			tower.attackRange = 100;
			tower.attackDamage = 10;
			tower.attackCooldown = 1;

			core.hitPoint = 100;

			proj1.speed = 20;

			enemy1.routeTime = 25;
			enemy1.hitPoint = 20;
			enemy1.damage = 2;
			enemy1.gold = 0;
			enemy1.powerUp = 0.5;

			enemy2.routeTime = 50;
			enemy2.hitPoint = 100;
			enemy2.damage = 5;
			enemy2.gold = 0;
			enemy2.powerUp = 1;

			enemy3.routeTime = 10;
			enemy3.hitPoint = 70;
			enemy2.damage = 5;
			enemy3.gold = 0;
			enemy3.powerUp = 2;

			var wave = new Wave();
			wave.waitTime = 34;
			wave.bundles = new List<Wave.Bundle>();
			var bundle = new Wave.Bundle();
			bundle.waitTime = 0.1;
			bundle.typeName = "Enemy1";
			bundle.pathIndex = 2;
			bundle.count = 5;
			bundle.interval = 0.5;
			wave.bundles.Add(bundle);
			waves.Add(wave);

			for( int i = 0; i < 3; ++i )
			{
				wave = new Wave();
				wave.waitTime = 5.0;
				wave.bundles = new List<Wave.Bundle>();
				for( int j = 0; j < 3; ++j )
				{
					var b = new Wave.Bundle();
					b.waitTime = 0.1;
					if( j == 0 ) { b.typeName = "Enemy1"; }
					else if( j == 1 ) { b.typeName = "Enemy2"; }
					else { b.typeName = "Enemy3"; }

					b.pathIndex = RandomH.Next(3);
					b.count = 5;
					b.interval = 1.5;
					wave.bundles.Add(b);
				}

				waves.Add(wave);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	계산할 값 수정
		void Adjust()
		{
			tower.attackRangeSqr = tower.attackRange * tower.attackRange;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Game
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	귀찮다. 싱글턴 여기 다 모으자 -_-;
	public static class Game
	{
		public static Setting Setting;
		public static WorldControl World;
		public static UIPanelControl UI;
		public static MainWindow MainWindow;
		public static WaveGenerator Generator;
	}
}