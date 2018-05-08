/*
 * 5/14/2003
 * class   :  StringTokenizer
 * author  :  Dan Schwie
 * website :  http://www.opensourcealternatives.org
 * email   :  dschwie@opensourcealternatives.org
 * 
 */
 
using System;

namespace ecn.wizard.webservice.Objects {
  
	/* class to parse a string into tokens
	 * an instance of StringTokenizer behaves as follows:
	 *
	 *  StringTokenizer st = new StringTokenizer("a b c d e f");
	 *  while(st.HasMoreTokens()) {
	 *    Console.WriteLine(st.NextToken());
	 *  }
	 *
	 *  output is:
	 *  a
	 *  b
	 *  c
	 *  d
	 *  e
	 *  f
	 */
	public class StringTokenizer {
    
		private string[] elements;
		private int currentIndex;
  
		/*
		 * constructor uses a space (' ') as a default delimiter
		 * args: string s
		 *       a string to be parsed
		 */
		public StringTokenizer(string s) {
			elements = new string[s.Length];
			elements = s.Split(' ');
			currentIndex = 0;
		}
    
		/*
		 * constructor uses a specified delimiter
		 * args: string s - a string to be parsed
		 *       char delimiter - a char to use as a delimiter
		 */
		public StringTokenizer(string s, char delimiter) {
			elements = new string[s.Length];
			elements = s.Split(delimiter);
			currentIndex = 0;
		}
  
		/*
		 * bool HasAnyTokens
		 * determine whether this string contains any tokens
		 * returns: true if this string contains at least 1 token
		 *          false if this string contains no tokens
		 */
		public bool HasAnyTokens() {
			return elements.Length > 0;
		}
    
		/*
		 * bool HasMoreTokens
		 * determine whether this string contains any more tokens
		 * returns: true if this string contains at least 1 more token
		 *          false if this string contains no more tokens
		 */
		public bool HasMoreTokens() {
			return currentIndex < elements.Length;
		}
    
		/*
		 * string NextToken
		 * get the next token in this string if there is one
		 * returns: the next token in this string if there is one
		 * throws : NoSuchElementException
		 *          thrown if there are no more tokens in this string
		 */
		public string NextToken() {
			string next;
			if(currentIndex < elements.Length) {
				next = elements[currentIndex];
				currentIndex++;
			}
			else {
				throw new NoSuchElementException("No more tokens.");
			}
			return next;
		}
    
		/*
		 * string GetToken
		 * get a token based on it's position within this string
		 * args   : int index
		 *          the position within the string of the desired character
		 * returns: the token at position[index] if index is a valid index
		 * throws : NoSuchElementException
		 *          thrown if there is no token positioned at index
		 */
		public string GetToken(int index) {
			if((index < elements.Length) && (index >= 0)) {
				return elements[index];
			}
			else {
				throw new NoSuchElementException("No token at position: " + index.ToString());
			}
		}
    
		/*
		 * string FirstToken
		 * get the first token in this string
		 * returns: the first token in this string
		 */
		public string FirstToken() {
			return elements[0];
		}
    
		/*
		 * string LastToken
		 * get the last token in this string
		 * returns: the last token in this string
		 */
		public string LastToken() {
			return elements[elements.Length - 1];
		}
    
		/*
		 * int CountTokens
		 * count the tokens in this string
		 * returns: the number of tokens in this string
		 */
		public int CountTokens() {
			return elements.Length;
		}
    
		/*
		 * int CurrentIndex
		 * get the index of the current token
		 * returns: the index of the current token
		 */
		public int CurrentIndex() {
			return currentIndex;
		}
	}
}