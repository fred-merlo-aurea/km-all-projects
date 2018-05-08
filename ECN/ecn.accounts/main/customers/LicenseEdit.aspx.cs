using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

using SecurityAccess = ECN_Framework.Common.SecurityAccess; 

namespace ecn.accounts.customersmanager
{
    public partial class LicenseEdit : ECN_Framework.WebPageHelper
    {
        int requestCLID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icocustomers.gif><b>Unsent Emails</b><br />These are the emails you wrote or started writing but have not sent. You can also edit an email before you send it, Click the edit link. To send the email, first set the groups you want to recieve this Blast. </p>&#13;&#10;&#9;&#9;<p><b>Sent Emails</b><br />These emails are stored in your database and are available to view and/or send again. </p>&#13;&#10;&#9;&#9;<p><b>Helpful Hint</b><br />To send an email again, first 'view' the email and while viewing the email click 'write new email' link in the navigation. All you have to do is select the layout you want, rename it and click the preview email button.</p>&#13;&#10;&#9;&#9;";
            Master.HelpTitle = "Customer Manager";
            lblErrorMessage.Text = "";
            phError.Visible = false;

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                requestCLID = getCLID();
                if (requestCLID == 0)
                {
                    Response.Redirect("../default.aspx");
                }
                if (!IsPostBack)
                {
                    if (LoadValues())
                    {
                        SetupView();
                    }
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }
        }

        public void btnUpdate_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Accounts.CustomerLicense customerLicense = ECN_Framework_BusinessLayer.Accounts.CustomerLicense.GetByCLID(requestCLID, Master.UserSession.CurrentUser);

            customerLicense.Quantity = Convert.ToInt32(tbQuantity.Text);
            customerLicense.AddDate = adtAddDate.Date;
            customerLicense.ExpirationDate = adtExpirationDate.Date;
            customerLicense.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

            try
            {
                ECN_Framework_BusinessLayer.Accounts.CustomerLicense.Save(customerLicense, Master.UserSession.CurrentUser);
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
           
            Response.Redirect("customerdetail.aspx?CustomerID=" + customerLicense.CustomerID);
        }

        private void SetupView()
        {

            if (Master.UserSession.CurrentUser.IsKMStaff)
            {
                //if (staff.LicenseUpdateFlag)
                //{
                    tbQuantity.Enabled = true;
                    btnUpdate.Enabled = true;
                //}
                //else
                //{
                //    tbQuantity.Enabled = false;
                //    btnUpdate.Enabled = false;
                //}
            }
            else
            {
                tbQuantity.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private bool LoadValues()
        {
            try
            {
                ECN_Framework_Entities.Accounts.CustomerLicense customerLicense = ECN_Framework_BusinessLayer.Accounts.CustomerLicense.GetByCLID(requestCLID, Master.UserSession.CurrentUser);

                lblCustomerName.Text = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerLicense.CustomerID, false).CustomerName;
                tbQuantity.Text = customerLicense.Quantity.ToString();
                adtAddDate.Date = Convert.ToDateTime(customerLicense.AddDate);
                adtExpirationDate.Date = Convert.ToDateTime(customerLicense.ExpirationDate);
                return true;
            }
            catch (Exception)
            {
                pnlViewEdit.Visible = false;
                lblError.Text = "Error loading license values";
                pnlError.Visible = true;
                return false;
            }
        }

        private int getCLID()
        {
            int theCLID = 0;
            try
            {
                theCLID = Convert.ToInt32(Request.QueryString["CLID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theCLID;
        }

        public void Cancel(object sender, EventArgs e)
        {
            int customerID = ECN_Framework_BusinessLayer.Accounts.CustomerLicense.GetByCLID(requestCLID, Master.UserSession.CurrentUser).CustomerID;

            if (customerID > 0)
            {
                Response.Redirect("customerdetail.aspx?CustomerID=" + customerID.ToString());
            }
        }
    }
}
