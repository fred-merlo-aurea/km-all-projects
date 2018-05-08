using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ecn.communicator.classes;
using Ecn.Communicator.Main.Lists.Interfaces;
using ecn.communicator.SalesForcePartner;
using ecn.communicator.SalesForcePartner.Interfaces;
using Ecn.Communicator.Main.Lists.Helpers;

namespace ecn.communicator.main.lists
{
    public partial class sfintegration : System.Web.UI.Page
    {
        private HttpSessionState SessionCollection;
        private IDatabaseAdapter DBAdapter;
        private const string StartRowIndexParameter = "@startrowIndex";
        private const string MaximumRowsParameter = "@maximumRows";
        private const string CustomerIdParameter = "@customerID";
        private const string GroupIdParameter = "@groupID";
        private const string EmailAddressParameter = "@emailAddress";
        private const string LoggedInKey = "loggedIn";
        private const string BindingKey = "binding";
        private const string SFContactsKey = "SFContacts";
        private const int SObjectXmlElementsCount = 14;
        private const string IdKey = "Id";
        private const string AccountIdKey = "AccountId";
        private const string EmailKey = "Email";
        private const string FaxKey = "Fax";
        private const string FirstNameKey = "FirstName";
        private const string HomePhoneKey = "HomePhone";
        private const string LastNameKey = "LastName";
        private const string MailingCityKey = "MailingCity";
        private const string MailingStateKey = "MailingState";
        private const string MailingCountryKey = "MailingCountry";
        private const string MailingPostalCodeKey = "MailingPostalCode";
        private const string MailingStreetKey = "MailingStreet";
        private const string MobilePhoneKey = "MobilePhone";
        private const string PhoneKey = "Phone";
        private const string TitleKey = "Title";
        private const string ContactType = "Contact";
        private const string CustomerIDKey = "CustomerID";
        private const int GroupIDValueNegativeOne = -1;
        private const int GroupIDValueNegativeTwo = -2;
        private const string GroupIdKey = "GroupId";
        private const string GroupNameKey = "GroupName";
        private const string EmailAddressKey = "EmailAddress";
        private const string AddressKey = "Address";
        private const string CityKey = "City";
        private const string StateKey = "State";
        private const string ZipKey = "Zip";
        private const string CountryKey = "Country";
        private const string CompanyKey = "Company";
        private const string VoiceKey = "Voice";
        private const string MobileKey = "Mobile";
        private const string GroupNameValueAll = "- All -";
        private const string GroupNameValueSelectGroup = "- Select Group -";
        private const string GroupNameValueNewGroup = "- New Group -";
        private const string GetGroupsByCustomerIdQuery = "SELECT GroupID,GroupName FROM Groups WHERE CustomerID ={0} ORDER BY GroupName";
        private const string GetECNContactsByCustomerIdAndGroupIdQuery =
            "SELECT Count(e.EmailAddress) FROM Emails e INNER JOIN Groups g ON g.CustomerID  = e.CustomerID WHERE e.CustomerID ={0} AND g.GroupID ={1}";
        private const string GetECNContactsByCustomerIdQuery = "SELECT Count(e.EmailAddress) FROM Emails e WHERE e.CustomerID ={0}";

        #region Properties
        int _pagerCurrentPage = 1;
        int _pagerRecordCount = 1;
        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage; }
        }
        public int pagerRecordCount
        {
            set { _pagerRecordCount = value; }
            get { return _pagerRecordCount; }
        }
        private SforceService binding
        {
            get { return (SforceService)Session["binding"]; }
            set { Session["binding"] = value; }
        }
        private bool loggedIn
        {
            get { return (bool)SessionCollection[LoggedInKey]; }
            set { Session[LoggedInKey] = value; }
        }

        private String[] contacts
        {
            get { return (String[])Session["contacts"]; }
            set { Session["contacts"] = value; }
        }
        private LoginResult loginResult
        {
            get { return (LoginResult)Session["loginResult"]; }
            set { Session["loginResult"] = value; }
        }
        private int CustomerID
        {
            get { return (int)SessionCollection[CustomerIDKey]; }
            set { SessionCollection[CustomerIDKey] = value; }
        }
        #endregion

        public sfintegration()
        {
            this.SessionCollection = base.Session;
        }

        public sfintegration(HttpSessionState session, IDatabaseAdapter dbAdapter = null)
        {
            this.SessionCollection = session;
            DBAdapter = dbAdapter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.Heading = "SalesForce.com Integration";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Import and Export with SalesForce.com</b><br />You can import contacts from SalesForce.com into existing Email Groups or a New Email Group.<br /><br />Contacts can also be exported from ECN to SalesForce.com.  Export all contacts or by Groups.  Contacts can be exported itno precreated SalesForce Accounts.";
            Master.HelpTitle = "Groups Manager";	

            if (!Page.IsPostBack && !Page.IsCallback)
            {
                //testing values
                //tbUserName.Text = "justin.wagner@knowledgemarketing.com";
                //tbPassword.Text = "brickm19pJ06cccVVkaU2tzqq0YyJkY2";
                //CustomerID = 1;
                //end testing values
                if (Session["loggedIn"] == null)
                    loggedIn = false;
                if (!loggedIn)
                {
                    ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
                    CustomerID = es.CurrentCustomer.CustomerID;
                    loggedIn = false;
                }
                else
                {
                    pnlFeatures.Visible = true;
                    pnlSFLogin.Visible = false;
                }
            }

        }

        protected void rblImportExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblImportExport.SelectedIndex == 0)
            {
                LoadECN_Groups();
                pnlECNtoSF.Visible = true;
                pnlSFtoECN.Visible = false;
                pnlCampaignData.Visible = false;
            }
            else if (rblImportExport.SelectedIndex == 1)
            {
                LoadSF_Accounts();
                LoadECNGroupForSF();
                pnlSFtoECN.Visible = true;
                pnlECNtoSF.Visible = false;
                pnlCampaignData.Visible = false;
            }
            else if (rblImportExport.SelectedIndex == 2)//campaigns
            {
                pnlSFtoECN.Visible = false;
                pnlECNtoSF.Visible = false;
                pnlCampaignData.Visible = true;
                GetSFCampaignNames();
                GetEcnEmailBlasts();
            }
        }

        #region ECN_Data
        private void LoadECNGroupForSF()
        {
            var ecnGroups = new List<ECN_Group>();
            ecnGroups.Add(CreateECNGroup(GroupIDValueNegativeTwo, GroupNameValueSelectGroup));
            ecnGroups.Add(CreateECNGroup(GroupIDValueNegativeOne, GroupNameValueNewGroup));

            PopulateDropDownWithECNGroupsFromDB(ddlECNGroupFromSF, ecnGroups);
        }
        private void LoadECN_Groups()
        {
            var ecnGroups = new List<ECN_Group>();
            ecnGroups.Add(CreateECNGroup(GroupIDValueNegativeOne, GroupNameValueAll));

            PopulateDropDownWithECNGroupsFromDB(ddlECNGroups, ecnGroups);
        }

        private void PopulateDropDownWithECNGroupsFromDB(DropDownList dropdownListControl, List<ECN_Group> ecnGroups)
        {
            // create a command object
            var command = DBAdapter.CreateCommand(string.Format(GetGroupsByCustomerIdQuery, CustomerID), DBAdapter.Connection);

            DBAdapter.Connection.Open();

            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var item = new ECN_Group();

                var name = GroupIdKey;
                var index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    item.GroupID = (int)dataReader[index];
                }

                name = GroupNameKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    item.GroupName = dataReader[index] as string;
                }

                ecnGroups.Add(item);
            }

            dataReader.Close();

            dropdownListControl.DataSource = ecnGroups;
            dropdownListControl.DataTextField = GroupNameKey;
            dropdownListControl.DataValueField = GroupIdKey;
            dropdownListControl.DataBind();
        }

        private ECN_Group CreateECNGroup(int groupID, string groupName)
        {
            return new ECN_Group()
            {
                GroupID = groupID,
                GroupName = groupName
            };
        }

        private List<ECN_Contact> GetECNContacts(int GroupID)
        {
            var connection = GetSqlConn();
            var command = CreateStoredProcedureCommand("sp_GetEcnContacts", connection);
            DBAdapter.AddParameter(command, StartRowIndexParameter, SqlDbType.Int, pagerCurrentPage);
            DBAdapter.AddParameter(command, MaximumRowsParameter, SqlDbType.Int, pagerECNContacts.PageSize);
            DBAdapter.AddParameter(command, CustomerIdParameter, SqlDbType.Int, CustomerID);
            DBAdapter.AddParameter(command, GroupIdParameter, SqlDbType.Int, GroupID);

            connection.Open();

            var ecnContacts = GetECNContacts(connection, command);
            var cmdRowCount = DBAdapter.CreateCommand(string.Format(GetECNContactsByCustomerIdAndGroupIdQuery, CustomerID, GroupID), connection);
            pagerRecordCount = Convert.ToInt32(cmdRowCount.ExecuteScalar().ToString());

            connection.Close();
            return ecnContacts;
        }

        private List<ECN_Contact> GetECNContactsAll()
        {
            var connection = GetSqlConn();
            var command = CreateStoredProcedureCommand("sp_GetEcnContactsAll", connection);
            DBAdapter.AddParameter(command, StartRowIndexParameter, SqlDbType.Int, pagerCurrentPage);
            DBAdapter.AddParameter(command, MaximumRowsParameter, SqlDbType.Int, pagerECNContacts.PageSize);
            DBAdapter.AddParameter(command, CustomerIdParameter, SqlDbType.Int, CustomerID);

            connection.Open();

            var ecnContacts = GetECNContacts(connection, command);
            var cmdRowCount = DBAdapter.CreateCommand(string.Format(GetECNContactsByCustomerIdQuery, CustomerID), connection);
            pagerRecordCount = Convert.ToInt32(cmdRowCount.ExecuteScalar().ToString());

            connection.Close();
            return ecnContacts;
        }

        private ECN_Contact GetECNContactByEmailAddress(string emailAddress)
        {
            var connection = GetSqlConn();
            var command = CreateStoredProcedureCommand("sp_GetEcnContactByEmailAddress", connection);
            DBAdapter.AddParameter(command, EmailAddressParameter, SqlDbType.VarChar, emailAddress);

            connection.Open();
            var ecnContacts = GetECNContacts(connection, command, false);
            connection.Close();

            if (ecnContacts.Any())
            {
                return ecnContacts[0];
            }

            return new ECN_Contact();
        }

        private List<ECN_Contact> GetECNContacts(IDbConnection connection, IDbCommand command, bool updateGroupName = true)
        {
            var ecnContacts = new List<ECN_Contact>();
            var dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                var newContact = new ECN_Contact();

                var name = EmailAddressKey;
                var index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.EmailAddress = dataReader[index] as string;
                }

                name = FirstNameKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.FirstName = dataReader[index] as string;
                }

                name = LastNameKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.LastName = dataReader[index] as string;
                }

                name = AddressKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Address = dataReader[index] as string;
                }

                name = CityKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.City = dataReader[index] as string;
                }

                name = StateKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.State = dataReader[index] as string;
                }

                name = ZipKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.PostalCode = dataReader[index] as string;
                }

                name = CountryKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Country = dataReader[index] as string;
                }

                name = CompanyKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Company = dataReader[index] as string;
                }

                name = TitleKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Title = dataReader[index] as string;
                }

                name = VoiceKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Voice = dataReader[index] as string;
                }

                name = MobileKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Mobile = dataReader[index] as string;
                }

                name = FaxKey;
                index = dataReader.GetOrdinal(name);
                if (!dataReader.IsDBNull(index))
                {
                    newContact.Fax = dataReader[index] as string;
                }

                if (updateGroupName)
                {
                    name = GroupNameKey;
                    index = dataReader.GetOrdinal(name);
                    if (!dataReader.IsDBNull(index))
                    {
                        newContact.GroupName = dataReader[index] as string;
                    }
                }

                ecnContacts.Add(newContact);
            }

            dataReader.Close();
            return ecnContacts;
        }

        private IDbCommand CreateStoredProcedureCommand(string storedProcedureName, IDbConnection connection)
        {
            var command = DBAdapter.CreateCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;
            return command;
        }

        private IDbConnection GetSqlConn()
        {
            if (DBAdapter == null)
            {
                DBAdapter = new SqlDatabaseAdapter(
                    new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connString"].ToString()));
            }

            return DBAdapter.Connection;
        }
        #endregion

        #region SalesForce
        private void LoadSF_Accounts()
        {
            ListItem dummy = new ListItem();
            dummy.Text = "-- Make a Selection --";
            dummy.Value = "-- Make a Selection --";
            dummy.Selected = true;
            ddlSFAccounts.Items.Add(dummy);

            ListItem liAccounts = new ListItem();
            liAccounts.Text = "Contacts - Accounts";
            liAccounts.Value = "Contacts - Accounts";
            ddlSFAccounts.Items.Add(liAccounts);

            ListItem liLeads = new ListItem();
            liLeads.Text = "Contacts - Leads";
            liLeads.Value = "Contacts - Leads";
            ddlSFAccounts.Items.Add(liLeads);

            ListItem liAll = new ListItem();
            liAll.Text = "Contacts - All";
            liAll.Value = "Contacts - All";
            ddlSFAccounts.Items.Add(liAll);


            //List<SF_Account> sfAccounts = GetSFAccounts();

            //ddlSFAccounts.DataSource = sfAccounts;
            //ddlSFAccounts.DataTextField = "Name";
            //ddlSFAccounts.DataValueField = "AccountId";
            //ddlSFAccounts.DataBind();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (login())
            {
                pnlFeatures.Visible = true;
                pnlSFLogin.Visible = false;
                lbMessage.Text = "Successfully logged in to SalesForce.com";
                mpeMessages.Show();
            }
            else
            {
                lbMessage.Text = "You where unable to log in to SalesForce.com, please try again.";
                mpeMessages.Show();
            }
        }
        private bool login()
        {
            string un = tbUserName.Text.ToString();
            string pw = tbPassword.Text.ToString();

            //Create the binding to the sforce servics
            binding = new SforceService();

            // Time out after a minute
            binding.Timeout = 60000;

            //Attempt the login 
            try
            {
                loginResult = binding.login(un, pw);
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                // This is likley to be caused by bad username or password
                return false;
            }
            catch (Exception e)
            {
                // This is something else, probably comminication
                return false;
            }

            //tbResults.Text += "\nThe session id is: " + loginResult.sessionId;
            //tbResults.Text += "\nThe new server url is: " + loginResult.serverUrl;

            //Change the binding to the new endpoint
            binding.Url = loginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            binding.SessionHeaderValue = new SalesForcePartner.SessionHeader();
            binding.SessionHeaderValue.sessionId = loginResult.sessionId;

            loggedIn = true;
            return true;
        }
        private List<SF_Contact> GetSFContacts(string AccountId)
        {
            var getSFContactsByAccountIdQuery = new StringBuilder();
            getSFContactsByAccountIdQuery.AppendLine("Select Id,AccountID,Email,Fax,FirstName,HomePhone,LastName,MailingCity,MailingState,MailingCountry,MailingPostalCode,MailingStreet,MobilePhone,Phone,Title ");
            getSFContactsByAccountIdQuery.AppendLine("From Contact Where IsDeleted = false and AccountId = '{0}'");

            var queryText = string.Format(getSFContactsByAccountIdQuery.ToString(), AccountId);
            return GetSFContactsList(queryText);
        }

        private List<SF_Contact> GetSFContactsList(string queryText)
        {
            var listSFContacts = new List<SF_Contact>();
            var sessionBinding = (ISforceService)SessionCollection[BindingKey];
            var queryResult = sessionBinding.query(queryText);
            for (int i = 0; i < queryResult.records.Length; i++)
            {
                var contact = queryResult.records[i];
                var sfContact = new SF_Contact();

                sfContact.ContactId = getFieldValue(IdKey, contact.Any);
                sfContact.AccountId = getFieldValue(AccountIdKey, contact.Any);
                sfContact.Email = getFieldValue(EmailKey, contact.Any);
                sfContact.Fax = getFieldValue(FaxKey, contact.Any);
                sfContact.FirstName = getFieldValue(FirstNameKey, contact.Any);
                sfContact.HomePhone = getFieldValue(HomePhoneKey, contact.Any);
                sfContact.LastName = getFieldValue(LastNameKey, contact.Any);
                sfContact.MailingCity = getFieldValue(MailingCityKey, contact.Any);
                sfContact.MailingCountry = getFieldValue(MailingCountryKey, contact.Any);
                sfContact.MailingPostalCode = getFieldValue(MailingPostalCodeKey, contact.Any);
                sfContact.MailingState = getFieldValue(MailingStateKey, contact.Any);
                sfContact.MailingStreet = getFieldValue(MailingStreetKey, contact.Any);
                sfContact.MobilePhone = getFieldValue(MobilePhoneKey, contact.Any);
                sfContact.Phone = getFieldValue(PhoneKey, contact.Any);
                sfContact.Title = getFieldValue(TitleKey, contact.Any);

                listSFContacts.Add(sfContact);
            }

            return listSFContacts;
        }

        private List<SF_Contact> GetSFContactsAll()
        {
            if (SessionCollection[SFContactsKey] == null)
            {
                var getAllSFContactsQuery = new StringBuilder();
                getAllSFContactsQuery.AppendLine("Select Id,AccountID,Email,Fax,FirstName,HomePhone,LastName,MailingCity,MailingState,MailingCountry,MailingPostalCode,MailingStreet,MobilePhone,Phone,Title ");
                getAllSFContactsQuery.AppendLine("From Contact Where IsDeleted = false");

                var listSFContacts = GetSFContactsList(getAllSFContactsQuery.ToString());
                SessionCollection[SFContactsKey] = listSFContacts;
                return listSFContacts;
            }
            else
            {
                return (List<SF_Contact>)SessionCollection[SFContactsKey];
            }
        }
        private List<SF_Account> GetSFAccounts()
        {
            if (Session["SFAccounts"] == null)
            {
                List<SF_Account> listSFAccounts = new List<SF_Account>();
                //add a dummy account for selecting all 
                SF_Account dummy = new SF_Account();
                dummy.AccountId = "-1";
                dummy.Name = "- All -";
                listSFAccounts.Add(dummy);

                //get a list of Accounts so the user can select one
                SforceService sessionBinding = (SforceService)Session["binding"];
                SalesForcePartner.QueryResult qr = sessionBinding.query("Select Id, Name,Description,AccountNumber,BillingStreet,BillingCity,BillingState,BillingPostalCode,BillingCountry,Phone,Fax from Account");

                for (int i = 0; i < qr.records.Length; i++)
                {
                    SalesForcePartner.sObject account = qr.records[i];
                    SF_Account sfAccount = new SF_Account();

                    sfAccount.AccountId = getFieldValue("Id", account.Any);
                    sfAccount.AccountNumber = getFieldValue("AccountNumber", account.Any);
                    sfAccount.BillingCity = getFieldValue("BillingCity", account.Any);
                    sfAccount.BillingCountry = getFieldValue("BillingCountry", account.Any);
                    sfAccount.BillingPostalCode = getFieldValue("BillingPostalCode", account.Any);
                    sfAccount.BillingState = getFieldValue("BillingState", account.Any);
                    sfAccount.BillingStreet = getFieldValue("BillingStreet", account.Any);
                    sfAccount.Description = getFieldValue("Description", account.Any);
                    sfAccount.Fax = getFieldValue("Fax", account.Any);
                    sfAccount.Name = getFieldValue("Name", account.Any);
                    sfAccount.Phone = getFieldValue("Phone", account.Any);

                    listSFAccounts.Add(sfAccount);
                }
                Session["SFAccounts"] = listSFAccounts;
                return listSFAccounts;
            }
            else
            {
                return (List<SF_Account>)Session["SFAccounts"];
            }
        }
        private List<SF_Lead> GetSFLeads()
        {
            if (Session["SFLeads"] == null)
            {
                List<SF_Lead> listSFLeads = new List<SF_Lead>();
                //add a dummy account for selecting all 
                SF_Lead dummy = new SF_Lead();
                dummy.LeadId = "-1";
                dummy.Email = "- All -";
                listSFLeads.Add(dummy);

                //get a list of Accounts so the user can select one
                SforceService sessionBinding = (SforceService)Session["binding"];
                SalesForcePartner.QueryResult qr = sessionBinding.query("Select Id, AnnualRevenue,City,Company,ConvertedAccountId,ConvertedContactId,ConvertedDate,ConvertedOpportunityId,Country,Description,Email,EmailBouncedDate,EmailBouncedReason,Fax,FirstName,LastName,MobilePhone,NumberOfEmployees,Phone,PostalCode,State,Street,Title,Website From Lead");

                for (int i = 0; i < qr.records.Length; i++)
                {
                    SalesForcePartner.sObject lead = qr.records[i];
                    SF_Lead sfLead = new SF_Lead();
                    int AnnRev = 0;
                    if (!string.IsNullOrEmpty(getFieldValue("AnnualRevenue", lead.Any)))
                    {
                        try
                        {
                            AnnRev = Convert.ToInt32(getFieldValue("AnnualRevenue", lead.Any).ToString());
                        }
                        catch
                        {
                            AnnRev = 0;
                        }
                    }
                    int numEmp = 0;
                    if (!string.IsNullOrEmpty(getFieldValue("NumberOfEmployees", lead.Any)))
                    {
                        try
                        {
                            numEmp = Convert.ToInt32(getFieldValue("NumberOfEmployees", lead.Any).ToString());
                        }
                        catch
                        {
                            numEmp = 0;
                        }
                    }
                    DateTime conDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(getFieldValue("ConvertedDate", lead.Any)))
                    {
                        try
                        {
                            conDate = Convert.ToDateTime(getFieldValue("ConvertedDate", lead.Any).ToString());
                        }
                        catch
                        {
                            conDate = DateTime.MinValue;
                        }
                    }
                    DateTime ebDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(getFieldValue("EmailBouncedDate", lead.Any)))
                    {
                        try
                        {
                            ebDate = Convert.ToDateTime(getFieldValue("EmailBouncedDate", lead.Any).ToString());
                        }
                        catch
                        {
                            ebDate = DateTime.MinValue;
                        }
                    }

                    sfLead.LeadId = getFieldValue("Id", lead.Any);
                    sfLead.AnnualRevenue = AnnRev;
                    sfLead.City = getFieldValue("City", lead.Any);
                    sfLead.Company = getFieldValue("Company", lead.Any);
                    sfLead.ConvertedAccountId = getFieldValue("ConvertedAccountId", lead.Any);
                    sfLead.ConvertedContactId = getFieldValue("ConvertedContactId", lead.Any);
                    sfLead.ConvertedDate = conDate;
                    sfLead.ConvertedOpportunityId = getFieldValue("ConvertedOpportunityId", lead.Any);
                    sfLead.Country = getFieldValue("Country", lead.Any);
                    sfLead.Description = getFieldValue("Description", lead.Any);
                    sfLead.Email = getFieldValue("Email", lead.Any);
                    sfLead.EmailBouncedDate = ebDate;
                    sfLead.EmailBouncedReason = getFieldValue("EmailBouncedReason", lead.Any);
                    sfLead.Fax = getFieldValue("Fax", lead.Any);
                    sfLead.FirstName = getFieldValue("FirstName", lead.Any);
                    sfLead.HomePhone = getFieldValue("HomePhone", lead.Any);
                    sfLead.LastName = getFieldValue("LastName", lead.Any);
                    sfLead.MobilePhone = getFieldValue("MobilePhone", lead.Any);
                    sfLead.NumberOfEmployees = numEmp;
                    sfLead.Phone = getFieldValue("Phone", lead.Any);
                    sfLead.PostalCode = getFieldValue("PostalCode", lead.Any);
                    sfLead.State = getFieldValue("State", lead.Any);
                    sfLead.Street = getFieldValue("Street", lead.Any);
                    sfLead.Title = getFieldValue("Title", lead.Any);
                    sfLead.Website = getFieldValue("Website", lead.Any);

                    listSFLeads.Add(sfLead);
                }
                Session["SFLeads"] = listSFLeads;
                return listSFLeads;
            }
            else
            {
                return (List<SF_Lead>)Session["SFLeads"];
            }
        }
        private string getFieldValue(string fieldName, System.Xml.XmlElement[] fields)
        {
            string returnValue = string.Empty;
            if (fields != null)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].LocalName.ToLower().Equals(fieldName.ToLower()))
                    {
                        returnValue = fields[i].InnerText;
                    }
                }
            }

            return returnValue;
        }
        private List<SF_Campaign> GetSFCampaignsAll()
        {
            if (Session["SFCampaigns"] == null)
            {
                List<SF_Campaign> listSFCampaigns = new List<SF_Campaign>();
                SF_Campaign dummy = new SF_Campaign();
                dummy.CampaignId = "-1";
                dummy.Name = "-- Select SF Campaign --";
                listSFCampaigns.Add(dummy);

                SF_Campaign dummyNew = new SF_Campaign();
                dummyNew.CampaignId = "-2";
                dummyNew.Name = "-- NEW SF Campaign --";
                listSFCampaigns.Add(dummyNew);

                SforceService sessionBinding = (SforceService)Session["binding"];
                SalesForcePartner.QueryResult qr = sessionBinding.query("Select Id, ActualCost,AmountAllOpportunities,AmountWonOpportunities,BudgetedCost,CampaignMemberRecordTypeId,Description,EndDate,ExpectedResponse,ExpectedRevenue,Name,NumberOfContacts,NumberOfConvertedLeads,NumberOfLeads,NumberOfOpportunities,NumberOfResponses,NumberOfWonOpportunities,NumberSent,StartDate From Campaign Where IsActive = true");

                if (qr.records.Length > 0)
                {
                    for (int i = 0; i < qr.records.Length; i++)
                    {
                        SalesForcePartner.sObject campaign = qr.records[i];
                        SF_Campaign sfCampaign = new SF_Campaign();

                        sfCampaign.CampaignId = getFieldValue("Id", campaign.Any);

                        if (string.IsNullOrEmpty(getFieldValue("ActualCost", campaign.Any).ToString()))
                            sfCampaign.ActualCost = 0;
                        else
                            sfCampaign.ActualCost = Convert.ToDouble(getFieldValue("ActualCost", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("AmountAllOpportunities", campaign.Any).ToString()))
                            sfCampaign.AmountAllOpportunities = 0;
                        else
                            sfCampaign.AmountAllOpportunities = Convert.ToDouble(getFieldValue("AmountAllOpportunities", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("AmountWonOpportunities", campaign.Any).ToString()))
                            sfCampaign.AmountWonOpportunities = 0;
                        else
                            sfCampaign.AmountWonOpportunities = Convert.ToDouble(getFieldValue("AmountWonOpportunities", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("BudgetedCost", campaign.Any).ToString()))
                            sfCampaign.BudgetedCost = 0;
                        else
                            sfCampaign.BudgetedCost = Convert.ToDouble(getFieldValue("BudgetedCost", campaign.Any).ToString());

                        sfCampaign.CampaignMemberRecordTypeId = getFieldValue("CampaignMemberRecordTypeId", campaign.Any);
                        sfCampaign.Description = getFieldValue("Description", campaign.Any);

                        if (string.IsNullOrEmpty(getFieldValue("EndDate", campaign.Any).ToString()))
                            sfCampaign.EndDate = DateTime.MinValue;
                        else
                            sfCampaign.EndDate = Convert.ToDateTime(getFieldValue("EndDate", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("ExpectedResponse", campaign.Any).ToString()))
                            sfCampaign.ExpectedResponse = 0;
                        else
                            sfCampaign.ExpectedResponse = Convert.ToDouble(getFieldValue("ExpectedResponse", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("ExpectedRevenue", campaign.Any).ToString()))
                            sfCampaign.ExpectedRevenue = 0;
                        else
                            sfCampaign.ExpectedRevenue = Convert.ToDouble(getFieldValue("ExpectedRevenue", campaign.Any).ToString());

                        sfCampaign.Name = getFieldValue("Name", campaign.Any);

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfContacts", campaign.Any).ToString()))
                            sfCampaign.NumberOfContacts = 0;
                        else
                            sfCampaign.NumberOfContacts = Convert.ToInt32(getFieldValue("NumberOfContacts", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfConvertedLeads", campaign.Any).ToString()))
                            sfCampaign.NumberOfConvertedLeads = 0;
                        else
                            sfCampaign.NumberOfConvertedLeads = Convert.ToInt32(getFieldValue("NumberOfConvertedLeads", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfLeads", campaign.Any).ToString()))
                            sfCampaign.NumberOfLeads = 0;
                        else
                            sfCampaign.NumberOfLeads = Convert.ToInt32(getFieldValue("NumberOfLeads", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfOpportunities", campaign.Any).ToString()))
                            sfCampaign.NumberOfOpportunities = 0;
                        else
                            sfCampaign.NumberOfOpportunities = Convert.ToInt32(getFieldValue("NumberOfOpportunities", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfResponses", campaign.Any).ToString()))
                            sfCampaign.NumberOfResponses = 0;
                        else
                            sfCampaign.NumberOfResponses = Convert.ToInt32(getFieldValue("NumberOfResponses", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberOfWonOpportunities", campaign.Any).ToString()))
                            sfCampaign.NumberOfWonOpportunities = 0;
                        else
                            sfCampaign.NumberOfWonOpportunities = Convert.ToInt32(getFieldValue("NumberOfWonOpportunities", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("NumberSent", campaign.Any).ToString()))
                            sfCampaign.NumberSent = 0;
                        else
                            sfCampaign.NumberSent = Convert.ToDouble(getFieldValue("NumberSent", campaign.Any).ToString());

                        if (string.IsNullOrEmpty(getFieldValue("StartDate", campaign.Any).ToString()))
                            sfCampaign.StartDate = DateTime.MinValue;
                        else
                            sfCampaign.StartDate = Convert.ToDateTime(getFieldValue("StartDate", campaign.Any).ToString());

                        listSFCampaigns.Add(sfCampaign);
                    }
                }
                Session["SFCampaigns"] = listSFCampaigns;
                return listSFCampaigns;
            }
            else
            {
                return (List<SF_Campaign>)Session["SFCampaigns"];
            }

        }
        private List<SF_CampaignMemberStatus> GetCampaignMemberStatusByCampaignID(string campaignID)
        {
            List<SF_CampaignMemberStatus> listSFCampaigns = new List<SF_CampaignMemberStatus>();
            SforceService sessionBinding = (SforceService)Session["binding"];
            SalesForcePartner.QueryResult qr = sessionBinding.query("Select Id, CampaignId, HasResponded, IsDefault, Label, SortOrder From CampaignMemberStatus Where CampaignId = '" + campaignID + "'");

            if (qr.records.Length > 0)
            {
                for (int i = 0; i < qr.records.Length; i++)
                {
                    SalesForcePartner.sObject cms = qr.records[i];
                    SF_CampaignMemberStatus sfCMS = new SF_CampaignMemberStatus();

                    sfCMS.CampaignMemberStatusId = getFieldValue("Id", cms.Any);
                    sfCMS.CampaignId = getFieldValue("CampaignId", cms.Any);
                    sfCMS.HasResponded = Convert.ToBoolean(getFieldValue("HasResponded", cms.Any).ToString());
                    sfCMS.IsDefault = Convert.ToBoolean(getFieldValue("IsDefault", cms.Any).ToString());
                    sfCMS.Label = getFieldValue("Label", cms.Any);
                    sfCMS.SortOrder = Convert.ToInt32(getFieldValue("SortOrder", cms.Any).ToString());

                    listSFCampaigns.Add(sfCMS);
                }
            }

            return listSFCampaigns;
        }
        private void CreateNewSF_CampaignMemberStatus(List<SF_CampaignMemberStatus> newCMSList)
        {
            if (!loggedIn)
            {
                if (!login())
                    return;
            }


            sObject[] SFcampaignMemberStatus = new sObject[newCMSList.Count];
            int counter = 0;

            foreach (SF_CampaignMemberStatus item in newCMSList)
            {
                System.Xml.XmlElement[] cms = new System.Xml.XmlElement[5];
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                SalesForcePartner.sObject campaignMemberStatus = new SalesForcePartner.sObject();

                cms[0] = doc.CreateElement("CampaignId"); cms[0].InnerText = item.CampaignId.ToString();
                cms[1] = doc.CreateElement("HasResponded"); cms[1].InnerText = item.HasResponded.ToString();
                cms[2] = doc.CreateElement("IsDefault"); cms[2].InnerText = item.IsDefault.ToString();
                cms[3] = doc.CreateElement("Label"); cms[3].InnerText = item.Label.ToString();
                cms[4] = doc.CreateElement("SortOrder"); cms[4].InnerText = item.SortOrder.ToString();

                campaignMemberStatus.type = "CampaignMemberStatus";
                campaignMemberStatus.Any = cms;
                SFcampaignMemberStatus[counter] = campaignMemberStatus;

                counter++;
            }

            //create the object(s) by sending the array to the web service
            SforceService sessionBinding = (SforceService)Session["binding"];
            SaveResult[] sr = sessionBinding.create(SFcampaignMemberStatus);
            for (int j = 0; j < sr.Length; j++)
            {
                if (!sr[j].success)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sr[j].errors.Length; i++)
                    {
                        //get the next error
                        Error err = sr[j].errors[i];
                        sb.AppendLine("Errors were found on item " + j.ToString());
                        sb.AppendLine("Error code is: " + err.statusCode.ToString());
                        sb.AppendLine("Error message: " + err.message);
                    }
                }
            }
        }
        private void CreateUpdateCampaignMember(List<SF_CampaignMember> listCM)
        {
            if (!loggedIn)
            {
                if (!login())
                    return;
            }
            sObject[] sfCampMember = new sObject[listCM.Count];
            int counter = 0;

            foreach (SF_CampaignMember item in listCM)
            {
                System.Xml.XmlElement[] cm = new System.Xml.XmlElement[3];
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                SalesForcePartner.sObject campaignMember = new SalesForcePartner.sObject();

                cm[0] = doc.CreateElement("CampaignId"); cm[0].InnerText = item.CampaignId.ToString();
                cm[1] = doc.CreateElement("ContactId"); cm[1].InnerText = item.ContactId.ToString();
                //cm[2] = doc.CreateElement("FirstRespondedDate"); cm[2].InnerText = String.Format("{0:yyyy-MM-dd}", item.FirstRespondedDate);
                cm[2] = doc.CreateElement("Status"); cm[2].InnerText = item.Status.ToString();

                campaignMember.type = "CampaignMember";
                campaignMember.Any = cm;
                sfCampMember[counter] = campaignMember;

                counter++;
            }

            //create the object(s) by sending the array to the web service
            if (sfCampMember.Length > 0)
            {
                SforceService sessionBinding = (SforceService)Session["binding"];
                SaveResult[] srCreate = sessionBinding.create(sfCampMember);
                for (int j = 0; j < srCreate.Length; j++)
                {
                    if (!srCreate[j].success)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < srCreate[j].errors.Length; i++)
                        {
                            //get the next error
                            Error err = srCreate[j].errors[i];
                            sb.AppendLine("CREATE-Errors were found on item " + j.ToString());
                            sb.AppendLine("Error code is: " + err.statusCode.ToString());
                            sb.AppendLine("Error message: " + err.message);
                        }
                    }
                }

                //to update I need the CampaignMemberId of the items already there.
                //1.query the CampaignMember table
                //2.match up the items and assign the id
                //3. update
                SaveResult[] srUpdate = sessionBinding.update(sfCampMember);
                for (int j = 0; j < srUpdate.Length; j++)
                {
                    if (!srUpdate[j].success)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < srUpdate[j].errors.Length; i++)
                        {
                            //get the next error
                            Error err = srUpdate[j].errors[i];
                            sb.AppendLine("UPDATE-Errors were found on item " + j.ToString());
                            sb.AppendLine("Error code is: " + err.statusCode.ToString());
                            sb.AppendLine("Error message: " + err.message);
                        }
                    }
                }
            }
        }
        private string CreateSfContact(SF_Contact sfContact)
        {
            if (!loggedIn)
            {
                login();
            }

            var contacts = new sObject[]
            {
                CreateSObject(sfContact)
            };

            //create the object(s) by sending the array to the web service
            var sessionBinding = (ISforceService)SessionCollection[BindingKey];
            var saveResults = sessionBinding.create(contacts);

            //not positive if this is the id of the newly created contact - TEST THIS - it is the id of the newly created contact
            return saveResults[0].id;
        }

        private static sObject CreateSObject(SF_Contact sfContact)
        {
            var con = new XmlElement[SObjectXmlElementsCount];
            var doc = new XmlDocument();
            var contact = new sObject();

            con[0] = doc.CreateElement(AccountIdKey);
            con[0].InnerText = sfContact.AccountId.ToString();
            con[1] = doc.CreateElement(EmailKey);
            con[1].InnerText = sfContact.Email.ToString();
            con[2] = doc.CreateElement(FaxKey);
            con[2].InnerText = sfContact.Fax.ToString();
            con[3] = doc.CreateElement(FirstNameKey);
            con[3].InnerText = sfContact.FirstName.ToString();
            con[4] = doc.CreateElement(HomePhoneKey);
            con[4].InnerText = sfContact.HomePhone.ToString();
            con[5] = doc.CreateElement(LastNameKey);
            con[5].InnerText = sfContact.LastName.ToString();
            con[6] = doc.CreateElement(MailingCityKey);
            con[6].InnerText = sfContact.MailingCity.ToString();
            con[7] = doc.CreateElement(MailingStateKey);
            con[7].InnerText = sfContact.MailingState.ToString();
            con[8] = doc.CreateElement(MailingCountryKey);
            con[8].InnerText = sfContact.MailingCountry.ToString();
            con[9] = doc.CreateElement(MailingPostalCodeKey);
            con[9].InnerText = sfContact.MailingPostalCode.ToString();
            con[10] = doc.CreateElement(MailingStreetKey);
            con[10].InnerText = sfContact.MailingStreet.ToString();
            con[11] = doc.CreateElement(MobilePhoneKey);
            con[11].InnerText = sfContact.MobilePhone.ToString();
            con[12] = doc.CreateElement(PhoneKey);
            con[12].InnerText = sfContact.Phone.ToString();
            con[13] = doc.CreateElement(TitleKey);
            con[13].InnerText = sfContact.Title.ToString();
            contact.type = ContactType;
            contact.Any = con;
            return contact;
        }

        private string GetSFAccountID(string companyName)
        {
            string accountID = string.Empty;
            //get a list of Accounts so the user can select one
            List<SF_Account> sfAccounts = GetSFAccounts();

            foreach (SF_Account a in sfAccounts)
            {
                if (a.Name.ToUpper().Equals(companyName.ToUpper()))
                    accountID = a.AccountId;
            }

            if (string.IsNullOrEmpty(accountID))
            {
                //create new account
                SforceService sessionBinding = (SforceService)Session["binding"];
                SalesForcePartner.sObject account;
                sObject[] accs = new sObject[1];
                account = new SalesForcePartner.sObject();
                System.Xml.XmlElement[] acct = new System.Xml.XmlElement[2];
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                acct[0] = doc.CreateElement("AccountNumber"); acct[0].InnerText = companyName.Replace(" ", string.Empty).ToString();
                acct[1] = doc.CreateElement("Name"); acct[1].InnerText = companyName.ToString();
                account.type = "Account";
                account.Any = acct;
                accs[0] = account;


                //create the object(s) by sending the array to the web service
                SaveResult[] sr = sessionBinding.create(accs);
                for (int j = 0; j < sr.Length; j++)
                {
                    if (sr[j].success)
                    {
                        accountID = sr[j].id;
                    }
                }

                SF_Account newAccount = new SF_Account();
                newAccount.AccountId = accountID;
                newAccount.Name = companyName.ToString();
                newAccount.AccountNumber = companyName.Replace(" ", string.Empty).ToString();
                sfAccounts.Add(newAccount);

                Session["SFAccounts"] = sfAccounts;
            }
            return accountID;
        }
        #endregion

        #region Campaigns

        private void GetSFCampaignNames()
        {
            ddlSFCampaigns.DataSource = GetSFCampaignsAll();
            ddlSFCampaigns.DataTextField = "Name";
            ddlSFCampaigns.DataValueField = "CampaignId";
            ddlSFCampaigns.DataBind();
        }
        private void GetEcnEmailBlasts()
        {
            ddlEcnEmailBlast.DataSource = ECN_Blasts.GetBlastByCustomerID(CustomerID);
            ddlEcnEmailBlast.DataTextField = "EmailSubject";
            ddlEcnEmailBlast.DataValueField = "BlastID";
            ddlEcnEmailBlast.DataBind();
        }
        protected void ddlEcnEmailBlast_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSFCampaigns.Enabled = true;
        }
        protected void ddlSFCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSFCampaigns.SelectedValue.Equals("-2"))//-- NEW SF Campaign --
            {
                tbCampName.Text = string.Empty;
                tbCampBudget.Text = string.Empty;
                tbCampDesc.Text = string.Empty;
                tbCampEnd.Text = string.Empty;
                tbCampExpResp.Text = string.Empty;
                tbCampExpRev.Text = string.Empty;
                tbCampStart.Text = string.Empty;

                mpeNewSfCampaign.Show();
            }
            else if (!ddlSFCampaigns.SelectedValue.Equals("-1"))
            {
                CreateCampaignMemberStatus();
                btnCampaignDataUpload.Enabled = true;
            }
        }
        private void CreateCampaignMemberStatus()
        {
            List<SF_CampaignMemberStatus> listCMS = GetCampaignMemberStatusByCampaignID(ddlSFCampaigns.SelectedValue);
            if (listCMS.Count > 0)
            {
                List<SF_CampaignMemberStatus> newCMSList = new List<SF_CampaignMemberStatus>();
                //check and see if ECN values are there - add missing ones
                foreach (string typeCode in Enum.GetNames(typeof(ActionTypeCode)))
                {
                    bool contains = false;
                    foreach (SF_CampaignMemberStatus cms in listCMS)
                    {
                        if (cms.Label.ToUpper().Equals(typeCode.ToUpper()))
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        SF_CampaignMemberStatus newCMS = new SF_CampaignMemberStatus();
                        newCMS.CampaignId = ddlSFCampaigns.SelectedValue;
                        newCMS.HasResponded = Convert.ToBoolean((byte)Enum.Parse(typeof(ActionTypeCode), typeCode));
                        newCMS.IsDefault = false;
                        newCMS.Label = typeCode;
                        newCMS.SortOrder = listCMS.Count + newCMSList.Count + 1;

                        newCMSList.Add(newCMS);
                    }
                }
                if (newCMSList.Count > 0)
                    CreateNewSF_CampaignMemberStatus(newCMSList);
            }
            else //insert all values of ECN
            {
                // get a list of member names from Volume enum,
                // figure out the numeric value, and display
                List<SF_CampaignMemberStatus> newCMSList = new List<SF_CampaignMemberStatus>();

                foreach (string typeCode in Enum.GetNames(typeof(ActionTypeCode)))
                {
                    SF_CampaignMemberStatus newCMS = new SF_CampaignMemberStatus();
                    newCMS.CampaignId = ddlSFCampaigns.SelectedValue;
                    newCMS.HasResponded = Convert.ToBoolean((byte)Enum.Parse(typeof(ActionTypeCode), typeCode));
                    newCMS.IsDefault = false;
                    newCMS.Label = typeCode;
                    newCMS.SortOrder = listCMS.Count + newCMSList.Count + 1;

                    newCMSList.Add(newCMS);
                }

                CreateNewSF_CampaignMemberStatus(newCMSList);
            }
        }

        protected void btnCampaignDataUpload_Click(object sender, EventArgs e)
        {
            ddlSFCampaigns.Enabled = false;
            btnCampaignDataUpload.Enabled = false;
            //Get results of Blast by email address
            //create sf campaign members
            List<ECN_EmailActivityLog> listLog = ECN_EmailActivityLog.GetByBlastID(Convert.ToInt32(ddlEcnEmailBlast.SelectedValue.ToString()));
            List<SF_CampaignMember> listCampMember = new List<SF_CampaignMember>();
            List<SF_Contact> listSfAllContacts = GetSFContactsAll();

            string contactID = string.Empty;
            string previousEmail = string.Empty;
            string EcnAccountId = string.Empty;

            foreach (ECN_EmailActivityLog l in listLog)
            {
                if (!previousEmail.Equals(l.EmailAddress))
                    contactID = string.Empty;

                if (string.IsNullOrEmpty(contactID))
                {
                    foreach (SF_Contact s in listSfAllContacts)
                    {
                        if (s.Email.Equals(l.EmailAddress))
                        {
                            contactID = s.ContactId;
                        }
                    }
                }

                //if contactID is empty then the contact does not yet exist in SF so we need to create them
                if (string.IsNullOrEmpty(contactID))
                {
                    if (string.IsNullOrEmpty(EcnAccountId))
                        EcnAccountId = CheckForEcnAccount();

                    ECN_Contact ecnContact = GetECNContactByEmailAddress(l.EmailAddress);
                    SF_Contact sfCon = new SF_Contact();

                    sfCon.AccountId = EcnAccountId;
                    sfCon.Email = ecnContact.EmailAddress;
                    sfCon.Fax = ecnContact.Fax;
                    sfCon.FirstName = ecnContact.FirstName;
                    sfCon.HomePhone = ecnContact.Voice;
                    if (!string.IsNullOrEmpty(ecnContact.LastName))
                        sfCon.LastName = ecnContact.LastName;
                    else
                        sfCon.LastName = "UNKOWN";
                    sfCon.MailingCity = ecnContact.City;
                    sfCon.MailingCountry = ecnContact.Country;
                    sfCon.MailingPostalCode = ecnContact.PostalCode;
                    sfCon.MailingState = ecnContact.State;
                    sfCon.MailingStreet = ecnContact.Address;
                    sfCon.MobilePhone = ecnContact.Mobile;
                    sfCon.Phone = ecnContact.Voice;
                    sfCon.Title = ecnContact.Title;

                    contactID = CreateSfContact(sfCon);
                }

                previousEmail = l.EmailAddress;

                SF_CampaignMember cm = new SF_CampaignMember();
                cm.CampaignId = ddlSFCampaigns.SelectedValue;
                cm.ContactId = contactID;
                cm.FirstRespondedDate = l.ActionDate;
                cm.Status = l.ActionTypeCode;

                listCampMember.Add(cm);
            }

            //send CampaignMembers to SF
            if (listCampMember.Count > 0)
            {
                CreateUpdateCampaignMember(listCampMember);
                lbMessage.Text = "Successfully Created/Updated " + listCampMember.Count + " campaign member records. Per SalesForce please wait up to 10 minutes for data to be available.";
                mpeMessages.Show();
            }
            else
            {
                lbMessage.Text = "There is no data in the selected Campaign...Nothing will be sent to SalesForce";
                mpeMessages.Show();
            }
        }

        protected void btnCampOK_Click(object sender, EventArgs e)
        {
            if (!loggedIn)
            {
                if (!login())
                    return;
            }

            SalesForcePartner.sObject camp;
            sObject[] camps = new sObject[1];
            camp = new SalesForcePartner.sObject();
            System.Xml.XmlElement[] c = new System.Xml.XmlElement[8];
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            //validate text entry
            string cName = CleanString(tbCampName.Text.ToString());
            string cBudget = CleanNumber(tbCampBudget.Text.ToString());
            string cDesc = CleanString(tbCampDesc.Text.ToString());
            DateTime cStart = Convert.ToDateTime(tbCampStart.Text.ToString());
            DateTime cEnd = Convert.ToDateTime(tbCampEnd.Text.ToString());
            string cExpResp = CleanNumber(tbCampExpResp.Text.ToString());
            string cExpRev = CleanNumber(tbCampExpRev.Text.ToString());
            //String.Format("{0:yyyy-MM-dd}", item.FirstRespondedDate);
            c[0] = doc.CreateElement("Name"); c[0].InnerText = cName;
            c[1] = doc.CreateElement("BudgetedCost"); c[1].InnerText = cBudget;
            c[2] = doc.CreateElement("Description"); c[2].InnerText = cDesc;
            c[3] = doc.CreateElement("EndDate"); c[3].InnerText = cEnd.ToString("yyyy-MM-dd");
            c[4] = doc.CreateElement("StartDate"); c[4].InnerText = cStart.ToString("yyyy-MM-dd");
            c[5] = doc.CreateElement("ExpectedResponse"); c[5].InnerText = cExpResp;
            c[6] = doc.CreateElement("ExpectedRevenue"); c[6].InnerText = cExpRev;
            c[7] = doc.CreateElement("IsActive"); c[7].InnerText = "1";

            camp.type = "Campaign";
            camp.Any = c;
            camps[0] = camp;

            //create the object(s) by sending the array to the web service
            SforceService sessionBinding = (SforceService)Session["binding"];
            SaveResult[] sr = sessionBinding.create(camps);
            string selectedCampID = string.Empty;

            for (int j = 0; j < sr.Length; j++)
            {
                if (sr[j].success)
                {
                    List<SF_Campaign> listCamps = (List<SF_Campaign>)Session["SFCampaigns"];
                    SF_Campaign newCamp = new SF_Campaign();
                    newCamp.Name = cName;
                    newCamp.BudgetedCost = Convert.ToInt32(cBudget.ToString());
                    newCamp.Description = cDesc;
                    newCamp.StartDate = Convert.ToDateTime(cStart.ToString());
                    newCamp.EndDate = Convert.ToDateTime(cEnd.ToString());
                    newCamp.ExpectedResponse = Convert.ToInt32(cExpResp.ToString());
                    newCamp.ExpectedRevenue = Convert.ToInt32(cExpRev.ToString());
                    newCamp.CampaignId = sr[j].id.ToString();
                    listCamps.Add(newCamp);
                    Session["SFCampaigns"] = listCamps;

                    selectedCampID = sr[j].id;
                    lbMessage.Text = "A campaign was create with an id of: " + sr[j].id;
                    mpeMessages.Show();
                }
                else
                {
                    //there were errors during the create call, go through the errors
                    //array and write them to the screen
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sr[j].errors.Length; i++)
                    {
                        //get the next error
                        Error err = sr[j].errors[i];
                        sb.AppendLine("Errors were found on item " + j.ToString());
                        sb.AppendLine("Error code is: " + err.statusCode.ToString());
                        sb.AppendLine("Error message: " + err.message);
                    }
                    lbMessage.Text = sb.ToString();
                    mpeMessages.Show();
                }
            }

            GetSFCampaignNames();
            ddlSFCampaigns.SelectedValue = selectedCampID;
            CreateCampaignMemberStatus();
            btnCampaignDataUpload.Enabled = true;
        }
        #endregion

        #region ECN_to_SF

        protected void pagerECNContacts_IndexChanged(object sender, System.EventArgs e)
        {
            pagerCurrentPage = pagerECNContacts.CurrentPage;
            int groupID = Convert.ToInt32(ddlECNGroups.SelectedValue.ToString());
            BindECNContactGrid(groupID);

        }
        protected void ddlECNGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            int groupID = Convert.ToInt32(ddlECNGroups.SelectedValue.ToString());
            BindECNContactGrid(groupID);
        }
        private void BindECNContactGrid(int GroupID)
        {
            if (GroupID == -1)
                gvECNContacts.DataSource = GetECNContactsAll();
            else
                gvECNContacts.DataSource = GetECNContacts(GroupID);
            pagerECNContacts.RecordCount = pagerRecordCount;
            gvECNContacts.DataBind();
        }
        protected void btnSendToSF_Click(object sender, EventArgs e)
        {
            //1.do we have an ECN account setup in SF
            //2.get all selected items from the grid
            //3.send list to SF
            string EcnAccountId = CheckForEcnAccount(); //1. done
            //step 2
            List<SF_Contact> listSfContacts = new List<SF_Contact>();

            foreach (GridViewRow r in gvECNContacts.Rows)
            {
                CheckBox item = (CheckBox)r.FindControl("cbRowItem");
                string sfAccountID = string.Empty;
                if (item.Checked)
                {
                    SF_Contact sfCon = new SF_Contact();
                    //1(EmailAddress)2(FirstName)3(LastName)4(Address)5(State)6(PostalCode)7(Country)8(City)9(title)10(Company)11(Voice)12(Mobile)13(Fax)14(GroupName)
                    if (!string.IsNullOrEmpty(r.Cells[10].Text.ToString()))
                        sfAccountID = GetSFAccountID(r.Cells[10].Text.ToString());
                    else
                        sfAccountID = EcnAccountId;
                    sfCon.AccountId = sfAccountID;
                    sfCon.Email = r.Cells[1].Text.ToString();
                    sfCon.Fax = r.Cells[13].Text.ToString();
                    sfCon.FirstName = r.Cells[2].Text.ToString();
                    sfCon.HomePhone = r.Cells[11].Text.ToString();
                    sfCon.LastName = r.Cells[3].Text.ToString();
                    sfCon.MailingCity = r.Cells[8].Text.ToString();
                    sfCon.MailingCountry = r.Cells[7].Text.ToString();
                    sfCon.MailingPostalCode = r.Cells[6].Text.ToString();
                    sfCon.MailingState = r.Cells[5].Text.ToString();
                    sfCon.MailingStreet = r.Cells[4].Text.ToString();
                    sfCon.MobilePhone = r.Cells[12].Text.ToString();
                    sfCon.Phone = r.Cells[11].Text.ToString();
                    sfCon.Title = r.Cells[9].Text.ToString();

                    listSfContacts.Add(sfCon);
                }
            }

            //make sure contact already isn't created
            List<SF_Contact> sfAll = GetSFContactsAll();
            foreach (SF_Contact sAll in sfAll)
            {
                foreach (SF_Contact sf in listSfContacts)
                {
                    if (sAll.Email.Equals(sf.Email))
                    {
                        listSfContacts.Remove(sf);
                        break;
                    }
                }
            }
            //step 3
            if (listSfContacts.Count > 0)
                CreateSfContacts(listSfContacts);
            //done
            //give some validation that the process completed.
            lbMessage.Text = "Successfully sent " + listSfContacts.Count + " contacts to SalesForce.com.";
            mpeMessages.Show();
        }
        private void CreateSfContacts(List<SF_Contact> list)
        {
            if (!loggedIn)
            {
                if (!login())
                    return;
            }

            var contacts = new sObject[list.Count];
            var counter = 0;

            foreach (var sfContact in list)
            {
                contacts[counter] = CreateSObject(sfContact);
                counter++;
            }

            //create the object(s) by sending the array to the web service
            var sessionBinding = (ISforceService)SessionCollection[BindingKey];
            var saveResults = sessionBinding.create(contacts);
            for (var j = 0; j < saveResults.Length; j++)
            {
                if (!saveResults[j].success)
                {
                    var stringBuilder = new StringBuilder();
                    for (var i = 0; i < saveResults[j].errors.Length; i++)
                    {
                        //get the next error
                        var error = saveResults[j].errors[i];
                        stringBuilder.AppendLine(string.Format("Errors were found on item {0}", j.ToString()));
                        stringBuilder.AppendLine(string.Format("Error code is: {0}", error.statusCode.ToString()));
                        stringBuilder.AppendLine(string.Format("Error message: {0}" + error.message));
                    }
                    lbMessage.Text = stringBuilder.ToString();
                    mpeMessages.Show();
                }
            }
        }
        private string CheckForEcnAccount()
        {
            List<SF_Account> sfAccounts = GetSFAccounts();
            bool ecnAccount = false;
            foreach (SF_Account sf in sfAccounts)
            {
                if (sf.AccountNumber.ToString().Equals("ECN2010"))
                {
                    ecnAccount = true;
                    break;
                }
            }
            string EcnAccountId = string.Empty;
            if (ecnAccount)
            {
                SF_Account ecn = new SF_Account();
                foreach (SF_Account a in sfAccounts)
                {
                    if (a.AccountNumber.ToString().Equals("ECN2010"))
                        ecn = a;
                }
                EcnAccountId = ecn.AccountId.ToString();
            }
            else
            {
                EcnAccountId = CreateEcnAccountInSF();
            }

            return EcnAccountId;
        }
        private string CreateEcnAccountInSF()
        {
            string acctID = string.Empty;
            //Verify that we are already authenticated, if not
            //call the login function to do so
            if (!loggedIn)
            {
                if (!login())
                    return acctID;
            }

            try
            {
                SalesForcePartner.sObject account;
                sObject[] accs = new sObject[1];
                account = new SalesForcePartner.sObject();
                System.Xml.XmlElement[] acct = new System.Xml.XmlElement[10];
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                acct[0] = doc.CreateElement("AccountNumber"); acct[0].InnerText = "ECN2010";
                acct[1] = doc.CreateElement("Name"); acct[1].InnerText = "ECN";
                acct[2] = doc.CreateElement("Description"); acct[2].InnerText = "Account for integrating SalesForce and ECN data.";
                acct[3] = doc.CreateElement("BillingStreet"); acct[3].InnerText = "15301 Highway 55 Suite 3A";
                acct[4] = doc.CreateElement("BillingCity"); acct[4].InnerText = "Plymouth";
                acct[5] = doc.CreateElement("BillingState"); acct[5].InnerText = "MN";
                acct[6] = doc.CreateElement("BillingPostalCode"); acct[6].InnerText = "55447";
                acct[7] = doc.CreateElement("BillingCountry"); acct[7].InnerText = "US";
                acct[8] = doc.CreateElement("Phone"); acct[8].InnerText = "763.746.2780";
                acct[9] = doc.CreateElement("Website"); acct[9].InnerText = "www.knowledgemarketing.com";
                account.type = "Account";
                account.Any = acct;
                accs[0] = account;


                //create the object(s) by sending the array to the web service
                SforceService sessionBinding = (SforceService)Session["binding"];
                SaveResult[] sr = sessionBinding.create(accs);
                for (int j = 0; j < sr.Length; j++)
                {
                    if (sr[j].success)
                    {
                        lbMessage.Text = "An account was create with an id of: " + sr[j].id;
                        mpeMessages.Show();
                        acctID = sr[j].id;
                        return acctID;
                    }
                    else
                    {
                        //there were errors during the create call, go through the errors
                        //array and write them to the screen
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < sr[j].errors.Length; i++)
                        {
                            //get the next error
                            Error err = sr[j].errors[i];
                            sb.AppendLine("Errors were found on item " + j.ToString());
                            sb.AppendLine("Error code is: " + err.statusCode.ToString());
                            sb.AppendLine("Error message: " + err.message);
                        }
                        lbMessage.Text = sb.ToString();
                        mpeMessages.Show();
                        return acctID;
                    }
                }
                return acctID;
            }
            catch (Exception ex)
            {
                lbMessage.Text = "Failed to create account, error message was: \n" + ex.Message;
                mpeMessages.Show();
                return acctID;
            }
        }

        #endregion

        private string CleanString(string dirty)
        {
            string clean = string.Empty;
            clean = dirty.Replace(@",", string.Empty);
            clean = dirty.Replace(@"$", string.Empty);
            clean = dirty.Replace(@"'", string.Empty);
            clean = dirty.Replace(@"%", string.Empty);
            clean = dirty.Replace(@"*", string.Empty);
            clean = dirty.Replace(@"#", string.Empty);
            clean = dirty.Replace(@"\", string.Empty);
            clean = dirty.Replace(@"/", string.Empty);
            clean = dirty.Replace(@".", string.Empty);
            return clean;
        }

        private string CleanNumber(string dirty)
        {
            if (string.IsNullOrEmpty(dirty))
            {
                return "0";
            }
            else
            {
                string clean = string.Empty;
                System.Text.RegularExpressions.Regex x = new System.Text.RegularExpressions.Regex("^0-9");
                clean = x.Replace(dirty, string.Empty);
                return clean;
            }
        }

        #region SF_to_ECN

        protected void ddlSFAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string accountID = ddlSFAccounts.SelectedValue.ToString();
            //BindSFContactGrid(accountID);
            switch (ddlSFAccounts.SelectedValue.ToString())
            {
                case "Contacts - Accounts":
                    gvSFContacts.DataSource = GetSFContactsAll();
                    gvSFContacts.DataBind();
                    break;
                case "Contacts - Leads":
                    gvSFContacts.DataSource = ConvertLeadToContact();
                    gvSFContacts.DataBind();
                    break;
                case "Contacts - All":
                    gvSFContacts.DataSource = CombineSfLeadsSfContacts();
                    gvSFContacts.DataBind();
                    break;
            }
        }
        private List<SF_Contact> ConvertLeadToContact()
        {
            List<SF_Contact> retCon = new List<SF_Contact>();
            List<SF_Lead> allLead = GetSFLeads();
            foreach (SF_Lead l in allLead)
            {
                SF_Contact c = new SF_Contact();
                c.Email = l.Email;
                c.Fax = l.Fax;
                c.FirstName = l.FirstName;
                c.HomePhone = l.HomePhone;
                c.LastName = l.LastName;
                c.MailingCity = l.City;
                c.MailingCountry = l.Country;
                c.MailingPostalCode = l.PostalCode;
                c.MailingState = l.State;
                c.MailingStreet = l.Street;
                c.MobilePhone = l.MobilePhone;
                c.Phone = l.Phone;
                c.Title = l.Title;
                retCon.Add(c);
            }
            return retCon;
        }
        private List<SF_Contact> CombineSfLeadsSfContacts()
        {
            List<SF_Contact> allContact = GetSFContactsAll();
            List<SF_Lead> allLead = GetSFLeads();

            foreach (SF_Lead l in allLead)
            {
                SF_Contact c = new SF_Contact();
                c.Email = l.Email;
                c.Fax = l.Fax;
                c.FirstName = l.FirstName;
                c.HomePhone = l.HomePhone;
                c.LastName = l.LastName;
                c.MailingCity = l.City;
                c.MailingCountry = l.Country;
                c.MailingPostalCode = l.PostalCode;
                c.MailingState = l.State;
                c.MailingStreet = l.Street;
                c.MobilePhone = l.MobilePhone;
                c.Phone = l.Phone;
                c.Title = l.Title;
                allContact.Add(c);
            }
            return allContact;
        }
        protected void ddlECNGroupFromSF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlECNGroupFromSF.SelectedValue.ToString().Equals("-1"))
            {
                //create a new group
                tbNewGroupDesc.Text = string.Empty;
                tbNewGroupName.Text = string.Empty;
                mpeNewEcnGroup.Show();
            }

            btnImportToECN.Enabled = true;
        }
        protected void btnCreateNewGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNewGroupName.Text.ToString()))
            {
                //throw an error
                lbMessage.Text = "Group Name can not be blank.";
                mpeMessages.Show();
            }
            else
            {
                string desc = tbNewGroupDesc.Text.ToString();
                string name = tbNewGroupName.Text.ToString();
                // create a connection object
                var conn = GetSqlConn();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("INSERT INTO Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,PublicFolder,AllowUDFHistory,IsSeedList) ");
                sb.AppendLine("VALUES(" + CustomerID + ",0,'" + name + "','" + desc + "','customer',0,'N',0)");
                // create a command object
                var cmd = DBAdapter.CreateCommand(sb.ToString(), conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                //add the new group to the ddl of groups
                AddNewGroup(name);
                //pnlNewEcnGroup.Visible = false;

                lbMessage.Text = "Group created successfully.";
                mpeMessages.Show();
            }

        }
        private void AddNewGroup(string groupName)
        {
            // create a connection object
            var conn = GetSqlConn();
            // create a command object
            var cmd = DBAdapter.CreateCommand("SELECT GroupID,GroupName FROM Groups WHERE CustomerID =" + CustomerID + " AND GroupName='" + groupName + "'", conn);
            conn.Open();
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ECN_Group item = new ECN_Group();
                int index;
                string name;

                name = "GroupID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.GroupID = (int)rdr[index];

                name = "GroupName";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    item.GroupName = (string)rdr[index];

                ListItem li = new ListItem();
                li.Text = item.GroupName.ToString();
                li.Value = item.GroupID.ToString();

                ddlECNGroupFromSF.Items.Add(li);
                ddlECNGroupFromSF.SelectedValue = item.GroupID.ToString();
            }
            rdr.Close();
            conn.Close();
        }
        //private void BindSFContactGrid(string accountID)
        //{
        //    if (accountID.Equals("-1"))
        //        gvSFContacts.DataSource = GetSFContactsAll();
        //    else
        //        gvSFContacts.DataSource = GetSFContacts(accountID);
        //    gvSFContacts.DataBind();
        //}
        protected void btnImportToECN_Click(object sender, EventArgs e)
        {
            //1.get all selected items from the grid
            List<ECN_Contact> listECNContacts = new List<ECN_Contact>();

            foreach (GridViewRow r in gvSFContacts.Rows)
            {
                CheckBox item = (CheckBox)r.FindControl("cbRowItem");

                if (item.Checked)
                {
                    if (r.Cells[3].Text.ToString().Contains("@"))
                    {
                        ECN_Contact ecnCon = new ECN_Contact();
                        //0-select all //1 - ContactID //2-AccountID //3-Email //4-Fax //5-FirstName //6-HomePhone //7-LastName 
                        //8-MailingCity //9-MailingState //10-MailingCountry //11-MailingPostalCode //12-MailingStreet
                        //13-MobilePhone //14-Phone //15-Title
                        ecnCon.Address = r.Cells[12].Text.ToString();
                        ecnCon.City = r.Cells[8].Text.ToString();
                        ecnCon.Country = r.Cells[10].Text.ToString();
                        ecnCon.EmailAddress = r.Cells[3].Text.ToString();
                        ecnCon.Fax = r.Cells[4].Text.ToString();
                        ecnCon.FirstName = r.Cells[5].Text.ToString();
                        ecnCon.GroupName = ddlECNGroupFromSF.SelectedItem.Text.ToString();
                        ecnCon.LastName = r.Cells[7].Text.ToString();
                        ecnCon.Mobile = r.Cells[13].Text.ToString();
                        ecnCon.PostalCode = r.Cells[11].Text.ToString();
                        ecnCon.State = r.Cells[9].Text.ToString();
                        ecnCon.Title = r.Cells[15].Text.ToString();
                        ecnCon.Voice = r.Cells[14].Text.ToString();

                        listECNContacts.Add(ecnCon);
                    }
                }
            }

            //2.send list to ECN via sp_importEmails have to get XML files
            ExecuteSproc(EmailAddressToXML(listECNContacts));

            //give some validation that the process completed.
            lbMessage.Text = "Successfully imported " + listECNContacts.Count + " contacts from SalesForce.com";
            mpeMessages.Show();
        }
        private XmlDocument EmailAddressToXML(List<ECN_Contact> list)
        {
            XmlDocument xmlDoc = new XmlDocument();
            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("XML");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);


            foreach (ECN_Contact n in list)
            {
                // Create a new <Category> element and add it to the root node
                XmlElement parentNode = xmlDoc.CreateElement("Emails");
                xmlDoc.DocumentElement.PrependChild(parentNode);
                // Create the required nodes
                XmlElement nEmail = xmlDoc.CreateElement("emailaddress");
                XmlElement nAddress = xmlDoc.CreateElement("address");
                XmlElement nCity = xmlDoc.CreateElement("city");
                XmlElement nCountry = xmlDoc.CreateElement("country");
                XmlElement nFax = xmlDoc.CreateElement("fax");
                XmlElement nFirstName = xmlDoc.CreateElement("firstname");
                XmlElement nLastName = xmlDoc.CreateElement("lastname");
                XmlElement nMobile = xmlDoc.CreateElement("mobile");
                XmlElement nPostalCode = xmlDoc.CreateElement("zip");
                XmlElement nFullName = xmlDoc.CreateElement("fullname");
                XmlElement nState = xmlDoc.CreateElement("state");
                XmlElement nTitle = xmlDoc.CreateElement("title");
                XmlElement nVoice = xmlDoc.CreateElement("voice");
                // retrieve the text 
                XmlText tEmail = xmlDoc.CreateTextNode(n.EmailAddress.ToString());
                XmlText tAddress = xmlDoc.CreateTextNode(n.Address.ToString());
                XmlText tCity = xmlDoc.CreateTextNode(n.City.ToString());
                XmlText tCountry = xmlDoc.CreateTextNode(n.Country.ToString());
                XmlText tFax = xmlDoc.CreateTextNode(n.Fax.ToString());
                XmlText tFirstName = xmlDoc.CreateTextNode(n.FirstName.ToString());
                XmlText tLastName = xmlDoc.CreateTextNode(n.LastName.ToString());
                XmlText tMobile = xmlDoc.CreateTextNode(n.Mobile.ToString());
                XmlText tPostalCode = xmlDoc.CreateTextNode(n.PostalCode.ToString());
                XmlText tFullName = xmlDoc.CreateTextNode(n.FirstName.ToString() + " " + n.LastName.ToString());
                XmlText tState = xmlDoc.CreateTextNode(n.State.ToString());
                XmlText tTitle = xmlDoc.CreateTextNode(n.Title.ToString());
                XmlText tVoice = xmlDoc.CreateTextNode(n.Voice.ToString());

                // append the nodes to the parentNode without the value
                parentNode.AppendChild(nEmail);
                parentNode.AppendChild(nAddress);
                parentNode.AppendChild(nCity);
                parentNode.AppendChild(nCountry);
                parentNode.AppendChild(nFax);
                parentNode.AppendChild(nFirstName);
                parentNode.AppendChild(nLastName);
                parentNode.AppendChild(nMobile);
                parentNode.AppendChild(nPostalCode);
                parentNode.AppendChild(nFullName);
                parentNode.AppendChild(nState);
                parentNode.AppendChild(nTitle);
                parentNode.AppendChild(nVoice);

                // save the value of the fields into the nodes
                nEmail.AppendChild(tEmail);
                nAddress.AppendChild(tAddress);
                nCity.AppendChild(tCity);
                nCountry.AppendChild(tCountry);
                nFax.AppendChild(tFax);
                nFirstName.AppendChild(tFirstName);
                nLastName.AppendChild(tLastName);
                nMobile.AppendChild(tMobile);
                nPostalCode.AppendChild(tPostalCode);
                nFullName.AppendChild(tFullName);
                nState.AppendChild(tState);
                nTitle.AppendChild(tTitle);
                nVoice.AppendChild(tVoice);
            }


            return xmlDoc;
        }
        private void ExecuteSproc(XmlDocument docEmail)
        {
            //get email xml
            StringWriter swEmail = new StringWriter();
            XmlTextWriter xtwEmail = new XmlTextWriter(swEmail);
            docEmail.WriteTo(xtwEmail);

            //dummy xml for udf
            XmlDocument xmlDoc = new XmlDocument();
            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("XML");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);
            StringWriter swDummy = new StringWriter();
            XmlTextWriter xtwDummy = new XmlTextWriter(swDummy);
            xmlDoc.WriteTo(xtwDummy);

            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(Master.UserSession.CurrentUser, CustomerID, Convert.ToInt32(ddlECNGroupFromSF.SelectedValue.ToString()), swEmail.ToString(), swDummy.ToString(), "HTML", "S", true, "", "Ecn.communicator.main.lists.sfintegration.ExecuteSproc");

            
        }

        #endregion


    }
}
