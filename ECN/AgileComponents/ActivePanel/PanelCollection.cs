using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;

namespace ActiveUp.WebControls
{
	#region PanelCollection

	/// <summary>
	/// Collection of <see cref="Panel"/>
	/// </summary>
	[Serializable]
	public class PanelCollection : ICollection, IEnumerable, IList
	{
		#region Fields

		/// <summary>
		/// ArrayList of <see cref="Panel"/>.
		/// </summary>
		private ArrayList _panels;

		private ControlCollection _controls;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a collection of <see cref="Panel"/> by specifying the collections of controls.
		/// </summary>
		public PanelCollection(ControlCollection controls)
		{
			_panels = new ArrayList();
			_controls = controls;
		}

		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets a <see cref="Panel"/> at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get {return (Panel)_panels[index];}
			set 
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (!(value is Panel))
					throw new ArgumentException("value must be a Panel.");

				_panels[index] = (Panel)value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="Panel"/> at the specified index.
		/// </summary>		
		public Panel this[int index]
		{
			get {return (Panel)_panels[index];}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get {return _panels.Count;}
		}

		/// <summary>
		/// Gets a value indicating whether access to the collection is synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get {return true;}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot
		{
			get {return _panels.SyncRoot;}
		}

		/// <summary>
		/// Gets a boolean value indicating whether the current instance has a fixed size.
		/// </summary>
		public bool IsFixedSize
		{
			get {return false;}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get {return false;}
		}


		#endregion

		#region Methods

		/// <summary>	
		/// Removes the first occurrence of a specific <see cref="Panel"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to remove from the collection.</param>
		void IList.Remove(object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is Panel)) 
			{
				throw new ArgumentException("item must be a Panel");
			}

			_panels.Remove(item);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="Panel"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to remove from the collection.</param>
		public void Remove(Panel item) 
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
			_controls.Remove(item);
		}

		/// <summary>
		/// Inserts a <see cref="Panel"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="Panel"/> to insert into the Collection.</param>		
		void IList.Insert(int index, object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is Panel)) 
			{
				throw new ArgumentException("item must be a Panel");
			}

			Insert(index, (Panel)item);
		}

		/// <summary>
		/// Inserts a <see cref="Panel"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="Panel"/> to insert into the Collection.</param>		
		public void Insert(int index, Panel item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			
			_panels.Insert(index,item);

		}

		/// <summary>
		/// Adds a <see cref="Panel"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is Panel)) 
			{
				throw new ArgumentException("item must be a Panel");
			}

			return Add((Panel)item);
		}

		/// <summary>
		/// Adds a <see cref="Panel"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(Panel item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			
			_controls.Add(item);
			return _panels.Add(item);

		}

		/// <summary>
		/// Removes all <see cref="Panel"/> from the collection.
		/// </summary>
		void IList.Clear() 
		{
			Clear();
		}

		/// <summary>
		/// Removes all <see cref="Panel"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_panels.Clear();
		}

		/// <summary>
		/// Determines whether a <see cref="Panel"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		bool IList.Contains(object item) 
		{
			return Contains(item as Panel);
		}

		/// <summary>
		/// Determines whether a <see cref="Panel"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(Panel item) 
		{
			if (item == null) 
			{
				return false;
			}
			return _panels.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="Panel"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		int IList.IndexOf(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is Panel)) 
			{
				throw new ArgumentException("item must be a Panel");
			}

			return IndexOf((Panel)item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="Panel"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="Panel"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(Panel item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			return _panels.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="Panel"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="Panel"/> to remove.</param>
		void IList.RemoveAt(int index) 
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Removes the <see cref="Panel"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="Panel"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_panels.Remove(index);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _panels.GetEnumerator();
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo(Array array, int index)
		{
			_panels.CopyTo(array);
		}

		#endregion

	}

	#endregion
}
