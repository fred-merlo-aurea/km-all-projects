// Html TextBox 2.x
// Copyright (c) 2003 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a labeled code to use with SpecialChars, FontColor and CodeSnippet tools.
	/// </summary>
	[Serializable]
	public class LabeledCode
	{
		string _label, _code;
 
		/// <summary>
		/// The default constructor.
		/// </summary>
		public LabeledCode()
		{
			Label = string.Empty;
			Code = string.Empty;
		}

		/// <summary>
		/// Add a LabeledCode based on it's code. Label will take the same value as the code.
		/// </summary>
		/// <param name="code"></param>
		public LabeledCode(string code)
		{
			Label = code;
			Code = code;
		}

		/// <summary>
		/// Add a LabeledCode based on it's label and code.
		/// </summary>
		/// <param name="label"></param>
		/// <param name="code"></param>
		public LabeledCode(string label, string code)
		{
			Label = label;
			Code = code;
		}	

		/// <summary>
		/// Gets or sets the Label of the LabeledCode.
		/// </summary>
		public string Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		/// <summary>
		/// Gets or sets the Code of the LabeledCode.
		/// </summary>
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
			}
		}
	}
}
