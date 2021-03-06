﻿using System;
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
    public partial class AddContentToFolder : System.Web.UI.UserControl
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
                //if (txtFolder.Text.Trim().Length > 0)
                //{
                    txtReturn.Text = ws.AddContent(txtAccessKey.Text.Trim(), txtTitle.Text.Trim(), txtContentHTML.Text, txtContentText.Text, Convert.ToInt32(txtFolder.Text.Trim()), Convert.ToBoolean(txtWYSIWYG.Text.Trim()));
                //}
                //else
                //{
                //    txtReturn.Text = ws.AddContent(txtAccessKey.Text.Trim(), txtTitle.Text.Trim(), txtContentHTML.Text, txtContentText.Text);
                //}
            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}