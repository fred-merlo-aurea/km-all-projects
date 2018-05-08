using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ecn.accounts.customersmanager;
using ecn.accounts.customersmanager.Fakes;
using ECN.Accounts.Tests.Helper;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using MasterPage = ecn.accounts.MasterPages;

namespace ECN.Accounts.Tests.main.Customers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CustomerDetailTest : PageHelper
    {
        private const string ClientID = "1";
        private const string BaseChannelID = "1";
        private const string CustomerID = "1";
        private const string QuerystringCustomerIDKey = "CustomerID";
        private const string TestExceptionMessage = "UT Exception";
        private const string SampleCustomerValidationMessage = "Customer is not valid";
        private const string SampleCustomer = "SampleCustomer";
        private customerdetail _testEntity;
        private PrivateObject _privateTestObj;
        private Client _savedClient = null;
        private bool _isCustomerValidated;
        private bool _isClientSaved;
        private bool _isGroupSaved;
        private Group _savedGroup;
        private List<string> _sqlCommandExecutedList = new List<string>();

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _isClientSaved = false;
            _savedClient = null;
            _isGroupSaved = false;
            _savedGroup = null;
            base.SetPageSessionContext();
            QueryString.Clear();
            _testEntity = new customerdetail();
            _privateTestObj = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            ShimCurrentUser();
            ShimMasterPage();
            
        }
        private void ShimMasterPage()
        {
            Shimcustomerdetail.AllInstances.MasterGet = (p) => new MasterPage.Accounts();
        }

        private void ShimCurrentUser()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new User { UserID = 1, UserName = "TestUser" };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }
    }
}
