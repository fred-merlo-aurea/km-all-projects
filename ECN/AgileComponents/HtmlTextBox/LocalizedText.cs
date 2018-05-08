using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="LocalizedText"/> object.
	/// </summary>
	public class LocalizedText
	{
		private string _id;
		private string _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizedText"/> class.
		/// </summary>
		public LocalizedText()
		{
			_Init(string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizedText"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public LocalizedText(string id)
		{
			_Init(id,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizedText"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="val">The val.</param>
		public LocalizedText(string id, string val)
		{
			_Init(id,val);
		}

		private void _Init(string id, string val)
		{
			_id = id;
			_value = val;
		}

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public string Id
		{
			get {return _id;}
			set {_id = value;}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get {return _value;}
			set {_value = value;}
		}
	}
}
