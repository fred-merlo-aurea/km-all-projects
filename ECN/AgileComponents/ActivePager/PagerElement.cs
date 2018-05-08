using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="PagerElement"/> object.
	/// </summary>
	[
	Serializable
	]
	public class PagerElement
	{
		private string _key, _label;

		/// <summary>
		/// Initializes a new instance of the <see cref="PagerElement"/> class.
		/// </summary>
		public PagerElement()
		{
			_key = string.Empty;
			_label = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagerElement"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="label">The label.</param>
		public PagerElement(string key, string label)
		{
			_key = key;
			_label = label;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagerElement"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		public PagerElement(string key)
		{
			_key = key;
			_label = key;
		}

		/// <summary>
		/// Gets or sets the element label (description).
		/// </summary>
		public string Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		/// <summary>
		/// Gets or sets the element key.
		/// </summary>
		public string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}
	}
}
