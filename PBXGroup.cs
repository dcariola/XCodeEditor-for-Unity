using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXGroup : PBXType
	{
		protected const string NAME_KEY = "name";
		protected const string CHILDREN_KEY = "children";
		protected const string PATH_KEY = "path";
		protected const string SOURCETREE_KEY = "sourceTree";
		
		public PBXGroup( string name, string path = null, string tree = "SOURCE_ROOT" ) : base()
		{
			this.id = GenerateId();
			this.Add( NAME_KEY, name );
			this.Add( CHILDREN_KEY, new PBXList() );
			
			if( path != null ) {
				this.Add( PATH_KEY, path );
				this.Add( SOURCETREE_KEY, tree );
			}
			else {
				this.Add( SOURCETREE_KEY, "<group>" );
			}
		}
		
		public string AddChild( PBXType child )
		{
			if( !( child is PBXDictionary ) )
				return null;
			return "";
			
			string isa = (string)child[ ISA_KEY ];
			if( string.Compare( isa, "PBXFileReference" ) != 0 || string.Compare( isa, "PBXGroup" ) != 0 )
				return null;
			
			PBXList children;
			if( !ContainsKey( CHILDREN_KEY ) ) {
				children = new PBXList();
				this.Add( CHILDREN_KEY, children );
			}
			
//			((PBXList)this["children"]).Add( child.id );
			children.Add( child.id );
			return child.id;
		}
		
		public void RemoveChild( string id )
		{
			if( !ContainsKey( CHILDREN_KEY ) ) {
				this.Add( CHILDREN_KEY, new PBXList() );
				return;
			}
			
			if( !IsGuid( id ) )
				return;
			
			((PBXList)this[ CHILDREN_KEY ]).Remove( id );
		}
		
		public bool HasChild( string id )
		{
			if( !ContainsKey( CHILDREN_KEY ) ) {
				this.Add( CHILDREN_KEY, new PBXList() );
				return false;
			}
			
			if( !IsGuid( id ) )
				return false;
			
			return ((PBXList)this[ CHILDREN_KEY ]).Contains( id );
		}
		
		public string GetName()
		{
			return (string)this[ NAME_KEY ];
		}
		
//	class PBXGroup(PBXType):
//    def add_child(self, ref):
//        if not isinstance(ref, PBXDict):
//            return None
//
//        isa = ref.get('isa')
//
//        if isa != 'PBXFileReference' and isa != 'PBXGroup':
//            return None
//
//        if not self.has_key('children'):
//            self['children'] = PBXList()
//
//        self['children'].add(ref.id)
//
//        return ref.id
//
//    def remove_child(self, id):
//        if not self.has_key('children'):
//            self['children'] = PBXList()
//            return
//
//        if not PBXType.IsGuid(id):
//            id = id.id
//
//        self['children'].remove(id)
//
//    def has_child(self, id):
//        if not self.has_key('children'):
//            self['children'] = PBXList()
//            return False
//
//        if not PBXType.IsGuid(id):
//            id = id.id
//
//        return id in self['children']
//
//    def get_name(self):
//        path_name = os.path.split(self.get('path',''))[1]
//        return self.get('name', path_name)
//
//    @classmethod
//    def Create(cls, name, path=None, tree='SOURCE_ROOT'):
//        grp = cls()
//        grp.id = cls.GenerateId()
//        grp['name'] = name
//        grp['children'] = PBXList()
//
//        if path:
//            grp['path'] = path
//            grp['sourceTree'] = tree
//        else:
//            grp['sourceTree'] = '<group>'
//
//        return grp
	}
}
