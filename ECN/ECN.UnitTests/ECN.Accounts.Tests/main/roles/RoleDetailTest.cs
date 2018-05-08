using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.accounts.main.roles;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ecn.accounts.MasterPages.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using ecn.accounts.main.roles.Fakes;

namespace ECN.Accounts.Tests.main.roles
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class RoleDetailTest : PageHelper
    {
        private roledetail _testEntity;
        private PrivateObject _privateTestObject;
        
        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new roledetail();
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

            Shimroledetail.AllInstances.MasterGet = (p) => new ecn.accounts.MasterPages.Accounts();
            ShimAccounts.AllInstances.UserSessionGet = (a) => shimSession.Instance;
        }
    }
}
