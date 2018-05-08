using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Principal;
using AjaxControlToolkit;
using System.Configuration;
using System.Data.SqlClient;
using KMPS.MD.Objects;


namespace KMPS.MD.MasterPages
{
    public partial class Login : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgLogo.ImageUrl = "~/Images/KM-logo.gif";
            }
        }
    }
}