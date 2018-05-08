using System;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Word class used to save words and then word with them
	/// </summary>
	public class Word
	{
		#region Members
		private int beginningOfTheWord;
		private int endOfTheWord;
		
		private string currentText;

		/// <summary>
		/// Current word.
		/// </summary>
		public string CurrentWord;
		
		private bool isTrue = false;
		
		private ArrayList trueWords = new ArrayList();
		private Hashtable allWords = new Hashtable();
		#endregion
		
		#region Properties
		/// <summary>
		/// Array list of possible correct words to current
		/// </summary>
		public ArrayList TrueWords
		{
			get{ return trueWords; }
			set{ trueWords = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is true.
		/// </summary>
		/// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
		public bool IsTrue
		{
			get{ return isTrue; }
			set{ isTrue = value; }
		}
		/// <summary>
		/// Gets or sets the current text.
		/// </summary>
		/// <value>The current text.</value>
		public string CurrentText
		{
			get{ return currentText; }
			set{ currentText = value; }
		}
		/// <summary>
		/// Gets or sets the end of the word.
		/// </summary>
		/// <value>The end of the word.</value>
		public int EndOfTheWord
		{
			get{ return endOfTheWord; }
			set{ endOfTheWord = value; }
		}
		/// <summary>
		/// Gets or sets the beginning of the word.
		/// </summary>
		/// <value>The beginning of the word.</value>
		public int BeginningOfTheWord
		{
			get{ return beginningOfTheWord; }
			set{ beginningOfTheWord = value; }
		}
        
		#endregion		
		
		#region Public Methods
		/// <summary>
		/// Check word is it true or if not check possible values of correct form to this word
		/// </summary>
		/// <param name="vars"></param>
		public void CheckWord( string vars)
		{
			SpellChecking sc = new	SpellChecking( this.CurrentWord , this.allWords , vars );
			trueWords = sc.CheckWord();
        			
			if ( trueWords.Count == 1)
			{
				if ( Convert.ToString( trueWords[ 0 ] ) == this.CurrentWord)
				{
					IsTrue = true;
				}
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Method used to get current word from whole text
		/// </summary>
		private void GetWord()
		{
			if ( this.BeginningOfTheWord > this.EndOfTheWord )
				this.BeginningOfTheWord = 0;
			this.CurrentWord = this.CurrentText.Substring ( this.BeginningOfTheWord  ,this.EndOfTheWord -this.BeginningOfTheWord );
			Console.WriteLine(this.CurrentWord.Length);
			this.CurrentWord = this.CurrentWord.Trim();
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Word"/> class.
		/// </summary>
		/// <param name="beg">The beg.</param>
		/// <param name="end">The end.</param>
		/// <param name="text">The text.</param>
		/// <param name="AllWords">All words.</param>
		public Word( int beg , int end , string text , Hashtable AllWords )
		{
			this.BeginningOfTheWord = beg;
			this.EndOfTheWord = end;
			this.CurrentText = text;
			this.allWords = AllWords;
			GetWord();
		}
		#endregion
	}
}
