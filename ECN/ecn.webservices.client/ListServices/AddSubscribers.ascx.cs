using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ecn.webservices.client.ListServices
{
    public partial class AddSubscribers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument xml = new XmlDocument();
                //xml.Load("c:\\temp\\t1.xml");
                //string temp = xml.InnerXml;


                txtReturn.Text = "";
                //ListManager_localhost.ListManager ws = new ecn.webservices.client.ListManager_localhost.ListManager();
                ListManager_PROD.ListManager ws = new ecn.webservices.client.ListManager_PROD.ListManager();
                ws.Timeout = 600000;
                txtReturn.Text = ws.AddSubscribers(txtAccessKey.Text.Trim(), Convert.ToInt32(txtList.Text.Trim()), txtSubscription.Text.Trim(), txtFormat.Text.Trim(), txtXMLString.Text.Trim());
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}