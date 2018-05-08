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

namespace ActiveUp.WebControls.Common
{
	#region class LicenseInformation

	/// <summary>
	/// Contains informations about license.
	/// </summary>
	internal class LicenseInformation
	{
		#region Variables

		/// <summary>
		/// Company.
		/// </summary>
		private string _company;

		/// <summary>
		/// Product code.
		/// </summary>
		private LicenseProduct _product;

		/// <summary>
		/// Expire date.
		/// </summary>
		private DateTime _expireDate;

		/// <summary>
		/// Landmark.
		/// </summary>
		private string _landmark;

		/// <summary>
		/// Separator of each element.
		/// </summary>
		private static readonly string _separator = "$";

		#endregion

		#region Constrcutors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public LicenseInformation()
		{
			_Init("",null,DateTime.MinValue,"");
		}

		/// <summary>
		/// Create a <see cref="LicenseInformation"/> object from the company, product code and the expire date.
		/// </summary>
		/// <param name="company">Company.</param>
		/// <param name="product">Product.</param>
		/// <param name="expireDate">Expire date.</param>
		public LicenseInformation(string company, LicenseProduct product, DateTime expireDate)
		{
			_Init(company, product, expireDate, "");
		}

		/// <summary>
		/// Create a <see cref="LicenseInformation"/> object from the company, product code, expire date and landmark.
		/// </summary>
		/// <param name="company">Company.</param>
		/// <param name="product">Product.</param>
		/// <param name="expireDate">Expire date.</param>
		/// <param name="landmark">Landmark.</param>
		public LicenseInformation(string company, LicenseProduct product, DateTime expireDate, string landmark)
		{
			_Init(company, product, expireDate, landmark);
		}

		/// <summary>
		/// Initialize the <see cref="LicenseInformation"/>.
		/// </summary>
		/// <param name="company">Company.</param>
		/// <param name="product">Product.</param>
		/// <param name="expireDate">Expire date.</param>
		/// <param name="landmark">Landmark.</param>
		private void _Init(string company, LicenseProduct product, DateTime expireDate, string landmark)
		{
			_company = company;
			_product = product;
			_expireDate = expireDate;
			_landmark = landmark;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		public string Company
		{
			get {return _company;}
			set {_company = value;}
		}

		/// <summary>
		/// Gets or sets the product code.
		/// </summary>
		public LicenseProduct Product
		{
			get {return _product;}
			set {_product = value;}
		}

		/// <summary>
		/// Gets or sets the expire date.
		/// </summary>
		public DateTime ExpireDate
		{
			get {return _expireDate;}
			set {_expireDate = value;}
		}

		/// <summary>
		/// Gets or sets the landmark.
		/// </summary>
		public string Landmark
		{
			get {return _landmark;}
			set {_landmark = value;}
		}

		#endregion

		#region Operators

		/// <summary>
		/// Convert a string to this representation in a <see cref="LicenseInformation"/> object.
		/// </summary>
		/// <param name="licenseInfo">String contains the license informations.</param>
		/// <returns><see cref="LicenseInformation"/> object.</returns>
		public static implicit operator LicenseInformation(string licenseInfo)
		{
			string[] infos = licenseInfo.Split(_separator[0]);
			string[] expireDate = infos[2].Split('/');
			
			return new LicenseInformation(infos[0],infos[1],new DateTime(Int32.Parse(expireDate[0]),Int32.Parse(expireDate[1]),Int32.Parse(expireDate[2])),infos[3]);
		}

		/// <summary>
		/// Convert a <see cref="LicenseInformation"/> object to this representation in string.
		/// </summary>
		/// <param name="licenseInfo"><see cref="LicenseInformation"/> object contains the license information.</param>
		/// <returns>String representation of a <see cref="LicenseInformation"/> object.</returns>
		public static implicit operator string(LicenseInformation licenseInfo)
		{
			string converted = "";
			converted += licenseInfo.Company;
			converted += _separator;
			converted += licenseInfo.Product;
			converted += _separator;
			string expireDate = "";
			expireDate += licenseInfo.ExpireDate.Year.ToString(); 
			expireDate += "/";
			expireDate += string.Format("{0:00}",licenseInfo.ExpireDate.Month);
			expireDate += "/";
			expireDate += string.Format("{0:00}", licenseInfo.ExpireDate.Day);
			converted += expireDate;
			converted += _separator;
			converted += licenseInfo.Landmark;
			return converted;
		}

		#endregion
	}

	#endregion
}
