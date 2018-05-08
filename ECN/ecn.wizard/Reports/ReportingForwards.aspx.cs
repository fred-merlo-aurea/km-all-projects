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
using System.Configuration;


namespace ecn.wizard.Reports
{
    /// <summary>
    /// Summary description for ReportingForwards.
    /// </summary>
    public partial class ReportingForwards : ecn.wizard.MasterPage
    {
        public int BlastID = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["id"].ToString() != string.Empty)
            {
                BlastID = Convert.ToInt32(Request.QueryString["id"]);
            }
            // Populate Forwards data grid
            PopulateForwardsDataGrid();
        }

        private void PopulateForwardsDataGrid()
        {
            //string sqlquery=
            //    " SELECT eal.EMailID, e.EmailAddress, eal.ActionDate as ForwardTime, eal.ActionValue as Referral, '>>' as ForwardTo, "+
            //    " 'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL' "+
            //    " FROM Emails e JOIN EmailActivityLog eal ON e.EMailID=eal.EMailID JOIN Blasts b ON eal.BlastID = b.BlastID  "+
            //    " WHERE eal.BlastID="+ BlastID+
            //    " AND ActionTypeCode='refer' "+
            //    " ORDER BY ActionDate DESC";

            string sqlquery =
                " SELECT barf.EMailID, e.EmailAddress, barf.ReferTime as ForwardTime, barf.EmailAddress as Referral, '>>' as ForwardTo, " +
                " 'EmailID='+CONVERT(VARCHAR,barf.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL' " +
                " FROM ecn5_communicator..Emails e JOIN BlastActivityRefer barf ON e.EMailID = barf.EMailID " +
                " JOIN ecn5_communicator..Blasts b ON barf.BlastID = b.BlastID " +
                " WHERE barf.BlastID = " + BlastID +
                " ORDER BY barf.ReferTime DESC";

            DataTable dt = DataFunctions.GetDataTable(sqlquery, DataFunctions.con_activity);
            dgForwards.DataSource = dt.DefaultView;
            dgForwards.DataBind();
            if (dt.Rows.Count > 0)
                ForwardsPager.RecordCount = dt.Rows.Count;
            else
                ForwardsPager.Visible = false;
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
            this.btnReportMenu.Click += new System.Web.UI.ImageClickEventHandler(this.btnReportMenu_Click);

        }
        #endregion

        private void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("ReportingMsgDetail.aspx?ID=" + BlastID);
        }

        private void btnReportMenu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("ReportingMain.aspx");
        }
    }
}
