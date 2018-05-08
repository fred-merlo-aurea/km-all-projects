#region Used Files
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
#endregion

namespace ActiveUp.WebControls
{
	#region SpellChecker
	/// <summary>
	/// Represents a <see cref="SpellChecker"/> object.
	/// </summary>
	[ToolboxBitmap(typeof(SpellChecker), "ToolBoxBitmap.Spellchecker.bmp")]
	public class SpellChecker : Component
	{
		#region Members
#if (LOG)
		private bool logFileClosed = true;
#endif 
		private bool newWord = true ;
		//use this bool to ignore word
		private bool ignoreThisWord = false;
		//saving all possible variants of beginnig word to this string
		private string possibleVarriants = string.Empty;
		//current text
		private string currentText = string.Empty;
		//string to make changes , saving first two letters etc
		private string workString = string.Empty;
		//string with path to dictioary
		private string dictionaryFile = "englishwords.txt";
		//string with log file name
#if (LOG)
		private string logFileName = "log.txt";
#endif
				
		//statistic
		private int wordsCount = 0;
		private int badWordsCount = 0;
		
		//beginning of thw word
		private int beg = 0;
		//shifting of the words when make changing
		private int shifting = 0;

		//saving all words from dict to this hashtable
		private Hashtable allWords = new Hashtable();
		//word collection of the current text
		private ArrayList wordsCollection = new ArrayList();
		//regex to check words if it ignore
		private Regex regx;
#if (LOG)
		//stream reader to read files
		private StreamWriter logFileWriter ;
#endif		
		private ArrayList correctWords = new ArrayList();
		
		private string incorrectWord;
		#endregion
		
		#region Properties
				/// <summary>
		/// Gets or sets the incorrect word.
		/// </summary>
		/// <value>The incorrect word.</value>
		public string IncorrectWordValue
		{
			get{ return incorrectWord; }
			set{ incorrectWord = value; }
		}
		/// <summary>
		/// Gets or sets the correct words.
		/// </summary>
		/// <value>The correct words.</value>
		public ArrayList CorrectWords
		{
			get{ return correctWords; }
			set{ correctWords = value; }
		}
		/// <summary>
		/// Enum of supported languges
		/// </summary>
		public enum DictLanguges:int
		{
			/// <summary>
			/// English language.
			/// </summary>
			EnglishLang = 1,
			/// <summary>
			/// Spanish language.
			/// </summary>
			SpanishLang,
			/// <summary>
			/// French language.
			/// </summary>
			FrenchLang,
			/// <summary>
			/// German language.
			/// </summary>
			GermanLang,
			/// <summary>
			/// Dutch language.
			/// </summary>
			DutchLang
		}
		/// <summary>
		/// Gets or sets the name of the log file.
		/// </summary>
		/// <value>The name of the log file.</value>		
		[ Browsable ( true ) ]
		public DictLanguges WorkLanguge = DictLanguges.EnglishLang ;
		
#if (LOG)
		public string LogFileName
		{
			get{ return logFileName; }
			set{ logFileName = value; }
		}
#endif

		/// <summary>
		/// Gets or sets the dictionary path.
		/// </summary>
		/// <value>The dictionary path.</value>
		public string DictionaryFile
		{
			get{ return dictionaryFile; }
			set{ dictionaryFile = value; }
		}
		/// <summary>
		/// get collection of the words in text
		/// </summary>
		[ Browsable ( false ) ]
		public ArrayList WordsCollection
		{
			get{ return wordsCollection; }
			set{ wordsCollection = value; }
		}
		/// <summary>
		/// Gets or sets the current stream reader.
		/// </summary>
		/// <value>The current stream reader.</value>
		public string CurrentText
		{
			get{ return currentText; }
			set{ currentText = value; }
		}
		#endregion
		
		#region Events
		/// <summary>
		/// Incorrect word event.
		/// </summary>
		public event IncorrectWordEventHandler IncorrectWord;
		/// <summary>
		/// Dictionary initialization event.
		/// </summary>
        public event DictInitEventHandler DictInitialization;
		/// <summary>
		/// Dictionary change event.
		/// </summary>
		public event DictionaryChangedEventHandler DictionaryChange;
		/// <summary>
		/// Complete event.
		/// </summary>
		public event CompleteEventHandler Complete;
		/// <summary>
		/// Replace word event.
		/// </summary>
		public event ReplaceEventHandler ReplaceWordEvent;
		/// <summary>
		/// Correct word event.
		/// </summary>
		public event CorrectWordEventHandler CorrectWord;
		/// <summary>
		/// Start check event.
		/// </summary>
		public event StartCheckEventHandler StartCheck;
		#endregion
		
       	#region Component Initialising
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="SpellChecker"/> class.
		/// </summary>
		public SpellChecker()
		{
			InitializeComponent();
			
		}
		#endregion
    
		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion
    
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
    			
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Method used to set path to other dictionary to use
		/// </summary>
		/// <param name="pathToNewDict"></param>
		public void ChangeDictionary ( string pathToNewDict )
		{
#if (LOG)
			if ( this.logFileClosed)
			{
				this.logFileWriter = new StreamWriter ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName ,true );
				this.logFileWriter.Write ( "Log started at " + DateTime.Now + " . ");
				this.logFileClosed = false;
			}
			
			this.logFileWriter.WriteLine ( "Dictionary was changed to  "  + pathToNewDict );
#endif
			this.dictionaryFile = pathToNewDict;
			this.allWords = new Hashtable();
			GetAllWords();
			this.RaiseDictChange( EventArgs.Empty );
		}
		/// <summary>
		/// Replace current badword on selected correct word
		/// </summary>
		/// <param name="wrd"></param>
		/// <param name="correctWord"></param>
		public void ReplaceWord( Word wrd , string correctWord)
		{			
			this.RaiseReplaceWordEvent( EventArgs.Empty );
			//replasing current word with correct word
			this.CurrentText = this.CurrentText.Substring( 0 , wrd.BeginningOfTheWord + shifting ) + correctWord + this.CurrentText.Substring ( wrd.EndOfTheWord + shifting ); 

#if (LOG)
			if ( this.logFileWriter == null )
			{
				this.logFileWriter = new StreamWriter ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName );
			}
			this.logFileWriter.WriteLine ( "Word " + wrd.CurrentWord + " was replaced with " + correctWord );
#endif
			//shifting fo the next word 
			shifting += correctWord.Length - wrd.CurrentWord.Length;
		}
		/// <summary>
		/// check if word is correct by serching it in hashtable 
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		public bool WordIsCorrect( string word )
		{
			string work = word.Substring( 0, 2 );
			work = work.Substring( 0, 1 ).ToUpper() + work.Substring ( 1, 1 );
			//look at first two letters of the word as hashtable key
			bool exist = this.allWords.ContainsKey( work );
			//if key exist look if hashtable string contains this word 
			if ( exist )
			{
				string stringToCheck = 	Convert.ToString ( this.allWords[ work ] ).ToLower();
				string[] wordsToCheck = stringToCheck.Split( ' ' );
				foreach ( string str in wordsToCheck )
				{
					if ( word == str )
						return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Checking all words 
		/// </summary>
		public void CheckAllWords()
		{
			this.RaiseStartCheck ( EventArgs.Empty );
			foreach ( Word wrd in this.WordsCollection )
			{
				//check if this word need to be ignored
				CheckIfIgnored( wrd.CurrentWord );
				//if not 
				if ( !this.ignoreThisWord && wrd.CurrentWord.Trim().Length != 0 )
				{
					//check if word length == 1
					if ( wrd.CurrentWord.Length == 1 || wrd.CurrentWord.Length == 0 )
					{
						wrd.IsTrue = true;
					}
					
					if (  !wrd.IsTrue )
					{
						//check if word is correct
						wrd.IsTrue = WordIsCorrect( wrd.CurrentWord.ToLower() );
							
						if ( wrd.IsTrue )
							this.RaiseCorrectWord ( EventArgs.Empty );
						//if not
						else
						{
							string work = wrd.CurrentWord.Substring( 0, 2 );
							work = work.Substring( 0, 1 ).ToUpper() + work.Substring ( 1, 1 );
							//create all posible vaiants of beginning to this word
							CreatePossibleVarriants( work.ToLower() );
							
							//check current word
							wrd.CheckWord( possibleVarriants );
							
							this.IncorrectWordValue = wrd.CurrentWord;
							this.CorrectWords = wrd.TrueWords;

							IncorrectWordEventArgs args = new IncorrectWordEventArgs( IncorrectWordProblemEnum.WordIsNotInDict);
							this.RaiseIncorrectWord(args);
							++this.badWordsCount;
						}
					}
				}
				this.ignoreThisWord = false;
			}
			
			this.wordsCount = this.WordsCollection.Count;
#if (LOG)			
			if ( this.logFileWriter == null )
			{
				this.logFileWriter = new StreamWriter ( this.LogFileName );
			}
			this.logFileWriter.Write( "Total words count " + this.wordsCount + "\r\n" );
			this.logFileWriter.Write( "Bad words count " + this.badWordsCount + "\r\n" );
			this.logFileWriter.WriteLine ( "-------------------------------------------------");
			this.logFileWriter.WriteLine ();
			this.logFileWriter.WriteLine ();
			this.logFileWriter.WriteLine ();
			this.logFileWriter.Close ();
			this.logFileClosed = true;
#endif
			this.allWords = new Hashtable();
			this.badWordsCount = 0;
		}
		/// <summary>
		/// get words collection of current text
		/// </summary>
		public void GetWordsCollection()
		{
			this.allWords = new Hashtable();
			GetAllWords();
			
			char temp = '\n' ;
			if (CurrentText != null && CurrentText != string.Empty) 
			{
						
				for ( int i = 0 ; i <= this.CurrentText.Length  ; i++ )
				{
					//if end of word
					bool endOfWord = ( temp == ',' || temp == '.' || temp == ' ' || temp == '!' || temp == '?' || temp == '\0' || temp == ';' || temp == ':' );
					if ( endOfWord )
					{
						if (newWord)
						{
							//creating new instance of word and adding it to the word collection
							workString = workString.Trim();
							if ( workString.Length == 1 )
							{
								Word wrd = new Word( beg + 1, beg + 2, this.CurrentText, allWords );
								wordsCollection.Add ( wrd );
							}
							
							else 
							{
								Word wrd = new Word( beg , i - 1  , this.CurrentText, allWords );
								wordsCollection.Add ( wrd );
							}
						
							newWord = false; 
							workString = string.Empty;
						}
						
						bool newWordBeginning = ( temp != ',' || temp != '.' || temp != ' ' || temp != '!' || temp != '?' || temp != '\0' || temp != ';' || temp != ':');
						if ( newWordBeginning )
						{
							newWord = true;
							beg = i  ;
						}
					}
				
					temp = this.CurrentText[ i ];	
				
					//when checking last word of string
					if ( i + 1 == this.CurrentText.Length)
					{
						bool endOfCurrentWord = ( temp == ',' || temp == '.' || temp == ' ' || temp == '!' || temp == '?' || temp == '\0' || temp == ':' || temp == ';');
						if ( endOfCurrentWord )
						{
							Word wrd = new Word( beg, i, this.CurrentText, allWords );
							wordsCollection.Add ( wrd );
							break;
						}
						else 
						{
							Word wrd = new Word( beg , i + 1 , this.CurrentText , allWords);
							wordsCollection.Add ( wrd );
							break;
						}
					}
				
					else
						workString += temp;
				}
			}
		}
		/// <summary>
		/// get all words from dictionary
		/// </summary>
		public void GetAllWords()
		{
			DictInitEventArgs args = new DictInitEventArgs( DictInitLangugeEnum.LangEnglish );
			switch( Convert.ToInt32(this.WorkLanguge) )
			{
				case 2:
				{
					args = new DictInitEventArgs( DictInitLangugeEnum.LangSpanish );
					break;
				}
				case 3:
				{
					args = new DictInitEventArgs( DictInitLangugeEnum.LangFrench );
					break;
				}
				case 4:
				{
					args = new DictInitEventArgs( DictInitLangugeEnum.LangGerman );
					break;
				}
				case 5:
				{
					args = new DictInitEventArgs( DictInitLangugeEnum.LangDutch );
					break;
				}
			}
			this.RaiseDictInit(args);
#if (LOG)
			if ( this.logFileWriter == null )
			{
				this.logFileWriter = new StreamWriter ( this.LogFileName );
			}
			this.logFileWriter.Write( "Used " + this.DictPath + " dictionary file\r\n");
#endif
			StreamReader words = new StreamReader( AppDomain.CurrentDomain.BaseDirectory + this.DictionaryFile );
			while ( words.Peek() != -1 )
			{
				string stringToAdd = words.ReadLine() ;
			
				if ( stringToAdd.Length != 1)
				{
					if (stringToAdd.Length > 0)
					{
						if ( this.allWords.Contains( stringToAdd.Substring ( 0, 2) ) )
						{
							this.allWords[ stringToAdd.Substring ( 0, 2) ] += ' ' + stringToAdd;
						}
				
				
						else
						{
                            if (!this.allWords.Contains(stringToAdd.Substring(0, 2)))
                            {
                                this.allWords.Add(stringToAdd.Substring(0, 2), stringToAdd);
                            }
						}
					}
				}
				else
				{
					//if (stringToAdd.TrimEnd() != string.Empty && allWords[stringToAdd] != null)
                    if (!this.allWords.Contains(stringToAdd))
                    {
                        this.allWords.Add(stringToAdd, stringToAdd);
                    }
				}
			}

			words.Close();
		}
		/// <summary>
		/// Gets the text from string.
		/// </summary>
		/// <param name="textString">The text string.</param>
		/// <returns></returns>
		public string GetTextFromString( string textString )
		{
#if (LOG)
			if ( !File.Exists ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName) )
			{
				FileStream temp = File.Create( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName );
				temp.Close();
			}
			
			if ( this.logFileClosed )
			{
				this.logFileWriter = new StreamWriter ( "C:\\" + this.LogFileName ,true );
				this.logFileWriter.Write ( "Log started at " + DateTime.Now + " . ");
				this.logFileClosed = false;
			}
#endif

			this.WordsCollection = new ArrayList(); 
			this.shifting = 0;

#if (LOG)
			this.logFileWriter.Write ("Reading text from string\r\n");
#endif
			this.CurrentText = textString;
			GetWordsCollection();
			return this.CurrentText;
		}
		/// <summary>
		/// Gets the text from file.
		/// </summary>
		/// <param name="pathToFile">The path to file.</param>
		/// <returns></returns>
		public string GetTextFromFile( string pathToFile )
		{
#if (LOG)
			if ( !File.Exists ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName) )
			{
				FileStream temp = File.Create( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName );
				temp.Close();
			}
	

			if ( this.logFileClosed)
			{
				this.logFileWriter = new StreamWriter ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName ,true );
				this.logFileWriter.Write ( "Log started at " + DateTime.Now + " . ");
				this.logFileClosed = false;
			}
#endif
			
			this.WordsCollection = new ArrayList();
			this.shifting = 0;
	
#if (LOG)
			this.logFileWriter.Write ( "Reading text from file\r\n" );
#endif
			StringBuilder sb = new StringBuilder();
			StreamReader workStreamReader = new StreamReader( pathToFile );
			
			while ( workStreamReader.Peek() != -1 )
				sb.Append ( workStreamReader.ReadLine() + "\r\n" );
			
			this.CurrentText = sb.ToString();
			GetWordsCollection();
			return this.CurrentText;
		}
		/// <summary>
		/// Gets the text from file using specified stream reader.
		/// </summary>
		/// <param name="tempStreamReader">The temp stream reader.</param>
		/// <returns></returns>
		public string GetTextFromFileUsingSpecifiedStreamReader( StreamReader tempStreamReader )
		{
#if (LOG)
			if ( !File.Exists ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName) )
			{
				FileStream temp = File.Create( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName );
				temp.Close();
			}
		
			if ( this.logFileClosed)
			{
				this.logFileWriter = new StreamWriter ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName ,true );
				this.logFileWriter.Write ( "Log started at " + DateTime.Now + " . ");
				this.logFileClosed = false;
			}
#endif
		
			this.WordsCollection = new ArrayList();
			this.shifting = 0;
	
#if (LOG)		
			this.logFileWriter.Write ( "Reading text from specified file reader\r\n");
#endif
			//creating stringBuilder to build string from file
			StringBuilder sb = new StringBuilder();
			
			while ( tempStreamReader.Peek() != -1)
				sb.Append(tempStreamReader.ReadLine() + "\r\n");
			
			// returning current text from file as string
			this.CurrentText = sb.ToString();
			GetWordsCollection();
			return this.CurrentText;
		}
		/// <summary>
		/// Gets the text from memory stream.
		/// </summary>
		/// <param name="tempStream">The temp stream.</param>
		/// <returns></returns>
		public string GetTextFromMemoryStream( MemoryStream tempStream )
		{
#if (LOG)
			if ( !File.Exists ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName) )
			{
				FileStream temp = File.Create( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName );
				temp.Close();
			}
	
			if ( this.logFileClosed)
			{
				this.logFileWriter = new StreamWriter ( AppDomain.CurrentDomain.BaseDirectory + this.LogFileName ,true );
				this.logFileWriter.Write ( "Log started at " + DateTime.Now + " . ");
				this.logFileClosed = false;
			}
#endif
			
			this.WordsCollection = new ArrayList();
			this.shifting = 0;
#if (LOG)
			this.logFileWriter.Write ( "Reading text from stream\r\n");
#endif
			ASCIIEncoding encodedText = new ASCIIEncoding();
			int i = 0;
			byte[] textBuffer = new byte[ 1024 ];
			
			while ( true )
			{
				byte[] buff = new byte[ 2 ];
				int bytes = tempStream.Read( buff, i, i + 1 );
				if ( bytes == 1 )
				{
					textBuffer[ i ] = buff[ 0 ];
					++i;
					if ( buff [ 0 ] == '\0' )
						break;
				}
				else
					break;
			}
			this.CurrentText = encodedText.GetString( textBuffer, 0, i);
			GetWordsCollection();
			return this.CurrentText;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Creating possible varriants of the beginnig of the bad word
		/// </summary>
		/// <param name="h"></param>
		private void CreatePossibleVarriants( string h )
		{
			char ch = 'a';
			int i = ( int )ch;
			
			this.possibleVarriants = h.Substring( 0, 1 ).ToUpper() + h.Substring( 1, 1 );
			
			//creating possible variants if first letter wrong
			while ( ch >= 'a' && ch <= 'z')
			{
				string workstr = string.Empty;
				workstr += ch;
				workstr = workstr.Substring( 0, 1 ).ToUpper();
				if ( ch != Convert.ToChar (h.Substring( 0, 1 ) ) )
					this.possibleVarriants += workstr + h.Substring( 1, 1 );
				i = ( int )ch;
				++i;
				ch = ( char )i;
			}
			
			ch = 'a';
			i = ( int )ch;
			
			//creating possible variants if second letter wrong
			while ( ch >= 'a' && ch <= 'z' )
			{
				if ( ch != Convert.ToChar (h .Substring ( 1, 1 )))
					this.possibleVarriants +=h.Substring( 0, 1 ).ToUpper() + ch;
				i = ( int )ch;
				++i;
				ch = ( char )i;
			}
			
			ch = 'a';
			i = ( int )ch;
			
			//creating all possible variants when missed first letter
			while ( ch >= 'a' && ch <= 'z')
			{
				string workstr = string.Empty;
				workstr += ch;
				workstr = workstr.Substring( 0, 1 ).ToUpper();
				this.possibleVarriants += workstr + h.Substring( 0, 1 ) ;
				i = ( int )ch;
				++i;
				ch = ( char )i;
			}
		}
		/// <summary>
		/// chech if word need to be ignored
		/// </summary>
		/// <param name="currentWord"></param>
		private void CheckIfIgnored( string currentWord )
		{
			if ( IgnoreVariants.Instance.IgnoreAll)
			{
				this.ignoreThisWord = true;
			}
				//if ignorelowercase words check if word is lowercase
			else
			{
				if ( IgnoreVariants.Instance.IgnoreLowercase )
				{
					regx = new Regex ( RegExConstants.LowerCase );
					this.ignoreThisWord = regx.IsMatch( currentWord );
				}
			
				if ( !this.ignoreThisWord )
				{
					//if ignoreuppercase words check if word is uppercase
					if ( IgnoreVariants.Instance.IgnoreUppercase )
					{
						regx = new Regex ( RegExConstants.UpperCase );
						this.ignoreThisWord = regx.IsMatch ( currentWord );
					}
				}
			
				if ( !this.ignoreThisWord )
				{
					//if ignore words with numbers check if word is with numbers
					if ( IgnoreVariants.Instance.IgnoreNumbers )
					{
						regx = new Regex ( RegExConstants.WithDigits );
						this.ignoreThisWord = regx.IsMatch ( currentWord );
					}
				}
			
				if ( !this.ignoreThisWord )
				{
					//if ignore words with mixed case check if word is mixedcase
					if ( IgnoreVariants.Instance.IgnorMixed )
					{
						regx = new Regex ( RegExConstants.MixedCase1 );
						ignoreThisWord = regx.IsMatch (currentWord);
					
						if ( !this.ignoreThisWord )
						{
							regx = new Regex( RegExConstants.MixedCase2);
							this.ignoreThisWord = regx.IsMatch (currentWord);
						}
					}
				}
			}
		}
		
		
		#endregion
		
		#region Protectet Voids
		/// <summary>
		/// Raises the start check.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void RaiseStartCheck( EventArgs args )
		{
			if( StartCheck != null )
			{
				StartCheck( this, args );
			}
		}
		/// <summary>
		/// Raises the correct word.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void RaiseCorrectWord( EventArgs args )
		{
			if( CorrectWord != null )
			{
				CorrectWord( this, args );
			}
		}
		/// <summary>
		/// Raises the replace word event.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void RaiseReplaceWordEvent( EventArgs args )
		{
			if( ReplaceWordEvent != null )
			{
				ReplaceWordEvent( this, args );
			}
		}
		/// <summary>
		/// Raises the complete.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void RaiseComplete( EventArgs args )
		{
			if( Complete != null )
			{
				Complete( this, args );
			}
		}
		/// <summary>
		/// Raises the dict change.
		/// </summary>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void RaiseDictChange( EventArgs args )
		{
			if( DictionaryChange != null )
			{
				DictionaryChange( this, args );
			}
		}
		/// <summary>
		/// Raises the incorrect word.
		/// </summary>
		/// <param name="args">The <see cref="ActiveUp.WebControls.SpellChecking.IncorrectWordEventArgs"/> instance containing the event data.</param>
		protected void RaiseIncorrectWord( IncorrectWordEventArgs args )
		{
			if( this.IncorrectWord != null )
			{
				this.IncorrectWord( this, args );
			}
		}
       	/// <summary>
		/// Raises the dict init.
		/// </summary>
		/// <param name="args">The <see cref="ActiveUp.WebControls.SpellChecking.DictInitEventArgs"/> instance containing the event data.</param>
		protected void RaiseDictInit( DictInitEventArgs args )
		{
			if( DictInitialization != null )
			{
				DictInitialization( this, args );
			}
		}
        
		#endregion		
	}
	#endregion
}
