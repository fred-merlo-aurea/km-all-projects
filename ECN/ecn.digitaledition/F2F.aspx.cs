using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.Web.UI;
using ecn.communicator.classes;
using ecn.common.classes;
using Ecn.DigitalEdition.Helpers;
using ecn.publisher.classes;

namespace ecn.digitaledition
{
	/// <summary>
	/// Summary description for F2F.
	/// </summary>
	public partial class F2F : System.Web.UI.Page
	{
        private string BlastIdQueryStringKey = "b";
        private string EmailIdQueryStringKey = "e";
        private string EditionIdQueryStringKey = "eID";
        private string SessionIdQueryStringKey = "s";
        private HttpRequestBase _request;

        public KMPlatform.Entity.User User = null;

        public F2F()
        {
            _request = new HttpRequestAdapter(Request);
        }

        public F2F(HttpRequestBase request)
        {
            _request = request;
        }

        private int getBlastID()
        {
            return QueryStringHelper.GetIntValue(_request, BlastIdQueryStringKey);
        }

        private int getEmailID()
        {
            return QueryStringHelper.GetIntValue(_request, EmailIdQueryStringKey);
        }

        private int getEditionID()
        {
            return QueryStringHelper.GetIntValue(_request, EditionIdQueryStringKey);
        }

        private string getSessionID()
        {
            return QueryStringHelper.GetStringValue(_request, SessionIdQueryStringKey);
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
            DataTable dt1 = DataFunctions.GetDataTable("select '/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/' as imgpath, IsNull(IsLoginRequired,0) as IsLoginRequired  from edition e join Publication m on e.PublicationID = m.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where m.IsDeleted=0 and e.IsDeleted=0 and e.editionID=" + getEditionID());
			
			if (dt1.Rows.Count > 0)
			{
				string thumbnailpath = dt1.Rows[0]["imgpath"].ToString();
				imgThumbnail.ImageUrl= ConfigurationManager.AppSettings["ImagePath"] + thumbnailpath + "/150/1.png";
				lblLoginRequired.Text = dt1.Rows[0]["IsLoginRequired"].ToString();
			}

			if (getEmailID() > 0)
			{
				DataTable dt = DataFunctions.GetDataTable("select emailaddress, isnull(fullname,'') as FullName from emails where emailID = " + getEmailID(), ConfigurationManager.AppSettings["com"]);

				if (dt.Rows.Count > 0) 
				{
					txtEmail.Text = dt.Rows[0]["emailaddress"].ToString();
					txtFrom.Text = dt.Rows[0]["FullName"].ToString();
					txtEmail.Attributes.Add("readonly","true");
					//txtFrom.Attributes.Add("readonly","true");
				}
			}
		}

		protected void EmailButton_Click(object sender, EventArgs e)
		{
			Page.Validate();
			int referrerEmailID = getEmailID();
			int F2FEmailID = 0;

			int GroupID = 0;
			int CustomerID = 0;
			string ImgPath = string.Empty;
			string EditionName = string.Empty;
			string pwd = string.Empty;

			if(Page.IsValid)
			{
                DataTable dt = DataFunctions.GetDataTable("select GroupID, m.CustomerID, e.EditionName, '/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/' as imgpath from Publication m join edition e on m.PublicationID = e.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where  m.IsDeleted=0 and e.IsDeleted=0 and e.editionID = " + getEditionID());
				
				if (dt.Rows.Count > 0) 
				{
					GroupID = Convert.ToInt32(dt.Rows[0]["GroupID"]);
					CustomerID = Convert.ToInt32(dt.Rows[0]["CustomerID"]);
					EditionName = dt.Rows[0]["EditionName"].ToString();
					ImgPath = dt.Rows[0]["imgpath"].ToString();
					try
					{
						Groups group = new Groups(GroupID);
						if (referrerEmailID==0)
						{
							referrerEmailID = SubscribeToGroup(group, txtFrom.Text, txtEmail.Text, CustomerID);
							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());

						}
					
                        ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                        ed.CustomerID = CustomerID;
                        ed.FromName = "Digital Edition";
                        ed.Process = "Digital Edition - F2F.EmailButton_Click";
                        ed.Source = "Digital Edition";
                        ed.EmailSubject = txtSubject.Text;
                        ed.ReplyEmailAddress = txtEmail.Text;
                        //ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                        ed.SendTime = DateTime.Now;
                        ed.CreatedUserID = User.UserID;

						if (Email1.Text != "") 
						{
							F2FEmailID = SubscribeToGroup(group, Name1.Text, Email1.Text, CustomerID);
							pwd = GeneratePassword(F2FEmailID);

                            //Send Email with EmailDirect
                            ed.EmailAddress = Email1.Text;
                            ed.Content = ReplaceCodeSnippets(getEditionID(), F2FEmailID, ImgPath, txtFrom.Text, EditionName, Email1.Text, pwd, GroupID);
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,   "refer", Email1.Text, Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
							Edition.CreateActivity(getEditionID(), F2FEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
						}

						if (Email2.Text != "") 
						{
							F2FEmailID = SubscribeToGroup(group, Name2.Text, Email2.Text, CustomerID);
							pwd = GeneratePassword(F2FEmailID);

                            //Send Email with EmailDirect
                            ed.EmailAddress = Email2.Text;
                            ed.Content = ReplaceCodeSnippets(getEditionID(), F2FEmailID, ImgPath, txtFrom.Text, EditionName, Email2.Text, pwd, GroupID);
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,  "refer", Email2.Text, Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
							Edition.CreateActivity(getEditionID(), F2FEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
						}

						if (Email3.Text != "") 
						{
							F2FEmailID = SubscribeToGroup(group, Name3.Text, Email3.Text, CustomerID);
							pwd = GeneratePassword(F2FEmailID);

                            //Send Email with EmailDirect
                            ed.EmailAddress = Email3.Text;
                            ed.Content = ReplaceCodeSnippets(getEditionID(), F2FEmailID, ImgPath, txtFrom.Text, EditionName, Email3.Text, pwd, GroupID);
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,  "refer", Email3.Text, Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
							Edition.CreateActivity(getEditionID(), F2FEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
						}

						if (Email4.Text != "") 
						{
							F2FEmailID = SubscribeToGroup(group, Name4.Text, Email4.Text, CustomerID);
							pwd = GeneratePassword(F2FEmailID);

                            //Send Email with EmailDirect
                            ed.EmailAddress = Email4.Text;
                            ed.Content = ReplaceCodeSnippets(getEditionID(), F2FEmailID, ImgPath, txtFrom.Text, EditionName, Email4.Text, pwd, GroupID);
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,  "refer", Email4.Text, Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
							Edition.CreateActivity(getEditionID(), F2FEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
						}

						if (Email5.Text != "") 
						{
							F2FEmailID = SubscribeToGroup(group, Name5.Text, Email5.Text, CustomerID);
							pwd = GeneratePassword(F2FEmailID);

                            //Send Email with EmailDirect
                            ed.EmailAddress = Email5.Text;
                            ed.Content = ReplaceCodeSnippets(getEditionID(), F2FEmailID, ImgPath, txtFrom.Text, EditionName, Email5.Text, pwd, GroupID);
                            ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

							Edition.CreateActivity(getEditionID(), referrerEmailID,  getBlastID(), 0, 0,  "refer", Email5.Text, Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
							Edition.CreateActivity(getEditionID(), F2FEmailID,  getBlastID(), 0, 0,   "subscribe", "Subscription thru F2F", Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());
						}
						pnlf2f.Visible = false;
						pnlMessage.Visible=true;
						lblMessage.Text = "sent";

					}
					catch(Exception ex)
					{
						pnlf2f.Visible = true;
						pnlMessage.Visible=false;
						lblError.Text = ex.Message;
					}
				}
			}
		}

		private string ReplaceCodeSnippets(int EditionID, int EmailID, string ImgPath, string FromName, string EditionName, string username, string password, int GroupID)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(ConfigurationManager.AppSettings["F2FEmailTemplate"].ToString());

			if (Convert.ToBoolean(lblLoginRequired.Text))
			{
				string loginInfo = "<font face='Arial' size='2'>Please use this Username and Password to login and view your digital edition.<br><br>Username: <strong>%%username%%</strong><br>Password: <strong>%%password%%</strong></font><br><hr color='#999999' size='1' />";
				sb = sb.Replace("%%LoginInfo%%", loginInfo);
				sb = sb.Replace("%%username%%", username);
				sb = sb.Replace("%%password%%", password);
			}
			else
				sb = sb.Replace("%%LoginInfo%%", "");

            sb = sb.Replace("%%sub_link%%", ConfigurationManager.AppSettings["F2FSubscribePage"] + EditionID.ToString() + "&g=" + GroupID.ToString() + "&e=" + EmailID.ToString());
            sb = sb.Replace("%%Name%%", FromName);
			sb = sb.Replace("%%EditionName%%", EditionName);
			sb = sb.Replace("%%Message%%", txtcomments.Text);
			sb = sb.Replace("%%cover%%", ConfigurationManager.AppSettings["ImagePath"] + ImgPath + "150/1.png");

            string sPublicationCode = DataFunctions.ExecuteScalar("select Isnull(PublicationCode,'') from Publication P join Edition E on p.publicationID = E.PublicationID where  P.IsDeleted=0 and E.IsDeleted=0  and E.EditionID = " + EditionID).ToString();

			if (sPublicationCode == string.Empty)
				sb = sb.Replace("%%DigitalEditionlink%%", ConfigurationManager.AppSettings["host"] +  "/Magazine.aspx?eID=" + EditionID + "&e=" + EmailID);
			else
				sb = sb.Replace("%%DigitalEditionlink%%", ConfigurationManager.AppSettings["host"].Replace("www",sPublicationCode) +  "/Magazine.aspx?eID=" + EditionID + "&e=" + EmailID);	

			return sb.ToString();
		}

		private string GeneratePassword(int EmailID)
		{
			string pwd = string.Empty;

			DataTable dt = DataFunctions.GetDataTable("select emailaddress , Isnull(password,'') as pwd from ecn5_communicator..Emails where EmailID = " + EmailID);

			if (dt.Rows.Count > 0) //dt.Rows[0]["pwd"].ToString() == string.Empty
			{
				if (dt.Rows[0]["pwd"].ToString() == string.Empty)
				{
					pwd = Guid.NewGuid().ToString().Substring(0,5);
					DataFunctions.Execute("update ecn5_communicator..emails set password='" + pwd + "' where EmailID = " + EmailID); 
				}
				else
				{
					pwd = dt.Rows[0]["pwd"].ToString();
				}
			}

			return pwd;
		}

		#region Subscribe email to the Group
		private int SubscribeToGroup(Groups group, string FullName, string EmailAddress, int CustomerID) 
		{
			Emails email =  group.WhatEmailForCustomer(EmailAddress);
			int EmailID;
			SqlDateTime sqldatenull = SqlDateTime.Null;	

			if (null==email) 
			{

				SqlConnection db_connection = new SqlConnection(ConfigurationManager.AppSettings["com"]);
				SqlCommand InsertCommand = new SqlCommand(null,db_connection);
				InsertCommand.CommandText =
					"INSERT INTO Emails "+ 
					"(EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateAdded, DateUpdated)" 
					+ " VALUES " 
					+ "(@emailAddress,@customer_id,@title,@first_name,@last_name,@full_name,@company,@occupation,@address,@address2,@city,@state,@zip,@country,@voice,@mobile,@fax,@website,@age,@income,@gender,@user1,@user2,@user3,@user4,@user5,@user6,@birthdate,@user_event1,@user_event1_date,@user_event2,@user_event2_date,@notes,@DateAdded,@DateUpdated) SELECT @@IDENTITY";
 
				InsertCommand.Parameters.Add ("@emailAddress", SqlDbType.VarChar,250).Value = EmailAddress;  
				InsertCommand.Parameters.Add ("@customer_id", SqlDbType.Int,4).Value = CustomerID;
				InsertCommand.Parameters.Add ("@title",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@first_name",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@last_name",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@full_name",SqlDbType.VarChar,50).Value = FullName;
				InsertCommand.Parameters.Add ("@company",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@occupation",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@address",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@address2",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@city",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@state",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@zip",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@country",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@voice",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@mobile",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@fax",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@website",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@age",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@income",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@gender",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user1",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user2",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user3",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user4",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user5",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user6",SqlDbType.VarChar,255).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@birthdate",SqlDbType.DateTime).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user_event1",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user_event1_date",SqlDbType.DateTime).Value = sqldatenull;
				InsertCommand.Parameters.Add ("@user_event2",SqlDbType.VarChar,50).Value = DBNull.Value;
				InsertCommand.Parameters.Add ("@user_event2_date",SqlDbType.DateTime).Value =DBNull.Value;
				InsertCommand.Parameters.Add ("@notes",SqlDbType.Text).Value = "F2F through Website. DateAdded: "+DateTime.Now.ToString(); 
				InsertCommand.Parameters.Add ("@DateAdded",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 
				InsertCommand.Parameters.Add ("@DateUpdated",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 

				InsertCommand.CommandTimeout = 0;
				InsertCommand.Connection.Open();
				EmailID = Convert.ToInt32(InsertCommand.ExecuteScalar());
				InsertCommand.Connection.Close();

				email= new Emails(EmailID);
			}
			
			group.AttachEmail(email, "HTML", "P");
			
			return email.ID();
		}
		#endregion

	}
}
