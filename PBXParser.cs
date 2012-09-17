using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityEditor.XCodeEditor
{
	public class PBXParser
	{
	
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
		public const char DATA_BEGIN_TOKEN = '<';
		public const char DATA_END_TOKEN = '>';
		public const char DATA_GSOBJECT_BEGIN_TOKEN = '*';
		public const char DATA_GSDATE_BEGIN_TOKEN = 'D';
		public const char DATA_GSBOOL_BEGIN_TOKEN = 'B';
		public const char DATA_GSBOOL_TRUE_TOKEN = 'Y';
		public const char DATA_GSBOOL_FALSE_TOKEN = 'N';
		public const char DATA_GSINT_BEGIN_TOKEN = 'I';
		public const char DATA_GSREAL_BEGIN_TOKEN = 'R';
		public const char DATE_DATE_FIELD_DELIMITER = '-';
		public const char DATE_TIME_FIELD_DELIMITER = ':';
		public const char DATE_GS_DATE_TIME_DELIMITER = ' ';
		public const char DATE_APPLE_DATE_TIME_DELIMITER = 'T';
		public const char DATE_APPLE_END_TOKEN = 'Z';
		public const char END_OF_FILE = (char)0x1A;
		public const string COMMENT_BEGIN_TOKEN = "/*";
		public const string COMMENT_END_TOKEN = "*/";
		public const string COMMENT_LINE_TOKEN = "//";

		//
		private char[] data;
		private int index;
//		private bool success;
		private int indent = 0;
	
		public object Decode( string data )
		{
//			success = true;
			if( !data.StartsWith( "// !$*UTF8*$!" ) ) {
				Debug.Log( "Wrong file format." );
				return null;
			}

			data = data.Substring( 13 );
//			char[] charToTrim = { WHITESPACE_TAB, WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN };
//			data = data.Trim( charToTrim );

			this.data = data.ToCharArray();
			Debug.Log( this.data );
			return Parse();
			//return null;
		}
	



		// ALTRO METODO
//		#region Read
//
//		private bool Accept( char[] acceptableSymbols )
//		{
//			bool symbolPresent = false;
//			foreach( char c in acceptableSymbols ) {
//				if( data[index] == c )
//					symbolPresent = true;
//			}
//			return symbolPresent;
//	    }
//
//		private bool Accept( char acceptableSymbol )
//		{
//			return data[index] == acceptableSymbol;
//		}
//
//		private void Expect( char[] expectedSymbols )
//		{
//			if( !Accept( expectedSymbols ) ) {
//				string excString = "Expected " + expectedSymbols[0] + "'";
//				foreach( char c in expectedSymbols )
//					excString += "'" + c + "' ";
//
//				excString += " but found '" + (char)data[index] + "' at index " + index;
//	            throw new System.Exception( excString );
//	        }
//	    }
//
//		private void Expect( char expectedSymbol )
//		{
//			if( !Accept( expectedSymbol ) )
//				throw new System.Exception( "Expected '" + expectedSymbol + "' but found '" + (char)data[index] + "' at index " + index );
//		}
//
//		private void Read( char symbol )
//		{
//			Expect( symbol );
//			index++;
//		}
//
//		private string ReadInputUntil( char[] symbols )
//		{
//			string s = string.Empty;
//			while( !Accept( symbols ) ) {
//				s += (char)data[index];
//				Skip();
//			}
//			index--;
//			return s;
//		}
//
//		private string ReadInputUntil( char symbol )
//		{
//			string s = string.Empty;
//			while( !Accept( symbol ) ) {
//				s += (char)data[index];
//				Skip();
//			}
//			index--;
//			return s;
//		}
//
//		#endregion
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
//			Debug.Log( "Skip whitespace start: " + index );
			
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
//			Debug.Log( "Skip comment" );

			string s = string.Empty;
			string tag = Peek( 2 );
			switch( tag ) {
				case COMMENT_BEGIN_TOKEN: {
						while( Peek( 2 ).CompareTo( COMMENT_END_TOKEN ) != 0 ) {
							s += StepForeward();
						}
						s += StepForeward( 2 );
						Debug.Log( "Skipped comment: \"" + s + "\"" );
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
			Debug.Log( "Parse value" );

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

//		private object ParseObject()
//		{
//			Debug.Log( "Parse object" );
//			switch( NextToken() ) {
//				case ARRAY_BEGIN_TOKEN: {
//					return ParseArray();
//				}
//				case DICTIONARY_BEGIN_TOKEN: {
//					return ParseDictionary();
//				}
////				case DATA_BEGIN_TOKEN : {
////					return ParseData();
////				}
////				case QUOTEDSTRING_BEGIN_TOKEN : {
////					String quotedString = parseQuotedString();
////					//apple dates are quoted strings of length 20 and after the 4 year digits a dash is found
////					if(quotedString.length()==20 && quotedString.charAt(4)==DATE_DATE_FIELD_DELIMITER) {
////						try {
////                        	NSDate date = new NSDate(quotedString);
////                        	return date;
////                    	} catch(Exception ex) {
////                        	//not a date? --> return string
////                        	return new NSString(quotedString);
////                    	}
////                	} else {
////                    	return new NSString(quotedString);
////                	}
////            	}
//            	default : {
//					Debug.Log( "default" );
//					return ParseEntity();
//
//                	//0-9
////                	if( data[index] > 0x2F && data[index] < 0x3A ) {
////                    	//int, real or date
////                    	return ParseNumerical();
////                	} else {
////                    	//non-numerical -> string or boolean
////						return ParseString();
//
////                    	string parsedString = ParseString();
////
////                    	if( parsedString.Equals( "YES" ) ) {
////                        	return new  NSNumber(true);
////                    	} else if(parsedString.equals("NO")) {
////                        	return new NSNumber(false);
////                    	} else {
////                        	return new NSString(parsedString);
////                    	}
////                	}
//            	}
//        	}
//    	}

		private object ParseDictionary()
		{
			Debug.Log( "Parse dictionary" );
			
			SkipWhitespaces();
			Hashtable dictionary = new Hashtable();
			string keyString = string.Empty;
			object valueObject = null;
//			KeyValuePair<string, object> entry = new KeyValuePair<string, object>();
			
//			indent += 1;
//			Debug.Log( "iniziato " + indent );
			bool complete = false;
			while( !complete ) {

				switch( NextToken() ) {
					case END_OF_FILE:
						Debug.Log( "Error: reached end of file inside a dictionary: " + index );
						break;

					case DICTIONARY_ASSIGN_TOKEN: {
						Debug.Log( "Parse dictionary assign: " + keyString + " - " + valueObject );
						if( string.IsNullOrEmpty( keyString ) ) {
							throw new System.Exception( "Unexpected " + DICTIONARY_ASSIGN_TOKEN + " token. Expected ke before assign token." );
						} else
						if( valueObject != null ) {
								throw new System.Exception( "An object is already set for key " + keyString + "." );
							}
						Debug.Log( "completa?" );
						break;
					}
					case DICTIONARY_ITEM_DELIMITER_TOKEN: {
						Debug.Log( "Parse dictionary delimeter" );
						if( string.IsNullOrEmpty( keyString ) ) {
							throw new System.Exception( "Missing key before assign token." );
						}
						dictionary.Add( keyString, valueObject );
//						keyString = string.Empty;
//						valueObject = null;
						break;
					}
					case DICTIONARY_END_TOKEN: {
						Debug.Log( "Parse dictionary end" );
						if( !string.IsNullOrEmpty( keyString ) && valueObject != null ) {
							dictionary.Add( keyString, valueObject );
//							keyString = string.Empty;
//							valueObject = null;
						}

						Debug.Log( "Chiuso " + indent );
						indent -= 1;
						complete = true;
						break;
					}
					default: {
						Debug.Log( "Parse dictionary default" );
						StepBackward();
						if( string.IsNullOrEmpty( keyString ) ) {
							keyString = ParseValue() as string;
						} else {
							valueObject = ParseValue();
							if( valueObject == null )
								Debug.Log( "VALUE: Qualcosa non va!" );
							else
								Debug.Log( "VALUE: " + valueObject.ToString() );
						}
						break;
					}
				}
			}
			Debug.Log( "Parse dictionary completo" );
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
			Debug.Log( "Parse array" );

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
			Debug.Log( "Parse string" );

			string s = string.Empty;
			char c = StepForeward();
			while( c != QUOTEDSTRING_END_TOKEN ) {
				s += c;

				if( c == QUOTEDSTRING_ESCAPE_TOKEN )
					s += StepForeward();

				c = StepForeward();
			}

			Debug.Log( s );
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
			Debug.Log( "Parse entity" );

			string word = string.Empty;
			char c = StepForeward();

			while( !Regex.IsMatch( c.ToString(), @"[;,\s=]" ) ) {
				word += c;
				c = StepForeward();
			}

			if( word.Length != 24 && Regex.IsMatch( word, @"^\d+$" ) ) {
				Debug.Log( "Found number: " + word );
				return Int32.Parse( word );
			}

			Debug.Log( "Found word: " + word );
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

		public void test()
		{
			Debug.Log( "TEST: " + index + ", " + data[ index ] + " - " + NextToken() + ", " + index );
		}

	}
}