using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.KabamXCodeEditor
{
	public class PBXList : ArrayList
	{
		public PBXList()
		{
			
		}
		
		public PBXList( object firstValue )
		{
			this.Add( firstValue );
		}
	}
	
//	public class PBXList<T> : ArrayList
//	{
//		public int Add( T value )
//		{
//			return (ArrayList)this.Add( value );
//		}
//	}
}
