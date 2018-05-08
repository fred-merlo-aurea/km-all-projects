using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace ecn.common.classes {
	
	
	
	public class ECNErrors {

		#region Getters & Setters
		//defaults
		private int _errorID					= 0;
		private int _customerID			= 0;
		private string _product			= "";
		private string _errorType			= "";
		private string _errorPath			= "";
		private string _errorStack		= "";
		private string _status				= "";
		private string _assignedTo		= "";
		private string _notifyAM			= "";
		private string _internalNotes	= "";
		private string _dateUpdated	= DateTime.Now.ToString();	

		public int errorID {
			get { return this._errorID; }
			set { this._errorID = value; }
		}

		public int customerID {
			get { return this._customerID; }
			set { this._customerID = value; }
		}

		public string product {
			get { return this._product; }
			set { this._product = value; }
		}

		public string errorType {
			get { return this._errorType; }
			set { this._errorType = value; }
		}

		public string errorPath {
			get { return this._errorPath; }
			set { this._errorPath = value; }
		}

		public string errorStack {
			get { return this._errorStack; }
			set { this._errorStack = value; }
		}

		public string status {
			get { return this._status; }
			set { this._status = value; }
		}

		public string assignedTo {
			get { return this._assignedTo; }
			set { this._assignedTo = value; }
		}

		public string notifyAM {
			get { return this._notifyAM; }
			set { this._notifyAM = value; }
		}

		public string internalNotes {
			get { return this._internalNotes; }
			set { this._internalNotes = value; }
		}

		public string dateUpdated {
			get { return this._dateUpdated; }
		}

		public string ecn_misc_connString{
			get { return ConfigurationManager.AppSettings["ecn_misc_connString"]; }
		}

		public SqlConnection connection{
			get { return new SqlConnection(ecn_misc_connString); }
		}

		public string smtpServer{
			get { return ConfigurationManager.AppSettings["SmtpServer"]; }
		}

		public string errorsListSQL{
			get { return " SELECT * FROM ECNErrors "; }
		}
		
		#endregion

		#region Constructors
		//default Contructor.
		public ECNErrors() {}

		//Constructor passing params
		public ECNErrors(int customerID, string product, string errorType, string errorPath, string errorStack, string status, string internalNotes) {
			this._customerID		= customerID;
			this._product			= product;
			this._errorType		= errorType;
			this._errorPath			= errorPath;
			this._errorStack		= errorStack;
			this._status				= status;
			this._internalNotes	= internalNotes;
		}
		#endregion

		#region public Methods - Insert, Update, Delete
		//Insert Error Record
		public int insertECNError(){
			int newErrorID = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand insertCmd = new SqlCommand(null, connection);
			insertCmd.CommandText =	" INSERT INTO ECNErrors (CustomerID, Product, ErrorType, ErrorPath, ErrorStack, Status, InternalNotes) "+
														" VALUES "+
														" (@CustomerID, @Product, @ErrorType, @ErrorPath, @ErrorStack, @Status, @InternalNotes);SELECT @@IDENTITY";
        
			insertCmd.Parameters.Add ("@CustomerID", SqlDbType.Int).Value				= this.customerID;
			insertCmd.Parameters.Add ("@Product", SqlDbType.VarChar).Value				= this.product;
			insertCmd.Parameters.Add ("@ErrorType", SqlDbType.VarChar).Value			= this.errorType;
			insertCmd.Parameters.Add ("@ErrorPath", SqlDbType.VarChar).Value			= this.errorPath;
			insertCmd.Parameters.Add ("@ErrorStack", SqlDbType.VarChar).Value			= this.errorStack;
			insertCmd.Parameters.Add ("@Status", SqlDbType.VarChar).Value				= this.status;
			insertCmd.Parameters.Add ("@InternalNotes", SqlDbType.VarChar).Value		= this.internalNotes;

			connection.Open();
			try{
				newErrorID = Convert.ToInt32(insertCmd.ExecuteScalar());
			}catch{
				newErrorID = 0;
			}
			connection.Close();

			return newErrorID;
		}

		//Update Error Record
		public int updateECNError(int errorID){
			int updateStatus = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand updateCmd = new SqlCommand(null, connection);
			updateCmd.CommandText =	" UPDATE ECNErrors set Status=@Status, InternalNotes=@InternalNotes, AssignedTo=@AssignedTo, NotifyAM=@NotifyAM "+
															" WHERE ErrorID = @ErrorID ";
        
			updateCmd.Parameters.Add ("@Status", SqlDbType.VarChar).Value				= this.status;
			updateCmd.Parameters.Add ("@InternalNotes", SqlDbType.VarChar).Value	= this.internalNotes;
			updateCmd.Parameters.Add ("@AssignedTo", SqlDbType.VarChar).Value		= assignedTo;
			updateCmd.Parameters.Add ("@AssignedTo", SqlDbType.VarChar).Value		= notifyAM;
			updateCmd.Parameters.Add ("@ErrorID", SqlDbType.Int).Value						= errorID;

			connection.Open();
			try{
				updateStatus = Convert.ToInt32(updateCmd.ExecuteNonQuery());
			}catch{
				updateStatus = 0;
			}
			connection.Close();

			return updateStatus;
		}

		//Delete Error Record
		public int deleteECNError(int errorID){
			int deleteStatus = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand deleateCmd = new SqlCommand(null, connection);
			deleateCmd.CommandText =	" DELETE FROM ECNErrors WHERE ErrorID = @ErrorID ";
        
			deleateCmd.Parameters.Add ("@ErrorID", SqlDbType.Int).Value	= errorID;
			connection.Open();
			try{
				deleteStatus = Convert.ToInt32(deleateCmd.ExecuteNonQuery());
			}catch{
				deleteStatus = 0;
			}
			connection.Close();

			return deleteStatus;
		}
		#endregion

		#region public Methods - Errors - Listing
		
		/// Use this method to get the Errors list by passing the Key & its value 
		/// example: key = "Status", Value = "pending"
		
		/// <param name="whereClauseKey"></param>
		/// <param name="whereClauseKeyValue"></param>
		public DataTable getAllErrorsListByKey(string whereClauseKey, string whereClauseKeyValue){
			string statusListSQL = "";
			statusListSQL += errorsListSQL + " WHERE "+whereClauseKey+" = '"+whereClauseKeyValue+"' ";

			DataTable dt = getDataTable(	statusListSQL );
			return dt;
		}

		
		/// Use this method to to handle complex conditions in getting the Errors list by passing the Where Clause 
		/// example: whereClause = "CustomerID = 1 AND Status = 'pending' "
		
		/// <param name="whereClause"></param>
		public DataTable getAllErrorsListByWhereClause(string whereClause){
			string statusListSQL = "";
			statusListSQL += errorsListSQL + " WHERE "+whereClause;

			DataTable dt = getDataTable(	statusListSQL );
			return dt;
		}

		private DataTable getDataTable(string sql){
			DataTable dt = null;
			try{
				dt = DataFunctions.GetDataTable(sql, ecn_misc_connString);
			}catch{}

			return dt;
		}
		#endregion

		#region Public Methods - Send Notification Email
		public void notifyErrorToAdmins(){
			string emailbody = "";
			emailbody	= "<table width=100% border=0><tr><td style=\"font-family:'verdana'; font-size:11;\">";
			emailbody	+= "<b>"+errorType+"</b><br><br><b>Customer ID:</b>&nbsp;"+customerID;
			emailbody	+= "<br><b>Product:</b>&nbsp;"+product.ToString();
			emailbody	+= "<br><b>Error Page Path:</b>&nbsp;"+errorPath.ToString();
			emailbody	+= "<br><br><b>Error Stack:</b><br>"+errorStack.Replace("\n","<br>").ToString();;
			emailbody	+= "<br><br><b>Online Error Status:</b>&nbsp;<a href='http://localhost/ecnintranetreports/errorhandling/ecnerrorslist.aspx?errID="+errorID+"' >Click to Update Status</a>";
			emailbody	+= "</td></tr></table>";


            MailMessage message = new MailMessage();
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["ERRORRPT_FromEmail"].ToString());
            message.To.Add(System.Configuration.ConfigurationManager.AppSettings["ERRORRPT_ToEmail"].ToString());
            message.Subject = System.Configuration.ConfigurationManager.AppSettings["ERRORRPT_Subject"].ToString();
            message.Body = emailbody;
            message.IsBodyHtml = true;
            message.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]);
            smtp.Send(message);
			//end sending mail to the NBD.
		}
		#endregion
	}
}