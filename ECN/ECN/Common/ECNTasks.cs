using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace ecn.common.classes {
	
	
	
	public class ECNTasks {

		#region Getters & Setters
		//defaults
		private int _taskID					= 0;
		private int _customerID			= 0;
		private string _product			= "";
		private string _taskType			= "";
		private string _taskPath			= "";
		private string _taskStack			= "";
		private string _status				= "";
		private string _priority				= "";
		private string _assignedTo		= "";
		private string _notifyAM			= "";
		private string _internalNotes	= "";
		private string _dateUpdated	= DateTime.Now.ToString();	

		public int taskID {
			get { return this._taskID; }
			set { this._taskID = value; }
		}

		public int customerID {
			get { return this._customerID; }
			set { this._customerID = value; }
		}

		public string product {
			get { return this._product; }
			set { this._product = value; }
		}

		public string taskType {
			get { return this._taskType; }
			set { this._taskType = value; }
		}

		public string taskPath {
			get { return this._taskPath; }
			set { this._taskPath = value; }
		}

		public string taskStack {
			get { return this._taskStack; }
			set { this._taskStack = value; }
		}

		public string status {
			get { return this._status; }
			set { this._status = value; }
		}

		public string priority {
			get { return this._priority; }
			set { this._priority = value; }
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

		public string TasksListSQL{
			get { return " SELECT * FROM ECNTasks "; }
		}
		
		#endregion

		#region Constructors
		//default Contructor.
		public ECNTasks() {}

		//Constructor passing params
		public ECNTasks(int customerID, string product, string taskType, string taskPath, string taskStack, string priority, string status, string internalNotes) {
			this._customerID		= customerID;
			this._product			= product;
			this._taskType			= taskType;
			this._taskPath			= taskPath;
			this._taskStack		= taskStack;
			this._status				= status;
			this._priority				= priority;
			this._internalNotes	= internalNotes;
		}
		#endregion

		#region public Methods - Insert, Update, Delete
		//Insert Tasks Record
		public int insertECNTask(){
			int newTaskID = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand insertCmd = new SqlCommand(null, connection);
			insertCmd.CommandText =	" INSERT INTO ECNTasks (CustomerID, Product, TaskType, TaskPath, TaskStack, Priority, Status, InternalNotes) "+
														" VALUES "+
														" (@CustomerID, @Product, @TaskType, @TaskPath, @TaskStack, @Priority, @Status, @InternalNotes);SELECT @@IDENTITY";
        
			insertCmd.Parameters.Add ("@CustomerID", SqlDbType.Int).Value		= this.customerID;
			insertCmd.Parameters.Add ("@Product", SqlDbType.VarChar).Value		= this.product;
			insertCmd.Parameters.Add ("@TaskType", SqlDbType.VarChar).Value		= this.taskType;
			insertCmd.Parameters.Add ("@TaskPath", SqlDbType.VarChar).Value		= this.taskPath;
			insertCmd.Parameters.Add ("@TaskStack", SqlDbType.VarChar).Value	= this.taskStack;
			insertCmd.Parameters.Add ("@Priority", SqlDbType.Text).Value		= this.priority;
			insertCmd.Parameters.Add ("@Status", SqlDbType.VarChar).Value		= this.status;
			insertCmd.Parameters.Add ("@InternalNotes", SqlDbType.Text).Value	= this.internalNotes;

			connection.Open();
			try{
				newTaskID = Convert.ToInt32(insertCmd.ExecuteScalar());
			}catch{
				newTaskID = 0;
			}
			connection.Close();

			return newTaskID;
		}

		public static int insertECNTask(int CustomerID, string Product, string TaskType, string TaskPath, string TaskStack, string Priority, string Status, string Notes)
		{
			int newTaskID = 0;
			SqlConnection connection	= new SqlConnection(ConfigurationManager.AppSettings["ecn_misc_connString"]);
			SqlCommand insertCmd = new SqlCommand(null, connection);
			insertCmd.CommandText =	" INSERT INTO ECNTasks (CustomerID, Product, TaskType, TaskPath, TaskStack, Priority, Status, InternalNotes) "+
				" VALUES "+
				" (@CustomerID, @Product, @TaskType, @TaskPath, @TaskStack, @Priority, @Status, @InternalNotes);SELECT @@IDENTITY";
        
			insertCmd.Parameters.Add ("@CustomerID", SqlDbType.Int).Value			= CustomerID;
			insertCmd.Parameters.Add ("@Product", SqlDbType.VarChar).Value			= Product;
			insertCmd.Parameters.Add ("@TaskType", SqlDbType.VarChar).Value			= TaskType;
			insertCmd.Parameters.Add ("@TaskPath", SqlDbType.VarChar).Value			= TaskPath;
			insertCmd.Parameters.Add ("@TaskStack", SqlDbType.VarChar).Value		= TaskStack;
			insertCmd.Parameters.Add ("@Priority", SqlDbType.Text).Value			= Priority;
			insertCmd.Parameters.Add ("@Status", SqlDbType.VarChar).Value			= Status;
			insertCmd.Parameters.Add ("@InternalNotes", SqlDbType.Text).Value		= Notes;

			connection.Open();
			try
			{
				newTaskID = Convert.ToInt32(insertCmd.ExecuteScalar());

				ECNTasks objtask = new ECNTasks();
				objtask.taskID = newTaskID;

				string notifyEmailBody = "";
				notifyEmailBody	= "<table width=100% border=0><tr><td style=\"font-family:'verdana'; font-size:11;\">";
				notifyEmailBody	+= "<b>"+TaskType+"</b><br>";
				notifyEmailBody	+= "<br><b>Product:</b>&nbsp;"+Product;
				notifyEmailBody	+= "<br><b>Customer ID:</b>&nbsp;"+CustomerID;
				notifyEmailBody	+= "<br><br><b>Task Stack:</b><br>"+TaskStack.Replace("\n","<br>").ToString();
				notifyEmailBody	+= "<br><br><b>Online Task Status:</b>&nbsp;<a href='http://www.ecn5.com/ecn.intranet.reports/taskhandling/ecntaskslist.aspx?tskID="+newTaskID+"' >Click to Update Status</a>";
				notifyEmailBody	+= "<br><b>Task Notes:</b>&nbsp;"+Notes;
				notifyEmailBody	+= "</td></tr></table>";

				objtask.notifyTaskstatus(notifyEmailBody);
			}
			catch
			{
				newTaskID = 0;
			}
			connection.Close();

			return newTaskID;
		}

		//Update Tasks Record
		public int updateECNTask(int taskID){
			int updateStatus = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand updateCmd = new SqlCommand(null, connection);
			updateCmd.CommandText =	" UPDATE ECNTasks set Proprity=@Priority, Status=@Status, InternalNotes=@InternalNotes, AssignedTo=@AssignedTo, NotifyAM=@NotifyAM "+
															" WHERE TaskID = @TaskID ";
        
			updateCmd.Parameters.Add ("@Priority", SqlDbType.VarChar).Value				= this.priority;
			updateCmd.Parameters.Add ("@Status", SqlDbType.VarChar).Value				= this.status;
			updateCmd.Parameters.Add ("@InternalNotes", SqlDbType.VarChar).Value	= this.internalNotes;
			updateCmd.Parameters.Add ("@AssignedTo", SqlDbType.VarChar).Value		= assignedTo;
			updateCmd.Parameters.Add ("@AssignedTo", SqlDbType.VarChar).Value		= notifyAM;
			updateCmd.Parameters.Add ("@TaskID", SqlDbType.Int).Value						= taskID;

			connection.Open();
			try{
				updateStatus = Convert.ToInt32(updateCmd.ExecuteNonQuery());
			}catch{
				updateStatus = 0;
			}
			connection.Close();

			return updateStatus;
		}

		//Delete Tasks Record
		public int deleteECNTask(int taskID){
			int deleteStatus = 0;
			SqlConnection connection	= new SqlConnection(ecn_misc_connString);
			SqlCommand deleateCmd = new SqlCommand(null, connection);
			deleateCmd.CommandText =	" DELETE FROM ECNTasks WHERE TaskID = @TaskID ";
        
			deleateCmd.Parameters.Add ("@TaskID", SqlDbType.Int).Value	= taskID;
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

		#region public Methods - Tasks - Listing
		
		/// Use this method to get the Tasks list by passing the Key & its value 
		/// example: key = "Status", Value = "pending"
		
		/// <param name="whereClauseKey"></param>
		/// <param name="whereClauseKeyValue"></param>
		public DataTable getAllTasksListByKey(string whereClauseKey, string whereClauseKeyValue){
			string statusListSQL = "";
			statusListSQL += TasksListSQL + " WHERE "+whereClauseKey+" = '"+whereClauseKeyValue+"' ";

			DataTable dt = getDataTable(	statusListSQL );
			return dt;
		}

		
		/// Use this method to to handle complex conditions in getting the Tasks list by passing the Where Clause 
		/// example: whereClause = "CustomerID = 1 AND Status = 'pending' "
		
		/// <param name="whereClause"></param>
		public DataTable getAllTasksListByWhereClause(string whereClause){
			string statusListSQL = "";
			statusListSQL += TasksListSQL + " WHERE "+whereClause;

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
		public void notifyTaskstatus(string emailbody){

            MailMessage message = new MailMessage();
            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["TASKRPT_FromEmail"].ToString());
            message.To.Add(System.Configuration.ConfigurationManager.AppSettings["TASKRPT_ToEmail"].ToString());
            message.Subject = System.Configuration.ConfigurationManager.AppSettings["TASKRPT_Subject"].ToString()+" Task ID: "+this.taskID;
            message.Body = emailbody;
            message.IsBodyHtml = true;
            message.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]);
            smtp.Send(message);
		}

        public void notifyTaskstatus(string fromEmail, string toEmail, string subject, string emailbody)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress((fromEmail.Length > 0) ? fromEmail : System.Configuration.ConfigurationManager.AppSettings["TASKRPT_FromEmail"].ToString());
            message.To.Add(toEmail);
            message.Subject = subject + " Task ID: " + this.taskID;
            message.Body = emailbody;
            message.IsBodyHtml = true;
            message.Priority = System.Net.Mail.MailPriority.High;
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]);
            smtp.Send(message);
        }
		#endregion
	}
}