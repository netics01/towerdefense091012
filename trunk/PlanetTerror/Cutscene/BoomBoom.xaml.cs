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
using System.Diagnostics;

using PlanetTerror.Util;


namespace PlanetTerror
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	BoomBoom
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public partial class BoomBoom : UserControl
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//	Trail
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//	귀찮다.  필드 그냥 공개
		class Trail
		{
			//===============================================================================================================================================
			//	필드
			public PathGeometry path;
			public double t;
			public double deltaT;
			public Point curPos;

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	생성자
			public Trail(Path rawPath)
			{
				this.path = rawPath.Data.GetFlattenedPathGeometry();
				Point startPoint, endPoint, tangent;
				path.GetPointAtFractionLength(0, out startPoint, out tangent);
				path.GetPointAtFractionLength(1, out endPoint, out tangent);

				deltaT = 1 / ((startPoint.X - endPoint.X) / Game.Setting.core.boomDistance);
				Debug.Assert( deltaT > 0 );

				Reset();
			}

			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	초기화
			public void Reset()
			{
				t = 0;
				Point tangent;
				path.GetPointAtFractionLength(t, out curPos, out tangent);
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	검사.
			public bool Check(double leftX, double rightX)
			{
				return (leftX < curPos.X && curPos.X <= rightX);
			}
			//-----------------------------------------------------------------------------------------------------------------------------------------------
			//	다음 위치로
			public void Next()
			{
				t += deltaT;
				Point tangent;
				path.GetPointAtFractionLength(t, out curPos, out tangent);
			}
		}

		//===============================================================================================================================================
		//	필드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		UpdatePump pump;
		bool bLastVisible;
		bool bBoom;
		float elapsedTime;
		int boomCount;
		List<Trail> trails;
		ClassPool<Boom> booms;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public BoomBoom()
		{
			this.InitializeComponent();

			pump = new UpdatePump();
			pump.Update += new UpdatePump.UpdateHandler(pump_Update);

			bBoom = false;
			trails = new List<Trail>();

			Loaded += new RoutedEventHandler(BoomBoom_Loaded);
			IsVisibleChanged += new DependencyPropertyChangedEventHandler(BoomBoom_IsVisibleChanged);
		}

		//===============================================================================================================================================
		//	핸들러
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomBoom_Loaded(object sender, RoutedEventArgs e)
		{
			bLastVisible = this.GetVisible();
			if( this.IsInDesignMode() ) { return; }

			booms = new ClassPool<Boom>(200);
			pump.Begin();

			for( int i = 0; i < LayoutRoot.Children.Count; ++i )
			{
				var path = LayoutRoot.Children[i] as Path;
				if( path != null )
				{
					trails.Add(new Trail(path));
				}
			}
			LayoutRoot.RemoveType<Path>();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void BoomBoom_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var curVisible = this.GetVisible();
			if( curVisible && !bLastVisible )
			{
				bLastVisible = curVisible;
			}
			else
			{
				bLastVisible = curVisible;
				return;
			}			

			bBoom = true;
			elapsedTime = 0;
			for( int i = 0; i < trails.Count; ++i )
			{
				trails[i].Reset();
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		void pump_Update(float delta)
		{
			if( !bBoom ) { return; }

			do 
			{
				float step = Math.Min(delta, (float)Game.Setting.core.boomLogicFrame);
				var lastTime = elapsedTime;
				elapsedTime += step;
				Process(lastTime, elapsedTime);
				delta = Math.Max(0, delta - step);
			} while ( delta > 0);

			for( int i = LayoutRoot.Children.Count - 1; i >= 0; --i )
			{
				var b = LayoutRoot.Children[i] as Boom;
				if( b != null && b.Done )
				{
					LayoutRoot.Children.RemoveAt(i);
					booms.Deallocate(b);
				}
			}

			bBoom = (elapsedTime < Game.Setting.core.boomTime);
			if( bBoom == false)
			{
				Debug.Print("{0}", boomCount);
			}
		}

		//===============================================================================================================================================
		//	전용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	타임스텝을 하나 돌린다.
		void Process(double lastTime, double curTime)
		{
			var leftX = ActualWidth * (1 - curTime/Game.Setting.core.boomTime);
			var rightX = ActualWidth * (1 - lastTime/Game.Setting.core.boomTime);
			for( int i = 0; i < trails.Count; ++i )
			{
				var trail = trails[i];
				if( trail.Check(leftX, rightX) )
				{
					var boom = booms.Allocate();
					boom.SetVisible(false);
					boom.SetZIndex(boomCount);
					boom.SetCenter(trail.curPos);
					LayoutRoot.Children.Add(boom);
					boom.SetVisible(true);

					trail.Next();
					boomCount++;
				}
			}
		}
	}
}