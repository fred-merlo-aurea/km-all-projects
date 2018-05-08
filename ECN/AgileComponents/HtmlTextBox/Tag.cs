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
	/// Used with the CustomTag or ParagraphStyle tool.
	/// </summary>
	[Serializable]
	public class Tag
	{
		private string _label, _tagName, _attributeName, _attributeValue;

		/// <summary>
		/// The default contructor.
		/// </summary>
		public Tag()
		{
			Label = string.Empty;
			TagName = string.Empty;
			AttributeName = string.Empty;
			AttributeValue = string.Empty;
		}

		/// <summary>
		/// Constructor to set the Label and Tag Name.
		/// </summary>
		/// <param name="label">The Label</param>
		/// <param name="tagName">The Tag Name</param>
		public Tag(string label, string tagName)
		{
			Label = label;
			TagName = tagName;
			AttributeName = string.Empty;
			AttributeValue = string.Empty;
		}

		/// <summary>
		/// Contrustor to set the Label, Tag Name, Attribute Name and Attribute Value.
		/// </summary>
		/// <param name="label">The Label.</param>
		/// <param name="tagName">The Tag Name.</param>
		/// <param name="attributeName">The Attribute Name.</param>
		/// <param name="attributeValue">The Attribute Value.</param>
		public Tag(string label, string tagName, string attributeName, string attributeValue)
		{
			Label = label;
			TagName = tagName;
			AttributeName = attributeName;
			AttributeValue = attributeValue;
		}

		/// <summary>
		/// The Label.
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
		/// The Tag Name.
		/// </summary>
		public string TagName
		{
			get
			{
				return _tagName;
			}
			set
			{
				_tagName = value;
			}
		}

		/// <summary>
		/// The Attribute Name.
		/// </summary>
		public string AttributeName
		{
			get
			{
				return _attributeName;
			}
			set
			{
				_attributeName = value;
			}
		}

		/// <summary>
		/// The Attribute Value.
		/// </summary>
		public string AttributeValue
		{
			get
			{
				return _attributeValue;
			}
			set
			{
				_attributeValue = value;
			}
		}
		
	}
}
