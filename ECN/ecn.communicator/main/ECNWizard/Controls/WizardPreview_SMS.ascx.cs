using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardPreview_SMS : System.Web.UI.UserControl, IECNWizard
    {
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
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID,  true);
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[0].LayoutID.Value, true);
                lblMessage.Text = layout.Slot1.ContentSMS;
                lblWelcome.Text = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.TextPowerWelcomeMsg;

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("GroupName", typeof(string)));
                dt.Columns.Add(new DataColumn("FilterName", typeof(string)));
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ci.BlastList)
                {
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ciBlast.GroupID.Value);
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlast.Filters)
                    {

                        ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                        DataRow dr = dt.NewRow();
                        dr["GroupName"] = group.GroupName;
                        dr["FilterName"] = filter.FilterName;
                        dt.Rows.Add(dr);
                    }
                }
                rpterGroupDetails.DataSource = dt;
                rpterGroupDetails.DataBind();
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
            }
        }

        public bool Save()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            ci.CompletedStep = 4;
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return true;
        }
    }
}