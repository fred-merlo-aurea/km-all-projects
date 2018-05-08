using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ecn.accounts.main.Notification
{
    public partial class Notifications : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION;

            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                Response.Redirect("/ecn.accounts/main/default.aspx");
            }

            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void rgNotifications_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            try
            {
                rgNotifications.DataSource = ECN_Framework_BusinessLayer.Accounts.Notification.GetAll();
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void rgNotifications_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridDataItem item = e.Item as GridDataItem;
                ECN_Framework_BusinessLayer.Accounts.Notification.Delete(Convert.ToInt32(item.GetDataKeyValue("NotificationID")), Master.UserSession.CurrentUser);
                LoadNotifications();
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("NotificationsSetup.aspx");
        }
    }
}