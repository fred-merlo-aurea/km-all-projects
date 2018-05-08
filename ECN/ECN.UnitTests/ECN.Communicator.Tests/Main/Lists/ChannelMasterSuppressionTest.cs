using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.main.lists;
using ecn.communicator.main.lists.Fakes;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageCommunicator = ecn.communicator.MasterPages.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class ChannelMasterSuppressionTest : PageHelper
    {
        private const string MethoEmailButtonClick = "addEmailBTN_Click";
        private const string MethoExportEmailButtonClick = "exportEmailsBTN_Click"; 
        private const string MethodLoadEmailsGrid = "loadEmailsGrid"; 
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private const string Unsubscribe = "U";
        private const string Subscribe = "S";
        private const string One = "1";
        private const int LayoutId = 1;
        private const int GroupId = 1;
        private const int CutomerDDValue = 0;
        private channelMasterSuppression _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new channelMasterSuppression { };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [Test]
        public void LoadEmailsGrid_Success_EmailsGridIsShown()
        {
            // Arrange 
            Initialize();
            CreateShims();
            
            // Act
            _privateTestObject.Invoke(MethodLoadEmailsGrid, null);

            // Assert
            var exportButton = ReflectionHelper.GetFieldValue(_testEntity, "exportEmailsBTN") as Button;
            var emailsGrid = ReflectionHelper.GetFieldValue(_testEntity, "emailsGrid") as DataGrid;
            _testEntity.ShouldSatisfyAllConditions(
                () => exportButton.ShouldNotBeNull(),
                () => emailsGrid.ShouldNotBeNull(),
                () => emailsGrid.DataSource.ShouldNotBeNull(),
                () => exportButton.Visible.ShouldBeTrue(),
                () => emailsGrid.Visible.ShouldBeTrue());
        }

        [Test]
        public void ExportEmailsBTN_Click_Success_EmailsAreExported()
        {
            // Arrange 
            Initialize();
            CreateShims();
            var emailsExported = false;
            ShimHttpResponse.AllInstances.WriteFileString = (x, y) => 
            {
                emailsExported = true;
            };

            // Act
            _privateTestObject.Invoke(MethoExportEmailButtonClick, null, EventArgs.Empty);

            // Assert
            emailsExported.ShouldBeTrue();
        }

        [TestCase("S")]
        public void AddEmailBTN_Click_WithNoEmailAddress_ResultGridIsHidden(string actionType)
        {
            // Arrange 
            ReflectionHelper.SetField(_testEntity, "emailAddresses", new TextBox { Text = string.Empty });
            ShimchannelMasterSuppression.AllInstances.MasterGet = (x) => new PageCommunicator();
            ShimEmailGroup.ImportEmailsToCSUserInt32String = (x, y, z) => CreateImportEmailsTable(actionType);

            // Act
            _privateTestObject.Invoke(MethoEmailButtonClick, null, EventArgs.Empty);

            // Assert
            var message = ReflectionHelper.GetFieldValue(_testEntity, "MessageLabel") as Label;
            var resultGrid = ReflectionHelper.GetFieldValue(_testEntity, "ResultsGrid") as DataGrid;
            _testEntity.ShouldSatisfyAllConditions(
                () => message.ShouldNotBeNull(),
                () => resultGrid.ShouldNotBeNull(),
                () => message.Visible.ShouldBeTrue(),
                () => resultGrid.Visible.ShouldBeFalse());
        }

        [TestCase("T")]
        [TestCase("I")]
        [TestCase("U")]
        [TestCase("D")]
        [TestCase("S")]
        public void AddEmailBTN_Click_WithOneOrMoreEmailAddress_EmailsAreFetchedAndImportPanelIsVisible(string actionType)
        {
            // Arrange 
            Initialize();
            ShimchannelMasterSuppression.AllInstances.MasterGet = (x) => new PageCommunicator();
            ShimEmailGroup.ImportEmailsToCSUserInt32String = (x, y, z) => CreateImportEmailsTable(actionType);

            // Act
            _privateTestObject.Invoke(MethoEmailButtonClick, null, EventArgs.Empty);

            // Assert
            var importPanel = ReflectionHelper.GetFieldValue(_testEntity, "importResultsPNL") as Panel;
            var resultGrid = ReflectionHelper.GetFieldValue(_testEntity, "ResultsGrid") as DataGrid;
            _testEntity.ShouldSatisfyAllConditions(
                () => importPanel.ShouldNotBeNull(),
                () => resultGrid.ShouldNotBeNull(),
                () => resultGrid.DataSource.ShouldNotBeNull(),
                () => importPanel.Visible.ShouldBeTrue(),
                () => resultGrid.Visible.ShouldBeTrue());
        }

        private void Initialize()
        {
            ReflectionHelper.SetField(_testEntity, "emailAddresses", new TextBox { Text = string.Format("{0} {0}",DummyString) });
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
            ShimchannelMasterSuppression.AllInstances.MasterGet = (x) => new PageCommunicator();
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DummyString);
            ShimFile.ExistsString = (x) => true;
            ShimFile.DeleteString = (x) => { };
            ShimChannelMasterSuppressionList.GetByBaseChannelIDInt32User = (x, y) => new List<ChannelMasterSuppressionList>
            {
                ReflectionHelper.CreateInstance(typeof(ChannelMasterSuppressionList))
            };
            ShimHttpResponse.AllInstances.WriteFileString = (x, y) => { };
            ShimHttpResponse.AllInstances.End = (x) => { };
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
            shimSession.Instance.CurrentCustomer = new Customer() { CustomerID = 1 };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}