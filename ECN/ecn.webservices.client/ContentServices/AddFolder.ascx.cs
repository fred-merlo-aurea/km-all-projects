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
    public partial class AddFolder : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                //ContentManager_localhost.ContentManager ws = new ecn.webservices.client.ContentManager_localhost.ContentManager();
                ContentManager_PROD.ContentManager ws = new ecn.webservices.client.ContentManager_PROD.ContentManager();
                if (txtParentFolder.Text.Trim().Length > 0)
                {
                    txtReturn.Text = ws.AddFolder(txtAccessKey.Text.Trim(), txtFolder.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt32(txtParentFolder.Text.Trim()));
                }
                else
                {
                    txtReturn.Text = ws.AddFolder(txtAccessKey.Text.Trim(), txtFolder.Text.Trim(), txtDescription.Text.Trim());
                }
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}