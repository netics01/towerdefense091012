using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	WPFUtil
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class WPFUtil
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	캔버스 밑의 모든 Image 의 스케일 변경
		public static void SetHighPerformanceImageMode(Canvas canvas)
		{
			for( int i = 0; i < canvas.Children.Count; ++i )
			{
				var child = canvas.Children[i];
				var image = child as Image;
				if( image != null )
				{
					RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.Linear);
				}
				var childCanvas = child as Canvas;
				if( childCanvas != null )
				{
					SetHighPerformanceImageMode(childCanvas);
				}
			}
		}
	}
}