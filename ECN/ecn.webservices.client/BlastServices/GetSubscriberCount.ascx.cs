using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.BlastServices
{
    public partial class GetSubscriberCount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                BlastManager_PROD.BlastManager ws = new ecn.webservices.client.BlastManager_PROD.BlastManager();
                //BlastManager_LOCAL.BlastManager ws = new ecn.webservices.client.BlastManager_LOCAL.BlastManager();
                txtReturn.Text = ws.GetSubscriberCount(txtAccessKey.Text.Trim(), Convert.ToInt32(txtList.Text.Trim()));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}