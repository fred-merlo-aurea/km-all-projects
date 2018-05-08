using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.ListServices
{
    public partial class GetSubscriberStatus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ListManager_PROD.ListManager listManager = new ListManager_PROD.ListManager();

            try
            {
                txtOutputXml.Text = listManager.GetSubscriberStatus(txtAccessKey.Text, txtEmailAddress.Text);
            }
            catch (Exception ex)
            {
                txtOutputXml.Text = ex.Message;
            }
        }
    }
}