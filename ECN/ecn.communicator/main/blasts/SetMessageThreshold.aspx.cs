using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ecn.communicator.blastsmanager
{
    public partial class SetMessageThreshold : ECN_Framework.WebPageHelper
    {
        int channelID = 0;
        int customerID = 0;
        int userID = 0;

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "message threshold";
            Master.Heading = "Set Message Threshold";
            Master.HelpContent = "";
            Master.HelpTitle = "Set Message Threshold";

            //if ((KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser)) && ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures. "SetMessageThresholds"))
            //if (KM.Platform.User.IsAdministratorOrHasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ConfigurationManager.AppSettings["PlatformApplication"].ToString() ,"Message Thresholds"))
            if ((KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser)) && KM.Platform.User.HasServiceFeature(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessageThresholds))
                {
                channelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                customerID = Master.UserSession.CurrentUser.CustomerID;
                userID = Master.UserSession.CurrentUser.UserID;

                if (!IsPostBack)
                {
                    GetCurrentValue();
                }
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Accounts.BaseChannel baseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(channelID);
                baseChannel.EmailThreshold = Convert.ToInt32(ddlThreshold.SelectedValue);
                baseChannel.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Accounts.BaseChannel.Save(baseChannel, Master.UserSession.CurrentUser);
                Response.Redirect("default.aspx");
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        private void GetCurrentValue()
        {
            ECN_Framework_Entities.Accounts.BaseChannel baseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(channelID);
           
            ddlThreshold.SelectedIndex = -1;
            if (baseChannel.EmailThreshold != null && ddlThreshold.Items.FindByValue(baseChannel.EmailThreshold.ToString()) != null)
            {
                ddlThreshold.Items.FindByValue(baseChannel.EmailThreshold.ToString()).Selected = true;
            }
            else
            {
                ddlThreshold.Items.FindByValue("0").Selected = true;
            }
        }
    }
}
