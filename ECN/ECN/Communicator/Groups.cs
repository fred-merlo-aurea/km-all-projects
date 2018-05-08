using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using ecn.common.classes;

namespace ecn.communicator.classes
{
	
	/// The Group object handles all of the connections between Groups, Emails, and EmailGroups. This allowed us to break
	/// away from the GroupID's being tied to the emails. 
	
    public class Groups : DatabaseAccessor {

        int customer_id;
        string format_type_code;
        string subscriber_type_code;
		public static string collectordb			= ConfigurationManager.AppSettings["collectordb"];
		public static string commConString	= ConfigurationManager.AppSettings["com"];

		
		/// Usual ID constructor
		
		/// <param name="input_id"> GroupID</param>
        public Groups(int input_id):base(input_id) {
        }
		
		/// String version of ID constructor
		
		/// <param name="input_id"></param>
        public Groups(string input_id):base(input_id) {
        }

		
		/// Nullary constructor
		
        public Groups():base() {
        }
		
		/// Gets the CustomerID from the DB for this group
		
		/// <returns>CustomerID</returns>
        public int CustomerID() {
            // We should keep a prepaired handler around to get at this data fast I'm going with a simpler model for
            // rad development
			string custID = DataFunctions.ExecuteScalar("communicator", "Select CustomerID from Groups where GroupID=" + ID()).ToString();

            customer_id = Convert.ToInt32(custID.ToString());
            return customer_id;
        }
		
		/// Sets the customerID on this object
		
		/// <param name="new_id">CustomerID</param>
		/// <returns>CustomerID</returns>
        public int CustomerID(int new_id) {
            customer_id = new_id;
            return customer_id;
        }

		private string _name = string.Empty;
		public string Name {
			get {
				if (ID() <= 0) {
					throw new ApplicationException(string.Format("Fail to retrieve group value when ID is {0}", ID()));					
				}
				if (_name == string.Empty ) {
					LoadGroupFields();
				}
				return (this._name);
			}
			set {
				this._name = value;
			}
		}

		private void LoadGroupFields() {
			//string sqlquery= string.Format("select * from Groups where GroupID = {0}", ID());
			//DataTable dt = DataFunctions.GetDataTable(sqlquery);
			/*if (dt.Rows.Count == 0) {
				return;
			}
			_name = dt.Rows[0]["GroupName"].ToString();			
			*/
			string sqlquery= string.Format("select GroupName from Groups where GroupID = {0}", ID());
			string nm = DataFunctions.ExecuteScalar("communicator", sqlquery).ToString();
			if(nm.Length == 0){
				return;
			}else{
				_name = nm;
			}
		}

		private SortedList  _udfHash = new SortedList();
		public SortedList  UDFHash {
			get { 
				if (ID() <= 0) {
					throw new ApplicationException(string.Format("Fail to retrieve UDF for this group value when ID is {0}", ID()));					
				}
				_udfHash.Clear();
				LoadGroupDataFields();
				return (this._udfHash);			
			}
		}

		//check if this Group has UDF's
		public bool HasGroupDataFields(){
			string sqlquery= string.Format("SELECT COUNT(*) from GroupDataFields where GroupID = {0}", ID());
            string count = DataFunctions.ExecuteScalar("communicator", sqlquery).ToString();

			if(Convert.ToInt32(count) > 0){
				return true;
			}else{
				return false;
			}
		}

		//check if this Group has UDF History Fields (DataFieldSets)
		public bool HasHistoryGroupDataFields(){
			string sqlquery= string.Format("SELECT COUNT(*) from DataFieldSets where GroupID = {0}", ID());
            string count = DataFunctions.ExecuteScalar("communicator", sqlquery).ToString();

			if(Convert.ToInt32(count) > 0){
				return true;
			}else{
				return false;
			}
		}

		//check if this Group has any Survey Tied
		public bool HasSurvey(){
			string sqlquery= string.Format("SELECT COUNT(*) from "+collectordb+".dbo.survey where Group_ID = {0}", ID());
			string count = DataFunctions.ExecuteScalar(sqlquery).ToString();

			if(Convert.ToInt32(count) > 0){
				return true;
			}else{
				return false;
			}
		}

		//Load the UDF's in to a SortedList
		private void LoadGroupDataFields(){
			string sqlquery= string.Format("SELECT GroupDataFieldsID, ShortName FROM GroupDataFields WHERE GroupID = {0} ORDER BY GroupDataFieldsID", ID());
			DataTable dt = DataFunctions.GetDataTable(sqlquery, commConString);
			if (dt.Rows.Count == 0) {
				return;
			}else{
				System.Collections.SortedList ht = new System.Collections.SortedList();
				foreach(DataRow dr in dt.Rows) {
					ht.Add(dr["GroupDataFieldsID"].ToString(), dr["ShortName"].ToString());					
				}
				_udfHash = ht;
			}
		}

		
		/// Sets the Format Type code for any mass updates
		
		/// <param name="ftc">FormatTypeCode in EmailGroups</param>
		/// <returns>FormatTypeCode</returns>
        public string FormatTypeCode( string ftc) {
            format_type_code = ftc;
            return format_type_code;
        }
		
		/// Sets the Subscribe type code for mass upades
		
		/// <param name="stc">SubscribeTypeCode in EmailGroups</param>
		/// <returns>SubscribeTypeCode</returns>
        public string SubscribeTypeCode(string stc) {
            subscriber_type_code = stc;
            return subscriber_type_code;
        }
		
		/// put in a new email from the forward to a friend object, it will attach the email to the group the blast was sent out with
		
		/// <param name="emailaddress">Email to Insert</param>
		/// <param name="fullname">Fullname of Person</param>
		/// <param name="note">Note to send</param>
		/// <returns>Email object of the newly created</returns>
        public Emails InsertEmailFromForward(string emailaddress, string fullname, string note) {
            Emails my_email = new Emails();
            my_email.InsertEmailFromForward(emailaddress,fullname,note,CustomerID());
            AttachEmail(my_email,"html","P");
            return my_email;
        }

		
		/// Attach an email to the group with the html and subscribed bits set
		
		/// <param name="to_attach">email to attach</param>
		/// <returns>number of email group attachments</returns>
       /* public int AttachHtmlSubscribe(Emails to_attach) {
            return AttachEmail(to_attach,"html","S");
        }*/

		
		/// Attach an email to the group with the html and pending bits set
		
		/// <param name="to_attach">email to attach</param>
		/// <returns>number of email group attachments</returns>
        /*public int AttachHtmlPending(Emails to_attach, ) {
            return AttachEmail(to_attach,"html","P");
        }*/


		
		/// Attach an email to the group based on the format and subscribe type stored on this object
		
		/// <param name="to_attach">email to attach</param>
		/// <returns>number of email group attachments</returns>
        public int AttachEmail(Emails to_attach) {
            return AttachEmail(to_attach,format_type_code,subscriber_type_code);
        }

		
		/// Ataches an email to this group. If our ID() == 0, then attach to all groups
		
		/// <param name="to_attach">Email to attach</param>
		/// <param name="format">Format to attach with</param>
		/// <param name="status">status to attach with</param>
		/// <returns>number of groups it added this email to</returns>
        public int AttachEmail( Emails to_attach, string format, string status) {
            // a 0 group ID means ALL Groups.

            if ( ID() == 0) {
                int i = 0;
                DataTable groups = DataFunctions.GetDataTable("Select * from Groups where CustomerID = " + CustomerID(), commConString);
                foreach(DataRow dr in groups.Rows) {
                    ModifiyEmailGroup(to_attach.ID(),Convert.ToInt32(dr["GroupID"]),format,status);
                    i++;
                }
                return i;
            } else {
                ModifiyEmailGroup(to_attach.ID(),ID(),format,status);
                return 1;
            }
        }

		
		/// helper function thatchanges the EmailGroup table or creates a new row if needed
		
		/// <param name="eid">EmailID from the Emails Table</param>
		/// <param name="gid"> GroupID from the Groups Table</param>
		/// <param name="format">FormatTypeCode</param>
		/// <param name="status">SubscribeTypeCode</param>
        public void ModifiyEmailGroup(int eid, int gid, string format, string status) {
            
			SqlCommand cmd = new SqlCommand("dbo.sp_SaveEmailGroup");
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
			cmd.Parameters["@GroupID"].Value = gid;	
			cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
			cmd.Parameters["@EmailID"].Value = eid;		

			cmd.Parameters.Add(new SqlParameter("@FormatTypeCode", SqlDbType.VarChar));
			cmd.Parameters["@FormatTypeCode"].Value = format;	
			cmd.Parameters.Add(new SqlParameter("@SubscribeTypeCode", SqlDbType.VarChar));
			cmd.Parameters["@SubscribeTypeCode"].Value = status;	

			try
			{
				DataFunctions.Execute("communicator", cmd);
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
        }

		
		/// helper function thatchanges the EmailDataValues table for that UDF or creates a new row if needed
		
		/// <param name="to_attach">emailaddress from the Emails Table</param>
		/// <param name="UDFid"> GroupDataFieldsID from the GroupsDataFields Table</param>
		/// <param name="UDFData">UDF data for that UDF from the Form</param>
		public void AttachUDFToEmail(Emails to_attach, string UDFid, string UDFdata, string EntryID) {
			//string emailsQuery= "";
			int emailID = to_attach.ID();
			SqlConnection conn = GetDbConnection("communicator");
			//emailsQuery = "SELECT EmailDataValuesID from EmailDataValues where EmailID = " + emailID + " AND GroupDataFieldsID = " + UDFid;
			//object edvid = DataFunctions.ExecuteScalar(emailsQuery);
			//if(null == edvid) {
				/*emailsQuery=
					" INSERT INTO EmailDataValues ( "+
					" EmailID, GroupDataFieldsID, DataValue, "+
					" ModifiedDate, SurveyGridID, EntryID "+
					" ) VALUES ( "+
					emailID +", "+UDFid+",'"+UDFdata+"', '"+
					DateTime.Now.ToString()+"', '-1', '"+EntryID+"' "+
					" ); ";
					*/

            if (UDFdata.Trim().Length > 0)
            {

                SqlCommand InsertCommand = new SqlCommand(null, conn);
                InsertCommand.CommandText =
                    " INSERT INTO EmailDataValues ( " +
                    " EmailID, GroupDataFieldsID, DataValue, ModifiedDate, SurveyGridID, EntryID " +
                    " ) VALUES ( " +
                    " @emailID,@UDFid,@UDFdata,@ModifiedDate,@SurveyID,@EntryID) ";

                InsertCommand.Parameters.Add("@emailID", SqlDbType.Int, 4).Value = emailID;
                InsertCommand.Parameters.Add("@UDFid", SqlDbType.Int, 4).Value = UDFid;
                InsertCommand.Parameters.Add("@UDFdata", SqlDbType.VarChar, 500).Value = UDFdata;
                InsertCommand.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = DateTime.Now;
                InsertCommand.Parameters.Add("@SurveyID", SqlDbType.Int, 4).Value = -1;
                InsertCommand.Parameters.Add("@EntryID", SqlDbType.VarChar).Value = EntryID;

                InsertCommand.CommandTimeout = 0;
                conn.Open();
                InsertCommand.ExecuteNonQuery();
                conn.Close();
            }
			//} else {
				//DONOT UPDATE the Record. Its a history Item & It WILL NOT be updated. 
				// it was wiping up the data clean for 2ndwind Transactions History.. !
				/*SqlCommand upd = new SqlCommand(null,GetDbConnection());
				upd.CommandText = " UPDATE EmailDataValues SET DataValue = @DataValue, ModifiedDate=@lastChanged "+ 
					" WHERE EmailID = @emailID AND GroupDataFieldsID = @UDFid";

				upd.Parameters.Add ("@UDFid",SqlDbType.Int,4,"GroupDatafieldsID").Value = Convert.ToInt32(UDFid.ToString());
				upd.Parameters.Add ("@DataValue",SqlDbType.VarChar,255,"DataValue").Value = UDFdata;
				upd.Parameters.Add ("@emailID", SqlDbType.Int,4,"EmailID").Value = emailID;
				upd.Parameters.Add ("@lastChanged", SqlDbType.DateTime,8,"LastChanged").Value = DateTime.Now;
				upd.Prepare();
				upd.ExecuteNonQuery();
				*/
			//}
		}
		
		public void AttachUDFToEmail(Emails to_attach, string UDFid, string UDFdata) {
			string emailsQuery= "";
			SqlConnection conn = GetDbConnection("communicator");
			int emailID = to_attach.ID();
			emailsQuery = "SELECT EmailDataValuesID from EmailDataValues where EmailID = " + emailID + " AND GroupDataFieldsID = " + UDFid;
			object edvid = DataFunctions.ExecuteScalar("communicator", emailsQuery);
			if(null == edvid) {

                if (UDFdata.Trim().Length > 0)
                {
                    SqlCommand InsertCommand = new SqlCommand(null, conn);
                    try
                    {
                        InsertCommand.CommandText =
                            " INSERT INTO EmailDataValues ( " +
                            " EmailID, GroupDataFieldsID, DataValue, ModifiedDate, SurveyGridID " +
                            " ) VALUES ( " +
                            " @emailID,@UDFid,@UDFdata,@ModifiedDate,@SurveyID) ";

                        InsertCommand.Parameters.Add("@emailID", SqlDbType.Int, 4).Value = emailID;
                        InsertCommand.Parameters.Add("@UDFid", SqlDbType.Int, 4).Value = UDFid;
                        InsertCommand.Parameters.Add("@UDFdata", SqlDbType.VarChar, 500).Value = UDFdata;
                        InsertCommand.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        InsertCommand.Parameters.Add("@SurveyID", SqlDbType.Int, 4).Value = -1;

                        InsertCommand.CommandTimeout = 0;
                        conn.Open();
                        InsertCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
			} else {
				SqlCommand upd = new SqlCommand(null,conn);
				try{
					upd.CommandText = " UPDATE EmailDataValues SET DataValue = @DataValue, ModifiedDate=@lastChanged "+ 
						" WHERE EmailID = @emailID AND GroupDataFieldsID = @UDFid";

					upd.Parameters.Add ("@UDFid",SqlDbType.Int,4,"GroupDatafieldsID").Value = Convert.ToInt32(UDFid.ToString());
					upd.Parameters.Add ("@DataValue",SqlDbType.VarChar,255,"DataValue").Value = UDFdata;
					upd.Parameters.Add ("@emailID", SqlDbType.Int,4,"EmailID").Value = emailID;
					upd.Parameters.Add ("@lastChanged", SqlDbType.DateTime,8,"LastChanged").Value = DateTime.Now;
					conn.Open();
					upd.Prepare();
					upd.ExecuteNonQuery();
					conn.Close();
				}catch(Exception ex){
					ex.ToString();
				}
			}
		}



		
		/// Ask if this group has this email address
		
		/// <param name="address">Email address to query</param>
		/// <returns>Email object of match or null</returns>
        public Emails WhatEmail(string address) 
        {
            string existsQuery = "SELECT e.EmailID FROM Emails e, EmailGroups eg  " +
                " WHERE e.EmailAddress = @EmailAddress" +
                " AND e.CustomerID = @CustomerID" +
                " AND eg.GroupID = @GroupID" +
                " AND eg.EmailID = e.EmailID ";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            cmd.CommandText = existsQuery;
            cmd.Parameters.AddWithValue("@EmailAddress", address);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID());
            cmd.Parameters.AddWithValue("@GroupID", ID());

            object theEmailID = DataFunctions.ExecuteScalar(cmd);
            if(null == theEmailID) 
                return null;
            return new Emails(Convert.ToInt32(theEmailID));
        }

        public bool HasPendingEmail(Emails my_email)
        {
            string existsQuery = "Select emailgroupid from emailgroups where groupid= " + ID() + " AND emailid=" + my_email.ID() + " AND subscribetypecode = 'P'";
            object is_in_group = DataFunctions.ExecuteScalar("communicator", existsQuery);
            if (null == is_in_group)
                return false;
            return true;
        }
		
		/// Rewrite the above method to this new one using the logic from Import Objects 
		/// Check to see if the email exists in the Customer first.. Since we were using the above method it was allowing 
		/// duplicate profiles to exist for the same customer account. WHICH SHOULD NOT BE. it might return more than one value
		/// catch the exception & get the top1 EmailID from the list.. - ashok 1/16/2005
		
		/// <param name="address">Email address to query</param>
		/// <returns>Email object of match or null</returns>
		public Emails WhatEmailForCustomer(string address) {
			object theEmailID = null;
			string existsQuery= string.Format(@"SELECT EmailID FROM Emails WHERE EmailAddress=@EmailAddress AND CustomerID=@CustomerID");
			SqlCommand cmd1 = null;
			SqlCommand cmd2 = null;
			SqlCommand cmd3 = null;
			try{
				cmd1 = new SqlCommand(existsQuery, new SqlConnection(DataFunctions.con_communicator));
				cmd1.Parameters.Add("@EmailAddress",SqlDbType.VarChar, 200).Value =  address;
				cmd1.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = CustomerID();	
				cmd1.Connection.Open();
				theEmailID = cmd1.ExecuteScalar();
				cmd1.Connection.Close();
				//Now only one occurence of this email address exists in this Customer.. 
				//return the EmailID (theEmailID)
			}catch{
				//Control comes here if the SubQuery returned more than 1 EmailID
				//So We know that Email address exists in that Customer. 
				//Check to see if that emailaddress exists in the group.
				try{
					existsQuery= string.Format(@"SELECT e.EmailID FROM Emails e join emailgroups eg on e.emailID = eg.emailID 
WHERE EmailAddress=@EmailAddress AND CustomerID=@CustomerID and eg.GroupID = @GroupID");
					cmd2 = new SqlCommand(existsQuery, new SqlConnection(DataFunctions.connStr));
					cmd2.Parameters.Add("@EmailAddress",SqlDbType.VarChar, 200).Value =  address;
					cmd2.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = ID();
					cmd2.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = CustomerID();	
					cmd2.Connection.Open();
					theEmailID = cmd2.ExecuteScalar();
					cmd2.Connection.Close();
					//If it exists return the EmailID  (theEmailID)
				}catch{
					
				}

				if(null == theEmailID){
					//Email doesn't exist in that Group 'cos EmailID is null so far.
					//Since we know that the email address exisits in this customer for another group, 
					//get the Top1 EmailID & return.
					try{
						existsQuery= string.Format(@"SELECT TOP 1 EmailID FROM Emails WHERE EmailAddress=@EmailAddress AND CustomerID=@CustomerID");
						cmd3 = new SqlCommand(existsQuery, new SqlConnection(DataFunctions.connStr));
						cmd3.Parameters.Add("@EmailAddress",SqlDbType.VarChar, 200).Value =  address;
						cmd3.Parameters.Add("@GroupID", SqlDbType.Int, 4).Value = ID();
						cmd3.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = CustomerID();	
						cmd3.Connection.Open();
						theEmailID = cmd3.ExecuteScalar();
						cmd3.Connection.Close();		
					}catch{
				
					}
				}
			}
			finally{
				//Close all the connections if they are open.
				if(!(cmd1 == null))
					cmd1.Connection.Close();
				if(!(cmd2 == null))
					cmd2.Connection.Close();
				if(!(cmd3 == null))
					cmd3.Connection.Close();
			}
			if(null == theEmailID) 
				return null;
			return new Emails(Convert.ToInt32(theEmailID));
		}
    }
}
