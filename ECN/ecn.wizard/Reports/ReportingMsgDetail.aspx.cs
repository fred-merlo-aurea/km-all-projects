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
using System.Configuration;

namespace ecn.wizard.Reports
{
    /// <summary>
    /// Summary description for ReportingMsgDetail.
    /// </summary>
    public partial class ReportingMsgDetail : ecn.wizard.MasterPage
    {
        public int BlastID = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["id"].ToString() != string.Empty)
            {
                BlastID = Convert.ToInt32(Request.QueryString["id"]);
            }

            lnkOpen.NavigateUrl = "ReportingOpens.aspx?ID=" + BlastID;
            lnkClick.NavigateUrl = "ReportingClicks.aspx?ID=" + BlastID;
            lnkBounce.NavigateUrl = "ReportingBounces.aspx?ID=" + BlastID;
            lnkForward.NavigateUrl = "ReportingForwards.aspx?ID=" + BlastID;
            lnkUnsubscribe.NavigateUrl = "ReportingUnsubscribes.aspx?ID=" + BlastID;

            // Show Details
            PopulateData();
        }

        private void PopulateData()
        {

            if (BlastID != -1)
            {
                string[] report_domains = new string[] { "msn.com", "aol.com", "excite.com", "yahoo.com" };

                Decimal SendTotal = 0;
                Decimal Success = 0;

                string sql =
                    "SELECT  " +
                    "'LayoutName' = CASE  " +
                    "WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName  ELSE '< CAMPAIGN DELETED >'  END, " +
                    "b.EmailSubject, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal " +
                    "FROM Blasts b JOIN Layouts l on b.LayoutID = l.LayoutID " +
                    "WHERE b.BlastID = " + BlastID;

                DataTable grpNmDT = DataFunctions.GetDataTable(sql);
                foreach (DataRow dr in grpNmDT.Rows)
                {
                    message.Text = dr["LayoutName"].ToString();
                    mail_subject.Text = dr["EmailSubject"].ToString();
                    mail_from.Text = dr["EmailFromName"].ToString() + " &lt;" + dr["EmailFrom"].ToString() + "&gt;";
                    send_time.Text = dr["SendTime"].ToString();
                    finish_time.Text = dr["FinishTime"].ToString();
                }

                string sendTotalSql = string.Format("select count(emailID) from BlastActivitySends where BlastID = {0}", BlastID);
                SendTotal = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", sendTotalSql));

                string bounceTotalSql = string.Format("select count(distinct emailID) from BlastActivityBounces where BlastID = {0} ", BlastID);
                int bounce = Convert.ToInt32(DataFunctions.ExecuteScalar("activity", bounceTotalSql));
                Success = SendTotal - bounce;

                Decimal clicksuniquecount = 0.0M;

                Decimal opensuniquecount = 0.0M;

                Decimal bouncesuniquecount = 0.0M;

                Decimal forwardsuniquecount = 0.0M;

                Decimal unsubscribeuniquecount = 0.0M;

                //CLICKS
                string sqlCLICKS =
                    " SELECT COUNT(DISTINCT bacl.EmailID) as 'DistinctEmailCount', count(bacl.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityClicks bacl JOIN ecn5_communicator..Emails e on e.EmailID = bacl.EmailID " +
                    " WHERE bacl.BlastID = " + BlastID.ToString();

                DataTable clicksDT = DataFunctions.GetDataTable(sqlCLICKS, DataFunctions.con_activity);
                foreach (DataRow dr in clicksDT.Rows)
                {
                    clicksuniquecount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    clicks_unique.Text = dr["DistinctEmailCount"].ToString();
                    lnkClick.Text = dr["EmailCount"].ToString();
                }

                //OPENS 
                //string sqlOPENS =
                //    " select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' " +
                //    " FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  " +
                //    " WHERE eal.BlastID = " + BlastID + "  AND eal.ActionTypeCode='open' ";

                string sqlOPENS =
                    " SELECT COUNT(DISTINCT baop.EmailID) as 'DistinctEmailCount', count(baop.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityOpens baop JOIN ecn5_communicator..Emails e on e.EmailID = baop.EmailID " +
                    " WHERE baop.BlastID = " + BlastID.ToString();

                DataTable opensDT = DataFunctions.GetDataTable(sqlOPENS, DataFunctions.con_activity);
                foreach (DataRow dr in opensDT.Rows)
                {
                    opensuniquecount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    opens_unique.Text = dr["DistinctEmailCount"].ToString();
                    lnkOpen.Text = dr["EmailCount"].ToString();
                }

                //BOUNCES
                //string sqlBOUNCES =
                //    " select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' " +
                //    " FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  " +
                //    " WHERE eal.BlastID = " + BlastID + "  AND eal.ActionTypeCode='bounce' ";

                string sqlBOUNCES =
                    " select COUNT(DISTINCT babo.EmailID) as 'DistinctEmailCount', count(babo.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityBounces babo JOIN ecn5_communicator..Emails e on e.EmailID = babo.EmailID " +
                    " WHERE babo.BlastID = " + BlastID.ToString();

                DataTable bouncesDT = DataFunctions.GetDataTable(sqlBOUNCES, DataFunctions.con_activity);
                foreach (DataRow dr in bouncesDT.Rows)
                {
                    bouncesuniquecount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    bounces_unique.Text = dr["DistinctEmailCount"].ToString();
                    lnkBounce.Text = dr["EmailCount"].ToString();
                }

                //string sqlUNsubscribes =
                //    " select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' " +
                //    " FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  " +
                //    " WHERE eal.BlastID = " + BlastID + "  AND eal.ActionTypeCode='subscribe' ";

                string sqlUNsubscribes =
                    " select COUNT(DISTINCT baus.EmailID) as 'DistinctEmailCount', count(baus.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityUnSubscribes baus JOIN ecn5_communicator..Emails e on e.EmailID = baus.EmailID " +
                    " join UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID " +
                    " WHERE baus.BlastID = " + BlastID + " and usc.UnsubscribeCode = 'subscribe'";

                DataTable unsubscribeDT = DataFunctions.GetDataTable(sqlUNsubscribes, DataFunctions.con_activity);
                foreach (DataRow dr in unsubscribeDT.Rows)
                {
                    unsubscribeuniquecount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    unsubscribe_unique.Text = dr["DistinctEmailCount"].ToString();
                    lnkUnsubscribe.Text = dr["EmailCount"].ToString();
                }

                //FORWARDS
                //string sqlFORWARDS =
                //    " select COUNT(DISTINCT eal.EmailID) as 'DistinctEmailCount', count(eal.EmailID) as 'EmailCount' " +
                //    " FROM EmailActivityLog eal JOIN Emails e on e.EmailID = eal.EmailID  " +
                //    " WHERE eal.BlastID = " + BlastID + "  AND eal.ActionTypeCode='refer' ";

                string sqlFORWARDS =
                    " SELECT COUNT(DISTINCT barf.EmailID) as 'DistinctEmailCount', count(barf.EmailID) as 'EmailCount' " +
                    " FROM BlastActivityRefer barf JOIN ecn5_communicator..Emails e on e.EmailID = barf.EmailID " +
                    " WHERE barf.BlastID = " + BlastID;

                DataTable forwardDT = DataFunctions.GetDataTable(sqlFORWARDS, DataFunctions.con_activity);
                foreach (DataRow dr in forwardDT.Rows)
                {
                    forwardsuniquecount = Convert.ToDecimal(dr["DistinctEmailCount"].ToString());
                    forward_unique.Text = dr["DistinctEmailCount"].ToString();
                    lnkForward.Text = dr["EmailCount"].ToString();
                }

                //set percentages
                success_rate.Text = "(" + Decimal.Round(((SendTotal == 0 ? 0 : Success / SendTotal) * 100), 0).ToString() + "%)";
                clicks_percent.Text = Decimal.Round(((SendTotal == 0 ? 0 : clicksuniquecount / SendTotal) * 100), 0).ToString() + " %";
                bounces_percent.Text = Decimal.Round(((SendTotal == 0 ? 0 : bouncesuniquecount / SendTotal) * 100), 0).ToString() + " %";
                opens_percent.Text = Decimal.Round(((SendTotal == 0 ? 0 : opensuniquecount / SendTotal) * 100), 0).ToString() + " %";
                //				SubscribesPercentage.Text=Decimal.Round(((SendTotal==0?0:subscribesuniquecount/SendTotal)*100),0).ToString()+" %";
                //				ResendsPercentage.Text=Decimal.Round(((SendTotal==0?0:resendsuniquecount/SendTotal)*100),0).ToString()+" %";
                forward_percent.Text = Decimal.Round(((SendTotal == 0 ? 0 : forwardsuniquecount / SendTotal) * 100), 0).ToString() + " %";
                unsubscribe_percent.Text = Decimal.Round(((SendTotal == 0 ? 0 : unsubscribeuniquecount / SendTotal) * 100), 0).ToString() + " %";
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);

        }
        #endregion


        private void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("ReportingMain.aspx");
        }


    }
}
