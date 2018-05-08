﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.webservices.client.WATTAPI
{
    public partial class GetNextTokenForSubscriber : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";

                //ecn.webservices.client.WATTAPI_PROD.WATTAPI wa = new ecn.webservices.client.WATTAPI_PROD.WATTAPI();
                ecn.webservices.client.WATTAPI_LOCAL.WATTAPI wa = new ecn.webservices.client.WATTAPI_LOCAL.WATTAPI();
                //com.ecn5.webservices.specialprojects.WATTAPI wa = new com.ecn5.webservices.specialprojects.WATTAPI();
                txtReturn.Text = wa.GetNextTokenForSubscriber(txtAccessKey.Text.Trim(), txtToken.Text.ToString(), Convert.ToInt32(txtIssueID.Text.Trim()));

            }
            catch (Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}