using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EmailPreview;
using AjaxControlToolkit;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using System.Text;
using KM.Common;
using Diagnostics = System.Diagnostics.Trace;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessApplication = ECN_Framework_BusinessLayer.Application;
using CommonObjects = ECN_Framework_Common.Objects;
using EntityEncryption = KM.Common.Entity.Encryption;
using KMCommonEncryption = KM.Common.Encryption;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.blasts
{
    public partial class BlastEmailPreview : System.Web.UI.Page
    {
        private const string DelimBackSlash = "\"";
        private const string DelimSingleQuote = "'";

        private const string AppSettingActivityDomainPath = "Activity_DomainPath";
        private const string UrlPreview = "/engines/BlastEmailPreview.aspx?";

        private const string MessageWait15Minutes = 
            "No results are available yet. Please wait 15 minutes and try again.  " +
            "If the issue persists please contact ECN Customer Support.";
        private const string MessageWait5Minutes = 
            "There is currently an issue with retrieving your results.  Please wait 5 minutes and try again.  " +
            "If the issue persists please contact ECN Customer Support.";
        private const string MessageWaitAtLeastTemplate = 
            "No results are available yet.  Please wait until at least {0} {1:hh\\:mm}";
        private const string MessageNoResults = "No results are available yet.";
        private const string MessageEmailPotentialProbems = 
            "We checked your email against {0} compatibility rules and found {1} Potential Problems and {2} Html Problems.";
        private const string MessageNoReport = "Processing not done - no report to display.";
        private const string MessageNoResultsContactSupport = 
            "No results are available yet. Please try again later.  " +
            "If the issue persists please contact ECN Customer Support.";
        
        private const string ErrorParseInt = "Couldn't parse {0} into int.";
        private const string ErrorPublicUrlGeneration = "Unable to generate public URL at this time";
        private static readonly TimeSpan Minute = new TimeSpan(0, 1, 0);

        private const string AppSettingCommonApplication = "KMCommon_Application";

        List<CodeAnalysisResult> lcar = new List<CodeAnalysisResult>();

        private int BlastID
        {
            get
            {
                int blastID = 0;
                int.TryParse(Request.QueryString["blastID"].ToString(), out blastID);
                return blastID;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPreview();
            }
        }
        private void BindPreview()
        {
            if (BlastID > 0)
            {
                var epList = BusinessCommunicator.EmailPreview.GetByBlastID(BlastID).ToList();
                var testResults = new List<EmailResult>();
                var preview = new Preview();
                var timeSpan = Minute;

                EntitiesCommunicator.EmailPreview emailPreview = null;
                try
                {
                    emailPreview = epList.OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.TimeCreated).First();
                    testResults.AddRange(preview.GetExistingTestResults(emailPreview.EmailTestID, emailPreview.ZipFile));
                }
                catch (Exception ex)
                {
                    Diagnostics.TraceError(ex.ToString());
                }

                ProcessTestResults(testResults, epList, timeSpan);

                AnalyzeCode(preview);

                TestLink(preview, emailPreview);

                CreatePublicLink();
            }
        }

        private void ProcessTestResults(
            IReadOnlyCollection<EmailResult> testResults, 
            IEnumerable<EntitiesCommunicator.EmailPreview> previews, 
            TimeSpan timeSpan)
        {
            Guard.NotNull(testResults, nameof(testResults));

            if (testResults.Count > 0)
            {
                var emailResults = testResults
                    .Where(x => x.ResultType.Equals(EmailResultEnum.ResultType.email.ToString()))
                    .OrderBy(x => x.ApplicationLongName).ToList();

                if (emailResults.Count > 0)
                {
                    BindSpamAndHtml(testResults, emailResults);
                }
                else
                {
                    lbMessage.Text = MessageWait15Minutes;
                    mpeMessage.Show();
                }
            }
            else
            {
                var firstEmailPreview = previews.FirstOrDefault();
                if (firstEmailPreview != null)
                {
                    if (DateTime.Now.Date > firstEmailPreview.DateCreated ||
                        DateTime.Now.TimeOfDay > firstEmailPreview.TimeCreated.Add(timeSpan))
                    {
                        lbMessage.Text = MessageWait5Minutes;
                    }
                    else
                    {
                        lbMessage.Text = string.Format(
                            MessageWaitAtLeastTemplate,
                            firstEmailPreview.DateCreated.ToShortDateString(),
                            firstEmailPreview.TimeCreated.Add(timeSpan));
                    }
                }
                else
                {
                    lbMessage.Text = MessageNoResults;
                }

                mpeMessage.Show();
            }
        }

        private void BindSpamAndHtml(
            IReadOnlyCollection<EmailResult> testResults, 
            IReadOnlyCollection<EmailResult> emailResults)
        {
            Guard.NotNull(testResults, nameof(testResults));
            Guard.NotNull(emailResults, nameof(emailResults));

            rptSideBar.DataSource = emailResults;
            rptSideBar.DataBind();

            var spamResults = testResults.Where(
                x => x.ResultType.Equals(EmailResultEnum.ResultType.spam.ToString()) &&
                     (!x.ApplicationName.Equals(EmailResultEnum.EmailSpam.htmlvalidation.ToString()) &&
                      !x.ApplicationName.Equals(EmailResultEnum.EmailSpam.linkcheck.ToString()))).ToList();
            BindSpamScore(spamResults);
            var htmlResults = testResults.Where(x =>
                x.ResultType.Equals(EmailResultEnum.ResultType.spam.ToString()) &&
                (EmailResultEnum.EmailSpam.htmlvalidation.ToString().Equals(
                     x.ApplicationName, 
                     StringComparison.OrdinalIgnoreCase) ||
                 EmailResultEnum.EmailSpam.linkcheck.ToString().Equals(
                     x.ApplicationName, 
                     StringComparison.OrdinalIgnoreCase))).ToList();
            BindHTML(htmlResults);
        }

        private void AnalyzeCode(Preview preview)
        {
            Guard.NotNull(preview, nameof(preview));

            var layoutId = BusinessCommunicator.Blast.GetByBlastID(
                BlastID,
                BusinessApplication.ECNSession.CurrentSession().CurrentUser,
                false
            ).LayoutID.GetValueOrDefault();

            var html = BusinessCommunicator.Layout.GetPreview(
                layoutId,
                CommonObjects.Communicator.Enums.ContentTypeCode.HTML,
                false,
                BusinessApplication.ECNSession.CurrentSession().CurrentUser);

            html = html.Replace(DelimBackSlash, DelimSingleQuote);

            var analysisTest = preview.GetCodeAnalysisTest(html);
            if (analysisTest?.CompatibilityRulesCount > 0)
            {
                if (analysisTest.CompatibilityProblems.Count > 0)
                    plPotentialProblems.Visible = true;

                if (analysisTest.HtmlProblems.Count > 0)
                {
                    plHtmlValidation.Visible = true;
                }

                var testingApplication = preview.GetTestingApplication();

                foreach (var problems in analysisTest.CompatibilityProblems)
                {
                    foreach (var apiId in problems.ApiIds)
                    {
                        var analysisResult = lcar.FirstOrDefault(
                            x => string.Equals(x.ApplicationID, apiId, StringComparison.CurrentCultureIgnoreCase));
                        if (analysisResult == null)
                        {
                            analysisResult = new CodeAnalysisResult();

                            analysisResult.ApplicationID = apiId;
                            analysisResult.ApplicationName = apiId;
                            var listItem = testingApplication.FirstOrDefault(x => x.ApplicationName == apiId);
                            analysisResult.ApplicationLongName = listItem != null
                                ? testingApplication.Find(x => x.ApplicationName == apiId).ApplicationLongName
                                : apiId;

                            analysisResult.CodeAnalysisResultDetails = new List<CodeAnalysisResultDetail>();
                            analysisResult.CodeAnalysisResultDetails.Add(new CodeAnalysisResultDetail(
                                problems.LineNumber,
                                problems.Severity,
                                problems.Description));
                            lcar.Add(analysisResult);
                        }
                        else
                        {
                            analysisResult.CodeAnalysisResultDetails.Add(new CodeAnalysisResultDetail(
                                problems.LineNumber,
                                problems.Severity,
                                problems.Description));
                        }
                    }
                }

                lblCodeAnalysisResult.Text = string.Format(
                    MessageEmailPotentialProbems,
                    analysisTest.CompatibilityRulesCount,
                    lcar.Count,
                    analysisTest.HtmlProblems.Count);

                BindPotentialProblems(lcar);
                BindHtmlValidation(analysisTest.HtmlProblems);
            }
            else
            {
                lblCodeAnalysisResult.Text = MessageNoReport;
            }
        }

        private void TestLink(Preview preview, EntitiesCommunicator.EmailPreview emailPreview)
        {
            var linkTest = new LinkTest();
            try
            {
                linkTest = preview.GetLinkTestResults(emailPreview.LinkTestID);
            }
            catch (Exception ex)
            {
                Diagnostics.TraceError(ex.ToString());
            }

            if (linkTest?.Links?.Count > 0)
            {
                BindLinkCheck(linkTest);
            }
            else
            {
                lblLinkCheckResult.Text = MessageNoResultsContactSupport;
            }
        }

        private void CreatePublicLink()
        {
            try
            {
                var publicUrl = new StringBuilder("Public URL: ");
                publicUrl.AppendFormat(
                    "{0}{1}",
                    ConfigurationManager.AppSettings[AppSettingActivityDomainPath],
                    UrlPreview);
                var queryString = $"BlastID={BlastID}";

                var applicationStr = ConfigurationManager.AppSettings[AppSettingCommonApplication];
                int applicationId;
                if (!int.TryParse(applicationStr, out applicationId))
                {
                    throw new InvalidOperationException(string.Format(ErrorParseInt, applicationStr));
                }

                var encryption = EntityEncryption.GetCurrentByApplicationID(applicationId);
                publicUrl.Append(HttpUtility.UrlEncode(KMCommonEncryption.Encrypt(queryString, encryption)));
                lblPublicURL.Text = publicUrl.ToString();
            }
            catch (Exception)
            {
                lblPublicURL.Text = ErrorPublicUrlGeneration;
            }
        }

        private void BindSpamScore(List<EmailPreview.EmailResult> spamResults)
        {
            rptSpam.DataSource = spamResults;
            rptSpam.DataBind();
        }
        protected void rptSpam_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.DataItem != null)
            {
                EmailPreview.EmailResult er = (EmailPreview.EmailResult)e.Item.DataItem;
                Literal litSpam = (Literal)e.Item.FindControl("litSpam");
                foreach (SpamHeader sh in er.SpamHeaders)
                {
                    if (!string.IsNullOrEmpty(sh.Key) && !string.IsNullOrEmpty(sh.Description))
                        litSpam.Text = litSpam.Text + "<b>" + sh.Key + " ::</b>" + sh.Description + "<br/><br/>";
                }

                System.Web.UI.WebControls.Image imgStatus = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgStatus");
                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                Label lblScore = (Label)e.Item.FindControl("lblScore");

                if (er.SpamHeaders.Count == 0 || string.IsNullOrEmpty(litSpam.Text))
                {
                    Panel pnlSpamDesc = (Panel)e.Item.FindControl("pnlSpamDesc");
                    pnlSpamDesc.Visible = false;

                    ImageButton imgExpand = (ImageButton)e.Item.FindControl("imgExpand");
                    imgExpand.Visible = false;

                    CollapsiblePanelExtender cpe1 = (CollapsiblePanelExtender)e.Item.FindControl("cpe1");
                    cpe1.Enabled = false;
                }

                if (er.FoundInSpam.HasValue && er.FoundInSpam.Value)
                {
                    imgStatus.ImageUrl = "http://images.ecn5.com/images/icon_delete.png";
                    lblStatus.Text = "FAILED";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    imgStatus.ImageUrl = "http://images.ecn5.com/images/icon_tick.png";
                    lblStatus.Text = "PASSED";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }

                if (er.SpamScore.Equals(0))
                {
                    lblScore.Visible = false; 
                }
                else
                {
                    lblScore.Text = " with a score of " + er.SpamScore.ToString();
                    lblScore.Visible = true;
                }
  
                Label lblspamApplication = (Label)e.Item.FindControl("lblspamApplication");
                lblspamApplication.Text = er.ApplicationLongName;
            }
        }
        private void BindHTML(List<EmailPreview.EmailResult> htmlResults)
        {
            if (htmlResults.Count == 0)
            {
                litHtml.Text = "Processing not done - no preview to display.";
            }
            else
            {
                foreach (EmailPreview.EmailResult er in htmlResults)
                {
                    System.Text.StringBuilder summary = er.Summary;
                    litHtml.Text += summary.ToString();
                }
            }
        }

        bool bfirst = true;

        protected void rptSideBar_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.DataItem != null)
            {
                EmailPreview.EmailResult er = (EmailPreview.EmailResult)e.Item.DataItem;
               
                Label lbEmailAppName = (Label)e.Item.FindControl("lbEmailAppName");
                if (er.ApplicationName.ToString().StartsWith("ff"))
                    lbEmailAppName.Text = "FireFox - " + er.ApplicationLongName;
                else
                    lbEmailAppName.Text = er.ApplicationLongName;

                if (string.IsNullOrWhiteSpace(er.FullpageImageThumb) == true)
                    lbEmailAppName.Text += " - Still Processing";

                ImageButton btnEmailThumb = (ImageButton)e.Item.FindControl("btnEmailThumb");
                
                if (er.Completed == true)
                {
                    btnEmailThumb.Visible = true;
                    btnEmailThumb.ImageUrl = (HttpContext.Current.Request.IsSecureConnection == true ? "https://" : "http://") + er.FullpageImageThumb;
                    btnEmailThumb.OnClientClick = "return SetFullImagePath(" + "'" + (HttpContext.Current.Request.IsSecureConnection == true ? "https://" : "http://") + er.FullpageImage + "','" + imgEmail.ClientID + "')";

                    if (bfirst)
                    {
                        bfirst = false;
                        imgEmail.ImageUrl = (HttpContext.Current.Request.IsSecureConnection == true ? "https://" : "http://") + er.FullpageImage;
                        imgEmail.Style.Add("display", "block");
                    }
                }
                else
                {
                    btnEmailThumb.Visible = false;
                    lbEmailAppName.Text += "</br> Still Processing";
                }

            }
        }
        protected void rptPotentialProblems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.DataItem != null)
            {
                CodeAnalysisResult car = (CodeAnalysisResult)e.Item.DataItem;

                Literal litPotentialProblems = (Literal)e.Item.FindControl("litPotentialProblems");

                StringBuilder description = new StringBuilder();

                foreach (CodeAnalysisResultDetail c in car.CodeAnalysisResultDetails)
                {
                    description.AppendLine("Line " + c.LineNumber + " :: " + Server.HtmlEncode(c.Description) + "<br /><br />");
                }

                litPotentialProblems.Text = description.ToString();
            }
        }
        protected void rptCodeHtmlValidation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.DataItem != null)
            {
                CodeAnalysisHtmlValidation cahv = (CodeAnalysisHtmlValidation)e.Item.DataItem;

                Literal litCodeHtmlValidation = (Literal)e.Item.FindControl("litCodeHtmlValidation");

                string description = string.Empty;

                description += description == string.Empty ? "Line " + cahv.LineNumber + " :: " + Server.HtmlEncode(cahv.Description) : "<br/>" + "Line " + cahv.LineNumber + " :: " + Server.HtmlEncode(cahv.Description);

                litCodeHtmlValidation.Text = description;
            }
        }
        private void BindPotentialProblems(List<EmailPreview.CodeAnalysisResult> Results)
        {
            rptPotentialProblems.DataSource = Results;
            rptPotentialProblems.DataBind();
        }
        private void BindHtmlValidation(List<EmailPreview.CodeAnalysisHtmlValidation> Results)
        {
            rptCodeHtmlValidation.DataSource = Results;
            rptCodeHtmlValidation.DataBind();
        }
        protected void rptLinkCheck_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.DataItem != null)
            {
                EmailPreview.Link lt = (EmailPreview.Link)e.Item.DataItem;

                ImageButton btnLinkThumb = (ImageButton)e.Item.FindControl("btnLinkThumb");
                btnLinkThumb.ImageUrl = lt.ThumbnailUri;

                System.Web.UI.WebControls.Image imgIsValid = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgIsValid");
                if (lt.IsValid.HasValue && lt.IsValid.Value)
                    imgIsValid.ImageUrl = "http://images.ecn5.com/images/icon_tick.png";
                else
                    imgIsValid.ImageUrl = "http://images.ecn5.com/images/icon_delete.png";

                System.Web.UI.WebControls.Image imgIsBlackListed = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgIsBlackListed");
                if (lt.IsBlacklisted.HasValue && lt.IsBlacklisted.Value)
                    imgIsBlackListed.ImageUrl = "http://images.ecn5.com/images/icon_delete.png";
                else
                    imgIsBlackListed.ImageUrl = "http://images.ecn5.com/images/icon_tick.png";

                //Image imgHasClickTracking = (Image)e.Item.FindControl("imgHasClickTracking");
                //if (lt.HasClickTracking)
                //    imgHasClickTracking.ImageUrl = "http://images.ecn5.com/images/icon_tick.png";
                //else
                //    imgHasClickTracking.ImageUrl = "http://images.ecn5.com/images/icon_delete.png";

                if (lt.HasGoogleAnalytics.HasValue && lt.HasGoogleAnalytics.Value)
                {
                    Literal litLinkCheck = (Literal)e.Item.FindControl("litLinkCheck");
                    PlaceHolder plGoogleAnalystic = (PlaceHolder)e.Item.FindControl("plGoogleAnalystic");

                    plGoogleAnalystic.Visible = true;

                    foreach (KeyValuePair<string, string> d in lt.GoogleAnalyticsParameters)
                    {
                        litLinkCheck.Text += litLinkCheck.Text + "<b><font color='green'>" + d.Key + "</font> " + d.Value + "</b><br/><br/>";
                    }
                }
            }
        }
        private void BindLinkCheck(EmailPreview.LinkTest linktest)
        {
            rptLinkCheck.DataSource = linktest.Links;
            rptLinkCheck.DataBind();

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(linktest.ImageUrl);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(ms);
            ms.Close();

            Graphics grPhoto = Graphics.FromImage(imgPhoto);

            foreach (Link l in linktest.Links)
            {
                WebClient wc1 = new WebClient();
                byte[] bytes1;

                if (l.IsValid.HasValue && l.IsValid.Value)
                    bytes1 = wc1.DownloadData("http://images.ecn5.com/images/icon-tick1.gif");
                else
                    bytes1 = wc1.DownloadData("http://images.ecn5.com/images/icon-delete1.gif");

                MemoryStream ms1 = new MemoryStream(bytes1);

                System.Drawing.Image imgmark = new Bitmap(ms1);

                grPhoto.DrawImage(imgmark, new Point(l.TopLeftX, l.TopLeftY));

                RectangleHotSpot hotSpot = new RectangleHotSpot();
                hotSpot.NavigateUrl = "javascript:ShowModalPopup('div_" + l.TopLeftX.ToString() + "_" + l.TopLeftY.ToString() + "')";
                hotSpot.Top = l.TopLeftY;
                hotSpot.Left = l.TopLeftX;
                hotSpot.Bottom = l.TopLeftY + 15;
                hotSpot.Right = l.TopLeftX + 15;
                hotSpot.AlternateText = l.Url;
                ImageMap1.HotSpots.Add(hotSpot);

                ms1.Close();
            }

            MemoryStream ms2 = new MemoryStream();
            imgPhoto.Save(ms2, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms2.ToArray());
            ImageMap1.ImageUrl = "data:image/gif;base64," + base64Data;

            ms2.Close();
        }
    }
}