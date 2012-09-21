using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXType : PBXDictionary
	{
		protected const string ISA_KEY = "isa";
		public string id;
		
		public PBXType()
		{
			if( !this.ContainsKey( ISA_KEY ) )
				this[ ISA_KEY ] = this.GetType().Name;
			
			this.id = null;
			Debug.Log( "chiamato" );
		}
		
		public static bool IsGuid( string aString )
		{
			return System.Text.RegularExpressions.Regex.IsMatch( aString, @"^[A-F0-9]{24}$" );
		}
		
		public static string GenerateId()
		{
			return System.Guid.NewGuid().ToString("N").Substring( 8 ).ToUpper();
		}
		
//		class PBXType(PBXDict):
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
//            if cls and issubclass(cls, PBXType):
//                return cls(o)
//
//            print 'warning: unknown PBX type: %s' % isa
//            return PBXDict(o)
//        else:
//            return o
	}
	
	public class PBXNativeTarget : PBXType
	{
		public PBXNativeTarget() : base() {
		}
	}

	public class PBXProject : PBXType
	{
		public PBXProject() : base() {
		}
	}

	public class PBXContainerItemProxy : PBXType
	{
		public PBXContainerItemProxy() : base() {
		}
	}

	public class PBXReferenceProxy : PBXType
	{
		public PBXReferenceProxy() : base() {
		}
	}

	public class PBXVariantGroup : PBXType
	{
		public PBXVariantGroup() : base() {
		}
	}
}
