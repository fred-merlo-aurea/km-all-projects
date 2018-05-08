using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ecn.communicator.main.Omniture;
using ecn.communicator.main.Omniture.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Omniture
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class OmnitureBaseChannelSetupTestPageLoad
    {
        private OmnitureBaseChannelSetup _page;
        private PrivateObject _pagePrivate;
        private IDisposable _shimsContext;
        private User _currentUser;
        private BaseChannel _currentBaseChannel;
        private Customer _currentCustomer;
        private PlaceHolder _phError;
        private Panel _pnlNoAccess;
        private Panel _pnlContent;
        private CheckBox _chkboxOverride;
        private TextBox _txtQueryName;
        private TextBox _txtDelimiter;
        private string _xmlQueryString;
        private string _xmlDelimiter;
        private bool _xmlAllowCustomrOverride;
        private ECNException _linkTrackingParamGetByLinkTrackingIDException;
        private readonly Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _xmlQueryString = GetString();
            _xmlDelimiter = GetString();
            _xmlAllowCustomrOverride = true;
            _linkTrackingParamGetByLinkTrackingIDException = null;
            _shimsContext = ShimsContext.Create();
            CommonShims();
            _page = new OmnitureBaseChannelSetup();
            _pagePrivate = new PrivateObject(_page);
            InitializeFields();
        }

        [TearDown]
        public void TearDown()
        {
            _page.Dispose();
            _shimsContext.Dispose();
        }

        [Test]
        public void PageLoad_UserHasNoAccess_ShowError()
        {
            //Arrange
            _currentUser.IsActive = false;
            //Act
            CallPageLoad();
            //Assert
            _pnlContent.Visible.ShouldBeFalse();
            _pnlNoAccess.Visible.ShouldBeTrue();
            _phError.Visible.ShouldBeFalse();
        }

        [Test]
        public void PgeLoad_UseWithAccess_XmlConfigWillShown()
        {
            //Arrange, Act
            CallPageLoad();
            //Assert
            _chkboxOverride.Checked.ShouldBe(_xmlAllowCustomrOverride);
            _txtQueryName.Text.ShouldBe(_xmlQueryString);
            _txtDelimiter.Text.ShouldBe(_xmlDelimiter);
        }

        [Test]
        public void PgeLoad_UseWithAccess_ShouldBindControls()
        {
            //Arrange, Act
            CallPageLoad();
            //Asssert
            AssertControlsProperlyConfigured();
        }

        [Test]
        public void PgeLoad_WhenECNExceptionThrown_ShouldShowError()
        {
            //Arrange
            var error = new ECNError
            {
                Entity = Enums.Entity.BlastAB,
                ErrorMessage = GetString()
            };
            var errors = new List<ECNError> { error };
            _linkTrackingParamGetByLinkTrackingIDException = new ECNException(errors);
            var lblErrorMessage = GetReferenceField<Label>("lblErrorMessage");
            //Act
            CallPageLoad();
            //Asssert
            lblErrorMessage.Text.ShouldBe($"<br/>{error.Entity}: {error.ErrorMessage}");
            _phError.Visible.ShouldBeTrue();
        }

        private void AssertControlsProperlyConfigured()
        {
            for (int i = 1; i <= 10; i++)
            {
                var imgbtnOmni = GetReferenceField<ImageButton>($"imgbtnOmni{i}");
                var ddlOmniDefault = GetReferenceField<DropDownList>($"ddlOmniDefault{i}");
                var rblCustomOmni = GetReferenceField<RadioButtonList>($"rblCustomOmni{i}");
                var rblReqOmni = GetReferenceField<RadioButtonList>($"rblReqOmni{i}");
                var lblOmniture = GetReferenceField<Label>($"lblOmniture{i}");
                lblOmniture.Text.ShouldBe($"omniture{i}");
                lblOmniture.Enabled.ShouldBeTrue();
                imgbtnOmni.Attributes["LTPID"].ShouldBe(i.ToString());
                imgbtnOmni.Enabled.ShouldBeTrue();
                ddlOmniDefault.SelectedValue.ShouldBe("1");
                ddlOmniDefault.Enabled.ShouldBeTrue();
                rblCustomOmni.Enabled.ShouldBeTrue();
                rblReqOmni.Enabled.ShouldBeTrue();
                lblOmniture.Enabled.ShouldBeTrue();
            }
        }

        private void CommonShims()
        {
            ShimECNSession.Constructor = instance => { };
            var session = CreateInstance<ECNSession>();
            _currentUser = new User
            {
                IsActive = true,
                UserID = GetNumber(),
                CustomerID = GetNumber(),
                CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };
            _currentBaseChannel = new BaseChannel
            {
                BaseChannelID = GetNumber()
            };
            _currentCustomer = new Customer
            {
                BaseChannelID = _currentBaseChannel.BaseChannelID,
                CustomerID = _currentUser.CustomerID
            };
            session.CurrentUser = _currentUser;
            session.CurrentBaseChannel = _currentBaseChannel;
            session.CurrentCustomer = _currentCustomer;
            ShimOmnitureBaseChannelSetup.AllInstances.MasterGet = instance =>
            {
                return new ShimCommunicator
                {
                    UserSessionGet = () => session
                };
            };
            ShimECNSession.CurrentSession = () => session;
            ShimPage.AllInstances.IsPostBackGet = instance => false;
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (baseChannelId, linkTrackingId) =>
            {
                return new LinkTrackingSettings
                {
                    XMLConfig = CreateLinkTrackingSettingsXmlConfig(_xmlQueryString, _xmlDelimiter,
                        _xmlAllowCustomrOverride)
                };
            };
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures =
                (customerId, serviceCode, featureCode) => true;
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (id) =>
            {
                if (_linkTrackingParamGetByLinkTrackingIDException != null)
                {
                    throw _linkTrackingParamGetByLinkTrackingIDException;
                }
                return Enumerable.Range(1, 10)
                    .Select(i => new LinkTrackingParam
                    {
                        DisplayName = $"omniture{i}",
                        LTID = i,
                        IsActive = true,
                        LTPID = i
                    }).ToList();
            };
            ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (lptId, baseChannelId) =>
            {
                return new LinkTrackingParamSettings
                {
                    DisplayName = $"omniture{lptId}",
                    IsRequired = true,
                    IsDeleted = true,
                    AllowCustom = true,
                };
            };
            ShimLinkTrackingParamOption.Get_LTPID_BaseChannelIDInt32Int32 = (lptId, baseChannelId) =>
            {
                return new List<LinkTrackingParamOption>
                {
                    new LinkTrackingParamOption
                    {
                        IsActive = true,
                        IsDeleted = false,
                        IsDefault = true,
                        IsDynamic = true,
                        LTPOID = 1,
                        LTPID = 1,
                        BaseChannelID = 1,
                        DisplayName = $"omniture{lptId}"
                    }
                };
            };
        }

        private string CreateLinkTrackingSettingsXmlConfig(
            string queryString,
            string delimiter,
            bool allowCustomerOverride)
        {
            XElement element = new XElement("Settings",
                new XElement("QueryString", queryString),
                new XElement("Delimiter", delimiter),
                new XElement("AllowCustomerOverride", allowCustomerOverride));
            return element.ToString();
        }

        private void InitializeFields()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            _page.GetType()
                .GetFields(flags)
                .Where(field => field.GetValue(_page) == null)
                .ToList()
                .ForEach(field => field.SetValue(_page, CreateInstance(field.FieldType)));
            _phError = GetReferenceField<PlaceHolder>("phError");
            _pnlNoAccess = GetReferenceField<Panel>("pnlNoAccess");
            _pnlContent = GetReferenceField<Panel>("pnlContent");
            _chkboxOverride = GetReferenceField<CheckBox>("chkboxOverride");
            _txtQueryName = GetReferenceField<TextBox>("txtQueryName");
            _txtDelimiter = GetReferenceField<TextBox>("txtDelimiter");
        }

        private object CreateInstance(Type type)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var value = type.GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]);
            return value;
        }

        private T CreateInstance<T>() where T : class
        {
            var result = CreateInstance(typeof(T)) as T;
            return result;
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _pagePrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private void CallPageLoad()
        {
            _pagePrivate.Invoke("Page_Load", new object[] { null, null });
        }

        private int GetNumber()
        {
            return _random.Next(10, 100);
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
