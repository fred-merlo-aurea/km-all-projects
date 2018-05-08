using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ECNCommon = ECN_Framework_Common.Objects;

namespace ecn.accounts.main.users
{
    public partial class EditUserProfile : System.Web.UI.Page
    {
        private static KMPlatform.Entity.User _currentUser;
        private static string RedirectURL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes["type"] = "password";
            txtNewPassword.Attributes["type"] = "password";
            txtConfirmPassword.Attributes["type"] = "password";
            phError.Visible = false;
            if (!Page.IsPostBack)
            {
                try
                {
                    RedirectURL = Request.QueryString["redirecturl"].ToString();
                }
                catch { }
                LoadUser(Master.UserSession.CurrentUser);
            }
        }

        public string SaveProfile()
        {
            if (Validate())
            {
                KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
                bool forceLogIn = false;
                string newPasswordString = string.Empty;

                if (newPassword.Visible)
                {
                    if (!txtNewPassword.Text.Trim().Equals(txtConfirmPassword.Text.Trim()))
                    {
                        throwECNException("Passwords must match.", phError, lblErrorMessagePhError);
                        return "validation";
                    }
                    else
                        newPasswordString = txtNewPassword.Text.Trim();
                }
                else
                    newPasswordString = _currentUser.Password.Trim();

                if (!_currentUser.UserName.Trim().Equals(txtUserName.Text.Trim()) || !_currentUser.Password.Trim().Equals(newPasswordString))
                {
                    if (!_currentUser.UserName.Equals(txtUserName.Text.Trim()))
                    {
                        if (uWorker.Validate_UserName(txtUserName.Text.Trim(),_currentUser.UserID))
                        {
                            throwECNException("UserName already exists", phError, lblErrorMessagePhError);
                            return "validation";
                        }
                    }
                    forceLogIn = true;
                }

                _currentUser.UserName = txtUserName.Text.Trim();
                _currentUser.Password = newPasswordString;
                _currentUser.FirstName = txtFirstName.Text.Trim();
                _currentUser.LastName = txtLastName.Text.Trim();
                _currentUser.Phone = txtPhone.Text.Trim();
                _currentUser.EmailAddress = txtEmail.Text.Trim();
                _currentUser.DefaultClientGroupID = Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString());
                _currentUser.DefaultClientID = Convert.ToInt32(ddlCustomer.SelectedValue.ToString());

                Master.UserSession.CurrentUser.FirstName = txtFirstName.Text.Trim();
                Master.UserSession.CurrentUser.LastName = txtLastName.Text.Trim();
                
                
                uWorker.Save(_currentUser);

                if (forceLogIn)
                {
                    return "redirect";
                }
                else
                {
                    return "noredirect";
                }
            }
            else
                return "validation";

        }

        private bool Validate()
        {
            rfvEmail.Validate();
            rfvFirstName.Validate();
            rfvLastName.Validate();
            //rfvPassword.Validate();
            rfvUserName.Validate();

            if (!rfvEmail.IsValid)
                return false;
            if (!rfvFirstName.IsValid)
                return false;
            if (!rfvLastName.IsValid)
                return false;
            //if (!rfvPassword.IsValid)
            //    return false;
            if (!rfvUserName.IsValid)
                return false;
            if (newPassword.Visible)
            {
                rfvNewPassword.Validate();
                rfvConfirmPassword.Validate();
                if (!rfvNewPassword.IsValid)
                    return false;
                if (!rfvConfirmPassword.IsValid)
                    return false;
            }

            return true;
        }

        public void LoadUser(KMPlatform.Entity.User user)
        {
            KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
            _currentUser = uWorker.SelectUser(user.UserID,true);
            

            txtUserName.Text = _currentUser.UserName;
            txtEmail.Text = _currentUser.EmailAddress;
            txtFirstName.Text = _currentUser.FirstName;
            txtLastName.Text = _currentUser.LastName;
            string maskedPass = string.Empty;
            for (int i = 0; i < _currentUser.Password.Length; i++)
            {
                maskedPass += "*";
            }
            txtPassword.Text = maskedPass;
            txtPhone.Text = _currentUser.Phone;
            lblAccessKey.Text = _currentUser.AccessKey.ToString();

            ddlBaseChannel.DataSource = _currentUser.ClientGroups;
            ddlBaseChannel.DataTextField = "ClientGroupName";
            ddlBaseChannel.DataValueField = "ClientGroupID";
            ddlBaseChannel.DataBind();

            ddlBaseChannel.SelectedValue = _currentUser.DefaultClientGroupID.ToString();
            LoadCustomers(true);
        }

        protected void ddlBaseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers(false);

        }

        private void LoadCustomers(bool initalLoad)
        {

            List<KMPlatform.Entity.Client> cList = new List<KMPlatform.Entity.Client>();
            KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
            if (KM.Platform.User.IsSystemAdministrator(_currentUser))
            {
                cList = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroup(Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()));
            }
            else
            {
                cList = cWorker.SelectbyUserIDclientgroupID(_currentUser.UserID, Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString()));
            }

            ddlCustomer.DataSource = cList;
            ddlCustomer.DataTextField = "ClientName";
            ddlCustomer.DataValueField = "ClientID";
            ddlCustomer.DataBind();

            if (initalLoad)
            {
                ddlCustomer.SelectedValue = _currentUser.DefaultClientID.ToString();
            }
            else
            {
                ddlCustomer.SelectedIndex = 0;
            }
        }

        protected void btnSaveEditProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string result = SaveProfile();

                if (result.Equals("redirect"))
                {
                    Response.Redirect("/EmailMarketing.Site/Login/Logout", false);
                }
                else if (result.Equals("noredirect"))
                {
                    Response.Redirect(RedirectURL, false);
                }
                else
                {
                    //validation error
                }
            }
            catch(Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EditUserProfile.SaveProfile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                Response.Redirect("~/error.aspx?E=HardError", false);
            }
        }

        protected void btnCancelEditProfile_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(RedirectURL))
            {
                Response.Redirect(RedirectURL, false);
            }
            else
            {
                Response.Redirect("/ecn.accounts/main", false);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if(!newPassword.Visible)
            {
                newPassword.Visible = true;
                confirmPassword.Visible = true;
            }
            else
            {
                newPassword.Visible = false;
                confirmPassword.Visible = false;
            }
        }

        private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            ECNCommon.ECNError ecnError = new ECNCommon.ECNError(ECNCommon.Enums.Entity.User, ECNCommon.Enums.Method.Get, message);
            List<ECNCommon.ECNError> errorList = new List<ECNCommon.ECNError> { ecnError };
            setECNError(new ECNCommon.ECNException(errorList, ECNCommon.Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void setECNError(ECNCommon.ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECNCommon.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

    }
}