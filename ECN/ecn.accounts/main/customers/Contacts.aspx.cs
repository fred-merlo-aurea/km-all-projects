using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using CommonFunctions = ECN_Framework_Common.Functions;
using AccountEntity = ECN_Framework_Entities.Accounts;
using AccountBLL = ECN_Framework_BusinessLayer.Accounts;

namespace ecn.accounts.main.customers
{
    public partial class Contacts : ECN_Framework.WebPageHelper
    {
        private int ContactID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["ID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int CustomerID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["CustomerID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int reload
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["reload"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        private ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            phError.Visible = false;

            btnClose.Attributes.Add("onclick", "javascript:closewindow(0);");

            if (UserSession.CurrentUser.IsKMStaff) 
            {
                if (!IsPostBack)
                {
                    if (ContactID > 0)
                    {
                        btnSave.Text = "Update";

                        AccountEntity.CustomerContact customerContact = AccountBLL.CustomerContact.GetByContactID(ContactID, UserSession.CurrentUser);

                        if (customerContact != null)
                        {
                            tbFirstName.Text = customerContact.FirstName;
                            tbLastName.Text = customerContact.LastName;
                            tbAddress.Text = customerContact.Address;
                            tbAddress2.Text = customerContact.Address2;
                            tbCity.Text = customerContact.City;
                            ddlState.ClearSelection();
                            ddlState.Items.FindByValue(customerContact.State).Selected = true;
                            tbZip.Text = customerContact.Zip;
                            tbPhone.Text = customerContact.Phone;
                            tbMobile.Text = customerContact.Mobile;
                            tbEmail.Text = customerContact.Email;
                        }
                    }
                }
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AccountEntity.CustomerContact customerContact = new AccountEntity.CustomerContact();
            customerContact.ContactID = ContactID;
            customerContact.CustomerID = CustomerID;
            customerContact.FirstName = tbFirstName.Text.Replace("'", "''");
            customerContact.LastName = tbLastName.Text.Replace("'", "''");
            customerContact.Address = tbAddress.Text.Replace("'","''");
            customerContact.Address2 = tbAddress2.Text.Replace("'","''");
            customerContact.City = tbCity.Text.Replace("'","''");
            customerContact.State = ddlState.SelectedItem.Value;
            customerContact.Zip = tbZip.Text.Replace("'","''") ;
            customerContact.Phone = tbPhone.Text.Replace("'","''");
            customerContact.Mobile = tbMobile.Text.Replace("'","''"); 
            customerContact.Email = tbEmail.Text.Replace("'","''");

            if (customerContact.ContactID > 0)
                customerContact.UpdatedUserID = Convert.ToInt32(UserSession.CurrentUser.UserID);
            else
                customerContact.CreatedUserID = Convert.ToInt32(UserSession.CurrentUser.UserID);

            try
            {
                AccountBLL.CustomerContact.Save(customerContact, UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            if (ContactID > 0)
            {
                Response.Write("<script>window.opener.location.reload();self.close();</script>");
            }
            else
            {
                if (reload == 1)
                    Response.Write("<script>window.opener.location.reload();self.close();</script>");
                else
                    Response.Write("<script>self.close();</script>");
            }
        }
    }
}
