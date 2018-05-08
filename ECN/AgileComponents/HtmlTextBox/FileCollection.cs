// Html TextBox 2.x
// Copyright (c) 2003 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Collection of File objects.
	/// </summary>
	[Serializable]
	public class FileCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FileCollection()
		{
			//
		}

		/// <summary>
		/// Adds a File object in the collection.
		/// </summary>
		/// <param name="file">The File object</param>
		public void Add(File file)
		{
			List.Add(file);
		}

		/// <summary>
		/// Adds a File object in the collection specifying the label and location.
		/// </summary>
		/// <param name="label">The label of the File object.</param>
		/// <param name="location">The location of the File object.</param>
		public void Add(string label, string location)
		{
			List.Add(new File(label, location, 0));
		}

		/// <summary>
		/// Adds a File object in the collection specifying the location and size.
		/// </summary>
		/// <param name="location">The location of the File object.</param>
		/// <param name="size">The size in bytes of the File object.</param>
		public void Add(string location, long size)
		{
			List.Add(new File(string.Empty, location, size));
		}

		/// <summary>
		/// Adds a file object in the collection specifying the label, location and size.
		/// </summary>
		/// <param name="label">The label of the File object.</param>
		/// <param name="location">The location of the File object.</param>
		/// <param name="size">The size in bytes of the File object.</param>
		public void Add(string label, string location, long size)
		{
			List.Add(new File(label, location, size));
		}

		/// <summary>
		/// Adds a file object in the collection specifying the label, location and size.
		/// </summary>
		/// <param name="label">The label of the File object.</param>
		/// <param name="location">The location of the File object.</param>
		/// <param name="size">The size in bytes of the File object.</param>
		/// <param name="width">The width of the File object (Image).</param>
		/// <param name="height">The height of the File object (Image).</param>
		public void Add(string label, string location, long size, int width, int height)
		{
			List.Add(new File(label, location, size, width, height));
		}

		/// <summary>
		/// Removes the File at the specified index position.
		/// </summary>
		/// <param name="index"></param>
		public void Remove(int index)
		{
			// Checks to see if there is a file at the supplied index.
			if (index < Count || index >= 0)
			{
				List.RemoveAt(index); 
			}
		}

		/// <summary>
		/// Returns the File at the specified index position.
		/// </summary>
		public File this[int index]
		{
			get
			{
				return (File) List[index];
			}
		}

		/// <summary>
		/// Gets the labels contained in the collection.
		/// </summary>
		public string[] Labels
		{
			get
			{
				string[] labels = new string[this.Count];

				for(int index=0;index<this.Count;index++)
					labels[index] = this[index].Label;
				
				return labels;
			}
		}

		/// <summary>
		/// Gets the locations contained in the collection.
		/// </summary>
		public string[] Locations
		{
			get
			{
				string[] locations = new string[this.Count];

				for(int index=0;index<this.Count;index++)
					locations[index] = this[index].Location;
				
				return locations;
			}
		}
	}
}
