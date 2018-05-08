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
using System.Data.SqlClient; 

namespace KMPS_JF_Setup.Publisher
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {
        string _currentmenu = string.Empty;

        public string CurrentMenu
        {
            set
            {
                _currentmenu = value;
            }
            get
            {
                return _currentmenu;
            }
        }
        string _pubCode = string.Empty;
        public string pubCode
        {
            set
            {
                _pubCode = value;
            }
            get
            {
                return _pubCode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["PubName"].ToString().Trim() != "")
                {
                    pubCode = Request.QueryString["PubName"].ToString();
                }
            }
            catch
            {
                SqlCommand cmdpubcode = new SqlCommand("select pubcode from Publications p where p.PubID = @PubId");
                cmdpubcode.Parameters.Add(new SqlParameter("@PubId", Request.QueryString["PubId"].ToString())); 
                pubCode = DataFunctions.ExecuteScalar(cmdpubcode).ToString();   
            }
        }
    }
}