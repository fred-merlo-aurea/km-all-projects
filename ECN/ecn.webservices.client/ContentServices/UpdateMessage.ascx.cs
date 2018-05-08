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
    public partial class UpdateMessage : System.Web.UI.UserControl
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
                txtReturn.Text = ws.UpdateMessage(txtAccessKey.Text.Trim(), txtLayout.Text.Trim(), txtTableBorder.Text.Trim(), Convert.ToInt32(txtTemplate.Text.Trim()), txtAddress.Text.Trim(), Convert.ToInt32(txtDept.Text.Trim()), Convert.ToInt32(txtContent1.Text.Trim()), Convert.ToInt32(txtContent2.Text.Trim()), Convert.ToInt32(txtContent3.Text.Trim()), Convert.ToInt32(txtContent4.Text.Trim()), Convert.ToInt32(txtContent5.Text.Trim()), Convert.ToInt32(txtContent6.Text.Trim()), Convert.ToInt32(txtContent7.Text.Trim()), Convert.ToInt32(txtContent8.Text.Trim()), Convert.ToInt32(txtContent9.Text.Trim()), Convert.ToInt32(txtMessageID.Text.Trim()));
                
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}