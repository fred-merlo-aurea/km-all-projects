using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager.addressloader;
using ecn.communicator.listsmanager.addressloader.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageCommunicator = ecn.communicator.MasterPages.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestClass]
    public class AddressLoaderTest : PageHelper
    {
        private const string MethoAddEmails = "AddEmails";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private const string Unsubscribe = "U";
        private const string Subscribe = "S";
        private const string One = "1";
        private const int LayoutId = 1;
        private const int GroupId = 1;
        private const int CutomerDDValue = 0; 
        private addressloader _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new addressloader { };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [TestCase("T")]
        [TestCase("I")]
        [TestCase("U")]
        [TestCase("D")]
        [TestCase("S")]
        [TestCase("M")]
        public void AddEmails_WhenUnsubscribed_EmailsGroupsAreFetched(string actionType)
        {
            // Arrange 
            Initialize();
            CreateShims();
            var emailGroupsFetched = false;
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) =>
            {
                emailGroupsFetched = true;
                return CreateInstance(typeof(EmailGroup));
            };
            SetSubscribeTypeCode(Unsubscribe);
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) => CreateImportEmailsTable(actionType);

            // Act
            _privateTestObject.Invoke(MethoAddEmails, null, EventArgs.Empty);

            // Assert
            emailGroupsFetched.ShouldBeTrue();
        }

        [TestCase("T")]
        [TestCase("I")]
        [TestCase("U")]
        [TestCase("D")]
        [TestCase("S")]
        [TestCase("M")]
        public void AddEmails_WhenUnsubscribed_EmailsGroupsAreNotFetched(string actionType)
        {
            // Arrange 
            Initialize();
            CreateShims();
            var emailGroupsFetched = false;
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) =>
            {
                emailGroupsFetched = true;
                return CreateInstance(typeof(EmailGroup));
            };
            SetSubscribeTypeCode(Subscribe);
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) => CreateImportEmailsTable(actionType);

            // Act
            _privateTestObject.Invoke(MethoAddEmails, null, EventArgs.Empty);

            // Assert
            emailGroupsFetched.ShouldBeFalse();
        }

        private void Initialize()
        {
            SetField(_testEntity, "Addresses", new TextBox { Text = DummyString });
            SetField(_testEntity, "FormatTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = DummyString
                    }
                }
            });
        }

        private void SetSubscribeTypeCode(string code)
        {
            SetField(_testEntity, "SubscribeTypeCode", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = code
                    }
                }
            });
        }

        private DataTable CreateImportEmailsTable(string actionType)
        {
            var importEmailTable = new DataTable();
            importEmailTable.Columns.Add("Action");
            importEmailTable.Columns.Add("Counts");
            importEmailTable.Rows.Add(actionType, One);
            return importEmailTable;
        }

        private void CreateShims()
        {
            var groupObject = CreateInstance(typeof(Group));
            var groupList = new List<Group> { groupObject };
            Shimaddressloader.AllInstances.GetSelectedGroups = (x) => groupList;
            ShimPage.AllInstances.IsValidGet = (x) => true;
            Shimaddressloader.AllInstances.MasterGet = (x) => new PageCommunicator();
            ShimCommunicator.AllInstances.CutomerDDValue = (x) => CutomerDDValue;
            ShimGroup.GetMasterSuppressionGroupInt32User = (x, y) => CreateInstance(typeof(Group));
            groupObject.IsSeedList = false;
            groupObject.GroupID = GroupId;
            ShimGroup.GetByGroupIDInt32User = (x, y) => groupObject;
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) => CreateInstance(typeof(EmailGroup));
            ShimEmail.IsValidEmailAddressString = (x) => true;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client
            };
            shimSession.Instance.CurrentCustomer = new Customer() { CustomerID = 1 };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetFieldValue(obj, fieldName);
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private void SetProperty(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetProperty(obj, fieldName, value);
        }

        private dynamic GetProperty(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetPropertyValue(obj, fieldName);
        }
    }
}
