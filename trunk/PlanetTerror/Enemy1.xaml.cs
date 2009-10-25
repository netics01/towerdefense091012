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
	//	Enemy1
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Enemy1 : UserControl
	{
		//===============================================================================================================================================
		//	프로퍼티
		public bool IsDeleted { get; protected set; }

		//===============================================================================================================================================
		//	필드
		PathGeometry path;
		double acc;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Enemy1(PathGeometry path)
		{
			this.InitializeComponent();

			this.path = path;

			Loaded += new RoutedEventHandler(Enemy1_Loaded);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void Enemy1_Loaded(object sender, RoutedEventArgs e)
		{
			var story = Resources["Move_Storyboard"] as Storyboard;
			story.Begin();
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
 			acc += delta / SettingXml.Instance.enemy1RouteTime;
			if( acc >= 1.0 )
			{
				IsDeleted = true;
				return;
			}

			Point pos, tangent;
			path.GetPointAtFractionLength(acc, out pos, out tangent);

			this.SetCenter(pos);
		}
	}
}