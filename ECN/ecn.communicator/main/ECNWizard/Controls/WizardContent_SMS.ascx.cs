using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardContent_SMS : System.Web.UI.UserControl, IECNWizard
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                loadData();
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
            }

        }

        public bool Save()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
          
            ECN_Framework_Entities.Communicator.Content content = new ECN_Framework_Entities.Communicator.Content();
            content.ContentTitle = ci.CampaignItemName;
            content.ContentTypeCode = "HTML";
            content.LockedFlag = "N";
            content.FolderID = 0;
            content.ContentSMS = ContentText.Text;
            content.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            content.Sharing = "N";
            content.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            content.ContentSource = "";
            content.ContentMobile = "";
            
            int contentID=ECN_Framework_BusinessLayer.Communicator.Content.Save(content,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);


            return true;
        }

        private void loadData()
        {
            WelcomeText.Text = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TextPowerWelcomeMsg;
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            if (ci.BlastList.Count > 0)
            {
                if (ci.BlastList[0].LayoutID != null)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(ci.BlastList[0].LayoutID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    ContentText.Text = layout.Slot1.ContentSMS;
                }
            }
        }
    }
}