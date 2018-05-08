using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ecn.common.classes;

namespace ecn.communicator.classes
{
    public class Wizard : StatusItem
    {

		private int _wizardID = 0;
		private int _completedstep = 0;
		private string _wizardname = string.Empty;
		private string _emailsubject=string.Empty;
		private string _fromname=string.Empty;
		private string _fromemail = string.Empty;
		private string _replyto = string.Empty;
		private string _createddate = string.Empty;
		private string _updateddate = string.Empty;
        private string _blastType = "Regular";
		private int _userID = 0;
		private int _groupID = 0;
		private int _contentID = 0;
		private int _layoutID = 0;
		private int _blastID = 0;
		private int _filterID = 0;
        private int _pageWatchID = 0;

		public Wizard()
		{
		}

		public int ID 
		{
			get {return (_wizardID);}
			set {_wizardID = value;}
		}

		public int CompletedStep 
		{
			get {return (_completedstep);}
			set {_completedstep = value;}
		}

		public string WizardName
		{
			get {return (_wizardname);}
			set {_wizardname = value;}
		}

		public string EmailSubject
		{
			get {return (_emailsubject);}
			set {_emailsubject = value;}
		}

		public string FromName
		{
			get {return (_fromname);}
			set {_fromname = value;}
		}

		public string FromEmail
		{
			get {return (_fromemail);}
			set {_fromemail = value;}
		}

		public string ReplyTo
		{
			get {return (_replyto);}
			set {_replyto = value;}
		}

		public string CreatedDate
		{
			get {return (_createddate);}
			set {_createddate = value;}
		}

		public string UpdatedDate
		{
			get {return (_updateddate);}
			set {_updateddate = value;}
		}

        public string BlastType
        {
            get { return (_blastType); }
            set { _blastType = value; }
        }

		public int UserID 
		{
			get {return (_userID);}
			set {_userID = value;}
		}

		public int GroupID 
		{
			get {return (_groupID);}
			set {_groupID = value;}
		}

		public int ContentID 
		{
			get {return (_contentID);}
			set {_contentID = value;}
		}

		public int LayoutID 
		{
			get {return (_layoutID);}
			set {_layoutID = value;}
		}

		public int BlastID 
		{
			get {return (_blastID);}
			set {_blastID = value;}
		}

		public int FilterID 
		{
			get {return (_filterID);}
			set {_filterID = value;}
		}

        public int PageWatchID
        {
            get { return (_pageWatchID); }
            set { _pageWatchID = value; }
        }


		public int Save()
		{
			SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["com"].ToString());

			SqlCommand cmd = new SqlCommand("dbo.sp_SaveWizard", conn);
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@WizardID", SqlDbType.Int));
			cmd.Parameters["@WizardID"].Value = _wizardID;	
	
			cmd.Parameters.Add(new SqlParameter("@StepNo", SqlDbType.Int));
			cmd.Parameters["@StepNo"].Value = _completedstep;		
			cmd.Parameters.Add(new SqlParameter("@WizardName", SqlDbType.VarChar));
			cmd.Parameters["@WizardName"].Value = _wizardname;	
			cmd.Parameters.Add(new SqlParameter("@EmailSubject", SqlDbType.VarChar));
            cmd.Parameters["@EmailSubject"].Value = _emailsubject;
            cmd.Parameters.Add(new SqlParameter("@BlastType", SqlDbType.VarChar));
            cmd.Parameters["@BlastType"].Value = _blastType;
			cmd.Parameters.Add(new SqlParameter("@FromName", SqlDbType.VarChar));
			cmd.Parameters["@FromName"].Value = _fromname;	
			cmd.Parameters.Add(new SqlParameter("@FromEmail", SqlDbType.VarChar));
			cmd.Parameters["@FromEmail"].Value = _fromemail;	
			cmd.Parameters.Add(new SqlParameter("@ReplyTo", SqlDbType.VarChar));
			cmd.Parameters["@ReplyTo"].Value = _replyto;
            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
            cmd.Parameters["@status"].Value = Status;	
			cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmd.Parameters["@UserID"].Value = _userID;		
			cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
			cmd.Parameters["@GroupID"].Value = _groupID;		
			cmd.Parameters.Add(new SqlParameter("@ContentID", SqlDbType.Int));
			cmd.Parameters["@ContentID"].Value = _contentID;	
			cmd.Parameters.Add(new SqlParameter("@LayoutID", SqlDbType.Int));
			cmd.Parameters["@LayoutID"].Value = _layoutID;	
			cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
			cmd.Parameters["@BlastID"].Value = _blastID;	
			cmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.Int));
			cmd.Parameters["@FilterID"].Value = _filterID;
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", SqlDbType.Int));
            cmd.Parameters["@PageWatchID"].Value = _pageWatchID;

            cmd.Parameters.Add(new SqlParameter("@CardHolderName", SqlDbType.VarChar));
            cmd.Parameters["@CardHolderName"].Value = string.Empty;

            cmd.Parameters.Add(new SqlParameter("@CardType", SqlDbType.VarChar));
            cmd.Parameters["@CardType"].Value = string.Empty;

            cmd.Parameters.Add(new SqlParameter("@CardNumber", SqlDbType.VarChar));
            cmd.Parameters["@CardNumber"].Value = string.Empty;

            cmd.Parameters.Add(new SqlParameter("@CardcvNumber", SqlDbType.VarChar));
            cmd.Parameters["@CardcvNumber"].Value = string.Empty;

            cmd.Parameters.Add(new SqlParameter("@CardExpiration", SqlDbType.VarChar));
            cmd.Parameters["@CardExpiration"].Value = string.Empty;

            cmd.Parameters.Add(new SqlParameter("@TransactionNo", SqlDbType.VarChar));
            cmd.Parameters["@TransactionNo"].Value = string.Empty;

			try
			{
				conn.Open();
				this._wizardID = Convert.ToInt32(cmd.ExecuteScalar());	
				conn.Close();
			}
			catch(SqlException SqlEx)
			{
				throw SqlEx;
			}
			return _wizardID;		
		}

		public static Wizard GetWizardbyID(int WizardID)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT * from Wizard where WizardID = " + WizardID);
				cmd.CommandTimeout = 0;
				cmd.CommandType = CommandType.Text;

				DataTable dt = DataFunctions.GetDataTable(cmd);
	
				if (dt.Rows.Count > 0) 
				{
					Wizard w = new Wizard();

					w.ID = WizardID;
					w.WizardName= dt.Rows[0]["WizardName"].ToString();
					w.EmailSubject =  dt.Rows[0]["EmailSubject"].ToString();
					w.CompletedStep = Convert.ToInt32(dt.Rows[0]["CompletedStep"]);
					w.FromName = dt.Rows[0]["FromName"].ToString();
					w.FromEmail = dt.Rows[0]["FromEmail"].ToString();
					w.ReplyTo = dt.Rows[0]["ReplyTo"].ToString();
					w.Status = dt.Rows[0]["StatusCode"].ToString();
					w.UserID =  Convert.ToInt32(dt.Rows[0]["CreatedUserID"]);
					w.GroupID =  Convert.ToInt32(dt.Rows[0]["GroupID"]);
					w.ContentID =  Convert.ToInt32(dt.Rows[0]["ContentID"]);
					w.LayoutID =  Convert.ToInt32(dt.Rows[0]["LayoutID"]);
					w.BlastID =  Convert.ToInt32(dt.Rows[0]["BlastID"]);
                    w.BlastType = dt.Rows[0]["BlastType"].ToString();
					w.FilterID =  Convert.ToInt32(dt.Rows[0]["FilterID"]);
                    w.PageWatchID = dt.Rows[0].IsNull("PageWatchID")?0:Convert.ToInt32(dt.Rows[0]["PageWatchID"]);
                    w.CreatedDate = dt.Rows[0].IsNull("CreatedDate") ? "" : dt.Rows[0]["CreatedDate"].ToString();
                    w.UpdatedDate = dt.Rows[0].IsNull("UpdatedDate") ? "" : dt.Rows[0]["UpdatedDate"].ToString();

					return w;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}
	}
}
