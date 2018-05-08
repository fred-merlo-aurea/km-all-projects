using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;

namespace ActiveUp.WebControls
{
    #region MenuItemCollection

    /// <summary>
    /// A Collection of <see cref="MenuItemCollection"/>.
    /// </summary>
    [
        Serializable
    ]
    public class MenuItemCollection : ICollection, IEnumerable, IList, IStateManager
    {
        #region Variables

        /// <summary>
        /// A collection of ToolMenuItem.
        /// </summary>
        private ArrayList _toolMenuItemCollection;

        private bool _isTrackingViewState;
        private bool _saveAll;

        private ControlCollection _controls;

        #endregion

		#region Constructor

        /// <summary>
        /// The default constructor.
        /// </summary>
        public MenuItemCollection(ControlCollection controls)
        {
            _toolMenuItemCollection = new ArrayList();
            _controls = controls;
        }

		#endregion

        /// <summary>
        /// Gets or sets a <see cref="MenuItemCollection"/> at the specified index.
        /// </summary>
        object IList.this[int index]
        {
            get
            {
                return (MenuItem)_toolMenuItemCollection[index];
            }
            set
            {
                _toolMenuItemCollection[index] = (MenuItem)value;
            }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _toolMenuItemCollection.Count;
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
                return _toolMenuItemCollection.IsFixedSize;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _toolMenuItemCollection.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return _toolMenuItemCollection.SyncRoot;
            }
        }

        /// <summary>
        /// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
        /// </summary>
        /// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
        /// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            _toolMenuItemCollection.CopyTo(array, index);
        }

        /// <summary>
        /// Gets or sets a <see cref="MenuItem"/> at the specified index.
        /// </summary>
        public virtual MenuItem this[int index]
        {
            get
            {
                return (MenuItem)_toolMenuItemCollection[index];
            }
            set
            {
                _toolMenuItemCollection[index] = value;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="MenuItem"/> from the collection.
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to remove from the collection.</param>
        public void Remove(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is MenuItem))
            {
                throw new ArgumentException("item must be a MenuItem");
            }

            Remove((MenuItem)item);

            if (_isTrackingViewState)
            {
                _saveAll = true;
            }

            _controls.Remove((MenuItem)item);
        }

        /// <summary>
        /// Removes a <see cref="MenuItem"/> from the collection.
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to remove from the collection.</param>
        public void Remove(MenuItem item)
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
        /// Inserts a <see cref="MenuItem"/> to the collection at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="item">The <see cref="MenuItem"/> to insert into the Collection.</param>
        public void Insert(int index, object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is MenuItem))
            {
                throw new ArgumentException("Item must be a MenuItem");
            }

            Insert(index, (MenuItem)item);

            if (_isTrackingViewState)
            {
                ((IStateManager)item).TrackViewState();
                _saveAll = true;
            }
        }

        /// <summary>
        /// Inserts a <see cref="MenuItem"/> to the collection at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="item">The <see cref="MenuItem"/> to insert into the Collection.</param>
        public void Insert(int index, MenuItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _toolMenuItemCollection.Insert(index, item);

            if (_isTrackingViewState)
            {
                ((IStateManager)item).TrackViewState();
                _saveAll = true;
            }

            _controls.Add(item);
        }

        /// <summary>
        /// Adds a <see cref="MenuItem"/> to the collection.
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to add to the collection.</param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!(item is MenuItem))
            {
                throw new ArgumentException("Item must be a MenuItem");
            }

            return Add((MenuItem)item);
        }

        /// <summary>
        /// Adds a <see cref="MenuItem"/> to the collection
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to add to the collection.</param>
        /// <returns>The position into which the new element was inserted.</returns>
        public int Add(MenuItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (_isTrackingViewState)
            {
                ((IStateManager)item).TrackViewState();
                item.SetDirty();
            }

            _controls.Add(item);
            return _toolMenuItemCollection.Add(item);
        }

        /// <summary>
        /// Removes all <see cref="MenuItem"/> from the collection.
        /// </summary>
        public void Clear()
        {
            _toolMenuItemCollection.Clear();

            if (_isTrackingViewState)
            {
                _saveAll = true;
            }
        }

        /// <summary>
        /// Determines whether a <see cref="MenuItem"/> is in the collection.
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to locate in the collection. The element to locate can be a null reference.</param>
        /// <returns>true if value is found in the collection; otherwise, false.</returns>
        public bool Contains(object item)
        {
            if (item == null)
                return false;

            if (item is MenuItem == false)
                throw new InvalidCastException("Item must be a MenuItem object");

            return _toolMenuItemCollection.Contains(item);
        }

        /// <summary>
        /// Determines whether a <see cref="MenuItem"/> is in the collection.
        /// </summary>
        /// <param name="item">The <see cref="MenuItem"/> to locate in the collection. The element to locate can be a null reference.</param>
        /// <returns>true if value is found in the collection; otherwise, false.</returns>
        public bool Contains(MenuItem item)
        {
            if (item == null)
                return false;

            return _toolMenuItemCollection.Contains(item);
        }

        /// <summary>
        /// Determines the index of a specific <see cref="MenuItemCollection"/> in the current instance.
        /// </summary>
        /// <param name="item">The <see cref="MenuItemCollection"/> to locate in the current instance.</param>
        /// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
        public int IndexOf(object item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item is MenuItemCollection == false)
                throw new InvalidCastException("Item must be a MenuItemCollection object");

            return _toolMenuItemCollection.IndexOf(item);
        }

        /// <summary>
        /// Determines the index of a specific <see cref="MenuItemCollection"/> in the current instance.
        /// </summary>
        /// <param name="item">The <see cref="MenuItemCollection"/> to locate in the current instance.</param>
        /// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
        public int IndexOf(MenuItemCollection item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return _toolMenuItemCollection.IndexOf(item);
        }

        /// <summary>
        /// Removes the <see cref="MenuItemCollection"/> at the specified index within the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="MenuItemCollection"/> to remove.</param>
        public void RemoveAt(int index)
        {
            _toolMenuItemCollection.RemoveAt(index);

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
            return _toolMenuItemCollection.GetEnumerator();
        }

        #region ViewState

        bool IStateManager.IsTrackingViewState
        {
            get { return _isTrackingViewState; }
        }

        void IStateManager.TrackViewState()
        {
            _isTrackingViewState = true;
            foreach (MenuItem item in _toolMenuItemCollection)
                ((IStateManager)item).TrackViewState();
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
                    MenuItem toolMenuItem = (MenuItem)_toolMenuItemCollection[i];
                    toolMenuItem.SetDirty();
                    states.Add(((IStateManager)toolMenuItem).SaveViewState());
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
                    MenuItem toolMenuItem = (MenuItem)_toolMenuItemCollection[i];
                    object state = ((IStateManager)toolMenuItem).SaveViewState();
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

                for (int i = 0; i < indices.Count; i++)
                {
                    int index = (int)indices[i];
                    if (index < this.Count)
                    {
                        ((IStateManager)_toolMenuItemCollection[index]).LoadViewState(states[i]);
                    }
                    else
                    {
                        MenuItem toolMenuItem = new MenuItem();
                        Add(toolMenuItem);
                        ((IStateManager)toolMenuItem).LoadViewState(states[i]);
                    }
                }
            }
            else if (savedState is ArrayList)
            {

                _saveAll = true;
                ArrayList states = (ArrayList)savedState;

                _toolMenuItemCollection = new ArrayList(states.Count);
                for (int i = 0; i < states.Count; i++)
                {
                    MenuItem toolMenuItem = new MenuItem();
                    Add(toolMenuItem);
                    ((IStateManager)toolMenuItem).LoadViewState(states[i]);
                }
            }
        }

        #endregion
    }

    #endregion
}