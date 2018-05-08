using System;

namespace ecn.communicator.classes.ImportData
{
	
	
	
	public class ImportErrorReport
	{
		public ImportErrorReport(int rowIndex, string emailAddress, string errorMessage)
		{
			_rowIndex = rowIndex;
			_emailAddress = emailAddress;
			// _targetColumnName = targetColumnName;
			_errorMessage = errorMessage;
		}

		private int _rowIndex;
		public int RowIndex {
			get {
				return (this._rowIndex);
			}
			set {
				this._rowIndex = value;
			}
		}

		private string _emailAddress;
		public string EmailAddress {
			get {
				return (this._emailAddress);
			}
			set {
				this._emailAddress = value;
			}
		}

		private string _targetColumnName;
		public string TargetColumnName {
			get {
				return (this._targetColumnName);
			}
			set {
				this._targetColumnName = value;
			}
		}

		private string _errorMessage;
		public string ErrorMessage {
			get {
				return (this._errorMessage);
			}
			set {
				this._errorMessage = value;
			}
		}
	}
}
