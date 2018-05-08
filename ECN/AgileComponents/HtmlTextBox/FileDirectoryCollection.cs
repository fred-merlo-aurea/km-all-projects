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
	/// A collection of directories.
	/// </summary>
	[Serializable]
	public class FileDirectoryCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FileDirectoryCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds a FileDirectory object in the collection.
		/// </summary>
		/// <param name="directory">The FileDirectory object</param>
		public void Add(FileDirectory directory)
		{
			List.Add(directory);
		}

		/// <summary>
		/// Adds a FileDirectory object in the collection specifying the path.
		/// </summary>
		/// <param name="path">The path of the FileDirectory object.</param>
		public void Add(string path)
		{
			List.Add(new FileDirectory(path));
		}

		/// <summary>
		/// Adds a FileDirectory object in the collection specifying the path.
		/// </summary>
		/// <param name="name">The name of the FileDirectory object.</param>
		/// <param name="path">The path of the FileDirectory object.</param>
		public void Add(string name, string path)
		{
			List.Add(new FileDirectory(name, path));
		}

		/// <summary>
		/// Adds a FileDirectory object in the collection specifying the path.
		/// </summary>
		/// <param name="name">The name of the FileDirectory object.</param>
		/// <param name="path">The path of the FileDirectory object.</param>
		/// <param name="webPath">The web path of the FileDirectory.</param>
		public void Add(string name, string path, string webPath)
		{
			List.Add(new FileDirectory(name, path, webPath));
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
		public FileDirectory this[int index]
		{
			get
			{
				return (FileDirectory) List[index];
			}
		}

		/// <summary>
		/// Gets a directory based on its name.
		/// </summary>
		public FileDirectory this[string name]
		{
			get
			{
				foreach(FileDirectory directory in List)
				{
					if (directory.Name == name)
					{ return directory; }
				}
				return null;
			}
		}

		/// <summary>
		/// Loads all the image contained in the directories.
		/// </summary>
		public void LoadAll()
		{
			foreach(FileDirectory fileDirectory in List)
			{
				fileDirectory.LoadFiles();
			}
		}

		/// <summary>
		/// Clear all the image contained in the directories.
		/// </summary>
		public void ClearFiles()
		{
			foreach(FileDirectory fileDirectory in List)
			{
				fileDirectory.Files.Clear();
			}
		}
	}
}
