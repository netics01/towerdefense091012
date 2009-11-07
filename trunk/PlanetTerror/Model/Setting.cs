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
		SelectedTower = TowerMax,
		Projectile = 101
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Setting
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class Setting
	{
		//===============================================================================================================================================
		//	필드
		public const int VERSION = 15;

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
			public class Stat
			{
				public double attackRange;
				public double attackRangeSqr;
				public double attackCooldown;
				public double attackDamage;
				public double projSpeed;
				public double turretRotSpeed;
				public int upgCost;
			}
			public List<Stat> stats;
		}
		public Tower tower;

		public class Core
		{
			public double hitPoint;
			public double warningBefore;
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

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본값 설정
		void SetDefault()
		{
			version = VERSION;
			startGold = 1000;
			powerMax = 100;

			gold.interval = 15;
			gold.mineGold = 5;
			gold.labGold = -5;
			gold.labPower = 4;

			tower.towerCost = 50;
			tower.labCost = 200;
			tower.dismantleCost = -30;

			tower.stats = new List<Tower.Stat>();
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());
			tower.stats.Add(new Tower.Stat());

			tower.stats[0].attackRange = 110;
			tower.stats[0].attackDamage = 10;
			tower.stats[0].attackCooldown = 1;
			tower.stats[0].projSpeed = 100;
			tower.stats[0].turretRotSpeed = 150;
			tower.stats[0].upgCost = 150;

			tower.stats[1].attackRange = 130;
			tower.stats[1].attackDamage = 15;
			tower.stats[1].attackCooldown = 0.9;
			tower.stats[1].projSpeed = 110;
			tower.stats[1].turretRotSpeed = 170;
			tower.stats[1].upgCost = 200;

			tower.stats[2].attackRange = 150;
			tower.stats[2].attackDamage = 20;
			tower.stats[2].attackCooldown = 0.8;
			tower.stats[2].projSpeed = 120;
			tower.stats[2].turretRotSpeed = 190;
			tower.stats[2].upgCost = 250;

			tower.stats[3].attackRange = 170;
			tower.stats[3].attackDamage = 25;
			tower.stats[3].attackCooldown = 0.7;
			tower.stats[3].projSpeed = 130;
			tower.stats[3].turretRotSpeed = 210;
			tower.stats[3].upgCost = 1000;

			core.hitPoint = 100;
			core.warningBefore = 5;

			enemy1.routeTime = 35;
			enemy1.hitPoint = 20;
			enemy1.damage = 5;
			enemy1.gold = 0;
			enemy1.powerUp = 0.5;

			enemy2.routeTime = 60;
			enemy2.hitPoint = 100;
			enemy2.damage = 7.5;
			enemy2.gold = 0;
			enemy2.powerUp = 1;

			enemy3.routeTime = 20;
			enemy3.hitPoint = 50;
			enemy2.damage = 10;
			enemy3.gold = 0;
			enemy3.powerUp = 2;

			var w = MakeWave(34);
			MakeBundle(w, 0.1, "Enemy1", 2, 5, 0.5);

			w = MakeWave(5);
			MakeBundle(w, 0.1, "Enemy1", 2, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy2", 0, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy3", 2, 5, 1.5);

			w = MakeWave(5);
			MakeBundle(w, 0.1, "Enemy1", 0, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy2", 1, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy3", 2, 5, 1.5);

			w = MakeWave(5);
			MakeBundle(w, 0.1, "Enemy1", 2, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy2", 2, 5, 1.5);
			MakeBundle(w, 0.1, "Enemy3", 2, 5, 1.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	웨이브 생성
		Wave MakeWave(double waitTime)
		{
			var w = new Wave();
			w.waitTime = waitTime;
			w.bundles = new List<Wave.Bundle>();
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