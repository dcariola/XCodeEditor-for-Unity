using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.XCodeEditor
{
	public class XCProject : System.IDisposable
	{
		private XCFileOperationQueue _fileOperrationQueue;
		
//		private string _filePath;
		private PBXDictionary _datastore;
		private PBXDictionary _objects;
		private PBXDictionary _groups;
		private PBXDictionary _configurations;
		private PBXDictionary _rootObject;
		private PBXDictionary _rootGroup;
		private string _defaultConfigurationName;
		private string _rootObjectKey;
	
		public string filePath { get; private set; }
		private string sourcePathRoot;
		private bool modified = false;
		
		#region Constructor
		
		public XCProject()
		{
			
		}
		
		public XCProject( string filePath ) : this()
		{
			if( !System.IO.Directory.Exists( filePath ) ) {
				Debug.LogWarning( "Path does not exists." );
//				throw 
				return;
			}
			
			if( filePath.EndsWith( ".xcodeproj" ) ) {
				Debug.Log( "This is a project" );
				this.filePath = filePath;
			} else {
				Debug.Log( "Looking for xcodeproj files in the folder." );
				string[] projects = System.IO.Directory.GetDirectories( filePath, "*.xcodeproj" );
				if( projects.Length > 0 ) {
					Debug.Log( "Found: " + projects[ 0 ] );
					this.filePath = projects[ 0 ];
				}	
			}
			
			string projPath = System.IO.Path.Combine( this.filePath, "project.pbxproj" );
			string contents = System.IO.File.OpenText( projPath ).ReadToEnd();

			PBXParser parser = new PBXParser();
			_datastore = parser.Decode( contents );
			if( _datastore == null ) {
				throw new System.Exception( "Project file not found at file path " + filePath );
			}

			_fileOperrationQueue = new XCFileOperationQueue();
//			_groups = new Dictionary<string, object>();
			_objects = (PBXDictionary)_datastore["objects"];
			modified = false;
			
			_rootObjectKey = (string)_datastore["rootObject"];
			if( !string.IsNullOrEmpty( _rootObjectKey ) ) {
				_rootObject = (PBXDictionary)_objects[ _rootObjectKey ];
				_rootGroup = (PBXDictionary)_objects[ (string)_rootObject[ "mainGroup" ] ];
			}
			else {
				Debug.LogWarning( "error: project has no root object" );
				_rootObject = null;
				_rootGroup = null;
			}

		}
		
		#endregion
		#region Files


		/// <summary>
		/// Returns all file resources in the project, as an array of `XCSourceFile` objects.
		/// </summary>
		/// <returns>
		/// The files.
		/// </returns>
		public ArrayList GetFiles()
		{
			return null;
		}

		/// <summary>
		/// Returns the project file with the specified key, or nil.
		/// </summary>
		/// <returns>
		/// The file with key.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public XCSourceFile GetFileWithKey( string key )
		{
			return null;
		}
		
		/// <summary>
		/// Returns the project file with the specified name, or nil. If more than one project file matches the specified name,
		/// which one is returned is undefined.
		/// </summary>
		/// <returns>
		/// The file with name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		public XCSourceFile GetFileWithName( string name )
		{
			return null;
		}

		/// <summary>
		/// Returns all header files in the project, as an array of `XCSourceFile` objects.
		/// </summary>
		/// <returns>
		/// The header files.
		/// </returns>
		public ArrayList GetHeaderFiles()
		{
			return null;
		}

		/**
		* Returns all implementation obj-c implementation files in the project, as an array of `XCSourceFile` objects.
		*/
		public ArrayList GetObjectiveCFiles()
		{
			return null;
		}

		/**
		* Returns all implementation obj-c++ implementation files in the project, as an array of `XCSourceFile` objects.
		*/
		public ArrayList GetObjectiveCPlusPlusFiles()
		{
			return null;
		}

		/**
		* Returns all the xib files in the project, as an array of `XCSourceFile` objects.
		*/
		public ArrayList GetXibFiles()
		{
			return null;
			
		}

		public ArrayList getImagePNGFiles()
		{
			return null;
		}
		
		#endregion
		#region Groups
		/**
		* Lists the groups in an xcode project, returning an array of `PBXGroup` objects.
		*/
		public PBXList groups {
			get {
				return null;
			}
		}
		
		/**
		 * Returns the root (top-level) group.
		 */
		public PBXGroup rootGroup {
			get {
				return null;	
			}
		}
		
		/**
		 * Returns the root (top-level) groups, if there are multiple. An array of rootGroup if there is only one.
		 */
		public ArrayList rootGroups {
			get {
				return null;	
			}
		}
		
		/**
		* Returns the group with the given key, or nil.
		*/
		public PBXGroup GetGroupWithKey( string key )
		{
			return null;
		}
		
		/**
		 * Returns the group with the specified display name path - the directory relative to the root group. Eg Source/Main
		 */
		public PBXGroup GetGroupWithPathFromRoot( string path )
		{
			return null;
		}
		
		/**
		* Returns the parent group for the group or file with the given key;
		*/
		public PBXGroup GetGroupForGroupMemberWithKey( string key )
		{
			return null;
		}
		
		/**
		 * Returns the parent group for the group or file with the source file
		 */
		public PBXGroup GetGroupWithSourceFile( XCSourceFile sourceFile )
		{
			return null;
		}
		
		#endregion
		#region Target
		
		/**
		* Lists the targets in an xcode project, returning an array of `XCTarget` objects.
		*/
		public ArrayList targets {
			get {
				return null;
			}
		}
		
		/**
		* Returns the target with the specified name, or nil. 
		*/
		public XCTarget GetTargetWithName( string name )
		{
			return null;
		}
		
		#endregion
		#region Configurations
		
		/**
		* Returns the target with the specified name, or nil. 
		*/
		public Dictionary<string, string> configurations {
			get {
				return null;
			}
		}
			
		public Dictionary<string, string> GetConfigurationWithName( string name )
		{
			return null;
		}

		public XCBuildConfigurationList defaultConfiguration {
			get {
				return null;
			}
		}
		
		#endregion
		#region Savings
		/**
		* Saves a project after editing.
		*/
		public void Save()
		{
			
		}
		
		/**
		* Raw project data.
		*/
		public Dictionary<string, object> objects {
			get {
				return null;
			}
		}
		
		
		#endregion
		
		public void Dispose()
		{
			
		}
	}
}