using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace ecn.common.classes {
	
	/// This class provides basic OO access to the Email table. Pass in an email_id and you can access 
	/// and update column names and information. Warning, interface will change soon as I figgure out get/set methods.
	
	public class EmailAccessor {
		public EmailAccessor() {
			this.email_id = 0;
			string db_con_str = ConfigurationManager.AppSettings["connString"];
			this.email_ds = new DataSet();
			this.db_con = new SqlConnection(db_con_str);
			this.da = new SqlDataAdapter();
			this.da.TableMappings.Add("Table", "Emails");
		}
		// Sets up a SQL table with the values from The app.
		/*     public EmailAccessor( int eid) 
			 {

				 this.email_id = eid;
				 string db_con_str = "server=mustardseed.teckman.com;uid=ecn;pwd=tknow2003;database=ecn_communicator";
				 this.email_ds = new DataSet();
				 this.db_con = new SqlConnection(db_con_str);
				 this.da = new SqlDataAdapter();
				 this.da.TableMappings.Add("Table", "Emails");
				 // Create the SelectCommand.

				 SqlCommand cmd = new SqlCommand("SELECT * FROM Emails " +
					 "WHERE EmailID = @EmailID", db_con);

				 SqlParameter email_param = cmd.Parameters.Add("@EmailID", SqlDbType.Int, 4);
				 email_param.Value = email_id;
            
				 // and get the Email address   
				 this.da.SelectCommand = cmd;
				 this.da.Fill(email_ds);
			 }
		  */
		public EmailAccessor (int eid) {
			this.email_id = eid;
			string sqlBlastQuery=
				" SELECT * "+
				" FROM Emails "+
				" WHERE EmailID="+eid.ToString()+" ";
			DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery);
			email_data = dt.Rows[0];
		}
		public string FirstName() {            
			return email_data["FirstName"].ToString();
		}

		private int email_id;
		private SqlConnection db_con;
		private SqlDataAdapter da;
		private DataSet email_ds;
		private DataRow email_data;
	}
}
