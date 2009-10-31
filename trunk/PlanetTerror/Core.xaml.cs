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
		const string HP100_STATE = "Core_HP100_State";
		const string HP60_STATE = "Core_HP60_State";
		const string HP30_STATE = "Core_HP30_State";
		const string HP10_STATE = "Core_HP10_State";
		const string HP0_STATE = "Core_HP0_State";

		//===============================================================================================================================================
		//	프로퍼티
		public double HitPoint { get; protected set; }
		
		//===============================================================================================================================================
		//	필드
		VSM vsm;		

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Core()
		{
			this.InitializeComponent();
			
			vsm = new VSM(this, LayoutRoot);

			Loaded += new RoutedEventHandler(Core_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Core_Loaded(object sender, RoutedEventArgs e)
		{
			WPFUtil.SetImageScaleMode(LayoutRoot, BitmapScalingMode.Linear);

			var story = Resources.FindStoryboard("Idle_Storyboard");
			story.RepeatBehavior = RepeatBehavior.Forever;
			story.Begin();

			vsm.SetState(HP100_STATE);
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

			switch( vsm.GetState() )
			{
			case HP100_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.6 )
				{
					vsm.SetState(HP60_STATE);
				}
				break;
			case HP60_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.3 )
				{
					vsm.SetState(HP30_STATE);
				}
				break;
			case HP30_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= Game.Setting.core.hitPoint * 0.1 )
				{
					vsm.SetState(HP10_STATE);
				}
				break;
			case HP10_STATE:
				if( vsm.GetStateFinished() &&
					HitPoint <= 0 )
				{
					vsm.SetState(HP0_STATE);
				}
				break;
			case HP0_STATE:
				break;
			}
		}
	}
}