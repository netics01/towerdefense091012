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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	싱글턴 액세서
		public static WorldControl Instance { get; protected set; }

		//===============================================================================================================================================
		//	프로퍼티
		public PathGeometry Route1 { get; protected set; }


		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		float acc;
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		List<Enemy1> enemy1s;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WorldControl()
		{
			this.InitializeComponent();

			Instance = this;
			Route1 = route1_Path.Data.GetFlattenedPathGeometry();

			enemy1s = new List<Enemy1>();

			route1_Path.SetVisible(false);			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			for( int i = 0; i < enemy1s.Count; ++i )
			{
				enemy1s[i].Update(delta);
			}

			for( int i = 0; i < enemy1s.Count; ++i )
			{
				if( enemy1s[i].IsDeleted )
				{
					LayoutRoot.Children.Remove(enemy1s[i]);
					enemy1s.RemoveAt(i);					
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 생성
		public void CreateEnemy1(PathGeometry path)
		{
			var enemy = new Enemy1(path);
			enemy1s.Add(enemy);
			LayoutRoot.Children.Add(enemy);
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	적 제거
// 		void DeleteEnemy(Enemy1 enemy)
// 		{
// 			LayoutRoot.Children.Remove(enemy);
// 		}
	}
}