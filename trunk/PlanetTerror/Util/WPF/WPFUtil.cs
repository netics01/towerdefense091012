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
		//	Panel 하위의 모든 Image 의 스케일링 방식을 Linear 로 변경
		public static void SetImageScaleMode(Panel panel, BitmapScalingMode mode)
		{
			for( int i = 0; i < panel.Children.Count; ++i )
			{
				var child = panel.Children[i];
				var image = child as Image;
				if( image != null )
				{
					RenderOptions.SetBitmapScalingMode(image, mode);
				}
				var childCanvas = child as Panel;
				if( childCanvas != null )
				{
					SetImageScaleMode(childCanvas, mode);
				}
			}
		}
	}
}