using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class PBXFileReference : PBXType
	{
		public string buildPhase;
		public readonly Dictionary<string, string> types = new Dictionary<string, string> {
			{"a", "archive.ar" }
//			{".a", {"archive.ar", "PBXFrameworksBuildPhase"}},
//			{".app", {"wrapper.application", null }}
		};
//        '.a':('archive.ar', 'PBXFrameworksBuildPhase'),
//        '.app': ('wrapper.application', None),
//        '.s': ('sourcecode.asm', 'PBXSourcesBuildPhase'),
//        '.c': ('sourcecode.c.c', 'PBXSourcesBuildPhase'),
//        '.cpp': ('sourcecode.cpp.cpp', 'PBXSourcesBuildPhase'),
//        '.framework': ('wrapper.framework','PBXFrameworksBuildPhase'),
//        '.h': ('sourcecode.c.h', None),
//        '.icns': ('image.icns','PBXResourcesBuildPhase'),
//        '.m': ('sourcecode.c.objc', 'PBXSourcesBuildPhase'),
//        '.mm': ('sourcecode.cpp.objcpp', 'PBXSourcesBuildPhase'),
//        '.nib': ('wrapper.nib', 'PBXResourcesBuildPhase'),
//        '.plist': ('text.plist.xml', 'PBXResourcesBuildPhase'),
//        '.png': ('image.png', 'PBXResourcesBuildPhase'),
//        '.rtf': ('text.rtf', 'PBXResourcesBuildPhase'),
//        '.tiff': ('image.tiff', 'PBXResourcesBuildPhase'),
//        '.txt': ('text', 'PBXResourcesBuildPhase'),
//        '.xcodeproj': ('wrapper.pb-project', None),
//        '.xib': ('file.xib', 'PBXResourcesBuildPhase'),
//        '.strings': ('text.plist.strings', 'PBXResourcesBuildPhase'),
//        '.bundle': ('wrapper.plug-in', 'PBXResourcesBuildPhase'),
//        '.dylib': ('compiled.mach-o.dylib', 'PBXFrameworksBuildPhase')
//    }
		
		public PBXFileReference() : base()
		{
			
		}
		
//	class PBXFileReference(PBXType):
//    def __init__(self, d=None):
//        PBXType.__init__(self, d)
//        self.build_phase = None
//
//    types = {
//        '.a':('archive.ar', 'PBXFrameworksBuildPhase'),
//        '.app': ('wrapper.application', None),
//        '.s': ('sourcecode.asm', 'PBXSourcesBuildPhase'),
//        '.c': ('sourcecode.c.c', 'PBXSourcesBuildPhase'),
//        '.cpp': ('sourcecode.cpp.cpp', 'PBXSourcesBuildPhase'),
//        '.framework': ('wrapper.framework','PBXFrameworksBuildPhase'),
//        '.h': ('sourcecode.c.h', None),
//        '.icns': ('image.icns','PBXResourcesBuildPhase'),
//        '.m': ('sourcecode.c.objc', 'PBXSourcesBuildPhase'),
//        '.mm': ('sourcecode.cpp.objcpp', 'PBXSourcesBuildPhase'),
//        '.nib': ('wrapper.nib', 'PBXResourcesBuildPhase'),
//        '.plist': ('text.plist.xml', 'PBXResourcesBuildPhase'),
//        '.png': ('image.png', 'PBXResourcesBuildPhase'),
//        '.rtf': ('text.rtf', 'PBXResourcesBuildPhase'),
//        '.tiff': ('image.tiff', 'PBXResourcesBuildPhase'),
//        '.txt': ('text', 'PBXResourcesBuildPhase'),
//        '.xcodeproj': ('wrapper.pb-project', None),
//        '.xib': ('file.xib', 'PBXResourcesBuildPhase'),
//        '.strings': ('text.plist.strings', 'PBXResourcesBuildPhase'),
//        '.bundle': ('wrapper.plug-in', 'PBXResourcesBuildPhase'),
//        '.dylib': ('compiled.mach-o.dylib', 'PBXFrameworksBuildPhase')
//    }
//
//    trees = [
//        '<absolute>',
//        '<group>',
//        'BUILT_PRODUCTS_DIR',
//        'DEVELOPER_DIR',
//        'SDKROOT',
//        'SOURCE_ROOT',
//    ]
//
//    def guess_file_type(self):
//        self.remove('explicitFileType')
//        self.remove('lastKnownFileType')
//        ext = os.path.splitext(self.get('name', ''))[1]
//
//        f_type, build_phase = PBXFileReference.types.get(ext, ('?', None))
//
//        self['lastKnownFileType'] = f_type
//        self.build_phase = build_phase
//
//        if f_type == '?':
//            print 'unknown file extension: %s' % ext
//            print 'please add extension and Xcode type to PBXFileReference.types'
//
//        return f_type
//
//    def set_file_type(self, ft):
//        self.remove('explicitFileType')
//        self.remove('lastKnownFileType')
//
//        self['explicitFileType'] = ft
//
//    @classmethod
//    def Create(cls, os_path, tree='SOURCE_ROOT'):
//        if tree not in cls.trees:
//            print 'Not a valid sourceTree type: %s' % tree
//            return None
//
//        fr = cls()
//        fr.id = cls.GenerateId()
//        fr['path'] = os_path
//        fr['name'] = os.path.split(os_path)[1]
//        fr['sourceTree'] = '<absolute>' if os.path.isabs(os_path) else tree
//        fr.guess_file_type()
//
//        return fr
	}
}
