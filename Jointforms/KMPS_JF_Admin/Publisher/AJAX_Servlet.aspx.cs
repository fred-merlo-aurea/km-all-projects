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

namespace KMPS_JF_Setup.Publisher
{
    public partial class AJAX_Servlet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["function"] != null)
            {
                if (Request.QueryString["function"] == "saveNavigatorState")
                    SaveNavigatorState();
            }
        }

        private void SaveNavigatorState()
        {        
            ArrayList arrlst = new ArrayList();

            arrlst.Add(Request.QueryString["FormId"]);            
            arrlst.Add(Request.QueryString["v1_FormName"]);
            arrlst.Add(Request.QueryString["v1_Description"]);
            //arrlst.Add(Request.QueryString["v2_ResType"]);
            //arrlst.Add(Request.QueryString["v2_FromName"]);
            //arrlst.Add(Request.QueryString["v2_FromEmail"]);
            //arrlst.Add(Request.QueryString["v2_EmailSubject"]);
            //arrlst.Add(Request.QueryString["v2_EmailBody"]);
            //arrlst.Add(Request.QueryString["v2_NewsSubject"]);
            //arrlst.Add(Request.QueryString["v2_NewsBody"]);
            //arrlst.Add(Request.QueryString["v2_AdminEmail"]);
            //arrlst.Add(Request.QueryString["v2_AdminSubject"]);
            //arrlst.Add(Request.QueryString["v2_AdminBody"]);

            Session["Entry"] = arrlst;
        }
    }
}
