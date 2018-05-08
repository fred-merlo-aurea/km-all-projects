using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net.Mail;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.common.classes.license;
using ecn.accounts.classes.PayFlowPro;
using ecn.accounts.includes;
using ecn.accounts.classes;
using System.Configuration;
using KM.Common;

namespace ecn.accounts.Engines
{

    public enum QuoteApproveStepsEnum
    {
        ViewQuote = 1,
        InputCCInfo = 2,
        CreateCustomer = 3,
        CreateAdminUser = 4,
        ApproveQuote = 5,
        ThankYou = 6
    }


    public partial class QuoteApproval : System.Web.UI.Page, ICreditCardProcessor
    {
        private const string BillTypeCC = "CreditCard";

        private const string ControlUclQuoteViewer = "uclQuoteViewer";
        private const string ControlUclCCInforCollector = "uclCCInforCollector";
        private const string ControlUclCustomerInfoCollector = "uclCustomerInfoCollector";
        private const string ErrorUnkownQuoteStep = "Unknow step for proving quote.";
        private const string ErrorCustomerNameUsed = "Customer name is used. Please choose another one.";

        #region Session Variables
        private Quote CurrentQuote
        {
            get
            {
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


        private int Index
        {
            get
            {
                if (Session["ProveQuoteProcessStepIndex"] == null)
                {
                    Session["ProveQuoteProcessStepIndex"] = 0;
                }
                return (int)Session["ProveQuoteProcessStepIndex"];
            }
            set
            {
                Session["ProveQuoteProcessStepIndex"] = value;
            }
        }

        private int LastVisitedIndex
        {
            get
            {
                if (Session["LastVisitedIndex"] == null)
                {
                    Session["LastVisitedIndex"] = 0;
                }
                return (int)Session["LastVisitedIndex"];
            }
            set
            {
                Session["LastVisitedIndex"] = value;
            }
        }

        private ArrayList Steps
        {
            get { return (ArrayList)Session["ProveQuoteProcessSteps"]; }
            set { Session["ProveQuoteProcessSteps"] = value; }
        }

        private CreditCard CreditCard
        {
            get { return (CreditCard)Session["CreditCardInformation"]; }
            set { Session["CreditCardInformation"] = value; }
        }

        private User NewUser
        {
            get
            {
                if (Session["NewUser"] == null)
                {
                    Session["NewUser"] = new User(string.Empty, string.Empty, CurrentQuote.Customer);
                }
                return (User)Session["NewUser"];
            }
            set { Session["NewUser"] = value; }
        }

        #endregion

        private ICreditCardProcessor _CCHandler;
        // This class doesn't do anything right now. Plan to use it to perform an authrozie transaction.
        protected ICreditCardProcessor CCHandler
        {
            get
            {
                if (_CCHandler == null)
                {
                    _CCHandler = this;
                }
                return (this._CCHandler);
            }
            set
            {
                this._CCHandler = value;
            }
        }


        #region Event Handler
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                int quoteID = -1;
                int baseChannelID = -1;
                try
                {
                    quoteID = Convert.ToInt32(HttpRequestProcessor.Decrypt(Request["QuoteID"]));
                    baseChannelID = Convert.ToInt32(HttpRequestProcessor.Decrypt(Request["BaseChannelID"]));
                    if (quoteID <= 0)
                    {
                        throw new ApplicationException("Invalid quote id.");
                    }
                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Invalid Quote ID";
                    return;
                }

                CurrentQuote = null;
                CurrentQuote = Quote.GetQuoteByID(quoteID);

                if (CurrentQuote == null)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Quote does not exist in the system.";
                    return;
                }

                if (CurrentQuote.Customer.ID == -1 && baseChannelID <= 0)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Create a quote for new customer requires a channel ID.";
                    return;
                }

                if (CurrentQuote.Status == QuoteStatusEnum.Approved)
                {
                    dltProgressIndicator.Visible = false;
                    lblMessage.Visible = true;
                    lblMessage.Text = "Quote has been approved on " + CurrentQuote.ApproveDate.ToLongDateString() + " by " + CurrentQuote.Email;
                }

                if (baseChannelID > 0)
                {
                    CurrentQuote.Customer.BaseChannelID = baseChannelID;
                }
                CurrentQuote.AddItems(QuoteItem.GetQuoteItemsByQuoteID(quoteID));
                Steps = GetSteps(CurrentQuote);
                Index = 0;
                LastVisitedIndex = 0;
            }

            int currentStep = Convert.ToInt32(Request["Index"]);
            if (currentStep > 0 && currentStep <= LastVisitedIndex)
            {
                Index = currentStep;
            }
            InitializeUserControlForStep(Index);
        }

        protected void btnNextAction_OnClick(object sender, EventArgs e)
        {
            if (!PerformAction(Index))
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = _errorMessage;
                return;
            }
            Index += 1;
            if (Index > LastVisitedIndex)
            {
                LastVisitedIndex = Index;
            }
            InitializeUserControlForStep(Index);
        }

        protected void btnPreviousAction_OnClick(object sender, EventArgs e)
        {
            Index -= 1;
            InitializeUserControlForStep(Index);
        }

        protected void btnDecline_OnClick(object sender, EventArgs e)
        {
            CurrentQuote.Status = QuoteStatusEnum.Denied;
            CurrentQuote.Save();

            phdComponents.Controls.Clear();
            Label lblDoneInfo = new Label();
            lblDoneInfo.Text = string.Format("Thank you for taking time viewing the quote. {0} will contact you shortly to see what else we can do for your business. If you declined this quote in error, please click on the Back button above to return to the quote.", new KMPlatform.BusinessLogic.User().SelectUser(CurrentQuote.CreatedUserID).FirstName);
            phdComponents.Controls.Add(lblDoneInfo);

            Index++;
            btnPrevious.Visible = true;
            btnNextAction.Visible = btnDecline.Visible = false;

            //send email to Account Executive about the offer Decline - ashok 11/22/05			
            notifyNBDonQuoteStatus();
        }

        private void notifyNBDonQuoteStatus()
        {
            //send email to Account Executive about the offer Decline - ashok 11/22/05
            //made a separate Object called NotifyBillingDept & moved the functionality there. - ashok 12/27/05
            #region CLICK to view OLD CODE
            /*SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
			MailMessage message = new MailMessage();
			Staff nbd = (Staff) CurrentQuote.NBDs[0];			
			message.From = "sales@knowldegemarketing.com";
			message.To = nbd.FromEmailAddress;
			message.Cc = "lindac@teckman.com";
			message.Subject = "An Invoice Email from "+CurrentQuote.Company+" has been "+CurrentQuote.Status.ToString().ToUpper();
			string body = nbd.FirstName+",<br><br>";
			body += " Quote to "+CurrentQuote.Company+" is <b>"+CurrentQuote.Status.ToString().ToUpper()+"</b>. Please co-ordinate with the right person to discuss further details<br>";
			body+="Quote URL: <a href='"+Request.Url+"'>"+Request.Url+"</a><br><br><hr size=1>";
			body += "This is an automated message from ECN5 Billing System. Please do not reply to this message";
			message.Body = body;
			message.BodyFormat = MailFormat.Html;
			message.Priority = MailPriority.High;
			SmtpMail.Send(message);
			*/
            #endregion

            //Staff StaffCreated = Staff.GetStaffByUserID(CurrentQuote.CreatedUserID);
            KMPlatform.Entity.User StaffCreated = new KMPlatform.BusinessLogic.User().SelectUser(CurrentQuote.CreatedUserID);
            string body = " Quote to " + CurrentQuote.Company + " is <b>" + CurrentQuote.Status.ToString().ToUpper() + "</b>. Please co-ordinate with the right person to discuss further details<br>";
            body += "Quote URL: <a href='" + Request.Url + "'>" + Request.Url + "</a><br><br><hr size=1>";
            body += "This is an automated message from ECN5 Billing System. Please do not reply to this message";

            NotifyBillingDept.NotifyBillingAdmin = true;
            //NotifyBillingDept.StaffInfo = StaffCreated;
            NotifyBillingDept.EmailSubject = CurrentQuote.QuoteCode + " - An Invoice Email from " + CurrentQuote.Company + " has been " + CurrentQuote.Status.ToString().ToUpper() + " by " + CurrentQuote.Email;
            NotifyBillingDept.EmailBody = body;
            //NotifyBillingDept.notifyNBDonQuoteStatus();
            NotifyBillingDept.notifyBillingDept();
            //end sending mail to the Account Executive.
        }

        private void dltProgressIndicator_ItemCreated(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Header)
            {
                return;
            }

            LinkButton btnStep = (LinkButton)e.Item.FindControl("btnStep");
            if ((e.Item.ItemIndex % 2) == 0)
            {
                int stepIndex = e.Item.ItemIndex / 2;
                btnStep.Enabled = stepIndex <= LastVisitedIndex;
                btnStep.CommandArgument = stepIndex.ToString();
            }

            if (!btnStep.Enabled)
            {
                btnStep.Attributes.Add("Font-Underline", "False");
            }
        }

        protected void btnStep_OnClick(object sender, EventArgs e)
        {
            LinkButton btnStep = (LinkButton)sender;
            Index = Convert.ToInt32(btnStep.CommandArgument);
            InitializeUserControlForStep(Index);
        }
        private void ShowProgressIndicator()
        {
            dltProgressIndicator.DataSource = GetIndicatorStrings(Steps);
            dltProgressIndicator.SelectedIndex = Index * 2;
            dltProgressIndicator.DataBind();
        }
        #endregion

        #region Quote Prove Process Flow Control Methods
        private bool IsVisited
        {
            get { return Index < LastVisitedIndex; }
        }
        private ArrayList GetSteps(Quote quote)
        {
            ArrayList steps = new ArrayList();
            if (quote.Customer.IsNew)
            {
                if (quote.BillType == "CreditCard")
                {
                    steps.Add(QuoteApproveStepsEnum.ViewQuote);
                    steps.Add(QuoteApproveStepsEnum.CreateCustomer);
                    // steps.Add(QuoteApproveStepsEnum.CreateAdminUser);
                    steps.Add(QuoteApproveStepsEnum.InputCCInfo);
                    steps.Add(QuoteApproveStepsEnum.ApproveQuote);
                    steps.Add(QuoteApproveStepsEnum.ThankYou);
                    return steps;
                }

                steps.Add(QuoteApproveStepsEnum.ViewQuote);
                steps.Add(QuoteApproveStepsEnum.CreateCustomer);
                // steps.Add(QuoteApproveStepsEnum.CreateAdminUser);
                //steps.Add(QuoteApproveStepsEnum.ApproveQuote);
                steps.Add(QuoteApproveStepsEnum.ThankYou);
                return steps;
            }

            if (quote.BillType == "CreditCard")
            {
                steps.Add(QuoteApproveStepsEnum.ViewQuote);
                steps.Add(QuoteApproveStepsEnum.InputCCInfo);
                //steps.Add(QuoteApproveStepsEnum.ApproveQuote);
                steps.Add(QuoteApproveStepsEnum.ThankYou);
                return steps;
            }

            steps.Add(QuoteApproveStepsEnum.ViewQuote);
            //steps.Add(QuoteApproveStepsEnum.ApproveQuote);
            steps.Add(QuoteApproveStepsEnum.ThankYou);
            return steps;
        }

        private string GetBriefSummary(ArrayList steps)
        {
            StringBuilder summary = new StringBuilder("<UL>");
            for (int i = 1; i < steps.Count - 2; i++)
            {
                QuoteApproveStepsEnum step = (QuoteApproveStepsEnum)steps[i];
                switch (step)
                {
                    case QuoteApproveStepsEnum.CreateCustomer:
                        summary.Append(string.Format("<li>Customer '{0}' will be created.</li>", CurrentQuote.Customer.Name));
                        break;
                    case QuoteApproveStepsEnum.CreateAdminUser:
                        summary.Append(string.Format("<li>User '{0} will be created.</li>", NewUser.UserName));
                        break;
                    case QuoteApproveStepsEnum.InputCCInfo:
                        summary.Append(string.Format("<li>${0} will be charged to the credit card({1}).</li>", CurrentQuote.Total.ToString("###,###,###.00"), CreditCard.MaskedCardNumber));
                        break;
                    default:
                        throw new ApplicationException(string.Format("'{0}' should not be shown in brief summary.", step));
                }
            }
            summary.Append("</UL>");
            return summary.ToString();
        }
        private StringCollection GetIndicatorStrings(ArrayList steps)
        {
            StringCollection indicators = new StringCollection();
            for (int i = 0; i < steps.Count; i++)
            {
                if (i > 0)
                {
                    indicators.Add("&nbsp;>>&nbsp;");
                }
                indicators.Add(GetFriendlyStepName((QuoteApproveStepsEnum)steps[i]));
            }
            return indicators;
        }

        private string GetFriendlyStepName(QuoteApproveStepsEnum step)
        {
            switch (step)
            {
                case QuoteApproveStepsEnum.ViewQuote:
                    return "View Quote";
                case QuoteApproveStepsEnum.InputCCInfo:
                    return "Input Credit Card info";
                case QuoteApproveStepsEnum.CreateCustomer:
                    return "Input Customer info";
                case QuoteApproveStepsEnum.CreateAdminUser:
                    return "Input User info";
                case QuoteApproveStepsEnum.ApproveQuote:
                    return "Confirm";
                case QuoteApproveStepsEnum.ThankYou:
                    return "Done";
                default:
                    throw new ApplicationException("Unknow step for proving quote.");
            }
        }

        private void InitializeUserControlForStep(int stepIndex)
        {
            QuoteApproveStepsEnum currentStep = (QuoteApproveStepsEnum)Steps[stepIndex];
            lblErrorMessage.Visible = false;
            phdComponents.Controls.Clear();
            ShowProgressIndicator();
            switch (currentStep)
            {
                case QuoteApproveStepsEnum.ViewQuote:
                    QuoteViewer viewer = (QuoteViewer)LoadControl(@"..\includes\QuoteViewer.ascx");
                    viewer.ID = "uclQuoteViewer";
                    viewer.Quote = CurrentQuote;
                    viewer.ContactEditor.Contact = CurrentQuote.Customer.GeneralContact;
                    viewer.ContactEditor.Company = CurrentQuote.Customer.Name;
                    if (!IsVisited && CurrentQuote.Customer.IsNew)
                    {
                        Contact contact = new Contact("", string.Format("{0} {1}", CurrentQuote.FirstName, CurrentQuote.LastName), "", CurrentQuote.Phone, CurrentQuote.Fax, CurrentQuote.Email, "", "", "", "US", "");
                        viewer.ContactEditor.Company = CurrentQuote.Company;
                        viewer.ContactEditor.Contact = contact;
                    }
                    NewUser.UserName = CurrentQuote.Email;
                    viewer.ShowHeader(CurrentQuote, NewUser);
                    phdComponents.Controls.Add(viewer);
                    SetButtons(stepIndex);
                    break;
                case QuoteApproveStepsEnum.InputCCInfo:
                    CCInfoCollector ccCollector = (CCInfoCollector)LoadControl(@"..\includes\CCInfoCollector.ascx");
                    ccCollector.ID = "uclCCInforCollector";
                    if (IsVisited)
                    {
                        ccCollector.CreditCard = CreditCard;
                        ccCollector.ContactEditor.Contact = CurrentQuote.Customer.BillingContact;
                        ccCollector.ContactEditor.Company = CurrentQuote.Customer.Name;
                    }
                    else
                    {
                        ccCollector.ContactEditor.Contact = CurrentQuote.Customer.BillingContact;
                        ccCollector.ContactEditor.Company = CurrentQuote.Customer.Name;
                    }
                    phdComponents.Controls.Add(ccCollector);
                    SetButtons(stepIndex);
                    break;
                case QuoteApproveStepsEnum.CreateCustomer:
                    CustomerInfoCollector customerInfoCollector = (CustomerInfoCollector)LoadControl(@"..\includes\CustomerInfoCollector.ascx");
                    customerInfoCollector.ID = "uclCustomerInfoCollector";
                    customerInfoCollector.Customer = CurrentQuote.Customer;
                    phdComponents.Controls.Add(customerInfoCollector);
                    SetButtons(stepIndex);
                    break;
                case QuoteApproveStepsEnum.ApproveQuote:
                    InfoSummary infoSummary = (InfoSummary)LoadControl(@"..\includes\InfoSummary.ascx");
                    infoSummary.Summary = GetBriefSummary(Steps);
                    phdComponents.Controls.Add(infoSummary);
                    SetButtons(stepIndex);
                    break;
                case QuoteApproveStepsEnum.ThankYou:
                    Label lblDoneInfo = new Label();
                    lblDoneInfo.Text = "You have successfully accepted your quote. Please close this window to end your quote session.<br><br>" +
                                                    "Thank you for doing business with Knowledge Marketing.";
                    phdComponents.Controls.Add(lblDoneInfo);
                    SetButtons(stepIndex);
                    break;
                default:
                    throw new ApplicationException("Unknow step for proving quote.");
            }
        }

        private string _errorMessage;
        private bool PerformAction(int stepIndex)
        {
            var currentStep = (QuoteApproveStepsEnum)Steps[stepIndex];
            _errorMessage = string.Empty;
            switch (currentStep)
            {
                case QuoteApproveStepsEnum.ViewQuote:
                    return PerformViewQuoteAction();
                case QuoteApproveStepsEnum.InputCCInfo:
                    var ccInfoCollector = phdComponents.FindControl(ControlUclCCInforCollector) as CCInfoCollector;
                    Guard.NotNull(ccInfoCollector, nameof(ccInfoCollector));
                    if (!ccInfoCollector.AgreeToUseRecurringService)
                    {
                        _errorMessage = "You need to agree to use recurring billing service.";
                        return false;
                    }
                    CreditCard = ccInfoCollector.CreditCard;
                    _errorMessage = "Please check credit card information.";
                    return CCHandler.IsCreditCardValid(CreditCard);
                case QuoteApproveStepsEnum.CreateCustomer:
                    return PerformViewCustomerAction();
                case QuoteApproveStepsEnum.ApproveQuote:
                case QuoteApproveStepsEnum.ThankYou:
                    return true;
                default:
                    throw new ApplicationException(ErrorUnkownQuoteStep);
            }
        }

        private bool PerformViewCustomerAction()
        {
            var customerInfoCollector = (CustomerInfoCollector) phdComponents.FindControl(ControlUclCustomerInfoCollector);

            customerInfoCollector.SetCustomer(CurrentQuote.Customer);
            _errorMessage = ErrorCustomerNameUsed;

            if (CurrentQuote.Customer.CustomerNameExists())
            {
                return false;
            }
            else
            {
                CurrentQuote.Status = QuoteStatusEnum.Approved;
                var bill = CreateBill();
                notifyNBDonQuoteStatus();

                if (CurrentQuote.BillType == BillTypeCC)
                {
                    bool isChargedToCreditCard;
                    try
                    {
                        isChargedToCreditCard = ChargeCreditCard(bill);
                    }
                    catch (Exception exception)
                    {
                        _errorMessage = exception.Message;
                        return false;
                    }

                    if (!isChargedToCreditCard)
                    {
                        // Even if something goes wrong, still need to save profile ID of the successful transaction.
                        CurrentQuote.Status = QuoteStatusEnum.Pending;
                        CurrentQuote.Save();
                        return false;
                    }
                }

                SaveAll(bill);
                CreateLicenses(bill);
                return true;
            }
        }

        private bool PerformViewQuoteAction()
        {
            var viewer = (QuoteViewer) phdComponents.FindControl(ControlUclQuoteViewer);
            CurrentQuote.Customer.GeneralContact = viewer.ContactEditor.Contact;
            CurrentQuote.Customer.Name = viewer.ContactEditor.Company;
            CurrentQuote.StartDate = viewer.StartDate;
            if (viewer.ContactEditor.IsTheSameAsBillingAddress)
            {
                CurrentQuote.Customer.BillingContact = viewer.ContactEditor.Contact;
            }

            if (viewer.ContactEditor.IsTheSameAsTechContact)
            {
                CurrentQuote.Customer.TechContact = viewer.ContactEditor.Contact.ContactName;
                CurrentQuote.Customer.TechEmail = viewer.ContactEditor.Contact.Email;
                CurrentQuote.Customer.TechPhone = viewer.ContactEditor.Contact.Phone;
            }

            viewer.UserInfoEditor.SetUser(NewUser);
            if (!viewer.AgreeToTermsAndConditions)
            {
                _errorMessage = "You need to agree to Terms & Conditions.";
                return false;
            }

            if (!CurrentQuote.Customer.IsNew)
            {
                CurrentQuote.Status = QuoteStatusEnum.Approved;
                var bill = CreateBill();
                notifyNBDonQuoteStatus();

                if (CurrentQuote.BillType == BillTypeCC)
                {
                    bool IsChargedToCreditCard;
                    try
                    {
                        IsChargedToCreditCard = ChargeCreditCard(bill);
                    }
                    catch (Exception exception)
                    {
                        _errorMessage = exception.Message;
                        return false;
                    }

                    if (!IsChargedToCreditCard)
                    {
                        // Even if something goes wrong, still need to save profile ID of the successful transaction.
                        CurrentQuote.Status = QuoteStatusEnum.Pending;
                        CurrentQuote.Save();
                        return false;
                    }
                }

                SaveAll(bill);
                CreateLicenses(bill);
            }

            return true;
        }

        private void SetButtons(int stepIndex)
        {
            int count = Steps.Count;

            btnPrevious.Visible = stepIndex != 0;

            if (CurrentQuote.Status == QuoteStatusEnum.Approved && !CurrentQuote.Customer.IsNew)
            {
                btnPrevious.Visible = btnNextAction.Visible = btnDecline.Visible = false;
                return;
            }

            if (stepIndex == 0)
            {
                btnNextAction.Text = "  Accept  ";
                btnNextAction.Visible = btnDecline.Visible = true;
            }
            else
            {
                btnDecline.Visible = false;
            }

            if (stepIndex == count - 2 && stepIndex != 0)
            {
                btnNextAction.Text = "  Done  ";
                return;
            }

            if (stepIndex == count - 1)
            {
                btnPrevious.Visible = false;
                btnNextAction.Visible = false;
                btnDecline.Visible = false;
                return;
            }

            btnNextAction.Text = stepIndex == 0 ? "Accept" : "  Next  ";
            btnNextAction.Visible = true;
        }

        private void SaveAll(Bill bill)
        {
            if (CurrentQuote.Customer.IsNew)
            {
                CurrentQuote.Customer.CreatedDate = DateTime.Now;
                CurrentQuote.Customer.IsActive = "Y";
                CurrentQuote.Customer.IsStrategic = "N";
                //CurrentQuote.Customer.AccountExecutiveID = bill.Quote.AccountExecutiveID;
                CurrentQuote.Customer.AccountManagerID = bill.Quote.AccountManagerID;
                bool success = DoFrameworkClientSave(CurrentQuote.Customer, CurrentQuote.CreatedUserID);
                //CurrentQuote.Customer.Save(CurrentQuote.CreatedUserID);
                if (success)
                {
                    CurrentQuote.Customer.CreateAssertPaths(Server);
                    //CurrentQuote.Customer.CreateDefaultFeatures(CurrentQuote.CreatedUserID);
                    //CurrentQuote.Customer.CreateDefaulRole(CurrentQuote.CreatedUserID);
                    //CurrentQuote.Customer.CreateMasterSupressionGroup();
                    //NewUser.Save();
                }
                else
                {
                    return;
                }
            }
            CurrentQuote.Save();
            bill.Save(CurrentQuote.CreatedUserID);
        }

        private bool DoFrameworkClientSave(Customer customer, int createdUserID)
        {
            KMPlatform.Entity.User sysAdmin = new KMPlatform.BusinessLogic.User().ECN_SelectUser(createdUserID,true);
            ECN_Framework_Entities.Accounts.Customer c = new ECN_Framework_Entities.Accounts.Customer();
            KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
            KMPlatform.BusinessLogic.Client clientBusinessLogic = new KMPlatform.BusinessLogic.Client();
            ECN_Framework_Entities.Accounts.BaseChannel existingBC = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(customer.BaseChannelID);
            //Get Customer object ready and validate
            #region customerobject
            //c.PlatformClientID = clientID;
            c.CustomerID = -1;
            c.BaseChannelID = customer.BaseChannelID;
            c.CustomerName = customer.Name;
            c.GeneralContant = new ECN_Framework_Entities.Accounts.Contact(customer.GeneralContact.Salutation, customer.GeneralContact.FirstName, customer.GeneralContact.LastName, customer.GeneralContact.ContactTitle, customer.GeneralContact.Phone,
                                                                            customer.GeneralContact.Fax, customer.GeneralContact.Email, customer.GeneralContact.StreetAddress, customer.GeneralContact.City, customer.GeneralContact.State, customer.GeneralContact.Country, customer.GeneralContact.Zip);
            c.BillingContact = new ECN_Framework_Entities.Accounts.Contact(customer.BillingContact.Salutation, customer.BillingContact.FirstName, customer.BillingContact.LastName, customer.BillingContact.ContactTitle, customer.BillingContact.Phone,
                                                                            customer.BillingContact.Fax, customer.BillingContact.Email, customer.BillingContact.StreetAddress, customer.BillingContact.City, customer.BillingContact.State, customer.BillingContact.Country, customer.BillingContact.Zip);
            c.ActiveFlag = customer.IsActive;
            c.DemoFlag = customer.IsDemo;
            c.WebAddress = customer.WebAddress;
            c.TechContact = customer.TechContact;
            c.TechEmail = customer.TechEmail;
            c.TechPhone = customer.TechPhone;
            c.SubscriptionsEmail = customer.SubscriptionsEmail;
            c.CustomerType = customer.CustomerType;
            c.CreatedUserID = createdUserID;

            c.AccountExecutiveID = null;
            c.AccountManagerID = null;
            c.IsStrategic = false;
            c.ABWinnerType = "clicks";
            c.DefaultBlastAsTest = false; 
            
            try // catch any validation issues before we create the Client record
            {
                ECN_Framework_BusinessLayer.Accounts.Customer.Validate(c, sysAdmin);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                lblErrorMessage.Visible = true;
                return false;
            }
            #endregion

            //Get Client Object ready 
            #region client object
            client.ClientID = -1;
            client.ClientName = c.CustomerName;
            client.DisplayName = c.CustomerName;
            client.IsActive = true;
            //client.ClientCode = c.CustomerID.ToString();
            client.ParentClientId = 0;
            client.IsKMClient = false;
            client.DateCreated = DateTime.Now;
            client.CreatedByUserID = createdUserID;
            #endregion

            //Start saving
            bool clientSave = false;
            try
            {
                c.PlatformClientID = clientBusinessLogic.Save(client,true);
                clientSave = true;
                c.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.Save(c, sysAdmin);
            }
            catch(Exception ex) 
            {
                if (clientSave && c.PlatformClientID > 0)
                    clientBusinessLogic.Delete(c.PlatformClientID);
                lblErrorMessage.Text = "Error creating account";
                lblErrorMessage.Visible = true;
                return false;
            }
            KMPlatform.Entity.ClientGroupClientMap cgcm = new KMPlatform.Entity.ClientGroupClientMap();
            cgcm.ClientGroupID = existingBC.PlatformClientGroupID;
            cgcm.ClientID = c.PlatformClientID;
            cgcm.IsActive = true;
            cgcm.DateCreated = DateTime.Now;
            cgcm.CreatedByUserID = createdUserID;
            new KMPlatform.BusinessLogic.ClientGroupClientMap().Save(cgcm);


            new KMPlatform.BusinessLogic.SecurityGroup().CreateFromTemplateForClient(
                        "Administrator", 0, c.PlatformClientID, "Administrator", sysAdmin);

            CurrentQuote.Customer.ID = c.CustomerID;
            return true;
        }

        private bool ChargeCreditCard(Bill bill)
        {
            if (bill == null)
            {
                throw new ApplicationException("No bills is generated for this quote.");
            }
            int failedTransactionCount = 0;
            string response = string.Empty;
            PayFlowProCreditCardProcessor processor = PayFlowProCreditCardProcessor.Instance;
            foreach (BillItem billItem in bill.CurrentBillItems)
            {
                if (billItem.QuoteItem.Frequency == FrequencyEnum.OneTime && billItem.TransactionID == "")
                {
                    response = processor.ProcessSalesTransaction(CreditCard, billItem.Total, billItem.QuoteItem.Description);
                    billItem.TransactionID = ResponseParser.GetTransactionID(response);
                    billItem.Status = ResponseParser.IsTransactionSuccessful(response) ? BillItemStatusEnum.ChargedToCreditCard : BillItemStatusEnum.Pending;

                    if (!ResponseParser.IsTransactionSuccessful(response))
                    {
                        failedTransactionCount++;
                        _errorMessage += ResponseParser.GetReturnMessage(response);

                        processor.VoidSaleTransaction(billItem.TransactionID);
                    }
                    continue;
                }

                if (billItem.Total == 0)
                {
                    continue;
                }

                Profile profile = new Profile(
                    string.Format("bs_{0}_{1}", billItem.QuoteItem.Parent.QuoteID, billItem.QuoteItem.ID),
                    billItem.QuoteItem.Parent.Customer.Name, CreditCard, billItem.QuoteItem.Frequency, billItem.Total);

                profile.ID = billItem.QuoteItem.RecurringProfileID;

                if (profile.IsNew)
                {
                    response = processor.AddProfile(profile, new DateTime(billItem.StartDate.Year, billItem.StartDate.Month, billItem.StartDate.Day));
                    billItem.QuoteItem.RecurringProfileID = profile.ID;
                }
                else
                {
                    response = processor.ModifyProfile(profile, new DateTime(billItem.StartDate.Year, billItem.StartDate.Month, billItem.StartDate.Day));
                }
                if (!ResponseParser.IsTransactionSuccessful(response))
                {
                    failedTransactionCount++;
                    _errorMessage += string.Format("{0}: The system did not accept the bill line item {1}.", ResponseParser.GetReturnMessage(response), billItem.QuoteItem.Name);
                }
            }
            _errorMessage = string.Format("{0} transactoin(s) fail(s):<br/>", failedTransactionCount) + _errorMessage + "<br/>" + response + "<br/>Please click on the 'Done' button again.";
            return failedTransactionCount == 0;
        }

        private Bill CreateBill()
        {
            BillingEngine billingHistoryManager = new BillingEngine(DateTime.Now, 10);
            Bill bill = CurrentQuote.CreateBill(billingHistoryManager, DateTime.Now);
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

        #endregion

        #region ICreditCardProcessor Members
        /// Need to find out if the bank charges for the authoriz-only transaction		
        public bool IsCreditCardValid(CreditCard creditCard)
        {
            return true;
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }

}
