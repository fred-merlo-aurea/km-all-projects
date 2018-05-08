using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ecn.webservices.client.ListServices
{
    public partial class AddSubscribersWithDupes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument xml = new XmlDocument();
                //xml.Load("c:\\temp\\dupeSubscribers.xml");
                //string temp = xml.InnerXml;


                txtReturn.Text = "";
                //ListManager_localhost.ListManager ws = new ecn.webservices.client.ListManager_localhost.ListManager();
                ListManager_PROD.ListManager ws = new ecn.webservices.client.ListManager_PROD.ListManager();
                ws.Timeout = 600000;

                if (txtSmartFormID.Text.Trim().Length > 0 && Convert.ToInt32(txtSmartFormID.Text) > 0)
                    txtReturn.Text = ws.AddSubscribersWithDupesUsingSmartForm(txtAccessKey.Text.Trim(), Convert.ToInt32(txtList.Text.Trim()), txtSubscription.Text.Trim(), txtFormat.Text.Trim(), txtComposite.Text.Trim(), Convert.ToBoolean(txtOverwrite.Text.Trim()), txtXMLString.Text.Trim(), Convert.ToInt32(txtSmartFormID.Text));
                else
                    txtReturn.Text = ws.AddSubscribersWithDupes(txtAccessKey.Text.Trim(), Convert.ToInt32(txtList.Text.Trim()), txtSubscription.Text.Trim(), txtFormat.Text.Trim(), txtComposite.Text.Trim(), Convert.ToBoolean(txtOverwrite.Text.Trim()), txtXMLString.Text.Trim());
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}