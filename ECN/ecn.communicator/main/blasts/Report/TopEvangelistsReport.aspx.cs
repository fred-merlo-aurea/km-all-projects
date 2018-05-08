using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.Reporting.WebForms;
using ecn.communicator.CommonControls;
using ECN_Framework;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.blasts.Report
{
    public partial class TopEvangelistsReport : WebPageHelper
    {
        private DataSet m_dataSet;
        private MemoryStream m_rdl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.TopEvangelistsReport, KMPlatform.Enums.Access.View))
                {
                    DateTime date = DateTime.Now;
                    txtEndDate.Text = date.ToShortDateString();
                    txtStartDate.Text = date.AddMonths(-1).ToShortDateString();
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
            }
        }

        protected bool ValidateDates(TextBox tbStart, TextBox tbEnd)
        {
            int archiveYears = 1;
            if (Master.UserSession.CurrentBaseChannel.IsPublisher != null && (bool)Master.UserSession.CurrentBaseChannel.IsPublisher) { archiveYears = 2; }

            var dateValidator = new DateValidator();

            return dateValidator.ValidateDates(tbStart, tbEnd, archiveYears, phError, lblErrorMessage, true);
        }

        protected void btnLoadCampaignItems_Click(object sender, EventArgs e)
        {
            if (ValidateDates(txtStartDate, txtEndDate))
            {
                DataTable dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetSentCampaignItems(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, !string.IsNullOrWhiteSpace(txtStartDate.Text) ? Convert.ToDateTime(txtStartDate.Text) : DateTime.MinValue, !string.IsNullOrWhiteSpace(txtEndDate.Text) ? Convert.ToDateTime(txtEndDate.Text) : DateTime.Now, null, false, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                btnShowReport.Enabled = dt.DefaultView.Table.Rows.Count != 0;

                dt.DefaultView.Sort = "CampaignItemName asc";
                ddlCampaignItem.DataSource = dt.DefaultView;
                ddlCampaignItem.DataValueField = "CampaignItemID";
                ddlCampaignItem.DataTextField = "CampaignItemName";
                ddlCampaignItem.DataBind();
                lowerFilters.Style.Clear();
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            ReportGridPager.CurrentPage = 1;
            ReportGridPager.CurrentIndex = 0;
            ReportGrid.CurrentPageIndex = 0;
            LoadReport();
        }

        private void LoadReport()
        {
            DataTable resultsTbl = GetResultsTable();
            if (resultsTbl == null) return;

            ReportGrid.Columns.Add(new BoundColumn { DataField = "FixSubName", HeaderText = "Subscriber Name" });
            ReportGrid.Columns.Add(new BoundColumn { DataField = "EmailAddress", HeaderText = "Email Address" });
            ReportGrid.Columns.Add(new BoundColumn
            {
                DataField = "TotalNumberOfShares",
                HeaderText = "Total Number of Shares",
            });
            if (resultsTbl.Columns.Contains("FriendShares"))
            {
                ReportGrid.Columns.Add(new BoundColumn
                {
                    DataField = "FriendShares",
                    HeaderText = "Forward to a Friend"
                });
            }
            if (resultsTbl.Columns.Contains("Facebook"))
            {
                ReportGrid.Columns.Add(new BoundColumn
                {
                    DataField = "Facebook",
                    HeaderText = "Facebook"
                });
            }
            if (resultsTbl.Columns.Contains("Twitter"))
            {
                ReportGrid.Columns.Add(new BoundColumn
                {
                    DataField = "Twitter",
                    HeaderText = "Twitter"
                });
            }
            if (resultsTbl.Columns.Contains("LinkedIn"))
            {
                ReportGrid.Columns.Add(new BoundColumn
                {
                    DataField = "LinkedIn",
                    HeaderText = "LinkedIn"
                });
            }

            ReportGrid.DataSource = resultsTbl;
            ReportGrid.DataBind();
            ReportGridPager.RecordCount = resultsTbl.Rows.Count;
            lowerFilters.Style.Clear();  
            pnlReport.Visible = true;

            btnDownload.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.TopEvangelistsReport, KMPlatform.Enums.Access.Download);
            drpExport.Visible = btnDownload.Visible;
        }

        public class TopEvangelistsRow
        {
            public int EmailID { get; set; }
            public int Facebook { get; set; }
            public int Twitter { get; set; }
            public int LinkedIn { get; set; }
            public int FriendShares { get; set; }
            public string EmailAddress { get; set; }
            public string FixSubName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MergeCtrl { get; set; }
            public string TotalNumberOfShares { get; set; }
        }

        private DataTable GetResultsTable()
        {
            var campaignItemId = ddlCampaignItem.SelectedValue;
            var isFacebook = cbxFacebook.Checked;
            var isTwitter = cbxTwitter.Checked;
            var isLinkedIn = cbxLinkedIn.Checked;
            var isForwardToFriend = cbxForwardToFriend.Checked;

            if (!isFacebook && !isTwitter && !isLinkedIn && !isForwardToFriend)
            {
                throwECNException("Social Media type must be selected");
                return null;
            }

            var resultsTable = TopEvangelistsLists.Get(Convert.ToInt32(campaignItemId));
            resultsTable.Columns.Add(new DataColumn
            {
                ColumnName = "TotalNumberOfShares",
                DefaultValue = 0
            });

            return GetMergeDataTable(resultsTable, isFacebook, isTwitter, isLinkedIn, isForwardToFriend);
        }

        private DataTable GetMergeDataTable(
            DataTable resultsTable,
            bool isFacebook,
            bool isTwitter,
            bool isLinkedIn,
            bool isForwardToFriend)
        {
            var resultsDataView = new DataView(resultsTable);
            var mergeTable = resultsDataView.ToTable();
            mergeTable.Clear();

            var distinctValues = resultsDataView.ToTable(true, "EmailID");
            foreach (DataRow distinctRow in distinctValues.Rows)
            {
                var emailId = Convert.ToInt32(distinctRow["EmailID"].ToString());
                var teRowEmail = GetTopEvangelistsEmailRow(resultsTable, emailId);
                var teRowSocial = GetTopEvangelistsSocialRow(resultsTable, emailId);

                AddRowToMergeTable(mergeTable, teRowEmail, teRowSocial);
            }

            UpdateMergeTableBasedOnSocialMediaType(mergeTable, isFacebook, isTwitter, isLinkedIn, isForwardToFriend);
            return mergeTable;
        }

        private TopEvangelistsRow GetTopEvangelistsSocialRow(DataTable resultsTable, int emailId)
        {
            var topEvangelistSocialRow = new TopEvangelistsRow();
            var socialDataRow = resultsTable
                .AsEnumerable()
                .SingleOrDefault(r =>
                    r.Field<int>("EmailID") == emailId &&
                    r.Field<string>("MergeCtrl") == "SocialNetwork_Share");

            if (socialDataRow != null)
            {
                topEvangelistSocialRow.EmailID = Convert.ToInt32(socialDataRow["EmailID"].ToString());
                topEvangelistSocialRow.Facebook = Convert.ToInt32(socialDataRow["Facebook"].ToString());
                topEvangelistSocialRow.Twitter = Convert.ToInt32(socialDataRow["Twitter"].ToString());
                topEvangelistSocialRow.LinkedIn = Convert.ToInt32(socialDataRow["LinkedIn"].ToString());
                topEvangelistSocialRow.EmailAddress = socialDataRow["EmailAddress"].ToString();
                topEvangelistSocialRow.FirstName = socialDataRow["FirstName"].ToString();
                topEvangelistSocialRow.LastName = socialDataRow["LastName"].ToString();
                topEvangelistSocialRow.FixSubName = socialDataRow["FixSubName"].ToString();
            }

            return topEvangelistSocialRow;
        }

        private TopEvangelistsRow GetTopEvangelistsEmailRow(DataTable resultsTable, int emailId)
        {
            var topEvangelistEmailRow = new TopEvangelistsRow();

            var emailDataRow = resultsTable.AsEnumerable().SingleOrDefault(r =>
                r.Field<int>("EmailID") == emailId &&
                r.Field<string>("MergeCtrl") == "Email_Share");

            if (emailDataRow != null)
            {
                topEvangelistEmailRow.EmailID = Convert.ToInt32(emailDataRow["EmailID"].ToString());
                topEvangelistEmailRow.FriendShares = Convert.ToInt32(emailDataRow["FriendShares"].ToString());
                topEvangelistEmailRow.EmailAddress = emailDataRow["EmailAddress"].ToString();
                topEvangelistEmailRow.FirstName = emailDataRow["FirstName"].ToString();
                topEvangelistEmailRow.LastName = emailDataRow["LastName"].ToString();
                topEvangelistEmailRow.FixSubName = emailDataRow["FixSubName"].ToString();
            }

            return topEvangelistEmailRow;
        }

        private void UpdateMergeTableBasedOnSocialMediaType(
            DataTable mergeTable,
            bool isFacebook,
            bool isTwitter,
            bool isLinkedIn,
            bool isForwardToFriend)
        {
            RemoveSocialMediaColumnsFromMergeTable(mergeTable, isFacebook, isTwitter, isLinkedIn, isForwardToFriend);

            var columnsCollection = mergeTable.Columns;
            foreach (DataRow row in mergeTable.Rows)
            {
                var currentNumberOfShares = Convert.ToInt32(row["TotalNumberOfShares"].ToString());

                if (columnsCollection.Contains("Facebook"))
                {
                    currentNumberOfShares += Convert.ToInt32(row["Facebook"].ToString());
                }

                if (columnsCollection.Contains("Twitter"))
                {
                    currentNumberOfShares += Convert.ToInt32(row["Twitter"].ToString());
                }

                if (columnsCollection.Contains("LinkedIn"))
                {
                    currentNumberOfShares += Convert.ToInt32(row["LinkedIn"].ToString());
                }

                if (columnsCollection.Contains("FriendShares"))
                {
                    currentNumberOfShares += Convert.ToInt32(row["FriendShares"].ToString());
                }

                row["TotalNumberOfShares"] = currentNumberOfShares;
            }
        }

        private void RemoveSocialMediaColumnsFromMergeTable(
            DataTable mergeTable,
            bool isFacebook,
            bool isTwitter,
            bool isLinkedIn,
            bool isForwardToFriend)
        {
            if (!isFacebook)
            {
                mergeTable.Columns.Remove("Facebook");
            }

            if (!isTwitter)
            {
                mergeTable.Columns.Remove("Twitter");
            }

            if (!isLinkedIn)
            {
                mergeTable.Columns.Remove("LinkedIn");
            }

            if (!isForwardToFriend)
            {
                mergeTable.Columns.Remove("FriendShares");
            }
        }

        private void AddRowToMergeTable(
            DataTable mergeTable,
            TopEvangelistsRow topEvangelistEmailRow,
            TopEvangelistsRow topEvangelistSocialRow)
        {
            var newRow = mergeTable.NewRow();
            if (topEvangelistEmailRow.EmailID != 0)
            {
                newRow[0] = topEvangelistEmailRow.EmailID;
                newRow[1] = 0;
                newRow[2] = 0;
                newRow[3] = 0;
                newRow[4] = topEvangelistEmailRow.FriendShares;
                newRow[5] = topEvangelistEmailRow.EmailAddress;
                newRow[6] = topEvangelistEmailRow.FirstName;
                newRow[7] = topEvangelistEmailRow.LastName;
                newRow[8] = topEvangelistEmailRow.FixSubName;
                newRow[9] = string.Empty;
                newRow[10] = 0;
            }

            if (topEvangelistSocialRow.EmailID != 0)
            {
                newRow[0] = topEvangelistSocialRow.EmailID;
                newRow[1] = topEvangelistSocialRow.Facebook;
                newRow[2] = topEvangelistSocialRow.Twitter;
                newRow[3] = topEvangelistSocialRow.LinkedIn;
                if (string.IsNullOrWhiteSpace(newRow[4].ToString()))
                {
                    newRow[4] = 0;
                }

                newRow[5] = topEvangelistSocialRow.EmailAddress;
                newRow[6] = topEvangelistSocialRow.FirstName;
                newRow[7] = topEvangelistSocialRow.LastName;
                newRow[8] = topEvangelistSocialRow.FixSubName;
                newRow[9] = string.Empty;
                newRow[10] = 0;
            }

            mergeTable.Rows.Add(newRow);
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void setECNError(ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void ShowReport_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DynamicReport();
        }

        private void DynamicReport()
        {
            DataTable dataTable = GetResultsTable();
            dataTable.Columns[8].ColumnName = "SubscriberName";

            DataColumnCollection columns = dataTable.Columns;
            if (columns.Contains("TotalNumberOfShares"))
            {
                dataTable.Columns["TotalNumberOfShares"].ColumnName = "TotalNumberOfShare";
            }
            if (columns.Contains("FriendShares"))
            {
                dataTable.Columns["FriendShares"].ColumnName = "ForwardToAFriend";
            }

            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataTable);
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = string.Empty;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("TopEvangelistsReport.rdlc");

            ReportParameter[] yourParams = new ReportParameter[4];
            yourParams[0] = new ReportParameter("ShowFtoFriend", cbxForwardToFriend.Checked.ToString());
            yourParams[1] = new ReportParameter("ShowFacebook", cbxFacebook.Checked.ToString());
            yourParams[2] = new ReportParameter("ShowTwitter", cbxTwitter.Checked.ToString());
            yourParams[3] = new ReportParameter("ShowLinkedIn", cbxLinkedIn.Checked.ToString());

            ReportViewer1.LocalReport.SetParameters(yourParams);
            ReportViewer1.LocalReport.Refresh();

            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;

            switch (drpExport.SelectedItem.Value.ToUpper())
            {
                case "PDF":
                    bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/pdf";
                    break;
                case "XLS":
                    bytes = ReportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case "XLSDATA":
                    if (dataTable.Columns.Contains("EmailID")) { dataTable.Columns.Remove("EmailID"); }
                    if (dataTable.Columns.Contains("BlastID")) { dataTable.Columns.Remove("BlastID"); }
                    string csv = ECN_Framework_Common.Functions.DataTableFunctions.ToCSV(dataTable);
                    string fileName = "TopEvangelistsReport_" + Master.UserSession.CurrentCustomer.CustomerID + "_" + DateTime.Now.ToShortDateString();
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToCSV(csv, fileName);
                    break;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=TopEvangelistsReport." + drpExport.SelectedItem.Value);
            Response.BinaryWrite(bytes);
            Response.End();
        }

        public static string ToXml(DataSet ds)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, ds);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}