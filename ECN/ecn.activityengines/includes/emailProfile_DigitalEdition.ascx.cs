using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.common.classes;
using AjaxControlToolkit;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes
{
    public partial class emailProfile_DigitalEdition : EmailProfileBaseControl
    {
        DataTable emailDigEdActivityDT = null;
        DataTable digEdActivityDT = null;
        private string _emailId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return messageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "emailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");

            digEdActivityDT = new DataTable();
            digEdActivityDT.Columns.Add("EditionID");
            digEdActivityDT.Columns.Add("BlastID");
            digEdActivityDT.Columns.Add("EmailSubject");
            digEdActivityDT.Columns.Add("SendTime");
            digEdActivityDT.Columns.Add("PageNumber");
            digEdActivityDT.Columns.Add("ActionCount");
            digEdActivityDT.Columns.Add("ActionTypeCode");
            digEdActivityDT.Columns.Add("ActionValue");

            loadDigEdActivityDT();
            loadDigEDDetailsForEmailProfile();
		}

        private void loadDigEDDetailsForEmailProfile( ){
            string sqlQuery =   "SELECT DISTINCT ed.EditionID, ed.EditionName, ed.Pages, p.PublicationID, p.PublicationName, p.PublicationCode, "+
                                "   (SELECT CONVERT(decimal(18,2),ROUND(AVG(inn.seconds)/60.00,2)) "+
                                "   FROM (SELECT SessionID, DATEDIFF(ss, MIN(ActionDate), MAX(ActionDate)) AS 'Seconds' "+
                                "       FROM "+ConfigurationManager.AppSettings["publisherdb"]+"..Editionactivitylog "+
                                "           WHERE EditionID = ed.EditionID and isdeleted = 0 AND ISNULL(SessionID, '') <> '' AND emailID = "+_emailId+" GROUP BY SessionID) inn) AS 'AvgTimeSpent', "+
                                " ('http://images.ecn5.com/customers/'+CONVERT(VARCHAR,p.CustomerID)+'/publisher/'+CONVERT(VARCHAR,ed.EditionID)+'/150/1.png') AS 'ThumbNailImg', "+
                                " ('http://images.ecn5.com/customers/'+CONVERT(VARCHAR,p.CustomerID)+'/publisher/'+CONVERT(VARCHAR,ed.EditionID)+'/450/1.jpg') AS 'ThumbNailImg450'" +
                                " FROM 	" + ConfigurationManager.AppSettings["publisherdb"] + "..EditionActivityLog edal " +
                                "    JOIN " + ConfigurationManager.AppSettings["publisherdb"] + "..Edition ed ON ed.EditionID = edal.EditionID " +
                                "    JOIN " + ConfigurationManager.AppSettings["publisherdb"] + "..Publication p ON p.PublicationID = ed.PublicationID" +
                                " WHERE EmailID = "+ _emailId + " and edal.isdeleted = 0 and ed.isdeleted = 0 and p.isdeleted = 0";

            digEdDataList.DataSource = DataFunctions.GetDataTable(sqlQuery);
            digEdDataList.DataBind();
        }

        protected void digEdDataList_ItemDataBound( object sender, DataListItemEventArgs e ) {
            //BWare.UI.Web.WebControls.DataPanel DataPanel1 = (BWare.UI.Web.WebControls.DataPanel)e.Item.FindControl("DataPanel1");
           // DataPanel1.Collapsed = false;
            AjaxControlToolkit.TabContainer activityTabsContainer = (TabContainer)e.Item.FindControl("activityTabsContainer");
            DataGrid visitActivityGrid = (DataGrid)activityTabsContainer.Tabs[0].FindControl("visitActivityGrid");
            DataGrid clickActivityGrid = (DataGrid)activityTabsContainer.Tabs[1].FindControl("clickActivityGrid");
            DataGrid referralsActivityTabGrid = (DataGrid)activityTabsContainer.Tabs[2].FindControl("referralsActivityTabGrid");
            DataGrid subscriptionsActivityGrid = (DataGrid)activityTabsContainer.Tabs[3].FindControl("subscriptionsActivityGrid");
            DataGrid printsActivityGrid = (DataGrid)activityTabsContainer.Tabs[4].FindControl("printsActivityGrid");
            DataGrid searchesActivityGrid = (DataGrid)activityTabsContainer.Tabs[5].FindControl("searchesActivityGrid");


            DataRow[] visitActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] +"' AND ActionTypeCode = 'visit' ");
            foreach(DataRow dr in visitActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            visitActivityGrid.DataSource = digEdActivityDT.DefaultView;
            visitActivityGrid.DataBind();

            digEdActivityDT.Clear();
            DataRow[] clickActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] + "' AND ActionTypeCode = 'click' ");
            foreach(DataRow dr in clickActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            clickActivityGrid.DataSource = digEdActivityDT.DefaultView;
            clickActivityGrid.DataBind();

            digEdActivityDT.Clear();
            DataRow[] referralActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] + "' AND ActionTypeCode = 'refer' ");
            foreach(DataRow dr in referralActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            referralsActivityTabGrid.DataSource = digEdActivityDT.DefaultView;
            referralsActivityTabGrid.DataBind();

            digEdActivityDT.Clear();
            DataRow[] subscribeActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] + "' AND ActionTypeCode = 'subscribe' ");
            foreach(DataRow dr in subscribeActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            subscriptionsActivityGrid.DataSource = digEdActivityDT.DefaultView;
            subscriptionsActivityGrid.DataBind();

            digEdActivityDT.Clear();
            DataRow[] printActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] + "' AND ActionTypeCode = 'print' ");
            foreach(DataRow dr in printActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            printsActivityGrid.DataSource = digEdActivityDT.DefaultView;
            printsActivityGrid.DataBind();

            digEdActivityDT.Clear();
            DataRow[] searchActivityDRs = emailDigEdActivityDT.Select("EditionID = '" + (int)digEdDataList.DataKeys[(int)e.Item.ItemIndex] + "' AND ActionTypeCode = 'search' ");
            foreach(DataRow dr in searchActivityDRs) {
                digEdActivityDT.ImportRow(dr);
            }
            digEdActivityDT.AcceptChanges();
            searchesActivityGrid.DataSource = digEdActivityDT.DefaultView;
            searchesActivityGrid.DataBind();

        }

        #region Load the Campaign Activity Data Table
        private void loadDigEdActivityDT() {
            emailDigEdActivityDT = new DataTable();
            string emailDigEdActivitySQL = 
                                    " SELECT ed.EditionID,b.BlastID, (CASE WHEN ISNULL(b.EmailSubject,'') = '' THEN '[DIRECT MAGAZINE ACCESS]' ELSE b.EmailSubject END) AS 'EmailSubject', " +
                                    " b.SendTime, p.PageNumber, COUNT(eal.PageID) AS 'ActionCount', ed.EditionName, ActionTypeCode, ActionValue " +
                                    " FROM " + ConfigurationManager.AppSettings["publisherdb"] + "..EditionActivityLog eal " +
                                    " LEFT OUTER JOIN Blast b ON eal.BlastID = b.BlastID " +
                                    " LEFT OUTER JOIN Emails e ON eal.EmailID = e.EmailID " +
                                    " JOIN " + ConfigurationManager.AppSettings["publisherdb"] + "..Edition ed on ed.EditionID = eal.EditionID " +
                                    " LEFT OUTER JOIN " + ConfigurationManager.AppSettings["publisherdb"] + "..Page p on eal.PageID = p.PageID " +
                                    " WHERE eal.EmailID = " + _emailId + " and eal.IsDeleted = 0 and b.StatusCode <> 'Deleted' and ed.Isdeleted = 0 and p.isdeleted = 0 " + //" AND eal.EditionID = " + digEdID +
                                    " GROUP BY ed.EditionID, b.BlastID, b.EmailSubject, b.SendTime, p.PageNumber, eal.PageID, ed.EditionName, ActionTypeCode, ActionValue";

            emailDigEdActivityDT = DataFunctions.GetDataTable(emailDigEdActivitySQL);
        }
        #endregion

        #region Load Campaign Activity Grid by Action (OPEN / SEND / CLICK etc.,)
        /*private void loadDigEdActivityByAction( string activityType ) {
            if(activityType.Length == 0) {
                activityType = "visit";
            }
            DataTable selectedRowsDT = new DataTable();
            selectedRowsDT.Columns.Add("EmailSubject");
            selectedRowsDT.Columns.Add("SendTime");
            selectedRowsDT.Columns.Add("PageNumber");
            selectedRowsDT.Columns.Add("ActionValue");
            selectedRowsDT.Columns.Add("ActionCount");

            if(activityType.Equals("visit")) {
                visitActivityGrid.Columns[2].Visible = true;
                visitActivityGrid.Columns[3].Visible = false;
                visitActivityGrid.Columns[4].Visible = true;
            } else if(activityType.Equals("click")) {
                clickActivityGrid.Columns[2].Visible = false;
                clickActivityGrid.Columns[3].Visible = true;
                clickActivityGrid.Columns[4].Visible = false;
                clickActivityGrid.Columns[3].HeaderText = "Click URL";
            } else if(activityType.Equals("refer")) {
                referActivityGrid.Columns[2].Visible = false;
                referActivityGrid.Columns[3].Visible = true;
                referActivityGrid.Columns[4].Visible = false;
                referActivityGrid.Columns[3].HeaderText = "Referral EmailAddress";
            } else if(activityType.Equals("subscribe")) {
                subscribeActivityGrid.Columns[2].Visible = false;
                subscribeActivityGrid.Columns[3].Visible = true;
                subscribeActivityGrid.Columns[4].Visible = false;
                subscribeActivityGrid.Columns[3].HeaderText = "Subscription Action";
            } else if(activityType.Equals("print")) {
                printActivityGrid.Columns[2].Visible = false;
                printActivityGrid.Columns[3].Visible = true;
                printActivityGrid.Columns[4].Visible = false;
                printActivityGrid.Columns[3].HeaderText = "Print Pages";
            } else if(activityType.Equals("search")) {
                searchActivityGrid.Columns[2].Visible = false;
                searchActivityGrid.Columns[3].Visible = true;
                searchActivityGrid.Columns[4].Visible = false;
                searchActivityGrid.Columns[3].HeaderText = "Search Text";
            }

            if(digEdActivityDT.Rows.Count > 0) {
                digEdActivityGrid.Visible = true;
                messageLabel.Visible = false;
                DataRow[] drs = digEdActivityDT.Select("ActionTypeCode = '" + activityType + "'");

                foreach(DataRow dr in drs) {
                    selectedRowsDT.ImportRow(dr);
                }

                digEdActivityGrid.DataSource = selectedRowsDT.DefaultView;
                digEdActivityGrid.DataBind();
            } else {
                digEdActivityGrid.Visible = false;
                messageLabel.Visible = true;
                messageLabel.Text = "No Data available to display";
            }
        }*/
        #endregion


        #region reset Styles on all the LinkButtons
        /*protected void resetStyles() {
            btnVisit.CssClass = "";
            btnClick.CssClass = "";
            btnForward.CssClass = "";
            btnSubscribe.CssClass = "";
            btnPrint.CssClass = "";
            btnSearch.CssClass = "";
        }*/
        #endregion

        /*protected void btnVisit_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("visit");
        }

        protected void btnClick_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("click");
        }

        protected void btnForward_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("refer");
        }

        protected void btnSubscribe_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("subscribe");
        }

        protected void LinkPrint_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("print");
        }

        protected void LinkSearch_Click( object sender, EventArgs e ) {
            loadDigEdActivityByAction("search");
        }*/
    }
}