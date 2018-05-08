// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
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
	#region enumerations

	/// <summary>
	/// Contains all valid product code.
	/// </summary>
	internal enum ProductCode
	{
		/// <summary>
		/// ActiveCalendar.
		/// </summary>
		ACL	,
		/// <summary>
		/// HTML Text Box.
		/// </summary>
		HTB,
		/// <summary>
		/// ActiveQ.
		/// </summary>
		AVQ,
		/// <summary>
		/// ActiveMail.
		/// </summary>
		AML,
		/// <summary>
		/// ActivePager.
		/// </summary>
		APG,
		/// <summary>
		/// ActiveWebControls.
		/// </summary>
		AWC,
		/// <summary>
		/// ActiveWhoIs.
		/// </summary>
		AWI,
		/// <summary>
		/// ActiveTree.
		/// </summary>
		ATV,
		/// <summary>
		/// ActiveRTF.
		/// </summary>
		RTF,
		/// <summary>
		/// ActiveDateTime.
		/// </summary>
		ADT,
		/// <summary>
		/// ActiveToolbar
		/// </summary>
		ATB,
		/// <summary>
		/// ActivePopup
		/// </summary>
		APP,
		/// <summary>
		/// HtmlProtector
		/// </summary>
		HPT,
		/// <summary>
		/// ActiveUpload
		/// </summary>
		AUP,
		/// <summary>
		/// ActiveLoader
		/// </summary>
		ALD,
		/// <summary>
		/// ActiveTimer
		/// </summary>
		AWT,
		/// <summary>
		/// ActiveImage
		/// </summary>
		AIE,
		/// <summary>
		/// ActiveRotator
		/// </summary>
		ACR,
		/// <summary>
		/// ActiveInput
		/// </summary>
		AIP,
		/// <summary>
		/// ActiveAjax
		/// </summary>
		AAX,
		/// <summary>
		/// ActiveMenu
		/// </summary>
		AMN,
		/// <summary>
		/// ActivePanel
		/// </summary>
		APN,
		/// <summary>
		/// SpellChecker
		/// </summary>
		ASC,
		/// <summary>
		/// Color
		/// </summary>
		ACP,
		/// <summary>
		/// Active Autosuggest
		/// </summary>
		AAS
	}

	/// <summary>
	/// Contains all valid edition.
	/// </summary>
	internal enum Edition
	{
		/// <summary>
		/// 1 developer with subscription
		/// </summary>
		S1,
		/// <summary>
		/// 1 developer with subscription + source code
		/// </summary>
		W1,
		/// <summary>
		/// renewal 1 developer
		/// </summary>
		SR,
		/// <summary>
		/// renewal 1 developer + source code
		/// </summary>
		WR
	}

	#endregion

	#region class Product

	/// <summary>
	/// Contains all information about one product.
	/// </summary>
	internal class LicenseProduct
	{
		#region Variables

		/// <summary>
		/// Code.
		/// </summary>
		ProductCode _productCode;

		/// <summary>
		/// Major version.
		/// </summary>
		int _majorVersion;

		/// <summary>
		/// Edition.
		/// </summary>
		Edition _edition;

		#endregion
	
		#region Constrcutors

		/// <summary>
		/// Create a <see cref="ProductCode"/> object specifying the product code, major version and edition.
		/// </summary>
		/// <param name="productCode">Product code.</param>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="edition">Edition.</param>
		public LicenseProduct(ProductCode productCode, int majorVersion, Edition edition)
		{
			_Init(productCode, majorVersion, edition);
		}

		/// <summary>
		/// Initialize a <see cref="ProductCode"/> object.
		/// </summary>
		/// <param name="productCode">Product code.</param>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="edition">Edition.</param>
		private void _Init(ProductCode productCode, int majorVersion, Edition edition)
		{
			_productCode = productCode;
			_majorVersion = majorVersion;
			_edition = edition;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the product code.
		/// </summary>
		public ProductCode ProductCode
		{
			get {return _productCode;}
			set {_productCode = value;}
		}

		/// <summary>
		/// Gets or sets the major version.
		/// </summary>
		public int MajorVersion
		{
			get {return _majorVersion;}
			set {_majorVersion = value;}
		}

		/// <summary>
		/// Gets or sets the edition.
		/// </summary>
		public Edition Edition
		{
			get {return _edition;}
			set {_edition = value;}
		} 

		#endregion

		#region Operators

		/// <summary>
		/// Convert a <see cref="ProductCode"/> object to this representation in string.
		/// </summary>
		/// <param name="product">tring contains the product informations.</param>
		/// <returns><see cref="ProductCode"/> object.</returns>
		public static implicit operator string(LicenseProduct product)
		{
			return product.ProductCode.ToString() + product.MajorVersion.ToString() + product.Edition.ToString();
		}

		/// <summary>
		/// Convert a string to this representation in a <see cref="ProductCode"/> object.
		/// </summary>
		/// <param name="product"><see cref="ProductCode"/> object contains the product information.</param>
		/// <returns>String representation of a <see cref="ProductCode"/> object.</returns>
		public static implicit operator LicenseProduct(string product)
		{
			return new LicenseProduct((ProductCode)Enum.Parse(typeof(ProductCode),product.Substring(0,3),true),Int32.Parse(product.Substring(3,1)), (Edition)Enum.Parse(typeof(Edition), product.Substring(4) ,true));
		}

		#endregion
	}

	#endregion
}
