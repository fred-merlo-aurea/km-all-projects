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
	/// A Collection of <see cref="DateStyleCollectionItem"/>.
	/// </summary>
	public class DateStyleCollection : ICollection, IEnumerable, IList
	{
		/// <summary>
		/// A collection of DateStyleCollectionItem.
		/// </summary>
		private ArrayList _dateStyleCollection;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DateStyleCollection()
		{
			_dateStyleCollection = new ArrayList();
		}

		/// <summary>
		/// Gets or sets a <see cref="DateStyleCollectionItem"/> at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get
			{
				return (DateStyleCollectionItem)_dateStyleCollection[index];
			}
			set
			{
				_dateStyleCollection[index] = (DateStyleCollectionItem)value;
			}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _dateStyleCollection.Count;
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
				return _dateStyleCollection.IsFixedSize;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return _dateStyleCollection.IsReadOnly;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return _dateStyleCollection.SyncRoot;
			}
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo (Array array, int index)
		{
			_dateStyleCollection.CopyTo(array,index);
		}

		/// <summary>
		/// Gets or sets a <see cref="DateStyleCollectionItem"/> at the specified index.
		/// </summary>
		public virtual DateStyleCollectionItem this[int index]
		{
			get
			{
				return (DateStyleCollectionItem)_dateStyleCollection[index];
			}
			set
			{
				_dateStyleCollection[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="DateStyleCollectionItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateStyleCollectionItem"/> to remove from the collection.</param>
		public void Remove (object item)
		{
			_dateStyleCollection.Remove (item);
		}
        
		/// <summary>
		/// Inserts a <see cref="DateStyleCollectionItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="DateStyleCollectionItem"/> to insert into the Collection.</param>
		public void Insert (int index, object item)
		{
			if (item is DateStyleCollectionItem == false)
				throw new InvalidCastException("Item must be a DateStyleCollectionItem object");

			_dateStyleCollection[index] = item;
		}

		/// <summary>
		/// Adds a <see cref="DateStyleCollectionItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateStyleCollectionItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add (object item)
		{
			if (item is DateStyleCollectionItem == false)
				throw new InvalidCastException("Item must be a DateStyleCollectionItem object");

			return _dateStyleCollection.Add(item);
		}

		/// <summary>
		/// Removes all <see cref="DateStyleCollectionItem"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_dateStyleCollection.Clear();
		}

		/// <summary>
		/// Determines whether a <see cref="DateStyleCollectionItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateStyleCollectionItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(object item)
		{
			if (item is DateStyleCollectionItem == false)
				throw new InvalidCastException("Item must be a DateStyleCollectionItem object");

			return _dateStyleCollection.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="DateStyleCollectionItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="DateStyleCollectionItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf (object item)
		{
			if (item is DateStyleCollectionItem == false)
				throw new InvalidCastException("Item must be a DateStyleCollectionItem object");

			return _dateStyleCollection.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="DateStyleCollectionItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="DateStyleCollectionItem"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_dateStyleCollection.RemoveAt(index);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _dateStyleCollection.GetEnumerator();
		}

		/// <summary>
		/// Get a string containing all the elements separated by the separator.
		/// </summary>
		/// <param name="separator">Separator used to separated each element in the return string.</param>
		/// <param name="imagePath">Path where the images are located.</param>
		/// <returns>String containing all the elements separated by the separator.</returns>
		public string GetItemListToString(string separator,string imagePath)
		{
			string ret = "";

			for (int i = 0 ; i < _dateStyleCollection.Count ; i++)
			{
				int val = this[i].Date.Day;
				string day = "";
				if (val < 10) day = '0' + val.ToString();
				else day = val.ToString();

				val = this[i].Date.Month;
				string month = "";
				if (val < 10) month = '0' + val.ToString();
				else month = val.ToString();

				ret += string.Format("{0}/{1}/{2};{3}{4}",this[i].Date.Year,month,day,Utils.CreateStyleVariable(this[i].BackColor,this[i].BackgroundImage,this[i].ForeColor,imagePath),separator);
			}

			return ret;
		}

		/// <summary>
		/// Indicates if a Date is already in the collection.
		/// </summary>
		/// <param name="date">Date to check.</param>
		/// <returns>True if the date is already present, otherwise false.</returns>
		public bool IsDatePresent(DateTime date)
		{
			DateTime dateToCheck = new DateTime(date.Year,date.Month,date.Day);

			foreach(DateStyleCollectionItem dateItem in _dateStyleCollection)
			{
				DateTime dateInColl = new DateTime(dateItem.Date.Year,dateItem.Date.Month,dateItem.Date.Day);

				if (dateToCheck == dateInColl)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Indicates if a Date is already in the collection.
		/// </summary>
		/// <param name="dateItem"><see cref="DateStyleCollectionItem"/> that contains the date to check.</param>
		/// <returns>True if the date is already present, otherwise false.</returns>
		public bool IsDatePresent(DateStyleCollectionItem dateItem)
		{
			return IsDatePresent(dateItem.Date);
		}
	}
}