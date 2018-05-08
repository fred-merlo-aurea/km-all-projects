using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Ecn.Communicator.Main.Interfaces;
using Ecn.Communicator.Main.Helpers;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Admin.Helpers;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;

namespace ecn.communicator.main.admin.landingpages
{
    public partial class CustomerAbuse : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        private static LandingPageAssign LPA = null;
        private static List<LandingPageOption> _listOptions = null;

        private const int LandingPageID = 4;

        private BaseChannelOperationsHandler _landingPagesOperations = null;
        private IMasterCommunicator MasterCommunicator;
        private ILandingPageAssign LandingPageAssign;
        private ILandingPageAssignContent LandingPageAssignContent;
        private HttpResponseBase HttpResponse;
        private IApplicationLog ApplicationLog;
        public CustomerAbuse()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
            ApplicationLog = new ApplicationLogAdapter();
            _landingPagesOperations = new BaseChannelOperationsHandler(LandingPageAssign);
        }

        public CustomerAbuse(
            IMasterCommunicator masterCommunicator, 
            ILandingPageAssign landingPageAssign, 
            ILandingPageAssignContent landingPageAssignContent, 
            HttpResponseBase response,
            IApplicationLog applicationLog)
        {
            MasterCommunicator = masterCommunicator;
            LandingPageAssign = landingPageAssign;
            LandingPageAssignContent = landingPageAssignContent;
            HttpResponse = response;
            ApplicationLog = applicationLog;
            _landingPagesOperations = new BaseChannelOperationsHandler(LandingPageAssign);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CUSTOMER;
            if (!IsPostBack)
            {
                btnPreview.Enabled = false;
                btnPreview.Visible = false;

                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    ECN_Framework_Entities.Accounts.LandingPageAssign lpaList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value, 4);

                    if (lpaList != null && lpaList.CustomerCanOverride.Value)
                    {
                        pnlNoAccess.Visible = false;
                        pnlSettings.Visible = true;
                        LoadData();
                        LoadPreview();
                    }
                    else
                    {
                        pnlNoAccess.Visible = true;
                        pnlSettings.Visible = false;
                    }
                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }
        private void LoadData()
        {
            LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, 4);
            _listOptions = ECN_Framework_BusinessLayer.Accounts.LandingPageOption.GetByLPID(4);

            if (LPA != null)
            {
                if (LPA.CustomerDoesOverride == true)
                {
                    rblBasechannelOverride.SelectedValue = "Yes";
                }

                txtHeader.Text = LPA.Header;
                txtFooter.Text = LPA.Footer;

                List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> lpacList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssignContent.GetByLPAID(LPA.LPAID);

                try
                {
                    var ThankYouText = lpacList.First(x => x.LPOID == _listOptions.First(y => y.Name.ToLower().Replace(" ", "").Contains("thankyou")).LPOID);
                    if (ThankYouText != null)
                    {
                        txtThankYou.Text = ThankYouText.Display;
                    }
                }
                catch { }
                btnPreview.Visible = true;
                btnPreview.Enabled = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_landingPagesOperations.CanSaveCustomer(
                    MasterCommunicator,
                    phError,
                    ref LPA,
                    LandingPageID,
                    MasterCommunicator.GetCustomerID(),
                    rblBasechannelOverride,
                    txtHeader,
                    txtFooter,
                    true,
                    lblErrorMessage))
                {
                    return;
                }

                var codeSnippetError = new CodeSnippetError();
                codeSnippetError = _landingPagesOperations.ValidCodeSnippets(txtThankYou.Text);
                if (!codeSnippetError.valid)
                {
                    _landingPagesOperations.ThrowECNException(
                        string.Format("{0} in Thank You Message", codeSnippetError.message), 
                        phError, 
                        lblErrorMessage);
                    return;
                }

                LandingPageAssignContent.Delete(LPA.LPAID, MasterCommunicator.GetCurrentUser());

                _landingPagesOperations.SaveLandingPageAssign(
                    txtThankYou,
                    MasterCommunicator.GetUserID(),
                    LPA.LPAID,
                    _listOptions,
                    LandingPageAssignContent,
                    MasterCommunicator.GetCurrentUser());

                _landingPagesOperations.SetButtonCssClass(btnPreview);
                LoadPreview();
                HttpResponse.Redirect("CustomerMain.aspx");
            }
            catch (ECNException ex)
            {
                _landingPagesOperations.HandleCriticalError(ApplicationLog, ex, phError, lblErrorMessage);
            }
        }

        private void LoadPreview()
        {
            lblCustomerOverrideWarning.Text = "";
            lblCustomerOverrideWarning.Text = "";
            if (LPA != null)
            {
                //returns BlastID, GroupID, CustomerID and EmailAddress
                DataTable dt = LandingPageAssign.GetPreviewParameters(LPA.LPAID, Convert.ToInt32(MasterCommunicator.GetCustomerID()));
                string url;
                if (dt.Rows.Count > 0)
                {
                    btnPreview.Visible = true;
                    btnPreview.Enabled = true;
                    string p = dt.Rows[0][0].ToString();

                    url = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/reportSpam.aspx?p=" + p + "&preview=" + LPA.LPAID;
                    string s = "window.open('" + url + "', 'popup_window', 'width=1000,height=750,resizable=yes');";
                    btnPreview.Attributes.Add("onclick", s);
                }
                else
                {
                    btnPreview.Enabled = false;
                    btnPreview.Visible = false;
                    lblSentBlastsWarning.Text = "Preview functionality will not be available until after you have sent at least one blast from this customer account.";
                }

                if (LPA.CustomerDoesOverride.Value == false)
                {
                    lblCustomerOverrideWarning.Text = "Note: The above settings will not be visible to customers until you override the Basechannel settings.";
                }
            }
        }
    }
}