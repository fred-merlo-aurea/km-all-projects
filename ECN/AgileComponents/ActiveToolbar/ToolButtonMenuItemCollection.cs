using System;
using System.Collections;
using System.Web.UI;
using System.ComponentModel;

namespace ActiveUp.WebControls
{
	#region class ToolButtonMenuItem

	/// <summary>
	/// Collection of <see cref="ToolButtonMenuItem"/>
	/// </summary>
	[Serializable]
	public class ToolButtonMenuItemCollection : IList, IStateManager
	{
		#region Variables

		/// <summary>
		/// ArrayList of <see cref="ToolButtonMenuItem"/>.
		/// </summary>
		private ArrayList _items;
		private bool _isTrackingViewState;
		private bool _saveAll;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolButtonMenuItemCollection()
		{
			_items = new ArrayList();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a <see cref="ToolButtonMenuItem"/> at the specified index.
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

				if (!(value is ToolButtonMenuItem))
					throw new ArgumentException("value must be a ToolButtonMenuItem.");

				_items[index] = (ToolButtonMenuItem)value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="ToolButtonMenuItem"/> at the specified index.
		/// </summary>		
		public ToolButtonMenuItem this[int index]
		{
			get {return (ToolButtonMenuItem)_items[index];}
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
		/// Removes the first occurrence of a specific <see cref="ToolButtonMenuItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to remove from the collection.</param>
		void IList.Remove(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolButtonMenuItem)) 
			{
				throw new ArgumentException("item must be a ToolButtonMenuItem");
			}

			Remove((ToolButtonMenuItem)item);

			if (_isTrackingViewState)
			{
				_saveAll = true;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="ToolButtonMenuItem"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to remove from the collection.</param>
		public void Remove(ToolButtonMenuItem item) 
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
		/// Inserts a <see cref="ToolButtonMenuItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to insert into the Collection.</param>
		void IList.Insert(int index, object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolButtonMenuItem)) 
			{
				throw new ArgumentException("item must be a ToolButtonMenuItem");
			}

			Insert(index, (ToolButtonMenuItem)item);

			if (_isTrackingViewState)
			{
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Inserts a <see cref="ToolButtonMenuItem"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to insert into the Collection.</param>
		public void Insert(int index, ToolButtonMenuItem item)
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
		/// Adds a <see cref="ToolButtonMenuItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolButtonMenuItem)) 
			{
				throw new ArgumentException("item must be a ToolButtonMenuItem");
			}

			return Add((ToolButtonMenuItem)item);
		}

		/// <summary>
		/// Adds a <see cref="ToolButtonMenuItem"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(ToolButtonMenuItem item)
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
		/// Removes all <see cref="ToolButtonMenuItem"/> from the collection.
		/// </summary>
		void IList.Clear() 
		{
			Clear();
		}

		/// <summary>
		/// Removes all <see cref="ToolButtonMenuItem"/> from the collection.
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
		/// Determines whether a <see cref="ToolButtonMenuItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		bool IList.Contains(object item) 
		{
			return Contains(item as ToolButtonMenuItem);
		}

		/// <summary>
		/// Determines whether a <see cref="ToolButtonMenuItem"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(ToolButtonMenuItem item) 
		{
			if (item == null) 
			{
				return false;
			}

			return _items.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="ToolButtonMenuItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		int IList.IndexOf(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolButtonMenuItem)) 
			{
				throw new ArgumentException("item must be a Toolbar.");
			}

			return IndexOf((ToolButtonMenuItem)item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="ToolButtonMenuItem"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="ToolButtonMenuItem"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(ToolButtonMenuItem item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}

			return _items.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="ToolButtonMenuItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="ToolButtonMenuItem"/> to remove.</param>
		void IList.RemoveAt(int index) 
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Removes the <see cref="ToolButtonMenuItem"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="ToolButtonMenuItem"/> to remove.</param>
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
		
		#endregion

		#region ViewState

		bool IStateManager.IsTrackingViewState
		{
			get {return _isTrackingViewState;}
		}

		/// <summary>
		/// When implemented by a class, instructs the server control to track changes to its view state.
		/// </summary>
		void IStateManager.TrackViewState()
		{
			_isTrackingViewState = true;
			foreach(ToolButtonMenuItem t in _items)
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
					ToolButtonMenuItem toolButtonMenuItem = (ToolButtonMenuItem)_items[i];
					toolButtonMenuItem.SetDirty();
					states.Add(((IStateManager)toolButtonMenuItem).SaveViewState());
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
					ToolButtonMenuItem toolButtonMenuItem = (ToolButtonMenuItem)_items[i];
					object state = ((IStateManager)toolButtonMenuItem).SaveViewState();
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
						ToolButtonMenuItem toolButtonMenuItem = new ToolButtonMenuItem();
						Add(toolButtonMenuItem);
						((IStateManager)toolButtonMenuItem).LoadViewState(states[i]);
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
					ToolButtonMenuItem toolButtonMenuItem = new ToolButtonMenuItem();
					Add(toolButtonMenuItem);
					((IStateManager)toolButtonMenuItem).LoadViewState(states[i]);
				}

			}
		}

		#endregion

	}

	#endregion

}

