using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class groupconfig : ECN_Framework.WebPageHelper
    {
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;


            if (Page.IsPostBack == false)
            {            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupConfig, KMPlatform.Enums.Access.View))
                {
                    gvGroupConfig.Columns[3].Visible = btnAdd.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.Edit);
                   
                    Master.Heading = "Group Configuration";
                    loadData(Master.UserSession.CurrentUser.CustomerID);
                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }

        }

        private void loadData(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.GroupConfig> grpConfigList = ECN_Framework_BusinessLayer.Communicator.GroupConfig.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);
            gvGroupConfig.DataSource = grpConfigList;
            gvGroupConfig.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string isPublic = isPublicChkbox.Checked ? "Y" : "N";
            ECN_Framework_Entities.Communicator.GroupConfig grpConfig = new ECN_Framework_Entities.Communicator.GroupConfig();
            grpConfig.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            grpConfig.CustomerID = Master.UserSession.CurrentUser.CustomerID;
            grpConfig.ShortName = txtShortName.Text.Trim().Replace(" ", "_");
            grpConfig.IsPublic = isPublic;
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GroupConfig.Save(grpConfig, Master.UserSession.CurrentUser);
                loadData(Master.UserSession.CurrentUser.CustomerID);
                isPublicChkbox.Checked = false;
                txtShortName.Text = string.Empty;
            }
            catch (ECNException ecn)
            {
                setECNError(ecn);
                return;
            }
        }

        protected void gvGroupConfig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int GroupConfigID = Convert.ToInt32(e.CommandArgument.ToString());
            ECN_Framework_BusinessLayer.Communicator.GroupConfig.Delete(GroupConfigID, Master.UserSession.CurrentUser);
            loadData(Master.UserSession.CurrentUser.CustomerID);
        }

    }
}
