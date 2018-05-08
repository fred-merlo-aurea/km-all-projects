using System;
using ecn.common.classes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ecn.publisher.classes
{
	public class Publications
	{
		public Publications()
		{
		} 

		private int _PublicationID = 0;
		private string _PublicationName = "";
        private string _PublicationType = "";
        private int _categoryID = 0;
        private int _circulation = 0;
        private int _frequencyID = 0;
        private string _PublicationCode = "";
        private int _customerID = 0;
        private int _groupID = 0;
		private bool _active = true;
        private string _ContactAddress1 = "";
        private string _ContactAddress2 = "";
        private string _ContactEmail = "";
        private string _ContactPhone = "";
        private string _ContactFormLink = "";
        private bool _enablesubscription = true;
        private int _subscriptionOption = 0;
        private string _SubscriptionFormLink = "";
        private string _LogoURL = "";
        private string _LogoLink = "";
        private int _UserID = 0;

		public int PublicationID 
		{
			get {return (this._PublicationID);}
			set {this._PublicationID = value;}
		}

        public string PublicationName
        {
            get { return (this._PublicationName); }
            set { this._PublicationName = value; }
        }

        public string PublicationType
        {
            get { return (this._PublicationType); }
            set { this._PublicationType = value; }
        }

        public int CategoryID
        {
            get { return (this._categoryID); }
            set { this._categoryID = value; }
        }

        public int Circulation
        {
            get { return (this._circulation); }
            set { this._circulation = value; }
        }

        public int FrequencyID
        {
            get { return (this._frequencyID); }
            set { this._frequencyID = value; }
        }

        public string PublicationCode
        {
            get { return (this._PublicationCode); }
            set { this._PublicationCode = value; }
        }

		public int CustomerID 
		{
			get {return (this._customerID);}
			set {this._customerID = value;}
		}

        public int GroupID
        {
            get { return (this._groupID); }
            set { this._groupID = value; }
        }
		
        public bool Active 
		{
			get {return (this._active);}
			set {this._active = value;}
		}

        public string ContactAddress1
        {
            get { return (this._ContactAddress1); }
            set { this._ContactAddress1 = value; }
        }

        public string ContactAddress2
        {
            get { return (this._ContactAddress2); }
            set { this._ContactAddress2 = value; }
        }

        public string ContactEmail
        {
            get { return (this._ContactEmail); }
            set { this._ContactEmail = value; }
        }

        public string ContactPhone
        {
            get { return (this._ContactPhone); }
            set { this._ContactPhone = value; }
        }

        public string ContactFormLink
        {
            get { return (this._ContactFormLink); }
            set { this._ContactFormLink = value; }
        }
        
        public bool EnableSubscription 
		{
			get {return (this._enablesubscription);}
			set {this._enablesubscription = value;}
		}

        public int SubscriptionOption
        {
            get { return (this._subscriptionOption); }
            set { this._subscriptionOption = value; }
        }

        public string SubscriptionFormLink
        {
            get { return (this._SubscriptionFormLink); }
            set { this._SubscriptionFormLink = value; }
        }

        public string LogoURL
        {
            get { return (this._LogoURL); }
            set { this._LogoURL = value; }
        }

        public string LogoLink
        {
            get { return (this._LogoLink); }
            set { this._LogoLink = value; }
        }

        public int UserID
        {
            get { return (this._UserID); }
            set { this._UserID = value; }
        }

        public Publications(int PublicationID, string PublicationName, string PublicationCode, int customerID, bool active, int GroupID, bool EnableSubscription, string contactEmail, string contactphone, string contactAddress1, string contactAddress2) 
		{
			_PublicationID = PublicationID;
			_PublicationName = PublicationName;
            _PublicationCode = PublicationCode;
			_customerID = customerID;
			_active = active;
			_groupID = GroupID;
			_enablesubscription = EnableSubscription;
			_ContactEmail = contactEmail;
			_ContactPhone = contactphone;
			_ContactAddress1 = contactAddress1;
			_ContactAddress2 = contactAddress2;

		}

		public static DataTable getPublications(int CustomerID)
		{
            return DataFunctions.GetDataTable("select m.PublicationID, m.PublicationName, m.PublicationCode, (case when m.Active=1 then 'Yes' else 'No' end) as 'Active',  count(case when e.status='Active' then 1 end) as 'ActiveEdition', count(case when e.status='Archieve' then 1 end) as 'ArchievedEdition' from Publication m left outer join Edition e on m.PublicationID = e.PublicationID where CustomerID = " + CustomerID + " group by  m.PublicationID, m.PublicationName,m.PublicationCode, m.active order by m.PublicationName", ConfigurationManager.AppSettings["pub"]);					
		}

		public int Save() 
		{
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["pub"]);

			SqlCommand cmd = new SqlCommand("dbo.sp_SavePublication", conn);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@PublicationID", SqlDbType.Int));
			cmd.Parameters["@PublicationID"].Value = PublicationID;		
			cmd.Parameters.Add(new SqlParameter("@PublicationName", SqlDbType.VarChar));
			cmd.Parameters["@PublicationName"].Value = PublicationName;
            cmd.Parameters.Add(new SqlParameter("@PublicationType", SqlDbType.VarChar));
            cmd.Parameters["@PublicationType"].Value = PublicationType;
            cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int));
            cmd.Parameters["@CategoryID"].Value = CategoryID;
            cmd.Parameters.Add(new SqlParameter("@Circulation", SqlDbType.Int));
            cmd.Parameters["@Circulation"].Value = Circulation;
            cmd.Parameters.Add(new SqlParameter("@FrequencyID", SqlDbType.Int));
            cmd.Parameters["@FrequencyID"].Value = FrequencyID;	
            cmd.Parameters.Add(new SqlParameter("@PublicationCode", SqlDbType.VarChar));
            cmd.Parameters["@PublicationCode"].Value = PublicationCode;	
            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
			cmd.Parameters["@CustomerID"].Value = CustomerID;
            cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            cmd.Parameters["@GroupID"].Value = GroupID;	
			cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
			cmd.Parameters["@Active"].Value =Active;
            cmd.Parameters.Add(new SqlParameter("@ContactAddress1", SqlDbType.VarChar));
            cmd.Parameters["@ContactAddress1"].Value = ContactAddress1;
            cmd.Parameters.Add(new SqlParameter("@ContactAddress2", SqlDbType.VarChar));
            cmd.Parameters["@ContactAddress2"].Value = ContactAddress2;
            cmd.Parameters.Add(new SqlParameter("@ContactEmail", SqlDbType.VarChar));
            cmd.Parameters["@ContactEmail"].Value = ContactEmail;
            cmd.Parameters.Add(new SqlParameter("@ContactPhone", SqlDbType.VarChar));
            cmd.Parameters["@ContactPhone"].Value = ContactPhone;
            cmd.Parameters.Add(new SqlParameter("@ContactFormLink", SqlDbType.VarChar));
            cmd.Parameters["@ContactFormLink"].Value = ContactFormLink;	
    		cmd.Parameters.Add(new SqlParameter("@EnableSubscription", SqlDbType.Bit));
			cmd.Parameters["@EnableSubscription"].Value = EnableSubscription;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionOption", SqlDbType.Int));
            cmd.Parameters["@SubscriptionOption"].Value = SubscriptionOption;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionFormLink", SqlDbType.VarChar));
            cmd.Parameters["@SubscriptionFormLink"].Value = SubscriptionFormLink;
            cmd.Parameters.Add(new SqlParameter("@LogoURL", SqlDbType.VarChar));
            cmd.Parameters["@LogoURL"].Value = _LogoURL;
            cmd.Parameters.Add(new SqlParameter("@LogoLink", SqlDbType.VarChar));
            cmd.Parameters["@LogoLink"].Value = _LogoLink;
            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            cmd.Parameters["@UserID"].Value = _UserID;


			try
			{
				conn.Open();
				PublicationID = Convert.ToInt32(cmd.ExecuteScalar());		
				conn.Close();
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
			return PublicationID;
		}

		public void Delete(int PublicationID) 
		{
			DataFunctions.ExecuteScalar("publisher","if ((select count(PublicationID) from edition where PublicationID = " + PublicationID + ") = 0) Delete from Publication where PublicationID = " + PublicationID + "  else RAISERROR('Cannot Delete Publication. Delete the Edition before deleting the Publication.',16,1)");					
		}
	}
}
