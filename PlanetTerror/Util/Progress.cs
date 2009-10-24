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
	//	ProgressMgr
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Idea:
	//	외부에서 Update() 를 호출할 필요가 없도록 직전 StoryBoard 나 UpdatePump 등의 객체를 가지고 있을까?
	//	하지만 게임로직에서 쓰는 Update 루프와 동기가 맞아서 돌아가는게 더 낮지 않겠어? 정확히 언제 실행될지 제어할 수 있도록.
	public class ProgressMgr
	{
		#region Singleton
		static ProgressMgr instance;
		public static ProgressMgr Instance
		{
			get
			{
				if( instance == null ) { instance = new ProgressMgr(); }
				return instance;
			}			
		}
		#endregion Singleton

		//===============================================================================================================================================
		//	타입
		public interface IProgress
		{
			void Update(float delta);
			bool IsSetDeleted();
		}

		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public List<IProgress> Progresses { get; protected set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		ProgressMgr()
		{
			Progresses = new List<IProgress>();
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	IProgress 등록
		public void Register(IProgress progress)
		{
			Progresses.Add(progress);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	등록해제. 쓸일이 있을까?
		public void Unregister(IProgress progress)
		{
			Progresses.Remove(progress);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			for( int i = 0; i < Progresses.Count; ++i )
			{
				Progresses[i].Update(delta);
			}
			for( int i = Progresses.Count - 1; i >= 0; i-- )
			{
				if( Progresses[i].IsSetDeleted() )
				{
					Progresses.RemoveAt(i);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ProgressVarBase
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Idea:
	//	변수 타입을 double 이외에 다양하게
	//	값 변경 방식을 선형적인 것 외에 다양하게
	//	리버스? 모드?
	public abstract class ProgressVarBase : ProgressMgr.IProgress
	{
		//===============================================================================================================================================
		//	타입
		public delegate void Setter(double value);
		public delegate void Completer(double value);

		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public float TotalTime { get; set; }
		public float CurTime { get; set; }
		public float TimeRatio { get { return CurTime / TotalTime; } }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public double StartValue { get; set; }
		public double EndValue { get; set; }

		//===============================================================================================================================================
		//	필드
		protected Setter setter;
		protected Completer completer;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public ProgressVarBase()
		{
			ProgressMgr.Instance.Register(this);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	초기화
		//	생성자에서 하면 상속객체마다 귀찮은 코드를 작성해야 하기 때문에 초기화 메소드를 사용하는 쪽으로 디자인
		public void Initialize(Setter setter, Completer completer, float time, double startValue, double endValue)
		{
			this.setter = setter;
			this.completer = completer;
			TotalTime = time;
			CurTime = 0.0f;
			StartValue = startValue;
			EndValue = endValue;
		}

		//===============================================================================================================================================
		//	IProgress 구현
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	업데이트
		public void Update(float delta)
		{
			OnUpdate(delta);

			if( completer != null &&
				IsSetDeleted() )
			{
				completer(EndValue);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	삭제처리
		public bool IsSetDeleted()
		{
			return CurTime == TotalTime;
		}

		//===============================================================================================================================================
		//	보호
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	템플릿 메소드
		protected abstract void OnUpdate(float delta);
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ProgressVar
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class ProgressVar : ProgressVarBase
	{
		protected override void OnUpdate(float delta)
		{
			CurTime = Math.Min(CurTime + delta, TotalTime);
			setter(StartValue + (EndValue - StartValue) * CurTime / TotalTime);
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ProgressVarDelta
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class ProgressVarDelta : ProgressVarBase
	{
		protected override void OnUpdate(float delta)
		{
			var diff = EndValue - StartValue;
			var oldValue = diff * CurTime / TotalTime;
			CurTime = Math.Min(CurTime + delta, TotalTime);
			var newValue = diff * CurTime / TotalTime;			
			setter(newValue - oldValue);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Progress
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	사용하기 좋게 래핑
	//	todo:핸들을 받아놨다가 캔슬시킬수 있어야겠다. 하나의 변수에 여러 progress 가 값을 쓰게 되는 경우 확실하게 결과를 예측하려면
	//		이 기능이 꼭 필요하다.
	//	idea:Delayed Action
	public class Progress
	{
		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	시간에 맞춰 변수에 값을 설정한다.
		public static void SetVar(ProgressVarBase.Setter setter, ProgressVarBase.Completer completer, float time, double startValue, double endValue)
		{
			var progress = new ProgressVar();
			progress.Initialize(setter, completer, time, startValue, endValue);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void SetVar(ProgressVarBase.Setter setter, float time, double startValue, double endValue)
		{
			var progress = new ProgressVar();
			progress.Initialize(setter, null, time, startValue, endValue);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	시간에 맞춰 변수에 값을 더한다.
		public static void AddToVar(ProgressVarBase.Setter setter, ProgressVarBase.Completer completer, float time, double startValue, double endValue)
		{
			var progress = new ProgressVarDelta();
			progress.Initialize(setter, completer, time, startValue, endValue);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void AddToVar(ProgressVarBase.Setter setter, float time, double startValue, double endValue)
		{
			var progress = new ProgressVarDelta();
			progress.Initialize(setter, null, time, startValue, endValue);
		}
	}
}