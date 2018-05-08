// Active WebControls v3.0
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
	/// A Collection of <see cref="DateCollectionItem"/>.
	/// </summary>
	[Serializable]
	public class LabelValueCollection : ICollection, IEnumerable, IList
	{
		/// <summary>
		/// A collection of DateCollectionItem.
		/// </summary>
		private ArrayList _labelValueCollection;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public LabelValueCollection()
		{
			_labelValueCollection = new ArrayList();
		}

		/// <summary>
		/// Gets or sets a <see cref="LabelValueCollectionItem"/> at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get
			{
				return (LabelValueCollectionItem)_labelValueCollection[index];
			}
			set
			{
				_labelValueCollection[index] = (LabelValueCollectionItem)value;
			}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _labelValueCollection.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the collection is synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a boolean value indicating whether the current instance has a fixed size.
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return _labelValueCollection.IsFixedSize;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return _labelValueCollection.IsReadOnly;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return _labelValueCollection.SyncRoot;
			}
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo (Array array, int index)
		{
			_labelValueCollection.CopyTo(array,index);
		}

		/// <summary>
		/// Gets or sets a <see cref="DateCollectionItem"/> at the specified index.
		/// </summary>
		public virtual LabelValueCollectionItem this[int index]
		{
			get
			{
				return (LabelValueCollectionItem)_labelValueCollection[index];
			}
			set
			{
				_labelValueCollection[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to remove from the collection.</param>
		public void Remove (object item)
		{
			_labelValueCollection.Remove (item);
		}

		/// <summary>
		/// Removes a <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to remove from the collection.</param>
		public void Remove (LabelValueCollectionItem item)
		{
			_labelValueCollection.Remove(item);
		}
        
		/// <summary>
		/// Inserts a <see cref="LabelValueCollectionItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to insert into the Collection.</param>
		public void Insert (int index, object item)
		{
			if (item is LabelValueCollectionItem == false)
				throw new InvalidCastException("Item must be a LabelValueCollectionItem object");

			_labelValueCollection[index] = item;
		}

		/// <summary>
		/// Inserts a <see cref="LabelValueCollectionItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to insert into the Collection.</param>
		public void Insert (int index, DateCollectionItem item)
		{
			_labelValueCollection[index] = item;
		}

		/// <summary>
		/// Adds a <see cref="DateCollectionItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add (object item)
		{
			if (item is LabelValueCollectionItem == false)
				throw new InvalidCastException("Item must be a LabelValueCollectionItem object");

			return _labelValueCollection.Add(item);
		}

		/// <summary>
		/// Adds a <see cref="DateCollectionItem"/> to the collection.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="svalue">The svalue.</param>
		/// <returns>
		/// The position into which the new element was inserted.
		/// </returns>
		public int Add (string label, string svalue)
		{
			return _labelValueCollection.Add(new LabelValueCollectionItem(label, svalue));
		}

		/// <summary>
		/// Adds a <see cref="LabelValueCollectionItem"/> to the collection
		/// </summary>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add (DateCollectionItem item)
		{
			return _labelValueCollection.Add(item);	
		}

		/// <summary>
		/// Removes all <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_labelValueCollection.Clear();
		}

		/// <summary>
		/// Determines whether a <see cref="LabelValueCollectionItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(object item)
		{
			if (item == null)
				return false;

			if (item is LabelValueCollectionItem == false)
				throw new InvalidCastException("Item must be a LabelValueCollectionItem object");

			return _labelValueCollection.Contains(item);
		}

		/// <summary>
		/// Determines whether a <see cref="LabelValueCollectionItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(LabelValueCollectionItem item)
		{
			if (item == null)
				return false;

			return _labelValueCollection.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="LabelValueCollectionItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf (object item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item is LabelValueCollectionItem == false)
				throw new InvalidCastException("Item must be a LabelValueCollectionItem object");

			return _labelValueCollection.IndexOf(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="LabelValueCollectionItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="LabelValueCollectionItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(DateCollectionItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			return _labelValueCollection.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="LabelValueCollectionItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="LabelValueCollectionItem"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_labelValueCollection.RemoveAt(index);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _labelValueCollection.GetEnumerator();
		}

	}
}