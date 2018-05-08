using System;
using ecn.common.classes;

namespace ecn.communicator.classes
{
	
	
	
	public class UserDefinedField
	{
		public UserDefinedField(int emailID, int groupDataFieldsID, string dataValue, DateTime modifiedDate, int surveyGridID)
		{
			EmailID = emailID;
			GroupDataFieldsID = groupDataFieldsID;
			DataValue = dataValue;
			ModifiedDate = modifiedDate;
			SurveyGridID = surveyGridID;
		}

		#region Properties
		private int _emailID;
		public int EmailID {
			get {
				return (this._emailID);
			}
			set {
				this._emailID = value;
			}
		}

		private int _groupDataFieldsID;
		public int GroupDataFieldsID {
			get {
				return (this._groupDataFieldsID);
			}
			set {
				this._groupDataFieldsID = value;
			}
		}

		private string _dataValue;
		public string DataValue {
			get {
				return (this._dataValue);
			}
			set {
				this._dataValue = value;
			}
		}

		private DateTime _modifiedDate;
		public DateTime ModifiedDate {
			get {
				return (this._modifiedDate);
			}
			set {
				this._modifiedDate = value;
			}
		}

		private int _surveyGridID;
		public int SurveyGridID {
			get {
				return (this._surveyGridID);
			}
			set {
				this._surveyGridID = value;
			}
		}
		#endregion

		#region Database Methods
		public void Save() {
			// Excute the SaveSqlString
		}

		public string SaveSqlString {
			get {
				return string.Format(@"
IF (SELECT count(EmailDataValuesID) from EmailDataValues where EmailID = {0} and GroupDataFieldsID = {1}) = 0 
 BEGIN
	INSERT INTO EmailDataValues (EmailID, GroupDataFieldsID, DataValue, ModifiedDate, SurveyGridID) VALUES 
					({0}, {1}, '{2}', '{3}', {4});
 END
ELSE
 BEGIN
	UPDATE EmailDataValues SET DataValue = '{2}', ModifiedDate = '{3}', SurveyGridID = {4} WHERE EmailID = {0} and GroupDataFieldsID = {1};
 END;
", EmailID, GroupDataFieldsID, DataFunctions.CleanString(DataValue), ModifiedDate.ToString(), SurveyGridID);
			}		
		}
		#endregion
	}
}
