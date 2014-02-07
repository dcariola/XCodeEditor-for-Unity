using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Kabam.MiniJSON;

namespace UnityEditor.KabamXCodeEditor 
{
	public class XCMod 
	{
//		private string group;
		private List<string> _patches;
		private List<XCModFile> _libs;
		private List<string> _frameworks;
		private List<string> _headerpaths;
		private List<string> _files;
		private List<string> _folders;
		private List<string> _excludes;
		private Dictionary<string,object> _datastore;
		
		public string name { get; private set; }
		public string path { get; private set; }

		private List<string> convertList(object input) {
			if (input != null && input.GetType ().Equals (typeof(List<object>))) {
								return convertList ((List<object>)input);
			} else if (input != null && input.GetType ().Equals (typeof(List<string>))) {
					return (List<string>)input;
			} else {
				return new List<string> ();
			}
		}

		private List<string> convertList(List<object> input) {
			List<string> output = new List<string> ();
			if (input != null) {
				foreach (object obj in input) {
					output.Add (obj.ToString ());
				}
			}
			return output;
		}

		public string group {
			get {
				return (string)_datastore["group"];
			}
		}
		
		public List<string> patches {
			get {
				if( _patches == null) {
					_patches = convertList (_datastore["patches"]);
				}
				return _patches;
			}
		}
		
		public List<XCModFile> libs {
			get {
				if( _libs == null && _datastore["libs"] != null) {
					_libs = new List<XCModFile>();
					foreach( string fileRef in convertList (_datastore["libs"]) ) {
						_libs.Add( new XCModFile( fileRef ) );
					}
				}
				return _libs;
			}
		}
		
		public List<string> frameworks {
			get {
				if( _frameworks == null) {
					_frameworks = convertList (_datastore["frameworks"]);
				}
				return _frameworks;
			}
		}
		
		public List<string> headerpaths {
			get {
				if( _headerpaths == null) {
					_headerpaths = convertList (_datastore["headerpaths"]);
				}
				return _headerpaths;
			}
		}
		
		public List<string> files {
			get {
				if( _files == null) {
					_files = convertList (_datastore["files"]);
				}
				return _files;
			}
		}
		
		public List<string> folders {
			get {
				if( _folders == null) {
					_folders = convertList (_datastore["folders"]);
				}
				return _folders;
			}
		}
		
		public List<string> excludes {
			get {
				if( _excludes == null) {
					_excludes = convertList (_datastore["excludes"]);
				}
				return _excludes;
			}
		}
		
		public XCMod( string filename )
		{	
			FileInfo projectFileInfo = new FileInfo( filename );
			if( !projectFileInfo.Exists ) {
				Debug.LogWarning( "File does not exist." );
			}
			
			name = System.IO.Path.GetFileNameWithoutExtension( filename );
			path = System.IO.Path.GetDirectoryName( filename );
			
			string contents = projectFileInfo.OpenText().ReadToEnd();
			_datastore = (Dictionary<string, object>)Json.Deserialize (contents);
		}
		
			
//	"group": "GameCenter",
//	"patches": [],
//	"libs": [],
//	"frameworks": ["GameKit.framework"],
//	"headerpaths": ["Editor/iOS/GameCenter/**"],					
//	"files":   ["Editor/iOS/GameCenter/GameCenterBinding.m",
//				"Editor/iOS/GameCenter/GameCenterController.h",
//				"Editor/iOS/GameCenter/GameCenterController.mm",
//				"Editor/iOS/GameCenter/GameCenterManager.h",
//				"Editor/iOS/GameCenter/GameCenterManager.m"],
//	"folders": [],	
//	"excludes": ["^.*\\.meta$", "^.*\\.mdown^", "^.*\\.pdf$"]
		
	}
	
	public class XCModFile
	{
		public string filePath { get; private set; }
		public bool isWeak { get; private set; }
		
		public XCModFile( string inputString )
		{
			isWeak = false;
			
			if( inputString.Contains( ":" ) ) {
				string[] parts = inputString.Split( ':' );
				filePath = parts[0];
				isWeak = ( parts[1].CompareTo( "weak" ) == 0 );	
			}
			else {
				filePath = inputString;
			}
		}
	}
}
