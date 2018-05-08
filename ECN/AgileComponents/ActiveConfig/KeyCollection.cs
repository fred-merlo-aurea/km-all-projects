using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="KeyCollection"/> object.
	/// </summary>
	[Serializable]
	public class KeyCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyCollection"/> class.
		/// </summary>
		public KeyCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Add(Key key)
		{
			List.Add(key);
		}

		/// <summary>
		/// Adds the specified key name.
		/// </summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="keyValue">The key value.</param>
		public void Add(string keyName, string keyValue)
		{
			List.Add(new Key(keyName, keyValue));
		}

		/// <summary>
		/// Removes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void Remove(int index)
		{
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index); 
			}
		}

		/// <summary>
		/// Removes the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		public void Remove(string name)
		{
			for(int index = 0;index<this.Count;index++)
				if (this[index].Name == name)
					this.Remove(index);
		}

		/// <summary>
		/// Gets the <see cref="Key"/> at the specified index.
		/// </summary>
		/// <value></value>
		public Key this[int index]
		{
			get
			{
				return (Key) List[index];
			}
		}	

		/// <summary>
		/// Gets the <see cref="Key"/> with the specified name.
		/// </summary>
		/// <value></value>
		public Key this[string name]
		{
			get
			{
				name = name.ToUpper();

				foreach(Key key in List)
				{
					if (key.Name.ToUpper() == name)
						return key;
				}

				throw new Exception("Specified Key doen't belong to this collection.");
				//return null;
			}
		}

		/// <summary>
		/// Determines whether [contains] [the specified name].
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name)
		{
			foreach(Key key in this.List)
				if (key.Name.ToUpper() == name.ToUpper())
					return true;

			return false;
		}
	}
}
