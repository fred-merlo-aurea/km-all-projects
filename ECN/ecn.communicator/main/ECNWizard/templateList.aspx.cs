using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.ECNWizard
{
    public partial class templateList : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.Heading = "Blasts/Reporting > Campaign Item Templates";
            Master.SubMenu = "campaign item templates";
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.View))
                {
                    gvCampaignItemTemplate.Columns[4].Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.Edit);
                    gvCampaignItemTemplate.Columns[5].Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.Delete);

                    loadCampaignItemTemplate(ddlArchiveFilter.SelectedValue.ToString());
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
                gvCampaignItemTemplate.Columns[6].Visible = true;
            }
            else
            {
                gvCampaignItemTemplate.Columns[6].Visible = false;
            }
        }

        private void loadCampaignItemTemplate(string archiveFilter)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> domainTrackerList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser,false,archiveFilter);
            gvCampaignItemTemplate.DataSource = domainTrackerList;
            gvCampaignItemTemplate.DataBind();
        }

        protected void gvCampaignItemTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string CampaignItemTemplateID = e.CommandArgument.ToString();
            if (e.CommandName == "CampaignItemTemplateDelete")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Delete(Convert.ToInt32(CampaignItemTemplateID), Master.UserSession.CurrentUser);
                    loadCampaignItemTemplate(ddlArchiveFilter.SelectedValue.ToString());
                }
                catch(ECN_Framework_Common.Objects.ECNException ecn)
                {
                    setECNError(ecn);
                }
                
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("templateedit.aspx");
        }

        protected void ddlArchiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCampaignItemTemplate(ddlArchiveFilter.SelectedValue.ToString());
        }

        protected void chkIsArchived_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkArchived = (CheckBox)sender;

            int index = Convert.ToInt32(chkArchived.Attributes["index"].ToString());
            int citID = Convert.ToInt32(gvCampaignItemTemplate.DataKeys[index].Value.ToString());
            ECN_Framework_Entities.Communicator.CampaignItemTemplate template = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(citID, Master.UserSession.CurrentUser, false);
            template.Archived = chkArchived.Checked;
            template.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

            ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.Save(template, Master.UserSession.CurrentUser);
            loadCampaignItemTemplate(ddlArchiveFilter.SelectedValue.ToString());
        }

        protected void gvCampaignItemTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkArchived = (CheckBox)e.Row.FindControl("chkIsArchived");
                ECN_Framework_Entities.Communicator.CampaignItemTemplate template = (ECN_Framework_Entities.Communicator.CampaignItemTemplate)e.Row.DataItem;

                chkArchived.Checked = template.Archived.HasValue ? template.Archived.Value : false;
                chkArchived.Attributes.Add("index", e.Row.RowIndex.ToString());

            }
        }
    }
}