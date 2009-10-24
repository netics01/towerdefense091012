﻿using System;
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
		public static readonly Point ZeroPoint;

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
		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	난수생성좀 편하게 합시다.
		public static Random Random = new Random();

		//===============================================================================================================================================
		//	메소드
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static int Next(int max)
		{
			return Random.Next(max);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static float Next(float start, float end)
		{
			return start + (end - start) * (float)Random.NextDouble();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	start, end 사이의 값을 반환
		public static double Next(double start, double end)
		{
			return start + (end - start) * Random.NextDouble();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	[0, 1] 사이의 Point 반환
		public static Point GetPoint()
		{
			return new Point(Random.NextDouble(), Random.NextDouble());
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	[-0.5, 0.5] 사이의 Point 반환
		public static Point GetCenterPoint()
		{
			return new Point(Random.NextDouble() - 0.5, Random.NextDouble() - 0.5);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	방향벡터 반환
		public static Point GetDirectionPoint()
		{
			return GetCenterPoint().Normalize();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	랜덤 단위 벡터(Point)를 얻는다.
		public static Point GetUnitPoint()
		{
			var unit = new Vector(Random.NextDouble() - 0.5, Random.NextDouble() - 0.5);
			unit.Normalize();
			return unit.ToPoint();
		}
	}
}
