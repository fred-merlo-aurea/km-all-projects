using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.ListServices
{
	public partial class GetFilters : System.Web.UI.UserControl
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
                //ListManager_localhost.ListManager ws = new ecn.webservices.client.ListManager_localhost.ListManager();
                txtReturn.Text = ws.GetFilters(txtAccessKey.Text.Trim(), Convert.ToInt32(txtListID.Text.Trim()));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
	}
}