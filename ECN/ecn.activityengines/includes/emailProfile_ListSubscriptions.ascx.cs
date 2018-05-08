using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes {

    public partial class emailProfile_ListSubscriptions : EmailProfileBaseControl
    {
        private string _emailId = string.Empty;
        private string _customerId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return this.messageLabel;
            }
        }

        protected void Page_Load( object sender, EventArgs e )
        {
            _emailId = GetFromQueryString("eID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _customerId = GetFromQueryString("cID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            loadListSubscriptionsGrid();
        }

        #region Load List Subscriptions Grid
        private void loadListSubscriptionsGrid( ) {
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            var subListCmd = new SqlCommand(string.Format("SELECT RTRIM(LTRIM(g.GroupName)) AS 'ListName', g.GroupID AS 'ListID', (CASE 	WHEN eg.FormatTYpeCode IS NULL THEN '' ELSE eg.FormatTYpeCode END) AS 'Type', (CASE 	WHEN eg.SubscribeTypeCode = 'S' THEN '<img src=http://images.ecn5.com/images/tick.gif alt=\''Subscribed to this list\''>'	WHEN eg.SubscribeTypeCode = 'U' THEN '<img src=http://images.ecn5.com/images/unsubscribe.gif alt=\''Un-Subscribed to this list\''>' ELSE 	eg.SubscribeTypeCode END) AS 'Status', eg.CreatedOn AS 'DateSubscribed', eg.LastChanged AS 'DateModified' FROM Groups G JOIN EmailGroups eg ON eg.GroupID = g.GroupID AND EmailID = {0} JOIN emails e ON e.emailID = eg.emailID WHERE G.CUSTOMERid = {1} ORDER BY RTRIM(LTRIM(g.GroupName)) ASC", _emailId, _customerId), dbConn);

            SqlDataAdapter da = new SqlDataAdapter(subListCmd);
            DataSet ds = new DataSet();
            dbConn.Open();
            da.Fill(ds, "subscriptionsList");
            dbConn.Close();
            listSubscriptionsGrid.DataSource = ds.Tables[0];
            listSubscriptionsGrid.DataBind();
        }
        #endregion
    }
}