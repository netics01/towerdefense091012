using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	FrameworkElementExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class FrameworkElementExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	ActualWidth, ActualHeight 를 Size 형으로 얻는다.
		public static Point GetActualSize(this FrameworkElement e)
		{
			return new Point(e.ActualWidth, e.ActualHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기 설정
		public static void SetSize(this FrameworkElement e, Point size)
		{
			e.Width = size.X;
			e.Height = size.Y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void SetSize(this FrameworkElement e, double width, double height)
		{
			e.Width = width;
			e.Height = height;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	ActualWidth, ActualHeight 를 Rect 형으로 얻는다. X, Y 는 0.
		public static Rect GetActualSizeRect(this FrameworkElement e)
		{
			return new Rect(0, 0, e.ActualWidth, e.ActualHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Width, Height 를 Rect 형으로 얻는다. X, Y 는 0.
		public static Rect GetSizeRect(this FrameworkElement e)
		{
			return new Rect(0, 0, e.Width, e.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Canvas.Left, Canvas.Top 을 Point 형으로 얻는다.
		public static Point GetLeftTop(this FrameworkElement e)
		{
			return new Point(Canvas.GetLeft(e), Canvas.GetTop(e));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Canvas.Left, Canvas.Top 을 Point 형으로 설정한다.
		public static void SetLeftTop(this FrameworkElement e, Point p)
		{
			Canvas.SetLeft(e, p.X);
			Canvas.SetTop(e, p.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void SetLeftTop(this FrameworkElement e, double x, double y)
		{
			Canvas.SetLeft(e, x);
			Canvas.SetTop(e, y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Canvas.Left, Canvas.Top, ActualWidth, ActualHeight 를 Rect 형으로 얻는다.
		public static Rect GetActualBound(this FrameworkElement e)
		{
			return new Rect(Canvas.GetLeft(e), Canvas.GetTop(e), e.ActualWidth, e.ActualHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Canvas.Left, Canvas.Top, Width, Height 를 bound 값에 따라 설정한다.
		public static void SetActualBound(this FrameworkElement e, Rect bound)
		{
			Canvas.SetLeft(e, bound.X);
			Canvas.SetTop(e, bound.Y);
			e.Width = bound.Width;
			e.Height = bound.Height;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	자신의 크기 외부의 부분을 클리핑한다.
		public static void ClipOwnRect(this FrameworkElement e)
		{
			var rectangle = new RectangleGeometry();
			rectangle.Rect = e.GetActualSizeRect();
			e.Clip = rectangle;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	이름으로 VisualState 를 찾는다.
		public static VisualState FindState(this FrameworkElement e, string stateName)
		{
			var list = VisualStateManager.GetVisualStateGroups(e);
			foreach( var item in list )
			{
				VisualStateGroup group = item as VisualStateGroup;
				foreach( var item2 in group.States )
				{
					VisualState state = item2 as VisualState;
					if( state.Name == stateName ) { return state; }
				}
			}
			return null;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	그룹이름/상태이름으로 VisualState 를 찾는다.
		public static VisualState FindState(this FrameworkElement e, string groupName, string stateName)
		{
			var list = VisualStateManager.GetVisualStateGroups(e);
			foreach( var item in list )
			{
				VisualStateGroup group = item as VisualStateGroup;
				if( group.Name != groupName ) { continue; }

				foreach( var item2 in group.States )
				{
					VisualState state = item2 as VisualState;
					if( state.Name == stateName ) { return state; }
				}
			}
			return null;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	UIElementExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class UIElementExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Visibility 를 쉽게 설정한다.
		public static void SetVisible(this UIElement e, bool value)
		{
			//e.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
			var vis = value ? Visibility.Visible : Visibility.Collapsed;
			if( e.Visibility != vis ) { e.Visibility = vis; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool GetVisible(this UIElement e)
		{
			return e.Visibility == Visibility.Visible ? true : false;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Visibility 와 함께 Opacity 도 설정한다.
		public static void SetVisibleOpacity(this UIElement e, bool value)
		{
			var vis = value ? Visibility.Visible : Visibility.Collapsed;
			if( e.Visibility != vis ) { e.Visibility = vis; }
			e.Opacity = value ? 1 : 0;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	투명도를 점차 늘린다.
		public static void FadeIn(this UIElement e, float time)
		{
			Progress.AddToVar(v => { e.Opacity = NumberH.Clamp(0, e.Opacity + v, 1); e.SetVisible(e.Opacity > 0.01); }, time, 0, 1.05);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	투명도를 점차 줄인다.
		public static void FadeOut(this UIElement e, float time)
		{
			Progress.AddToVar(v => { e.Opacity = NumberH.Clamp(0, e.Opacity + v, 1); e.SetVisible(e.Opacity > 0.01); }, time, 1.05, 0);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Visibility 를 점차적으로 변화한다.
		public static void SetVisible(this UIElement e, bool value, float time)
		{
			if( value ) { e.FadeIn(time); }
			else { e.FadeOut(time); }
		}
	}
}