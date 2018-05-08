using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ecn.communicator.classes;

namespace ecn.communicator.classes.ImportData {
	
	
	
	public class ImportSqlBuilder {
		Hashtable _parameters;
		Hashtable _keyColumns;
		public ImportSqlBuilder() {			
			
			_parameters = new Hashtable();
			_keyColumns = new Hashtable();			
			_keyColumns.Add("EmailAddress", "");
			_keyColumns.Add("CustomerID", 0);
			_keyColumns.Add("GroupID", 0);
			
		}


		#region Parameter Methods
		public void AddParameter(string columnName, object val) {
			if (IsKeyColumn(columnName)) {
				_keyColumns[columnName] = val;
				return;
			}			
			object existVal = GetValueByParameterName(columnName);
			if (existVal == null) {
				_parameters.Add(columnName, val);	
				return;
			}
			_parameters[columnName] = val;
		}		

		public int ParameterCount {
			get { return _parameters.Count;}
		}

		public object GetValueByParameterName(string columnName) {
			return _parameters[columnName];
		}
		#endregion

		#region Key Columns Methods
		public object GetKeyColumnValue(string keyColumnName) {
			return _keyColumns[keyColumnName];
		}		

		
		public bool IsKeyColumn(string columnName) {
			foreach(string keyCol in _keyColumns.Keys) {
				if (keyCol == columnName) {
					return true;
				}
			}
			return false;
		}

		#endregion

		
		/// Return sql command to insert/update email table with
		
		/// <returns></returns>
		public SqlCommand GetSqlCommand(ArrayList groupDataFields, EmailTableColumnManagerCommunicator manager) {
			if (GetKeyColumnValue("EmailAddress").ToString() == string.Empty) {
				return null;
			}		
	
			if (Convert.ToInt32(GetKeyColumnValue("GroupID")) == 0) {
				return null;
			}		

			if (Convert.ToInt32(GetKeyColumnValue("CustomerID")) == 0) {
				return null;
			}		


			SqlCommand cmd = new SqlCommand(GetSqlCommandText(groupDataFields));
			InitializeParameters(cmd, manager);
			cmd.CommandTimeout = 0;
			return cmd;
		}		
		
		private string GetSqlCommandText(ArrayList groupDataFields) {
			// Added a Error handling condition lines 95-99 'cos if the select returns multiple rows there's no eid & will 
			// insert a dup Email Profile though there's already one. 
			// - ashok 9/1/05
			return string.Format(@"DECLARE @eid AS int,@egid AS int,@edvid AS bigint;
SET @eid=(SELECT EmailID FROM Emails WHERE EmailAddress=@EmailAddress AND CustomerID=@CustomerID);
if @@error <> 0
	BEGIN
		PRINT 'Error Occured'
	END
ELSE
	BEGIN
		IF (@eid IS NULL)
			BEGIN
				{0}
				SET @eid=(SELECT @@IDENTITY);
				SELECT 1 AS IFlag
			END
		ELSE
			BEGIN
				{1}
				SELECT 0 AS IFlag
			END
		SET @egid=(SELECT EmailGroupID FROM EmailGroups WHERE EmailID=@eid AND GroupID=@GroupID);
		IF (@egid IS NULL)
			BEGIN
				{2}
			END
{3}
{4}
END", GetInsertEmailSql(), GetUpdateEmailSql(), GetInsertGroupSql(), GetUpdateGroupSql(), GetUDFSql(groupDataFields));
		}

		private void InitializeParameters(SqlCommand command, EmailTableColumnManagerCommunicator manager) {
			command.Parameters.Add("@EmailAddress",SqlDbType.VarChar, 200).Value =  _keyColumns["EmailAddress"].ToString().Trim();
			command.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value =  _keyColumns["GroupID"];
			command.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = _keyColumns["CustomerID"];			
			command.Parameters.Add("@ModifiedDate", SqlDbType.DateTime, 8).Value = DateTime.Now;			

			foreach(string colName in _parameters.Keys) {
				command.Parameters.Add("@"+colName, manager.GetSqlDbTypeByName(colName), manager.GetColumnLengthByName(colName)).Value =_parameters[colName];
			}
		}


		#region Properties for Build Sql
		private StringCollection _emailTableColumns = null;
		protected StringCollection EmailTableColumns {
			get {
				if (_emailTableColumns == null) {
					InitializeColumns();
				}
				return (this._emailTableColumns);
			}			
		}

		private StringCollection _emailGroupTableColumns = null;
		protected StringCollection EmailGroupTableColumns {
			get {
				if (_emailGroupTableColumns == null) {
					InitializeColumns();
				}
				return (this._emailGroupTableColumns);
			}			
		}

		private StringCollection _userDefinedColumns = null;
		public StringCollection UserDefinedColumns {
			get {
				if (_userDefinedColumns == null) {
					InitializeColumns();
				}
				return (this._userDefinedColumns);
			}			
		}

		private void InitializeColumns() {
			_emailGroupTableColumns = new StringCollection();
			_emailTableColumns = new StringCollection();
			_userDefinedColumns = new StringCollection();
			foreach(string colName in _parameters.Keys) {
				if (EmailTableColumnManagerCommunicator.IsUserDefinedField(colName)) {
					_userDefinedColumns.Add(colName);
					continue;
				}
				if (EmailTableColumnManagerCommunicator.BelongToEmailGroupTable(colName)) {
					_emailGroupTableColumns.Add(colName);
					continue;
				} 
				_emailTableColumns.Add(colName);				
			}
		}
		#endregion

		#region Methods to generate Insert Email Sql
		private string GetInsertEmailSql() {
			return string.Format("INSERT INTO Emails ({0}) VALUES ({1});", GetInsertEmailColumnList(), GetInsertEmailParameterList());			
		}
		
		private string GetInsertEmailColumnList() {
			return GetEmailColumnList("");
		}

		private string GetInsertEmailParameterList() {
			return GetEmailColumnList("@");
		}

		private string GetEmailColumnList(string prefix) {
			StringBuilder columns = new StringBuilder(string.Format("{0}EmailAddress,{0}CustomerID",prefix));
			foreach(string colName in EmailTableColumns) {				
				columns.Append(string.Format(",{0}{1}", prefix, colName));				
			}
			if (prefix.Length > 0) {
				columns.Append(",@ModifiedDate");				
			} else {
				columns.Append(",DateAdded");
			}
			return columns.ToString();
		}
		#endregion

		#region Methods to generate Update Email Sql
		private string GetUpdateEmailSql() {			
			if (EmailTableColumns.Count == 0) {
				return string.Empty;
			}
			return string.Format("UPDATE Emails SET {0} WHERE EmailID=@eid;", GetUpdateEmailParameterList());
		}

		private string GetUpdateEmailParameterList() {
			StringBuilder columns = new StringBuilder();
			foreach(string colName in EmailTableColumns) {				
				columns.Append(string.Format("{0}=@{0},", colName));
			}
			columns.Append("DateUpdated=@ModifiedDate");
			return columns.ToString();
		}
		#endregion

		#region Methods to generate Insert Group Sql
		private string GetInsertGroupSql() {
			return string.Format("INSERT INTO EmailGroups ({0}) VALUES ({1});", GetInsertGroupColumnList(), GetInsertGroupParameterList());			
		}

		private string GetInsertGroupColumnList() {
			return GetGroupColumnList("");
		}

		private string GetInsertGroupParameterList() {
			return GetGroupColumnList("@");
		}

		private string GetGroupColumnList(string prefix) {			
			StringBuilder columns;
			if (prefix.Length > 0) {
				columns = new StringBuilder(string.Format("{0}eid,{0}GroupID",prefix));			
			} else {
				columns = new StringBuilder("EmailID,GroupID");
			}
			foreach(string colName in EmailGroupTableColumns) {				
				columns.Append(string.Format(",{0}{1}", prefix, colName));					
			}
			if (prefix.Length > 0) {
				columns.Append(",@ModifiedDate");					
			} else {
				columns.Append(",CreatedOn");
			}
			return columns.ToString();
		}
		#endregion

		#region Methods to generate Update Group Sql
		private string GetUpdateGroupSql() {
			if (EmailGroupTableColumns.Count == 0) {
				return string.Empty;
			}
			return string.Format(@"ELSE
BEGIN
	UPDATE EmailGroups SET {0} WHERE EmailGroupID=@egid;
END", GetUpdateGroupParameters());
		}

		private string GetUpdateGroupParameters() {
			StringBuilder columns = new StringBuilder();
			foreach(string colName in EmailGroupTableColumns) {
				if(colName.Equals("SubscribeTypeCode")){
					columns.Append(string.Format(@"
SubscribeTypeCode = 
	CASE
		WHEN SubscribeTypeCode = 'U' 
		THEN 'U' 
		ELSE @SubscribeTypeCode 
	END,"));
				}else{
					columns.Append(string.Format("{0}=@{0},", colName));
				}
			}
			columns.Append("LastChanged=@ModifiedDate");
			return columns.ToString();
		}
		#endregion

		#region Methods to generate sql to update/insert UDF (support UDF history)
		private string GetUDFSql(ArrayList groupDataFields) {
			if (groupDataFields==null || groupDataFields.Count == 0) {
				return string.Empty;
			}

			StringBuilder udfSql = new StringBuilder();
			foreach(string udfColName in UserDefinedColumns) {
				GroupDataField gf = GroupDataField.GetGroupDataFieldsByShortName(groupDataFields, udfColName.Replace("user_",""));
				if (gf == null) {
					continue;
				}
				if (gf.SupportHistory) {
					AddNewLine(udfSql);
					udfSql.Append(GetSqlForUDFWithHistory(gf.ID,udfColName));
					continue;
				}
				AddNewLine(udfSql);
				udfSql.Append(GetSqlForUDFWithoutHistory(gf.ID, udfColName));
			}
			return udfSql.ToString();
		}

		private void AddNewLine(StringBuilder str) {
			if (str.Length > 0) {
				str.Append(System.Environment.NewLine);
			}
		}

		private string GetSqlForUDFWithHistory(int groupdDatafieldsID, string UDFColName) {
			return string.Empty;
			// return string.Format("INSERT INTO EmailDataValues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,EntryID) VALUES (@eid,{0},@{1},@ModifiedDate,NewID());",groupdDatafieldsID, UDFColName);
		}

		private string GetSqlForUDFWithoutHistory(int groupdDatafieldsID, string UDFColName) {
			return string.Format(@"SET @edvid=(SELECT EmailDataValuesID FROM EmailDataValues WHERE EmailID=@eid AND GroupDatafieldsID={0});
IF (@edvid IS NULL) 
BEGIN
	{1}
END
ELSE
BEGIN
	{2}
END", groupdDatafieldsID, GetSqlToInsertUDFWithoutHistory(groupdDatafieldsID, UDFColName), GetSqlToUpdateUDFWithoutHistory(UDFColName));
		}

		private string GetSqlToInsertUDFWithoutHistory(int groupdDatafieldsID, string UDFColName) {
			return string.Format("INSERT INTO EmailDataValues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate) VALUES (@eid,{0},@{1},@ModifiedDate);",groupdDatafieldsID, UDFColName);
		}
		private string GetSqlToUpdateUDFWithoutHistory(string UDFColName) {
			return string.Format("UPDATE EmailDataValues SET DataValue=@{0},ModifiedDate=@ModifiedDate WHERE EmailDataValuesID=@edvid;",UDFColName);
		}
		#endregion
	}	
}
