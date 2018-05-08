using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using AccessSubscriberFinal = FrameworkUAD.DataAccess.SubscriberFinal;
using EntitySubscriberFinal = FrameworkUAD.Entity.SubscriberFinal;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberFinalTest
    {
        private const int MaxEmailLength = 100;
        private const string EmailAddressKey = "EmailAddress";
        private const string SubscribeTypeCodeKey = "SubscribeTypeCode";
        private const string EcnOtherProductsSuppression = "ECN_OtherProductsSuppression";
        private const string EcnThirdPartySuppresion = "ECN_ThirdPartySuppresion";
        private const string InsertType = "Insert";
        private const string UpdateType = "Update";
        private const string SampleEmail = "sample@email.com";
        private const string OpenEmailNode = "<Email>";
        private const string CloseEmailNode = "</Email>";
        private const string StartEmailsNode = "<Emails>";
        private const string EndEmailsNode = "</Emails>";
        private const string UKey = "U";

        private const string CreateAndSaveXmlMethod = "CreateAndSaveXml";
        private const string SaveOrUpdateBulkInsertMethod = "SaveOrUpdateBulkInsert";

        private readonly SubscriberFinal SubscriberFinal = new SubscriberFinal();
        private PrivateObject _privateObject;
        private ClientConnections _clientConnections;
        private string _processCode;
        private IEnumerable<int> _groupIds;

        private List<EntitySubscriberFinal> _subscriberList;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
            _privateObject = new PrivateObject(SubscriberFinal, new PrivateType(typeof(SubscriberFinal)));

            _clientConnections = new ClientConnections();
            _processCode = "Sample Code";
            _groupIds = new List<int>();
            _subscriberList = new List<EntitySubscriberFinal>();
        }

        [TearDown]
        public void CleanUp()
        {
            _context?.Dispose();
        }

        [Test]
        public void CreateAndSaveXml_WhenClientConnectionsIsNull_ThrowsException()
        {
            // Arrange
            _clientConnections = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
                _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnOtherProductsSuppression));
        }

        [Test]
        public void CreateAndSaveXml_WhenProcessCodeIsNull_ThrowsException()
        {
            // Arrange
            _processCode = null;

            // Act, Assert
            Should.Throw<ArgumentException>(() =>
                _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnOtherProductsSuppression));
        }

        [Test]
        public void CreateAndSaveXml_WhenGroupIdsIsNull_ThrowsException()
        {
            // Arrange
            _groupIds = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnOtherProductsSuppression));
        }

        [Test]
        public void CreateAndSaveXml_WhenGroupIdsIsNotNull_ShouldCreateAndSaveXml()
        {
            // Arrange
            _groupIds = CreateGroupIds();
            var processedXml = string.Empty;

            ShimSubscriberFinal.GetEmailListFromEcnInt32 = _ => CreateDefaultDataTable(SampleEmail);

            ShimSubscriberFinal.ECN_OtherProductsSuppressionStringBuilderStringClientConnections =
                (xml, processCode, client) =>
                {
                    processedXml = xml.ToString();
                };

            var resultXml = CreateFinalXml(_groupIds, EcnOtherProductsSuppression);

            // Act
            _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnOtherProductsSuppression);

            // Assert
            processedXml.ShouldSatisfyAllConditions(
                () => processedXml.ShouldNotBeNullOrWhiteSpace(),
                () => processedXml.ShouldBe(resultXml));
        }

        [Test]
        public void CreateAndSaveXml_WhenGroupIdsLengthIsEqualToZero_ShouldNotCreateXml()
        {
            // Arrange
            _groupIds = new List<int>();
            var processedXml = string.Empty;

            ShimSubscriberFinal.GetEmailListFromEcnInt32 = _ => new DataTable();

            ShimSubscriberFinal.ECN_OtherProductsSuppressionStringBuilderStringClientConnections =
                (xml, processCode, client) =>
                {
                    processedXml = xml.ToString();
                };

            // Act
            _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnOtherProductsSuppression);

            // Assert
            processedXml.ShouldBeEmpty();
        }

        [Test]
        [TestCase("sample@email.com")]
        [TestCase("sampleemailwithmorethan100characteres@randomvalue1sampleemailwithmorethan100characteressampleemail.com")]
        public void CreateAndSaveXml_WhenTypeIsEcnThirdPartySuppresion_ShouldCreateAndSaveXml(string sampleEmail)
        {
            // Arrange
            _groupIds = CreateGroupIds();
            var processedXml = string.Empty;

            ShimSubscriberFinal.GetEmailListFromEcnInt32 = _ => CreateDefaultDataTable(sampleEmail);

            ShimSubscriberFinal.ECN_ThirdPartySuppresionStringBuilderStringClientConnections =
                (xml, processCode, client) =>
                {
                    processedXml = xml.ToString();
                };

            var resultXml = CreateFinalXml(_groupIds, EcnThirdPartySuppresion);

            // Act
            _privateObject.Invoke(CreateAndSaveXmlMethod, _clientConnections, _processCode, _groupIds, EcnThirdPartySuppresion);

            // Assert
            processedXml.ShouldSatisfyAllConditions(
                () => processedXml.ShouldNotBeNullOrWhiteSpace(),
                () => processedXml.ShouldBe(resultXml));
        }

        [Test]
        public void SaveOrUpdateBulkInsert_WhenSubscriberListIsNull_ThrowsException()
        {
            // Arrange
            _subscriberList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, InsertType));
        }

        [Test]
        public void SaveOrUpdateBulkInsert_WhenClientConnectionIsNull_ThrowsException()
        {
            // Arrange
            _clientConnections = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, InsertType));
        }

        [Test]
        public void SaveOrUpdateBulkInsert_WhenOperationTypeIsEmpty_ThrowsException()
        {
            // Arrange
            var type = string.Empty;

            // Act, Assert
            Should.Throw<ArgumentException>(() =>
                _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, type));
        }

        [Test]
        public void SaveOrUpdateBulkInsert_WhenOperationTypeIsInsert_ReturnsTrue()
        {
            // Arrange
            _subscriberList = CreateSubscriberList() as List<EntitySubscriberFinal>;

            ShimSubscriberFinal.SaveBulkInsertStringClientConnections = (xml, clientConnection) => true;

            // Act
            var result = _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, InsertType) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(() => result.Value.ShouldBeTrue());
        }

        [Test]
        public void SaveOrUpdateBulkInsert_WhenOperationTypeIsUpdate_ReturnsTrue()
        {
            // Arrange
            _subscriberList = CreateSubscriberList() as List<EntitySubscriberFinal>;

            ShimSubscriberFinal.SaveBulkUpdateStringClientConnections = (xml, clientConnection) => true;

            // Act
            var result = _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, UpdateType) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(() => result.Value.ShouldBeTrue());
        }

        [Test]
        public void SaveOrUpdateBulkInsert_ShouldLogErrorWhenExceptionIsRaised_ReturnsFalse()
        {
            // Arrange
            _subscriberList = CreateSubscriberList() as List<EntitySubscriberFinal>;

            ShimSubscriberFinal.SaveBulkUpdateStringClientConnections = (xml, clientConnection) => throw new ArgumentException();

            ShimFileLog.AllInstances.SaveFileLog = (_, fileLog) => true;

            // Act
            var result = _privateObject.Invoke(SaveOrUpdateBulkInsertMethod, _subscriberList, _clientConnections, UpdateType) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(() => result.Value.ShouldBeFalse());
        }

        private static DataTable CreateDefaultDataTable(string email)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SubscribeTypeCodeKey, typeof(string));
            dataTable.Columns.Add(EmailAddressKey, typeof(string));

            var dataRow = dataTable.NewRow();
            dataRow[SubscribeTypeCodeKey] = UKey;
            dataRow[EmailAddressKey] = email;
            dataTable.Rows.Add(dataRow);

            return dataTable;
        }

        private static IEnumerable<int> CreateGroupIds()
        {
            return new[] {1};
        }

        private static string CreateFinalXml(IEnumerable<int> groupIds, string type)
        {
            var xml = new StringBuilder();

            foreach (var groupId in groupIds)
            {
                var emailList = AccessSubscriberFinal.GetEmailListFromEcn(groupId);
                if (!(emailList?.Rows.Count > 0))
                {
                    continue;
                }
                
                xml.AppendLine("<XML>");

                foreach (DataRow row in emailList.Rows)
                {
                    xml.AppendLine(StartEmailsNode);
                    TryAppendEmailNode(row, xml, type);
                    xml.AppendLine(EndEmailsNode);
                }
                xml.AppendLine("</XML>");
            }

            return xml.ToString();
        }

        private static void TryAppendEmailNode(DataRow row, StringBuilder xml, string type)
        {
            if (type.Equals(EcnOtherProductsSuppression))
            {
                if (row[SubscribeTypeCodeKey].ToString().Equals(UKey, StringComparison.CurrentCultureIgnoreCase))
                {
                    AppendEmailNode(xml, row);
                }
            }
            else
            {
                AppendEmailNode(xml, row);
            }
        }

        private static void AppendEmailNode(StringBuilder xml, DataRow row)
        {
            var email = row[EmailAddressKey].ToString().Trim();
            if (email.Length > MaxEmailLength)
            {
                email = email.Substring(0, MaxEmailLength);
            }

            xml.AppendLine(OpenEmailNode);
            xml.AppendLine(email);
            xml.AppendLine(CloseEmailNode);
        }

        private static IEnumerable<EntitySubscriberFinal> CreateSubscriberList()
        {
            return new List<EntitySubscriberFinal> { new EntitySubscriberFinal() };
        }
    }
}
