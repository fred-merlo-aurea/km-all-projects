using System;

namespace ActiveUp.WebControls.Common
{
	/// <summary>
	/// Represents a <see cref="LicenseStatus"/> object.
	/// </summary>
	internal class LicenseStatus
	{
		private LicenseError _errorType;
		private bool _isRegistered;
		private LicenseInformation _info;
		private string _errorMessage;

		public LicenseStatus()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public LicenseStatus(LicenseError errorType, bool isRegistered, LicenseInformation info, string errorMessage)
		{
			_info = info;
			_isRegistered = isRegistered;
			_errorType = errorType;
			_errorMessage = errorMessage;
		}

		public LicenseInformation Info
		{
			get
			{
				return _info;
			}
			set
			{
				_info = value;
			}
		}

		public bool IsRegistered
		{
			get
			{
				return _isRegistered;
			}
			set
			{
				_isRegistered = value;
			}
		}

		public LicenseError ErrorType
		{
			get
			{
				return _errorType;
			}
			set
			{
				_errorType = value;
			}
		}

		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
				_errorMessage = value;
			}
		}
	}
}
