using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="PagerElementCollection"/> object.
	/// </summary>
	[Serializable]
	public class PagerElementCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PagerElementCollection"/> class.
		/// </summary>
		public PagerElementCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		public void Add(PagerElement element)
		{
			if (this.Contains(element.Key))
				throw new Exception("The PagerElement key must be unique.");
			List.Add(element);
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="label">The label.</param>
		public void Add(string key, string label)
		{
			List.Add(new PagerElement(key, label));
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Add(string key)
		{
			List.Add(new PagerElement(key, key));
		}
	
		/// <summary>
		/// Removes at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void Remove(int index)
		{
			// Checks to see if there is an element at the supplied index.
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index); 
			}
		}

		/// <summary>
		/// Gets the <see cref="PagerElement"/> at the specified index.
		/// </summary>
		/// <value></value>
		public PagerElement this[int index]
		{
			get
			{
				return (PagerElement) List[index];
			}
		}

		/// <summary>
		/// Gets the <see cref="PagerElement"/> with the specified key.
		/// </summary>
		/// <value></value>
		public PagerElement this[string key]
		{
			get
			{
				foreach(PagerElement element in this.List)
					if (element.Key == key)
						return element;

				return null;
			}
		}

		/// <summary>
		/// Gets the index from key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public int GetIndexFromKey(string key)
		{
			int index = 0;
			for(index=0;index<this.Count;index++)
			{
				if (this[index].Key == key)
					return index;
			}

			return -1;
		}

		/// <summary>
		/// Determines whether contains the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if contains the specified key; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string key) 
		{
			int index = 0;
			for(index=0;index<this.Count;index++)
			{
				if (this[index].Key == key)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Gets the labels.
		/// </summary>
		/// <value>The labels.</value>
		public string[] Labels
		{
			get
			{
				string[] labels = new string[this.Count];

				for(int index=0;index<this.Count;index++)
					labels[index] = this[index].Label;
				
				return labels;
			}
		}

		/// <summary>
		/// Gets the keys.
		/// </summary>
		/// <value>The keys.</value>
		public string[] Keys
		{
			get
			{
				string[] keys = new string[this.Count];

				for(int index=0;index<this.Count;index++)
					keys[index] = this[index].Key;
				
				return keys;
			}
		}
	}
}
