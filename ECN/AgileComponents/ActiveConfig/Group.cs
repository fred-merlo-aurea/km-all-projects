using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Group"/> object.
	/// </summary>
	[Serializable]
	public class Group
	{
		string _name;
		KeyCollection _keys;

		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class.
		/// </summary>
		public Group()
		{
			_name = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public Group(string name)
		{
			_name = name;
		}

		/// <summary>
		/// Gets or sets the keys.
		/// </summary>
		/// <value>The keys.</value>
		public KeyCollection Keys
		{
			get
			{
				if (_keys == null)
					_keys = new KeyCollection();
				return _keys;
			}
			set
			{
				_keys = value;
			}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
	}
}
