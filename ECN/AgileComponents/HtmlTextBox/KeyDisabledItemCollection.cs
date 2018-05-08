// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;

namespace ActiveUp.WebControls  
{
	/// <summary>
	/// Represents a <see cref="KeyDisabledItemCollection"/> object.
	/// </summary>
	public class KeyDisabledItemCollection : ICollection, IEnumerable, IList
	{
		private ArrayList _keyDisabledItemCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyDisabledItemCollection"/> class.
		/// </summary>
		public KeyDisabledItemCollection()
		{
			_keyDisabledItemCollection = new ArrayList();
		}

		object IList.this[int index]
		{
			get
			{
				return (KeyDisabledItem)_keyDisabledItemCollection[index];
			}
			set
			{
				_keyDisabledItemCollection[index] = (KeyDisabledItem)value;
			}
		}

		/// <summary>
		/// When implemented by a class, gets the number of
		/// elements contained in the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		public int Count
		{
			get
			{
				return _keyDisabledItemCollection.Count;
			}
		}

		/// <summary>
		/// When implemented by a class, gets a value
		/// indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized
		/// (thread-safe).
		/// </summary>
		/// <value></value>
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// When implemented by a class, gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
		/// </summary>
		/// <value></value>
		public bool IsFixedSize
		{
			get
			{
				return _keyDisabledItemCollection.IsFixedSize;
			}
		}

		/// <summary>
		/// When implemented by a class, gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
		/// </summary>
		/// <value></value>
		public bool IsReadOnly
		{
			get
			{
				return _keyDisabledItemCollection.IsReadOnly;
			}
		}

		/// <summary>
		/// When implemented by a class, gets an object that
		/// can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		public object SyncRoot
		{
			get
			{
				return _keyDisabledItemCollection.SyncRoot;
			}
		}

		/// <summary>
		/// When implemented by a class, copies the elements of
		/// the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is <see langword="null"/>.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<para>
		/// 		<paramref name="array"/> is multidimensional.</para>
		/// 	<para>-or-</para>
		/// 	<para>
		/// 		<paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.</para>
		/// 	<para>-or-</para>
		/// 	<para>The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.</para>
		/// </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo (Array array, int index)
		{
			_keyDisabledItemCollection.CopyTo(array,index);
		}

		/// <summary>
		/// Gets or sets the <see cref="KeyDisabledItem"/> at the specified index.
		/// </summary>
		/// <value></value>
		public virtual KeyDisabledItem this[int index]
		{
			get
			{
				return (KeyDisabledItem)_keyDisabledItemCollection[index];
			}
			set
			{
				_keyDisabledItemCollection[index] = value;
			}
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Remove (object item)
		{
			_keyDisabledItemCollection.Remove (item);
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Remove (KeyDisabledItem item)
		{
			_keyDisabledItemCollection.Remove(item);
		}
        
		/// <summary>
		/// Inserts at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert (int index, object item)
		{
			if (item is KeyDisabledItem == false)
				throw new InvalidCastException("Item must be a KeyDisabledItem object");

			_keyDisabledItemCollection[index] = item;
		}

		/// <summary>
		/// Inserts at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert (int index, KeyDisabledItem item)
		{
			_keyDisabledItemCollection[index] = item;
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int Add (object item)
		{
			if (item is KeyDisabledItem == false)
				throw new InvalidCastException("Item must be a KeyDisabledItem object");

			return _keyDisabledItemCollection.Add(item);
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int Add (KeyDisabledItem item)
		{
			return _keyDisabledItemCollection.Add(item);	
		}

		/// <summary>
		/// Adds the specified code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public int Add (KeyCode code)
		{
			KeyDisabledItem item = new KeyDisabledItem();
			item.Code = code;
			return _keyDisabledItemCollection.Add(item);	
		}

		/// <summary>
		/// When implemented by a class, removes all items from the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.</exception>
		public void Clear()
		{
			_keyDisabledItemCollection.Clear();
		}

		/// <summary>
		/// Determines whether contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if contains the specified item; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(object item)
		{
			if (item == null)
				return false;

			if (item is KeyDisabledItem == false)
				throw new InvalidCastException("Item must be a KeyDisabledItem object");

			return _keyDisabledItemCollection.Contains(item);
		}

		/// <summary>
		/// Determines whether contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if contains the specified item; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(KeyDisabledItem item)
		{
			if (item == null)
				return false;

			return _keyDisabledItemCollection.Contains(item);
		}

		/// <summary>
		/// Searches for the specified <see cref="KeyDisabledItem"/> and returns the zero-based index of the first occurrence within the entire collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int IndexOf (object item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item is KeyDisabledItem == false)
				throw new InvalidCastException("Item must be a KeyDisabledItem object");

			return _keyDisabledItemCollection.IndexOf(item);
		}

		/// <summary>
		/// Searches for the specified <see cref="KeyDisabledItem"/> and returns the zero-based index of the first occurrence within the entire collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int IndexOf(KeyDisabledItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			return _keyDisabledItemCollection.IndexOf(item);
		}

		/// <summary>
		/// When implemented by a class, removes the <see cref="T:System.Collections.IList"/>
		/// item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">
		/// 	<para>The <see cref="T:System.Collections.IList"/> is read-only.</para>
		/// 	<para>-or-</para>
		/// 	<para>The <see cref="T:System.Collections.IList"/> has a fixed size.</para>
		/// </exception>
		public void RemoveAt(int index)
		{
			_keyDisabledItemCollection.RemoveAt(index);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/>
		/// that can be used to iterate through the collection.
		/// </returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _keyDisabledItemCollection.GetEnumerator();
		}


	}
}