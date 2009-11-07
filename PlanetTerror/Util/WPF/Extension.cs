using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows;

namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Size
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class SizeExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Point 타입으로 변환
		public static Point ToPoint(this Size s)
		{
			return new Point(s.Width, s.Height);
		}
	}

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
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	각도 획득
		public static double Angle(this Vector v)
		{
			return Math.Atan2(v.Y, v.X);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	PointExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class PointExtMethod
	{
		public static Point Zero = new Point(0, 0);

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	클램핑
		public static Point Clamp(this Point value, Point min, Point max)
		{
			return new Point(NumberH.Clamp(min.X, value.X, max.X), NumberH.Clamp(min.Y, value.Y, max.Y));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Clamp(this Point value, Size max)
		{
			return new Point(Math.Min(value.X, max.Width), Math.Min(value.Y, max.Height));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	거리를 구한다.
		public static double Distance(this Point p, Point pt)
		{
			var dx = pt.X - p.X;
			var dy = pt.Y - p.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	거리의 제곱을 구한다.
		public static double DistanceSqaure(this Point p, Point pt)
		{
			var dx = pt.X - p.X;
			var dy = pt.Y - p.Y;
			return dx * dx + dy * dy;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	길이를 구한다.
		public static double Length(this Point p)
		{
			return Math.Sqrt(p.X * p.X + p.Y * p.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	길이의 제곱을 구한다.
		public static double LengthSquare(this Point p)
		{
			return p.X * p.X + p.Y * p.Y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	정규화
		public static Point Normalize(this Point p)
		{
			double invLength = 1 / Length(p);
			return new Point(p.X * invLength, p.Y * invLength);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 안에 들어가면서 s 와 비율이 같은 최대의 크기를 얻는다.
		public static Point FitSmaller(this Point p, Point bound)
		{
			double ratio = p.X / p.Y;
			double boundRatio = bound.X / bound.Y;
			if( ratio > 1.0 )
			{
				return ratio > boundRatio ?
					new Point(bound.X, p.Y / p.X * bound.X) :
					new Point(p.X / p.Y * bound.Y, bound.Y);
			}
			else
			{
				return ratio < boundRatio ?
					new Point(bound.X, p.Y / p.X * bound.X) :
					new Point(p.X / p.Y * bound.Y, bound.Y);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Rect 기준 비례 변환. from 기준의 p 를 to 기준으로 변환한다.
		public static Point Proportion(this Point p, Rect from, Rect to)
		{
			return new Point(to.X + (p.X-from.X) * to.Width / from.Width, to.Y + (p.Y-from.Y) * to.Height / from.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Size 타입으로 변환
		public static Size ToSize(this Point p)
		{
			return new Size(p.X, p.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	곱셈
		public static Point Multiply(this Point p, double scalar)
		{
			return new Point(p.X * scalar, p.Y * scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Multiply(this Point p, Point lhs)
		{
			return new Point(p.X * lhs.X, p.Y * lhs.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Multiply(this Point p, Size size)
		{
			return new Point(p.X * size.Width, p.Y * size.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	나눗셈
		public static Point Divide(this Point p, double scalar)
		{
			return new Point(p.X / scalar, p.Y / scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Divide(this Point p, Point lhs)
		{
			return new Point(p.X / lhs.X, p.Y / lhs.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Divide(this Point p, Size size)
		{
			return new Point(p.X / size.Width, p.Y / size.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Add(this Point lhs, Point rhs)
		{
			return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Minus(this Point lhs, Point rhs)
		{
			return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Point Negate(this Point lhs)
		{
			return new Point(-lhs.X, -lhs.Y);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	RectExtMedhod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class RectExtMedhod
	{
		//===============================================================================================================================================
		//	공용 필드
		public static Rect InfiniteRect = new Rect(double.MinValue, double.MinValue, double.PositiveInfinity, double.PositiveInfinity);
		public static Rect UnitRect = new Rect(0, 0, 1, 1);

		//===============================================================================================================================================
		//	프로퍼티(로 짜면 좋았을 것들...)
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	X, Y 를 Point 형으로 얻는다.
		public static Point GetLeftTop(this Rect r)
		{
			return new Point(r.X, r.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	오른쪽 바닥위치를 Point 형으로 얻는다.
		public static Point GetRightBottom(this Rect r)
		{
			return new Point(r.Right, r.Bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Width, Height 를 Point 형으로 얻는다
		public static Point GetSize(this Rect r)
		{
			return new Point(r.Width, r.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 얻는다.
		public static Point GetCenter(this Rect r)
		{
			return new Point(r.X + r.Width * 0.5, r.Y + r.Height * 0.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 설정한다. 넓이/높이는 유지된다.
		public static Rect SetCenter(this Rect r, Point center)
		{
			return FromCenterSize(center.X, center.Y, r.Width, r.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 설정한다. 넓이/높이는 유지된다.
		public static Rect SetCenter(this Rect r, double x, double y)
		{
			return FromCenterSize(x, y, r.Width, r.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	넓이를 얻는다.
		public static double GetArea(this Rect r)
		{
			return r.Width * r.Height;
		}

		//===============================================================================================================================================
		//	생성
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기에서 Rect 생성, 즉 위치는 0, 0
		public static Rect FromSize(Point size)
		{
			return new Rect(0, 0, size.X, size.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점과 크기로부터 Rect 생성
		public static Rect FromCenterSize(Point center, Size size)
		{
			return FromCenterSize(center.X, center.Y, size.Width, size.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect FromCenterSize(Point center, Point size)
		{
			return FromCenterSize(center.X, center.Y, size.X, size.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect FromCenterSize(double centerX, double centerY, double width, double height)
		{
			return new Rect(centerX - width * 0.5, centerY - height *0.5, width, height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	왼쪽, 위, 오른족, 아래 좌표에서 Rect 생성
		public static Rect FromBoundary(double left, double top, double right, double bottom)
		{
			return new Rect(left, top, right - left, bottom - top);
		}
		//===============================================================================================================================================
		//	변형
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	p 만큼 이동한다.
		public static Rect Move(this Rect r, Point p)
		{
			return new Rect(r.X + p.X, r.Y + p.Y, r.Width, r.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Width/Height 를 s 배 한다.
		//public static Rect MultiplySize(this Rect r, double s)
		//{
		//    return new Rect(r.X, r.Y, r.Width * s, r.Height * s);
		//}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 기준으로 크기를 설정한다.
		public static Rect SetSizeOnCenter(this Rect r, Point size)
		{
			return FromCenterSize(r.GetCenter(), size);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect SetSizeOnCenter(this Rect r, double width, double height)
		{
			return FromCenterSize(r.GetCenter(), new Point(width, height));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	각 방향으로 확장한다.
		public static Rect Expand(this Rect r, double left, double top, double right, double bottom)
		{
			return new Rect(
				r.X - left,
				r.Y - top,
				r.Width + left + right,
				r.Height + top + bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect Expand(this Rect r, double offset)
		{
			return r.Expand(offset, offset, offset, offset);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect Expand(this Rect r, double leftRight, double topBottom)
		{
			return r.Expand(leftRight, topBottom, leftRight, topBottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	선형보간
		public static Rect Lerp(Rect a, Rect b, double t)
		{
			double it = 1.0 - t;
			return new Rect(
				a.X * it + b.X * t,
				a.Y * it + b.Y * t,
				a.Width * it + b.Width * t,
				a.Height * it + b.Height * t);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 집어넣는다. 불가능하면 예외가 발생한다.
		public static Rect Bound(this Rect r, Rect bound)
		{
// 			if( r.Width > bound.Width ||
// 				r.Height > bound.Height ) { throw new Exception("Bound Rect is too small"); }
			//부동소수점 오차때문에 여유를 두어야 한다.
			const double MARGIN = 0.01;
			if( r.Width > bound.Width + MARGIN ||
				r.Height > bound.Height + MARGIN ) { throw new Exception("Bound Rect is too small"); }

			r.X = Math.Max(r.X, bound.X);
			r.Y = Math.Max(r.Y, bound.Y);
			if( r.Right > bound.Right ) { r.X -= r.Right - bound.Right; }
			if( r.Bottom > bound.Bottom ) { r.Y -= r.Bottom - bound.Bottom; }
			return r;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 제한한다. bound 보다 크다면 크기를 변형한다.
		public static Rect Restrain(this Rect r, Rect bound)
		{
			r.Width = Math.Min(r.Width, bound.Width);
			r.Height = Math.Min(r.Height, bound.Height);
			r.X = Math.Max(r.X, bound.X);
			r.Y = Math.Max(r.Y, bound.Y);
			if( r.Right > bound.Right ) { r.X -= r.Right - bound.Right; }
			if( r.Bottom > bound.Bottom ) { r.Y -= r.Bottom - bound.Bottom; }
			return r;
		}
		//===============================================================================================================================================
		//	상대좌표 계산
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	절대 좌표 p 를 상대좌표로 변환한다.
		public static Point GetNormalizedPoint(this Rect r, Point absPoint)
		{
			return new Point((absPoint.X - r.X) / r.Width, (absPoint.Y - r.Y) / r.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	상대좌표 relPoint 를 절대좌표로 변환한다.
		public static Point GetDenormalizedPoint(this Rect r, Point relPoint)
		{
			return new Point(r.X + r.Width * relPoint.X, r.Y + r.Height * relPoint.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	baseRect 를 기준으로 상대좌표로 변환한다.
		public static Rect Normalize(this Rect absRect, Rect baseRect)
		{
			return new Rect(
				(absRect.X - baseRect.X) / baseRect.Width,
				(absRect.Y - baseRect.Y) / baseRect.Height,
				absRect.Width / baseRect.Width,
				absRect.Height / baseRect.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	baseRect 를 기준으로 절대좌표로 변환한다. (Denormalize 보다 더 좋은 이름?)
		public static Rect Denormalize(this Rect relRect, Rect baseRect)
		{
			return new Rect(
				relRect.X * baseRect.Width + baseRect.X,
				relRect.Y * baseRect.Height + baseRect.Y,
				relRect.Width * baseRect.Width,
				relRect.Height * baseRect.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	절대좌표 absRect 가 상대좌표 relRect 로 표현될때의 기준계를 계산한다.
		public static Rect CalculateBaseRect(this Rect absRect, Rect relRect)
		{
			double baseWidth = absRect.Width / relRect.Width;
			double baseHeight = absRect.Height / relRect.Height;
			return new Rect(
				absRect.X - relRect.X * baseWidth,
				absRect.Y - relRect.Y * baseHeight,
				baseWidth, baseHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Rect 기준 비례 변환. from 기준의 r 를 to 기준으로 변환한다.
		public static Rect Proportion(this Rect r, Rect from, Rect to)
		{
			return new Rect(
				to.X + (r.X-from.X) * to.Width / from.Width,
				to.Y + (r.Y-from.Y) * to.Height / from.Height,
				r.Width * to.Width / from.Width,
				r.Height * to.Height / from.Height);
		}
		//===============================================================================================================================================
		//	연산자
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	곱셈
		public static Rect Multiply(this Rect r, double scalar)
		{
			return new Rect(r.X * scalar, r.Y * scalar, r.Width * scalar, r.Height * scalar);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static Rect Multiply(this Rect r, Point lhs)
		{
			return new Rect(r.X * lhs.X, r.Y * lhs.Y, r.Width * lhs.X, r.Height * lhs.Y);
		}
	}
}