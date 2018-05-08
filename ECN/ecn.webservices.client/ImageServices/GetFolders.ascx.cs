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

namespace ecn.webservices.client.ImageServices
{
    public partial class GetFolders : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                //ImageManager_localhost.ImageManager ws = new ecn.webservices.client.ImageManager_localhost.ImageManager();
                ImageManager_PROD.ImageManager ws = new ecn.webservices.client.ImageManager_PROD.ImageManager();
                //ImageManager_Test.ImageManager ws = new ecn.webservices.client.ImageManager_Test.ImageManager();
                if (txtParentFolder.Text.Trim().Length > 0)
                {
                    txtReturn.Text = ws.GetFolders(txtAccessKey.Text.Trim(), txtParentFolder.Text.Trim());
                }
                else
                {
                    txtReturn.Text = ws.GetFolders(txtAccessKey.Text.Trim());
                }
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}