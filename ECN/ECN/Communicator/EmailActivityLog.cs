using System;
using ecn.common.classes;
namespace ecn.communicator.classes {
    
    /// Handles the inserting of EmailActivityLogs as done by the "engines" directory. The static event inserters also
    /// run the event engine to process any requests that fire based on the event added.
    
    public class EmailActivityLog : DatabaseAccessor {

        private Blasts _blast = null;
        private Emails _email = null;
        private Groups _group = null;
        private int _smartformID = 0;


		/// Usual Int constructor
		
		/// <param name="input_id">EAID</param>
        public EmailActivityLog(int input_id):base(input_id) {
        }
		
		/// String constructor
		
		/// <param name="input_id">EAID</param>
        public EmailActivityLog(string input_id):base(input_id) {
        }
		
		/// Nullary Constructor
		
        public EmailActivityLog():base() { }

		
		/// Gets the blast ID for this EmailAcitity.
		
		/// <returns>Blast of this log record</returns>
		
		
        public Blasts Blast() {            
            if(_blast == null && ID() != 0) {
				_blast = new Blasts();
                _blast.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select BlastID from EmailActivityLog where EAID=" + ID())));                
            } 			
            return _blast;
        }
		public void SetBlast(Blasts blast) {
			_blast = blast;
		}
		
		/// Gets the email that this record points to.
		
		/// <returns>Email linked to this log</returns>
		
        public Emails Email() {            
            if (_email == null && ID() != 0) {
				_email = new Emails();
                _email.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select EmailID from EmailActivityLog where EAID=" + ID())));              
            } 
            return _email;
        }

		public void SetEmail(Emails email) {
			_email = email;
		}
		
		/// Gets the Layout associated with this email acitivty log based on the blast ID
		
		/// <returns>Corrisponding Layout</returns>
        public Layouts Layout() {
            Layouts my_layout = new Layouts();
            if(ID() != 0) {
                my_layout.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select b.LayoutID from EmailActivityLog l, Blast b where l.EAID=" + ID() + " AND b.BlastID = l.BlastID and b.StatusCode <> 'Deleted'")));
                return my_layout;
            } 
            return null;
        }

		
		/// Gets the Group associated with this email acitivity log based on the blast
		
		/// <returns>Group of blast</returns>
		
        public Groups Group() 
        {            
            if(_group==null && ID() != 0) 
            {
				_group = new Groups();
                _group.ID(Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", "Select b.GroupID from EmailActivityLog l, Blast b where l.EAID=" + ID() + " AND b.BlastID = l.BlastID and b.StatusCode <> 'Deleted'")));                
            } 
            return _group;
        }
		public void SetGroup(Groups group) {
			_group = group;
		}

        public int SmartFormID
        {
            get
            {
                return (this._smartformID);
            }
            set
            {
                _smartformID = value;
            }
        }
		
		/// Returns the ActionTypeCode of EmailActivityLog
		
		/// <returns>ActionTypeCode</returns>
        public string EventType() {

            if(ID() != 0) {
                return DataFunctions.ExecuteScalar("communicator", "Select ActionTypeCode from EmailActivityLog with (nolock) where EAID=" + ID()).ToString();
            } 
            return "";
        }

		
		/// Returns the Action Value or EmailActivityLog
		
		/// <returns>ActionValue</returns>
        public string ActionValue() {
            if(ID() != 0) {
                return DataFunctions.ExecuteScalar("communicator", "Select ActionValue from EmailActivityLog with (nolock) where EAID=" + ID()).ToString();
            } 
            return "";
        }


		
		/// General Event Inesert function. Statically mapped to class to allow events to get added from anyone who has 
		/// an email id and a blast. (and type of course)
		
		/// <param name="EmailID">Email id to insert into EmailActivityLog</param>
		/// <param name="BlastID">Blast to instert into EmailActivityLog</param>
		/// <param name="event_type">open, subscribe, click, forward </param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static int GeneralInsert(int EmailID, int BlastID, string event_type, string info) 
		{
            if(info.Length > 2048) {
                info = info.Substring(0, 2048);
            }
            string SQLQuery = string.Empty;
            
            if (event_type.ToLower() == "click")
            {
                SQLQuery = " if not exists (select top 1 EAID from EmailActivityLog where EmailID = @EmailID and BlastID = @BlastID and ActionTypeCode = @ActionTypeCode and datediff(ss,ActionDate,getdate()) <= 5) ";
            }
            else if (event_type.ToLower() == "open" || event_type.ToLower() == "read")
            {
                SQLQuery = " if not exists (select top 1 EAID from EmailActivityLog where EmailID = @EmailID and BlastID = @BlastID and ActionTypeCode = @ActionTypeCode and datediff(ss,ActionDate,getdate()) <= 2) ";
            }
            SQLQuery += "INSERT INTO EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate) VALUES (@EmailID, @BlastID, @ActionTypeCode, @ActionValue, GetDate()); SELECT @@IDENTITY;";

            //-- for tracking UNIQUE open & click records per profile - commented on 06/25/2010 - as per Iris. 
            //if (event_type.ToLower() == "read" || event_type.ToLower() == "open" ) 
            //    SQLQuery = " IF NOT EXISTS (SELECT TOP 1 EAID FROM EmailActivityLog WHERE EmailID = " + EmailID + " AND BlastID = " + BlastID + " AND ActionTypeCode = '" + event_type + "') ";
            //else if (event_type.ToLower() == "click")
            //    SQLQuery = " IF NOT EXISTS (SELECT TOP 1 EAID FROM EmailActivityLog WHERE EmailID = " + EmailID + " AND BlastID = " + BlastID + " AND ActionTypeCode = '" + event_type + "' AND ActionValue='" + info + "') ";

           System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
           cmd.CommandType = System.Data.CommandType.Text;
           cmd.CommandText = SQLQuery;
           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmailID", EmailID));
           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BlastID", BlastID));
           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionTypeCode", event_type));
           cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ActionValue", info));

            int eaid = 0;
            try {
                eaid = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
                ProcessEvent(eaid);
            } catch { }

			return eaid;
		}

        public static int InsertOpen(int EmailID, int BlastID, string spyinfo) {
			return GeneralInsert(EmailID,BlastID,"open",spyinfo);
        }

        public static int InsertRead( int EmailID, int BlastID, string spyinfo ) {
            return GeneralInsert(EmailID, BlastID, "read", spyinfo);
        }

        public static int InsertClick(int EmailID, int BlastID, string Link, string spyInfo) {
			int opensCount = GetOpensCount(EmailID, BlastID);
			if(opensCount > 0){
				return GeneralInsert(EmailID,BlastID,"click",Link);
			}else{
				InsertOpen(EmailID, BlastID, spyInfo);
				return GeneralInsert(EmailID,BlastID,"click",Link);
			}
        }

        public static int InsertSubscribe(int EmailID, int BlastID, string theSubscribe) {
			return GeneralInsert(EmailID,BlastID,"subscribe",theSubscribe);

        }
		public static int InsertForwardToFriend(int EmailID, int BlastID, string email_address) 
		{
			return GeneralInsert(EmailID,BlastID,"refer",email_address);
		}

		
		/// Processes an Insert event ensuring that any plans based on these actions get inserted into the system.
		
		/// <param name="ActivityID">EAID</param>
        private static void ProcessEvent(int ActivityID) {
            EmailActivityLog my_event = new EmailActivityLog(ActivityID);

            Blasts my_blast = my_event.Blast();

            // Find this customer's eventer
            EventOrganizer eventer = new EventOrganizer();
            eventer.CustomerID(my_blast.CustomerID());

            // Process the ruleset for this event
            eventer.Event(my_event);
        }

		private static int GetOpensCount(int emailID, int blastID){
			int opensCnt = 0;
			string opensCntSQL = " SELECT COUNT(EmailID) "+
												" FROM EmailActivityLog "+
												" WHERE EmailID = "+emailID+" AND BlastID = "+blastID+" AND ActionTypeCode = 'open' ";
			try{
                opensCnt = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", opensCntSQL));
			}catch{
				opensCnt = 0;
			}

			return opensCnt;
		}
    }
}
