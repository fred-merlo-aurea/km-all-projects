using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using KMPS_JF_Objects.Objects;

namespace KMPS_JF.Forms
{
    public partial class ViewPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Theme = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int PCPID = Convert.ToInt32(Request.QueryString["PCPID"].ToString());

                    SqlCommand cmddt = new SqlCommand("select PageName, PageHTML from PubCustomPages where IsActive=1 and PCPID= @PCPID");
                    cmddt.CommandType = CommandType.Text;
                    cmddt.Parameters.Add(new SqlParameter("@PCPID", PCPID));
                    DataTable dt = DataFunctions.GetDataTable(cmddt);

                    if (dt.Rows.Count > 0)
                    {
                        Page.Title = dt.Rows[0]["PageName"].ToString();
                        Response.Write(dt.Rows[0]["PageHTML"].ToString()); 
                    }
                }
                catch { }
            }
        }
    }
}
