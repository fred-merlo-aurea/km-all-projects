using System;
using System.Data;
using System.Web.UI;
using Ecn.Accounts.Interfaces;
using Ecn.Accounts.Helpers;
using ecn.common.classes;
using SecurityAccess = ECN_Framework.Common.SecurityAccess;

namespace ecn.accounts.customersmanager
{
    public partial class licensedetail : ECN_Framework.WebPageHelper
    {
        protected string CL_CLID = "";
        protected string CL_CustomerID = "";
        protected string CL_LicenseTypeCode = "";
        protected string CL_Quantity = "";
        protected string CL_Used = "";
        protected string CL_ExpirationDate = "";
        protected string CL_AddDate = "";

        private IDataFunctions _dataFunctions;

        public licensedetail()
        {
            Page.Init += new System.EventHandler(Page_Init);
            _dataFunctions = new DataFunctionsAdapter();
        }

        public licensedetail(IDataFunctions dataFunctions)
        {
            _dataFunctions = dataFunctions;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icocustomers.gif><b>Unsent Emails</b><br />These are the emails you wrote or started writing but have not sent. You can also edit an email before you send it, Click the edit link. To send the email, first set the groups you want to recieve this Blast. </p>&#13;&#10;&#9;&#9;<p><b>Sent Emails</b><br />These emails are stored in your database and are available to view and/or send again. </p>&#13;&#10;&#9;&#9;<p><b>Helpful Hint</b><br />To send an email again, first 'view' the email and while viewing the email click 'write new email' link in the navigation. All you have to do is select the layout you want, rename it and click the preview email button.</p>&#13;&#10;&#9;&#9;";
            Master.HelpTitle = "Customer Manager";          

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                int requestCLID = getCLID();
                if (Page.IsPostBack == false)
                {
                    if (requestCLID > 0)
                    {
                        if (!(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser)))
                        {
                            SecurityAccess.canI("CustomerLicenses", requestCLID.ToString());
                        }
                        string cust_id = getCustFromCLID(requestCLID);
                        string base_channel_id = getBaseChannelFromCust(cust_id);
                        //throw new Exception("cust=" + cust_id + " channel= " + base_channel_id);
                        LoadBaseChannelDD(base_channel_id);
                        LoadChannelDD();
                        LoadCustomersDD();
                        LoadLicenseDD();
                    }
                    else
                    {
                        LoadBaseChannelDD();
                        LoadChannelDD();
                        LoadCustomersDD();
                        LoadLicenseDD();
                    }
                    if (requestCLID > 0)
                    {
                        //existing customer
                        SetUpdateInfo(requestCLID);
                        //LoadFormData(requestCLID);
                    }
                    if (KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                    {
                        BaseChannelList.Enabled = false;
                    }
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        public int getCLID()
        {
            int theCLID = 0;
            try
            {
                theCLID = Convert.ToInt32(Request.QueryString["CLID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCLID;
        }


        #region Form Prep
        private void SetUpdateInfo(int setCLID)
        {
            CLID.Text = setCLID.ToString();
            SaveButton.Text = "Update";
            SaveButton.Visible = false;
            //UpdateButton.Visible=false;
            UpdateButton.Visible = true;


            //pageheader.divContentTitle="Edit License";

            this.BaseChannelList.Enabled = false;
            this.ChannelList.Enabled = false;
            this.CustomerID.Enabled = false;
            this.LicenseTypeCode.Enabled = false;

            if (this.CL_LicenseTypeCode.StartsWith("emailblock"))
            {
                ChannelList.ClearSelection();
                ChannelList.Items.FindByText(BaseChannelList.SelectedItem.Text.ToString() + " (communicator)").Selected = true;
            }
            try
            {
                CustomerID.Items.FindByValue(CL_CustomerID).Selected = true;
            }
            catch
            {
                //do nothing
            }
            this.Quantity.Text = CL_Quantity;
            this.Used.Text = CL_Used;
            AddDate.Date = DateTime.Parse(CL_AddDate);
            ExpirationDate.Date = DateTime.Parse(CL_ExpirationDate);
        }

        public void LoadBaseChannelDD()
        {
            BaseChannelList.DataSource = DataLists.GetBaseChannelsDR();
            BaseChannelList.DataBind();
            BaseChannelList.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
        }

        public void LoadBaseChannelDD(string base_channel_id)
        {
            BaseChannelList.DataSource = DataLists.GetBaseChannelsDR();
            BaseChannelList.DataBind();
            BaseChannelList.Items.FindByValue(base_channel_id).Selected = true;
        }

        private void LoadChannelDD()
        {
            ChannelList.DataSource = DataLists.GetChannelsDR(BaseChannelList.SelectedValue);
            ChannelList.DataBind();
        }

        private void LoadCustomersDD()
        {
            LoadCustomersDropdown();
        }

        private string getCustFromCLID(int CLID)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM [CustomerLicense] " +
                " WHERE CLID=" + CLID + " and IsDeleted = 0 ";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                CL_CLID = dr["CLID"].ToString();
                CL_CustomerID = dr["CustomerID"].ToString();
                CL_LicenseTypeCode = dr["LicenseTypeCode"].ToString();
                CL_Quantity = dr["Quantity"].ToString();
                CL_Used = dr["Used"].ToString();
                CL_ExpirationDate = dr["ExpirationDate"].ToString();
                CL_AddDate = dr["AddDate"].ToString();
            }
            return CL_CustomerID;
        }

        private string getChannelFromCust(string cust_id)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM [Customer] " +
                " WHERE CustomerID=" + cust_id + " and IsDeleted = 0 ";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
            DataRow dr = dt.Rows[0];
            return dr["BaseChannelID"].ToString();
        }

        private string getBaseChannelFromCust(string cust_id)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM [Customer] " +
                " WHERE CustomerID=" + cust_id + " and IsDeleted = 0 ";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
            DataRow dr = dt.Rows[0];
            return dr["BaseChannelID"].ToString();
        }

        //channelDD overload
        private void LoadChannelDD(string channelID)
        {
            ChannelList.DataSource = DataLists.GetChannelsDR(channelID);
            ChannelList.DataBind();
            LoadCustomersDD();
        }

        public void LoadChannelDDfromBaseChannel(object sender, System.EventArgs e)
        {
            ChannelList.DataSource = DataLists.GetChannelsDR(BaseChannelList.SelectedValue);
            ChannelList.DataBind();
            LoadChannelDD(BaseChannelList.SelectedValue);
        }

        public void LoadCustomersDDfromChannels(object sender, System.EventArgs e)
        {
            LoadCustomersDropdown();
        }

        private void LoadLicenseDD()
        {
            LicenseTypeCode.DataSource = DataLists.GetCodesDR("LicenseType");
            LicenseTypeCode.DataBind();
        }



        #endregion

        #region Data Load

        private void LoadFormData(int setCLID)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM [CustomerLicense] " +
                " WHERE CLID=" + setCLID + " and IsDeleted = 0 ";
            DataTable customerLicenseDataTable = _dataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
            if (customerLicenseDataTable == null)
            {
                throw new InvalidOperationException("Unable to retrieve customer license information. Result was null.");
            }
            if (customerLicenseDataTable.Rows != null && customerLicenseDataTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("Unable to retrieve customer license information. Result was empty.");
            }

            DataRow dr = customerLicenseDataTable.Rows[0];
            //            throw new Exception("bob="+dt.Rows[0]["CustomerID"].ToString());
            CustomerID.Items.FindByValue(dr["CustomerID"].ToString()).Selected = true;
            CustomerID.Enabled = false;
            LicenseTypeCode.Items.FindByValue(dr["LicenseTypeCode"].ToString()).Selected = true;
            Quantity.Text = dr["Quantity"].ToString();
            Used.Text = dr["Used"].ToString();
            AddDate.Date = (DateTime)dr["AddDate"];
            ExpirationDate.Date = (DateTime)dr["ExpirationDate"];
        }

        public void LoadCustomersDropdown()
        {
            LoadCustomersBasedOnChannelListSelectedItem();
        }

        private void LoadCustomersBasedOnChannelListSelectedItem()
        {
            var channelName = ChannelList.SelectedItem.Text;
            if (channelName.EndsWith("communicator)"))
            {
                var sqlquery = string.Format(
                    "{0}{1}{2}{3}{4}",
                    " SELECT CustomerID, CustomerName, CreateDate, ActiveFlag ",
                    " FROM [Customer] ",
                    " WHERE CommunicatorChannelID= ",
                    ChannelList.SelectedItem.Value.ToString(),
                    " AND ActiveFlag = 'Y' and IsDeleted = 0 ORDER BY CustomerName ");

                CustomerID.DataSource = _dataFunctions.GetDataTable(sqlquery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
                CustomerID.DataBind();
            }
            else if (channelName.EndsWith("collector)"))
            {
                var sqlquery = string.Format(
                    "{0}{1}{2}{3}{4}",
                    " SELECT CustomerID, CustomerName, CreateDate, ActiveFlag ",
                    " FROM [Customer] ",
                    " WHERE CollectorChannelID= ",
                    ChannelList.SelectedItem.Value.ToString(),
                    " AND ActiveFlag = 'Y' and IsDeleted = 0 ORDER BY CustomerName ");

                CustomerID.DataSource = _dataFunctions.GetDataTable(sqlquery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
                CustomerID.DataBind();
            }
            else if (channelName.EndsWith("creator)"))
            {
                var sqlquery = string.Format(
                    "{0}{1}{2}{3}{4}",
                    " SELECT CustomerID, CustomerName, CreateDate, ActiveFlag ",
                    " FROM [Customer] ",
                    " WHERE CreatorChannelID= ",
                    ChannelList.SelectedItem.Value.ToString(),
                    " AND ActiveFlag = 'Y' and IsDeleted = 0 ORDER BY CustomerName ");

                CustomerID.DataSource = _dataFunctions.GetDataTable(sqlquery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
                CustomerID.DataBind();
            }
        }

        public void LoadCustomersDropdown(object sender, System.EventArgs e)
        {
            LoadCustomersBasedOnChannelListSelectedItem();

            if (CustomerID.Items.Count > 0)
            {
                CustomerID.Items[0].Selected = true;
                LoadFormData(Convert.ToInt32(CustomerID.SelectedItem.Value));
            }
            else
            {
                LoadFormData(0);
            }
        }
        #endregion

        #region Data Handlers

        public void CreateLicense(object sender, System.EventArgs e)
        {
            string id = CLID.Text;
            string chID = ChannelList.SelectedItem.Value;
            string q = ECN_Framework_DataLayer.DataFunctions.CleanString(Quantity.Text);
            string u = ECN_Framework_DataLayer.DataFunctions.CleanString(Used.Text);
            string c = CustomerID.SelectedItem.Value;
            string ltc = LicenseTypeCode.SelectedItem.Value;

            string sqlquery =
                " INSERT INTO [CustomerLicense] ( " +
                " CustomerID, LicenseTypeCode, Quantity, Used, AddDate, ExpirationDate, CreatedUserID, CreatedDate, IsDeleted " +
                " ) VALUES ( " +
                c + ", '" + ltc + "', '" + q + "', '" + u + "', '" + AddDate.Date.ToString() + "', '" + ExpirationDate.Date.ToString() + "', " + Master.UserSession.CurrentUser.UserID + ", GETDATE(), 0 ) ";
            DataFunctions.Execute(sqlquery);
            Response.Redirect("customerdetail.aspx?CustomerID=" + CustomerID.SelectedItem.Value);
        }

        public void UpdateLicense(object sender, System.EventArgs e)
        {

            string id = CLID.Text;
            string chID = ChannelList.SelectedItem.Value;
            string q = ECN_Framework_DataLayer.DataFunctions.CleanString(Quantity.Text);
            string u = ECN_Framework_DataLayer.DataFunctions.CleanString(Used.Text);
            string ltc = LicenseTypeCode.SelectedItem.Value;

            string sqlquery =
                " UPDATE [CustomerLicense] SET " +
                " Quantity = " + q + ", " +
                " Used='" + u + "', " +
                " LicenseTypeCode='" + ltc + "', " +
                " AddDate='" + AddDate.Date.ToString() + "', " +
                " ExpirationDate='" + ExpirationDate.Date.ToString() + "' " +
                " UpdatedUserID = " + Master.UserSession.CurrentUser.UserID + ", " +
                " UpdatedDate = GETDATE(), " +
                " WHERE CLID=" + id;
            DataFunctions.Execute(sqlquery);
            Response.Redirect("customerdetail.aspx?CustomerID=" + CustomerID.SelectedItem.Value);
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion


    }
}
