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
//	Tower				[10, 100]
//	Projectile			101
//	Boss				150
//	Core				200





//----작업----


//타워 업그레이드 버튼 바로 안뜨는 문제
//코어 공격 버튼에 문제가 있다.


//보스 공격시 데미지 타임 조절


//발사체 모양 다듬는다.
//게임플레이를 다듬는다.


//이지/하드모드?
//웨이브 경로를 랜덤으로?
//마지막 웨이브는 모든 경로로?
//포탄/자코 클래스 풀링



namespace PlanetTerror
{
	//-----------------------------------------------------------------------------------------------------------------------------------------------
	//	Enum
	enum ELayer
	{
		EnemyMin = -1000,
		EnemyMax = 0,
		Tower = 10,
		TowerMax = 100,
		SelectedTower = TowerMax,
		Projectile = 101,
		Boss = 150,
		Core = 200,
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Setting
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class Setting
	{
		//===============================================================================================================================================
		//	필드
		public const int VERSION = 35;

		public int version;
		public bool debugMode;
		public bool title;
		public int startGold;

		public class Gold
		{
			public double interval;
			public int mineGold;
			public int towerGold;
			public int labGold;
		}
		public Gold gold;
		public class Tower
		{
			public int buildCost;
			public int dismantleGain;
			public double centerX;
			public double centerY;
			public double barrelLength;
			public class Stat
			{
				public double attackRange;
				public double attackRangeSqr;
				public double attackCooldown;
				public double attackDamage;
				public double projSpeed;
				public double turretRotSpeed;
				public int upgCost;
				public int dismantleGain;
			}
			public List<Stat> stats;
		}
		public Tower tower;
		public class Lab
		{
			public int buildCost;
			public double powerGain;
			public double range;
			public double rangeSqr;
			public double inactiveTime;
		}
		public Lab lab;

		public class Core
		{
			public double hitPoint;
			public double warningBefore;
			public double attackDamage;
			public double boomTime;
			public double boomDistance;
			public double boomLogicFrame;
			public double upg1Research;
			public double upg2Research;
			public double upg3Research;
			public double upg4Research;
		}
		public Core core;

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
		public Enemy enemy4;
		public class Boss
		{
			public double hitPoint;
			public double damage;
		}
		public Boss boss;

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
			lab = new Lab();
			core = new Core();
			enemy1 = new Enemy();
			enemy2 = new Enemy();
			enemy3 = new Enemy();
			enemy4 = new Enemy();
			boss = new Boss();

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

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본값 설정
		void SetDefault()
		{
			version = VERSION;
			debugMode = true;
			title = true;
			startGold = 500;

			gold.interval = 5;
			gold.mineGold = 1;
			gold.towerGold = -3;
			gold.labGold = -5;

			tower.buildCost = 50;
			tower.dismantleGain = 190;
			tower.centerX = 38;
			tower.centerY = 8;
			tower.barrelLength = 26;

			tower.stats = new List<Tower.Stat>();
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());

			tower.stats[0].attackRange = 140;
			tower.stats[0].attackCooldown = 0.5;
			tower.stats[0].attackDamage = 10;			
			tower.stats[0].projSpeed = 200;
			tower.stats[0].turretRotSpeed = 250;
			tower.stats[0].upgCost = 150;
			tower.stats[0].dismantleGain = 45;

			tower.stats[1].attackRange = 150;
			tower.stats[1].attackCooldown = 0.5;
			tower.stats[1].attackDamage = 20;			
			tower.stats[1].projSpeed = 200;
			tower.stats[1].turretRotSpeed = 1000;
			tower.stats[1].upgCost = 200;
			tower.stats[1].dismantleGain = 190;

			tower.stats[2].attackRange = 230;
			tower.stats[2].attackCooldown = 2.4;
			tower.stats[2].attackDamage = 100;
			tower.stats[2].projSpeed = 240;
			tower.stats[2].turretRotSpeed = 1000;
			tower.stats[2].upgCost = 300;
			tower.stats[2].dismantleGain = 380;

			tower.stats[3].attackRange = 250;
			tower.stats[3].attackCooldown = 1.2;
			tower.stats[3].attackDamage = 55;
			tower.stats[3].projSpeed = 350;
			tower.stats[3].turretRotSpeed = 210;
			tower.stats[3].upgCost = 1000;
			tower.stats[3].dismantleGain = 640;

			lab.buildCost = 200;
			lab.powerGain = 1;
			lab.range = 150;
			lab.rangeSqr = 0;
			lab.inactiveTime = 5;

			core.hitPoint = 100;
			core.warningBefore = 8;
			core.attackDamage = 700;
			core.boomTime = 2;
			core.boomDistance = 130;
			core.boomLogicFrame = 0.1;
			core.upg1Research = 50;
			core.upg2Research = 120;
			core.upg3Research = 200;
			core.upg4Research = 500;

			enemy1.routeTime = 35;
			enemy1.hitPoint = 30;
			enemy1.damage = 10;
			enemy1.gold = 0;
			enemy1.powerUp = 0.5;

			enemy2.routeTime = 50;
			enemy2.hitPoint = 200;
			enemy2.damage = 10;
			enemy2.gold = 0;
			enemy2.powerUp = 1;

			enemy3.routeTime = 18;
			enemy3.hitPoint = 80;
			enemy2.damage = 10;
			enemy3.gold = 0;
			enemy3.powerUp = 2;

			enemy4.routeTime = 27;
			enemy3.hitPoint = 110;
			enemy2.damage = 10;
			enemy3.gold = 0;
			enemy3.powerUp = 1;

			boss.hitPoint = 2000;
			boss.damage = 150;

			var w = MakeWave(17);
			MakeBundle(w, 0.1, "Enemy1", 1, 5, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy1", 1, 10, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 2, 5, 1.5);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 3, 5, 1.5);
			MakeBundle(w, 5, "Enemy1", 3, 5, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 4, 10, 1.5);
			MakeBundle(w, 5, "Enemy1", 4, 10, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy3", 5, 5, 1);

			w = MakeWave(25);
			MakeBundle(w, 0.1, "Enemy2", 6, 15, 1.5);
			MakeBundle(w, 5, "Enemy1", 6, 15, 1);
			MakeBundle(w, 5, "Enemy1", 6, 15, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy1", 7, 20, 1.5);
			MakeBundle(w, 4, "Enemy2", 7, 20, 1);
			MakeBundle(w, 4, "Enemy1", 7, 20, 1);
			MakeBundle(w, 4, "Enemy1", 7, 20, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 8, 20, 1.5);
			MakeBundle(w, 4, "Enemy3", 8, 10, 1);
			MakeBundle(w, 4, "Enemy2", 8, 20, 1.5);
			MakeBundle(w, 4, "Enemy3", 8, 15, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 7, 30, 1.5);
			MakeBundle(w, 3, "Enemy3", 7, 30, 1);
			MakeBundle(w, 3, "Enemy2", 7, 5, 1);

			w = MakeWave(30);
			MakeBundle(w, 0.1, "Enemy2", 6, 30, 1.5);
			MakeBundle(w, 3, "Enemy2", 6, 35, 1);
			MakeBundle(w, 3, "Enemy3", 6, 10, 1);

			w = MakeWave(25);
			MakeBundle(w, 3, "Enemy3", 5, 30, 1);

			w = MakeWave(20);
			MakeBundle(w, 0.1, "Enemy2", 4, 40, 1.5);
			MakeBundle(w, 2, "Enemy3", 4, 30, 1);
			MakeBundle(w, 2, "Enemy3", 4, 20, 1);

			w = MakeWave(20);
			MakeBundle(w, 0.1, "Enemy2", 3, 40, 1);
			MakeBundle(w, 1, "Enemy3", 3, 30, 0.5);
			MakeBundle(w, 1, "Enemy1", 3, 20, 1);
			MakeBundle(w, 1, "Enemy2", 3, 40, 1);
			MakeBundle(w, 1, "Enemy3", 3, 40, 0.6);
			MakeBundle(w, 1, "Enemy1", 3, 20, 1);

			w = MakeWave(40);
			MakeBundle(w, 3, "Boss1", 1, 1, 0.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	웨이브 생성
		Wave MakeWave(double waitTime)
		{
			var w = new Wave();
			w.waitTime = waitTime;
			w.bundles = new List<Wave.Bundle>();
			waves.Add(w);
			return w;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	번들 생성
		Wave.Bundle MakeBundle(Wave w, double waitTime, string typeName, int pathIndex, int count, double interval)
		{
			var b = new Wave.Bundle();
			b.waitTime = waitTime;
			b.typeName = typeName;
			b.pathIndex = pathIndex;
			b.count = count;
			b.interval = interval;
			w.bundles.Add(b);
			return b;
		}	
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	계산할 값 수정
		void Adjust()
		{
			Debug.Assert( tower.stats.Count == 4 );
			for( int i = 0; i < tower.stats.Count; ++i )
			{
				tower.stats[i].attackRangeSqr = tower.stats[i].attackRange * tower.stats[i].attackRange;
			}
			lab.rangeSqr = lab.range * lab.range;

			for( int i = 0; i < waves.Count; ++i )
			{
				for( int j = 0; j < waves[i].bundles.Count; ++j )
				{
					waves[i].bundles[j].pathIndex--;
				}
			}
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
		public static SoundMgr SoundMgr;
	}
}