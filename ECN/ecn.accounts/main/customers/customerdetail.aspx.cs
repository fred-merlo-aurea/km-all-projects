using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ECNCommon = ECN_Framework_Common.Objects;
using AccountsCommon = ECN_Framework_Common.Objects.Accounts;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using KMPlatformEntity = KMPlatform.Entity;
using KMPlatformBLL = KMPlatform.BusinessLogic;

using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using ApplicationBLL = ECN_Framework_BusinessLayer.Application;
using CommunicatorBLL = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEntity = ECN_Framework_Entities.Communicator;
using ECN_Framework_Common.Objects;
using Telerik.Web.UI;

namespace ecn.accounts.customersmanager
{
    public partial class customerdetail : ApplicationBLL.WebPageHelper
    {

        AccountsEntity.Customer customer = null;
        AccountsEntity.BaseChannel basechannel = null;
        KMPlatform.Entity.Client currentClient = null;

        public int getCustomerID()
        {
            int theCustomerID = 0;
            try
            {
                theCustomerID = Convert.ToInt32(Request.QueryString["CustomerID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCustomerID;
        }

        int getClientID()
        {
            if (getCustomerID() == 0)
                return -1;
            else
            {
                customer = AccountsBLL.Customer.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser, true);
                return customer.PlatformClientID;        
            }
        }

        int getClientGroupID()
        {
            int clientGroupID = 0;
            if (basechannel == null)
            {
                int baseChannelID = 0; Int32.TryParse(ddlBaseChannels.SelectedItem.Value, out baseChannelID);
                if (baseChannelID > 0)
                {
                    basechannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(baseChannelID);
                }
            }
            if (basechannel != null)
            {
                clientGroupID = basechannel.PlatformClientGroupID;
            }
            return clientGroupID;
        }

        public int getCTID()
        {
            int theCTID = 0;
            try
            {
                theCTID = Convert.ToInt32(Request.QueryString["CTID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCTID;
        }

        public int getCPID()
        {
            int theCPID = 0;
            try
            {
                theCPID = Convert.ToInt32(Request.QueryString["CPID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCPID;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = AccountsCommon.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "add customer";
            Master.Heading = "";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icocustomers.gif><b>Unsent Emails</b><br />These are the emails you wrote or started writing but have not sent. You can also edit an email before you send it, Click the edit link. To send the email, first set the groups you want to recieve this Blast. </p>&#13;&#10;&#9;&#9;<p><b>Sent Emails</b><br />These emails are stored in your database and are available to view and/or send again. </p>&#13;&#10;&#9;&#9;<p><b>Helpful Hint</b><br />To send an email again, first 'view' the email and while viewing the email click 'write new email' link in the navigation. All you have to do is select the layout you want, rename it and click the preview email button.</p>&#13;&#10;&#9;&#9;";
            Master.HelpTitle = "Customer Manager";
            lblErrorMessage.Text = "";
            phError.Visible = false;

            //if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            // TODO: is this correct?
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                if (!Page.IsPostBack)
                {
                    grdContacts.PageSize = 10;
                    grdNotes.PageSize = 10;

                    List<AccountsEntity.Code> code = AccountsBLL.Code.GetAll();

                    code = (from c in code
                            where c.CodeType == "CustomerType"
                            select c).ToList();

                    ddlCustomerType.DataSource = code;
                    ddlCustomerType.DataBind();
                    ddlCustomerType.Items.Insert(0, new ListItem("----- Select Type -----", ""));

                    int customerID = getCustomerID();

                    if (customerID > 0)
                    {
                        customer = AccountsBLL.Customer.GetByCustomerID(customerID, Master.UserSession.CurrentUser, true);
                        if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                        {
                            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                            {
                                List<AccountsEntity.Customer> lCust = AccountsBLL.Customer.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value);

                                var custExists = lCust.Where(x => x.CustomerID == customer.CustomerID);

                                if (!custExists.Any())
                                    Response.Redirect("~/main/securityAccessError.aspx");
                            }
                            else
                            {
                                Response.Redirect("~/main/securityAccessError.aspx");
                            }
                        }
                    }

                    if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                    {
                        ddlBaseChannels.Enabled = false;
                    }


                    LoadBaseChannels();
                    LoadMSCustomers();
                    LoadProductOptions();
                    LoadCommunicatorLevelDD();
                    LoadAccountExecutiveandManager();

                    if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                        phAssignment.Visible = true;
                    else
                        phAssignment.Visible = false;

                    if (customerID > 0)
                    {
                        if (Master.UserSession.CurrentUser.IsKMStaff)
                        {
                            hlAddContacts.NavigateUrl = "javascript:void window.open('Contacts.aspx?reload=1&CustomerID=" + customerID + "',null,'height=400,width=600,status=yes,toolbar=no');";
                            hlAddNotes.NavigateUrl = "javascript:void window.open('Notes.aspx?reload=1&CustomerID=" + customerID + "',null,'height=400,width=600,status=yes,toolbar=no');";
                        }
                        else
                        {
                            hlAddContacts.Visible = false;
                            hlAddNotes.Visible = false;
                        }

                        LoadCustomer();

                        btnSave.Text = "Update Customer";
                    }
                    else
                    {
                        hlAddContacts.Visible = false;
                        hlAddNotes.Visible = false;
                    }
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }
        }

        private void LoadBaseChannels()
        {
            ddlBaseChannels.DataSource = AccountsBLL.BaseChannel.GetAll().Select(x => new { x.BaseChannelID, x.BaseChannelName }).OrderBy(x => x.BaseChannelName);
            ddlBaseChannels.DataBind();
            ddlBaseChannels.Items.FindByValue(customer != null ? customer.BaseChannelID.Value.ToString() : Master.UserSession.CurrentCustomer.BaseChannelID.Value.ToString()).Selected = true;
        }

        private void LoadMSCustomers()
        {
            ddlMSCustomer.DataSource = AccountsBLL.Customer.GetByBaseChannelID(Convert.ToInt32(ddlBaseChannels.SelectedValue));
            ddlMSCustomer.DataBind();
            ddlMSCustomer.Items.Insert(0, new ListItem("--- Use Current Customer ---", "0"));
            ddlMSCustomer.Items.FindByValue((customer != null && customer.MSCustomerID != null) ? customer.MSCustomerID.Value.ToString() : "0").Selected = true;
        }

        private void LoadCommunicatorLevelDD()
        {
            //ddlCommunicatorLevelOptions.DataSource = AccountsBLL.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.CustomerSecurity, Master.UserSession.CurrentUser);
            //ddlCommunicatorLevelOptions.DataBind();
        }

        private void LoadAccountExecutiveandManager()
        {

            List<KMPlatform.Entity.User> lu = (List<KMPlatform.Entity.User>)(new KMPlatformBLL.User()).Select(false).Where(x => x.IsKMStaff.Equals(true)).ToList();

            ddlAccountExecutive.DataSource = lu;
            ddlAccountExecutive.DataTextField = "Username";
            ddlAccountExecutive.DataValueField = "UserID";
            ddlAccountExecutive.DataBind();

            ddlAccountExecutive.Items.Insert(0, new ListItem("--- Select Executive ---", "0"));

            ddlAccountManager.DataSource = lu;
            ddlAccountManager.DataTextField = "Username";
            ddlAccountManager.DataValueField = "UserID";
            ddlAccountManager.DataBind();

            ddlAccountManager.Items.Insert(0, new ListItem("--- Select Manager ---", "0"));

        }


        private void LoadProductOptions()
        {
            //basechannel = AccountsBLL.BaseChannel.GetByBaseChannelID(int.Parse(ddlBaseChannels.SelectedItem.Value));

            //cbCommunicator.Enabled = basechannel.AccessCommunicator.Value;
            //cbCollector.Enabled = basechannel.AccessCollector.Value;
            //cbCreator.Enabled = basechannel.AccessCreator.Value;
            //cbPublisher.Enabled = basechannel.AccessPublisher.Value;
            //cbCharity.Enabled = basechannel.AccessCharity.Value;
        }

        public void cbCommunicator_CheckedChanged(object sender, System.EventArgs e)
        {
            //if (cbCommunicator.Checked)
            //{
            //    CommLevelPanel.Visible = true;
            //}
            //else
            //{
            //    CommLevelPanel.Visible = false;
            //}
        }

        public void ddlBaseChannels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadProductOptions();
            LoadMSCustomers();
            LoadtlClientServiceFeatures();
        }

        private void LoadNotes()
        {
            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                List<AccountsEntity.CustomerNote> customernote = AccountsBLL.CustomerNote.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser);
                List<KMPlatformEntity.User> user = KMPlatformBLL.User.GetByCustomerID(getCustomerID());



                //var query = from c in customernote
                //            join u in user on c.UpdatedUserID == null ? (c.CreatedUserID == null ? -1 : c.CreatedUserID) : c.UpdatedUserID equals u.UserID into note_user
                //            from s in note_user.DefaultIfEmpty()
                //            orderby c.UpdatedDate descending
                //            select new
                //            {
                //                c.CustomerID,
                //                c.NoteID,
                //                c.Notes,
                //                c.IsBillingNotes,
                //                UpdatedDate = c.UpdatedDate == null ? c.CreatedDate : c.UpdatedDate,
                //                UserName = c.UpdatedUserID == null ? (c.CreatedUserID == null ? "" : s.UserName) : s.UserName
                //            };

                grdNotes.DataSource = customernote;
                grdNotes.DataBind();
            }
        }

        protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNotes.PageIndex = e.NewPageIndex;
            LoadNotes();
        }

        private void LoadContacts()
        {
            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                grdContacts.DataSource = AccountsBLL.CustomerContact.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser).OrderBy(x => x.FirstName).ToList();
                grdContacts.DataBind();
            }
        }

        protected void grdContacts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdContacts.PageIndex = e.NewPageIndex;
            LoadContacts();
        }

        protected void grdContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.FindControl("hlEdit");

                hl.NavigateUrl = "javascript:void window.open('Contacts.aspx?ID=" + grdContacts.DataKeys[e.Row.RowIndex].Values[0].ToString() + "&CustomerID=" + grdContacts.DataKeys[e.Row.RowIndex].Values[1].ToString() + "',null,'height=400,width=600,status=yes,toolbar=no');";
            }
        }

        private void LoadCustomer()
        {
            hfCustomerPlatformClientID.Value = customer.PlatformClientID.ToString();
            txtCustomerName.Text = customer.CustomerName;
            customer.GeneralContant.Salutation = customer.Salutation;
            customer.GeneralContant.ContactName = customer.ContactName;
            customer.GeneralContant.FirstName = customer.FirstName;
            customer.GeneralContant.LastName = customer.LastName;
            customer.GeneralContant.ContactTitle = customer.ContactTitle;
            customer.GeneralContant.Phone = customer.Phone;
            customer.GeneralContant.State = customer.State;
            customer.GeneralContant.Country = customer.Country;
            customer.GeneralContant.Zip = customer.Zip;
            customer.GeneralContant.Fax = customer.Fax;
            customer.GeneralContant.Email = customer.Email;
            customer.GeneralContant.StreetAddress = customer.Address;
            customer.GeneralContant.City = customer.City;

            GeneralContact.Contact = customer.GeneralContant;
            BillingContact.Contact = customer.BillingContact;
            cbActiveStatus.Checked = customer.ActiveFlag.ToUpper().Equals("Y") ? true : false;
            cbDemoCustomer.Checked = customer.DemoFlag.ToUpper().Equals("Y") ? true : false;
            txtWebAddress.Text = customer.WebAddress;
            txttechContact.Text = customer.TechContact;
            txttechEmail.Text = customer.TechEmail;
            txttechPhone.Text = customer.TechPhone;
            

            string abWinnerType = customer.ABWinnerType;
            if (abWinnerType == null)
            {
                abWinnerType = System.Configuration.ConfigurationManager.AppSettings["KMWinnerTypeDefault"];
                ddlAbWinnerType.SelectedValue = abWinnerType;
            }
            else if (abWinnerType.ToLower() == "opens")
            {
                ddlAbWinnerType.SelectedValue = "opens";
            }
            else if (abWinnerType.ToLower() == "clicks")
            {
                ddlAbWinnerType.SelectedValue = "clicks";
            }

            chkDefaultBlastAsTest.Checked = customer.DefaultBlastAsTest.HasValue ? customer.DefaultBlastAsTest.Value : false;

            txtSubscriptionEmail.Text = customer.SubscriptionsEmail;
            ddlBaseChannels.Items.FindByValue(customer.BaseChannelID.ToString()).Selected = true;
            ddlMSCustomer.Items.FindByValue(customer.MSCustomerID != null ? customer.MSCustomerID.Value.ToString() : "0").Selected = true;
            ddlCustomerType.Items.FindByValue(customer.CustomerType).Selected = true;

            //if (customer.CommunicatorChannelID > 0)
            //{
            //    CommLevelPanel.Visible = true;
            //    cbCommunicator.Checked = true;
            //    ddlCommunicatorLevelOptions.Items.FindByValue(customer.CommunicatorLevel.ToString()).Selected = true;
            //}
            //if (customer.CollectorChannelID > 0)
            //{
            //    cbCollector.Checked = true;
            //}
            //if (customer.CreatorChannelID > 0)
            //{
            //    cbCreator.Checked = true;
            //}
            //if (customer.PublisherChannelID > 0)
            //{
            //    cbPublisher.Checked = true;
            //}
            //if (customer.CharityChannelID > 0)
            //{
            //    cbCharity.Checked = true;
            //}

            ddlAccountExecutive.ClearSelection();
            ddlAccountManager.ClearSelection();
            rblStrategic.ClearSelection();

            if (customer.AccountExecutiveID > 0)
            {
                try
                {
                    ddlAccountExecutive.Items.FindByValue(customer.AccountExecutiveID.ToString()).Selected = true;
                }
                catch
                { }
            }

            if (customer.AccountManagerID > 0)
            {
                try
                {
                    ddlAccountManager.Items.FindByValue(customer.AccountManagerID.ToString()).Selected = true;
                }
                catch
                { }
            }

            try
            {
                rblStrategic.Items.FindByValue(customer.IsStrategic.Value ? "Y" : "N").Selected = true;
            }
            catch
            { }


            // Set the pickup directory if any	

            ECN_Framework_Entities.Accounts.CustomerConfig cpConfig = ECN_Framework_BusinessLayer.Accounts.CustomerConfig.GetByCustomerID(customer.CustomerID, Master.UserSession.CurrentUser).Find(x => x.ProductID == 100 && x.ConfigName == ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.PickupPath.ToString());
            if (cpConfig != null)
            {
                txtPickupPath.Text = cpConfig.ConfigValue;
            }

            ECN_Framework_Entities.Accounts.CustomerConfig cmConfig = ECN_Framework_BusinessLayer.Accounts.CustomerConfig.GetByCustomerID(customer.CustomerID, Master.UserSession.CurrentUser).Find(x => x.ProductID == 100 && x.ConfigName == ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.MailingIP.ToString());
            if (cmConfig != null)
            {
                txtMailingIP.Text = cmConfig.ConfigValue;
            }
            else
            {
                ECN_Framework_Entities.Accounts.Channel channel = ECN_Framework_BusinessLayer.Accounts.Channel.GetAll().Find(x => x.ChannelID == customer.CommunicatorChannelID);
                if (channel != null) txtMailingIP.Text = channel.MailingIP;
            }

            LoadContacts();
            LoadNotes();
            LoadTemplates();
            LoadQuotes();
            LoadLicenses();
            LoadFeatures();
        }

        private void LoadTemplates()
        {
            List<AccountsEntity.CustomerTemplate> customertemplate = AccountsBLL.CustomerTemplate.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser);
            List<AccountsEntity.Code> code = AccountsBLL.Code.GetAll();

            var query = from ct in customertemplate
                        join c in code on ct.TemplateTypeCode equals c.CodeValue
                        orderby ct.TemplateTypeCode, ct.UpdatedDate
                        select new
                        {
                            ct.CTID,
                            c.CodeName,
                            Active = ct.IsActive == true ? 'Y' : 'N',
                            ct.UpdatedDate
                        };

            grdTemplates.DataSource = query.ToList();
            grdTemplates.DataBind();
        }

        public void DeleteTemplate(int theCTID)
        {
            try
            {
                AccountsBLL.CustomerTemplate.Delete(theCTID, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            LoadTemplates();
        }


        protected void grdTemplates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                DeleteTemplate(Convert.ToInt32(e.CommandArgument.ToString()));
            }
        }

        protected void grdTemplates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTemplates.PageIndex = e.NewPageIndex;
            LoadTemplates();
        }

        private void LoadQuotes()
        {
            List<AccountsEntity.Quote> quote = AccountsBLL.Quote.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser, true);

            grdQuote.DataSource = quote;
            grdQuote.DataBind();

            //AccountsEntity.Staff staff = AccountsBLL.Staff.GetStaffByUserID(Master.UserSession.CurrentUser.UserID);
            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                //if (staff.LicenseUpdateFlag)

                grdQuote.Columns[7].Visible = true;
            }
        }

        public void DeleteQuote(int QuoteID)
        {
            try
            {
                AccountsBLL.Quote.Delete(QuoteID, getCustomerID(), Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }
            LoadQuotes();
        }

        protected void grdQuote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex >= 0)
                {
                    GridView grdQuoteItem = (GridView)e.Row.FindControl("grdQuoteItem");

                    int QuoteID = Convert.ToInt32(grdQuote.DataKeys[e.Row.RowIndex].Value);

                    grdQuoteItem.DataSource = AccountsBLL.QuoteItem.GetByQuoteID(QuoteID, Master.UserSession.CurrentUser);
                    grdQuoteItem.DataBind();
                }
            }
        }

        protected void grdQuote_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdQuote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "EDIT":
                    //ECN_Framework_Common.Functions.HttpRequestProcessor processor = new ECN_Framework_Common.Functions.HttpRequestProcessor("../billingSystem/QuoteDetail.aspx");
                    //processor.Add("CustomerID", getCustomerID());
                    //processor.Add("QuoteID", );
                    //Server.Transfer(processor.EncryptedHttpRequest);

                    Response.Redirect(string.Format("../billingSystem/QuoteDetail.aspx?CustomerID={0}&QuoteID={1}", getCustomerID(), Convert.ToInt32(e.CommandArgument.ToString())));

                    break;
                case "DELETE":
                    DeleteQuote(Convert.ToInt32(e.CommandArgument.ToString()));
                    break;
            }

        }

        protected void grdQuote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdQuote.PageIndex = e.NewPageIndex;
            LoadQuotes();
        }

        protected void grdQuoteItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex >= 0)
                {
                    Label lblIsCustomerCredit = e.Row.FindControl("lblIsCustomerCredit") as Label;

                    if (lblIsCustomerCredit.Text == "true")
                    {
                        e.Row.Cells[5].CssClass = "CustomerCreditItem";
                    }
                }
            }
        }

        private void LoadLicenses()
        {
            List<AccountsEntity.CustomerLicense> customerlicence = AccountsBLL.CustomerLicense.GetByCustomerID(getCustomerID(), Master.UserSession.CurrentUser);

            customerlicence = (from cl in customerlicence
                               orderby cl.ExpirationDate, cl.LicenseTypeCode
                               select cl).ToList();

            grdLicense.DataSource = customerlicence;
            grdLicense.DataBind();

            //AccountsEntity.Staff staff = AccountsBLL.Staff.GetStaffByUserID(Master.UserSession.CurrentUser.UserID);
            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                //if (staff.LicenseUpdateFlag)

                grdQuote.Columns[6].Visible = true;
            }
        }

        public void DeleteLicense(int theCLID)
        {
            try
            {
                AccountsBLL.CustomerLicense.Delete(theCLID, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            LoadLicenses();
        }


        protected void grdLicense_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdLicense_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                DeleteLicense(Convert.ToInt32(e.CommandArgument.ToString()));
            }
        }

        private void LoadFeatures()
        {
            //List<ECN_Framework_Entities.Accounts.CustomerProduct> lCustomerProducts = ECN_Framework_BusinessLayer.Accounts.CustomerProduct.GetbyCustomerID(getCustomerID(), true, Master.UserSession.CurrentUser);
            //List<ECN_Framework_Entities.Accounts.ProductDetail> productDetailList = ECN_Framework_BusinessLayer.Accounts.ProductDetail.GetAll();
            //productDetailList = (from src in productDetailList
            //                     where src.ProductID != null
            //                     select src).ToList();

            //var temp = (from cp in lCustomerProducts
            //            join pd in productDetailList on cp.ProductDetailID.Value equals pd.ProductDetailID
            //            select new
            //            {
            //                pd.ProductID,
            //                cp.UpdatedDate,
            //                pd.ProductDetailName,
            //                pd.ProductDetailDesc,
            //                cp.Active,
            //                cp.CustomerProductID
            //            }).ToList();


            //var cproducts = from t in temp
            //                join p in ECN_Framework_BusinessLayer.Accounts.Product.GetAll() on t.ProductID.Value equals p.ProductID
            //                orderby p.ProductName
            //                select new 
            //                { 
            //                    t.UpdatedDate, 
            //                    p.ProductName, 
            //                    t.ProductDetailName, 
            //                    t.ProductDetailDesc, 
            //                    t.Active, 
            //                    t.CustomerProductID,
            //                    p.HasWebsiteTarget,
            //                    t.ProductID
            //                };

            //grdFeatures.DataSource = cproducts.ToList();
            //grdFeatures.DataBind();

            //AccountsEntity.Staff staff = AccountsBLL.Staff.GetStaffByUserID(Master.UserSession.CurrentUser.UserID);
            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                //removed the hard coded values for updating features 11/19/2013 JWelter
                //if (staff.FeatureUpdateFlag) 

                //        grdFeatures.Columns[5].Visible = true;
                //}
            }
        }

        protected void grdFeatures_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "TOGGLE")
            {
                ToggleProduct(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            else if (e.CommandName == "showPopup")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = gvr.RowIndex;
                Label lblProductDetailDesc = (Label)gvr.FindControl("lblProductDetailDesc");
                Label WebsiteTargetLabel = (Label)gvr.FindControl("WebsiteTargetLabel");


                int productID = (Convert.ToInt32(e.CommandArgument.ToString()));
                ViewState["SelectedProductID"] = productID.ToString();
                ViewState["SelectedProductDetailDesc"] = lblProductDetailDesc.Text;
                lblWebsiteTarget.Text = lblProductDetailDesc.Text;
                txtWebsiteAddress.Text = WebsiteTargetLabel.Text;
                mdlWebsiteTarget.Show();
            }
        }

        public void ToggleProduct(int CPID) { }        

        private string GetMTAEmail_Content(string customerName, int CustomerID, string channelName, int ChannelID)
        {
            StringBuilder sbMTA = new StringBuilder();
            sbMTA.Append("<HTML><HEAD></HEAD><BODY><TABLE>");
            sbMTA.Append("<TR><TD><p>BaseChannel ID: " + ChannelID.ToString() + "</p></TD></TR>");
            sbMTA.Append("<TR><TD><p>BaseChannel Name: " + channelName + "</p></TD></TR>");
            sbMTA.Append("<TR><TD><p>Customer ID: " + CustomerID.ToString() + "</p></TD></TR>");
            sbMTA.Append("<TR><TD><p>Customer Name: " + customerName + "</p></TD></TR>");
            sbMTA.Append("</TABLE></BODY></HTML>");
            return sbMTA.ToString();
        }

        protected void lbtnCopy_Click(object sender, System.EventArgs e)
        {
            BillingContact.Contact = GeneralContact.Contact;
        }

        protected void grdFeatures_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // You could just do yourGrid and ignore casting the sender but this 
            // makes the code generic for reuse.
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            LoadFeatures(); // need to rebind once the edit index is set.
        }

        protected void btnSaveWebsiteTarget_Click(object sender, EventArgs e)
        {
            int productID = Convert.ToInt32(ViewState["SelectedProductID"] ?? 0);
            string websiteUrl = txtWebsiteAddress.Text;

            if (!string.IsNullOrEmpty(websiteUrl))
            {
                //get the text of the product
                string productDescription = ViewState["SelectedProductDetailDesc"] != null ? ViewState["SelectedProductDetailDesc"].ToString() : string.Empty;

                //get the code by product/current user
                CommunicatorEntity.Code code = CommunicatorBLL.Code.GetByCustomerAndCategory(productDescription, getCustomerID(), Master.UserSession.CurrentUser).FirstOrDefault();

                //if null, create new
                if (code == null)
                {
                    code = new CommunicatorEntity.Code()
                    {
                        CustomerID = getCustomerID(),
                        CodeType = productDescription,
                        SortOrder = 0,
                        DisplayFlag = "Y",
                        CreatedUserID = Master.UserSession.CurrentUser.UserID,
                        IsDeleted = false
                    };
                }

                //populate with text
                code.CodeValue = websiteUrl;
                code.CodeDisplay = websiteUrl;
                code.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

                //save
                try
                {
                    CommunicatorBLL.Code.Save(code, Master.UserSession.CurrentUser);
                    LoadFeatures();
                }
                catch (ECNException ex)
                {
                    StringBuilder builder = new StringBuilder(256);

                    foreach (ECNError error in ex.ErrorList)
                    {
                        builder.AppendLine(error.ErrorMessage);
                    }

                    WebsiteTargetErrorText.Text = builder.ToString();
                    WebsiteAddressErrorExtender.Show();
                }

                mdlWebsiteTarget.Hide();
            }
        }

        protected void btnCloseWebsiteTarget_Click(object sender, EventArgs e)
        {
            mdlWebsiteTarget.Hide();
        }


        protected void grdFeatures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdFeatures = sender as GridView;

                if ((bool)grdFeatures.DataKeys[e.Row.RowIndex].Values["HasWebsiteTarget"])
                {
                    Label websiteTargetLabel = e.Row.FindControl("WebsiteTargetLabel") as Label;
                    //TextBox websiteTargetTextBox = e.Row.FindControl("txtWebsiteAddress") as TextBox;
                    string type = ((Label)e.Row.Cells[2].FindControl("lblProductDetailDesc")).Text;

                    CommunicatorEntity.Code code = CommunicatorBLL.Code.GetByCustomerAndCategory(type, getCustomerID(), Master.UserSession.CurrentUser).FirstOrDefault();

                    if (code != null)
                    {
                        websiteTargetLabel.Text = code.CodeValue;
                        //websiteTargetTextBox.Text = code.CodeValue;
                    }
                    else
                    {
                        websiteTargetLabel.Text = "[no target defined]";
                    }
                }
            }
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            WebsiteAddressErrorExtender.Hide();
        }

        protected void tlClientServiceFeatures_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var dataItem = e.Item as TreeListDataItem;
                Control expandButton = e.Item.FindControl("ExpandCollapseButton");

                // prevent collapse
                if (expandButton != null)
                {
                    expandButton.Visible = false;
                }

                if (dataItem.DataItem is KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow)
                {
                    // hide features that don't have an additional cost, these are always enabled
                    var data = dataItem.DataItem as KMPlatform.Entity.ServiceFeature.ClientGroupTreeListRow;
                    if (data.ServiceFeatureID > 0 && false == data.IsAdditionalCost)
                    {
                        dataItem.Visible = false;
                    }
                    // set selected based on the IsEnabled flag
                    if (data.IsEnabled)
                    {
                        dataItem.Selected = true;
                    }

                    // fully expand
                    if (dataItem.CanExpand)
                    {
                        dataItem.Expanded = true;
                    }
                }
            }
            else if (e.Item is TreeListHeaderItem)
            {
                (e.Item as TreeListHeaderItem)["SelectColumn"].Controls.Add(new Label() { ID = "tlhSelect", Text = "Select" });
            }
        }

        protected void tlClientServiceFeatures_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            int clientID = getClientID();
            int clientGroupID = getClientGroupID();
            if (clientGroupID == 0)
            {
                return;
            }

            tlClientServiceFeatures.DataSource = new KMPlatform.BusinessLogic.ServiceFeature().GetClientTreeList(clientGroupID, clientID, true);
        }

        private void LoadtlClientServiceFeatures()
        {
            int clientID = getClientID();
            int clientGroupID = getClientGroupID();
            if (clientGroupID == 0)
            {
                return;
            }

            tlClientServiceFeatures.ClearSelectedItems();

            tlClientServiceFeatures.DataSource = new KMPlatform.BusinessLogic.ServiceFeature().GetClientTreeList(clientGroupID, clientID, true);
            tlClientServiceFeatures.DataBind();
        }


        protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccountsEntity.CustomerNote cn = (AccountsEntity.CustomerNote)e.Row.DataItem;
                if (cn != null)
                {
                    int userID = cn.UpdatedUserID == null ? cn.CreatedUserID == null ? -1 : cn.CreatedUserID.Value : cn.UpdatedUserID.Value;
                    if (userID > 0)
                    {
                        KMPlatformEntity.User noteUser = KMPlatformBLL.User.GetByUserID(cn.UpdatedUserID == null ? cn.CreatedUserID.Value : cn.UpdatedUserID.Value, false);
                        Label lblNoteUserName = (Label)e.Row.FindControl("lblNoteUserName");
                        Label lblNoteUpdated = (Label)e.Row.FindControl("lblNoteUpdatedDate");
                        lblNoteUserName.Text = noteUser.UserName;
                        lblNoteUpdated.Text = cn.UpdatedDate == null ? cn.CreatedDate.Value.ToShortDateString() : cn.UpdatedDate.Value.ToShortDateString();
                    }
                }
            }
        }
    }
}
