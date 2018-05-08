using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using ECN_Framework_Common.Objects;
using Ecn.Communicator.Main.Admin.Interfaces;
using Ecn.Communicator.Main.Interfaces;
using Ecn.Communicator.Main.Helpers;
using Ecn.Communicator.Main.Admin.Helpers;

namespace ecn.communicator.main.admin.landingpages
{
    public partial class CustomerForwardToFriend : System.Web.UI.Page
    {
        private static ECN_Framework_Entities.Accounts.LandingPageAssign LPA = null;

        private const int LandingPageID = 3;

        private BaseChannelOperationsHandler _landingPagesOperations = null;
        private IMasterCommunicator MasterCommunicator;
        private ILandingPageAssign LandingPageAssign;
        private ILandingPageAssignContent LandingPageAssignContent;
        private HttpResponseBase HttpResponse;

        public CustomerForwardToFriend()
        {
            MasterCommunicator = new MasterCommunicatorAdapter(Master);
            LandingPageAssign = new LandingPageAssignAdapter();
            HttpResponse = new HttpResponseAdapter(Response);
            _landingPagesOperations = new BaseChannelOperationsHandler(LandingPageAssign);
        }

        public CustomerForwardToFriend(IMasterCommunicator masterCommunicator, ILandingPageAssign landingPageAssign, ILandingPageAssignContent landingPageAssignContent, HttpResponseBase response)
        {
            MasterCommunicator = masterCommunicator;
            LandingPageAssign = landingPageAssign;
            LandingPageAssignContent = landingPageAssignContent;
            HttpResponse = response;
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
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    btnPreview.Enabled = false;
                    btnPreview.Visible = false;
                    ECN_Framework_Entities.Accounts.LandingPageAssign lpaList = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentCustomer.BaseChannelID.Value, 3);

                    if (lpaList != null && lpaList.CustomerCanOverride.Value)
                    {
                        pnlNoAccess.Visible = false;
                        pnlSettings.Visible = true;
                        LoadData();
                        loadPreview();
                    }
                    else
                    {
                        pnlNoAccess.Visible = true;
                        pnlSettings.Visible = false;
                    }
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }
        private void LoadData()
        {
            LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, 3);
            if (LPA != null)
            {
                if (LPA.CustomerDoesOverride.Value == true)
                    rblBasechannelOverride.SelectedValue = "Yes";

                txtHeader.Text = LPA.Header;
                txtFooter.Text = LPA.Footer;


            }
        }
        private void loadPreview()
        {
            lblCustomerOverrideWarning.Text = "";
            lblCustomerOverrideWarning.Text = "";
            if (LPA != null)
            {
                //returns BlastID, GroupID, CustomerID and EmailAddress
                DataTable dataTable = LandingPageAssign.GetPreviewParameters(LPA.LPAID, Convert.ToInt32(MasterCommunicator.GetCustomerID()));
                string url;
                if (dataTable.Rows.Count > 0)
                {
                    btnPreview.Visible = true;
                    btnPreview.Enabled = true;
                    var blastId = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
                    var emailID = Convert.ToInt32(dataTable.Rows[0].ItemArray[1]);

                    url = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailtofriend.aspx?e=" + emailID + "&b=" + blastId + "&preview=" + LPA.LPAID;
                    var clickEvent = "window.open('" + url + "', 'popup_window', 'width=1000,height=750,resizable=yes');";
                    btnPreview.Attributes.Add("onclick", clickEvent);
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

        protected void btnSave_Click(object sender, EventArgs e)
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

            _landingPagesOperations.SetButtonVisibility(btnPreview, true);
            loadPreview();
            HttpResponse.Redirect("CustomerMain.aspx");
        }
    }
}