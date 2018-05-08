using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Communicator;
using KM.Common;
using KM.Common.Entity;
using BlastAbstract = ECN_Framework_Entities.Communicator.BlastAbstract;
using Encryption = KM.Common.Entity.Encryption;
using CommunicationEnums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.activityengines
{
    public partial class publicPreview
    {
        private const string LayoutBlastType = "LAYOUT";
        private const string NoOpenBlastType = "NOOPEN";
        private const string BlastIdColumnName = "BlastID";
        private const string LayoutIdColumnName = "LayoutID";
        private const string GroupIdColumnName = "GroupID";
        private const string PublicMethodName = "PublicPreview.Page_Load";
        private static readonly string[] Mobiles = {
            "midp", "j2me", "avant", "docomo",
            "novarra", "palmos", "palmsource",
            "240x320", "opwv", "chtml",
            "pda", "windows ce", "mmp/",
            "blackberry", "mib/", "symbian",
            "wireless", "nokia", "hand", "mobi",
            "phone", "cdm", "up.b", "audio",
            "SIE-", "SEC-", "samsung", "HTC",
            "mot-", "mitsu", "sagem", "sony",
            "alcatel", "lg", "eric", "vx",
            "NEC", "philips", "mmm", "xx",
            "panasonic", "sharp", "wap", "sch",
            "rover", "pocket", "benq", "java",
            "pt", "pg", "vox", "amoi",
            "bird", "compal", "kg", "voda",
            "sany", "kdd", "dbt", "sendo",
            "sgh", "gradi", "jb", "dddi",
            "moto", "iphone"
        };
        private readonly int _applicationId = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);

        public static bool IsMobileBrowser()
        {
            var context = HttpContext.Current;
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_ACCEPT"]?.IndexOf("wap", StringComparison.InvariantCultureIgnoreCase)>-1)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_USER_AGENT"] == null)
            {
                return false;
            }

            return Mobiles.Any(s => context.Request.ServerVariables["HTTP_USER_AGENT"]?.IndexOf(
                s,
                StringComparison.InvariantCultureIgnoreCase) > -1);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WriteToLog("PageLoad");

            if (Request.Url.Query.Length > 0)
            {
                GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            }

            WriteToLog("Got query string values");


            var isMobile = IsMobileBrowser();
            if (LayoutID > 0)
            {
                LoadLayoutHtmlPreview(isMobile);
            }
            else if (BlastID > 0 && EmailID > 0)
            {
                LoadBlastForEmail(isMobile);
            }
        }

        private void LoadBlastForEmail(bool isMobile)
        {
            try
            {
                WriteToLog("BlastID > 0 and EmailID > 0");
                var enc = Encryption.GetCurrentByApplicationID(_applicationId);

                WriteToLog("Got Encryption object");
                var blast = Blast.GetByBlastID_NoAccessCheck(BlastID, false);
                WriteToLog("Got Blast object");

                if (blast != null)
                {
                    LoadMergedEmailIdIfExist();
                    LoadBlastItem(isMobile, blast, enc);
                }
                else
                {
                    literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                    ApplicationLog.LogNonCriticalError($"Invalid BlastID: {BlastID}",
                        PublicMethodName,
                        _applicationId);
                }
            }
            catch (Exception ex)
            {
                literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                ApplicationLog.LogNonCriticalError(
                    $"Unknown issue. BlastID: {BlastID} , EmailID: {EmailID} \r\n {ex.Message}",
                    PublicMethodName,
                    _applicationId);
            }
        }

        private void LoadBlastItem(bool isMobile, BlastAbstract blast, Encryption enc)
        {
            if (blast.BlastType.Equals(LayoutBlastType, StringComparison.InvariantCultureIgnoreCase) ||
                blast.BlastType.Equals(NoOpenBlastType, StringComparison.InvariantCultureIgnoreCase) ||
                (blast.GroupID != null &&
                  EmailGroup.GetByEmailIDGroupID_NoAccessCheck(EmailID, blast.GroupID.Value) != null) ||
                 EmailGroup.EmailExistsInCustomerSeedList(EmailID, blast.CustomerID.GetValueOrDefault()))
            {
                WriteToLog("Trigger blast");
                var testBlastLinks = new Dictionary<string, int>();
                var testBlast = blast.TestBlast.Equals("y",StringComparison.InvariantCultureIgnoreCase);
                var customer = Customer.GetByCustomerID(blast.CustomerID.GetValueOrDefault(), false);
                var baseChannel = BaseChannel.GetByBaseChannelID(customer.BaseChannelID.GetValueOrDefault());
                WriteToLog("Got Customer and BC objects");

                var groupId = 0;
                var emailRowsDt = GetEmailRows(blast, ref groupId);
                var firstrow = emailRowsDt.Rows.Count > 0
                    ? emailRowsDt.Rows[0]
                    : null;
                firstrow = SeedFirstRowIfMissing(blast, firstrow, emailRowsDt);
                firstrow.Table.Columns[BlastIdColumnName].ReadOnly = false;
                firstrow[BlastIdColumnName] = blast.BlastID;

                WriteToLog("Start getting Layout preview");

                new HtmlBuilder(WriteToLog, BlastID, EmailID, Transnippet, literalPreview, Trace)
                        .GenerateLayoutPreview(isMobile, blast, groupId, firstrow, baseChannel)
                        .GenerateSocialScripts(blast, enc, groupId)
                        .GenerateTransnippets(blast, enc, emailRowsDt, firstrow, testBlast, testBlastLinks);
            }
            else
            {
                literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                ApplicationLog.LogNonCriticalError(
                    $"Invalid BlastID: {BlastID} or EmailID: {EmailID}",
                    PublicMethodName,
                    _applicationId);
            }
        }

        private DataRow SeedFirstRowIfMissing(BlastAbstract blast, DataRow firstrow, DataTable emailRowsDt)
        {
            Guard.NotNull(blast, nameof(blast));
            Guard.NotNull(emailRowsDt, nameof(emailRowsDt));

            if (firstrow != null)
            {
                return firstrow;
            }

            WriteToLog("Check if email is in seed list");

            firstrow = emailRowsDt.NewRow();
            if (EmailGroup.EmailExistsInCustomerSeedList(EmailID, blast.CustomerID.GetValueOrDefault()))
            {
                var email = Email.GetByEmailID_NoAccessCheck(EmailID);
                foreach (var columnName in Email.GetColumnNames().AsEnumerable())
                {
                    try
                    {
                        firstrow[columnName[0].ToString()] = email.GetType().GetProperty(columnName[0].ToString()).GetValue(email).ToString();
                    }
                    catch (Exception ex)
                    {
                        Trace.Warn("Error", "Exception suppressed", ex);
                    }
                }

                firstrow[BlastIdColumnName] = BlastID;
                firstrow[LayoutIdColumnName] = blast.LayoutID.GetValueOrDefault().ToString();
                firstrow[GroupIdColumnName] = blast.GroupID.GetValueOrDefault().ToString();
                WriteToLog("Got Seed list email");
            }

            return firstrow;
        }

        private DataTable GetEmailRows(BlastAbstract blast, ref int groupId)
        {
            Guard.NotNull(blast, nameof(blast));

            DataTable emailRowsDt = null;
            try
            {
                if(blast.GroupID != null)
                {
                    groupId = blast.GroupID.Value;
                }

                if (blast.BlastType.Equals(LayoutBlastType, StringComparison.InvariantCultureIgnoreCase) ||
                    blast.BlastType.Equals(NoOpenBlastType, StringComparison.InvariantCultureIgnoreCase))
                {
                    WriteToLog("Get refBlast and GroupID");
                    var refBlastId = BlastSingle.GetRefBlastID(
                        BlastID,
                        EmailID,
                        blast.CustomerID.GetValueOrDefault(),
                        blast.BlastType);
                    var blastRef = Blast.GetByBlastID_NoAccessCheck(refBlastId, false);
                    groupId = blastRef.GroupID.GetValueOrDefault();
                    emailRowsDt = Blast.GetHTMLPreview(blastRef.BlastID, EmailID);
                    WriteToLog("Got HTML Preview");
                }
                else
                {
                    emailRowsDt = Blast.GetHTMLPreview(BlastID, EmailID);
                    WriteToLog("Got HTML Preview");
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("Error", "Exception suppressed", ex);
            }

            return emailRowsDt ?? new DataTable();
        }

        private void LoadMergedEmailIdIfExist()
        {
            WriteToLog("Check for merged email");
            var eToCheck = Email.GetByEmailID_NoAccessCheck(EmailID);
            if (eToCheck != null)
            {
                return;
            }

            var emailId = EmailHistory.FindMergedEmailID(EmailID);
            if (emailId <= 0)
            {
                return;
            }

            WriteToLog("Got Merged email");
            EmailID = emailId;
        }

        private void LoadLayoutHtmlPreview(bool isMobile)
        {
            var userId = 0;
            var customerId = -1;
            WriteToLog("LayoutID > 0");

            try
            {
                customerId = Layout.GetByLayoutID_NoAccessCheck(LayoutID, false).CustomerID.GetValueOrDefault();
                userId = Layout.GetLayoutUserID(LayoutID);

                WriteToLog("Got CustomerID and UserID");
            }
            catch (Exception)
            {
                literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                ApplicationLog.LogNonCriticalError(
                    $"Invalid LayoutID: {LayoutID}",
                    PublicMethodName,
                    _applicationId);
            }

            if (userId != 0)
            {
                WriteToLog("Get Preview");
                var html = Layout.GetPreviewNoAccessCheck(
                    LayoutID,
                    CommunicationEnums.ContentTypeCode.HTML,
                    isMobile,
                    customerId);
                literalPreview.Text = html;
                WriteToLog("Preview Loaded");
            }
            else
            {
                literalPreview.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
                ApplicationLog.LogNonCriticalError(
                    $"Invalid LayoutID: {LayoutID}",
                    PublicMethodName,
                    _applicationId);
            }
        }
    }
}