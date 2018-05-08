using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
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
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Omniture
{
    /// <summary>
    /// Unit test for <see cref="OmnitureCustomerSetup"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class OmnitureCustomerSetupTestPageLoad : PageHelper
    {
        private const string PageLoad = "Page_Load";
        private const string PanelContent = "pnlContent";
        private const string PanelNoAccess = "pnlNoAccess";
        private const string CurrentMenuCode = "currentMenuCode";
        private const string BtnSaveSettings = "btnSaveSettings";
        private const string TxtDelimiter = "txtDelimiter";
        private const string TxtQueryName = "txtQueryName";
        private OmnitureCustomerSetup _omnitureCustomerSetup;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _omnitureCustomerSetup = new OmnitureCustomerSetup();
            InitializeAllControls(_omnitureCustomerSetup);
            _privateObject = new PrivateObject(_omnitureCustomerSetup);
            ShimPage.AllInstances.IsPostBackGet = (x) => { return false; };
            CreateCustomerSettingXml();
            CreatePageShimObject();
        }

        [Test]
        public void PageLoad_IsNotASystemAdministrator_ReturnFalseStatus()
        {
            // Arrange
            ShimCurrentUser();
            HttpContext.Current.Session[CurrentMenuCode] = MenuCode.OMNITURE;
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            _privateObject.Invoke(PageLoad, parameters);

            // Assert
            Get<Panel>(_privateObject, PanelContent).Visible.ShouldBeFalse();
            Get<Panel>(_privateObject, PanelNoAccess).Visible.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_PageIsNotPostBackAndAllowCustomerOverrideIsFalse_LoadPageControl()
        {
            // Arrange
            HttpContext.Current.Session[CurrentMenuCode] = MenuCode.OMNITURE;
            CreateAllowCustomerOverrideXmlObject();
            ShimCurrentUser(true);
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            _privateObject.Invoke(PageLoad, parameters);

            // Assert
            AssertMethodResult(false);
        }

        [Test]
        public void PageLoad_PageIsNotPostBackAndAllowCustomerOverrideIsTrue_LoadPageControl()
        {
            // Arrange
            ShimCurrentUser(true);
            HttpContext.Current.Session[CurrentMenuCode] = MenuCode.OMNITURE;
            CreateAllowCustomerOverrideXmlObject(true);
            var parameters = new object[] { this, EventArgs.Empty };

            // Act
            _privateObject.Invoke(PageLoad, parameters);

            // Assert
            AssertMethodResult(true);
        }

        private void CreateAllowCustomerOverrideXmlObject(bool allowCustomerOverride = false)
        {
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings
                {
                    XMLConfig = CreateSettingXml(allowCustomerOverride)
                };
            };
        }

        private void ShimCurrentUser(bool isActive = false)
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = isActive,
                CurrentSecurityGroup = new KMPlatform.Entity.SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                    IsActive = true
                },
                IsPlatformAdministrator = true
            };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1, CustomerName = "TestUser" };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }
        private string CreateSettingXml(bool allowCustomerOverride = false)
        {
            XElement element = new XElement("Settings",
             new XElement("Override", "true"),
             new XElement("QueryString", "a"),
             new XElement("Delimiter", ","),
             new XElement("AllowCustomerOverride", allowCustomerOverride));
            return element.ToString();
        }

        private void CreatePageShimObject()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            ecnSession.CurrentBaseChannel = new BaseChannel { BaseChannelID = 3 };

            Common.Fakes.ShimMasterPageEx.AllInstances.HeadingSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpContentSetString = (masterpageEx, inputString) => { };
            Common.Fakes.ShimMasterPageEx.AllInstances.HelpTitleSetString = (masterpageEx, inputString) => { };

            ShimOmnitureCustomerSetup.AllInstances.MasterGet = (x) =>
            {
                ecn.communicator.MasterPages.Communicator communicator = new ShimCommunicator
                {
                    CurrentMenuCodeGet = () => { return MenuCode.OMNITURE; },
                    CurrentMenuCodeSetEnumsMenuCode = (y) => { },
                    UserSessionGet = () => { return ecnSession; }
                };
                return communicator;
            };
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) =>
            {
                var linkTrackingParamResult = new List<LinkTrackingParam>();
                for (int i = 0; i <= 10; i++)
                {
                    linkTrackingParamResult.Add(new LinkTrackingParam
                    {
                        DisplayName = string.Concat("omniture", i),
                        LTID = i,
                        IsActive = true,
                        LTPID = i
                    });
                }
                return linkTrackingParamResult;
            };
            ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingParamSettings
                {
                    DisplayName = string.Concat("omniture", x),
                    IsRequired = true,
                    IsDeleted = true,
                    AllowCustom = true,
                };
            };
            ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
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
                        DisplayName = string.Concat("omniture", x)
                    }
                  };
            };
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => { return true; };
        }

        private void CreateCustomerSettingXml()
        {
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings
                {
                    XMLConfig = CreateSettingXml()
                };
            };
        }

        private void AssertMethodResult(bool result)
        {
            // Assert image buttong
            for (int i = 1; i < 11; i++)
            {
                var imagebutton = string.Concat("imgbtnOmni", i);
                Get<ImageButton>(_privateObject, imagebutton).Enabled.ShouldBe(result);
            }

            // Assert image buttong
            for (int i = 1; i < 11; i++)
            {
                var dropdown = string.Concat("ddlOmniDefault", i);
                Get<DropDownList>(_privateObject, dropdown).Enabled.ShouldBe(result);
            }

            // Assert image buttong
            for (int i = 1; i < 11; i++)
            {
                var radioButtonList = string.Concat("rblCustomOmni", i);
                Get<RadioButtonList>(_privateObject, radioButtonList).Enabled.ShouldBe(result);
            }

            // Assert image buttong
            for (int i = 1; i < 11; i++)
            {
                var rblReqOmni = string.Concat("rblReqOmni", i);
                Get<RadioButtonList>(_privateObject, rblReqOmni).Enabled.ShouldBe(result);
            }
        }
    }
}
