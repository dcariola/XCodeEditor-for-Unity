using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UnityEditor.XCodeEditor
{
	public class XCodeEditorMenu
	{

		[MenuItem ("Build Tools/XCode Editor/DebugTest %t")]
		static void DebugTest()
		{
			string projectPath = Path.Combine( Directory.GetParent( Application.dataPath ).ToString(), "XCode" );
			Debug.Log( "XcodePath: " + projectPath );
			
			XCProject currentProject = new XCProject( projectPath );
		}
		
		
		[MenuItem ("Build Tools/XCode Editor/DebugTest2 %y")]
		static void DebugTest2()
		{
			string projectPath = Path.Combine( Directory.GetParent( Application.dataPath ).ToString(), "XCode" );
			
//			string[] files = System.IO.Directory.GetFiles( projectPath, "Info.plist" );
//			string contents = System.IO.File.OpenText( files[0] ).ReadToEnd();
			
			string[] projects = System.IO.Directory.GetDirectories( projectPath, "*.xcodeproj" );
			string projPath = System.IO.Path.Combine( projects[0], "project.pbxproj" );
			string contents = System.IO.File.OpenText( projPath ).ReadToEnd();
//			Debug.Log( System.IO.File.OpenText( projPath ).ReadToEnd );

			PBXParser parser = new PBXParser();
			Debug.Log( parser.Decode( contents ) );
		}
	}
}