using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	IntExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class IntExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	작은쪽으로 스냅
		public static int SnapLower(this int value, int snap)
		{
			//return value / snap * snap;
			int c = value > 0 ? 0 : -1;
			return (value + c * (snap -1))/ snap * snap;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	큰쪽으로 스냅
		public static int SnapUpper(this int value, int snap)
		{
			//return (value + snap -1) / snap * snap;
			int c = value > 0 ? 1 : 0;
			return (value + c * (snap -1)) / snap * snap;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	큰쪽으로 반
		public static int BiggerHalf(this int value)
		{
			return value / 2 + value % 2;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	값을 증가시키면서 회전한다.
		public static int RotateInc(this int value, int max)
		{
			return (value + 1) % max;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	값을 감소시키면서 회전한다.
		public static int RotateDec(this int value, int max)
		{
			return (value + max - 1) % max;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	total 을 n 개로 나누는데 이때 i 번째에게 돌아갈 몫은?
		public static int Distribute(int total, int n, int i)
		{
			return total / n + (i < total % n ? 1 : 0);
		}

		//===============================================================================================================================================
		//	비트연산
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	플래그를 세운다. [LSB, MSB] => [0, 31]
		public static int BitSet(this int value, int pos)
		{
			return value | (0x01 << pos);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	플래그를 끈다.
		public static int BitReset(this int value, int pos)
		{
			return value & ~(0x01 << pos);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	플래그를 토글한다
		public static int BitToggle(this int value, int pos)
		{
			return value ^ (0x01 << pos);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	플래그가 설정되었는가?
		public static bool IsBitSet(this int value, int pos)
		{
			return (value & (0x01 << pos)) != 0;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	FloatExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class FloatExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	float 가 epsilon 범위인지 확인
		public static bool IsEpsilonRange(this float value)
		{
			return -float.Epsilon <= value && value <= float.Epsilon;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	float 가 몹쓸 범위인지 확인
		public static bool IsBadDividingFloat(this float value)
		{
			return float.IsNaN(value) || IsEpsilonRange(value);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	입실론을 사용한 비교
		public static bool EqualWithError(this float lhs, float rhs, float error)
		{
			float diff = lhs - rhs;
			return -error < diff && diff < error;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	DoubleExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class DoubleExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	입실론을 사용한 비교
		public static bool EqualWithError(this double lhs, double rhs, double error)
		{
			double diff = lhs - rhs;
			return -error < diff && diff < error;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	StringExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class StringExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	빈 스트링인가?
		public static bool IsEmpty(this string str)
		{
			return str.Length == 0;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	라인수를 센다.
		public static int LineCount(this string str)
		{
			int newLineCount = 1;
			for( int i = 0; i < str.Length; ++i )
			{
				if( str[i] == '\n' ) { newLineCount++; }
			}
			return newLineCount;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디폴트 값이 제공된 int 로 변환
		public static int ToInt(this string text, int defaultValue)
		{
			int result;
			if( int.TryParse(text, out result) ) { return result; }
			return defaultValue;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디폴트 값이 제공된 float 로 변환
		public static float ToDouble(this string text, float defaultValue)
		{
			float result;
			if( float.TryParse(text, out result) ) { return result; }
			return defaultValue;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	디폴트 값이 제공된 double 로 변환
		public static double ToDouble(this string text, double defaultValue)
		{
			double result;
			if( double.TryParse(text, out result) ) { return result; }
			return defaultValue;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	... 을 포함해서 제한길이를 설정한다.
		public static string EllipsisReduce(this string str, int limitLength)
		{
			if( str.Length <= limitLength ) { return str; }
			return str.Substring(0, limitLength - 3) + "...";
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	포함된 모든 공백문자를 제거한다.
		public static string RemoveWhitespaceChar(this string str)
		{
			var builder = new StringBuilder(str.Length);
			for( int i = 0; i < str.Length; ++i )
			{
				if( !char.IsWhiteSpace(str, i) )
				{
					builder.Append(str[i]);
				}
			}
			return builder.ToString();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	IEnumerableExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class IEnumerableExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	최대로 평가되는 엘리먼트를 구한다.
		public static TSource MaxElement<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			var enumerator = source.GetEnumerator();
			if( !enumerator.MoveNext() ) { return default(TSource); }

			TSource maxElem = enumerator.Current;
			int maxValue = selector(maxElem);
			while( enumerator.MoveNext() )
			{
				int curValue = selector(enumerator.Current);
				if( curValue > maxValue )
				{
					maxValue = curValue;
					maxElem = enumerator.Current;
				}
			}
			return maxElem;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	ListExtMethod
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class ListExtMethod
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	주어진 원소로 채운다.
		public static void Fill<T>(this List<T> list, int count, T element)
		{
			for( int i = 0; i < count; ++i )
			{
				list.Add(element);
			}
		}
	}
}
