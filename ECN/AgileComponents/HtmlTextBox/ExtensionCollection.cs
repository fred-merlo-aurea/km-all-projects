using System;
using System.Collections;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Represents a <see cref="ExtensionCollection"/> object.
	/// </summary>
	[Serializable]
	public class ExtensionCollection : CollectionBase
	{
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionCollection"/> class.
		/// </summary>
		public ExtensionCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds the specified extension.
		/// </summary>
		/// <param name="extension">The extension.</param>
		public void Add(string extension)
		{
			VerifyExtension(ref extension);
			List.Add(extension.ToUpper());
		}
		
		/// <summary>
		/// Removes at the specified index.
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
		/// Gets the <see cref="ExtensionCollection"/> at the specified index.
		/// </summary>
		/// <value></value>
		public ExtensionCollection this[int index]
		{
			get
			{
				return (ExtensionCollection) List[index];
			}
		}

		/// <summary>
		/// Determines whether contains the specified extension.
		/// </summary>
		/// <param name="extension">The extension.</param>
		/// <returns>
		/// 	<c>true</c> if contains the specified extension; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string extension)
		{
			VerifyExtension(ref extension);
			for (int i = 0 ; i < List.Count ; i++)
				if (extension.ToUpper() == List[i].ToString())
					return true;
			return false;
		}

		private void VerifyExtension(ref string extension)
		{
			if (extension.StartsWith(".") == false)
				extension = extension.Insert(0,".");
		}
	}
}
