using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Windows;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	DebugHelper
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class DebugHelper
	{
		public static bool IsNetics
		{
			//get { return ProtoExileGame.USER == "netics"; }
			get { return true; }
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Validity
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class Validity
	{
// 		public static bool invalid;
// 		public static bool IsInvalid { get { return invalid; } }
// 		public static bool IsValid { get { return !invalid; } }
// 
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	invalid 상태를 초기화한다.
// 		public static void Clear() { invalid = false; }
// 
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	조건이 유효하지 않다면 로그를 출력한다.
// 		public static bool Check(bool condition, string log)
// 		{
// 			if( !condition ) { Log.Info(log); invalid = true; }
// 			return condition;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool Check(bool condition, string log, object arg0)
// 		{
// 			if( !condition ) { Log.Info(log, arg0); invalid = true; }
// 			return condition;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool Check(bool condition, string log, params object[] args)
// 		{
// 			if( !condition ) { Log.Info(log, args); invalid = true; }
// 			return condition;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool Check(bool condition, string log, object arg0, object arg1)
// 		{
// 			if( !condition ) { Log.Info(log, arg0, arg1); invalid = true; }
// 			return condition;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	preCondition 일때만 condition 을 확인한다.
// 		public static bool CheckIf(bool preCondition, bool condition, string log)
// 		{
// 			bool result = !preCondition || condition;
// 			if( !result ) { Log.Info(log); invalid = true; }
// 			return result;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool CheckIf(bool preCondition, bool condition, string log, object arg0)
// 		{
// 			bool result = !preCondition || condition;
// 			if( !result ) { Log.Info(log, arg0); invalid = true; }
// 			return result;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool CheckIf(bool preCondition, bool condition, string log, params object[] args)
// 		{
// 			bool result = !preCondition || condition;
// 			if( !result ) { Log.Info(log, args); invalid = true; }
// 			return result;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static bool CheckIf(bool preCondition, bool condition, string log, object arg0, object arg1)
// 		{
// 			bool result = !preCondition || condition;
// 			if( !result ) { Log.Info(log, arg0, arg1); invalid = true; }
// 			return result;
// 		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	AssertH
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class AssertH
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	preCondition 일때만 condition 을 확인한다.
		public static void AssertIf(bool preCondition, bool condition)
		{
			Debug.Assert(!preCondition || condition);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void AssertIf(bool preCondition, bool condition, string message)
		{
			Debug.Assert(!preCondition || condition, message);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Helper
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class Helper
	{
		//===============================================================================================================================================
		//	상수객체
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	0.0, 0.0, 1.0, 1.0 으로 채워진 Rect
// 		public static readonly Rect InfiniteRect = new Rect(float.MinValue, float.MinValue, float.PositiveInfinity, float.PositiveInfinity);
// 		public static readonly Rect UnitRect = new Rect(0, 0, 1, 1);

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	스왑
		public static void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	네임스페이스를 제거한다. (제일 마지막의 . 이후의 문자열만 남긴다.)
		public static string GetClassName(string fullClassName)
		{
			return fullClassName.Substring(fullClassName.LastIndexOf('.') + 1);
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	T 를 시리얼라이징 한다.
		public static void Serialize<T>(string filePath, T t)
		{
			var serializer = new XmlSerializer(typeof(T));
			var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			serializer.Serialize(stream, t);
			stream.Close();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	T 를 디시리얼라이징 한다.
		public static T Deserialize<T>(string filePath)
		{
			var serializer = new XmlSerializer(typeof(T));
			var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read);
			T result = (T)serializer.Deserialize(stream);
			stream.Close();
			return result;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static void Deserialize<T>(string filePath, out T result)
		{
			result = Deserialize<T>(filePath);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	NumberH
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class NumberH
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	클램프
		public static int Clamp(int min, int value, int max)
		{
			return Math.Min(Math.Max(min, value), max);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static float Clamp(float min, float value, float max)
		{
			return Math.Min(Math.Max(min, value), max);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static double Clamp(double min, double value, double max)
		{
			return Math.Min(Math.Max(min, value), max);
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	비율계산. [a, b] 구간의 t 에 해당되는 값을 [c, d] 구간에서 비례해서 구한다.
		public static float Proportion(float a, float t, float b, float c, float d)
		{
			return c + (t-a) * (d-c) / (b-a);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static double Proportion(double a, double t, double b, double c, double d)
		{
			return c + (t-a) * (d-c) / (b-a);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	RandomH
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class RandomH
	{
// 		//===============================================================================================================================================
// 		//	프로퍼티
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	난수생성좀 편하게 합시다.
// 		public static EngineRandom Random = new EngineRandom();
// 
// 		//===============================================================================================================================================
// 		//	메소드
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static int Next(int max)
// 		{
// 			return Random.Next(max);
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		public static float Next(float start, float end)
// 		{
// 			return start + (end - start) * Random.NextFloat();
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	start, end 사이의 값을 반환
// 		public static double Next(double start, double end)
// 		{
// 			return start + (end - start) * Random.NextDouble();
// 		}
// 
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	[0, 1] 사이의 Vec2 반환
// 		public static Vec2 GetVec2()
// 		{
// 			return new Vec2(Random.NextFloat(), Random.NextFloat());
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	[-0.5, 0.5] 사이의 Vec2 반환
// 		public static Vec2 GetCenterVec2()
// 		{
// 			return new Vec2(Random.NextFloatCenter(), Random.NextFloatCenter());
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	랜덤 단위 벡터(Vec2)를 얻는다.
// 		public static Vec2 GetUnitVec2()
// 		{
// 			var unit = new Vec2(Random.NextFloatCenter(), Random.NextFloatCenter());
// 			return unit.GetNormalizeFast();
// 		}
// 
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	랜덤 벡터(Vec3)를 얻는다. 각 원소는 [-1.0, 1.0] 사이의 값을 갖는다.
// 		public static Vec3 GetCubeVec3()
// 		{
// 			return new Vec3(Random.NextFloatCenter(), Random.NextFloatCenter(), Random.NextFloatCenter()) * 2.0f;
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	랜덤 단위 벡터(Vec3)를 얻는다.
// 		public static Vec3 GetUnitVec3()
// 		{
// 			var unit = new Vec3(Random.NextFloatCenter(), Random.NextFloatCenter(), Random.NextFloatCenter());
// 			return unit.GetNormalizeFast();
// 		}
// 
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	랜덤 회전 쿼터니온을 얻는다.
// 		public static Quat GetQuat()
// 		{
// 			return Quat.FromDirectionZAxisUp(GetUnitVec3());
// 		}
// 		//-----------------------------------------------------------------------------------------------------------------------------------------------
// 		//	X 축을 중심으로 하고 꼭지각이 angle 인 Spherical Cone 에서 랜덤한 단위벡터를 얻는다.
// 		public static Vec3 GetXAxisSphericalConeVec3(float angle)
// 		{
// 			//YZ 평면상의 반지름 1인 원 위의 랜덤벡터
// 			var circle = new Vec3(0.0f, Random.NextFloatCenter(), Random.NextFloatCenter());
// 			circle.NormalizeFast();
// 			//YZ 평면상의 반지름 Tan(angle)인 원 내부의 랜덤벡터
// 			circle *= (0.0000001f + Random.NextFloat()) * MathFunctions.Tan(angle);
// 			//반지름 1인 Spherical Cone 상의 랜덤벡터
// 			circle.X = 1.0f;
// 			circle.NormalizeFast();
// 			return circle;
// 		}
	}
}
