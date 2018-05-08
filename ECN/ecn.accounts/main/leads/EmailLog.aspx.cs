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
using ecn.common.classes;

namespace ecn.accounts.main.leads
{
    public partial class EmailLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            dgdEmailLog.DataSource = ECN_Framework_DataLayer.DataFunctions.GetDataTable(string.Format("select * from EmailActivityLog where BlastID = {0} and EmailID = {1}", BlastID, EmailID), DataFunctions.GetConnectionString("activity")); 
            dgdEmailLog.DataBind();
        }

        int EmailID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["EmailID"]);
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        int BlastID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["BlastID"]);
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
}