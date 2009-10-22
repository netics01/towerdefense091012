using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows;

namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	VectorExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class VectorExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Point 로 변환
		public static Point ToPoint(this Vector v)
		{
			return new Point(v.X, v.Y);
		}
	}
}