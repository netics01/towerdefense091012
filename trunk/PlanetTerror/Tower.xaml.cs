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
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Tower
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Tower : UserControl
	{
		//===============================================================================================================================================
		//	상수
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string BUILD_GROUP = "Build_StateGroup";
		const string NOTYETBUILT_STATE = "Build_NotYetBuilt_State";
		const string TOWER_BUILT_STATE = "Build_Tower_Built_State";
		const string TOWER_DISMANTLE_STATE = "Build_Tower_Dismantle_State";
		const string LAB_BUILT_STATE = "Build_Lab_Built_State";
		const string LAB_DISMANTLE_STATE = "Build_Lab_Dismantle_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string MENU_GROUP = "Menu_StateGroup";
		const string MENU_NOMENU_STATE = "Menu_NoMenu_State";
		const string MENU_BUILD_STATE = "Menu_Build_State";
		const string MENU_UPGRADE1_STATE = "Menu_Upgrade_State";
		const string MENU_UPGRADE2_STATE = "Menu_Upgrade2_State";
		const string MENU_UPGRADE3_STATE = "Menu_Upgrade3_State";
		const string MENU_DISMANTLE_STATE = "Menu_DismantleOnly_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string GOLD_GROUP = "Gold_StateGroup";
		const string GOLD_GAIN_STATE = "Gold_Gain_State";
		const string GOLD_LOSE_STATE = "Gold_Lose_State";
		const string GOLD_NORMAL_STATE = "Gold_Normal_State";

		//===============================================================================================================================================
		//	프로퍼티
		public Setting.Tower.Stat Stat { get { return Game.Setting.tower.stats[towerLevel]; } }

		//===============================================================================================================================================
		//	스태틱
		static Tower s_selectedTower;
		static int s_selectedTowerZ;

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const int MAX_UPGRADE = 3;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Enemy target;
		Point towerCenter;
		double cooldownTime;
		int towerLevel;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		VSM vsm;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		List<Storyboard> effectStories;
		Storyboard notYetBuiltStory;
		Storyboard labStory;
		Storyboard upgEffectStory;
		bool bDismantling;
		Storyboard upg1AttachtStory;
		Storyboard upg2AttachtStory;
		Storyboard upg3AttachtStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Tower()
		{
			this.InitializeComponent();

			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup(BUILD_GROUP);

			effectStories = new List<Storyboard>();
			effectStories.Add(Resources.FindStoryboard("Upg0_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg1_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg2_Tower_Storyboard"));
			effectStories.Add(Resources.FindStoryboard("Upg3_Tower_Storyboard"));
			for( int i = 0; i < effectStories.Count; ++i )
			{
				effectStories[i].RepeatForever();
			}

			notYetBuiltStory = Resources.FindStoryboard("NotYetBuilt_Storyboard");
			notYetBuiltStory.RepeatForever();
			labStory = Resources.FindStoryboard("Lab_Tower_Storyboard");
			labStory.RepeatForever();
			upgEffectStory = Resources.FindStoryboard("Upg_Effect_Storyboard");
			bDismantling = false;
			upg1AttachtStory = Resources.FindStoryboard("Upg1_Attach_Storyboard");
			upg1AttachtStory.Completed += new EventHandler(upg1AttachtStory_Completed);
			upg2AttachtStory = Resources.FindStoryboard("Upg2_Attach_Storyboard");
			upg2AttachtStory.Completed += new EventHandler(upg2AttachtStory_Completed);
			upg3AttachtStory = Resources.FindStoryboard("Upg3_Attach_Storyboard");
			upg3AttachtStory.Completed += new EventHandler(upg3AttachtStory_Completed);

			Loaded += new RoutedEventHandler(Tower_Loaded);
			MouseEnter += new MouseEventHandler(Tower_MouseEnter);
			MouseLeave += new MouseEventHandler(Tower_MouseLeave);
			Menu_Build_Button.Click += new RoutedEventHandler(Menu_Build_Button_Click);
			Menu_Upgrade_Button.Click += new RoutedEventHandler(Menu_Upgrade_Button_Click);
			Menu_Upgrade2_Button.Click += new RoutedEventHandler(Menu_Upgrade2_Button_Click);
			Menu_Upgrade3_Button.Click += new RoutedEventHandler(Menu_Upgrade3_Button_Click);
			Menu_Dismantle_Button.Click += new RoutedEventHandler(Menu_Dismantle_Button_Click);
			Menu_Lab_Button.Click += new RoutedEventHandler(Menu_Lab_Button_Click);
			
			Gold_Gain_State.Storyboard.Completed += new EventHandler(GoldGain_Storyboard_Completed);
			Gold_Lose_State.Storyboard.Completed += new EventHandler(GoldLose_Storyboard_Completed);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			towerCenter = this.GetLeftTop() + new Vector(Game.Setting.tower.centerX, Game.Setting.tower.centerY);
			vsm.SetState(NOTYETBUILT_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseEnter(object sender, MouseEventArgs e)
		{
			Select(this);
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				vsm.SetState(MENU_GROUP, MENU_BUILD_STATE);
				break;
			case TOWER_BUILT_STATE:
				if( vsm.GetStateFinished() )
				{
					switch( towerLevel )
					{
					case 0:
						vsm.SetState(MENU_GROUP, MENU_UPGRADE1_STATE);
						break;
					case 1:
						vsm.SetState(MENU_GROUP, MENU_UPGRADE2_STATE);
						break;
					case 2:
						vsm.SetState(MENU_GROUP, MENU_UPGRADE3_STATE);
						break;
					case 3:
						vsm.SetState(MENU_GROUP, MENU_DISMANTLE_STATE);
						break;
					}					
				}
				break;
			case LAB_BUILT_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(MENU_GROUP, MENU_DISMANTLE_STATE);
				}
				break;
			case TOWER_DISMANTLE_STATE:
				vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
				break;
			}			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Tower_MouseLeave(object sender, MouseEventArgs e)
		{
			Select(null);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Build_Button_Click(object sender, RoutedEventArgs e)
		{
			int gold = Game.Setting.tower.towerCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			towerLevel = 0;
			bDismantling = false;
			notYetBuiltStory.Stop();
			vsm.SetState(TOWER_BUILT_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Upgrade_Button_Click(object sender, RoutedEventArgs e)
		{
			if( !Game.UI.SpendGold(Stat.upgCost) )
			{
				return;
			}

			effectStories[towerLevel].Stop();
			towerLevel++;
			effectStories[towerLevel].Begin();
			upgEffectStory.Begin();
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);

			var attachStory = towerLevel == 1 ? upg1AttachtStory : (towerLevel == 2 ? upg2AttachtStory : upg3AttachtStory);
			attachStory.AutoReverse = false;
			attachStory.Begin();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Upgrade2_Button_Click(object sender, RoutedEventArgs e)
		{
			Menu_Upgrade_Button_Click(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Upgrade3_Button_Click(object sender, RoutedEventArgs e)
		{
			Menu_Upgrade_Button_Click(sender, e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Dismantle_Button_Click(object sender, RoutedEventArgs e)
		{
			int gold = Game.Setting.tower.dismantleCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			bDismantling = true;
			effectStories[0].Stop();
			labStory.Stop();

			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);

			if( towerLevel == 0 )
			{
				if( vsm.GetState() == TOWER_BUILT_STATE )
				{
					vsm.SetState(TOWER_DISMANTLE_STATE);
				}
				else { vsm.SetState(LAB_DISMANTLE_STATE); }
			}
			else
			{
				var attachStory = towerLevel == 1 ? upg1AttachtStory : (towerLevel == 2 ? upg2AttachtStory : upg3AttachtStory);
				attachStory.BeginReverse(this);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Menu_Lab_Button_Click(object sender, RoutedEventArgs e)
		{
			int gold = Game.Setting.tower.labCost;
			if( !Game.UI.SpendGold(gold) )
			{
				return;
			}
			bDismantling = false;
			notYetBuiltStory.Stop();
			vsm.SetState(LAB_BUILT_STATE);
			vsm.SetState(MENU_GROUP, MENU_NOMENU_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GoldGain_Storyboard_Completed(object sender, EventArgs e)
		{
			vsm.SetState(GOLD_GROUP, GOLD_NORMAL_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void GoldLose_Storyboard_Completed(object sender, EventArgs e)
		{
			vsm.SetState(GOLD_GROUP, GOLD_NORMAL_STATE);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void upg1AttachtStory_Completed(object sender, EventArgs e)
		{
			if( !bDismantling ) { return; }
			if( vsm.GetState() == TOWER_BUILT_STATE )
			{
				vsm.SetState(TOWER_DISMANTLE_STATE);
			}
			else { vsm.SetState(LAB_DISMANTLE_STATE); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void upg2AttachtStory_Completed(object sender, EventArgs e)
		{
			if( !bDismantling ) { return; }
			upg1AttachtStory.BeginReverse(this);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void upg3AttachtStory_Completed(object sender, EventArgs e)
		{
			if( !bDismantling ) { return; }
			upg2AttachtStory.BeginReverse(this);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				if( vsm.GetStateJustFinished() )
				{
					notYetBuiltStory.Begin();
				}
				break;
			case TOWER_BUILT_STATE:
				var stat = Game.Setting.tower.stats[towerLevel];
				if( !vsm.GetStateFinished() ) { return; }
				if( vsm.GetStateJustFinished() )
				{
					effectStories[0].Begin();
				}
				//타겟 설정
				if( target == null ||
					target.IsInvalid ||
					target.Pos.DistanceSqaure(towerCenter) >= stat.attackRangeSqr )
				{
					target = Game.World.FindTarget(towerCenter, stat.attackRangeSqr);
				}
				//주포 회전
				bool bAim = false;
				double targetAngle = 0;
				var firePos = towerCenter;
				if( target != null )
				{
					var dir = target.Pos - towerCenter;
					targetAngle = NumberH.ToDegree(dir.Angle());

					var transformGroup = Turret.RenderTransform as TransformGroup;
					var rotTransform = transformGroup.Children[2] as RotateTransform;
					var curAngle = rotTransform.Angle;
					var maxAngleChange = stat.turretRotSpeed * delta;
					if( Math.Abs(targetAngle - curAngle) < maxAngleChange )
					{
						transformGroup.Children[2] = new RotateTransform(targetAngle);
						dir.Normalize();
						firePos = towerCenter + (dir * Game.Setting.tower.barrelLength);
						bAim = true;
					}
					else
					{
						transformGroup.Children[2] = new RotateTransform(curAngle + maxAngleChange * Math.Sign(targetAngle - curAngle));
					}
				}
				//쿨타임 대기
				if( cooldownTime > 0 )
				{
					cooldownTime -= delta;
					break;
				}
				//타겟이 있으면 타겟 공격
				if( target != null &&
					bAim )
				{
					FireProjectile(targetAngle, firePos);
				}
				break;
			case TOWER_DISMANTLE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NOTYETBUILT_STATE);
					effectStories[0].Stop();
				}
				break;
			case LAB_BUILT_STATE:
				if( vsm.GetStateJustFinished() )
				{
					labStory.Begin();
				}
				//파워 증가
				Game.UI.GainPower(Game.Setting.gold.labPower* delta);
				break;
			case LAB_DISMANTLE_STATE:
				if( vsm.GetStateFinished() )
				{
					vsm.SetState(NOTYETBUILT_STATE);
				}
				break;
			}			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	골드타임
		public void MakeGold()
		{
			int gold = EstimateGold();
			if( gold > 0 )
			{
				vsm.SetState(GOLD_GROUP, GOLD_GAIN_STATE);
				goldGain_Text.Text = "+" + gold.ToString();
				Game.UI.GainGold(gold);
			}
			else if( gold < 0 )
			{
				vsm.SetState(GOLD_GROUP, GOLD_LOSE_STATE);
				goldLose_Text.Text = gold.ToString();
				Game.UI.GainGold(gold);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	예상 수입
		public int EstimateGold()
		{
			switch( vsm.GetState() )
			{
			case NOTYETBUILT_STATE:
				return Game.Setting.gold.mineGold;
			case LAB_BUILT_STATE:
				return Game.Setting.gold.labGold;
			}
			return 0;
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	활성 타워
		static void Select(Tower t)
		{
			if( s_selectedTower != null )
			{
				Canvas.SetZIndex(s_selectedTower, (int)s_selectedTowerZ);
			}
			s_selectedTower = t;
			if( s_selectedTower != null )
			{
				s_selectedTowerZ = s_selectedTower.GetZIndex();
				Canvas.SetZIndex(s_selectedTower, (int)ELayer.SelectedTower);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	발사체 생성
		void FireProjectile(double angle, Point firePos)
		{
			Projectile p;
			switch( towerLevel )
			{
			case 0:
				p = new Projectile0();
				break;
			case 1:
				p = new Projectile1();
				break;
			case 2:
				p = new Projectile2();
				break;
			default:
				p = new Projectile3();
				break;
			}
			p.Initialize(target, firePos, angle);
			p.Damage = Stat.attackDamage;
			p.Speed = Stat.projSpeed;

			Game.World.AddProjectile(p);

			cooldownTime = Stat.attackCooldown;
		}
	}
}