using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;

namespace ecn.accounts.classes
{
	
	
	
	public class CallRecord
	{
		public CallRecord(int staffID, DateTime callDate, int callCount)
		{
			StaffID = staffID;
			CallDate = callDate;
			CallCount = callCount;
		}

		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}


		private int _staffID;
		public int StaffID {
			get {
				return (this._staffID);
			}
			set {
				this._staffID = value;
			}
		}

		private DateTime _callDate;
		public DateTime CallDate {
			get {
				return (this._callDate);
			}
			set {				
				this._callDate = new DateTime(value.Year, value.Month, value.Day);
			}
		}

		private int _callCount;
		public int CallCount {
			get {
				return (this._callCount);
			}
			set {
				this._callCount = value;
			}
		}

		#region Database Functions
		public void Save() {
			if (_id == -1) {
				Insert();
				return;
			} 

			Update();
		}

		private void Insert() {
			string sql = string.Format("INSERT INTO CallRecords (StaffID, CallDate, CallCount) VALUES ({0},'{1}',{2});select @@identity",StaffID,CallDate.ToShortDateString(),CallCount);
			_id =  Convert.ToInt32(DataFunctions.ExecuteScalar(sql));
		}

		private void Update() {
			string sql = string.Format("UPDATE CallRecords SET CallCount = {0} where callRecordID = {1}",CallCount, ID);
			DataFunctions.Execute(sql);
		}
		#endregion

		#region Static Methods
		public static CallRecord GetCallRecord(int staffID, DateTime callDate) {
			string sql = string.Format("Select * from CallRecords where StaffID = {0} and CallDate ='{1}'",staffID, callDate.ToShortDateString());
			DataTable dt = DataFunctions.GetDataTable(sql);
			if (dt.Rows.Count == 0) {
				return null;
			}

			DataRow row = dt.Rows[0];
			CallRecord record = new CallRecord(Convert.ToInt32(row["StaffID"]), Convert.ToDateTime(row["CallDate"]), Convert.ToInt32(row["CallCount"]));
			record.ID = Convert.ToInt32(row["CallRecordID"]);
			return record;
		}
		public static ArrayList GetCallRecords(int staffID, DateTime start, DateTime end) {
			string sql = string.Format("Select * from CallRecords where StaffID = {0} and CallDate >= '{1}' and CallDate<='{2}'",staffID, start.ToShortDateString(), end.ToShortDateString());
			DataTable dt = DataFunctions.GetDataTable(sql);
			
			ArrayList records = new ArrayList();
			foreach(DataRow row in dt.Rows) {
				CallRecord record = new CallRecord(Convert.ToInt32(row["StaffID"]), Convert.ToDateTime(row["CallDate"]), Convert.ToInt32(row["CallCount"]));
				record.ID = Convert.ToInt32(row["CallRecordID"]);
				records.Add(record);
			}
			return records;
		}
		#endregion
	}
}
