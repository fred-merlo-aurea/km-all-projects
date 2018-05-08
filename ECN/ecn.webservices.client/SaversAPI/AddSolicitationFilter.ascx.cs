using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ecn.webservices.client.SaversAPI
{
    public partial class AddSolicitationFilter : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturn.Text = "";
                //ecn.webservices.client.SaversAPI.SaversAPI savers = new SaversAPI();
                //ecn.webservices.client.SaversAPI_LOCAL.SaversAPI savers = new ecn.webservices.client.SaversAPI_LOCAL.SaversAPI();
                ecn.webservices.client.SaversAPI_PROD.SaversAPI savers = new ecn.webservices.client.SaversAPI_PROD.SaversAPI();
               
                txtReturn.Text = savers.CreateWeeklySolicitationFilter(txtAccessKey.Text.Trim(), Convert.ToInt32(txtGroupID.Text.Trim()), txtStartDate.Text, txtEndDate.Text, txtZips.Text);
            }
            catch(Exception Ex)
            {
                txtReturn.Text = "Error: " + Ex.Message;
            }
        }
    }
}