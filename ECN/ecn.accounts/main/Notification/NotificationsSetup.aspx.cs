using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using HtmlAgilityPack;
using Telerik.Web.UI;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using AccountsEntity = ECN_Framework_Entities.Accounts;

namespace ecn.accounts.main.Notification
{
    public partial class NotificationsSetup : ECN_Framework.WebPageHelper
    {
        private int getNotificationID()
        {
            int theNotificationID = 0;
            try
            {
                theNotificationID = Convert.ToInt32(Request.QueryString["NotificationID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theNotificationID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION;
            Master.SubMenu = "notifications";

            lblErrorMessage.Text = "";
            phError.Visible = false;
          //  texteditor.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + Master.UserSession.CurrentUser.CustomerID.ToString() + "/images" };
            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                Response.Redirect("/ecn.accounts/main/default.aspx");
            }

            if (!IsPostBack)
            {
                int notificationID = getNotificationID(); 
                if (notificationID > 0)
                {
                    Master.Heading = "Edit Notification";
                    LoadFormData(notificationID);
                }
                else
                    Master.Heading = "Create Notification";
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "jquery", "enabledtpicker();", true);
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

        #region Data Load
        private void LoadFormData(int notificatinID)
        {
            try
            {
                ECN_Framework_Entities.Accounts.Notification notification = ECN_Framework_BusinessLayer.Accounts.Notification.GetByNotificationID(notificatinID);

                txtName.Text = notification.NotificationName;
                txtStartDate.Text = notification.StartDate;
                txtStartTime.Text = notification.StartTime;
                txtEndDate.Text = notification.EndDate;
                txtEndTime.Text = notification.EndTime;

                if (notification.BackGroundColor != null && notification.CloseButtonColor != null)
                {
                    RadColorPicker1.SelectedColor = Color.FromName(notification.BackGroundColor);
                    RadColorPicker2.SelectedColor = Color.FromName(notification.CloseButtonColor);
                    colorOne.Text = notification.BackGroundColor;
                    colorTwo.Text = notification.CloseButtonColor;
                }
                else
                {
                    //RadColorPicker1.SelectedColor = Color.FromName("#045da4");
                    //RadColorPicker2.SelectedColor = Color.FromName("f47e1f");
                    colorOne.Text = "#045da4";
                    colorTwo.Text = "f47e1f";
                }
                

                string tSource = "";
                tSource = notification.NotificationText;
                tSource = tSource.Replace("<TBODY>", "");
                tSource = tSource.Replace("</TBODY>", "");
                texteditor.Text = tSource;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }
        #endregion

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Notification, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        public void btnSave_click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime startDate = new DateTime();
                DateTime.TryParse(txtStartDate.Text, out startDate);
                DateTime endDate = new DateTime();
                DateTime.TryParse(txtEndDate.Text, out endDate);

                if (startDate < DateTime.Now.Date)
                {
                    throwECNException("Start Date cannot be in the past");
                    return;
                }
                else if (startDate == DateTime.Now.Date)
                {
                    TimeSpan ts = new TimeSpan();
                    TimeSpan.TryParse(txtStartTime.Text, out ts);
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    if (ts < currentTime)
                    {
                        throwECNException("Send Time cannot be in the past");
                        return;
                    }
                } 
                else if (endDate < DateTime.Now.Date)
                {
                    throwECNException("End Date cannot be in the past");
                    return;
                }
                else if (endDate < startDate)
                {
                    throwECNException("End Date cannot come before the Start Date");
                    return;
                }

                if (startDate == endDate)
                {
                    DateTime startTime = new DateTime();
                    DateTime.TryParse(txtStartTime.Text, out startTime);
                    DateTime endTime = new DateTime();
                    DateTime.TryParse(txtEndTime.Text, out endTime);
                    if (startTime == endTime)
                    {
                        throwECNException("Start Time and End Time can not be the same");
                        return;    
                    }
                    if (startTime>endTime)
                    {
                        throwECNException("Start Time can not be after the End Time");
                        return;    
                    }
                }

                ECN_Framework_Entities.Accounts.Notification notification = new ECN_Framework_Entities.Accounts.Notification();
                notification.NotificationID = getNotificationID();
                notification.NotificationName = txtName.Text;
                notification.StartDate = txtStartDate.Text;
                notification.StartTime = txtStartTime.Text;
                notification.EndDate = txtEndDate.Text;
                notification.EndTime = txtEndTime.Text;
                notification.BackGroundColor = System.Drawing.ColorTranslator.ToHtml(RadColorPicker1.SelectedColor);
                notification.CloseButtonColor = System.Drawing.ColorTranslator.ToHtml(RadColorPicker2.SelectedColor);

                string tsource = ECN_Framework_Common.Functions.StringFunctions.CleanString(texteditor.Text);
                tsource = tsource.Replace("<TBODY>", "");
                tsource = tsource.Replace("</TBODY>", "");
                notification.NotificationText = tsource;

                if (notification.NotificationID > 0)
                    notification.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                else
                    notification.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                ECN_Framework_BusinessLayer.Accounts.Notification.Save(notification, Master.UserSession.CurrentUser);

                Response.Redirect("Notifications.aspx");
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        public void btnCancel_click(object sender, System.EventArgs e)
        {
            Response.Redirect("Notifications.aspx");
        }

        public void btnPreview_click(object sender, System.EventArgs e)
        {
            string userInput = WebUtility.HtmlEncode(texteditor.Text);


            //Method now checks for link query parameters that are reserved 11/19/2013 JWelter
            List<ECNError> errorList = new List<ECNError>();
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(userInput);
            try
            {

                HtmlNodeCollection allImages = doc.DocumentNode.SelectNodes("//img[@src!='']");
            }
            catch
            {
                
            }
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//[@src]");
            System.Collections.Generic.List<string> linkList = new System.Collections.Generic.List<string>();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["src"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//area[@src]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["src"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }


            AccountsEntity.Notification notification = AccountsBLL.Notification.GetByCurrentDateTime();
            if (notification != null)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "toastrNotification(\"" + userInput.Replace("\"", "'") + "\");", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "toastrNotification(\"" + notification.NotificationName + "\",\"" + notification.NotificationText.Replace("\"","'") + "\");", true);

            //lblNotifiationName.Text = txtName.Text;
            //lblPreview.Text =  texteditor.Text;
            //mdlPreview.Show();
        }

        public void btnClose_Click(object sender, System.EventArgs e)
        {
            lblPreview.Text = "";
            mdlPreview.Hide();
        }
    }
}