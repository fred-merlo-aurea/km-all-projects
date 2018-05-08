using System;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.common.classes.Fakes;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;
using CommunicatorFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using EcnCommunicator = ecn.communicator.classes;
using EcnCommunicatorFakes = ecn.communicator.classes.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ListImport_XMLEmailTest
    {
        private ListImport_XML _testEntity;
        private DataTable _dataTable;
        private DataTable _dataTable1;
        private IDisposable _shims;
        private const string SampleEcnAccessKey = "{2B1BDF1E-E365-41FA-BEDA-7BDCAF6F1D76}";
        private const string Response = "SampleResponse";
        private const string Name = "name";
        private const string ActionType = "M";
        private const int Id = 10;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
            _dataTable?.Dispose();
            _dataTable1?.Dispose();
        }

        [Test]
        public void ImportEmailProfiles_ForUserAndAuthorized_ReturnResponse()
        {
            // Arrange
            _testEntity = new ListImport_XML();
            InitializeImport();

            // Act
            var result = _testEntity.ImportEmailProfiles(SampleEcnAccessKey, Id, SampleEcnAccessKey);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void ImportEmailProfilesSF_ForUserAndAuthorized_ReturnResponse()
        {
            // Arrange
            _testEntity = new ListImport_XML();
            InitializeImport();
            InitializeEmailSf();

            // Act
            var result = _testEntity.ImportEmailProfilesSF(SampleEcnAccessKey, Id, SampleEcnAccessKey, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void UpdateEmailAddress_ForUserAndAuthorized_ReturnResponse()
        {
            // Arrange
            _testEntity = new ListImport_XML();
            InitializeImport();
            InitializeEmailSf();

            // Act
            var result = _testEntity.UpdateEmailAddress(SampleEcnAccessKey, Id, SampleEcnAccessKey, Name, Name, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        private void InitializeEmailSf()
        {
            EcnCommunicatorFakes.ShimEmails.GetEmailByIDInt32 = (x) => new EcnCommunicator.Emails();
            EcnCommunicatorFakes.ShimGroups.AllInstances.UDFHashGet = (x) => new SortedList();
            CommunicatorFakes.ShimEmailDirect.SaveEmailDirect = (x) => Id;
            ShimListImport_XML.AllInstances.ReplaceCodeSnippetsGroupsEmailsStringDataRow = (x, y, z, q, w) => Name;
            ShimListImport_XML.AllInstances.importDataWithUpdateDataTableStringStringString = (x, y, z, q, w) => true;
        }

        private void InitializeImport()
        {
            _dataTable = new DataTable()
            {
                Columns = { "BaseChannelID", "CustomerID", "CustomerName",
                    "UserID", "ShortName", "GroupDataFieldsID" , "user_", "emailaddress",
                    "Action", "Response_FromEmail", "Response_UserMsgSubject", "Response_UserMsgBody",
                    "Response_AdminEmail", "Response_AdminMsgSubject", "Response_AdminMsgBody" },
                Rows = { { Id, Id, Name, Id, "", Id , "", Name, Name, Name, Name, Name, Name, Name, Name } }
            };
            ShimSQLHelper.getDataTableStringString = (x, y) => _dataTable;
            ShimSQLHelper.executeScalarStringString = (x, y) => Id;
            ShimListImport_XML.AllInstances.extractColumnNamesFromXMLString = (x) => _dataTable;
            ShimSendResponse.responseStringStringInt32String = (x, y, z, q) => Response;
            BusinessLogicFakes.ShimUser.ECN_GetByAccessKeyStringBoolean = (x, y) => new User();
            _dataTable1 = new DataTable()
            {
                Columns = {"Action", "Counts"},
                Rows = { { ActionType, Id } }
            };
            CommunicatorFakes.ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x, y, z, q, w, e, r, t, u, i) => _dataTable1;
        }
    }
}
