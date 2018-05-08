using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.PharmaliveServices
{
    public partial class GetSubscribedNewsletters : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                PharmaliveServices_PROD.PharmaliveServices ws = new ecn.webservices.client.PharmaliveServices_PROD.PharmaliveServices();
                txtReturn.Text = ws.getSubcribedNewsletters(txtAccessKey.Text.Trim(), txtEmailAddress.Text.Trim());
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}