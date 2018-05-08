using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace ecn.communicator.main.ECNWizard
{
    public partial class templateEdit : System.Web.UI.Page
    {
        private int getCampaignItemTemplateID()
        {
            if (Request.QueryString["CampaignItemTemplateID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemTemplateID"]);
            }
            else
                return -1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.Heading = "Blasts/Reporting > Campaign Item Templates > Edit";
            Master.SubMenu = "campaign item templates";
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.View))
                {
                    addTemplate1.LoadControl();
                    btnSave.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.Edit);
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int templateID = addTemplate1.Save();
            if (templateID > 0)
            {
                addTemplate1.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Redit", "alert('Template saved successfully'); window.location='templateList.aspx';", true);
            }
        }
    }
}