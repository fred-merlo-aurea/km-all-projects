using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.AdvanstarServices
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                AdvanstarManager_PROD.AdvanstarServices ws = new ecn.webservices.client.AdvanstarManager_PROD.AdvanstarServices();
                txtReturn.Text = ws.Login(txtAccessKey.Text.Trim(), txtEmailAddress.Text.Trim());
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}