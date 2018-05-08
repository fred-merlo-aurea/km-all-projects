using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.ECNWizard.Controls;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardSFCampaignTest : PageHelper
    {
        private const string TestUser = "TestUser";
        private WizardSFCampaign _testEntity;
        private PrivateObject _privateTestObject;
        
        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new WizardSFCampaign { CampaignItemID = 1 };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
