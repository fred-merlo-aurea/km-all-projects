using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using EmailPreview;
using KM.Common.Entity;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using CommonObjectEnums = ECN_Framework_Common.Objects.Communicator.Enums;
using EntityCommunicator = ECN_Framework_Entities.Communicator;
using Layout = ECN_Framework_BusinessLayer.Communicator.Layout;

namespace ecn.activityengines.engines
{
    public partial class BlastEmailPreview : System.Web.UI.Page
    {
        private const string NoResultMessage = "No results are available yet.";
        private const string Wait15MinMessage = "No results are available yet. Please wait 15 minutes and try again.  " +
                                               "If the issue persists please contact ECN Customer Support.";
        private const string WaitMessage = "No results are available yet.  Please wait until at least ";
        private const string RetrievingResultIssueMessage = "There is currently an issue with retrieving your results.  " +
                                                            "Please wait 5 minutes and try again.  " +
                                                            "If the issue persists please contact ECN Customer Support.";
        private const string TryAgainMessage = "No results are available yet. Please try again later.  " +
                                               "If the issue persists please contact ECN Customer Support.";
        private const string ProcessingNotDoneMessage = "Processing not done - no report to display.";
        private const string BindPreviewMethodName = "BindPreview";
        private const string LinkTestMethodName = "LinkTest";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string ErrorPageName = "Error.aspx";
        private const string CompatibilityRuleMessage1 = "We checked your email against";
        private const string CompatibilityRuleMessage2 = "compatibility rules and found";
        private const string CompatibilityRuleMessage3 = "Potential Problems and";
        private const string CompatibilityRuleMessage4 = "Html Problems.";

        List<CodeAnalysisResult> lcar = new List<CodeAnalysisResult>();
        public KMPlatform.Entity.User User = null;
        string Decrypted = string.Empty;
        private int BlastID = 0;
        //{
        //    get
        //    {
        //        int blastID = 0;
        //        int.TryParse(Request.QueryString["blastID"].ToString(), out blastID);
        //        return blastID;
        //    }
        //}

        private void GetValuesFromQuerystring(string queryString)
        {
            KM.Common.QueryString qs = KM.Common.QueryString.GetECNParameters(queryString);
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastID).ParameterValue, out BlastID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                    Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                }

                if (Request.Url.Query.ToString().Length > 0)
                {
                    Decrypted = Helper.DeCrypt_DeCode_EncryptedQueryString(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
                }
                if (Decrypted != string.Empty)
                {
                    GetValuesFromQuerystring(Decrypted);
                }

                BindPreview();
            }
        }

        private void BindPreview()
        {
            if (BlastID > 0)
            {
                var emailPreviews = BusinessCommunicator.EmailPreview.GetByBlastID(BlastID).ToList();
                var results = new List<EmailResult>();
                var preview = new Preview();
                var timeSpan = new TimeSpan(0, 1, 0);
                var emailPreview = default(EntityCommunicator.EmailPreview);

                try
                {
                    emailPreview = emailPreviews.OrderByDescending(previewItem => previewItem.DateCreated)
                        .ThenByDescending(previewItem => previewItem.TimeCreated).First();
                    results.AddRange(preview.GetExistingTestResults(emailPreview.EmailTestID, emailPreview.ZipFile));
                }
                catch (Exception exception)
                {
                    var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]);
                    ApplicationLog.LogNonCriticalError(exception, BindPreviewMethodName, applicationId);
                }

                if (results.Count > 0)
                {
                    BindSpamScoreAndHtml(results);
                }
                else
                {
                    var firstPreview = emailPreviews.FirstOrDefault();
                    if (emailPreviews.Count > 0 && firstPreview != null)
                    {
                        if (DateTime.Now.Date > firstPreview.DateCreated ||
                            DateTime.Now.TimeOfDay > firstPreview.TimeCreated.Add(timeSpan))
                        {
                            lbMessage.Text = RetrievingResultIssueMessage;
                        }
                        else
                        {
                            lbMessage.Text = $"{WaitMessage}" +
                                             $"{emailPreviews.First().DateCreated.ToShortDateString()} " +
                                             $"{emailPreviews.First().TimeCreated.Add(timeSpan).ToString(@"hh\:mm")}";
                        }
                    }
                    else
                    {
                        lbMessage.Text = NoResultMessage;
                    }
                    mpeMessage.Show();
                }

                BindPreviewCodeAnalysis(preview);

                LinkTest(preview, emailPreview);
            }
            else
            {
                Response.Redirect(
                    $"~/{ErrorPageName}?E={ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink}",
                    false);
            }
        }

        private void BindSpamScoreAndHtml(IList<EmailResult> results)
        {
            var emailResults = results
                .Where(result => result.ResultType.Equals(EmailResultEnum.ResultType.email.ToString()))
                .OrderBy(result => result.ApplicationLongName).ToList();

            if (emailResults.Count > 0)
            {
                rptSideBar.DataSource = emailResults;
                rptSideBar.DataBind();

                var spamResults = results.Where(result =>
                    result.ResultType.Equals(EmailResultEnum.ResultType.spam.ToString()) &&
                    !result.ApplicationName.Equals(EmailResultEnum.EmailSpam.htmlvalidation.ToString()) &&
                    !result.ApplicationName.Equals(EmailResultEnum.EmailSpam.linkcheck.ToString())).ToList();

                BindSpamScore(spamResults);

                var htmlResults = results.Where(result =>
                    result.ResultType.Equals(EmailResultEnum.ResultType.spam.ToString()) &&
                    (result.ApplicationName.Equals(EmailResultEnum.EmailSpam.htmlvalidation.ToString()) ||
                     result.ApplicationName.Equals(EmailResultEnum.EmailSpam.linkcheck.ToString()))).ToList();
                
                BindHTML(htmlResults);
            }
            else
            {
                lbMessage.Text = Wait15MinMessage;
                mpeMessage.Show();
            }
        }

        private void BindPreviewCodeAnalysis(Preview preview)
        {
            var blast = BusinessCommunicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);
            var layoutId = blast.LayoutID.GetValueOrDefault();
            var html = string.Empty;
            if (blast.CustomerID != null)
            {
                html = Layout.GetPreviewNoAccessCheck(
                    layoutId,
                    CommonObjectEnums.ContentTypeCode.HTML,
                    false,
                    blast.CustomerID.Value);
            }
            html = html.Replace("\"", "'");

            var codeAnalysisTest = preview.GetCodeAnalysisTest(html);
            if (codeAnalysisTest != null && codeAnalysisTest.CompatibilityRulesCount > 0)
            {
                if (codeAnalysisTest.CompatibilityProblems.Count > 0)
                {
                    plPotentialProblems.Visible = true;
                }

                if (codeAnalysisTest.HtmlProblems.Count > 0)
                {
                    plHtmlValidation.Visible = true;
                }

                foreach (var potentialProblems in codeAnalysisTest.CompatibilityProblems)
                {
                    foreach (var apiId in potentialProblems.ApiIds)
                    {
                        var analysisResult = lcar.FirstOrDefault(result =>
                            result.ApplicationID.Equals(apiId, StringComparison.OrdinalIgnoreCase));
                        if (analysisResult == null)
                        {
                            analysisResult = new CodeAnalysisResult
                            {
                                ApplicationID = apiId,
                                ApplicationName = apiId
                            };

                            var testingApplication = preview.GetTestingApplication();
                            var listItem = testingApplication.FirstOrDefault(app => app.ApplicationName.Equals(apiId));
                            analysisResult.ApplicationLongName = listItem != null
                                ? testingApplication.Find(app => app.ApplicationName.Equals(apiId)).ApplicationLongName
                                : apiId;

                            analysisResult.CodeAnalysisResultDetails = new List<CodeAnalysisResultDetail>
                            {
                                new CodeAnalysisResultDetail(
                                    potentialProblems.LineNumber,
                                    potentialProblems.Severity,
                                    potentialProblems.Description)
                            };
                            lcar.Add(analysisResult);
                        }
                        else
                        {
                            analysisResult.CodeAnalysisResultDetails.Add(
                                new CodeAnalysisResultDetail(
                                    potentialProblems.LineNumber,
                                    potentialProblems.Severity,
                                    potentialProblems.Description));
                        }
                    }
                }

                lblCodeAnalysisResult.Text = $"{CompatibilityRuleMessage1} {codeAnalysisTest.CompatibilityRulesCount} " +
                                             $"{CompatibilityRuleMessage2} {lcar.Count} {CompatibilityRuleMessage3} " +
                                             $"{codeAnalysisTest.HtmlProblems.Count} {CompatibilityRuleMessage4}";

                BindPotentialProblems(lcar);
                BindHtmlValidation(codeAnalysisTest.HtmlProblems);
            }
            else
            {
                lblCodeAnalysisResult.Text = ProcessingNotDoneMessage;
            }
        }

        private void LinkTest(Preview preview, EntityCommunicator.EmailPreview emailPreview)
        {
            var linktest = new LinkTest();
            try
            {
                linktest = preview.GetLinkTestResults(emailPreview.LinkTestID);
            }
            catch (Exception exception)
            {
                var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]);
                ApplicationLog.LogNonCriticalError(exception, LinkTestMethodName, applicationId);
            }

            if (linktest?.Links != null && linktest.Links.Count > 0)
            {
                BindLinkCheck(linktest);
            }
            else
            {
                lblLinkCheckResult.Text = TryAgainMessage;
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
                    litSpam.Text = litSpam.Text + "<b>" + sh.Key + " ::</b>" + sh.Description + "<br/><br/>";
                }

                System.Web.UI.WebControls.Image imgStatus = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgStatus");
                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                Label lblScore = (Label)e.Item.FindControl("lblScore");

                if (er.SpamHeaders.Count == 0)
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

                string description = string.Empty;

                foreach (CodeAnalysisResultDetail c in car.CodeAnalysisResultDetails)
                {
                    description += description == string.Empty ? "Line " + c.LineNumber + " :: " + Server.HtmlEncode(c.Description) : "<br/><br/>" + "Line " + c.LineNumber + " :: " + Server.HtmlEncode(c.Description);
                }

                litPotentialProblems.Text = description;
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

                if (l.IsValid.Value && l.IsValid.Value)
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