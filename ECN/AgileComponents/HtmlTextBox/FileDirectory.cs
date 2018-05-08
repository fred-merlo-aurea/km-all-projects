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
using System.IO;
using System.Drawing;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// A file directory (usually containing images).
	/// </summary>
	[Serializable]
	public class FileDirectory
	{
		private string _path, _webPath;
		private FileCollection _files;
		private string _name; 

		
		/// <summary>
		/// The default constructor.
		/// </summary>
		public FileDirectory()
		{
			this.Path = string.Empty;
			this.WebPath = string.Empty;
			this.Name = string.Empty;
		}

		/// <summary>
		/// Create the FileDirectory object specifying the path.
		/// </summary>
		/// <param name="path">The full path to the directory.</param>
		public FileDirectory(string path)
		{
			this.Path = path;
			this.WebPath = string.Empty;
			this.Name = path;
		}

		/// <summary>
		/// Create the FileDirectory object specifying the name and path.
		/// </summary>
		/// <param name="name">The name of the directory.</param>
		/// <param name="path">The full path to the directory.</param>
		public FileDirectory(string name, string path)
		{
			this.Path = path;
			this.WebPath = string.Empty;
			this.Name = name;
		}

		/// <summary>
		/// Create the FileDirectory object specifying the name, path and webpath.
		/// </summary>
		/// <param name="name">The name of the directory.</param>
		/// <param name="path">The full path to the directory.</param>
		/// <param name="webPath">The web path to the directory.</param>
		public FileDirectory(string name, string path, string webPath)
		{
			this.Path = path;
			this.WebPath = webPath;
			this.Name = name;
		}

		/// <summary>
		/// Gets or sets the web path to access to the directory.
		/// </summary>
		/// <remarks>The web path is used by the editor to fill the SRC attribute of the image (IMG).</remarks>
		public string WebPath
		{
			get
			{
				return _webPath;
			}
			set
			{
				_webPath = value;
			}
		}

		/// <summary>
		/// Gets or sets the physical path to the directory.
		/// </summary>
		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the directory.
		/// </summary>
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
		/// Gets or sets the files contained in the directory.
		/// </summary>
		public FileCollection Files
		{
			get
			{
				if (_files == null)
					_files = new FileCollection();
				return _files;
			}
			set
			{
				_files = value;
			}
		}

		/// <summary>
		/// Load the files data (not content) in the Files collection.
		/// </summary>
		/// <returns>Return the number of files loaded.</returns>
		public int LoadFiles()
		{
			return LoadFiles(this.Path, true, string.Empty);
		}

		/// <summary>
		/// Load the files data (not content) in the Files collection.
		/// </summary>
		/// <param name="detectImageSize">Specify whether if you want to auto detect the image size or not.</param>
		/// <returns>Return the number of files loaded.</returns>
		public int LoadFiles(bool detectImageSize)
		{
			return LoadFiles(this.Path, detectImageSize, string.Empty);
		}

		/// <summary>
		/// Load the files data (not content) in the Files collection.
		/// </summary>
		/// <param name="detectImageSize">Specify whether if you want to auto detect the image size or not.</param>
		/// <param name="extFilter">Specify the accepted file extension accepted.</param>
		/// <returns>Return the number of files loaded.</returns>
		public int LoadFiles(bool detectImageSize, string extFilter)
		{
			return LoadFiles(this.Path, detectImageSize, extFilter);
		}

		/// <summary>
		/// Load the files data (not content) in the Files collection.
		/// </summary>
		/// <param name="path">Specify the path where to load the files.</param>
		/// <param name="detectImageSize">Specify whether if you want to auto detect the image size or not.</param>
		/// <param name="extFilter">Specify the accepted file extension accepted.</param>
		/// <returns>Return the number of files loaded.</returns>
		public int LoadFiles(string path, bool detectImageSize, string extFilter)
		{
			string[] filters = extFilter.Split(';');

			if (Directory.Exists(path))
			{
				DirectoryInfo directory = new DirectoryInfo(path);
				FileInfo[] fileArray = directory.GetFiles();

				int index = 0;

				foreach (FileInfo file in fileArray)
				{
					if (extFilter.Length > 1 && filters.Length > 0)
					{
						foreach(string extensions in filters)
						{
							if (file.Name.ToUpper().EndsWith(extensions.ToUpper()))
							{
								AddFile(file, detectImageSize);
								index++;
							}
						}
					}
					else
					{
						AddFile(file, detectImageSize);
						index++;
					}
						
				}

				return index;
			}
			
			return 0;
		}

		/// <summary>
		/// Add the specified file into the collection.
		/// </summary>
		/// <param name="file">The file to add.</param>
		/// <param name="detectImageSize">Specify whether if you want to auto detect the image size or not.</param>
		public void AddFile(FileInfo file, bool detectImageSize)
		{
			if (!detectImageSize)
				this._files.Add(file.Name, file.Name, file.Length);
			else
			{
				try
				{
					Bitmap image = new Bitmap(file.FullName);
					this.Files.Add(file.Name, file.Name, file.Length, image.Width, image.Height);
					image.Dispose();
				}
				catch
				{
					this.Files.Add(file.Name, file.Name, file.Length);
				}
			}
		}
	}
}
