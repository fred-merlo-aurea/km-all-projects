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
using System.IO;
using System.Text;
using System.Reflection;
using ActiveUp.WebControls.Common;

namespace ActiveUp.WebControls.Common
{
	#region classe License

	/// <summary>
	/// Allow to read and write the license file.
	/// </summary>
	internal class License
	{
		#region Variables

		/// <summary>
		/// Informations about the license.
		/// </summary>
		private LicenseInformation _licenseInfo;

		/// <summary>
		/// License filename.
		/// </summary>
		private readonly string _licenseFileName = "License.lic"; 

		/// <summary>
		/// Extention of the license filename.
		/// </summary>
		private readonly string _extention = ".lic";

		/// <summary>
		/// Name of the key in the registry (GAC)
		/// </summary>
		private string _globalAssemblyKeyName = "";

		/// <summary>
		/// Encryption key to crypt and decrypt data.
		/// </summary>
		private readonly string _encryptionKey = "a52frt4x";

		#endregion

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		public License()
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			_Init(null,"");
		}

		/// <summary>
		/// Create a <see cref="License"/> object from the global assembly key name.
		/// </summary>
		/// <param name="globalAssemblyKeyName">Name of the key in the registry.</param>
		public License(string globalAssemblyKeyName)
		{
			_Init(null, globalAssemblyKeyName);
		}

		/// <summary>
		/// Create a <see cref="License"/> object from the <see cref="LicenseInformation"/> object.
		/// </summary>
		/// <param name="licenseInfo">Information about the license.</param>
		public License(LicenseInformation licenseInfo)
		{
			_Init(licenseInfo, "");
		}

		/// <summary>
		/// Create a <see cref="License"/> object from the <see cref="LicenseInformation"/> object and the global assembly key name.
		/// </summary>
		/// <param name="licenseInfo">Information about the license.</param>
		/// <param name="globalAssemblyKeyName">Name of the key in the registry.</param>
		public License(LicenseInformation licenseInfo, string globalAssemblyKeyName)
		{
			_Init(licenseInfo, globalAssemblyKeyName);
		}

		/// <summary>
		/// Initialize the <see cref="License"/>.
		/// </summary>
		/// <param name="licenseInfo">Information about the license.</param>
		/// <param name="globalAssemblyKeyName">Name of the key in the registry.</param>
		private void _Init(LicenseInformation licenseInfo, string globalAssemblyKeyName)
		{
			if (licenseInfo == null)
				licenseInfo = new LicenseInformation();
			_licenseInfo = licenseInfo;

			_globalAssemblyKeyName = globalAssemblyKeyName;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the license informations
		/// </summary>
		public LicenseInformation LicenseInfo
		{
			get {return _licenseInfo;}
			set {_licenseInfo = value;}
		}

		/// <summary>
		/// Gets or sets the global assembly key name.
		/// </summary>
		public string GlobalAssemblyKeyName
		{
			get {return _globalAssemblyKeyName;}
			set {_globalAssemblyKeyName = value;}
		}

		/// <summary>
		/// Gets the license file name.
		/// </summary>
		public string LicenseFileName
		{
			get {return _licenseFileName;}
		}

		/// <summary>
		/// Gets the extention of the license filename.
		/// </summary>
		public string Extention 
		{
			get {return _extention;}
		}

		#endregion

		#region Functions

		/// <summary>
		/// Read the license file.
		/// </summary>
		/// <param name="productCode">Product code.</param>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="licenseKey">The license key.</param>
		public void Read(ProductCode productCode, int majorVersion, string licenseKey)
		{
			_licenseInfo = _Read(productCode, majorVersion, licenseKey);
		}

		private LicenseInformation _Read(ProductCode productCode, int majorVersion)
		{
			return _Read(productCode, majorVersion, string.Empty);
		}

		/// <summary>
		/// Read the license file.
		/// </summary>
		/// <param name="productCode">Product code.</param>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="licenseKey">The license key.</param>
		/// <returns>
		/// 	<see cref="LicenseInformation"/> object contains the license information.
		/// </returns>
		private LicenseInformation _Read(ProductCode productCode, int majorVersion, string licenseKey)
		{
			string encryptedInfo = string.Empty;

			if (licenseKey == null || licenseKey == string.Empty)
			{
				string localPathName = ""; 
				string globalPathName = "";
				string pathName = "";
				string licenseFile = "";
			
				int ndxExtention = _licenseFileName.LastIndexOf(_extention);
				licenseFile = _licenseFileName.Substring(0,ndxExtention) + "." + productCode.ToString() + majorVersion.ToString() + _extention;

				localPathName = Path.GetDirectoryName( new Uri( Assembly.GetExecutingAssembly().CodeBase ).LocalPath ) + @"\" + licenseFile;

				if (System.IO.File.Exists(localPathName) == true)
					pathName = localPathName;
				else if (_globalAssemblyKeyName.Trim() != "")
				{
					globalPathName = GetFileNameFromTheGAC() + licenseFile;
					if (System.IO.File.Exists(globalPathName) == true)
						pathName = globalPathName;
					else
						throw new FileNotFoundException(string.Format("Unable to find the file '{0}'.",globalPathName),globalPathName);
				}
				else
					throw new FileNotFoundException(string.Format("Unable to find the file '{0}'",localPathName),localPathName);

				StreamReader s = new StreamReader(pathName,ASCIIEncoding.Default); 
				encryptedInfo = s.ReadToEnd();
				s.Close();
			}
			else
				encryptedInfo = licenseKey;

			string info = Cryptography.Decrypt(encryptedInfo,_encryptionKey);
			return info;
		}

		/// <summary>
		/// Write the license file.
		/// </summary>
		internal string Write()
		{
			return _Write(_licenseInfo,null);
		}

		/// <summary>
		/// Write the license file in a specified directory.
		/// </summary>
		/// <param name="path">Path where the license file must be written.</param>
		internal string Write(string path)
		{
			return _Write(_licenseInfo, path);
		}

		/// <summary>
		/// Write the license.
		/// </summary>
		/// <param name="licenseInfo"><see cref="LicenseInformation"/> object contains the license information.</param>
		/// <param name="path">Path where the license file must be written.</param>
		internal string _Write(LicenseInformation licenseInfo, string path)
		{
			string info = licenseInfo;	
			string encryptedInfo = Cryptography.Encrypt(info,_encryptionKey);
			string fullPath = "";
			string licenseFile = "";
			
			int ndxExtention = _licenseFileName.LastIndexOf(_extention);
			licenseFile = _licenseFileName.Substring(0,ndxExtention) + "." + licenseInfo.Product.ProductCode.ToString() + licenseInfo.Product.MajorVersion.ToString() + _extention;			

			if (path != null && path.Trim() != "")
			{
				fullPath += path;
				if (path.Substring(path.Length-1,1) != @"\" && 
					path.Substring(path.Length-1,1) != @"/")
					fullPath += @"\";
				fullPath += licenseFile;
			}
			else
				fullPath = licenseFile;
			StreamWriter sw = new StreamWriter(fullPath,false);
			sw.Write(encryptedInfo);
			sw.Close();

			return fullPath;
		}

		public LicenseStatus CheckLicense(LicenseProductCollection licenses, string license)
		{
			Exception lastException = new Exception();
			
			LicenseStatus status = new LicenseStatus(), lastGood = new LicenseStatus();
			lastGood.ErrorMessage = "notset";

			foreach(LicenseProduct product in licenses)
			{
				status = this.CheckLicense(product.ProductCode,product.MajorVersion,license);

				if (status.IsRegistered)
					return status;
				else if (status.Info.ExpireDate < DateTime.Now)
					lastGood = status;
			}

			if (lastGood.ErrorMessage != "notset")
				return lastGood;
			else
				return status;
		}

		/// <summary>
		/// Check if the license is valid or not.
		/// </summary>
		/// <param name="productCode">Product code.</param>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="license">The license.</param>
		/// <returns></returns>
		public LicenseStatus CheckLicense(ProductCode productCode, int majorVersion, string license)
		{
			try
			{
				this.Read(productCode,majorVersion,license);
			}
		
			catch(FileNotFoundException exfnf)
			{
				return new LicenseStatus(LicenseError.FileNotFound, false, this.LicenseInfo, string.Format("License file not found ('{0}').Please ensure that the license file you obtained by email when you downloaded or purchased the product is placed in the /bin directory where the product DLL is. If you use the GAC, please place the license file in the /bin directory where the component installed (by default : 'c:\\Program Files\\Active Up\\ActiveWebControls\\bin'). ",exfnf.FileName));
				//throw new FileNotFoundException(string.Format("License file not found ('{0}').Please ensure that the license file you obtained by email when you downloaded or purchased the product is placed in the /bin directory where the product DLL is. If you use the GAC, please place the license file in the /bin directory where the component installed (by default : 'c:\\Program Files\\Active Up\\ActiveWebControls\\bin'). ",exfnf.FileName),exfnf.FileName);
			}

			catch(Exception ex)
			{
				//return LicenseStatus.Invalid;
				return new LicenseStatus(LicenseError.Invalid, false, this.LicenseInfo, string.Format("Your license file is invalid. Please contact our support department : support@activeup.com ({0})",ex.Message));
				//throw new InvalidLicenseException(string.Format("Your license file is invalid. Please contact our support department : support@activeup.com ({0})",ex.Message));
			}

			if (this.LicenseInfo.Product.ProductCode != productCode || this.LicenseInfo.Product.MajorVersion < majorVersion)
			{
				//return LicenseStatus.Invalid;
				return new LicenseStatus(LicenseError.Invalid, false, this.LicenseInfo, string.Format("Invalid license file, please contact support@activeup.com for more information. ({0}-{1})", this.LicenseInfo.Product.ProductCode, this.LicenseInfo.Product.MajorVersion));
				//throw new InvalidLicenseException(string.Format("Invalid license file, please contact support@activeup.com for more information. ({0}-{1})", this.LicenseInfo.Product.ProductCode, this.LicenseInfo.Product.MajorVersion));
			}

			if (DateTime.Now > this.LicenseInfo.ExpireDate)
			{
				//return LicenseStatus.TrialExpired;
				return new LicenseStatus(LicenseError.TrialExpired, false, this.LicenseInfo, "Your trial period expired. Please contact our sales departement to purchase the product at sales@activeup.com.");
				//throw new TrialException("Your trial period expired. Please contact our sales departement to purchase the product at sales@activeup.com.");
			}

			return new LicenseStatus(LicenseError.None, true, this.LicenseInfo, string.Format("Registered developer : {0}", this.LicenseInfo.Company));
		}

		private string GetFileNameFromTheGAC()
		{
			Microsoft.Win32.RegistryKey hKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\AssemblyFolders\" + _globalAssemblyKeyName);
			if (hKey != null && hKey.ValueCount > 0)
			{
				return (string)hKey.GetValue(hKey.GetValueNames()[0]);
			}

			return "";
		}

		#endregion 
	}

	#endregion
}
