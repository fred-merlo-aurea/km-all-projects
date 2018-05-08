using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using AttributeCollection = System.Web.UI.AttributeCollection;
using System.Drawing.Design;

namespace ActiveUp.WebControls
{
	#region class ToolCollection

	/// <summary>
	/// A Collection of <see cref="ToolBase"/>.
	/// </summary>
	[
		Editor(typeof(ToolCollectionEditor), typeof(UITypeEditor)),
		Serializable 
	]
	public class ToolCollection : ICollection, IEnumerable, IList
	{
		#region Variables

		/// <summary>
		/// ArrayList of <see cref="ToolBase"/>.
		/// </summary>
		private ArrayList _tools;
		private ControlCollection _controls;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ToolCollection(ControlCollection controls)
		{
			_tools = new ArrayList();
			_controls = controls;
		}

		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets a <see cref="ToolBase"/> at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get {return (ToolBase)_tools[index];}
			set 
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (!(value is Toolbar))
					throw new ArgumentException("value must be a Toolbar.");

				_tools[index] = (ToolBase)value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="ToolBase"/> at the specified index.
		/// </summary>		
		public ToolBase this[int index]
		{
			get {return (ToolBase)_tools[index];}
		}

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count
		{
			get {return _tools.Count;}
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
			get {return _tools.SyncRoot;}
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
		/// Removes the first occurrence of a specific <see cref="ToolBase"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to remove from the collection.</param>
		void IList.Remove(object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolBase)) 
			{
				throw new ArgumentException("item must be a ToolBase");
			}

			_tools.Remove(item);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="ToolBase"/> from the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to remove from the collection.</param>
		public void Remove(ToolBase item) 
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
		/// Inserts a <see cref="Toolbar"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="Toolbar"/> to insert into the Collection.</param>		
		void IList.Insert(int index, object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolBase)) 
			{
				throw new ArgumentException("item must be a ToolBase.");
			}

			Insert(index, (ToolBase)item);
		}

		/// <summary>
		/// Inserts a <see cref="ToolBase"/> to the collection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="item">The <see cref="ToolBase"/> to insert into the Collection.</param>
		public void Insert(int index, ToolBase item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			
			_tools.Insert(index,item);
		}

		/// <summary>
		/// Adds a <see cref="ToolBase"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object item)
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is ToolBase)) 
			{
				throw new ArgumentException("item must be a ToolBase.");
			}

			return Add((ToolBase)item);
		}

		/// <summary>
		/// Adds a <see cref="ToolBase"/> to the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(ToolBase item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			_controls.Add(item);
			return _tools.Add(item);
		}

		/// <summary>
		/// Removes all <see cref="ToolBase"/> from the collection.
		/// </summary>
		void IList.Clear() 
		{
			Clear();
		}

		/// <summary>
		/// Removes all <see cref="ToolBase"/> from the collection.
		/// </summary>
		public void Clear()
		{
			_tools.Clear();
		}

		/// <summary>
		/// Determines whether a <see cref="ToolBase"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		bool IList.Contains(object item) 
		{
			return Contains(item as ToolBase);
		}
 
		/// <summary>
		/// Determines whether a <see cref="ToolBase"/> is in the collection.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to locate in the collection. The element to locate can be a null reference.</param>
		/// <returns>true if value is found in the collection; otherwise, false.</returns>
		public bool Contains(ToolBase item) 
		{
			if (item == null) 
			{
				return false;
			}
			return _tools.Contains(item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="ToolBase"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		int IList.IndexOf(object item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			if (!(item is Toolbar)) 
			{
				throw new ArgumentException("item must be a ToolBase.");
			}

			return IndexOf((ToolBase)item);
		}

		/// <summary>
		/// Determines the index of a specific <see cref="ToolBase"/> in the current instance.
		/// </summary>
		/// <param name="item">The <see cref="ToolBase"/> to locate in the current instance.</param>
		/// <returns>The index of value if found in the current instance; otherwise, -1.</returns>
		public int IndexOf(ToolBase item) 
		{
			if (item == null) 
			{
				throw new ArgumentNullException("item");
			}
			return _tools.IndexOf(item);
		}

		/// <summary>
		/// Removes the <see cref="ToolBase"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="ToolBase"/> to remove.</param>
		void IList.RemoveAt(int index) 
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Removes the <see cref="ToolBase"/> at the specified index within the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="ToolBase"/> to remove.</param>
		public void RemoveAt(int index)
		{
			_tools.Remove(index);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return _tools.GetEnumerator();
		}

		/// <summary>
		/// Copies the elements from the current instance to the specified collection, starting at the specified index in the array.
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance. </param>
		/// <param name="index">A Int32 that specifies the zero-based index in array at which copying begins.</param>
		public void CopyTo(Array array, int index)
		{
			_tools.CopyTo(array);
		}

		#endregion
	}

	#endregion
}
