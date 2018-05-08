// ActiveInput
// Copyright (c) 2005 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.Web.UI;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region class OrderedListBoxItemCollection

	/// <summary>
	/// Collection of <see cref="OrderedListBoxItem"/>
	/// </summary>
	[Serializable]
	public class OrderedListBoxItemCollection : IList, IStateManager
	{
		#region Variables

		/// <summary>
		/// ArrayList of <see cref="OrderedListBoxItem"/>.
		/// </summary>
		private ArrayList _items;
		private bool _isTrackingViewState;
		private bool _saveAll;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public OrderedListBoxItemCollection()
		{
			_items = new ArrayList();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a <see cref="OrderedListBoxItem"/> at the specified index.
		/// </summary>		
		object IList.this[int index] 
		{
			get 
			{
				return _items[index];
			}
			set 
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_items[index] = (OrderedListBoxItem)value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="OrderedListBoxItem"/> at the specified index.
		/// </summary>		
		public OrderedListBoxItem this[int index]
		{
			get {return (OrderedListBoxItem)_items[index];}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count 
		{
			get 
			{
				return _items.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the collection is synchronized.
		/// </summary>
		public bool IsSynchronized 
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot 
		{
			get 
			{
				return this;
			}
		}

		/// <summary>
		/// Gets a boolean value indicating whether the current instance has a fixed size.
		/// </summary>
		public bool IsFixedSize 
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance is read-only.
		/// </summary>
		public bool IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		#endregion

		#region Functions

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="OrderedListBoxItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to remove from the collection.</param>
		void IList.Remove(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is OrderedListBoxItem)) 
			{
				throw new ArgumentException("item must be a OrderedListBoxItem");
			}

			Remove((OrderedListBoxItem)item);

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="OrderedListBoxItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to remove from the collection.</param>
		public void Remove(OrderedListBoxItem item) 
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
		/// Inserts a <see cref="OrderedListBoxItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to insert into the Collection.</param>
		void IList.Insert(int index, object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is OrderedListBoxItem)) 
			{
				throw new ArgumentException("item must be a OrderedListBoxItem");
			}

			Insert(index, (OrderedListBoxItem)item);

			if (_isTrackingViewState)
			{
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Inserts a <see cref="OrderedListBoxItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to insert into the Collection.</param>
		public void Insert(int index, OrderedListBoxItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			_items.Insert(index,item);

			if (_isTrackingViewState) 
			{
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Adds a <see cref="OrderedListBoxItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is OrderedListBoxItem)) 
			{
				throw new ArgumentException("item must be a OrderedListBoxItem");
			}

			return Add((OrderedListBoxItem)item);
		}

		/// <summary>
		/// Adds a <see cref="OrderedListBoxItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(OrderedListBoxItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (_isTrackingViewState) 
			{
				((IStateManager)item).TrackViewState();
				item.SetDirty();
			}

			return _items.Add(item);
		}

		/// <summary>
		/// Removes all <see cref="OrderedListBoxItem"/> from the collection.
		/// </summary>
		void IList.Clear() 
		{
			Clear();
		}

		/// <summary>
		/// Removes all <see cref="OrderedListBoxItem"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_items.Clear();

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Determines whether a <see cref="OrderedListBoxItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		bool IList.Contains(object item) 
		{
			return Contains(item as OrderedListBoxItem);
		}

		/// <summary>
		/// Determines whether a <see cref="OrderedListBoxItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(OrderedListBoxItem item) 
		{
			if (item == null) 
			{
				return false;
			}

			return _items.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="OrderedListBoxItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		int IList.IndexOf(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is OrderedListBoxItem)) 
			{
				throw new ArgumentException("item must be a Toolbar.");
			}

			return IndexOf((OrderedListBoxItem)item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="OrderedListBoxItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="OrderedListBoxItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(OrderedListBoxItem item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}

			return _items.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="OrderedListBoxItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="OrderedListBoxItem"/> to remove.</param>
		void IList.RemoveAt(int index) 
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Removes the <see cref="OrderedListBoxItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="OrderedListBoxItem"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_items.RemoveAt(index);

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public IEnumerator GetEnumerator() 
		{
			return _items.GetEnumerator();
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo(Array array, int index) 
		{
			_items.CopyTo(array,index);
		}

		/// <summary>
		/// Gets the order string.
		/// </summary>
		/// <returns>The order sting.</returns>
		public string GetOrderString()
		{
			string order = string.Empty;

			foreach(OrderedListBoxItem item in this._items)
				order += item.Value + "~" + item.Text + "~" + item.Selected.ToString().ToLower() + "|";

			return order.TrimEnd('|');
		}

		/// <summary>
		/// Sets the order string of each items..
		/// </summary>
		/// <param name="order">The order.</param>
		/// <param name="enumerate">if set to <c>true</c> enumerated each item.</param>
		public void SetOrderString(string order, bool enumerate)
		{
			this._items.Clear();

			System.Web.HttpContext.Current.Trace.Write(order);

			foreach(string item in order.Split('|'))
			{
				OrderedListBoxItem newItem = new OrderedListBoxItem();
				newItem.Value = item.Split('~')[0];

				System.Web.HttpContext.Current.Trace.Write(item.Split('~')[0]);
				System.Web.HttpContext.Current.Trace.Write(item.Split('~')[1]);
				System.Web.HttpContext.Current.Trace.Write(item.Split('~')[2]);

				string itemText = item.Split('~')[1];
				if (enumerate)
					newItem.Text = itemText.Substring(itemText.IndexOf(". ") + 1);
				else
					newItem.Text = itemText;
				newItem.Selected = Convert.ToBoolean(item.Split('~')[2]);
				this._items.Add(newItem);
			}
		}

		/// <summary>
		/// Move up an item at the start index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		public void MoveUp(int startIndex)
		{
			if (startIndex > 0)
			{
				startIndex--;
				this._items.Reverse(startIndex, 2);
			}
		}

		/// <summary>
		/// Moves the down an item at the start index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		public void MoveDown(int startIndex)
		{
			if (startIndex < _items.Count-1)
			{
				this._items.Reverse(startIndex, 2);
			}
		}

		/// <summary>
		/// Move the selected item up.
		/// </summary>
		public void MoveSelectedUp()
		{
			int index = 0;
			for(index=0;index<_items.Count;index++)
			{
				OrderedListBoxItem item = (OrderedListBoxItem)_items[index];

				if (item.Selected && !item.Locked)
					MoveUp(index);
			}
		}

		/// <summary>
		/// Move the selected item down.
		/// </summary>
		public void MoveSelectedDown()
		{
			int index = 0;
			for(index=_items.Count-1;index>=0;index--)
			{
				OrderedListBoxItem item = (OrderedListBoxItem)_items[index];

				if (item.Selected && !item.Locked)
					MoveDown(index);
			}
		}

		/// <summary>
		/// Moves the selected item to list box.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="control">The control id.</param>
		public void MoveSelectedToListBox(System.Web.UI.Page page, string control)
		{
			// Get left listbox
			OrderedListBox listbox = (OrderedListBox)page.FindControl(control);

			int index = 0;
			for(index=_items.Count-1;index>=0;index--)
			{
				OrderedListBoxItem item = (OrderedListBoxItem)_items[index];

				if (item.Selected && !item.Locked)
				{
					listbox.Items.Add(item);
					_items.RemoveAt(index);
				}
			}
		}
		#endregion

		#region ViewState

		bool IStateManager.IsTrackingViewState
		{
			get {return _isTrackingViewState;}
		}

		void IStateManager.TrackViewState()
		{
			_isTrackingViewState = true;
			foreach(OrderedListBoxItem t in _items)
				((IStateManager)t).TrackViewState();
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
					OrderedListBoxItem toolItem = (OrderedListBoxItem)_items[i];
					toolItem.SetDirty();
					states.Add(((IStateManager)toolItem).SaveViewState());
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
					OrderedListBoxItem toolItem = (OrderedListBoxItem)_items[i];
					object state = ((IStateManager)toolItem).SaveViewState();
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
						((IStateManager)_items[index]).LoadViewState(states[i]);
					}
					else 
					{
						OrderedListBoxItem toolItem = new OrderedListBoxItem();
						Add(toolItem);
						((IStateManager)toolItem).LoadViewState(states[i]);
					}
				}
			}
			else if (savedState is ArrayList)
			{
				
				_saveAll = true;
				ArrayList states = (ArrayList)savedState;

				_items = new ArrayList(states.Count);
				for (int i = 0; i < states.Count; i++) 
				{
					OrderedListBoxItem toolItem = new OrderedListBoxItem();
					Add(toolItem);
					((IStateManager)toolItem).LoadViewState(states[i]);
				}

			}
		}

		#endregion
	}
	#endregion

}


