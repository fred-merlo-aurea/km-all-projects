using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ecn.communicator.main.admin.SubscriptionManagement
{
    public partial class SubscriptionManagementList : System.Web.UI.Page
    {
        private static List<ECN_Framework_Entities.Accounts.SubscriptionManagement> listSM;
        protected void Page_Load(object sender, EventArgs e)
        {

            phError.Visible = false;
            if (!Page.IsPostBack)
            {
                listSM = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);
                if (listSM.Count > 0)
                {
                    pnlNoPages.Visible = false;
                    pnlMain.Visible = true;

                    gvPages.DataSource = listSM;
                    gvPages.DataBind();
                }
                else
                {
                    pnlNoPages.Visible = true;
                    pnlMain.Visible = false;
                }
            }
        }

        protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Accounts.SubscriptionManagement sm = (ECN_Framework_Entities.Accounts.SubscriptionManagement)e.Row.DataItem;

                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEditPage");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeletePage");
                Label lblURL = (Label)e.Row.FindControl("lblSMURL");

                lblURL.Text = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/subscriptionmanagement.aspx?smid=" + sm.SubscriptionManagementID.ToString() + "&e=%%EmailAddress%%";
                imgbtnEdit.CommandArgument = sm.SubscriptionManagementID.ToString();
                imgbtnDelete.CommandArgument = sm.SubscriptionManagementID.ToString();
            }
        }

        protected void gvPages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("editpage"))
            {
                Response.Redirect("SubscriptionManagementEdit.aspx?smid=" + e.CommandArgument);
            }
            else if (e.CommandName.ToLower().Equals("deletepage"))
            {
                if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    //Delete page
                    int SMID = -1;
                    int.TryParse(e.CommandArgument.ToString(), out SMID);

                    if (SMID > 0)
                    {
                        try
                        {
                            ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.Delete(SMID, Master.UserSession.CurrentUser);
                            Response.Redirect("SubscriptionManagementList.aspx");
                        }
                        catch (ECNException ex)
                        {
                            setECNError(ex);
                            return;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Redit", "alert('You do not have permission to delete');", true);
                    
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

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnCreateNewSMPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/admin/subscriptionmanagement/subscriptionmanagementEdit.aspx");
        }
    }
}