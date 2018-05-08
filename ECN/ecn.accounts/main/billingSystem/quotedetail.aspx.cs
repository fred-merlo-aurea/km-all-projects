using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Net.Mail;
using System.Web.SessionState;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Collections.Generic;
using ecn.accounts.includes;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.common.classes.license;
using ecn.accounts.classes.PayFlowPro;
using ecn.communicator.classes;
using System.Configuration;

using CommonFunctions = ECN_Framework_Common.Functions;
using AccountEntity = ECN_Framework_Entities.Accounts;
using AccountBLL = ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Accounts.View;
using ECN_Framework_Common.Objects;
using AccountFramework = ECN_Framework_Entities.Accounts;

namespace ecn.accounts.main.billingSystem
{
    public partial class quotedetail : ECN_Framework.WebPageHelper
    {
        protected System.Web.UI.WebControls.Label lblSavedOneTimeFees;
        protected System.Web.UI.WebControls.Label lblSavedMonthlyFees;
        protected System.Web.UI.WebControls.Label lblSavedQuarterlyFees;
        protected System.Web.UI.WebControls.Label lblSavedAnnualFees;

        //ECN_Framework_Entities.Accounts.Quote quote = null;

        public Quote CurrentQuote
        {
            get
            {
                if (Session[QuoteKey] == null)
                {
                    Customer c = new Customer(Convert.ToInt32(ddlCustomers.SelectedValue), Convert.ToInt32(ddlChannels.SelectedValue));
                    Quote q = new Quote(c);
                    Session[QuoteKey] = q;
                }

                return (Quote)Session[QuoteKey];
            }
            set
            {
                Session[QuoteKey] = value;
            }
        }

        string QuoteKey
        {
            get { return "QuoteKey"; }
        }


        #region Event Handler
        protected void Page_Load(object sender, System.EventArgs e)
        {
            clearECNError(phError, lblErrorMessageNew);
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM; 

            if (!IsPostBack)
            {
                if (!Master.UserSession.CurrentUser.IsKMStaff)
                {
                    Response.Redirect("../securityAccessError.aspx");
                }

                Session[QuoteKey] = null;
                int quoteID = Convert.ToInt32(Request["QuoteID"]);;//Convert.ToInt32(ECN_Framework_Common.Functions.HttpRequestProcessor.Decrypt(Request["QuoteID"]));
                int customerID = Convert.ToInt32(Request["CustomerID"]); ;//Convert.ToInt32(ECN_Framework_Common.Functions.HttpRequestProcessor.Decrypt(Request["CustomerID"]));
                LoadChannelAndCustomer();

                if (quoteID > 0)
                {
                    // quote =  ECN_Framework_BusinessLayer.Accounts.Quote.GetByQuoteID(quoteID, Master.UserSession.CurrentUser, false);
                    CurrentQuote = Quote.GetQuoteByID(quoteID);
                    CurrentQuote.AddItems(QuoteItem.GetQuoteItemsByQuoteID(quoteID));

                    ddlChannels.SelectedIndex = ddlChannels.Items.IndexOf(ddlChannels.Items.FindByValue(CurrentQuote.ChannelID.ToString()));
                    ddlChannels_SelectedIndexChanged(null, null);

                    if (customerID > 0)
                    {
                        Customer customer = Customer.GetCustomerByID(customerID);
                        ddlCustomers.SelectedIndex = ddlCustomers.Items.IndexOf(ddlCustomers.Items.FindByValue(customer.ID.ToString()));
                        CurrentQuote.Customer = customer;
                    }

                    if (!CurrentQuote.Customer.IsNew)
                    {
                        ddlChannels.Enabled = false;
                        ddlCustomers.Enabled = false;
                    }
                    UpdateOtherQuoteInfo();

                    lblQCreatedBy.Text = Master.UserSession.CurrentUser.EmailAddress;
                }
                else
                {
                    LoadInfoFromLeads();

                    ddlChannels.SelectedIndex = ddlChannels.Items.IndexOf(ddlChannels.Items.FindByValue("12"));
                    ddlChannels_SelectedIndexChanged(null, null);

                    if (Master.UserSession.CurrentUser.IsKMStaff)
                        lblQCreatedBy.Text = Master.UserSession.CurrentUser.EmailAddress; 
                }

                //LoadAccountExecutive(quoteID);
                //LoadAccountManager(quoteID);



                ucAnnualTechEditor.QuoteOptions = QuoteOption.GetServiceLevelQuoteOptions(1);
                    
                    //ECN_Framework_BusinessLayer.Accounts.QuoteOption.GetByLicenseType(1, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum.AnnualTechAccess, Master.UserSession.CurrentUser);
                    
                    
                QuoteItemCollection items = CurrentQuote.GetQuoteItemsByType(LicenseTypeEnum.AnnualTechAccess);
                if (items.Count > 0)
                {
                    ucAnnualTechEditor.AnnualQuoteItem = items[0];
                }
                else
                {
                    ucAnnualTechEditor.AnnualQuoteItem = null;
                }

                ucEmailOptionEditor.QuoteOptions = QuoteOption.GetEmailUsageQuoteOptions(1);
                ucEmailOptionEditor.QuoteItems = CurrentQuote.GetQuoteItemsByType(LicenseTypeEnum.EmailBlock);


                ucOptionEditor.QuoteOptions = QuoteOption.GetOptionQuoteOptions(1);
                ucOptionEditor.QuoteItems = CurrentQuote.GetQuoteItemsByType(LicenseTypeEnum.Option);
            }

            ucAnnualTechEditor.OnAnnualItemChanged += new EventHandler(OnOptionItemAdded);
            ucEmailOptionEditor.OnQuoteItemAdded += new EventHandler(OnOptionItemAdded);
            ucOptionEditor.OnQuoteItemAdded += new EventHandler(OnOptionItemAdded);
            UpdateUI();
        }


        protected void ddlChannels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlCustomers.DataSource = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(ddlChannels.SelectedValue)).OrderBy(x => x.CustomerName).ToList();
            ddlCustomers.DataTextField = "CustomerName";
            ddlCustomers.DataValueField = "CustomerID";
            ddlCustomers.DataBind();
            ddlCustomers.Items.Insert(0, new ListItem("New customer", "-1"));
            ResetCustomer();
            UpdateUI();
        }


        protected void ddlCustomers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ResetCustomer();
            if (CurrentQuote.Customer.IsNew)
            {
                txtCompanyName.Text = "";
                txtPhone.Text = "";
                txtFax.Text = "";
                txtEmail.Text = "";
            }
            else
            {
                txtCompanyName.Text = CurrentQuote.Customer.Name;
                txtPhone.Text = CurrentQuote.Customer.GeneralContact.Phone;
                txtFax.Text = CurrentQuote.Customer.GeneralContact.Fax;
                txtEmail.Text = CurrentQuote.Customer.GeneralContact.Email;
            }
            UpdateUI();
        }


        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            
            if (ddlCustomers.SelectedValue.ToString().Equals("-1") && (new KMPlatform.BusinessLogic.Client()).Exists(txtCompanyName.Text.Trim()))
            {
                lblErrorMessage.Text = string.Format("'{0}' is used. Please choose another customer name.", txtCompanyName.Text);
                lblErrorMessage.Visible = true;
                return;
            }
            else
            {
                lblErrorMessage.Visible = false;
            }

            if (CurrentQuote.IsNew)
            {
                CurrentQuote.CreatedDate = CurrentQuote.UpdatedDate = DateTime.Now;
                CurrentQuote.ChannelID = Convert.ToInt32(ddlChannels.SelectedValue);
                 
                //Need to create Client object here
            }
            else
            {
                CurrentQuote.UpdatedDate = DateTime.Now;
            }

            CurrentQuote.Salutation = "Mr.";
            CurrentQuote.StartDate = DateTime.Now.AddDays(30);
            CurrentQuote.FirstName = txtFirstName.Text;
            CurrentQuote.LastName = txtLastName.Text;
            CurrentQuote.Email = txtEmail.Text;
            CurrentQuote.Phone = txtPhone.Text;
            CurrentQuote.Fax = txtFax.Text;
            CurrentQuote.Company = txtCompanyName.Text;
            CurrentQuote.BillType = ddlBillType.SelectedValue;
            CurrentQuote.Notes = ucEmailNotes.Notes;
            CurrentQuote.InternalNotes = ucEmailNotes.InternalNotes;
            GetQuoteItems(CurrentQuote);
            CurrentQuote.CreatedUserID = Master.UserSession.CurrentUser.UserID;// Staff.CurrentStaff.ID;
            //CurrentQuote.AccountManagerID= Convert.ToInt32(ddlAccountManager.SelectedItem.Value); 

            
           
            
            try
            {
                // Don't change the order of the following three methods.
                //CreateTestAccount();
                CurrentQuote.Save();
                //CreateTestingLicense();
            }
            catch (InvalidOperationException IOE)
            {
                throwECNException(IOE.Message, phError, lblErrorMessageNew);
            }

            UpdateQuoteIDAndStatus();
            //SendLoginInfo(CurrentQuote);
            SendEmail();
            UpdateRcurringBillingProfiles();

            string notifyBillingEmailBody = CreateNotifyBillingDeptEmail(CurrentQuote);
            ecn.accounts.classes.NotifyBillingDept.EmailBody = notifyBillingEmailBody;
            ecn.accounts.classes.NotifyBillingDept.EmailSubject = CurrentQuote.QuoteCode + " - New Quote to " + CurrentQuote.Company + "(" + CurrentQuote.Email + ") by " + Master.UserSession.CurrentUser.EmailAddress;
            ecn.accounts.classes.NotifyBillingDept.notifyBillingDept();

            Response.Redirect("default.aspx", true);
        }

        

        private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
	        ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
	        List<ECNError> errorList = new List<ECNError> { ecnError };
	        setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void setECNError(ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
	        phError.Visible = true;
	        lblErrorMessage.Text = "";
	        foreach (ECNError ecnError in ecnException.ErrorList)
	        {
		        lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
	        }
        }

        private void clearECNError(PlaceHolder phError, Label lblErrorMessage)
        {
	        phError.Visible = false;
	        lblErrorMessage.Text = string.Empty;
        }

        private string CreateNotifyBillingDeptEmail(Quote CurrentQuote)
        {
            string body = "";
            

            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            body += "<table><tr>";
            body += "<td>New Quote has been created by <i>" + es.CurrentUser.EmailAddress + "</i> for <i>" + CurrentQuote.Company + "</i> under <i>" + ddlChannels.SelectedItem + "</i> Channel.</td></tr>";
            body += "<tr><td><BR><b><u>Customer Contact Details:</u></b></td></tr>";
            body += "<tr><td><p>First Name: " + CurrentQuote.FirstName + "<br>";
            body += "Last Name: " + CurrentQuote.LastName + "<br>";
            body += "Email: " + CurrentQuote.Email + "<br>";
            body += "Phone: " + CurrentQuote.Phone + "<br>";
            body += "Fax: " + CurrentQuote.Fax + "<br><br>";
            body += "<b>Public Notes:</b> " + CurrentQuote.Notes + "<br><br>";
            body += "<b>Internal Notes:</b> " + CurrentQuote.InternalNotes + "<br><br>";
            body += "Billing Method: " + CurrentQuote.BillType + "<br>";
            body += " </p></td></tr>";
            body += "<tr><td>&nbsp;</td></tr>";

            /*body += "<tr><td><b><u>User Information</u></b></td></tr>";
            body += "<tr><td></td></tr>";*/

            HttpRequestProcessor processor = new HttpRequestProcessor(string.Format("{0}/ecn.accounts/Engines/QuoteApproval.aspx", ConfigurationManager.AppSettings["Accounts_DomainPath"].ToString()));
            processor.Add("BaseChannelID", CurrentQuote.Customer.BaseChannelID);
            processor.Add("QuoteID", CurrentQuote.QuoteID);
            body += "<tr><td><b><u>Quote Information</u></b></td></tr>";
            body += "<tr><td><a href='" + processor.EncryptedHttpRequest + "'>View Quote here</a></td></tr>";
            body += "<tr><td><hr size=1>";
            body += "This is an automated message from ECN5 Billing System. Please do not reply to this message</td></tr>";

            /*String licenseSqlquery=
                " SELECT LicenseTypeCode, ExpirationDate, Quantity"+
                " FROM CustomerLicense "+
                " WHERE CustomerID= "+ CurrentQuote.Customer.ID + 
                " ORDER BY LicenseTypeCode,ExpirationDate ";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(licenseSqlquery);
            string licenceData = "<table><tr><td>LicenseTypeCode</td><td>ExpirationDate</td><td>Quantity</td></tr>";
            foreach(DataRow dr in dt.Rows){
                licenceData += "<tr><td>"+dr["LicenseTypeCode"]+"</td><td>"+dr["ExpirationDate"]+"</td><td>"+dr["Quantity"]+"</td></tr>";
            }
            licenceData +="</table>";
            body += "<tr><td><b><u>License Information</u></b></td></tr>";
            body += "<tr><td>"+licenceData+"</td></tr>";
            body += "<tr><td></td></tr>";

            String ecnFeaturesSqlquery= "SELECT pd.ProductDetailName, pd.ProductDetailDesc, cp.Active,cp.ModifyDate "+
                " FROM CustomerProduct cp join ProductDetails pd on cp.ProductDetailID = pd.ProductDetailID join Products p on pd.ProductID = p.ProductID"+
                " WHERE cp.CustomerID= "+CurrentQuote.Customer.ID +
                " ORDER BY ProductName ";
            DataTable dt2 = ECN_Framework_DataLayer.DataFunctions.GetDataTable(ecnFeaturesSqlquery);
            string featuresData = "<table><tr><td>ProductDetailName</td><td>ProductDetailDesc</td><td>Active Y/N</td><td>ModifyDate</td></tr>";
            foreach(DataRow dr in dt2.Rows){
                featuresData += "<tr><td>"+dr["ProductDetailName"]+"</td><td>"+dr["ProductDetailDesc"]+"</td><td>"+dr["Active"]+"</td><td>"+dr["ModifyDate"]+"</td></tr>";
            }
            body += "<tr><td><b><u>ECN Features</u></b></td></tr>";
            body += "<tr><td>"+featuresData+"</td></tr>";
            body += "<tr><td></td></tr>";*/
            body += "</table>";

            return body;
        }


        //protected void chkSendEmail_CheckedChanged(object sender, System.EventArgs e) {
        //    lblPreview.Visible = txtEmailPreview.Visible = chkSendEmail.Checked;
        //    txtEmailPreview.Text = string.Empty;
        //    if (chkSendEmail.Checked) {
        //        txtEmailPreview.Text = GetEmailPreview();
        //    }
        //}

        private void LoadChannelAndCustomer()
        {
            ddlChannels.DataSource = BaseChannel.GetBaseChannels();
            ddlChannels.DataTextField = "Name";
            ddlChannels.DataValueField = "ID";
            ddlChannels.DataBind();
            ddlChannels.Items.Insert(0, new ListItem("-- Select Channel Partner --", "-1"));

            if (ddlChannels.Items.Count > 0)
            {
                ddlChannels.SelectedIndex = 0;
                ddlChannels_SelectedIndexChanged(null, null);
            }
        }


        //private void LoadAccountExecutive(int quoteID)
        //{
        //    ArrayList staff = Staff.GetStaffByRole(StaffRoleEnum.AccountExecutive);

        //    ddlAccountExecutive.DataSource = staff;
        //    ddlAccountExecutive.DataTextField = "FullName";
        //    ddlAccountExecutive.DataValueField = "ID";
        //    ddlAccountExecutive.DataBind();

        //    ddlAccountExecutive.Items.Insert(0, new ListItem("Select a Account Executive", "0"));

        //    if (CurrentQuote.AccountExecutiveID > 0)
        //    {
        //        try
        //        {
        //            ddlAccountExecutive.Items.FindByValue(CurrentQuote.AccountExecutiveID.ToString()).Selected = true;
        //        }
        //        catch { }
        //    }
        //    //
        //    //			if (quoteID <= 0) {
        //    //				ddlAccountExecutive.SelectedIndex = ddlAccountExecutive.Items.IndexOf(ddlAccountExecutive.Items.FindByValue(Staff.CurrentStaff.ID.ToString()));
        //    //			}
        //}

        //private void LoadAccountManager(int quoteID)
        //{
        //    ArrayList staff = Staff.GetStaffByRole(StaffRoleEnum.AccountManager);
        //    ddlAccountManager.DataSource = staff;
        //    ddlAccountManager.DataTextField = "FullName";
        //    ddlAccountManager.DataValueField = "ID";
        //    ddlAccountManager.DataBind();

        //    ddlAccountManager.Items.Insert(0, new ListItem("Select a Account Manager", "0"));

        //    if (CurrentQuote.AccountManagerID > 0)
        //    {
        //        try
        //        {
        //            ddlAccountManager.Items.FindByValue(CurrentQuote.AccountManagerID.ToString()).Selected = true;
        //        }
        //        catch { }
        //    }
        //}

        private void ResetCustomer()
        {
            Customer c = ddlCustomers.SelectedValue == "-1" ? new Customer(-1, Convert.ToInt32(ddlChannels.SelectedValue)) : Customer.GetCustomerByID(Convert.ToInt32(ddlCustomers.SelectedValue));
            c.AddQuotes(Quote.GetQuotesByCustomerID(Convert.ToInt32(ddlCustomers.SelectedValue)));

            // Load quote items for all quotes.
            foreach (Quote q in c.Quotes)
            {
                q.AddItems(QuoteItem.GetQuoteItemsByQuoteID(q.QuoteID));
            }

            CurrentQuote.Customer = c;
            ucAnnualTechEditor.Visible = !CurrentQuote.Customer.HasOtherAnnualTechItem(CurrentQuote.QuoteID);
        }

        //private void CreateTestAccount()
        //{
        //    if (!chkTestAccount.Checked)
        //    {
        //        return;
        //    }
        //    var master = (this.Page.Master as ecn.accounts.MasterPages.Accounts);
        //    CurrentQuote.Customer.Name = txtCompanyName.Text;
        //    CurrentQuote.Customer.BaseChannelID = Convert.ToInt32(ddlChannels.SelectedValue);
        //    Contact contact = new Contact("Mr.", string.Format("{0} {1}", txtFirstName.Text, txtLastName.Text), "N/A", txtPhone.Text, txtFax.Text, txtEmail.Text, "N/A", "N/A", "N/A", "N/A", "N/A");

        //    CurrentQuote.Customer.CreatedDate = DateTime.Now;
        //    CurrentQuote.Customer.IsActive = "Y";
        //    CurrentQuote.Customer.IsDemo = "Y";
        //    CurrentQuote.Customer.GeneralContact = contact;
        //    CurrentQuote.Customer.BillingContact = contact;
        //    CurrentQuote.Customer.WebAddress = "N/A";
        //    CurrentQuote.Customer.TechContact = "N/A";
        //    CurrentQuote.Customer.TechPhone = "N/A";
        //    CurrentQuote.Customer.TechEmail = "N/A";
        //    CurrentQuote.Customer.SubscriptionsEmail = "subscriptions@bounce2.com";
        //    CurrentQuote.Customer.IsStrategic = "N";
        //    //CurrentQuote.Customer.AccountExecutiveID = Convert.ToInt32(ddlAccountExecutive.SelectedItem.Value);
        //    //CurrentQuote.Customer.AccountManagerID = Convert.ToInt32(ddlAccountManager.SelectedItem.Value);
        //    CurrentQuote.Customer.Save(master.UserSession.CurrentUser.UserID);
        //    CurrentQuote.Customer.CreateAssertPaths(Server);
        //    //CurrentQuote.Customer.CreateDefaultFeatures(master.UserSession.CurrentUser.UserID);
        //    //CurrentQuote.Customer.CreateDefaulRole(master.UserSession.CurrentUser.UserID);
        //    CurrentQuote.Customer.CreateMasterSupressionGroup();
        //    User user = new User(txtEmail.Text, HttpRequestProcessor.GetRandomString(8), CurrentQuote.Customer);
        //    user.RoleID = user.GetEverythingRoleID();
        //    user.AccountsOptions = "001100";
        //    user.Save();

        //    CurrentQuote.TestUserName = user.UserName;
        //    CurrentQuote.TestPassword = user.Password;

        //    CurrentQuote.AddItem(new QuoteItem(FrequencyEnum.BiWeekly, "TestingEMB", "200 Testing Email", "Email block for testing account", 200, 0, LicenseTypeEnum.EmailBlock, PriceTypeEnum.OneTime, true));
        //}

        //private void CreateTestingLicense()
        //{
        //    if (!chkTestAccount.Checked)
        //    {
        //        return;
        //    }

        //    Quote tempQuote = CloneQuoteWithoutEmailUsageItem();
        //    Bill bill = CreateBill(tempQuote);
        //    CreateLicenses(bill);
        //}

        private Quote CloneQuoteWithoutEmailUsageItem()
        {
            Quote quote = new Quote(CurrentQuote.Customer);
            quote.StartDate = DateTime.Now;
            quote.Status = QuoteStatusEnum.Approved;
            foreach (QuoteItem item in CurrentQuote.Items)
            {
                if (item.Code == "TestingEMB" || item.LicenseType != LicenseTypeEnum.EmailBlock)
                {
                    quote.AddItem(item);
                }
            }
            return quote;
        }

        private Bill CreateBill(Quote quote)
        {
            BillingEngine billingHistoryManager = new BillingEngine(DateTime.Now, 10);
            Bill bill = quote.CreateBill(billingHistoryManager, DateTime.Now);
            return bill;
        }

        private void CreateLicenses(Bill bill)
        {
            foreach (BillItem item in bill.CurrentBillItems)
            {
                ArrayList licenses = item.CreateLicenses();
                foreach (LicenseBase license in licenses)
                {
                    license.Enable();
                }
            }
        }

        private void SendEmail()
        {
            //if (chkSendEmail.Checked) {

            MailMessage message = new MailMessage();

            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_FROM_EMAIL"]);
            message.To.Add(CurrentQuote.Email);

            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            message.Subject = System.Configuration.ConfigurationManager.AppSettings["ApproveQuoteEmailSubjectTemplate"];
            HttpRequestProcessor processor = new HttpRequestProcessor(string.Format("{0}/ecn.accounts/Engines/QuoteApproval.aspx", ConfigurationManager.AppSettings["Accounts_DomainPath"].ToString()));
            processor.Add("BaseChannelID", CurrentQuote.Customer.BaseChannelID);
            processor.Add("QuoteID", CurrentQuote.QuoteID);

            string msgBody = string.Format(System.Configuration.ConfigurationManager.AppSettings["ApproveQuoteEmailBodyTemplate"], CurrentQuote.Salutation, CurrentQuote.FirstName, CurrentQuote.LastName, "<a href='" + processor.EncryptedHttpRequest + "'>View Quote here</a>", ucEmailNotes.Notes);
            msgBody = msgBody.Replace("\r\n", "<br>");

            message.Body = msgBody;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
            //}
        }

        private string GetEmailPreview()
        {

            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            HttpRequestProcessor processor = new HttpRequestProcessor(string.Format("http://{0}/ecn.accounts/Engines/QuoteApproval.aspx", es.CurrentBaseChannel.ChannelURL));
            processor.Add("BaseChannelID", CurrentQuote.Customer.BaseChannelID);
            processor.Add("QuoteID", CurrentQuote.QuoteID);

            StringBuilder email = new StringBuilder();
            // Email Server
            email.Append(string.Format("Email Server: {0}", System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]));
            email.Append(Environment.NewLine);
            /// To:
            email.Append(string.Format("To: {0}", CurrentQuote.Email));
            email.Append(Environment.NewLine);


            // Subject:
            email.Append(string.Format("Subject: {0}", System.Configuration.ConfigurationManager.AppSettings["ApproveQuoteEmailSubjectTemplate"]));
            email.Append(Environment.NewLine);
            email.Append(Environment.NewLine);

            // Body:
            email.Append(string.Format(System.Configuration.ConfigurationManager.AppSettings["ApproveQuoteEmailBodyTemplate"], CurrentQuote.Salutation, txtFirstName.Text, txtLastName.Text, processor.EncryptedHttpRequest, ucEmailNotes.Notes, Master.UserSession.CurrentUser.EmailAddress));
            email.Append(Environment.NewLine);
            email.Append("===== End of message =====");
            return email.ToString();
        }

//        private void SendLoginInfo(Quote quote)
//        {
//            if (!chkTestAccount.Checked)
//            {
//                return;
//            }
//            MailMessage message = new MailMessage();

//            message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_FROM_EMAIL"]);
//            message.To.Add(quote.Email);

//            message.Subject = "Testing account login Info";
//            message.Body = string.Format(@"{0},
//
//Here is the login information to access your testing account:
//URL: http://www.ecn5.com
//User name: {1}
//Password: {2}
//
//NOTE: This is an automated message from ECN5 Billing System. Please do not reply to this message.
//
//Best regards,
//
//Knowledge Marketing Team", quote.FirstName, quote.TestUserName, quote.TestPassword);


//            message.Priority = MailPriority.High;

//            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
//            smtp.Send(message);

//        }

        private void GetQuoteItems(Quote quote)
        {
            quote.Items.Clear();
            quote.AddItem(ucAnnualTechEditor.AnnualQuoteItem);
            quote.AddItems(ucEmailOptionEditor.QuoteItems);
            quote.AddItems(ucOptionEditor.QuoteItems);
        }

        private void UpdateRcurringBillingProfiles()
        {
            if (CurrentQuote.IsNew)
            {
                return;
            }

            foreach (QuoteItem item in CurrentQuote.Items)
            {
                if (item.IsActive)
                {
                    continue;
                }
                if (item.RecurringProfileID == null || item.RecurringProfileID == string.Empty)
                {
                    continue;
                }

                PayFlowProCreditCardProcessor.Instance.CancelProfileByID(item.RecurringProfileID);
            }
        }


        #endregion


        private void UpdateUI()
        {
            UpdateQuoteIDAndStatus();
            //chkTestAccount.Visible = ddlCustomers.SelectedIndex == 0;
            CheckSubmitButton();

            txtCompanyName.Enabled = CurrentQuote.Customer.IsNew;

            bool hasSaving = CurrentQuote.OneTimeTotalSaving > 0 || CurrentQuote.MonthTotalSaving > 0 || CurrentQuote.QuarterTotalSaving > 0 || CurrentQuote.AnnualTotalSaving > 0;

            double oneTimeTotal = CurrentQuote.OneTimeTotal;
            double monthlyTotal = CurrentQuote.MonthTotal;
            double quarterlyTotal = CurrentQuote.QuarterTotal;
            double annualTotal = CurrentQuote.AnnualTotal;

            if (hasSaving)
            {
                oneTimeTotal += CurrentQuote.OneTimeTotalSaving;
                monthlyTotal += CurrentQuote.MonthTotalSaving;
                quarterlyTotal += CurrentQuote.QuarterTotalSaving;
                annualTotal += CurrentQuote.AnnualTotalSaving;
            }

            /// display total
            lblOneTimeFees.Text = oneTimeTotal.ToString("$###,##0.00");
            lblMonthlyFees.Text = monthlyTotal.ToString("$###,##0.00");
            lblQuarterlyFees.Text = quarterlyTotal.ToString("$###,##0.00");
            lblAnnualFees.Text = annualTotal.ToString("$###,##0.00");

            lblDiscount.Visible = lblNetAmount.Visible = hasSaving;
            /// Show discount:
            lblOneTimeSaving.Text = (!hasSaving) ? " " : CurrentQuote.OneTimeTotalSaving.ToString("($###,##0.00)");
            lblMonthlySaving.Text = (!hasSaving) ? " " : CurrentQuote.MonthTotalSaving.ToString("($###,##0.00)");
            lblQuarterlySaving.Text = (!hasSaving) ? " " : CurrentQuote.QuarterTotalSaving.ToString("($###,##0.00)");
            lblAnnualSaving.Text = (!hasSaving) ? " " : CurrentQuote.AnnualTotalSaving.ToString("($###,##0.00)");

            /// Show Net Amount:
            lblOneTimeNetAmount.Text = (!hasSaving) ? " " : CurrentQuote.OneTimeTotal.ToString("$###,##0.00");
            lblMonthlyNetAmount.Text = (!hasSaving) ? " " : CurrentQuote.MonthTotal.ToString("$###,##0.00");
            lblQuarterlyNetAmount.Text = (!hasSaving) ? " " : CurrentQuote.QuarterTotal.ToString("$###,##0.00");
            lblAnnualNetAmount.Text = (!hasSaving) ? " " : CurrentQuote.AnnualTotal.ToString("$###,##0.00");

            //Added to make the Quote READONLY after Approval. -ashok 12/29/05
            if (CurrentQuote.Status.ToString().ToUpper().Equals("APPROVED"))
            {
                btnSubmit.Enabled = false;
                //chkSendEmail.Enabled		= false;
                //chkTestAccount.Enabled = false;
            }
        }

        private void UpdateOtherQuoteInfo()
        {
            txtFirstName.Text = CurrentQuote.FirstName;
            txtLastName.Text = CurrentQuote.LastName;
            txtEmail.Text = CurrentQuote.Email;
            txtPhone.Text = CurrentQuote.Phone;
            txtFax.Text = CurrentQuote.Fax;
            txtCompanyName.Text = CurrentQuote.Company;
            ucEmailNotes.Notes = CurrentQuote.Notes;
            ucEmailNotes.InternalNotes = CurrentQuote.InternalNotes;

            //this wasn't there so far.. added by ashok 12/07/05
            this.ddlBillType.SelectedValue = CurrentQuote.BillType;
        }

        private void LoadInfoFromLeads()
        {
            if (Request["EmailID"] == null || Request["EmailID"].Trim().Length == 0)
            {
                return;
            }

            int emailID = Convert.ToInt32(Request["EmailID"]);
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID, Master.UserSession.CurrentUser);

            txtFirstName.Text = email.FirstName;
            txtLastName.Text = email.LastName;
            txtEmail.Text = email.EmailAddress;
            txtPhone.Text = email.Voice;
            txtCompanyName.Text = email.Company;
        }

        private void UpdateQuoteIDAndStatus()
        {
            lblStatus.Text = string.Format("{0}", CurrentQuote.Status);
        }

        private void OnOptionItemAdded(object sender, EventArgs e)
        {
            GetQuoteItems(CurrentQuote);
            CheckSubmitButton();
            UpdateUI();
        }

        private void CheckSubmitButton()
        {
            btnSubmit.Enabled = ddlChannels.SelectedIndex > 0;
            if (CurrentQuote.Customer.ID <= 0)
            {
                btnSubmit.Enabled = CurrentQuote.HasAnnualTechItem;
            }
        }
    }
}
