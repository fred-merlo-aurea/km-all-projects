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
	/// Contains a collection of LabeledCode objects.
	/// </summary>
	[Serializable]
	public class LabeledCodeCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public LabeledCodeCollection()
		{
			
		}

		/// <summary>
		/// Add a LabeledCode object.
		/// </summary>
		/// <param name="labeledCode">The LabeledCode object.</param>
		public void Add(LabeledCode labeledCode)
		{
			List.Add(labeledCode);
		}

		/// <summary>
		/// Adds a LabeledCode based on its Code.
		/// </summary>
		/// <param name="code">The Code.</param>
		public void Add(string code)
		{
			List.Add(new LabeledCode(code));
		}

		/// <summary>
		/// Adds a range of labeled codes.
		/// </summary>
		/// <param name="codes">The codes in a string array.</param>
		public void AddRange(string[] codes)
		{
			foreach(string code in codes)
			{
				List.Add(new LabeledCode(code));
			}
		}

		/// <summary>
		/// Adds a LabeledCode based on its Label and Code.
		/// </summary>
		/// <param name="label">The Label.</param>
		/// <param name="code">The Code.</param>
		public void Add(string label, string code)
		{
			List.Add(new LabeledCode(label, code));
		}

		/// <summary>
		/// Removes the LabeledCode at the specified index position.
		/// </summary>
		/// <param name="index"></param>
		public void Remove(int index)
		{
			// Check to see if there is a LabeledCode at the supplied index.
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index); 
			}
		}

		/// <summary>
		/// Returns the LabeledCode at the specified index position.
		/// </summary>
		public LabeledCode this[int index]
		{
			get
			{
				return (LabeledCode) List[index];
			}
		}

		/// <summary>
		/// Returns the LabeledCode with the specified code.
		/// </summary>
		public LabeledCode this[string code]
		{
			get
			{
				foreach(LabeledCode labeledCode in this.List)
				{
					if (labeledCode.Code == code)
					{ return labeledCode; }
				}
				return null;
			}
			
		}

		/// <summary>
		/// Gets the labels contained in the collection.
		/// </summary>
		public string[] Labels
		{
			get
			{
				string[] labels = new string[this.Count];

				for(int index=0;index<this.Count;index++)
				{
					labels[index] = this[index].Label;
				}
			
				return labels;
			}
		}

		/// <summary>
		/// Gets the codes contained in the collection.
		/// </summary>
		public string[] Codes
		{
			get
			{
				string[] codes = new string[this.Count];

				for(int index=0;index<this.Count;index++)
				{
					codes[index] = this[index].Code;
				}
				
				return codes;
			}
		}
	}
}
