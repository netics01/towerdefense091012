using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	RectF
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public struct RectF
	{
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public float Left { get { return left; } set { left = value; } }
		public float Top { get { return top; } set { top = value; } }
		public float Right { get { return right; } set { right = value; } }
		public float Bottom { get { return bottom; } set { bottom = value; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public float Width
		{
			get { return right - left; }
			set { right = left + value; }
		}
		public float Height
		{
			get { return bottom - top; }
			set { bottom = top + value; }
		}
		public Point Size
		{
			get { return new Point(Width, Height); }
			set { Width = value.x; Height = value.y; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool Empty { get { return Width == 0 && Height == 0; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public Point LT
		{
			get { return new Point(left, top); }
			set { left = value.x; top = value.y; }
		}
		public Point RT
		{
			get { return new Point(right, top); }
			set { right = value.x; top = value.y; }
		}
		public Point LB
		{
			get { return new Point(left, bottom); }
			set { left = value.x; bottom = value.y; }
		}
		public Point RB
		{
			get { return new Point(right, bottom); }
			set { right = value.x; bottom = value.y; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public Point Center
		{
			get { return new Point((left + right) * 0.5f, (top + bottom) * 0.5f); }
			set { SetCenter(value); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public float Area
		{
			get { return Width * Height; }
		}

		//===============================================================================================================================================
		//	필드
		public static RectF ZeroRect = new RectF(0, 0, 0, 0);
		public static RectF UnitRect = new RectF(0, 0, 1, 1);

		public float left;
		public float top;
		public float right;
		public float bottom;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public RectF(Point lt, Point rb) { left = lt.x; top = lt.y; right = rb.x; bottom = rb.y; }
		public RectF(float left, float top, float right, float bottom) { this.left = left; this.top = top; this.right = right; this.bottom = bottom; }
		public RectF(RectF r) { left = r.left; top = r.top; right = r.right; bottom = r.bottom; }

		//===============================================================================================================================================
		//	생성
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기에서 Rect 생성. 위치는 (0, 0)
		public static RectF FromSize(float w, float y)
		{
			return new RectF(0, 0, w, y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectF FromSize(Point size) { return FromSize(size.x, size.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	왼쪽, 위, 오른족, 아래 좌표에서 Rect 생성
		public static RectF FromBoundary(float left, float top, float right, float bottom)
		{
			return new RectF(left, top, right, bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectF FromBoundary(Point lt, Point rb) { return new RectF(lt.x, lt.y, rb.x, rb.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Left/Top 좌표와 Width/Height 에서 Rect 생성
		public static RectF FromPosSize(float left, float top, float width, float height)
		{
			return new RectF(left, top, left + width, top + height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectF FromPosSize(Point pos, Point size) { return FromPosSize(pos.x, pos.y, size.x, size.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점과 크기로부터 Rect 생성
		public static RectF FromCenterSize(float centerX, float centerY, float width, float height)
		{
			float halfWidth = width * 0.5f;
			float halfHeight = height * 0.5f;
			return new RectF(centerX - halfWidth, centerY - halfHeight, centerX + halfWidth, centerY + halfHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectF FromCenterSize(Point center, Point size) { return FromCenterSize(center.x, center.y, size.x, size.y); }

		//===============================================================================================================================================
		//	위치/크기 변경
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	위치 직접 설정
		public void SetBoundary(float left, float top, float right, float bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetBoundary(Point lt, Point rb) { SetBoundary(lt.x, lt.y, rb.x, rb.y);}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	위치, 크기로 설정
		public void SetPosSize(float left, float top, float width, float height)
		{
			SetBoundary(left, top, left + width, right + height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetPosSize(Point lt, Point size) { SetBoundary(lt, lt + size); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기 변경
		public void SetSize(float w, float h)
		{
			Width = w;
			Height = h;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetSize(Point size) { Size = size; }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 설정한다.
		public void SetCenter(Point center)
		{
			var hw = Width * 0.5f;
			var hh = Height * 0.5f;
			SetBoundary(center.x - hw, center.y - hh, center.x + hw, center.y + hh);			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 기준으로 크기를 설정한다.
		public void SetSizeOnCenter(float width, float height)
		{
			var c = Center;
			SetSize(width, height);
			Center = c;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetSizeOnCenter(Point size) { SetSizeOnCenter(size.x, size.y); }

		//===============================================================================================================================================
		//	변형
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	이동한다.
		public void Move(float x, float y)
		{
			left += x;
			top += y;
			right += x;			
			bottom += y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Move(Point p) { Move(p.x, p.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF MoveRect(float x, float y)
		{
			return FromBoundary(left + x, top + y, right + x, bottom + y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF MoveRect(Point p) { return MoveRect(p.x, p.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	교차
		public void Intersect(RectF r)
		{
			left = Math.Max(left, r.left);
			top = Math.Max(top, r.top);
			right = Math.Min(right, r.right);
			bottom = Math.Min(bottom, r.bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF IntersectRect(RectF r)
		{
			return new RectF(Math.Max(left, r.left), Math.Max(top, r.top), Math.Min(right, r.right), Math.Min(bottom, r.bottom));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsIntersected(RectF r) { return !IntersectRect(r).Empty; }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포함
		public bool IsContained(RectF r)
		{
			return left <= r.left && top <= r.top && right >= r.right && bottom >= r.bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsContained(Point p)
		{
			return left <= p.x && p.x <= right && top <= p.y && p.y <= bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	병합
		public void Merge(RectF r)
		{
			SetBoundary(Math.Min(left, r.left), Math.Min(top, r.top), Math.Max(right, r.right), Math.Max(bottom, r.bottom));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF MergeRect(RectF r)
		{
			return FromBoundary(Math.Min(left, r.left), Math.Min(top, r.top), Math.Max(right, r.right), Math.Max(bottom, r.bottom));
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	각 방향으로 확장한다.
		public void Expand(float left, float top, float right, float bottom)
		{
			this.left -= left;
			this.top -= top;
			this.right += right;
			this.bottom += bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Expand(float leftRight, float topBottom) { Expand(leftRight, topBottom, leftRight, topBottom); }
		public void Expand(Point d) { Expand(d.x, d.y, d.x, d.y); }
		public void Expand(float d) { Expand(d, d, d, d); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF ExpandRect(float left, float top, float right, float bottom)
		{
			return FromBoundary(
				this.left - left,
				this.top - top,
				this.right + right,
				this.bottom + bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF ExpandRect(float leftRight, float topBottom) { return ExpandRect(leftRight, topBottom, leftRight, topBottom); }
		public RectF ExpandRect(Point d) { return ExpandRect(d.x, d.y, d.x, d.y); }
		public RectF ExpandRect(float d) { return ExpandRect(d, d, d, d); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	선형보간
		public static RectF Lerp(RectF a, RectF b, float t)
		{
			float it = 1.0f - t;
			return new RectF(
				a.Left * it + b.Left * t,
				a.Top * it + b.Top * t,
				a.Right * it + b.Right * t,
				a.Bottom * it + b.Bottom * t);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 집어넣는다. 불가능하면 예외가 발생한다.
		public void Bound(RectF bound) { this = BoundRect(bound); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF BoundRect(RectF bound)
		{
			//부동소수점 오차때문에 여유를 두어야 한다.
			const float MARGIN = 0.001f;
			if( Width > bound.Width + MARGIN ||
				Height > bound.Height + MARGIN ) { throw new Exception("Rect::Bound() : Bound Rect is too small"); }

			float dx = 0;
			if( left < bound.left ) { dx = bound.left - left; }
			else if( right > bound.right ) { dx = bound.right - right; }

			float dy = 0;
			if( top < bound.top ) { dy = bound.top - top; }
			else if( bottom > bound.bottom ) { dy = bound.bottom - bottom; }

			return MoveRect(dx, dy);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 제한한다. bound 보다 크다면 크기를 변형한다.
		public void Restrain(RectF bound) { this = RestrainRect(bound); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF RestrainRect(RectF bound)
		{
			var r = FromPosSize(LT.x, LT.y, Math.Min(Width, bound.Width), Math.Min(Height, bound.Height));

			float dx = 0;
			if( r.left < bound.left ) { dx = bound.left - r.left; }
			else if( r.right > bound.right ) { dx = bound.right - r.right; }

			float dy = 0;
			if( r.top < bound.top ) { dy = bound.top - r.top; }
			else if( r.bottom > bound.bottom ) { dy = bound.bottom - r.bottom; }

			r.Move(dx, dy);
			return r;
		}

		//===============================================================================================================================================
		//	상대좌표 계산
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	절대 좌표 absPoint 를 상대좌표로 변환한다.
		public Point GetNormalizedPoint(Point absPoint)
		{
			return new Point((absPoint.X - left) / Width, (absPoint.Y - top) / Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	상대좌표 relPoint 를 절대좌표로 변환한다.
		public Point GetDenormalizedPoint(Point relPoint)
		{
			return new Point(left + Width * relPoint.X, top + Height * relPoint.Y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	baseRect 를 기준으로 상대좌표로 변환한다.
		public void NormalizedBy(RectF baseRect) { this = NormalizedByRect(baseRect); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	baseRect 를 기준으로 상대좌표로 변환한다.
		public RectF NormalizedByRect(RectF baseRect)
		{
			return FromPosSize(
				(Left - baseRect.Left) / baseRect.Width,
				(Top - baseRect.Top) / baseRect.Height,
				Width / baseRect.Width,
				Height / baseRect.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	baseRect 를 기준으로 (상대좌표에서) 절대좌표로 변환한다.
		public void DenormalizedBy(RectF baseRect) { this = DenormalizedByRect(baseRect); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF DenormalizedByRect(RectF baseRect)
		{
			return FromPosSize(
				Left * baseRect.Width + baseRect.Left,
				Top * baseRect.Height + baseRect.Top,
				Width * baseRect.Width,
				Height * baseRect.Height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	(절대좌표인) this가 상대좌표 relRect 로 표현될때의 기준계를 계산한다.
		public RectF CalculateBaseRect(RectF relRect)
		{
			float baseWidth = Width / relRect.Width;
			float baseHeight = Height / relRect.Height;
			return FromPosSize(
				Left - relRect.Left * baseWidth,
				Top - relRect.Top * baseHeight,
				baseWidth, baseHeight);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Rect 기준 비례 변환. from 기준의 좌표에서 to 기준좌표로 변환한다.
		public void Proportion(RectF from, RectF to) { this = ProportionRect(from, to); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectF ProportionRect(RectF from, RectF to)
		{
			return FromPosSize(
				to.Left + (Left-from.Left) * to.Width / from.Width,
				to.Top + (Top-from.Top) * to.Height / from.Height,
				Width * to.Width / from.Width,
				Height * to.Height / from.Height);
		}

		//===============================================================================================================================================
		//	비교
		public bool IsSame(RectF rhs, float eps)
		{
			return left.EqualWithError(rhs.left, eps) &&
				top.EqualWithError(rhs.top, eps) &&
				right.EqualWithError(rhs.right, eps) &&
				bottom.EqualWithError(rhs.bottom, eps);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator==(RectF lhs, RectF rhs)
		{
			return lhs.left == rhs.left && lhs.top == rhs.top && lhs.right == rhs.right && lhs.bottom == rhs.bottom;
		}
		public static bool operator!=(RectF lhs, RectF rhs) { return !(lhs == rhs); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public override bool Equals(object rhs) { return this == (RectF)rhs; }
		public bool Equals(RectF rhs) { return this == rhs; }
		public override int GetHashCode() { return base.GetHashCode(); }
		
		//===============================================================================================================================================
		//	기타
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스트링으로
		public override string ToString()
		{
			return string.Format("Rect({0}, {1}, {2}, {3}", left, top, right, bottom);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	RectI
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public struct RectI
	{
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int Left { get { return left; } set { left = value; } }
		public int Top { get { return top; } set { top = value; } }
		public int Right { get { return right; } set { right = value; } }
		public int Bottom { get { return bottom; } set { bottom = value; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int Width
		{
			get { return right - left; }
			set { right = left + value; }
		}
		public int Height
		{
			get { return bottom - top; }
			set { bottom = top + value; }
		}
		public PointI Size
		{
			get { return new PointI(Width, Height); }
			set { Width = value.x; Height = value.y; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool Empty { get { return Width == 0 && Height == 0; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public PointI LT
		{
			get { return new PointI(left, top); }
			set { left = value.x; top = value.y; }
		}
		public PointI RT
		{
			get { return new PointI(right, top); }
			set { right = value.x; top = value.y; }
		}
		public PointI LB
		{
			get { return new PointI(left, bottom); }
			set { left = value.x; bottom = value.y; }
		}
		public PointI RB
		{
			get { return new PointI(right, bottom); }
			set { right = value.x; bottom = value.y; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중앙 (내림처리주의)
		public PointI Center
		{
			//get { return new PointI((left + right) / 2, (top + bottom) / 2); }
			get { return new PointI(left + Width / 2, top + Height / 2); }
			set { SetCenter(value); }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int Area
		{
			get { return Width * Height; }
		}

		//===============================================================================================================================================
		//	필드
		public static RectI ZeroRect = new RectI(0, 0, 0, 0);
		public static RectI UnitRect = new RectI(0, 0, 1, 1);

		public int left;
		public int top;
		public int right;
		public int bottom;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public RectI(PointI lt, PointI rb) { left = lt.x; top = lt.y; right = rb.x; bottom = rb.y; }
		public RectI(int left, int top, int right, int bottom) { this.left = left; this.top = top; this.right = right; this.bottom = bottom; }
		public RectI(RectI r) { left = r.left; top = r.top; right = r.right; bottom = r.bottom; }

		//===============================================================================================================================================
		//	생성
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기에서 Rect 생성. 위치는 (0, 0)
		public static RectI FromSize(int w, int y)
		{
			return new RectI(0, 0, w, y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectI FromSize(PointI size) { return FromSize(size.x, size.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	왼쪽, 위, 오른족, 아래 좌표에서 Rect 생성
		public static RectI FromBoundary(int left, int top, int right, int bottom)
		{
			return new RectI(left, top, right, bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectI FromBoundary(PointI lt, PointI rb) { return new RectI(lt.x, lt.y, rb.x, rb.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	Left/Top 좌표와 Width/Height 에서 Rect 생성
		public static RectI FromPosSize(int left, int top, int width, int height)
		{
			return new RectI(left, top, left + width, top + height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectI FromPosSize(PointI pos, PointI size) { return FromPosSize(pos.x, pos.y, size.x, size.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점과 크기로부터 Rect 생성. (내림처리주의)
		public static RectI FromCenterSize(int centerX, int centerY, int width, int height)
		{
			int halfWidth = width / 2;
			int halfHeight = height / 2;
			return new RectI(centerX - halfWidth, centerY - halfHeight,
				centerX + halfWidth + (width % 2 == 0 ? 0 : 1),
				centerY + halfHeight + (height % 2 == 0 ? 0 : 1));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static RectI FromCenterSize(PointI center, PointI size) { return FromCenterSize(center.x, center.y, size.x, size.y); }

		//===============================================================================================================================================
		//	위치/크기 변경
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	위치 직접 설정
		public void SetBoundary(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetBoundary(PointI lt, PointI rb) { SetBoundary(lt.x, lt.y, rb.x, rb.y);}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	위치, 크기로 설정
		public void SetPosSize(int left, int top, int width, int height)
		{
			SetBoundary(left, top, left + width, right + height);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetPosSize(PointI lt, PointI size) { SetBoundary(lt, lt + size); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	크기 변경
		public void SetSize(int w, int h)
		{
			Width = w;
			Height = h;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetSize(PointI size) { Size = size; }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 설정한다. (내림처리주의)
		public void SetCenter(PointI center)
		{
			var hw = Width / 2;
			var hh = Height / 2;
			SetBoundary(center.x - hw, center.y - hh,
				center.x + hw + (Width % 2 == 0 ? 0 : 1),
				center.y + hh + (Height % 2 == 0 ? 0 : 1));			
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	중심점을 기준으로 크기를 설정한다. (내림처리주의)
		public void SetSizeOnCenter(int width, int height)
		{
			var c = Center;
			SetSize(width, height);
			Center = c;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void SetSizeOnCenter(PointI size) { SetSizeOnCenter(size.x, size.y); }

		//===============================================================================================================================================
		//	변형
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	이동한다.
		public void Move(int x, int y)
		{
			left += x;
			top += y;
			right += x;			
			bottom += y;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Move(PointI p) { Move(p.x, p.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI MoveRect(int x, int y)
		{
			return FromBoundary(left + x, top + y, right + x, bottom + y);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI MoveRect(PointI p) { return MoveRect(p.x, p.y); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	교차
		public void Intersect(RectI r)
		{
			left = Math.Max(left, r.left);
			top = Math.Max(top, r.top);
			right = Math.Min(right, r.right);
			bottom = Math.Min(bottom, r.bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI IntersectRect(RectI r)
		{
			return new RectI(Math.Max(left, r.left), Math.Max(top, r.top), Math.Min(right, r.right), Math.Min(bottom, r.bottom));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsIntersected(RectI r)
		{
			return !IntersectRect(r).Empty;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포함
		public bool IsContained(RectI r)
		{
			return left <= r.left && top <= r.top && right >= r.right && bottom >= r.bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public bool IsContained(PointI p)
		{
			return left <= p.x && p.x < right && top <= p.y && p.y < bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	병합
		public void Merge(RectI r)
		{
			SetBoundary(Math.Min(left, r.left), Math.Min(top, r.top), Math.Max(right, r.right), Math.Max(bottom, r.bottom));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI MergeRect(RectI r)
		{
			return FromBoundary(Math.Min(left, r.left), Math.Min(top, r.top), Math.Max(right, r.right), Math.Max(bottom, r.bottom));
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	각 방향으로 확장한다.
		public void Expand(int left, int top, int right, int bottom)
		{
			this.left -= left;
			this.top -= top;
			this.right += right;
			this.bottom += bottom;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public void Expand(int leftRight, int topBottom) { Expand(leftRight, topBottom, leftRight, topBottom); }
		public void Expand(PointI d) { Expand(d.x, d.y, d.x, d.y); }
		public void Expand(int d) { Expand(d, d, d, d); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI ExpandRect(int left, int top, int right, int bottom)
		{
			return FromBoundary(
				this.left - left,
				this.top - top,
				this.right + right,
				this.bottom + bottom);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI ExpandRect(int leftRight, int topBottom) { return ExpandRect(leftRight, topBottom, leftRight, topBottom); }
		public RectI ExpandRect(PointI d) { return ExpandRect(d.x, d.y, d.x, d.y); }
		public RectI ExpandRect(int d) { return ExpandRect(d, d, d, d); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 집어넣는다. 불가능하면 예외가 발생한다.
		public void Bound(RectI bound) { this = BoundRect(bound); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI BoundRect(RectI bound)
		{
			if( Width > bound.Width ||
				Height > bound.Height ) { throw new Exception("Rect::Bound() : Bound Rect is too small"); }

			int dx = 0;
			if( left < bound.left ) { dx = bound.left - left; }
			else if( right > bound.right ) { dx = bound.right - right; }

			int dy = 0;
			if( top < bound.top ) { dy = bound.top - top; }
			else if( bottom > bound.bottom ) { dy = bound.bottom - bottom; }

			return MoveRect(dx, dy);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	bound 영역의 안쪽으로 제한한다. bound 보다 크다면 크기를 변형한다.
		public void Restrain(RectI bound) { this = RestrainRect(bound); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public RectI RestrainRect(RectI bound)
		{
			var r = FromPosSize(LT.x, LT.y, Math.Min(Width, bound.Width), Math.Min(Height, bound.Height));

			int dx = 0;
			if( r.left < bound.left ) { dx = bound.left - r.left; }
			else if( r.right > bound.right ) { dx = bound.right - r.right; }

			int dy = 0;
			if( r.top < bound.top ) { dy = bound.top - r.top; }
			else if( r.bottom > bound.bottom ) { dy = bound.bottom - r.bottom; }

			r.Move(dx, dy);
			return r;
		}

		//===============================================================================================================================================
		//	비교
		public bool IsSame(RectI rhs)
		{
			return this == rhs;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool operator==(RectI lhs, RectI rhs)
		{
			return lhs.left == rhs.left && lhs.top == rhs.top && lhs.right == rhs.right && lhs.bottom == rhs.bottom;
		}
		public static bool operator!=(RectI lhs, RectI rhs) { return !(lhs == rhs); }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public override bool Equals(object rhs) { return this == (RectI)rhs; }
		public bool Equals(RectI rhs) { return this == rhs; }
		public override int GetHashCode() { return base.GetHashCode(); }
		
		//===============================================================================================================================================
		//	기타
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	2차원으로 순회
		public delegate void Enumerate2D(int x, int y);
		public void ForEach2D(Enumerate2D enumerator2D)
		{
			for( int y = top; y < bottom; ++y )
			{
				for( int x = left; x < right; ++x )
				{
					enumerator2D(x, y);
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스트링으로
		public override string ToString()
		{
			return string.Format("Rect({0}, {1}, {2}, {3}", left, top, right, bottom);
		}
	}
}
