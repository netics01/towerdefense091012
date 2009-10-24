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
using System.Diagnostics;
using System.Reflection;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Boxed
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class Boxed<T> where T : struct
	{
		//===============================================================================================================================================
		//	프로퍼티
		public T Value { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public Boxed() {}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public Boxed(T value) { Value = value; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	변환
		public static implicit operator Boxed<T>(T value)
		{
			return new Boxed<T>(value);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static implicit operator T(Boxed<T> value)
		{
			return value.Value;
		}
	}
}