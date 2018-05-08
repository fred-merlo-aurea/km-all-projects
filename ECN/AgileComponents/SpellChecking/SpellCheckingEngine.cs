#region Used Files
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
#endregion

namespace ActiveUp.WebControls
{
	#region SpellChecking
	/// <summary>
	/// SpellCheckClass used for spellcheking of the current word
	/// use regular expressions to optimize checking
	/// </summary>
	public class SpellChecking
	{
		#region Members
		private string wordToCheck;
		
		private Hashtable allWords;
		
		private string allVaraints;
		
		private bool check = false;
		
		//private StreamReader words = new StreamReader(@"c:\inetpub\wwwroot\WebSpellCheckerApplication\words.txt");
		
		private ArrayList ifWordBad ;
		
		private Regex regx;
		private string regExString;
		#endregion
    							
		#region Properties
		/// <summary>
		/// Gets or sets the word to check.
		/// </summary>
		/// <value>The word to check.</value>
		public string WordToCheck
		{
			get{ return wordToCheck; }
			set{ wordToCheck = value; }
		}
		#endregion
    		
		#region Public Methods
		/// <summary>
		/// Check word from dictionary 
		/// </summary>
		/// <returns></returns>
		public ArrayList CheckWord()
		{
			//return this arrayList if word is correct and was found in dictionary
			ArrayList ifWordCorrect = new ArrayList();
			//returns this arraylist if word is bad
			ifWordBad = new ArrayList();
			//num is used to dynamically generate regular expression strings
			int num;
			//if word founded in dict can be correct 
			bool checkc;
			//get only low case chars to optimise search
			WordToCheck = WordToCheck.ToLower();	
			
			int variantsCount = 0;
			while ( variantsCount < this.allVaraints.Length )
			{
				string Key = this.allVaraints.Substring( variantsCount , 2);
				variantsCount += 2 ;
						
				if (WordToCheck.Length == 1)
				{
					ifWordCorrect.Add(WordToCheck );
					check = true;
					break;
				}
				bool keyExist = this.allWords.ContainsKey( Key );
				if ( keyExist )
				{
					//splitting string with words from dictionary
				
					string[] ar1= Convert.ToString (this.allWords[ Key ]).Split( ' ' );
    			
					num = this.WordToCheck.Length;
            		
					// if word founded in dict stop process of checking
					foreach( string strng in ar1 )
					{
						//get only low case chars to optimise search
						string str = strng.ToLower();
    											
						//if word length is 2 do this 
						if ( num == 2 && str.Length <= 3 && str.Length != 1 )
						{
							//checking for words in dict which length 2 or 3
							regExString = '[' + WordToCheck + ']';
							regx = new Regex(regExString);
            				
							if ( regx.Matches(str).Count == str.Length - 1)
							{
								//adding word if it can be true
								ifWordBad.Add( str );
							}
						}
            				
						//if word length more then 2
						if ( num > 2 && str.Length > 2 && str.Length <= num + 1 && str.Length >= num - 1 )
						{
							//dynamically generating regEx strings
							for (int i = 0 ; i <= num ; i++ )
							{
								if ( i == 0 ) 
									regExString = "^[^ ]" + WordToCheck.Substring( 1, num - 1) + "$";
								else
								{
									regExString = ( i == num ) ?
										"^" + WordToCheck + "[^ ]$" :
										"^" + WordToCheck.Substring( 0 , i) + "[^ ]" + WordToCheck.Substring( i + 1 , num - i -1) + "$";									
								}
								//update Regex with generated string
								regx = new Regex( regExString);
								//checking if word can be correct
								checkc = regx.IsMatch(str);
								//if word can be correct adding to arraylist 
								if (checkc)
								{
									ifWordBad.Add( str);
									checkc = false;
								}
							}
						}			
					}			
				}
			}

			//closing streamreader of words		
			//words.Close();
		            		
			//if word correct return it, otherwise return array of possible words
			return (check) ? ifWordCorrect : ifWordBad;
		}
            		
		#endregion
    		
		#region Constructor
    	
		/// <summary>
		/// Initializes a new instance of the <see cref="SpellChecking"/> class.
		/// </summary>
		/// <param name="word">The word.</param>
		/// <param name="words">The words.</param>
		/// <param name="vars">The vars.</param>
		public SpellChecking( string word , Hashtable words , string vars)
		{
			this.WordToCheck = word;
			this.allWords = words;
			this.allVaraints = vars;
		}
    		
		#endregion
	}
	#endregion
}
