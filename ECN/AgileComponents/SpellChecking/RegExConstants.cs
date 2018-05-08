using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Constatns wirh regular expressions to ignore some words
	/// </summary>
	public class RegExConstants
	{
		/// <summary>
		/// Use this regular expression to check if the word is only lower case.
		/// </summary>
		public const string LowerCase = "^[a-z]+$";
		/// <summary>
		/// Use this regular expression to check if the word is only upper case.
		/// </summary>
		public const string UpperCase = "^[A-Z]+$";
		/// <summary>
		/// Use this regular expression to check if the word use digits.
		/// </summary>
		public const string WithDigits = "^([a-zA-Z]+)?[0-9]+[a-z]?";
		/// <summary>
		/// Use this regulart expressions to check if this word is mixed case.
		/// </summary>
		public const string MixedCase1 = "^[A-Z]+[a-z]+([A-Za-z]+)?";
		/// <summary>
		/// Use this regulart expressions to check if this word is mixed case.
		/// </summary>
		public const string MixedCase2 = "^[a-z]+[A-Z]+([A-Za-z]+)?";
	}
}
