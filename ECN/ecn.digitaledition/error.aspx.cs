using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.digitaledition
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string error = "HardError";
            try
            {
                error = Request.QueryString["E"].ToString();
            }
            catch { }

            if (error == "InvalidLink")
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink);
            }
            else if (error == "PageNotFound")
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound);
            }
            else if (error == "Timeout")
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.Timeout);
            }
            else if(error == "SecurityError")
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.SecurityError);
            }
            else
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError);
            }
        }
    }
}