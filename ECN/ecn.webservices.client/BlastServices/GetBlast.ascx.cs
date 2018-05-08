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

namespace ecn.webservices.client.BlastServices
{
    public partial class GetBlast : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                BlastManager_PROD.BlastManager ws = new ecn.webservices.client.BlastManager_PROD.BlastManager();
                //BlastManager_localhost.BlastManager ws = new ecn.webservices.client.BlastManager_localhost.BlastManager();
                txtReturn.Text = ws.GetBlast(txtAccessKey.Text.Trim(), Convert.ToInt32(txtBlast.Text.Trim()));
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }

        }
    }
}