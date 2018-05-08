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
using System.IO;
using System.Configuration;

namespace ecn.wizard.Reports
{
    /// <summary>
    /// Summary description for ReportingOpens.
    /// </summary>
    public partial class ReportingOpens : ecn.wizard.MasterPage
    {
        protected ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
        public int BlastID = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["id"].ToString() != string.Empty)
            {
                BlastID = Convert.ToInt32(Request.QueryString["id"]);
            }

            // Populate First DataGrid
            PopulateMostActiveOpens();

            // Populate Second DataFrid
            PopulateListOfOpens();
        }

        private void PopulateMostActiveOpens()
        {
            //string sql =
            //    " SELECT TOP 15 " +
            //    " COUNT(eal.emailID) AS ActionCount, e.emailaddress as EmailAddress, e.FirstName, e.LastName, e.Voice AS Phone " +
            //    " FROM  Emails e JOIN EmailActivityLog eal ON eal.emailid=e.emailid JOIN Blasts b ON eal.BlastID = b.BlastID " +
            //    " WHERE eal.blastid=" + BlastID + " AND eal.ActionTypeCode='open' " +
            //    " group by eal.emailid, e.emailaddress, e.FirstName, e.LastName, e.Voice " +
            //    " order by ActionCount desc ";

            string sql =
                " SELECT TOP 15 " +
                " COUNT(baop.emailID) AS ActionCount, e.emailaddress as EmailAddress, e.FirstName, e.LastName, e.Voice AS Phone " +
                " FROM  ecn5_communicator..Emails e JOIN BlastActivityOpens baop ON baop.EmailID = e.EmailID " +
                " JOIN ecn5_communicator..Blasts b ON baop.BlastID = b.BlastID WHERE baop.BlastID = " + BlastID +
                " group by e.emailid, e.emailaddress, e.FirstName, e.LastName, e.Voice " +
                " order by ActionCount desc ";

            DataTable dt = DataFunctions.GetDataTable(sql, DataFunctions.con_activity);
            dgActive.DataSource = dt;
            dgActive.DataBind();
        }

        private void PopulateListOfOpens()
        {
            //string sql =
            //    " SELECT e.EmailAddress, eal.actionvalue as ActionValue, eal.ActionDate as OpenTime, e.FirstName, e.LastName, e.Voice AS Phone " +
            //    " FROM Emails e JOIN EmailActivityLog eal ON e.EMailID=eal.EMailID JOIN Blasts b ON eal.BlastID = b.BlastID  " +
            //    " WHERE b.BlastID=" + BlastID + " AND eal.ActionTypeCode='open' " +
            //    " ORDER BY ActionDate DESC";   

            string sql =
                " SELECT e.EmailAddress, baop.BrowserInfo as ActionValue, baop.OpenTime, e.FirstName, e.LastName, e.Voice AS Phone " +
                " FROM ecn5_communicator..Emails e JOIN BlastActivityOpens baop ON e.EmailID = baop.EMailID JOIN ecn5_communicator..Blasts b ON baop.BlastID = b.BlastID " +
                " WHERE b.BlastID = " + BlastID +
                " ORDER BY baop.OpenTime DESC";

            DataTable dt = DataFunctions.GetDataTable(sql, DataFunctions.con_activity);
            dgOpens.DataSource = dt.DefaultView;
            dgOpens.DataBind();

            if (dt.Rows.Count > 0)
                OpensPager.RecordCount = dt.Rows.Count;
            else
                OpensPager.Visible = false;
        }

        private void dgOpens_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            dgOpens.CurrentPageIndex = e.NewPageIndex;
            PopulateListOfOpens();
        }


        private void btnDl_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string newline = "";

            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;
            DataTable emailstable = new DataTable();

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + ChannelID + "/downloads/");

            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            string downloadType = ddlDLType.SelectedItem.Value.ToString();
            //string downloadSQL =
            //    " SELECT eal.ActionDate as OpenTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, e.Voice as Phone" +
            //    " FROM EmailActivityLog eal, Emails e " +
            //    " WHERE BlastID=" + BlastID +
            //    " AND ActionTypeCode='open' " +
            //    " AND eal.EMailID=e.EMailID " +
            //    " ORDER BY ActionDate DESC";

            string downloadSQL =
                " SELECT baop.OpenTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, e.Voice as Phone " +
                " FROM BlastActivityOpens baop join ecn5_communicator..Emails e on e.EmailID = baop.EmailID WHERE baop.BlastID = " + BlastID +
                " ORDER BY OpenTime DESC ";

            //output txt file format <customerID>-<blastID>-open-emails.<downloadType>
            DateTime date = DateTime.Now;
            string tfile = txtoutFilePath + "open-emails" + downloadType;
            TextWriter txtfile = File.AppendText(tfile);

            columnHeadings.Insert(0, "OpenTime");
            columnHeadings.Insert(1, "EmailAddress");
            columnHeadings.Insert(2, "FirstName");
            columnHeadings.Insert(3, "LastName");
            columnHeadings.Insert(4, "Phone");

            aListEnum = columnHeadings.GetEnumerator();
            while (aListEnum.MoveNext())
            {
                newline += aListEnum.Current.ToString() + ", ";
            }
            txtfile.WriteLine(newline);

            // get the data from the database 
            // reset the IEnumerator Object of the ArrayList so tha the pointer is set.
            emailstable = DataFunctions.GetDataTable(downloadSQL, DataFunctions.con_activity);
            foreach (DataRow dr in emailstable.Rows)
            {
                newline = "";
                aListEnum.Reset();
                while (aListEnum.MoveNext())
                {
                    newline += dr[aListEnum.Current.ToString()].ToString() + ", ";
                }
                txtfile.WriteLine(newline);
            }
            txtfile.Close();

            Response.ContentType = "text";
            Response.AddHeader("content-disposition", "attachment; filename=opens" + downloadType);
            Response.WriteFile(tfile);
            Response.Flush();
            Response.End();

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
            this.btnDl.Click += new System.Web.UI.ImageClickEventHandler(this.btnDl_Click);
            this.dgOpens.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgOpens_PageIndexChanged);
            this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
            this.btnReportMenu.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnToReportingMenu_Click);

        }
        #endregion

        private void ibtnToReportingMenu_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("ReportingMain.aspx");
        }

        private void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("ReportingMsgDetail.aspx?ID=" + BlastID);
        }
    }
}
