﻿using System;
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
	//	DependencyObjectExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class DependencyObjectExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디자이너 모드인지 확인
		public static bool IsInDesignMode(this DependencyObject obj)
		{
			return System.ComponentModel.DesignerProperties.GetIsInDesignMode(obj);
		}
	}

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
		//	중심위치를 얻는다.
		public static Point GetCenter(this FrameworkElement e)
		{
			return new Point(Canvas.GetLeft(e) + e.ActualWidth * 0.5, Canvas.GetTop(e) + e.ActualHeight * 0.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심위치를 통해 Canvas.Left, Canvas.Top 을 변경한다.
		public static void SetCenter(this FrameworkElement e, Point p)
		{
			e.SetLeftTop(p.X - e.ActualWidth * 0.5, p.Y - e.ActualHeight * 0.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void SetCenter(this FrameworkElement e, double x, double y)
		{
			e.SetLeftTop(x - e.ActualWidth * 0.5, y - e.ActualHeight * 0.5);
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
		//	ZIndex 액세서
		public static int GetZIndex(this UIElement e)
		{
			return Canvas.GetZIndex(e);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void SetZIndex(this UIElement e, int z)
		{
			Canvas.SetZIndex(e, z);
		}
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

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ControlExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class ControlExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스테이트를 설정한다.
		public static void SetState(this Control c, string stateName, bool useTransition)
		{
			VisualStateManager.GoToState(c, stateName, useTransition);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	PanelExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class PanelExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	특정 타입의 객체를 모두 제거한다.
		public static void RemoveType<T>(this Panel panel) where T : class
		{
			for( int i = panel.Children.Count - 1; i >= 0; --i )
			{
				var t = panel.Children[i] as T;
				if( t != null )
				{
					panel.Children.RemoveAt(i);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ResourceDictionaryExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class ResourceDictionaryExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스토리 보드를 찾는다.
		public static Storyboard FindStoryboard(this ResourceDictionary resources, string name)
		{
			return resources[name] as Storyboard;
		}		
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	StoryboardExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class StoryboardExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	무한반복 상태로 만든다.
		public static void RepeatForever(this Storyboard story)
		{
			story.RepeatBehavior = RepeatBehavior.Forever;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	거꾸로 재생한다. 주의! AutoReverse 속성이 변경된다.
		public static void BeginReverse(this Storyboard story, FrameworkElement e)
		{
			story.AutoReverse = true;
			story.Begin(e, true);
			story.Seek(e, new TimeSpan(0), TimeSeekOrigin.Duration);
		}
	}
}