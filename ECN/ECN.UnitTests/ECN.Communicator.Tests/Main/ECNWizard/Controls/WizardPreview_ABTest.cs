using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssertNunit = NUnit.Framework.Assert;
using EntityGroup = ECN_Framework_Entities.Communicator.Group;
using ShimKmUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.Controls.WizardPreview_AB"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardPreview_ABTest
    {
        private const int CampaignItemID = 1;
        private CampaignItem _campaignItemObject;
        private PrivateObject _testObject;
        private CampaignItemSocialMedia _dummySocialMedia;
        private IDisposable _shimObject;
        private List<string> _errorList = new List<string>();
        private SimpleShareDetail _simpleShareDetail;
        private CampaignItemSocialMedia _campaignItemSocialMedia;

        private NameValueCollection _queryString = new NameValueCollection();
        private const int SocialMediaIdFacebook = 1;
        private const int SocialMediaIdTwitter = 2;
        private const int SocialMediaIdLinkedin = 3;
        private const int SocialMediaIdFacebookLike = 4;
        private const int SocialMediaIdF2F = 5;
        private NameValueCollection _appSettings = new NameValueCollection
        {
            { "FBAPPID", String.Empty },
            { "FBSCOPE", String.Empty },
            { "LIAPPID", String.Empty },
            { "LIAPPSECRET", String.Empty },
            { "KMCommon_Application", "1" },
            { "TW_CONSUMER_KEY", string.Empty },
            { "TW_CONSUMER_SECRET", string.Empty }
        };

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Initialize_WizardPreview_AB_Success()
        {
            // Arrange
            InitilizeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();

            // Act
            _testObject.Invoke("Initialize");

            // Assert
            AssertPropertiesAndFields();
        }

        [Test]
        public void Initialize_WhenIgnoreSuppressionIsTrue_TransactionalEmailErrorIsShown()
        {
            // Arrange
            InitilizeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            _campaignItemObject.IgnoreSuppression = (bool?)true;
            var transactionLabel = "Transactional Emails - Don't Apply Suppression";

            // Act
            _testObject.Invoke("Initialize");

            // Assert
            var TransactionalLabel = _testObject.GetFieldOrProperty("lblTransactional") as Label;
            _testObject.ShouldSatisfyAllConditions(
                () => TransactionalLabel.Text.ShouldNotBeNullOrWhiteSpace(),
                () => TransactionalLabel.Text.ShouldBe(transactionLabel),
                () => TransactionalLabel.Visible.ShouldBeTrue());
        }

        [Test]
        public void Initialize_WhenSuppressionListIsNotEmpty_SuppressionRepeaterShowData()
        {
            // Arrange
            InitilizeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            CreateCampaignItemShim();
            var supressionRepeaterShown = false;
            ShimSmartSegment.GetSmartSegmentByIDInt32 = (x) =>
            {
                supressionRepeaterShown = true;
                var dummySmartSegment = CreateInstance(typeof(SmartSegment));
                return dummySmartSegment;
            };

            // Act
            _testObject.Invoke("Initialize");

            // Assert
            supressionRepeaterShown.ShouldBeTrue();
        }

        [Test]
        public void Initialize_WhenSimpleShareDetailIDIsNull_SubscriberTableIsShown()
        {
            // Arrange
            InitilizeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (x) =>
            {
                _dummySocialMedia = CreateInstance(typeof(CampaignItemSocialMedia));
                _dummySocialMedia.SimpleShareDetailID = null;
                var dummySocialMediaList = new List<CampaignItemSocialMedia>
                {
                    _dummySocialMedia
                };
                return dummySocialMediaList;
            };

            // Act
            _testObject.Invoke("Initialize");

            // Assert
            var subscriberTable = _testObject.GetFieldOrProperty("tblSubscriber") as HtmlTable;
            subscriberTable.Visible.ShouldBeTrue();
        }

        [Test]
        public void Initialize_WhenUserIsNotAuthorizedToAccess_ThrowsException()
        {
            // Arrange
            InitilizeFakes();
            InitilizeTestObject();
            InitializePropertiesAndFields();
            ShimKmUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => false;

            //Act, Assert
            AssertNunit.That(() =>
                _testObject.Invoke("Initialize"),
            Throws.InstanceOf<ApplicationException>());
        }

        private void InitilizeFakes()
        {
            ShimKmUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (a, b, c, d) => true;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
                var campaignItemBlastFilterList = new List<CampaignItemBlastFilter>
                {
                    campaignItemBlastFilterObject
                };
                var campaignItemBlastObject = CreateInstanceWithValues(typeof(CampaignItemBlast), new
                {
                    Filters = campaignItemBlastFilterList
                });
                var CampaignItemSuppressionObject = CreateInstance(typeof(CampaignItemSuppression));
                _campaignItemObject = CreateInstanceWithValues(typeof(CampaignItem), new
                {
                    BlastList = new List<CampaignItemBlast>()
                    {
                        campaignItemBlastObject,
                        campaignItemBlastObject
                    },
                    SuppressionList = new List<CampaignItemSuppression>
                    {
                        CampaignItemSuppressionObject
                    },
                });
                _campaignItemObject.IgnoreSuppression = (bool?)true;
                return _campaignItemObject;
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                var dummyLayout = CreateInstance(typeof(Layout));
                return dummyLayout;
            };
            ShimBlastFieldsName.GetByBlastFieldIDInt32User = (a, b) =>
            {
                var dummyFieldsName = CreateInstance(typeof(BlastFieldsName));
                return dummyFieldsName;
            };
            ShimGroup.GetByGroupIDInt32User = (x, y) =>
            {
                var dummyGroup = CreateInstance(typeof(EntityGroup));
                return dummyGroup;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) =>
            {
                var dummyGroup = CreateInstance(typeof(EntityGroup));
                return dummyGroup;
            };
            ShimSmartSegment.GetSmartSegmentByIDInt32 = (x) =>
            {
                var dummySmartSegment = CreateInstance(typeof(SmartSegment));
                return dummySmartSegment;
            };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (x) =>
            {
                var dummyFilter = CreateInstance(typeof(Filter));
                return dummyFilter;
            };
            ShimCampaignItemSuppression.GetByCampaignItemIDInt32UserBoolean = (x, y, z) =>
            {
                var campaignItemSuppressionList = new List<CampaignItemSuppression>();
                var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
                var campaignItemSuppressionObject = CreateInstanceWithValues(typeof(CampaignItemSuppression), new
                {
                    Filters = new List<CampaignItemBlastFilter>()
                        {
                            campaignItemBlastFilterObject
                        }
                });
                campaignItemSuppressionList.Add(campaignItemSuppressionObject);
                return campaignItemSuppressionList;
            };
            ShimECNSession.CurrentSession = () =>
            {
                var dummyCustormer = CreateInstance(typeof(Customer));
                dummyCustormer.CustomerID = 1;
                var dummyUser = CreateInstance(typeof(User));
                dummyUser.UserID = 1;
                var ecnSession = CreateInstance(typeof(ECNSession));
                SetField(ecnSession, "CurrentUser", dummyUser);
                SetField(ecnSession, "CurrentCustomer", dummyCustormer);
                return ecnSession;
            };
            ShimAuthenticationTicket.getTicket = () =>
            {
                var authTkt = CreateInstance(typeof(AuthenticationTicket));
                SetField(authTkt, "CustomerID", 1);
                return authTkt;
            };
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            ShimBlast.GetEstimatedSendsCountStringInt32Boolean = (a, b, c) =>
            {
                var dummyDataTable = new DataTable
                {
                    Columns = { "dummyColumn" },
                };
                dummyDataTable.Rows.Add("1");
                return dummyDataTable;
            };
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (x) =>
            {
                _dummySocialMedia = CreateInstance(typeof(CampaignItemSocialMedia));
                var dummySocialMediaList = new List<CampaignItemSocialMedia>
                {
                    _dummySocialMedia
                };
                return dummySocialMediaList;
            };
            ShimCampaignItemBlastFilter.GetByCampaignItemSuppressionIDInt32 = (x) =>
            {
                var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
                var campaignItemBlastFilterList = new List<CampaignItemBlastFilter>()
                {
                    campaignItemBlastFilterObject
                };
                return campaignItemBlastFilterList;
            };
        }

        private void CreateCampaignItemShim()
        {
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
            {
                var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
                var campaignItemBlastFilterList = new List<CampaignItemBlastFilter>
                {
                    campaignItemBlastFilterObject
                };
                var campaignItemBlastObject = CreateInstanceWithValues(typeof(CampaignItemBlast), new
                {
                    Filters = campaignItemBlastFilterList
                });
                var CampaignItemSuppressionObject = CreateInstance(typeof(CampaignItemSuppression));
                _campaignItemObject = CreateInstanceWithValues(typeof(CampaignItem), new
                {
                    BlastList = new List<CampaignItemBlast>()
                    {
                        campaignItemBlastObject,
                        campaignItemBlastObject
                    },
                    SuppressionList = new List<CampaignItemSuppression>
                    {
                        CampaignItemSuppressionObject
                    },
                });
                _campaignItemObject.IgnoreSuppression = (bool?)false;
                return _campaignItemObject;
            };
        }

        private void InitilizeTestObject()
        {
            var wizardPreview = new WizardPreview_AB();
            wizardPreview.Page = new Page();
            _testObject = new PrivateObject(wizardPreview);
            _campaignItemObject = new CampaignItem();
            _dummySocialMedia = new CampaignItemSocialMedia();
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private dynamic CreateInstanceWithValues(Type type, dynamic values)
        {
            return ReflectionHelper.CreateInstanceWithValues(type, values);
        }

        private void InitializePropertiesAndFields()
        {
            _testObject.SetProperty("CampaignItemID", CampaignItemID);
            _testObject.SetField("lblHeadingA", new Label());
            _testObject.SetField("lblMessageA", new Label());
            _testObject.SetField("lblSubjectA", new Label());
            _testObject.SetField("lblFromEmailA", new Label());
            _testObject.SetField("lblReplyToA", new Label());
            _testObject.SetField("lblFromNameA", new Label());
            _testObject.SetField("lblHeadingB", new Label());
            _testObject.SetField("lblMessageB", new Label());
            _testObject.SetField("lblSubjectB", new Label());
            _testObject.SetField("lblFromEmailB", new Label());
            _testObject.SetField("lblReplyToB", new Label());
            _testObject.SetField("lblFromNameB", new Label());
            _testObject.SetField("lblEstimatedSends", new Label());
            _testObject.SetField("lblSelected", new Label());
            _testObject.SetField("lblSuppresed", new Label());
            _testObject.SetField("lblTransactional", new Label());
            _testObject.SetField("lblBlastFieldHeader", new Label());
            _testObject.SetField("lblBlastField1", new Label());
            _testObject.SetField("lblBlastFieldValue1", new Label());
            _testObject.SetField("lblBlastField2", new Label());
            _testObject.SetField("lblBlastFieldValue2", new Label());
            _testObject.SetField("lblBlastField3", new Label());
            _testObject.SetField("lblBlastFieldValue3", new Label());
            _testObject.SetField("lblBlastField4", new Label());
            _testObject.SetField("lblBlastFieldValue4", new Label());
            _testObject.SetField("lblBlastField5", new Label());
            _testObject.SetField("lblBlastFieldValue5", new Label());
            _testObject.SetField("lblSocialHeader", new Label());
            _testObject.SetField("lblSimpleHeader", new Label());
            _testObject.SetField("lblSubscriberShare", new Label());
            _testObject.SetField("tblSocial", new HtmlTable());
            _testObject.SetField("tblSubscriber", new HtmlTable());
            _testObject.SetField("tblSimple", new HtmlTable());
            _testObject.SetField("gvSimpleShare", new GridView());
            _testObject.SetField("gvSubscriberShare", new GridView());
            _testObject.SetField("rpterGroupDetails", new Repeater());
            _testObject.SetField("rpterSuppression", new Repeater());
            _testObject.SetField("hlPreviewHTML1", new HyperLink());
            _testObject.SetField("hlPreviewTEXT1", new HyperLink());
            _testObject.SetField("hlPreviewHTML2", new HyperLink());
            _testObject.SetField("hlPreviewTEXT2", new HyperLink());
            _testObject.SetField("pnlBlastFields", new Panel());
        }

        private void AssertPropertiesAndFields()
        {
            var firstBlast = _campaignItemObject?.BlastList[0] as CampaignItemBlast;
            var emailLabel = (_testObject.GetFieldOrProperty("lblFromEmailA") as Label)?.Text;
            var emailReplyTo = (_testObject.GetFieldOrProperty("lblReplyToA") as Label)?.Text;
            var emailFrom = (_testObject.GetFieldOrProperty("lblFromNameA") as Label)?.Text;
            _testObject.ShouldSatisfyAllConditions(
                () => firstBlast.ShouldNotBeNull(),
                () => emailLabel.ShouldBe(firstBlast.EmailFrom),
                () => emailReplyTo.ShouldBe(firstBlast.ReplyTo),
                () => emailFrom.ShouldBe(firstBlast.FromName));
        }

        private void InitilizeAssertObjects()
        {
            _errorList = new List<string>();
            _simpleShareDetail = null;
            _campaignItemSocialMedia = null;
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
