﻿using System;
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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스크린 중앙에 위치할 수 있는 Left, Top 위치를 계산한다.
		public static Point GetScreenCenterLeftTop(double width, double height)
		{
			var left = (System.Windows.SystemParameters.PrimaryScreenWidth - width) / 2;
			var top = (System.Windows.SystemParameters.PrimaryScreenHeight - height) / 2;
			return new Point(left, top);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	VSM
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class VSM
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//	Group
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public class Group
		{
			//===============================================================================================================================================
			//	프로퍼티
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			public string Name { get { return StateGroup.Name; } }
			public VisualStateGroup StateGroup { get; protected set; }
			public Dictionary<string, VisualState> States { get; protected set; }
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			public string StateName { get { return State != null ? State.Name : string.Empty; } }
			public VisualState State { get; protected set; }
			public bool InProgress { get; set; }
			public bool JustFinished { get; set; }

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Group(VisualStateGroup stateGroup)
			{
				StateGroup = stateGroup;

				States = new Dictionary<string, VisualState>();
				for( int i = 0; i <	StateGroup.States.Count; ++i )
				{
					var state = StateGroup.States[i] as VisualState;
					States.Add(state.Name, state);
					
					if( state.Storyboard != null )
					{
						state.Storyboard.Completed += new EventHandler(Storyboard_Completed);
					}
				}
			}

			//===============================================================================================================================================
			//	내부
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	상태를 설정한다.
			internal bool SetState(string stateName)
			{
				if( stateName.Length == 0 )
				{
					State = null;
					InProgress = false;
					return true;
				}

				VisualState state = States.Find(stateName);
				if( state == null ) { return false; }
				State = state;
				InProgress = (State.Storyboard != null);
				JustFinished = false;
				return true;
			}

			//===============================================================================================================================================
			//	전용
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	상태 변화가 완료되었는가?
			void Storyboard_Completed(object sender, EventArgs e)
			{
				InProgress = false;
				JustFinished = true;
			}
		}

		//===============================================================================================================================================
		//	프로퍼티
		public Dictionary<string, Group> Groups { get; protected set; }
		public Group DefaultGroup { get; protected set; }
		public Control Control { get; protected set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public VSM(Control c, FrameworkElement e)
		{
			Control = c;
			var list = VisualStateManager.GetVisualStateGroups(e);

			Groups = new Dictionary<string, Group>();
			for( int i = 0; i < list.Count; ++i )
			{
				var g = list[i] as VisualStateGroup;
				Debug.Assert( g != null );

				var group = new Group(g);
				Groups.Add(g.Name, group);
				if( DefaultGroup == null ) { DefaultGroup = group; }
			}
			Debug.Assert( DefaultGroup != null );
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본상태 그룹을 설정한다.
		public bool SetDefaultGroup(string groupName)
		{
			Group group = Groups.Find(groupName);
			if( group == null ) { Debug.Assert(false); return false; }
			DefaultGroup = group;
			return true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본 스테이트그룹에서 스테이트를 설정한다.
		public bool SetState(string stateName, bool useTransition)
		{
			return SetState(DefaultGroup, stateName, useTransition);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool SetState(string stateName) { return SetState(stateName, true); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스테이트를 설정한다.
		public bool SetState(string groupName, string stateName, bool useTransition)
		{
			Group group = Groups.Find(groupName);
			if( group == null ) { Debug.Assert(false); return false; }
			return SetState(group, stateName, useTransition);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool SetState(string groupName, string stateName) { return SetState(groupName, stateName, true); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	기본 그룹에서 완됴된 상태로 스테이트를 설정한다.
		public bool SetStateFinished(string stateName)
		{
			return SetStateFinished(DefaultGroup, stateName);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool SetStateFinished(string groupName, string stateName)
		{
			Group group = Groups.Find(groupName);
			if( group == null ) { Debug.Assert(false); return false; }
			return SetStateFinished(group, stateName);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	VisualState 획득
		public VisualState GetVisualState(string stateName)
		{
			foreach( var group in Groups )
			{
				foreach( var state in group.Value.States )
				{
					if( state.Value.Name == stateName ) { return state.Value; }
				}
			}
			return null;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디폴트 그룹의 상태
		public string GetState()
		{
			return DefaultGroup.StateName;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public string GetState(string groupName)
		{
			var group = Groups.Find(groupName);
			Debug.Assert(group != null);
			return group.StateName;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	진행상태를 확인한다.
		public bool GetStateFinished()
		{
			return !DefaultGroup.InProgress;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool GetStateFinished(string groupName)
		{
			var group = Groups.Find(groupName);
			Debug.Assert( group != null );
			return group != null ? group.InProgress : false;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	진행이 방금 끝났는가?
		public bool GetStateJustFinished()
		{
			bool ret = DefaultGroup.JustFinished;
			DefaultGroup.JustFinished = false;
			return ret;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool GetStateJustFinished(string groupName)
		{
			var group = Groups.Find(groupName);
			Debug.Assert(group != null);
			bool ret = group.JustFinished;
			group.JustFinished = false;
			return ret;
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스테이트 설정
		bool SetState(Group group, string stateName, bool useTransition)
		{
			if( !group.SetState(stateName) ) { Debug.Assert(false); return false; }
			if( stateName.Length > 0 )
			{
				VisualStateManager.GoToState(Control, stateName, useTransition);
			}
			return true;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스테이트 완료된 상태로 설정
		bool SetStateFinished(Group group, string stateName)
		{
			if( !group.SetState(stateName) ) { Debug.Assert(false); return false; }
			if( stateName.Length > 0 )
			{
				VisualStateManager.GoToState(Control, stateName, false);
				if( group.InProgress )
				{
					group.State.Storyboard.SkipToFill();
					group.InProgress = false;
				}
			}
			return true;
		}
	}
}