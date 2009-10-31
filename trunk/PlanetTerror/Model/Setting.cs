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



//----레이어----
//	Background			-2001
//	Enemy				[-1000, 0]
//	Roof				5
//	Projectile			6
//	Boss				10
//	Core				15




//----작업----


//자코들 클래스 풀링해야 할듯







namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Setting
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	POD
	public class Setting
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static Setting Instance { get; set; }

		//===============================================================================================================================================
		//	필드
		public const int VERSION = 3;

		public class Tower
		{
			public int buildCost;
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
		}
		public Enemy enemy1;
		public Enemy enemy2;
		public Enemy enemy3;

		public int version;
		public int startGold;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Setting()
		{
			tower = new Tower();
			core = new Core();
			proj1 = new Projectile();
			enemy1 = new Enemy();
			enemy2 = new Enemy();
			enemy3 = new Enemy();

			SetDefault();
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
				try { Helper.Serialize("setting.xml", setting); }
				catch( Exception ) {}
			}

			setting.Adjust();
			Instance = setting;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본값 설정
		public void SetDefault()
		{
			version = VERSION;
			startGold = 1000;

			tower.buildCost = 50;
			tower.attackRange = 100;
			tower.attackDamage = 10;
			tower.attackCooldown = 1;

			core.hitPoint = 100;

			proj1.speed = 20;

			enemy1.routeTime = 20;
			enemy1.hitPoint = 50;
			enemy1.damage = 5;

			enemy2.routeTime = 20;
			enemy2.hitPoint = 50;
			enemy2.damage = 5;

			enemy3.routeTime = 20;
			enemy3.hitPoint = 50;
			enemy2.damage = 5;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	값 변환
		public void Adjust()
		{
			tower.attackRangeSqr = tower.attackRange * tower.attackRange;
		}
	}
}