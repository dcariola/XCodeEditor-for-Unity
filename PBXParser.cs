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

		//
		private char[] data;
		private int index;
		private bool success;
	
		public object Decode( string data )
		{
			success = true;
			if( !data.StartsWith( "// !$*UTF8*$!" ) ) {
				Debug.Log( "Wrong file format." );
				return null;
			}

			data = data.Substring(13);
			char[] charToTrim = { WHITESPACE_TAB, WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN };
			data = data.Trim( charToTrim );

			this.data = data.ToCharArray();
			Debug.Log( this.data );
			return Parse();
			//return null;
		}
	
		private char NextToken()
		{
//	def next_token(self):
//        self.eat_whitespace()
//
//        return self.fh.read(1)
			index++;
			this.SkipWhitespaces();
			this.SkipComments();
			return data[index];
		}
	

	
		private void Backup( int chars = 1 )
		{
//	def backup(self, chars=1):
//        self.fh.seek(-chars, 1)		
		}

		// ALTRO METODO

		private bool Accept( char[] acceptableSymbols )
		{
			bool symbolPresent = false;
			foreach( char c in acceptableSymbols ) {
				if( data[index] == c )
					symbolPresent = true;
			}
			return symbolPresent;
	    }

		private bool Accept( char acceptableSymbol )
		{
			return data[index] == acceptableSymbol;
		}

		private void Expect( char[] expectedSymbols )
		{
			if( !Accept( expectedSymbols ) ) {
				string excString = "Expected " + expectedSymbols[0] + "'";
				foreach( char c in expectedSymbols )
					excString += "'" + c + "' ";

				excString += " but found '" + (char)data[index] + "' at index " + index;
	            throw new System.Exception( excString );
	        }
	    }

		private void Expect( char expectedSymbol )
		{
			if( !Accept( expectedSymbol ) )
				throw new System.Exception( "Expected '" + expectedSymbol + "' but found '" + (char)data[index] + "' at index " + index );
		}

		private void Read( char symbol )
		{
			Expect( symbol );
			index++;
		}

		#region Skip

		private void Skip( int step = 1)
		{
     	   index += step;
 		}

		private void SkipWhitespaces()
		{
			Debug.Log( "Skip whitespace" );
			char[] whitespaces = { WHITESPACE_CARRIAGE_RETURN, WHITESPACE_NEWLINE, WHITESPACE_SPACE, WHITESPACE_TAB };
			while( Accept( whitespaces ) )
				Skip();
		}

		private bool SkipComments()
		{
			Debug.Log( "skip comment" );

			string s = string.Empty;
			if( ( data[index] == '/' ) && (data[index+1] == '*' ) ) {
				Skip( 2 );
				while( data[index] != '*' && data[index+1] != '/' ) {
					s += (char)data[index];
					Skip();
				}
				Debug.Log( "comment end: " + s );
				Skip( 2 );
			}
			else if ( data[index] == '/' && data[index+1] == '/' ) {
				Skip( 2 );
				while( Accept( new char[] { WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN } ) ) {
					Skip();
				}
			}
			else {
				return false;
			}

			SkipWhitespaces();
			return true;
//	def eat_comments(self):
//        comment = self.peek(2)
//
//        if comment == '/*':
//            while self.peek(2) != '*/':
//                self.fh.read(1)
//            self.fh.read(2)
//        elif comment == '//':
//            while not re.match('\n', self.fh.read(1)):
//                pass
//        else:
//            return False
//
//        return True
		}

		#endregion

		private char Peek( int chars = 1 )
		{
//	def peek(self, chars=1):
//        c = self.fh.read(chars)
//        self.backup(chars)
//        return c
			char c = ' '; //READ

			return c;
		}

		private string ReadInputUntil( char[] symbols )
		{
			string s = string.Empty;
			while( !Accept( symbols ) ) {
				s += (char)data[index];
				Skip();
			}
			return s;
		}

		private string ReadInputUntil( char symbol )
		{
			string s = string.Empty;
			while( !Accept( symbol ) ) {
				s += (char)data[index];
				Skip();
			}
			return s;
		}

		#region Parse

		public object Parse()
		{
			index = 0;
			SkipWhitespaces();
			Expect( new char[] { DICTIONARY_BEGIN_TOKEN, ARRAY_BEGIN_TOKEN } );
			try {
				return ParseObject();
			}
			catch( System.Exception ex ) {
//				ex.printStackTrace();
				Debug.Log( "Index " + data[index] );
				throw new System.Exception( "Reached end of input unexpectedly at index " + index );
			}
		}

		private object ParseObject()
		{
			Debug.Log( "parse object" );
			switch( data[index] ) {
				case ARRAY_BEGIN_TOKEN: {
					return ParseArray();
				}
				case DICTIONARY_BEGIN_TOKEN: {
					return ParseDictionary();
				}
//				case DATA_BEGIN_TOKEN : {
//					return ParseData();
//				}
//				case QUOTEDSTRING_BEGIN_TOKEN : {
//					String quotedString = parseQuotedString();
//					//apple dates are quoted strings of length 20 and after the 4 year digits a dash is found
//					if(quotedString.length()==20 && quotedString.charAt(4)==DATE_DATE_FIELD_DELIMITER) {
//						try {
//                        	NSDate date = new NSDate(quotedString);
//                        	return date;
//                    	} catch(Exception ex) {
//                        	//not a date? --> return string
//                        	return new NSString(quotedString);
//                    	}
//                	} else {
//                    	return new NSString(quotedString);
//                	}
//            	}
            	default : {
					Debug.Log( "default" );
					return ParseEntity();

                	//0-9
//                	if( data[index] > 0x2F && data[index] < 0x3A ) {
//                    	//int, real or date
//                    	return ParseNumerical();
//                	} else {
//                    	//non-numerical -> string or boolean
//						return ParseString();

//                    	string parsedString = ParseString();
//
//                    	if( parsedString.Equals( "YES" ) ) {
//                        	return new  NSNumber(true);
//                    	} else if(parsedString.equals("NO")) {
//                        	return new NSNumber(false);
//                    	} else {
//                        	return new NSString(parsedString);
//                    	}
//                	}
            	}
        	}
    	}

		private object ParseNumerical()
		{
			string n = string.Empty;
			n = ReadInputUntil( new char[] { WHITESPACE_SPACE, WHITESPACE_TAB, WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN,
                ARRAY_ITEM_DELIMITER_TOKEN, DICTIONARY_ITEM_DELIMITER_TOKEN, DICTIONARY_ASSIGN_TOKEN, ARRAY_END_TOKEN } );
			return Int32.Parse( n );
		}


		private ArrayList ParseArray()
		{
			Debug.Log( "parse array" );
			//Skip begin token
	        Skip();
	        SkipWhitespaces();
	        ArrayList objects = new ArrayList();
	        while( !Accept( ARRAY_END_TOKEN ) ) {
	            objects.Add( ParseObject() );
	            SkipWhitespaces();
	            if( Accept( ARRAY_ITEM_DELIMITER_TOKEN ) ) {
	                Skip();
	            } else {
	                break; //must have reached end of array
	            }
	            SkipWhitespaces();
	        }
	        //parse end token
	        Read( ARRAY_END_TOKEN );
	        return objects;
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
					return ParseEntity();
			}

//		def parse_value(self):
//        # move through file until a token is hit
//
//        token = self.next_token()
//
//        if token is  None:
//            print 'end of file'
//            return None
//        if token == '{':
//            return self.parse_dictionary()
//        elif token == '(':
//            return self.parse_list()
//        elif token == '"':
//            return self.parse_string()
//        else:
//            self.backup()
//            return self.parse_entity()
		}
	
		private object ParseList()
		{
			return null;
//		def parse_list(self):
//        l = []
//
//        while True:
//            token = self.next_token()
//
//            if token is None:
//                print 'error: reached end of file inside a list: %s' % l
//                return l
//            elif token == ',':
//                pass
//            elif token == ')':
//                return l
//            else:
//                self.backup()
//                l.append(self.parse_value())
		}

		private object ParseDictionary()
		{
			Debug.Log( "Parse dictionary" );
			Hashtable dictionary = new Hashtable();
			string keyString = string.Empty;
			object valueObject = null;

			 //Skip begin token


//			Skip();
//        	SkipWhitespaces();

	        while( !Accept( DICTIONARY_END_TOKEN ) ) {
//				SkipWhitespaces();
				Debug.Log( "iniziato" );
				switch( NextToken() ) {
					case DICTIONARY_ASSIGN_TOKEN: {
						if( string.IsNullOrEmpty( keyString ) ) {
							throw new System.Exception( "Unexpected " + DICTIONARY_ASSIGN_TOKEN + " token. Expected key before assign token." );
						}
						else if( valueObject != null ) {
							throw new System.Exception( "An object is already set for key " + keyString + "." );
						}
						break;
					}
					case DICTIONARY_ITEM_DELIMITER_TOKEN: {
						if( string.IsNullOrEmpty( keyString ) ) {
							throw new System.Exception( "Missing key before assign token." );
						}
						break;
					}
					case DICTIONARY_END_TOKEN: {
						dictionary.Add( keyString, valueObject );
						break;
					}
					default: {
						if( string.IsNullOrEmpty( keyString ) ) {
							keyString = ParseValue() as string;
						}
						else {
							valueObject = ParseValue();
						}
						break;
					}
				}
			}
			return dictionary;



//				NextToken();
//
//				//Parse key
//	            string keyString = (string)ParseString();
//
//				NextToken();
//	            //Parse assign token
//	            Expect( DICTIONARY_ASSIGN_TOKEN );
////	            SkipWhitespaces();
//				NextToken();
//
//	            object obj = ParseObject();
//	            dict.Add(keyString, obj);
//
//	            Read(DICTIONARY_ITEM_DELIMITER_TOKEN);
//	            SkipWhitespaces();
//	        }
//	        //skip end token
//	        Skip();
//        	return dict;

//	def parse_dictionary(self):
//        self.eat_whitespace()
//        d = {}
//
//        while True:
//            token = self.next_token()
//
//            if token is None:
//                print 'error: reached end of file inside a dictionary: %s' % str(d)
//                return d
//            elif token == ';':
//                pass
//            elif token == '}':
//                return d
//            else:
//                self.backup()
//                key = self.parse_value()
//
//                if not key:
//                    return d
//
//                token = self.next_token()
//
//                if token != '=':
//                    print 'error: could not find value of key %s of dictionary %s' % (key, d)
//                    return None
//
//                d[key] = self.parse_value()
		}

		private object ParseString()
		{
			Debug.Log( "Parse string" );

			string s = string.Empty;

			s = ReadInputUntil( new char[] { WHITESPACE_SPACE, WHITESPACE_TAB, WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN,
                ARRAY_ITEM_DELIMITER_TOKEN, DICTIONARY_ITEM_DELIMITER_TOKEN, DICTIONARY_ASSIGN_TOKEN, ARRAY_END_TOKEN } );
			Debug.Log( s);

			return s;
//	 def parse_string(self):
//        chars = []
//
//        c = self.fh.read(1)
//        while c != '"':
//            chars.append(c)
//
//            if c == '\\':
//                chars.append(self.fh.read(1))
//
//            c = self.fh.read(1)
//
//        return ''.join(chars)
		}

		private object ParseEntity()
		{
			Debug.Log( "Parse entity" );
			string word = string.Empty;
			word = ReadInputUntil( new char[] { WHITESPACE_SPACE, WHITESPACE_TAB, WHITESPACE_NEWLINE, WHITESPACE_CARRIAGE_RETURN,
                ARRAY_ITEM_DELIMITER_TOKEN, DICTIONARY_ITEM_DELIMITER_TOKEN, DICTIONARY_ASSIGN_TOKEN, ARRAY_END_TOKEN } );
			Debug.Log ("Entity: " + word );

			if( word.Length != 24 && Regex.IsMatch(word, @"^\d+$") )
				return Int32.Parse( word );

			return word;
//	def parse_entity(self):
//        chars = []
//        regexp = re.compile('[;,\s=]')
//
//        c = self.fh.read(1)
//        while not re.match(regexp, c):
//            chars.append(c)
//            c = self.fh.read(1)
//
//        word = ''.join(chars)
//        if len(word) != 24 and re.match('^[0-9]+$', word):
//            return int(word)
//
//        return word
		}

		#endregion

	}
}