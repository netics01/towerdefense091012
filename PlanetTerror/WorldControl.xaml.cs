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
		//	필드
		PathGeometry route1;
		float acc;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public WorldControl()
		{
			this.InitializeComponent();

			Instance = this;
			route1 = route1_Path.Data.GetFlattenedPathGeometry();

			route1_Path.SetVisible(false);			
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			acc += delta;
			var r = acc * 0.05;

			Point pos, tangent;
			route1.GetPointAtFractionLength(r, out pos, out tangent);

			enemy1.SetLeftTop(pos.X, pos.Y);			
		}
	}
}