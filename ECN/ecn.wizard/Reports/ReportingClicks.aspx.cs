using System;
using System.Configuration;
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
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace ecn.wizard.Reports
{
    /// <summary>
    /// Summary description for ReportingClicks.
    /// </summary>
    public partial class ReportingClicks : ecn.wizard.MasterPage
    {
        protected ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
        public int BlastID = -1;        

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["id"].ToString() != string.Empty)
            {
                BlastID = Convert.ToInt32(Request.QueryString["id"]);
            }

            // Top Click Thorughs
            GetTopClickThroughs();

            // Top Visitors
            GetTopVisitors();

            // Top Click Throughs by Person
            GetClickThroughsByPerson();
        }

        private string getLinkAlias(string BlastID, String Link)
        {
            string sqlquery = " SELECT Alias FROM " +
                " Blasts b, Layouts l, Content c, linkAlias la " +
                " WHERE " +
                " b.blastID = " + BlastID + " AND b.layoutID = l.layoutID AND " +
                " (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR " +
                " l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR " +
                " l.ContentSlot9 = c.contentID) AND " +
                " la.ContentID = c.ContentID AND la.Link = '" + Link + "'";
            string alias = "";
            try
            {
                alias = DataFunctions.ExecuteScalar(sqlquery).ToString();
            }
            catch (Exception)
            {
                alias = "";
            }

            return alias;
        }

        private void GetTopClickThroughs()
        {
            SqlConnection dbConn = new SqlConnection(DataFunctions.con_activity.ToString());
            SqlCommand topClicksCmd = new SqlCommand("spClickActivity", dbConn);
            topClicksCmd.CommandTimeout = 100;
            topClicksCmd.CommandType = CommandType.StoredProcedure;

            topClicksCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            topClicksCmd.Parameters["@BlastID"].Value = BlastID;
            topClicksCmd.Parameters.Add(new SqlParameter("@HowMuch", SqlDbType.VarChar, 10));
            topClicksCmd.Parameters["@HowMuch"].Value = "";

            SqlDataAdapter da = new SqlDataAdapter(topClicksCmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "spClickActivity");
            dbConn.Close();

            DataTable dt = ds.Tables[0];

            DataTable newDT = new DataTable();
            newDT.Columns.Add(new DataColumn("ClickCount"));
            newDT.Columns.Add(new DataColumn("Url"));
            newDT.Columns.Add("LinkDetail", typeof(String));

            DataRow newDR;

            foreach (DataRow dr in dt.Rows)
            {
                string clickCount = dr["ClickCount"].ToString();

                string linkDetail = "BlastID=" + BlastID + "&link=" + HttpUtility.UrlEncode(dr["NewActionValue"].ToString());
                string fullLink = dr["NewActionValue"].ToString();
                string smallLink = dr["SmallLink"].ToString();
                string linkORalias = "";

                newDR = newDT.NewRow();
                newDR[0] = clickCount.ToString();
                //newDR[0]		= "<center><a href='clickslinks.aspx?"+linkDetail+"'>"+clickCount.ToString()+"</a></center>";
                string alias = getLinkAlias(BlastID.ToString(), fullLink);
                if (alias.Length > 0)
                {
                    linkORalias = alias;
                }
                else
                {
                    linkORalias = smallLink;
                }
                newDR[1] = linkORalias.ToString();
                //newDR[1]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";

                newDT.Rows.Add(newDR);
            }

            dgTopClicks.DataSource = newDT.DefaultView;
            dgTopClicks.DataBind();
        }

        private void GetTopVisitors()
        {
            //string sqlquery =
            //    " SELECT TOP 10 Count(eal.ActionValue) AS ClickCount, e.EmailAddress, e.FirstName, e.LastName, e.Voice as Phone " +
            //    " FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID JOIN Blasts b on eal.BlastID = b.BlastID " +
            //    " WHERE eal.ActionTypeCode='click' " +
            //    " AND eal.BlastID=" + BlastID +
            //    " GROUP BY e.EmailAddress, e.FirstName, e.LastName, e.Voice " +
            //    " ORDER BY ClickCount DESC, e.EmailAddress ";

            string sqlquery =
                " SELECT TOP 10 Count(bacl.URL) AS ClickCount, e.EmailAddress, e.FirstName, e.LastName, e.Voice as Phone " +
                " FROM ecn5_communicator..Emails e JOIN BlastActivityClicks bacl on e.EMailID = bacl.EMailID " +
                " JOIN ecn5_communicator..Blasts b on bacl.BlastID = b.BlastID " +
                " WHERE bacl.BlastID = " + BlastID +
                " GROUP BY e.EmailAddress, e.FirstName, e.LastName, e.Voice " +
                " ORDER BY ClickCount DESC, e.EmailAddress";

            DataTable dt = DataFunctions.GetDataTable("activity", sqlquery);
            dgTopVisitors.DataSource = dt;
            dgTopVisitors.DataBind();
        }

        private void GetClickThroughsByPerson()
        {
            //string sqlquery =
            //    " SELECT e.EmailAddress as EmailAddress, eal.ActionDate as ClickTime, e.FirstName, e.LastName, e.Voice as Phone,  " +
            //    " eal.ActionValue AS FullLink, " +
            //    " 'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL'," +
            //    " CASE WHEN LEN(eal.ActionValue) > 6 THEN LEFT(RIGHT(eal.ActionValue,LEN(eal.ActionValue)-7),40) ELSE eal.ActionValue END AS SmallLink" +
            //    " FROM Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID JOIN Blasts b on eal.BlastID = b.BlastID " +
            //    " WHERE eal.BlastID=" + BlastID +
            //    " AND ActionTypeCode='click' " +
            //    " ORDER BY ActionDate DESC";

            string sqlquery =
                " SELECT e.EmailAddress as EmailAddress, bacl.ClickTime, e.FirstName, e.LastName, e.Voice as Phone, " +
                " bacl.URL AS FullLink, 'EmailID='+CONVERT(VARCHAR,bacl.EmailID)+'&GroupID='+CONVERT(VARCHAR,b.GroupID) AS 'URL', " +
                " CASE WHEN LEN(bacl.URL) > 6 THEN LEFT(RIGHT(bacl.URL,LEN(bacl.URL)-7),0) ELSE bacl.URL END AS SmallLink " +
                " FROM ecn5_communicator..Emails e JOIN BlastActivityClicks bacl on e.EMailID = bacl.EMailID JOIN ecn5_communicator..Blasts b on bacl.BlastID = b.BlastID " +
                " WHERE bacl.BlastID = " + BlastID.ToString() +
                " ORDER BY ClickTime DESC";

            DataTable dt = DataFunctions.GetDataTable("activity", sqlquery);
            //ClicksGrid.DataSource=dt.DefaultView;
            //ClicksGrid.DataBind();

            DataTable newDT = new DataTable();
            newDT.Columns.Add(new DataColumn("ClickTime"));
            newDT.Columns.Add(new DataColumn("EmailAddress"));
            newDT.Columns.Add(new DataColumn("FirstName"));
            newDT.Columns.Add(new DataColumn("LastName"));
            newDT.Columns.Add(new DataColumn("Phone"));
            newDT.Columns.Add(new DataColumn("Url"));

            DataRow newDR;
            foreach (DataRow dr in dt.Rows)
            {
                string emailAdd = dr["EmailAddress"].ToString();
                string clickTime = dr["ClickTime"].ToString();
                string fullLink = dr["FullLink"].ToString();
                string smallLink = dr["SmallLink"].ToString();
                string url = dr["URL"].ToString();
                string linkORalias = "";

                newDR = newDT.NewRow();
                newDR[0] = clickTime;
                newDR[1] = emailAdd;
                newDR[2] = dr["FirstName"].ToString();
                newDR[3] = dr["LastName"].ToString();
                newDR[4] = dr["Phone"].ToString();
                string alias = getLinkAlias(BlastID.ToString(), fullLink);
                if (alias.Length > 0)
                {
                    linkORalias = alias;
                }
                else
                {
                    linkORalias = smallLink;
                }
                newDR[5] = linkORalias.ToString();
                //newDR[5]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";

                newDT.Rows.Add(newDR);
            }
            dgClicksPerPerson.DataSource = newDT.DefaultView;
            dgClicksPerPerson.DataBind();

            if (newDT.Rows.Count > 0)
            {
                ClicksPager.Visible = true;
                ClicksPager.RecordCount = newDT.Rows.Count;
            }
            else
                ClicksPager.Visible = false;
        }

        private void dgClicksPerPerson_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            dgClicksPerPerson.CurrentPageIndex = e.NewPageIndex;
            GetClickThroughsByPerson();
        }


        private void btnDownload_Click(object sender, System.Web.UI.ImageClickEventArgs e)
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
            //" SELECT eal.ActionDate as ClickTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, " +
            //" e.Voice as Phone, eal.ActionValue AS FullLink " +
            //" FROM EmailActivityLog eal, Emails e " +
            //" WHERE BlastID=" + BlastID +
            //" AND ActionTypeCode='click' " +
            //" AND eal.EMailID*=e.EMailID " +
            //" ORDER BY ActionDate DESC";

            string downloadSQL =
                " SELECT bacl.ClickTime, e.EmailAddress as EmailAddress, e.FirstName as FirstName, e.LastName as LastName, " +
                " e.Voice as Phone, bacl.URL AS FullLink " +
                " FROM BlastActivityClicks bacl join ecn5_communicator..Emails e ON bacl.EmailID = e.EmailID " +
                " WHERE bacl.BlastID = " + BlastID.ToString() +
                " ORDER BY ClickID DESC";

            //output txt file format <customerID>-<blastID>-open-emails.<downloadType>
            DateTime date = DateTime.Now;
            string tfile = txtoutFilePath + "click-emails" + downloadType;
            TextWriter txtfile = File.AppendText(tfile);

            columnHeadings.Insert(0, "ClickTime");
            columnHeadings.Insert(1, "EmailAddress");
            columnHeadings.Insert(2, "FirstName");
            columnHeadings.Insert(3, "LastName");
            columnHeadings.Insert(4, "Phone");
            columnHeadings.Insert(5, "FullLink");

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

            //create the zip file
            Response.ContentType = "text";
            Response.AddHeader("content-disposition", "attachment; filename=clicks" + downloadType);
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
            this.btnDownload.Click += new System.Web.UI.ImageClickEventHandler(this.btnDownload_Click);
            this.dgClicksPerPerson.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgClicksPerPerson_PageIndexChanged);
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
