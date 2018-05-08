using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="Key"/> object.
	/// </summary>
	[Serializable]
	public class Key
	{
		string _name, _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public Key()
		{
			_name = string.Empty;
			_value = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="keyValue">The key value.</param>
		public Key(string keyName, string keyValue)
		{
			_name = keyName;
			_value = keyValue;
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

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}
}
