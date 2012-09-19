using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace UnityEditor.XCodeEditor
{
	public class PBXParser
	{
		public const string PBX_HEADER_TOKEN = "// !$*UTF8*$!\n";
		public const char WHITESPACE_SPACE = ' ';
		public const char WHITESPACE_TAB = '\t';
		public const char WHITESPACE_NEWLINE = '\n';
		public const char WHITESPACE_CARRIAGE_RETURN = '\r';
		public const char ARRAY_BEGIN_TOKEN = '(';
		public const char ARRAY_END_TOKEN = ')';
		public const char ARRAY_ITEM_DELIMITER_TOKEN = ',';
		public const char DICTIONARY_BEGIN_TOKEN = '{';
		public const char DICTIONARY_END_TOKEN = '}';
		public const char DICTIONARY_ASSIGN_TOKEN = '=';
		public const char DICTIONARY_ITEM_DELIMITER_TOKEN = ';';
		public const char QUOTEDSTRING_BEGIN_TOKEN = '"';
		public const char QUOTEDSTRING_END_TOKEN = '"';
		public const char QUOTEDSTRING_ESCAPE_TOKEN = '\\';
		public const char END_OF_FILE = (char)0x1A;
		public const string COMMENT_BEGIN_TOKEN = "/*";
		public const string COMMENT_END_TOKEN = "*/";
		public const string COMMENT_LINE_TOKEN = "//";
		private const int BUILDER_CAPACITY = 10000;

		//
		private char[] data;
		private int index;
//		public bool success;
		private int indent;
	
		public object Decode( string data )
		{
//			success = true;
			if( !data.StartsWith( PBX_HEADER_TOKEN ) ) {
				Debug.Log( "Wrong file format." );
				return null;
			}

			data = data.Substring( 13 );
			this.data = data.ToCharArray();
			return Parse();
		}

		public string Encode( Dictionary<string, object> pbxData, bool readable = false )
		{
			indent = 0;

			StringBuilder builder = new StringBuilder( PBX_HEADER_TOKEN, BUILDER_CAPACITY );
			bool success = SerializeValue( pbxData, builder, readable );

			return ( success ? builder.ToString() : null );
		}

		#region Move

		private char NextToken()
		{
			SkipWhitespaces();
			return StepForeward();
			
//		def next_token(self):
//      	self.eat_whitespace()
//
//      	return self.fh.read(1)
		}
		
		private string Peek( int step = 1 )
		{
			string sneak = string.Empty;
			for( int i = 1; i <= step; i++ ) {
				sneak += data[ index + i ];
			}
			return sneak;
			
//		def peek(self, chars=1):
//	        c = self.fh.read(chars)
//	        self.backup(chars)
//	        return c
		}

		private bool SkipWhitespaces()
		{
			bool whitespace = false;
			while( Regex.IsMatch( StepForeward().ToString(), @"\s" ) )
				whitespace = true;

			StepBackward();

			if( SkipComments() ) {
				whitespace = true;
				SkipWhitespaces();
			}

			return whitespace;

//		def eat_whitespace(self):
//      	whitespace = False
//      	regexp = re.compile('[\s]')
//
//      	while re.match(regexp, self.fh.read(1)):
//				whitespace = True
//
//      	self.backup()
//
//      	if self.eat_comments():
//          	whitespace = True
//          	self.eat_whitespace()
//
//      	return whitespace
		}

		private bool SkipComments()
		{
			string s = string.Empty;
			string tag = Peek( 2 );
			switch( tag ) {
				case COMMENT_BEGIN_TOKEN: {
						while( Peek( 2 ).CompareTo( COMMENT_END_TOKEN ) != 0 ) {
							s += StepForeward();
						}
						s += StepForeward( 2 );
//						Debug.Log( "Skipped comment: \"" + s + "\"" );
						break;
					}
				case COMMENT_LINE_TOKEN: {
						while( !Regex.IsMatch( StepForeward().ToString(), @"\n" ) )
							continue;

						break;
					}
				default:
					return false;
			}
			return true;
			
//		def eat_comments(self):
//      	comment = self.peek(2)
//
//      	if comment == '/*':
//          	while self.peek(2) != '*/':
//              	self.fh.read(1)
//          	self.fh.read(2)
//      	elif comment == '//':
//          	while not re.match('\n', self.fh.read(1)):
//              	pass
//      	else:
//          	return False
//
//      	return True
		}
		
		private char StepForeward( int step = 1 )
		{
//			index = Math.Min( data.lenght, index + step );
			index += step;
			return data[ index ];
			
//		def backup(self, chars=1):
//        	self.fh.seek(-chars, 1)
		}
		
		private char StepBackward( int step = 1 )
		{
			index = Math.Max( 0, index - step );
			return data[ index ];
			
//		def backup(self, chars=1):
//        	self.fh.seek(-chars, 1)
		}

		#endregion
		#region Parse

		public object Parse()
		{
			index = 0;
			try {
				return ParseValue();
			}
			catch( System.Exception ex ) {
//				Debug.Log( "[Index " + data[ index ] + "] " + ex.Message );
				Debug.LogWarning( ex.Message );
				return null;
//				throw new System.Exception( "Reached end of input unexpectedly at index " + index );
			}
		}

		private object ParseValue()
		{
			switch( NextToken() ) {
				case END_OF_FILE:
					Debug.Log( "End of file" );
					return null;
				case DICTIONARY_BEGIN_TOKEN:
					return ParseDictionary();
				case ARRAY_BEGIN_TOKEN:
					return ParseArray();
				case QUOTEDSTRING_BEGIN_TOKEN:
					return ParseString();
				default:
					StepBackward();
					return ParseEntity();
			}

//		def parse_value(self):
//      	# move through file until a token is hit
//
//        	token = self.next_token()
//
//        	if token is  None:
//          	print 'end of file'
//            	return None
//        	if token == '{':
//            	return self.parse_dictionary()
//        	elif token == '(':
//            	return self.parse_list()
//        	elif token == '"':
//            	return self.parse_string()
//        	else:
//            	self.backup()
//            	return self.parse_entity()
		}

		private object ParseDictionary()
		{
			SkipWhitespaces();
//			Hashtable dictionary = new Hashtable();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string keyString = string.Empty;
			object valueObject = null;

			bool complete = false;
			while( !complete ) {
				switch( NextToken() ) {
					case END_OF_FILE:
						Debug.Log( "Error: reached end of file inside a dictionary: " + index );
						complete = true;
						break;

					case DICTIONARY_ITEM_DELIMITER_TOKEN:
//						if( string.IsNullOrEmpty( keyString ) ) {
//							throw new System.Exception( "Missing key before assign token." );
//						}

						keyString = string.Empty;
						valueObject = null;
						break;

					case DICTIONARY_END_TOKEN:
//						if( !string.IsNullOrEmpty( keyString ) && valueObject != null ) {
//
//						}

						keyString = string.Empty;
						valueObject = null;
						complete = true;
						break;

					case DICTIONARY_ASSIGN_TOKEN:
						valueObject = ParseValue();
						dictionary.Add( keyString, valueObject );
						break;

					default:
						StepBackward();
						keyString = ParseValue() as string;
						break;
				}
			}
			return dictionary;

//		def parse_dictionary(self):
//      	self.eat_whitespace()
//        	d = {}
//
//        	while True:
//            	token = self.next_token()
//
//            	if token is None:
//                	print 'error: reached end of file inside a dictionary: %s' % str(d)
//                	return d
//            	elif token == ';':
//                	pass
//            	elif token == '}':
//                	return d
//            	else:
//                	self.backup()
//                	key = self.parse_value()
//
//                	if not key:
//                    	return d
//
//                	token = self.next_token()
//
//                	if token != '=':
//                    	print 'error: could not find value of key %s of dictionary %s' % (key, d)
//                    	return None
//
//                	d[key] = self.parse_value()
		}

		private ArrayList ParseArray()
		{
			ArrayList list = new ArrayList();
			bool complete = false;
			while( !complete ) {
				switch( NextToken() ) {
					case END_OF_FILE:
						Debug.Log( "Error: Reached end of file inside a list: " + list );
						complete = true;
						break;
					case ARRAY_END_TOKEN:
						complete = true;
						break;
					case ARRAY_ITEM_DELIMITER_TOKEN:
						break;
					default:
						StepBackward();
						list.Add( ParseValue() );
						break;
				}
			}
			return list;

//		def parse_list(self):
//        	l = []
//
//        	while True:
//            	token = self.next_token()
//
//            	if token is None:
//                	print 'error: reached end of file inside a list: %s' % l
//                	return l
//            	elif token == ',':
//                	pass
//            	elif token == ')':
//                	return l
//            	else:
//                	self.backup()
//                	l.append(self.parse_value())
		}

		private object ParseString()
		{
			string s = string.Empty;
			char c = StepForeward();
			while( c != QUOTEDSTRING_END_TOKEN ) {
				s += c;

				if( c == QUOTEDSTRING_ESCAPE_TOKEN )
					s += StepForeward();

				c = StepForeward();
			}

			return s;

//	 	def parse_string(self):
//        	chars = []
//
//        	c = self.fh.read(1)
//        	while c != '"':
//            	chars.append(c)
//
//            	if c == '\\':
//                	chars.append(self.fh.read(1))
//
//            	c = self.fh.read(1)
//
//        	return ''.join(chars)
		}

		private object ParseEntity()
		{
			string word = string.Empty;
			char c = StepForeward();

			while( !Regex.IsMatch( c.ToString(), @"[;,\s=]" ) ) {
				word += c;
				c = StepForeward();
			}

			if( word.Length != 24 && Regex.IsMatch( word, @"^\d+$" ) )
				return Int32.Parse( word );

			return word;

//		def parse_entity(self):
//        	chars = []
//        	regexp = re.compile('[;,\s=]')
//
//        	c = self.fh.read(1)
//        	while not re.match(regexp, c):
//            	chars.append(c)
//            	c = self.fh.read(1)
//
//        	word = ''.join(chars)
//        	if len(word) != 24 and re.match('^[0-9]+$', word):
//            	return int(word)
//
//        	return word
		}

		#endregion
		#region Serialize

		private bool SerializeValue( object value, StringBuilder builder, bool readable = false )
		{
			if( value == null ) {
				builder.Append( "null" );
			}
			else if( value is Dictionary<string, object> ) {
				SerializeDictionary( (Dictionary<string, object> )value, builder, readable );
			}
			else if( value.GetType().IsArray ) {
				SerializeArray( new ArrayList( (ICollection)value ), builder, readable );
			}
			else if( value is ArrayList ) {
				SerializeArray( (ArrayList)value, builder, readable );
			}
			else if( value is string ) {
				SerializeString( (string)value, builder, readable );
			}
			else if( value is Char ) {
				SerializeString( Convert.ToString( (char)value ), builder, readable );
			}
			else if( value.GetType().IsPrimitive ) {
//				SerializeNumber( Convert.ToDouble( value ), builder );
				builder.Append( Convert.ToString( value ) );
			}
//			else if( value is Hashtable )
//			{
//				serializeObject( (Hashtable)value, builder );
//			}
//			else if( ( value is Boolean ) && ( (Boolean)value == true ) )
//			{
//				builder.Append( "true" );
//			}
//			else if( ( value is Boolean ) && ( (Boolean)value == false ) )
//			{
//				builder.Append( "false" );
//			}
			else {
				return false;
			}
	
			return true;
		}

		private bool SerializeDictionary( Dictionary<string, object> dictionary, StringBuilder builder, bool readable = false )
		{

			builder.Append( DICTIONARY_BEGIN_TOKEN );

//			bool first = true;
			foreach( KeyValuePair<string, object> pair in dictionary ) {
//				if( !first )
//					builder.Append( DICTIONARY_ITEM_DELIMITER_TOKEN );

				SerializeString( pair.Key, builder );
				builder.Append( DICTIONARY_ASSIGN_TOKEN );
				SerializeValue( pair.Value, builder );
				builder.Append( DICTIONARY_ITEM_DELIMITER_TOKEN );
//				first = false;
			}

			builder.Append( DICTIONARY_END_TOKEN );
			return true;
		}

		private bool SerializeArray( ArrayList anArray, StringBuilder builder, bool readable = false )
		{
			builder.Append( ARRAY_BEGIN_TOKEN );
	
//			bool first = true;
			for( int i = 0; i < anArray.Count; i++ )
			{
				object value = anArray[i];
	
//				if( !first )
//				{
//					builder.Append( ARRAY_ITEM_DELIMITER_TOKEN );
//				}
	
				if( !SerializeValue( value, builder ) )
				{
					return false;
				}

				builder.Append( ARRAY_ITEM_DELIMITER_TOKEN );
//				first = false;
			}
	
			builder.Append( ARRAY_END_TOKEN );
			return true;
		}

		private bool SerializeString( string aString, StringBuilder builder, bool useQuotes = false, bool readable = false )
		{
			// Is a GUID?
			if( Regex.IsMatch( aString, @"^[A-F0-9]{24}$" ) ) {
				builder.Append( aString );
				return true;
			}

			// Is an empty string?
			if( string.IsNullOrEmpty( aString ) ) {
				builder.Append( QUOTEDSTRING_BEGIN_TOKEN );
				builder.Append( QUOTEDSTRING_END_TOKEN );
				return true;
			}

			if( !Regex.IsMatch( aString, @"^[A-Za-z0-9_.]+$" ) ) {
				useQuotes = true;
			}

			if( useQuotes )
				builder.Append( QUOTEDSTRING_BEGIN_TOKEN );

			builder.Append( aString );

//			char[] charArray = aString.ToCharArray();
//			for( int i = 0; i < charArray.Length; i++ )
//			{
//				char c = charArray[i];
//				if( c == '"' )
//				{
//					builder.Append( "\"" );
//				}
//				else if( c == '\\' )
//				{
//					builder.Append( "\\\\" );
//				}
//				else if( c == '\b' )
//				{
//					builder.Append( "\\b" );
//				}
//				else if( c == '\f' )
//				{
//					builder.Append( "\\f" );
//				}
//				else if( c == '\n' )
//				{
//					builder.Append( "\\n" );
//				}
//				else if( c == '\r' )
//				{
//					builder.Append( "\\r" );
//				}
//				else if( c == '\t' )
//				{
//					builder.Append( "\\t" );
//				}
//				else
//				{
//					int codepoint = Convert.ToInt32( c );
//					if( ( codepoint >= 32 ) && ( codepoint <= 126 ) )
//					{
//						builder.Append( c );
//					}
//					else
//					{
//						builder.Append( "\\u" + Convert.ToString( codepoint, 16 ).PadLeft( 4, '0' ) );
//					}
//				}
//			}

			if( useQuotes )
				builder.Append( QUOTEDSTRING_END_TOKEN );

			return true;
		}

//		private bool SerializeNumber( int number, StringBuilder builder )
//		{
//			builder.Append( Convert.ToString( number ) ); // , CultureInfo.InvariantCulture));
//			return true;
//		}

		#endregion
	}
}