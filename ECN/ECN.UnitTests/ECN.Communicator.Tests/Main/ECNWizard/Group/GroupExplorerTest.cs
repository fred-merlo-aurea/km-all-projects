using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class GroupExplorerTest : PageHelper
    {
        private const string GroupIDKey = "GroupID";
        private const string FilterIDKey = "FilterID";
        private const string TestUserName = "TestUser";
        private const string TestUrl = "http://km.com";
        private groupExplorer _groupExplorer;   
        private PrivateObject _privateGroupExplorerObj;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _groupExplorer = new groupExplorer();
            InitializeAllControls(_groupExplorer);
            _privateGroupExplorerObj = new PrivateObject(_groupExplorer);
            _privateGroupExplorerObj.SetField("IsSelect", BindingFlags.Static | BindingFlags.NonPublic, false);
            InitializeSessionFakes();
            QueryString.Clear();
            QueryString.Add(GroupIDKey, null);
            QueryString.Add(FilterIDKey, null);

            ShimUserControl.AllInstances.RequestGet = (p) =>
            {
                return new HttpRequest(string.Empty, TestUrl, string.Empty);
            };
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1 };
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUserName, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
