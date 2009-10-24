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
	//	ClassPool
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	클래스 생성 Pooling
	public class ClassPool<T> where T : new()
	{
		//===============================================================================================================================================
		//	델리게이트/이벤트
		public delegate T CreateMethod();

		//===============================================================================================================================================
		//	프로퍼티
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int AvailableCount
		{
			get { return unusedList.Count; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int UsedCount
		{
			get { return usedList.Count; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int TotalAllocateCount
		{
			get { return AvailableCount + UsedCount; }
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public int AllocateSize { get; set; }

		//===============================================================================================================================================
		//	필드
		//const int ALLOCATE_SIZE = 20;
		const int ALLOCATE_SIZE = 2;
		CreateMethod createMethod;
		LinkedList<T> unusedList;
		LinkedList<T> usedList;

		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성자
		public ClassPool() : this(0, null) { }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public ClassPool(int count) : this(count, null) { }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public ClassPool(CreateMethod createMethod) : this(0, createMethod) { }
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		public ClassPool(int count, CreateMethod createMethod)
		{
			AllocateSize = ALLOCATE_SIZE;
			this.createMethod = createMethod;

			unusedList = new LinkedList<T>();
			usedList = new LinkedList<T>();
			Reserve(count);
		}

		//===============================================================================================================================================
		//	공용
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	생성
		public T Allocate()
		{
			if( AvailableCount <= 0 ) { Reserve(ALLOCATE_SIZE); }

			var node = unusedList.First;
			unusedList.RemoveFirst();
			usedList.AddFirst(node);
			return node.Value;
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	해제
		public void Deallocate(T t)
		{
			bool bSuccess = usedList.Remove(t);
			Debug.Assert(bSuccess);
			unusedList.AddFirst(t);
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	여유분을 생성해 둔다.
		public void Reserve(int count)
		{
			if( AvailableCount >= count ) { return; }

			int allocateCount = count - AvailableCount;
			for( int i = 0; i < allocateCount; ++i )
			{
				T t;
				if( createMethod != null ) { t = createMethod(); }
				else { t = new T(); }
				unusedList.AddFirst(t);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------
		//	외부에서 생성한 인스턴스를 받아둔다.
		public void Reserve(T t)
		{
			unusedList.AddFirst(t);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SilverlightExt
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	http://silverlight.net/forums/t/1730.aspx
	//	WPF 객체 복사를 위한 것인데, 그다지 믿음직하지가 못하다. 잘 안돌아감
    public static class SilverlightExt
    {
        public static T Clone<T>(T source)
        {
            T cloned = (T)Activator.CreateInstance(source.GetType());
 
            foreach( PropertyInfo curPropInfo in source.GetType().GetProperties() )
            {
                if( curPropInfo.GetGetMethod() != null &&
					curPropInfo.GetSetMethod() != null )
                {
                    // Handle Non-indexer properties
                    if( curPropInfo.Name != "Item" )
                    {
                        // get property from source
                        object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] {});
 
                        // clone if needed
						if( getValue != null &&
							getValue is DependencyObject )
						{
							getValue = Clone((DependencyObject)getValue);
						}
 
                        // set property on cloned
						if( getValue != null )
						{
							curPropInfo.GetSetMethod().Invoke(cloned, new object[] { getValue });
						}
                    }
                    // handle indexer
                    else
                    {
                        // get count for indexer
                        int numberofItemInColleciton =
                            (int)
                            curPropInfo.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(source, new object[] {});
 
                        // run on indexer
                        for( int i = 0; i < numberofItemInColleciton; i++ )
                        {
                            // get item through Indexer
                            object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] {i});
 
                            // clone if needed
							if( getValue != null &&
								getValue is DependencyObject )
							{
								getValue = Clone((DependencyObject)getValue);
							}
                            // add item to collection
                            curPropInfo.ReflectedType.GetMethod("Add").Invoke(cloned, new object[] {getValue});
                        }
                    }
                }
            }

            return cloned;
        }
    }
}
