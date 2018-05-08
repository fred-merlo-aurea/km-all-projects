using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.ECNWizard.Controls;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardScheduleTest : PageHelper
    {
        private const string DrpCampaignContent = "drpCampaignContent";
        private const string DrpCampaignMedium = "drpCampaignMedium";
        private const string DrpCampaignName = "drpCampaignName";
        private const string DrpCampaignTerm = "drpCampaignTerm";
        private const string CustomValue = "CustomValue";
        private const string SampleCompany = "SampleCompany";
        private const string SomeSetting = "SomeSetting";
        private const string ChkboxOmnitureTracking = "chkboxOmnitureTracking";
        private const string PnlOmniture = "pnlOmniture";
        private const string SomeOtherCompany = "SomeOtherCompany";
        private const string SomeAnotherCompany = "SomeAnotherCompany";
        private const string LoadLinkTrackingParamOptionsMethodName = "LoadLinkTrackingParamOptions";
        private const string TemplateOmniture1 = "Omniture1";
        private const string TemplateOmniture2 = "Omniture2";
        private const string TemplateOmniture3 = "Omniture3";
        private const string TemplateOmniture4 = "Omniture4";
        private const string TemplateOmniture5 = "Omniture5";
        private const string TemplateOmniture6 = "Omniture6";
        private const string TemplateOmniture7 = "Omniture7";
        private const string TemplateOmniture8 = "Omniture8";
        private const string TemplateOmniture9 = "Omniture9";
        private const string TemplateOmniture10 = "Omniture10";
        private const string XMLElementSettings = "Settings";
        private const string XMLElementAllowCustomerOverride = "AllowCustomerOverride";
        private const string XMLElementOverride = "Override";
        private const string XMLElementQueryString = "QueryString";
        private const string XMLElementDelimiter = "Delimiter";

        private WizardSchedule _testEntity;
        private PrivateObject _privateTestObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new WizardSchedule { CampaignItemID = 1 };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = "TestUser", IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
