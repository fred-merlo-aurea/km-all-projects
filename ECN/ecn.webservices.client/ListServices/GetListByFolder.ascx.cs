using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.ListServices
{
    public partial class GetListByFolder : System.Web.UI.UserControl
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
                //ContentManager_localhost.ContentManager ws = new ecn.webservices.client.ContentManager_localhost.ContentManager();
                txtReturn.Text = ws.GetListsByFolderID(txtAccessKey.Text.Trim(), Convert.ToInt32(txtFolderID.Text.Trim()));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}