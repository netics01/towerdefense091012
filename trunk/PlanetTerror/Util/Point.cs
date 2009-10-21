﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Point
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public struct Point
	{
		//===============================================================================================================================================
		//	프로퍼티
		public float X { get { return x; } set { x = value; } }
		public float Y { get { return y; } set { y = value; } }

		//===============================================================================================================================================
		//	필드
		public static Point Zero;

		public float x;
		public float y;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public Point(Point pos)
		{
			x = pos.x;
			y = pos.y;
		}

		//===============================================================================================================================================
		//	메소드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	클램프
		public void Clamp(Point min, Point max)
		{
			X = NumberH.Clamp(min.X, X, max.X);
			Y = NumberH.Clamp(min.Y, Y, max.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Clamp(RectF bound)
		{
			X = NumberH.Clamp(bound.left, X, bound.right);
			Y = NumberH.Clamp(bound.top, Y, bound.bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	비교
		public bool IsSame(Point rhs)
		{
			return x == rhs.x && y == rhs.y;
		}

		//===============================================================================================================================================
		//	연산자
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator+(Point lhs, Point rhs)
		{
			return new Point(lhs.x + rhs.x, lhs.y + rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator-(Point lhs, Point rhs)
		{
			return new Point(lhs.x - rhs.x, lhs.y - rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator*(Point lhs, Point rhs)
		{
			return new Point(lhs.x * rhs.x, lhs.y * rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator/(Point lhs, Point rhs)
		{
			return new Point(lhs.x / rhs.x, lhs.y / rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator*(Point lhs, float scalar)
		{
			return new Point(lhs.x * scalar, lhs.y * scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator*(float scalar, Point lhs)
		{
			return new Point(lhs.x * scalar, lhs.y * scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point operator/(Point lhs, float scalar)
		{
			return new Point(lhs.x / scalar, lhs.y / scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator==(Point lhs, Point rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator!=(Point lhs, Point rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public override bool Equals(object rhs) { return this == (Point)rhs; }
		public bool Equals(Point rhs) { return this == rhs; }
		public override int GetHashCode() {	return base.GetHashCode();}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	PointI
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	정수형 Point
	public struct PointI
	{
		//===============================================================================================================================================
		//	프로퍼티
		public int X { get { return x; } set { x = value; } }
		public int Y { get { return y; } set { y = value; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextLeft
		{
			get { return new PointI(x-1, y); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextTop
		{
			get { return new PointI(x, y-1); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextRight
		{
			get { return new PointI(x+1, y); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextBottom
		{
			get { return new PointI(x, y+1); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextLT
		{
			get { return new PointI(x-1, y-1); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextRT
		{
			get { return new PointI(x+1, y-1); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextLB
		{
			get { return new PointI(x-1, y+1); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI NextRB
		{
			get { return new PointI(x+1, y+1); }
		}

		//===============================================================================================================================================
		//	필드
		public static PointI Zero;

		public int x;
		public int y;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public PointI(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI(PointI pos)
		{
			x = pos.x;
			y = pos.y;
		}

		//===============================================================================================================================================
		//	메소드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	클램프
		public void Clamp(PointI min, PointI max)
		{
			X = NumberH.Clamp(min.X, X, max.X);
			Y = NumberH.Clamp(min.Y, Y, max.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Clamp(RectI bound)
		{
			X = NumberH.Clamp(bound.left, X, bound.right);
			Y = NumberH.Clamp(bound.top, Y, bound.bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	비교
		public bool IsSame(PointI rhs)
		{
			return x == rhs.x && y == rhs.y;
		}

		//===============================================================================================================================================
		//	연산자
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator+(PointI lhs, PointI rhs)
		{
			return new PointI(lhs.x + rhs.x, lhs.y + rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator-(PointI lhs, PointI rhs)
		{
			return new PointI(lhs.x - rhs.x, lhs.y - rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator*(PointI lhs, PointI rhs)
		{
			return new PointI(lhs.x * rhs.x, lhs.y * rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator/(PointI lhs, PointI rhs)
		{
			return new PointI(lhs.x / rhs.x, lhs.y / rhs.y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator*(PointI lhs, int scalar)
		{
			return new PointI(lhs.x * scalar, lhs.y * scalar);
		}
		public static PointI operator*(int scalar, PointI lhs)
		{
			return new PointI(lhs.x * scalar, lhs.y * scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static PointI operator/(PointI lhs, int scalar)
		{
			return new PointI(lhs.x / scalar, lhs.y / scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator==(PointI lhs, PointI rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator!=(PointI lhs, PointI rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public override bool Equals(object rhs) { return this == (PointI)rhs; }
		public bool Equals(PointI rhs) { return this == rhs; }
		public override int GetHashCode() { return base.GetHashCode(); }
	}
}