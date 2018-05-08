using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using Entity = KMPlatform.Entity;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static KMPS.MD.Objects.Enums;
using ECN_Framework_Entities.Accounts;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class UtilitiesTests
    {
        private const int UserId = 1;
        private const int DummyId = 1;
        private const int CountOfZeroItems = 0;
        private const string DummyString = "DummyString";
        private const string MailStop = "MAILSTOP";
        private const string SequenceId = "SEQUENCEID";
        private const string SubscriptionId = "subscriptionID";
        private const string Email = "EMAIL";
        private const string Title = "TITLE";
        private const string FName = "FNAME";
        private const string FirstName = "FIRSTNAME";
        private const string LName = "LNAME";
        private const string LastName = "LASTNAME";
        private const string Company = "COMPANY";
        private const string Address = "ADDRESS";
        private const string Address1 = "ADDRESS1";
        private const string City = "CITY";
        private const string State = "STATE";
        private const string RegionCode = "REGIONCODE";
        private const string Zip = "ZIP";
        private const string ZipCode = "ZIPCODE";
        private const string Country = "COUNTRY";
        private const string Phone = "PHONE";
        private const string Fax = "FAX";
        private const string Mobile = "MOBILE";
        private const string ActionColumn = "Action";
        private const string Counts = "Counts";

        private IDisposable _shimObject;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase(ViewType.ProductView, 1)]
        [TestCase(ViewType.ConsensusView, 1)]
        [TestCase(ViewType.ConsensusView, 0)]
        public void GetExportFields_OnDemo_ReturnDictionary(ViewType viewType, int brandId)
        {
            // Arrange
            var clientConnections = new ClientConnections();
            var pubIDs = new List<int> { 1, 2 };
            var downloadFieldType = ExportFieldType.Demo;
            var responseGroupID = 1;
            ShimResponseGroup.GetActiveByPubIDClientConnectionsInt32 = (_, __) => new List<ResponseGroup>
            {
                new ResponseGroup
                {
                    ResponseGroupID = responseGroupID,
                    DisplayName = DummyString
                }
            };
            var masterGroupList = new List<MasterGroup>
            {
                new MasterGroup
                {
                    DisplayName = DummyString,
                    ColumnReference = responseGroupID.ToString()
                }
            };
            ShimMasterGroup.GetActiveByBrandIDClientConnectionsInt32 = (_, __) => masterGroupList;
            ShimMasterGroup.GetActiveMasterGroupsSortedClientConnections = (_) => masterGroupList;

            // Act	
            var actualResult = Utilities.GetExportFields(
                clientConnections,
                viewType,
                brandId,
                pubIDs,
                Enums.ExportType.Campaign,
                UserId,
                downloadFieldType,
                false);
            const int ExpectedCount = 2;

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(ExpectedCount),
                () =>
                {
                    var keys = actualResult.Keys.ToList();
                    keys.ShouldNotBeNull();
                    keys[1].ShouldNotBeNull();
                    keys[1].ShouldContain(responseGroupID.ToString());
                }
            );
        }

        [Test]
        [TestCase(ViewType.ProductView, FieldType.Varchar)]
        [TestCase(ViewType.ProductView, FieldType.Int)]
        [TestCase(ViewType.ConsensusView, FieldType.Varchar)]
        [TestCase(ViewType.ConsensusView, FieldType.Int)]
        public void GetExportFields_OnAdhoc_ReturnDictionary(ViewType viewType, FieldType customFieldDataType)
        {
            // Arrange
            var clientConnections = new ClientConnections();
            var pubIDs = new List<int> { 1, 2 };
            var downloadFieldType = ExportFieldType.Adhoc;
            ShimPubSubscriptionsExtensionMapper.GetActiveByPubIDClientConnectionsInt32 = (_, __) =>
                new List<PubSubscriptionsExtensionMapper>
            {
                new PubSubscriptionsExtensionMapper
                {
                    CustomFieldDataType = customFieldDataType.ToString(),
                    StandardField = DummyString
                }
            };
            ShimSubscriptionsExtensionMapper.GetActiveClientConnections = (_) => new List<SubscriptionsExtensionMapper>
            {
                new SubscriptionsExtensionMapper
                {
                    CustomFieldDataType = customFieldDataType.ToString(),
                    StandardField = DummyString
                }
            };

            // Act	
            var actualResult = Utilities.GetExportFields(
                clientConnections,
                viewType,
                1,
                pubIDs,
                Enums.ExportType.Campaign,
                UserId,
                downloadFieldType,
                false);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldHaveSingleItem(),
                () =>
                {
                    var keys = actualResult.Keys.ToList();
                    keys.ShouldNotBeNull();
                    keys[0].ShouldNotBeNull();
                    keys[0].ShouldContain(DummyString);
                }
            );
        }

        [Test]
        [TestCase(ViewType.ProductView, FieldType.Varchar, Enums.ExportType.ECN)]
        [TestCase(ViewType.ProductView, FieldType.Int, Enums.ExportType.Campaign)]
        [TestCase(ViewType.ConsensusView, FieldType.Varchar, Enums.ExportType.ECN)]
        [TestCase(ViewType.ConsensusView, FieldType.Int, Enums.ExportType.Campaign)]
        public void GetExportFields_OnProfile_ReturnDictionary(
            ViewType viewType,
            FieldType customFieldDataType,
            Enums.ExportType exportType)
        {
            // Arrange
            var clientConnections = new ClientConnections();
            var pubIDs = new List<int> { 1, 2 };
            var downloadFieldType = ExportFieldType.Profile;
            ShimPubs.GetActiveClientConnections = (_) => new List<Pubs>
            {
                new Pubs
                {
                    PubID = 1,
                    IsCirc = true
                }
            };
            ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (_, __) => new List<UserDataMask>();

            // Act	
            var actualResult = Utilities.GetExportFields(
                clientConnections,
                viewType,
                1,
                pubIDs,
                exportType,
                UserId,
                downloadFieldType,
                true);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ExportToECN_OnEmptyData_ReturnEmptyHashtable()
        {
            // Arrange
            var exportFields = new List<ExportFields>();
            var dtSubscribers = new DataTable();
            SetupShimsForExportToECNMethod();

            // Act	
            var actualResult = Utilities.ExportToECN(
                DummyId,
                string.Empty,
                DummyId,
                DummyId,
                string.Empty,
                string.Empty,
                exportFields,
                dtSubscribers,
                UserId,
                GroupExportSource.UADManualExport
            );

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(CountOfZeroItems)
            );
        }

        [Test]
        public void ExportToECN_OnNonEmptyData_ReturnHashtable()
        {
            // Arrange
            var exportFields = new List<ExportFields>
            {
                new ExportFields (MailStop, string.Empty, false, DummyId)
                {
                    isECNUDF = true   
                },
                new ExportFields (SequenceId, string.Empty, false, DummyId)
                {
                    isECNUDF = true
                },
                new ExportFields (Phone, string.Empty, false, DummyId)
                {
                    isECNUDF = true
                }
            };
            var dtSubscribers = GetSubscribersDataTable();
            SetupShimsForExportToECNMethod();

            // Act	
            var actualResult = Utilities.ExportToECN(
                DummyId,
                string.Empty,
                DummyId,
                DummyId,
                DummyString,
                DummyString,
                exportFields,
                dtSubscribers,
                UserId,
                GroupExportSource.UADManualExport
            );

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBeGreaterThan(CountOfZeroItems),
                () =>
                {
                    var keys = actualResult.Keys.Cast<string>().ToArray();
                    keys.ShouldNotBeNull();
                    keys.Length.ShouldBeGreaterThan(CountOfZeroItems);
                    keys[0].ShouldNotBeNull();
                    keys[0].ShouldContain(DummyString.ToUpper());
                }
            );
        }

        private void SetupShimsForExportToECNMethod()
        {
            var user = new Entity::User
            {
                UserID = UserId
            };
            ShimUser.AllInstances.SelectUserInt32Boolean = (_, __, ___) => user;
            ShimUser.AllInstances.SetAuthorizedUserObjectsUserInt32Int32 = (_, __, ___, ____) => user;
            ShimCustomer.GetByCustomerIDInt32Boolean = (_, __) => new Customer
            {
                CustomerID = DummyId,
                BaseChannelID = DummyId,
                PlatformClientID = DummyId
            };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (_) => new BaseChannel
            {
                BaseChannelID = DummyId,
                PlatformClientGroupID = DummyId
            };
            ShimUtilities.UdfExistsInt32String = (_, __) => 0;
            ShimUtilities.InsertUdfInt32String = (_, __) => 1;
            var dtImportedRecords = new DataTable();
            dtImportedRecords.Columns.Add(ActionColumn, typeof(string));
            dtImportedRecords.Columns.Add(Counts, typeof(string));
            var importRow = dtImportedRecords.NewRow();
            importRow[ActionColumn] = DummyString.ToUpper();
            importRow[Counts] = DummyId;
            var importRow2 = dtImportedRecords.NewRow();
            importRow2[ActionColumn] = DummyString.ToUpper();
            importRow2[Counts] = DummyId;
            dtImportedRecords.Rows.Add(importRow);
            dtImportedRecords.Rows.Add(importRow2);
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (usr, cusId, gId, profile, xml, code, sCode, email, fName, src) => dtImportedRecords;
        }

        private DataTable GetSubscribersDataTable()
        {
            var dataTable= new DataTable();
            dataTable.Columns.Add(SubscriptionId, typeof(string));
            dataTable.Columns.Add(Email, typeof(string));
            dataTable.Columns.Add(Title, typeof(string));
            dataTable.Columns.Add(FName, typeof(string));
            dataTable.Columns.Add(FirstName, typeof(string));
            dataTable.Columns.Add(LName, typeof(string));
            dataTable.Columns.Add(LastName, typeof(string));
            dataTable.Columns.Add(Company, typeof(string));
            dataTable.Columns.Add(Address, typeof(string));
            dataTable.Columns.Add(Address1, typeof(string));
            dataTable.Columns.Add(City, typeof(string));
            dataTable.Columns.Add(State, typeof(string));
            dataTable.Columns.Add(RegionCode, typeof(string));
            dataTable.Columns.Add(Zip, typeof(string));
            dataTable.Columns.Add(ZipCode, typeof(string));
            dataTable.Columns.Add(Country, typeof(string));
            dataTable.Columns.Add(Phone, typeof(string));
            dataTable.Columns.Add(Fax, typeof(string));
            dataTable.Columns.Add(Mobile, typeof(string));
            dataTable.Columns.Add(MailStop, typeof(string));
            dataTable.Columns.Add(SequenceId, typeof(string));
            var row = dataTable.NewRow();
            var row2 = dataTable.NewRow();
            var row3 = dataTable.NewRow();
            foreach (DataColumn column in dataTable.Columns)
            {
                row[column] = DummyString;
                row2[column] = DummyString;
                row3[column] = DummyString;
            }
            row3[SubscriptionId] = DummyId;
            dataTable.Rows.Add(row);
            dataTable.Rows.Add(row2);
            dataTable.Rows.Add(row3);
            return dataTable;
        }
    }
}
