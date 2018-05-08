using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Upload selection mode enumeration.
	/// </summary>
	public enum UploadSelectionMode
	{
		/// <summary>
		/// Files only.
		/// </summary>
		FilesOnly,
		/// <summary>
		/// Directories only.
		/// </summary>
		DirectoriesOnly,
		/// <summary>
		/// Files and directories.
		/// </summary>
		FilesAndDirectories
	}

	/// <summary>
	/// Script url type.
	/// </summary>
	public enum ScriptURLType
	{
		/// <summary>
		/// Absolute script url.
		/// </summary>
		Absolute,
		/// <summary>
		/// Relative script url.
		/// </summary>
		Relative
	}
}
