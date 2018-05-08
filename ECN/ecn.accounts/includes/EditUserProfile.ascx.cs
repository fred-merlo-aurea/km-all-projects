using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.accounts.includes
{
    public partial class EditUserProfile : System.Web.UI.UserControl
    {
        private static KMPlatform.Entity.User _currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string SaveProfile()
        {
            if (Validate())
            {
                bool forceLogIn = false;

                if (!_currentUser.UserName.Trim().Equals(txtUserName.Text.Trim()) || !_currentUser.Password.Trim().Equals(txtPassword.Text.Trim()))
                {
                    forceLogIn = true;
                }

                _currentUser.UserName = txtUserName.Text.Trim();
                _currentUser.Password = txtPassword.Text.Trim();
                _currentUser.FirstName = txtFirstName.Text.Trim();
                _currentUser.LastName = txtLastName.Text.Trim();
                _currentUser.Phone = txtPhone.Text.Trim();
                _currentUser.EmailAddress = txtEmail.Text.Trim();
                _currentUser.DefaultClientGroupID = Convert.ToInt32(ddlBaseChannel.SelectedValue.ToString());
                _currentUser.DefaultClientID = Convert.ToInt32(ddlCustomer.SelectedValue.ToString());

                KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
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
            rfvPassword.Validate();
            rfvUserName.Validate();

            if (!rfvEmail.IsValid)
                return false;
            if (!rfvFirstName.IsValid)
                return false;
            if (!rfvLastName.IsValid)
                return false;
            if (!rfvPassword.IsValid)
                return false;
            if (!rfvUserName.IsValid)
                return false;

            return true;
        }

        public void LoadUser(KMPlatform.Entity.User user)
        {
            _currentUser = user;

            txtUserName.Text = user.UserName;
            txtEmail.Text = user.EmailAddress;
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtPassword.Text = user.Password;
            txtPhone.Text = user.Phone;

            ddlBaseChannel.DataSource = user.ClientGroups;
            ddlBaseChannel.DataTextField = "ClientGroupName";
            ddlBaseChannel.DataValueField = "ClientGroupID";
            ddlBaseChannel.DataBind();

            ddlBaseChannel.SelectedValue = user.DefaultClientGroupID.ToString();
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
    }
}