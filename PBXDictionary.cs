using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXDictionary : Dictionary<string, object>
	{
		
	}
	
	public class PBXDictionary<T> : Dictionary<string, T>
	{
		public PBXDictionary()
		{
			
		}
		
		public PBXDictionary( PBXDictionary genericDictionary )
		{
			foreach( KeyValuePair<string, object> currentItem in genericDictionary ) {
				if( ((string)((PBXDictionary)currentItem.Value)[ "isa" ]).CompareTo( typeof(T).Name ) == 0 ) {
					T instance = (T)System.Activator.CreateInstance( typeof(T), currentItem.Key, (PBXDictionary)currentItem.Value );
					this.Add( currentItem.Key, instance );
				}
			}	
		}
	}
}
