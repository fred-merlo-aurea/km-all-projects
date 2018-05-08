using System;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardGroup : System.Web.UI.UserControl, IECNWizard
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

        private int getPrePopLayoutID()
        {
            int PrePopLayoutID = 0;
            if (Request.QueryString["PrePopLayoutID"] != null)
            {
                PrePopLayoutID = Convert.ToInt32(Request.QueryString["PrePopLayoutID"].ToString());
            }
            return PrePopLayoutID;
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

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            groupExplorer1.setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            groupExplorer1.enableSelectMode();

        }

        public void Initialize()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                groupExplorer1.setDataFromCampaignItem(ci);
                if (ci.BlastList.Count == 0 && ci.CampaignItemTemplateID.HasValue && ci.CampaignItemTemplateID.Value > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplate cit = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(ci.CampaignItemTemplateID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    if (cit.SelectedGroupList != null)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup citg in cit.SelectedGroupList)
                        {
                            if (citg.Filters.Count > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in citg.Filters)
                                {
                                    if (cibf.FilterID != null)
                                    {
                                        groupExplorer1.setSelectedGroup(citg.GroupID, cibf.FilterID.Value, string.Empty, 0);
                                    }
                                }
                            }
                            else
                                groupExplorer1.setSelectedGroup(citg.GroupID, null, string.Empty, 0);
                        }
                    }
                    if (cit.SuppressionGroupList != null)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup SuppressionGroup in cit.SuppressionGroupList)
                        {
                            if (SuppressionGroup.Filters.Count > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in SuppressionGroup.Filters)
                                {
                                    if (cibf.FilterID != null)
                                    {
                                        groupExplorer1.setSuppressionGroup(SuppressionGroup.GroupID.Value, cibf.FilterID.Value, string.Empty, 0);
                                    }
                                }
                            }
                            else
                                groupExplorer1.setSuppressionGroup(SuppressionGroup.GroupID.Value, null, string.Empty, 0);
                        }
                    }
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }
    }
}