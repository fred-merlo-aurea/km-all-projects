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
	#region class DateCollection

	/// <summary>
	/// A Collection of <see cref="DateCollectionItem"/>.
	/// </summary>
	public class DateCollection : ICollection, IEnumerable, IList, IStateManager
	{
		#region Variables

		/// <summary>
		/// A collection of DateCollectionItem.
		/// </summary>
		private ArrayList _dateCollection;

		private bool _isTrackingViewState;
		private bool _saveAll;

		#endregion

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DateCollection()
		{
			_dateCollection = new ArrayList();
		}

		/// <summary>
		/// Gets or sets a <see cref="DateCollectionItem"/> at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get
			{
				return (DateCollectionItem)_dateCollection[index];
			}
			set
			{
				_dateCollection[index] = (DateCollectionItem)value;
			}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _dateCollection.Count;
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
				return _dateCollection.IsFixedSize;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return _dateCollection.IsReadOnly;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return _dateCollection.SyncRoot;
			}
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo (Array array, int index)
		{
			_dateCollection.CopyTo(array,index);
		}

		/// <summary>
		/// Gets or sets a <see cref="DateCollectionItem"/> at the specified index.
		/// </summary>
		public virtual DateCollectionItem this[int index]
		{
			get
			{
				return (DateCollectionItem)_dateCollection[index];
			}
			set
			{
				_dateCollection[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to remove from the collection.</param>
		public void Remove (object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is DateCollectionItem)) 
			{
				throw new ArgumentException("item must be a DateCollectionItem");
			}

			Remove((DateCollectionItem)item);

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Removes a <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to remove from the collection.</param>
		public void Remove (DateCollectionItem item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}

			int index = IndexOf(item);
			if (index >= 0) 
			{
				RemoveAt(index);
			}

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}
        
		/// <summary>
		/// Inserts a <see cref="DateCollectionItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="DateCollectionItem"/> to insert into the Collection.</param>
		public void Insert (int index, object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is DateCollectionItem)) 
			{
				throw new ArgumentException("Item must be a DateCollectionItem");
			}

			Insert(index, (DateCollectionItem)item);

			if (_isTrackingViewState)
			{
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Inserts a <see cref="DateCollectionItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="DateCollectionItem"/> to insert into the Collection.</param>
		public void Insert (int index, DateCollectionItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			_dateCollection.Insert(index,item);

			if (_isTrackingViewState) 
			{
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Adds a <see cref="DateCollectionItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add (object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is DateCollectionItem)) 
			{
				throw new ArgumentException("Item must be a DateCollectionItem");
			}

			return Add((DateCollectionItem)item);
		}

		/// <summary>
		/// Adds a <see cref="DateCollectionItem"/> to the collection
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add (DateCollectionItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (_isTrackingViewState) 
			{
				((IStateManager)item).TrackViewState();
				item.SetDirty();
			}

			return _dateCollection.Add(item);
		}

		/// <summary>
		/// Removes all <see cref="DateCollectionItem"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_dateCollection.Clear();

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Determines whether a <see cref="DateCollectionItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(object item)
		{
			if (item == null)
				return false;

			if (item is DateCollectionItem == false)
				throw new InvalidCastException("Item must be a DateCollectionItem object");

			return _dateCollection.Contains(item);
		}

		/// <summary>
		/// Determines whether a <see cref="DateCollectionItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(DateCollectionItem item)
		{
			if (item == null)
				return false;

			return _dateCollection.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="DateCollectionItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf (object item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item is DateCollectionItem == false)
				throw new InvalidCastException("Item must be a DateCollectionItem object");

			return _dateCollection.IndexOf(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="DateCollectionItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="DateCollectionItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(DateCollectionItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			return _dateCollection.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="DateCollectionItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="DateCollectionItem"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_dateCollection.RemoveAt(index);

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _dateCollection.GetEnumerator();
		}

		/// <summary>
		/// Get a string containing all the elements separated by the separator.
		/// </summary>
		/// <param name="separator">Separator used to separated each element in the return string.</param>
		/// <returns>String containing all the elements separated by the separator.</returns>
		public string GetItemListToString(string separator)
		{
			string ret = "";

			for (int i = 0 ; i < _dateCollection.Count ; i++)
			{

				int val = this[i].Date.Day;
				string day = "";
				if (val < 10) day = '0' + val.ToString();
				else day = val.ToString();

				val = this[i].Date.Month;
				string month = "";
				if (val < 10) month = '0' + val.ToString();
				else month = val.ToString();

				ret += string.Format("{0}/{1}/{2}{3}",this[i].Date.Year,month,day,separator);
				
			}

			//ret = ret.TrimEnd(separator.ToCharArray());

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

			foreach(DateCollectionItem dateItem in _dateCollection)
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
		/// <param name="dateItem"><see cref="DateCollectionItem"/> that contains the date to check.</param>
		/// <returns>True if the date is already present, otherwise false.</returns>
		public bool IsDatePresent(DateCollectionItem dateItem)
		{
			return IsDatePresent(dateItem.Date);
		}

		#region ViewState

		bool IStateManager.IsTrackingViewState
		{
			get {return _isTrackingViewState;}
		}

		void IStateManager.TrackViewState()
		{
			_isTrackingViewState = true;
			foreach(DateCollectionItem date in _dateCollection)
				((IStateManager)date).TrackViewState();
		}
		/// <summary>
		/// When implemented by a class, saves the changes to a server control's view state to an
		/// <see cref="T:System.Object"/> .
		/// </summary>
		/// <returns>
		/// The <see langword="Object"/> that contains the view state changes.
		/// </returns>
		public object SaveViewState()
		{
			if (_saveAll == true) 
			{ 
				// Save all items.
				ArrayList states = new ArrayList(Count);
				for (int i = 0; i < Count; i++) 
				{
					DateCollectionItem dateItem = (DateCollectionItem)_dateCollection[i];
					dateItem.SetDirty();
					states.Add(((IStateManager)dateItem).SaveViewState());
				}
				if (states.Count > 0) 
				{
					return states;
				}
				else 
				{
					return null;
				}
			}
			else 
			{ 
				// Save only the dirty items.
				ArrayList indices = new ArrayList();
				ArrayList states = new ArrayList();
             
				for (int i = 0; i < Count; i++) 
				{
					DateCollectionItem dateItem = (DateCollectionItem)_dateCollection[i];
					object state = ((IStateManager)dateItem).SaveViewState();
					if (state != null) 
					{
						states.Add(state);
						indices.Add(i);
					}
				}

				if (indices.Count > 0) 
				{
					return new Pair(indices, states);
				}
			}

			return null;
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState == null) 
			{
				return;
			}

			if (savedState is Pair) 
			{
				Pair p = (Pair)savedState;
				ArrayList indices = (ArrayList)p.First;
				ArrayList states = (ArrayList)p.Second; 

				for (int i = 0 ;  i < indices.Count; i++) 
				{
					int index = (int)indices[i];
					if (index < this.Count) 
					{
						((IStateManager)_dateCollection[index]).LoadViewState(states[i]);
					}
					else 
					{
						DateCollectionItem dateItem = new DateCollectionItem();
						Add(dateItem);
						((IStateManager)dateItem).LoadViewState(states[i]);
					}
				}
			}
			else if (savedState is ArrayList)
			{
				
				_saveAll = true;
				ArrayList states = (ArrayList)savedState;

				_dateCollection = new ArrayList(states.Count);
				for (int i = 0; i < states.Count; i++) 
				{
					DateCollectionItem dateItem = new DateCollectionItem();
					Add(dateItem);
					((IStateManager)dateItem).LoadViewState(states[i]);
				}

			}
		}

		#endregion
	}

	#endregion
}