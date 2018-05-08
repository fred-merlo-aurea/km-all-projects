using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using aspNetMX;
using ecn.common.classes;

namespace ecn.communicator.classes
{
	
	
	
	public class ValidationFunctions {

        ECN_Framework.Common.SecurityCheck sc;
		string connStr="";
		public ValidationFunctions(){
            sc = new ECN_Framework.Common.SecurityCheck();
			connStr = ConfigurationManager.AppSettings["connString"];
		}

		public int ValidateEmails(string GroupID) {

			int TotalRecords = 0;

			SqlCommand cmd = new SqlCommand("dbo.sp_MarkBadEmails");
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
			cmd.Parameters["@GroupID"].Value = GroupID;	
			try
			{
				TotalRecords = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));		
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
			return TotalRecords;

//--- commented by Sunil - 02/27/2007 - INVALID LOGIC - MARK ALL EMAILS START WITH NUMERIC AS BAD EMAILS - ADDED LOGIC IN Stored proc - see above.
//			string SQL = 
//				" SELECT e.EmailAddress, e.EmailID "+
//				" FROM Emails e JOIN EmailGroups eg ON e.EmailID = eg.EmailID "+
//                " WHERE eg.GroupID="+GroupID+
//                " AND eg.SubscribeTypeCode IN ('S','P') ";
//				
//            DataTable dt =DataFunctions.GetDataTable(SQL);
//			int i=0;
//
//			string emailAddress="", emailID = "";
//			string emailIDsToProcess = "";
//			Regex validator = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
//			
//			foreach(DataRow dr in dt.Rows) {
//				emailAddress	= dr["EmailAddress"].ToString();		
//				emailID			= dr["EmailID"].ToString();		
//
//				if( !(validator.IsMatch(emailAddress))) {
//					emailIDsToProcess += emailID+",";
//					i++;
//				}
//			}
//			
//			if(i > 0){
//				emailIDsToProcess = emailIDsToProcess.Substring(0, (emailIDsToProcess.Length - 1));
//				SQL = "UPDATE EmailGroups SET SubscribeTypeCode = 'D', LastChanged='"+DateTime.Now.ToString()+"' WHERE EmailID IN ( "+emailIDsToProcess+" )";
//				DataFunctions.ExecuteScalar(SQL);
//			}
//			return i;
		}


    	public int DeleteBad(string GroupID) {
			string SQLdelete = 
				" DELETE FROM EmailGroups "+
				" WHERE GroupID="+ GroupID +
				" AND SubscribeTypeCode='D'; SELECT @@rowcount; ";
			int i=Convert.ToInt32(DataFunctions.ExecuteScalar(SQLdelete));
			return i;
		}
	}
}
