using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AddSubscribersTest : PageHelper
    {
        private const string MethoSave = "Save";
        private const string TestUser = "TestUser";
        private const string Action = "Action";
        private const string Count = "Counts";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private const string One = "12";
        private const int LayoutId = 1;
        private newGroup_add _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new newGroup_add { };
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
        public void Save_Success_ReturnsTrue(string actiontype)
        {
            // Arrange 
            Initialize();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) => CreateImportEmailsTable(actiontype);
            
            // Act
           var result= (bool) _privateTestObject.Invoke(MethoSave, null);

            // Assert
            result.ShouldBeTrue();
        }

        [TestCase("M")]
        public void Save_Failed_ReturnsFalse(string actiontype)
        {
            // Arrange 
            Initialize();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString = (a, b, c, d, e, f, g, h, i, j) => throw new ECNException(null);

            // Act
            var result = (bool)_privateTestObject.Invoke(MethoSave, null);

            // Assert
            result.ShouldBeFalse();
        }

        private void Initialize()
        {
            ReflectionHelper.SetField(_testEntity, "txtEmailAddress", new TextBox { Text = DummyString });
            ReflectionHelper.SetField(_testEntity, "SubscribeTypeCode", new DropDownList
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
            ReflectionHelper.SetField(_testEntity, "FormatTypeCode", new DropDownList
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
        private DataTable CreateImportEmailsTable(string actionType)
        {
            var emailRecordDt = new DataTable();
            emailRecordDt.Columns.Add(Action);
            emailRecordDt.Columns.Add(Count);
            emailRecordDt.Rows.Add(actionType, One);
            return emailRecordDt;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = ReflectionHelper.CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}