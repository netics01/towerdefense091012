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
	//	Projectile0
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class Projectile0 : InstantProjectile
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Projectile0()
		{
			this.InitializeComponent();
		}

		//===============================================================================================================================================
		//	핸들러

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		public override void Initialize(Enemy target, Point pos, double angle, bool bBegin)
		{
			Initialize(target, pos, angle, bBegin, LayoutRoot);
		}
// 		public override void Initialize(Enemy target, Point pos, double angle)
// 		{
// 			Initialize(target, pos, angle, LayoutRoot, Resources);
// 		}
	}
}