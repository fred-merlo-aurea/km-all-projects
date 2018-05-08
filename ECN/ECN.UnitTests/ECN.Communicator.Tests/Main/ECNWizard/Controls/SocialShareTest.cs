using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.OtherControls.SocialShare"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SocialShareTest
    {
        class TestTemplateItem : ITemplate
        {
            public Control control { get; set; }
            public void InstantiateIn(Control container)
            {
                container.Controls.Add(control);
            }
        }

        private NameValueCollection _queryString = new NameValueCollection();
        private const int SocialMediaIdFacebook = 1;
        private const int SocialMediaIdTwitter = 2;
        private const int SocialMediaIdLinkedin = 3;
        private const int SocialMediaIdFacebookLike = 4;
        private const int SocialMediaIdF2F = 5;

        private PrivateObject _testObject;
        private SimpleShareDetail _simpleShareDetail;
        private CampaignItemSocialMedia _campaignItemSocialMedia;
        private bool _enableRowSimpleShare;
        private IDisposable _shimObject;
        private string imageTraceMessage = string.Empty;
        private NameValueCollection _appSettings = new NameValueCollection
        {
            { "FBAPPID", String.Empty },
            { "FBSCOPE", String.Empty },
            { "LIAPPID", String.Empty },
            { "LIAPPSECRET", String.Empty },
            { "KMCommon_Application", "1" }
        };
        private List<string> _errorList = new List<string>();

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            ShimUserControl.AllInstances.TraceGet = page =>
            {
                return new ShimTraceContext()
                {
                    WarnStringStringException = (issueType, issueMessage, exception) => { }
                };
            };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private void SetCheckBox(string fieldName, bool value)
        {
            PrivateObject checkBox = new PrivateObject(_testObject.GetField(fieldName));
            checkBox.SetProperty("Checked", value);
        }

        private void InitilizeAssertObjects()
        {
            _errorList = new List<string>();
            _simpleShareDetail = null;
            _campaignItemSocialMedia = null;
        }

        private void InitilizeFakes()
        {
            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 = (p1, p2, p3, p4, p5, p6) =>  _errorList.Add(p2);
            ShimConfigurationManager.AppSettingsGet =
                () => _appSettings;
            ShimCampaignItemSocialMedia.DeleteInt32String = (campaignItemID, simpleOrSubscriber) => { };

            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User { UserID = 1 };
                session.CurrentCustomer = new ECN_Framework_Entities.Accounts.Customer { CustomerID = 1 };
                return session;
            };
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (socialMediaAuthID) =>
                new ECN_Framework_Entities.Communicator.SocialMediaAuth();
            ShimSocialMediaHelper.GetUserAccountsString = (accessToken) => new List<SocialMediaHelper.FBAccount>
            {
                new SocialMediaHelper.FBAccount { id = "facebookPageId" }
            };
            ShimSimpleShareDetail.SaveSimpleShareDetail = (simpleShareDetail) =>
            {
                _simpleShareDetail = simpleShareDetail;
                return 1;
            };
            ShimCampaignItemSocialMedia.SaveCampaignItemSocialMedia = (newCISM) =>
            {
                _campaignItemSocialMedia = newCISM;
                return 1;
            };
            ShimCampaignItemSocialMedia.DeleteInt32String = (campaignItemID, subscriber) => { };
            ShimCampaignItemMetaTag.SaveCampaignItemMetaTag = (cimtFBTitle) => 1;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (campaignItemID, user, getChildren) =>
                 new CampaignItem();
            ShimCampaignItemMetaTag.Delete_CampaignItemIDInt32Int32 = (campaignItemID, userID) => { };
            ShimCampaignItem.SaveCampaignItemUser = (ci, user) => 1;
            ShimSimpleShareDetail.DeleteFromCampaignItemInt32Int32 = (smaId, campaignItemID) => { };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (campaignItemID, getChildren) =>
                new CampaignItem { CompletedStep = 1 };

            ShimUserControl.AllInstances.TraceGet = (context) => 
                new ShimTraceContext()
                {
                    WarnStringStringException = (type, issueMessage, exception) => {
                        imageTraceMessage = string.Format(issueMessage, exception);
                    }
                };
        }

        private void InitilizeTestObject(SocialConfig scConfig)
        {
            var socialShare = new SocialShare();
            socialShare.Page = new Page();
            _testObject = new PrivateObject(socialShare);
            _testObject.SetField("gvSimpleShare", InitilizeGridView(scConfig));
            _testObject.SetField("UpdateProgress2", new UpdateProgress());
            _testObject.SetField("chkSimpleShare", new CheckBox { Checked = false });
            _testObject.SetField("phError", new PlaceHolder());
            _testObject.SetField("lblErrorMessage", new Label());
            _testObject.SetField("chkSubShare", new CheckBox { Checked = false });
            _testObject.SetField("chkF2FSubShare", new CheckBox { Checked = false });
            _testObject.SetField("chkFacebookLikeSubShare", new CheckBox { Checked = false });
            _testObject.SetField("chkFacebookSubShare", new CheckBox { Checked = false });
            var dropDownList = new DropDownList();
            dropDownList.Items.Add("Like Sub Share");
            dropDownList.Items.Add("1");
            dropDownList.SelectedIndex = 1;
            _testObject.SetField("ddlFacebookLikeSubShare", dropDownList);
            dropDownList = new DropDownList();
            dropDownList.Items.Add("Facebook User Accounts");
            dropDownList.Items.Add("Account1");
            dropDownList.SelectedIndex = 1;
            _testObject.SetField("ddlFacebookUserAccounts", dropDownList);
            _testObject.SetField("txtFBTitleMeta", new TextBox { Text = "Title" });
            _testObject.SetField("txtFBDescMeta", new TextBox { Text = "Description" });
            _testObject.SetField("hfFBTitleMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("hfFBDescMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("hfFBImageMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("imgbtnFBImageMeta", new ImageButton());
            _testObject.SetField("chkLinkedInSubShare", new CheckBox { Checked = false });
            _testObject.SetField("txtLITitleMeta", new TextBox { Text = "Title" });
            _testObject.SetField("txtLIDescMeta", new TextBox { Text = "Description" });
            _testObject.SetField("hfLITitleMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("hfLIDescMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("hfLIImageMetaID", new HiddenField { Value = "1" });
            _testObject.SetField("imgbtnLIImageMeta", new ImageButton());
            _testObject.SetField("chkTwitterSubShare", new CheckBox { Checked = false });
            _testObject.SetField("txtTWHashMeta", new TextBox { Text = "Hash" });
            _testObject.SetField("hfTWHashMeta", new HiddenField { Value = "1" });
        }

        private GridView InitilizeGridView(SocialConfig scConfig)
        {
            var gvSimpleShare = new GridView();
            gvSimpleShare.ID = "gvSimpleShare";
            var upSocialConfig = new UpdatePanel();
            upSocialConfig.Visible = true;
            upSocialConfig.ID = "upSocialConfig";
            upSocialConfig.ContentTemplateContainer.Controls.Add(scConfig);
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("Controls", typeof(Int32));
            dt.Columns.Add(dc);
            DataRow dr = dt.NewRow();
            dr["Controls"] = 0;
            dt.Rows.Add(dr);
            gvSimpleShare.DataKeyNames = new string[] { "Controls" };
            gvSimpleShare.DataSource = dt;
            gvSimpleShare.DataBind();
            gvSimpleShare.Rows[0].Cells[0].Controls.Add(upSocialConfig);
            gvSimpleShare.Rows[0].Cells[0].Controls.Add(new CheckBox { Checked = _enableRowSimpleShare, ID = "chkEnableSimpleShare" });

            return gvSimpleShare;
        }

        private SocialConfig ConfigureSocialConfig(int socialMediaId, string pageId, string message, string title, string subtitle)
        {
            var scConfig = new SocialConfig();
            InitializeAllControls(scConfig);
            scConfig.ID = "scConfig";
            var prvScConfig = new PrivateObject(scConfig);
            prvScConfig.SetField("hfSocialMediaID", new HiddenField { Value = socialMediaId.ToString() });
            prvScConfig.SetField("chkUseThumbNail", new CheckBox { Checked = true });
            var dropDownList = new DropDownList { ID = "ddlAccounts" };
            dropDownList.Items.Add("account");
            dropDownList.Items.Add("facebookPageId");
            dropDownList.Items.Add("linkedinPageId");
            dropDownList.Items.Add("twitterPageId");
            scConfig.Controls.Add(dropDownList);
            prvScConfig.SetField("ddlAccounts", dropDownList);
            prvScConfig.SetField("txtMessage", new HiddenField { ID = "txtMessage" });
            prvScConfig.SetField("txtPostLink", new HiddenField { ID = "txtPostLink" });
            prvScConfig.SetField("txtPostSubTitle", new HiddenField { ID = "txtPostSubTitle" });
            scConfig.PageID = pageId;
            scConfig.Message = message;
            scConfig.Title = title;
            scConfig.Subtitle = subtitle;

            return scConfig;
        }

        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                        }
                    }
                }
            }
        }
    }
}
