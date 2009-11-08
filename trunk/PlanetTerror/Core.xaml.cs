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
	//	Core
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Core : UserControl
	{
		//===============================================================================================================================================
		//	상수
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string CORE_GROUP = "Core_StateGroup";
		const string HP100_STATE = "Core_HP100_State";
		const string HP60_STATE = "Core_HP60_State";
		const string HP30_STATE = "Core_HP30_State";
		const string HP10_STATE = "Core_HP10_State";
		const string HP0_STATE = "Core_HP0_State";
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		const string ATTACK_GROUP = "Attack_StateGroup";
		const string ATTACK_NORMAL_STATE = "Attack_Normal_State";
		const string ATTACK_BEAM_STATE = "Attack_Beam_State";
		const string ATTACK_READY_STATE = "Attack_Ready_State";

		//===============================================================================================================================================
		//	프로퍼티
		public double HitPoint { get; protected set; }
		
		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		VSM vsm;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Storyboard ring100_Story;
		Storyboard ring60_Story;
		Storyboard ring30_Story;
		Storyboard ring10_Story;
		Storyboard ring0_Story;
		Storyboard hit_Story;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Core()
		{
			this.InitializeComponent();
			
			vsm = new VSM(this, LayoutRoot);
			vsm.SetDefaultGroup(CORE_GROUP);

			Loaded += new RoutedEventHandler(Core_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Core_Loaded(object sender, RoutedEventArgs e)
		{
			ring100_Story = Resources.FindStoryboard("Ring100_Storyboard");
			ring60_Story = Resources.FindStoryboard("Ring60_Storyboard");
			ring30_Story = Resources.FindStoryboard("Ring30_Storyboard");
			ring10_Story = Resources.FindStoryboard("Ring10_Storyboard");
			ring0_Story = Resources.FindStoryboard("Ring0_Storyboard");

			ring100_Story.RepeatForever();
			ring60_Story.RepeatForever();
			ring30_Story.RepeatForever();
			ring10_Story.RepeatForever();

			hit_Story = Resources.FindStoryboard("Hit_Storyboard");
			//hit_Story.FillBehavior = FillBehavior.Stop;

			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			vsm.SetState(HP100_STATE);
			ring100_Story.Begin();
			vsm.SetState(ATTACK_GROUP, ATTACK_NORMAL_STATE);

			//필살기 테스트를 위해
			Attack_Ready_Button.Click += new RoutedEventHandler(Attack_Ready_Button_Click);
			Attack_Beam_State.Storyboard.Completed += new EventHandler(Attack_Beam_State_Completed);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Attack_Beam_State_Completed(object sender, EventArgs e)
		{
			vsm.SetState(ATTACK_GROUP, ATTACK_NORMAL_STATE);
			Game.UI.GainPower(-1000);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Attack_Ready_Button_Click(object sender, RoutedEventArgs e)
		{
			vsm.SetState(ATTACK_GROUP, ATTACK_BEAM_STATE);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			HitPoint = Game.Setting.core.hitPoint;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	데미지를 입는다.
		public void TakeDamage(double damage)
		{
			HitPoint -= damage;

			if( vsm.GetState() != HP0_STATE )
			{
				hit_Story.Begin();
			}			

			switch( vsm.GetState() )
			{
			case HP100_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.6 )
				{
					vsm.SetState(HP60_STATE);
					ring100_Story.Stop();
					ring60_Story.Begin();
				}
				break;
			case HP60_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.3 )
				{
					vsm.SetState(HP30_STATE);
					ring60_Story.Stop();
					ring30_Story.Begin();
				}
				break;
			case HP30_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.1 )
				{
					vsm.SetState(HP10_STATE);
					ring30_Story.Stop();
					ring10_Story.Begin();
				}
				break;
			case HP10_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= 0 )
				{
					vsm.SetState(HP0_STATE);
					ring10_Story.Stop();
					ring0_Story.Begin();
				}
				break;
			case HP0_STATE:
				break;
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	게이지가 다 찼다.
		public void AttackReady()
		{
			vsm.SetState(ATTACK_GROUP, ATTACK_READY_STATE);
		}
	}
}