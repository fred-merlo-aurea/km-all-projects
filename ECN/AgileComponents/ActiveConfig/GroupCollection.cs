using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="GroupCollection"/> object.
	/// </summary>
	[Serializable]
	public class GroupCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupCollection"/> class.
		/// </summary>
		public GroupCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds the specified group.
		/// </summary>
		/// <param name="group">The group.</param>
		public void Add(Group group)
		{
			List.Add(group);
		}

		/// <summary>
		/// Adds the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		public void Add(string name)
		{
			List.Add(new Group(name));
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
			for(int index=0;index<this.Count;index++)
				if (this[index].Name == name)
					this.Remove(index);
		}

		/// <summary>
		/// Gets the <see cref="Group"/> at the specified index.
		/// </summary>
		/// <value></value>
		public Group this[int index]
		{
			get
			{
				return (Group ) List[index];
			}
		}	

		/// <summary>
		/// Gets the <see cref="Group"/> with the specified name.
		/// </summary>
		/// <value></value>
		public Group this[string name]
		{
			get
			{
				foreach(Group group in List)
					if (group.Name.ToUpper() == name.ToUpper())
						return group;

				return null;
			}
		}

		/// <summary>
		/// Gets the names.
		/// </summary>
		/// <value>The names.</value>
		public string[] Names
		{
			get
			{
				string[] names = new string[this.List.Count];

				for(int index=0;index<this.List.Count;index++)
					names[index] = ((Group)this.List[index]).Name;
				
				return names;
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
			foreach(Group group in this.List)
			{
				name = name.ToUpper();

				if (group.Name.ToUpper() == name)
					return true;
			}
			return false;
		}
	}
}
