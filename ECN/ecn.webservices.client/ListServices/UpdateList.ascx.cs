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

namespace ecn.webservices.client.ListServices
{
    public partial class UpdateList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                //ListManager_localhost.ListManager ws = new ecn.webservices.client.ListManager_localhost.ListManager();
                ListManager_PROD.ListManager ws = new ecn.webservices.client.ListManager_PROD.ListManager();
                if (txtFolder.Text.Trim().Length > 0)
                {
                    txtReturn.Text = ws.UpdateList(txtAccessKey.Text.Trim(), Convert.ToInt32(txtListID.Text.Trim()), txtList.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt32(txtFolder.Text.Trim()));
                }
                else
                {
                    txtReturn.Text = ws.UpdateList(txtAccessKey.Text.Trim(), Convert.ToInt32(txtListID.Text.Trim()), txtList.Text.Trim(), txtDescription.Text.Trim());
                }
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}