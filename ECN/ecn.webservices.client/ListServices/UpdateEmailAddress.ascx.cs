using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.ListServices
{
    public partial class UpdateEmailAddress : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                ListManager_PROD.ListManager ws = new ecn.webservices.client.ListManager_PROD.ListManager();
                txtReturn.Text = ws.UpdateEmailAddress(txtAccessKey.Text.Trim(), Convert.ToInt32(txtList.Text.Trim()), txtXMLString.Text.Trim(), txtOldEmail.Text.Trim(), txtNewEmail.Text, Convert.ToInt32(txtSFID.Text));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}