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
	/// This collection contains all Custom Tags to use with the CustomTag tool.
	/// </summary>
	[Serializable]
	public class TagCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public TagCollection()
		{

		}
 
		/// <summary>
		/// Adds a Custom Tag object.
		/// </summary>
		/// <param name="tag">The CustomTag object.</param>
		public void Add(Tag tag)
		{
			List.Add(tag);
		}

		/// <summary>
		/// Adds a Custom Tag based on it's Label and Tag Name.
		/// </summary>
		/// <param name="label">The Label.</param>
		/// <param name="tagName">The Tag Name.</param>
		public void Add(string label, string tagName)
		{
			List.Add(new Tag(label, tagName));
		}

		/// <summary>
		/// Adds a Tag based on its Label, Tag Name, Attribute Name and Attribute Value.
		/// </summary>
		/// <param name="label">The Label.</param>
		/// <param name="tagName">The Tag Name.</param>
		/// <param name="attributeName">The Attribute Name.</param>
		/// <param name="attributeValue">The Attribute Value.</param>
		public void Add(string label, string tagName, string attributeName, string attributeValue)
		{
			List.Add(new Tag(label, tagName, attributeName, attributeValue));
		}

		/// <summary>
		/// Removes the Tag at the specified index position.
		/// </summary>
		/// <param name="index"></param>
		public void Remove(int index)
		{
			// Check to see if there is a custom tag at the supplied index.
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index); 
			}
		}

		/// <summary>
		/// Returns the Custom Tag at the specified index position.
		/// </summary>
		public Tag this[int index]
		{
			get
			{
				return (Tag) List[index];
			}
		}

		/// <summary>
		/// Returns the Tag with the specified label.
		/// </summary>
		public Tag this[string label]
		{
			get
			{
				foreach(Tag tag in this.List)
				{
					if (tag.Label == label)
					{ return tag; }
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
		/// Gets the tag names contained in the collection.
		/// </summary>
		public string[] TagNames
		{
			get
			{
				string[] tagNames = new string[this.Count];

				for(int index=0;index<this.Count;index++)
				{
					tagNames[index] = this[index].TagName;
				}
				
				return tagNames;
			}
		}

		/// <summary>
		/// Gets the attribute values contained in the collection.
		/// </summary>
		public string[] AttributeValues
		{
			get
			{
				string[] attributeValues = new string[this.Count];

				for(int index=0;index<this.Count;index++)
				{
					attributeValues[index] = this[index].AttributeValue;
				}
				
				return attributeValues;
			}
		}

		/// <summary>
		/// Gets the attribute names contained in the collection.
		/// </summary>
		public string[] AttributeNames
		{
			get
			{
				string[] attributeNames = new string[this.Count];

				for(int index=0;index<this.Count;index++)
				{
					attributeNames[index] = this[index].AttributeName;
				}
				
				return attributeNames;
			}
		}
	}
}
