using System;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// This is a collection of attributes. Typically, this is associated with a particular
	/// element. This collection is searchable by both the index and the name of the attribute.
	/// </summary>
	internal class HtmlAttributeCollection: CollectionBase
	{
		HtmlElement _element;

		public HtmlAttributeCollection()
		{
			_element = null;
		}

		/// <summary>
		/// This will create an empty collection of attributes.
		/// </summary>
		/// <param name="element"></param>
		internal HtmlAttributeCollection(HtmlElement element)
		{
			_element = element;
		}

		/// <summary>
		/// This will add an element to the collection.
		/// </summary>
		/// <param name="attribute">The attribute to add.</param>
		/// <returns>The index at which it was added.</returns>
		public int Add(HtmlAttribute attribute)
		{
			return base.List.Add( attribute );
		}

		/// <summary>
		/// This provides direct access to an attribute in the collection by its index.
		/// </summary>
		public HtmlAttribute this [int index]
		{
			get
			{
				return (HtmlAttribute) base.List[ index ];
			}
			set
			{
				base.List[ index ] = value;
			}
		}

		/// <summary>
		/// This will search the collection for the named attribute. If it is not found, this
		/// will return null.
		/// </summary>
		/// <param name="name">The name of the attribute to find.</param>
		/// <returns>The attribute, or null if it wasn't found.</returns>
		public HtmlAttribute FindByName(string name)
		{
			int index = IndexOf( name );
			if( index == -1 )
			{
				return null;
			}
			else
			{
				return this[ IndexOf( name ) ];
			}
		}

		/// <summary>
		/// This will return the index of the attribute with the specified name. If it is
		/// not found, this method will return -1.
		/// </summary>
		/// <param name="name">The name of the attribute to find.</param>
		/// <returns>The zero-based index, or -1.</returns>
		public int IndexOf(string name)
		{
			for( int index = 0 ; index < this.List.Count ; index++ )
			{
				if( this[ index ].Name.ToLower().Equals( name.ToLower() ) )
				{
					return index;
				}
			}
			return -1;
		}

		/// <summary>
		/// This overload allows you to have direct access to an attribute by providing
		/// its name. If the attribute does not exist, null is returned.
		/// </summary>
		public HtmlAttribute this [string name]
		{
			get
			{
				return FindByName( name );
			}
		}
	}
}
