using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXObject : PBXDictionary
	{
		protected const string ISA_KEY = "isa";
		public string guid;
		
		public PBXObject()
		{
			this[ ISA_KEY ] = this.GetType().Name;
			this.guid = GenerateGuid();
		}
		
		public PBXObject( string guid ) : this()
		{
			if( IsGuid( guid ) )
				this.guid = guid;
		}
		
		public PBXObject( string guid, PBXDictionary dictionary )
		{
			Debug.Log( "constructor parent " + this.GetType().Name );
			if( IsGuid( guid ) )
				this.guid = guid;
			
			foreach( KeyValuePair<string, object> item in dictionary ) {
				this.Add( item.Key, item.Value );
			}
		}
		
		public static bool IsGuid( string aString )
		{
			return System.Text.RegularExpressions.Regex.IsMatch( aString, @"^[A-F0-9]{24}$" );
		}
		
		public static string GenerateGuid()
		{
			return System.Guid.NewGuid().ToString("N").Substring( 8 ).ToUpper();
		}
		
//		class PBXObject(PBXDict):
//    def __init__(self, d=None):
//        PBXDict.__init__(self, d)
//
//        if not self.has_key('isa'):
//            self['isa'] = self.__class__.__name__
//        self.id = None
//
//    @staticmethod
//    def Convert(o):
//        if isinstance(o, list):
//            return PBXList(o)
//        elif isinstance(o, dict):
//            isa = o.get('isa')
//
//            if not isa:
//                return PBXDict(o)
//
//            cls = globals().get(isa)
//
//            if cls and issubclass(cls, PBXObject):
//                return cls(o)
//
//            print 'warning: unknown PBX type: %s' % isa
//            return PBXDict(o)
//        else:
//            return o
	}
	
	public class PBXNativeTarget : PBXObject
	{
		public PBXNativeTarget() : base() {
		}
	}

	public class PBXContainerItemProxy : PBXObject
	{
		public PBXContainerItemProxy() : base() {
		}
	}

	public class PBXReferenceProxy : PBXObject
	{
		public PBXReferenceProxy() : base() {
		}
	}

	public class PBXVariantGroup : PBXObject
	{
		public PBXVariantGroup() : base() {
		}
	}
}
