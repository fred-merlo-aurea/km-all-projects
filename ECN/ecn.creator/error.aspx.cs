using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using ecn.common.classes;

namespace ecn.creator
{

    public partial class error : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
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
            else
            {
                lblErrorMsg.Text = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError);
            }
        }

    }
}
