using System;
using System.IO;
using System.Text;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// INI file generator.
	/// </summary>
	[Serializable]
	public class IniConfig
	{
		private GroupCollection _groups;

		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		public IniConfig()
		{

		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public IniConfig(string filename)
		{
			this.ParseFile(filename);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		public IniConfig(string filename, bool htmlEncodeValues)
		{
			this.ParseFile(filename, htmlEncodeValues);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		public IniConfig(string filename, char separation, bool htmlEncodeValues)
		{
			this.ParseFile(filename, separation, htmlEncodeValues);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <param name="encoding">The encoding.</param>
		public IniConfig(string filename, char separation, bool htmlEncodeValues, System.Text.Encoding encoding)
		{
			this.ParseFile(filename, separation, htmlEncodeValues, encoding);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IniConfig"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <param name="encoding">The encoding.</param>
		public IniConfig(string filename, bool htmlEncodeValues, System.Text.Encoding encoding)
		{
			this.ParseFile(filename, '=', htmlEncodeValues, encoding);
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void SaveFile(string filename)
		{
			SaveFile(filename, false);
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		public void SaveFile(string filename, bool htmlEncodeValues)
		{
			SaveFile(filename, '=', htmlEncodeValues);
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		public void SaveFile(string filename, char separation, bool htmlEncodeValues)
		{
			SaveFile(filename, separation, htmlEncodeValues, System.Text.Encoding.UTF7);
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <param name="encoding">The encoding.</param>
		public void SaveFile(string filename, bool htmlEncodeValues, System.Text.Encoding encoding)
		{
			SaveFile(filename, '=', htmlEncodeValues, encoding);
		}

		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <param name="encoding">The encoding.</param>
		public void SaveFile(string filename, char separation, bool htmlEncodeValues, System.Text.Encoding encoding)
		{
			// Try to open the ini file. If it doen't exist, the file is created.
			FileStream iniFile = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);

			// Initialize the streamreader object to read the content of the file then go
			// to the begin of the file.
			StreamWriter fileWrite = new StreamWriter(filename, false, encoding);
			fileWrite.Write(this.GetIniContent());
			fileWrite.Close();
		}

		/// <summary>
		/// Parses the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		public bool ParseFile(string filename)
		{
			return ParseFile(filename, '=', false);
		}

		/// <summary>
		/// Parses the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <returns></returns>
		public bool ParseFile(string filename, bool htmlEncodeValues)
		{
			return ParseFile(filename, '=', htmlEncodeValues);
		}

		/// <summary>
		/// Parses the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <returns></returns>
		public bool ParseFile(string filename, char separation, bool htmlEncodeValues)
		{
			return ParseFile(filename, '=', htmlEncodeValues, System.Text.Encoding.UTF7);
		}

		/// <summary>
		/// Parses the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="separation">The separation.</param>
		/// <param name="htmlEncodeValues">if set to <c>true</c> [HTML encode values].</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public bool ParseFile(string filename, char separation, bool htmlEncodeValues, System.Text.Encoding encoding)
		{
			string buffer, lastGroup = string.Empty, groupName, keyName, keyValue;
			//string[] tempArray;

			// Try to open the ini file. If it doen't exist, the file is created.
			FileStream iniFile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);

			// Initialize the streamreader object to read the content of the file then go
			// to the begin of the file.
			StreamReader fileRead = new StreamReader(iniFile, encoding);
			fileRead.BaseStream.Seek(0, SeekOrigin.Begin);   
			
			// Now read all the file till the end.
			while (fileRead.Peek() > -1) 
			{
				// Gets the line into the buffer string.
				buffer = fileRead.ReadLine().Trim();
				
				// Test if the string contain a subcategory delimiter.
				if (buffer.StartsWith("["))
				{
					groupName = ParseGroup(buffer);
					// Create a new item in the reference list.				
					if (!this.Groups.Contains(groupName))
					{
						this.Groups.Add(groupName);
						lastGroup = groupName;
					}
				}
				// Test if the string actually contain something intersting.
				else if (buffer.Length > 1 && buffer.IndexOf(separation) > -1 && !buffer.StartsWith(";") && !buffer.StartsWith("'"))
				{
					// Split the string to get the Key and Value.
					keyName = buffer.Substring(0, buffer.IndexOf(separation));
					keyValue = buffer.Substring(buffer.IndexOf(separation)+1, buffer.Length - buffer.IndexOf(separation)-1);
					
					if (htmlEncodeValues)
						keyValue = HtmlElementEncoder.EncodeValue(keyValue);
					//tempArray = buffer.Split(separation);
					
					//if (tempArray.Length > 1 && this.Groups.Contains(lastGroup))
					if (keyName != string.Empty && this.Groups.Contains(lastGroup))
						this.Groups[lastGroup].Keys.Add(keyName, keyValue);
						//this.Groups[lastGroup].Keys.Add(tempArray[0], tempArray[1]);

				}
			}

			fileRead.Close();

			return true;
		}

		/// <summary>
		/// Parses the string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public bool ParseString(string input)
		{
			return ParseString(input, '=');
		}

		/// <summary>
		/// Parses the string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="separation">The separation.</param>
		/// <returns></returns>
		public bool ParseString(string input, char separation)
		{
			string buffer, lastGroup = string.Empty, groupName, keyName, keyValue;

			// Open the ini file
			StringReader iniString = new StringReader(input);

			// Now read all the file till the end.
			while (iniString.Peek() > -1) 
			{
				// Gets the line into the buffer string.
				buffer = iniString.ReadLine().Trim();
				
				// Test if the string contain a subcategory delimiter.
				if (buffer.StartsWith("["))
				{
					groupName = ParseGroup(buffer);
					// Create a new item in the reference list.				
					if (!this.Groups.Contains(groupName))
					{
						this.Groups.Add(groupName);
						lastGroup = groupName;
					}
				}
					// Test if the string actually contain something intersting.
				else if (buffer.Length > 1 && buffer.IndexOf(separation) > -1)
				{
					// Split the string to get the Key and Value.
					keyName = buffer.Substring(0, buffer.IndexOf(separation));
					keyValue = buffer.Substring(buffer.IndexOf(separation)+1, buffer.Length - buffer.IndexOf(separation)-1);
					//tempArray = buffer.Split(separation);
					
					//if (tempArray.Length > 1 && this.Groups.Contains(lastGroup))
					if (keyName != string.Empty && this.Groups.Contains(lastGroup))
						this.Groups[lastGroup].Keys.Add(keyName, keyValue);
					//this.Groups[lastGroup].Keys.Add(tempArray[0], tempArray[1]);
				}
			}

			iniString.Close();

			return true;
		}

		/// <summary>
		/// Writes to trace.
		/// </summary>
		public void WriteToTrace()
		{
			System.Web.HttpContext.Current.Trace.Write(this.GetIniContent());
		}

		/// <summary>
		/// Gets the content of the ini.
		/// </summary>
		/// <returns></returns>
		public string GetIniContent()
		{
			return GetIniContent('=');
		}

		/// <summary>
		/// Gets the content of the ini.
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <returns></returns>
		public string GetIniContent(char separator)
		{
			StringBuilder content = new StringBuilder();

			foreach(Group group in this.Groups)
			{
				content.Append("[" + group.Name + "]\n\n");

				foreach(Key key in group.Keys)
				{
					content.Append(key.Name + separator + key.Value + "\n");
				}

				content.Append("\n");
			}

			return content.ToString();
		}

		/// <summary>
		/// Parses the group.
		/// </summary>
		/// <param name="group">The group.</param>
		/// <returns></returns>
		public string ParseGroup(string group)
		{
			return group.Substring(1, group.Length - 2);
		}

		/// <summary>
		/// Gets or sets the groups.
		/// </summary>
		/// <value>The groups.</value>
		public GroupCollection Groups
		{
			get
			{
				if (_groups == null)
					_groups = new GroupCollection();
				return _groups;
			}
			set
			{
				_groups = value;
			}
		}
	}
}
