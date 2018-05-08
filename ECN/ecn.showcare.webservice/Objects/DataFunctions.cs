using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace ecn.showcare.webservice.Objects {
	public class DataFunctions {
		public static string connStr=ConfigurationManager.AppSettings["connString"];
		public static string con_communicator=ConfigurationManager.AppSettings["com"];
		public static string con_accounts=ConfigurationManager.AppSettings["act"];

		//generic functions (method overload 1)
		public static DataTable GetDataTable(string SQL) {
			DataSet ds = new DataSet();
			SqlDataAdapter adapter = new SqlDataAdapter(SQL,connStr);
			//adapter.SelectCommand.CommandTimeout = 0;			
			adapter.Fill(ds,"DataTable");
			adapter.SelectCommand.Connection.Close();
			//adapter.Dispose();
			return ds.Tables["DataTable"];         
		}

		// method used for Data Mapper 
		//OverLoad the method with 2 params, by passing sql & the connString (method overload 2 )
		public static DataTable GetDataTable(string SQL, string connString) {
			DataSet ds = new DataSet();
			SqlDataAdapter adapter = new SqlDataAdapter(SQL,connString);
			//adapter.SelectCommand.CommandTimeout = 0;
			adapter.Fill(ds,"DataTable");
			adapter.SelectCommand.Connection.Close();
			//adapter.Dispose();
			return ds.Tables["DataTable"];         
		}

		// method used for Data Mapper 
		// Datatable as a param (method overload 1)
		public static ArrayList GetDataTableColumns(DataTable dataTable) {
			int nColumns = dataTable.Columns.Count;
			ArrayList columnHeadings = new ArrayList();
			DataColumn dataColumn = null;
			for(int i=0; i < nColumns; i++){
				dataColumn		= dataTable.Columns[i];
				columnHeadings.Add(dataColumn.ColumnName.ToString());
			}
			return columnHeadings;
		}

		// method used for Data Mapper 
		// Table name as a param (method overload 2 )
		public static ArrayList GetDataTableColumns(string tableName) {
			string sqlString = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS "+
				" WHERE TABLE_NAME = "+tableName;
			ArrayList columnHeadings = new ArrayList();
			DataTable dt = GetDataTable(sqlString);
			foreach ( DataRow dr in dt.Rows ) {
				columnHeadings.Add(dr["COLUMN_NAME"].ToString());
			}

			return columnHeadings;
		}

		public static int Execute(string SQL) {
			SqlConnection conn = new SqlConnection(connStr);
			SqlCommand cmd = new SqlCommand(SQL,conn);
			cmd.Connection.Open();
			int success =  cmd.ExecuteNonQuery();
			conn.Close();
			return success;
		}

		public static object ExecuteScalar(string db, string SQL) {
			SqlConnection conn;
			switch(db) {
				case "accounts":
					conn = new SqlConnection(con_accounts);
					break;
				case "communicator":
					conn = new SqlConnection(con_communicator);
					break;
				default:
					conn = new SqlConnection(connStr);
					break;
			}

			SqlCommand cmd = new SqlCommand(SQL,conn);
			cmd.Connection.Open();
			object obj = cmd.ExecuteScalar(); 
			conn.Close();
			return obj; 
		}

		public static object ExecuteScalar(string SQL) {
			return ExecuteScalar("default",SQL);
		}
	}
}
