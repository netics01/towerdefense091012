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
using System.Windows.Media.Animation;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	UIPanelControl
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class UIPanelControl : UserControl
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static UIPanelControl Instance { get; protected set; }

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		int gold;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		Storyboard noMoneyStory;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public UIPanelControl()
		{
			this.InitializeComponent();

			Instance = this;

			noMoneyStory = Resources.FindStoryboard("NoMoney_Storyboard");
			
			Loaded += new RoutedEventHandler(UIPanelControl_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void UIPanelControl_Loaded(object sender, RoutedEventArgs e)
		{
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public void Initialize()
		{
			gold = Game.Setting.startGold;
			power_Progress.Maximum = Game.Setting.powerMax;
			power_Progress.Minimum = 0;
			power_Progress.Value = 0;
			DisplayGold();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	돈 소모
		public bool SpendGold(int requiredGold)
		{
			if( requiredGold > gold )
			{
				noMoneyStory.Begin();
				return false;
			}

			gold -= requiredGold;
			DisplayGold();
			return true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	돈 추가
		public void GainGold(int gainedGold)
		{
			gold += gainedGold;
			DisplayGold();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	게이지 변화
		public void GainPower(double power)
		{
			power_Progress.Value = power_Progress.Value + power;
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	골드 표시
		void DisplayGold()
		{
			gold_Text.Text = gold.ToString();
		}
	}
}