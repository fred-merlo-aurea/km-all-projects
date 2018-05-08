using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.ECNWizard.Controls;
using ecn.communicator.main.ECNWizard.Group;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardGroupTest : PageHelper
    {
        private WizardGroup _testEntity;
        private groupExplorer _groupExplorer; 
        private PrivateObject _privateTestObject;
        private PrivateObject _privateGroupExplorerObj;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new WizardGroup { CampaignItemID = 1 };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            _groupExplorer = (groupExplorer)_privateTestObject.GetFieldOrProperty("groupExplorer1");
            InitializeAllControls(_groupExplorer);
            _privateGroupExplorerObj = new PrivateObject(_groupExplorer);
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
