using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXBuildFile : PBXObject
	{
		private const string SETTINGS_KEY = "settings";
		private const string ATTRIBUTES_KEY = "ATTRIBUTES";
		private const string WEAK_VALUE = "Weak";
		private const string COMPILER_FLAGS_KEY = "COMPILER_FLAGS";
		
		public PBXBuildFile( string fileRef, bool weak = false ) : base()
		{

//    def Create(cls, file_ref, weak=False):
//        if isinstance(file_ref, PBXFileReference):
//            file_ref = file_ref.id
//
//        bf = cls()
//        bf.id = cls.GenerateId()
//        bf['fileRef'] = file_ref
//
//        if weak:
//            bf.set_weak_link(True)
//
//        return bf
		}
		
		public PBXBuildFile( string guid, PBXDictionary dictionary ) : base ( guid, dictionary )
		{
//			Debug.Log( "constructor child" );
		}
		
		public bool SetWeakLink( bool weak = false )
		{
			PBXDictionary settings = _data[SETTINGS_KEY] as PBXDictionary;
			PBXList attributes = null;
			
			if( settings == null ) {
				if( weak ) {
					attributes = new PBXList();
					attributes.Add( WEAK_VALUE );
					
					settings = new PBXDictionary();
					settings.Add( ATTRIBUTES_KEY, attributes );
				}
				return true;
			}
			
			attributes = settings[ ATTRIBUTES_KEY ] as PBXList;
			if( attributes == null ) {
				if( weak ) {
					attributes = new PBXList();
				}
				else {
					return false;
				}
			}
			
			if( weak ) {
				attributes.Add( WEAK_VALUE );
			}
			else {
				attributes.Remove( WEAK_VALUE );
			}
			
			settings.Add( ATTRIBUTES_KEY, attributes );
			this.Add( SETTINGS_KEY, settings );
			
			return true;
		}
		
		public bool AddCompilerFlag( string flag )
		{
//			if( !this.ContainsKey( SETTINGS_KEY ) )
//				this[ SETTINGS_KEY ] = new PBXDictionary();
//			
//			if( !(PBXDictionary)this[ SETTINGS_KEY ]
			
			return false;
			
//		def add_compiler_flag(self, flag):
//        k_settings = 'settings'
//        k_attributes = 'COMPILER_FLAGS'
//
//        if not self.has_key(k_settings):
//            self[k_settings] = PBXDict()
//
//        if not self[k_settings].has_key(k_attributes):
//            self[k_settings][k_attributes] = flag
//            return True
//
//        flags = self[k_settings][k_attributes].split(' ')
//
//        if flag in flags:
//            return False
//
//        flags.append(flag)
//
//        self[k_settings][k_attributes] = ' '.join(flags)
		}
		
	}
}
