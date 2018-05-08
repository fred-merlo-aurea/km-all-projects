using System;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="LocalizedTexts"/> object.
	/// </summary>
	public class LocalizedTexts : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizedTexts"/> class.
		/// </summary>
		public LocalizedTexts()
		{

		}

		/// <summary>
		/// Gets the <see cref="LocalizedText"/> at the specified index.
		/// </summary>
		/// <value></value>
		public LocalizedText this[int index]
		{
			get
			{
				try
				{

					LocalizedText retValue = (LocalizedText) List[index];
					return retValue;
				}
				catch
				{
					return null;
				}
				
			}
		}

		/// <summary>
		/// Gets the <see cref="LocalizedText"/> with the specified id.
		/// </summary>
		/// <value></value>
		public LocalizedText this[string id]
		{
			get 
			{
				if (this.Count == 0) return new LocalizedText();
				for (int i = 0 ; i < this.Count ; i++)
				{
					if (this[i].Id == id)
						return this[i];
				}
				return new LocalizedText();
			}
		}

		/// <summary>
		/// Adds the specified localized text.
		/// </summary>
		/// <param name="localizedText">The localized text.</param>
		/// <returns></returns>
		public int Add(LocalizedText localizedText)
		{
			return List.Add(localizedText);
		}

		/// <summary>
		/// Adds <see cref="LocalizedText"/> at the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public int Add(string id, string val)
		{
			return Add(new LocalizedText(id,val));
		}

		/// <summary>
		/// Removes <see cref="LocalizedText"/> at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void Remove(int index)
		{
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index);  
			}
		}

		/*public string ConvertToString(char separator)
		{
			string result = string.Empty;	
			foreach (LocalizedText lt in this.List)
			{
				result += lt.Id;
				result += separator;
				result += lt.Value;
				result += separator;
			}

			result = result.TrimEnd(separator);

			return result;
		}*/

		/// <summary>
		/// Converts to string the registered array.
		/// </summary>
		/// <returns></returns>
		public string ConvertToStringToRegisterArray()
		{
			string result = string.Empty;	
			char separator = ',';
			foreach (LocalizedText lt in this.List)
			{
				result += string.Format("'{0}'",lt.Id);
				result += separator;
				result += string.Format("'{0}'",lt.Value);
				result += separator;
			}

			result = result.TrimEnd(separator);

			return result;
		}
	}
}
