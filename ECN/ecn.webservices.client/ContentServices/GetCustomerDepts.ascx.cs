using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ecn.webservices.client.ContentServices
{
    public partial class GetCustomerDepts : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                ContentManager_PROD.ContentManager ws = new ecn.webservices.client.ContentManager_PROD.ContentManager();
                txtReturn.Text = ws.GetCustomerDepts(txtAccessKey.Text.Trim());
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}