// Active WebControls v3.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace ActiveUp.WebControls.Common
{
	/// <summary>
	/// A collection of LicenseProduct.
	/// </summary>
	[Serializable]
	internal class LicenseProductCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// The default constructor.
		/// </summary>
		public LicenseProductCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Adds a LicenseProduct object in the collection.
		/// </summary>
		/// <param name="product">The LicenseProduct object</param>
		public void Add(LicenseProduct product)
		{
			List.Add(product);
		}

		/// <summary>
		/// Removes the LicenseProduct at the specified index position.
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
		/// Returns the LicenseProduct at the specified index position.
		/// </summary>
		public LicenseProduct this[int index]
		{
			get
			{
				return (LicenseProduct) List[index];
			}
		}
	}
}

