using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.ContentServices
{
    public partial class GetLayoutByFolder : System.Web.UI.UserControl
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
                //ContentManager_localhost.ContentManager ws = new ecn.webservices.client.ContentManager_localhost.ContentManager();
                txtReturn.Text = ws.GetMessageListByFolderID(txtAccessKey.Text.Trim(), Convert.ToInt32(txtFolderID.Text.Trim()));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}