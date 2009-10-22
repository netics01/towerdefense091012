

/*
namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Validity
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public static class Validity
	{
		public static bool invalid;
		public static bool IsInvalid { get { return invalid; } }
		public static bool IsValid { get { return !invalid; } }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	invalid 상태를 초기화한다.
		public static void Clear() { invalid = false; }

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	조건이 유효하지 않다면 로그를 출력한다.
		public static bool Check(bool condition, string log)
		{
			if( !condition ) { Log.Info(log); invalid = true; }
			return condition;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool Check(bool condition, string log, object arg0)
		{
			if( !condition ) { Log.Info(log, arg0); invalid = true; }
			return condition;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool Check(bool condition, string log, params object[] args)
		{
			if( !condition ) { Log.Info(log, args); invalid = true; }
			return condition;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool Check(bool condition, string log, object arg0, object arg1)
		{
			if( !condition ) { Log.Info(log, arg0, arg1); invalid = true; }
			return condition;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	preCondition 일때만 condition 을 확인한다.
		public static bool CheckIf(bool preCondition, bool condition, string log)
		{
			bool result = !preCondition || condition;
			if( !result ) { Log.Info(log); invalid = true; }
			return result;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool CheckIf(bool preCondition, bool condition, string log, object arg0)
		{
			bool result = !preCondition || condition;
			if( !result ) { Log.Info(log, arg0); invalid = true; }
			return result;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool CheckIf(bool preCondition, bool condition, string log, params object[] args)
		{
			bool result = !preCondition || condition;
			if( !result ) { Log.Info(log, args); invalid = true; }
			return result;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public static bool CheckIf(bool preCondition, bool condition, string log, object arg0, object arg1)
		{
			bool result = !preCondition || condition;
			if( !result ) { Log.Info(log, arg0, arg1); invalid = true; }
			return result;
		}
	}
}

*/