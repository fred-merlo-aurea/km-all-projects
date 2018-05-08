using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="LocalizationSettings"/> object.
	/// </summary>
	public class LocalizationSettings
	{
		private string _name;
		private string _code;
		private string _compatibleVersion;
		
		private LocalizedTexts _texts = new LocalizedTexts();

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationSettings"/> class.
		/// </summary>
		public LocalizationSettings()
		{
			_Init(string.Empty,string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationSettings"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public LocalizationSettings(string name)
		{
			_Init(name,string.Empty,string.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationSettings"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="code">The code.</param>
		public LocalizationSettings(string name, string code)
		{
			_Init(name,code,string.Empty);
		}	

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationSettings"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="code">The code.</param>
		/// <param name="compatibleVersion">The compatible version.</param>
		public LocalizationSettings(string name, string code, string compatibleVersion)
		{
			_Init(name,code,compatibleVersion);
		}

		private void _Init(string name, string code, string compatibleVersion)
		{
			_name = name;
			_code = code;
			_compatibleVersion = compatibleVersion;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get {return _name;}
			set {_name = value;}
		}

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
		public string Code
		{
			get {return _code;}
			set {_code = value;}
		}

		/// <summary>
		/// Gets or sets the compatible version.
		/// </summary>
		/// <value>The compatible version.</value>
		public string CompatibleVersion
		{
			get {return _compatibleVersion;}
			set {_compatibleVersion = value;}
		}

		/// <summary>
		/// Gets the texts.
		/// </summary>
		/// <value>The texts.</value>
		public LocalizedTexts Texts
		{
			get {return _texts;}
		}
	}
}
