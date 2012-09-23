using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor
{
	public class XCodeEditorMenu
	{

		[MenuItem ("Build Tools/XCode Editor/DebugTest %t")]
		static void DebugTest()
		{
			string projectPath = Path.Combine( Directory.GetParent( Application.dataPath ).ToString(), "XCode" );
//			Debug.Log( "XcodePath: " + projectPath );
			
			XCProject currentProject = new XCProject( projectPath );
			//Debug.Log(
//			PBXDictionary test = new PBXDictionary();
//			bool result = false;
//			if( test is Dictionary<string, object> )
//				result = true;
//			
//			Debug.Log( result );
			
//			PBXType type = new PBXType();
//			Debug.Log( "TYPE: " + type["isa"] );
//			
//			PBXBuildFile build = new PBXBuildFile( "" );
//			Debug.Log( "BUILDFILE: " + build["isa"] );
			
//			Debug.Log( PBXObject.GenerateGuid().ToUpper() );
//			PBXList testList = currentProject.GetObjectOfType( "XCBuildConfiguration" );
//			Debug.Log( testList.Count );
//			Debug.Log( currentProject.rootGroup.guid + " " + currentProject.rootGroup.name + " " + currentProject.rootGroup.path);
//			string path1 = "Data/mainData";
			string path2 = "/Users/Elyn/Projects/UnityPlugins/Modules/GameCenter/Editor/iOS/GameCenterBinding.m";
			currentProject.AddFile( path2 );
			
//			Debug.Log( "Files: " + currentProject.buildFiles.Count );
			
			
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
//			Hashtable test = (Hashtable)parser.Decode( contents );
			PBXDictionary test = parser.Decode( contents );
//			Debug.Log( MiniJSON.jsonEncode( test ) );
//			Debug.Log( test + " - " + test.Count );
//			Debug.Log( parser.Encode( test ) );
			
			
		}

	}
}