using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="Objects"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ObjectsTest
    {
        private const int Rows = 5;
        private const string EmailAddress = "email-address";
        private const string ProductCode = "product-code";

        private const string ProcGetProductDemographics = "o_SubscriberProduct_Select_Email_ProductCode";
        private const string ProcGetDemographics = "o_SubscriberConsensus_Select_Email";
        private const string ProcGetDimensions = "o_Dimension_Select";
        private const string ProcGetCustomFieldsProduct = "o_ProductCustomField_Select";
        private const string ProcGetCustomFieldsAdHoc = "o_ProductCustomFieldAdHoc_Select";
        private const string ProcGetCustomFieldsConsensus = "o_ConsensusCustomField_Select";
        private const string ProcGetCustomFieldsConsensusAdHoc = "o_ConsensusCustomFieldAdHoc_Select";
        private const string ProcGetCustomFieldsBrand = "o_BrandCustomField_Select";
        private const string ProcGetCustomFieldValues = "o_CustomFieldValue_Select_Product";
        private const string ProcGetConsensusCustomFieldValues = "o_CustomFieldValue_Select_Consensus";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetProductDemographics_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var products = new List<SubscriberProduct>
            {
                typeof(SubscriberProduct)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return products.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetProductDemographics(Client, EmailAddress, ProductCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(EmailAddress),
                () => _sqlCommand.Parameters["@ProductCode"].Value.ShouldBe(ProductCode),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetProductDemographics),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(products).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDemographics_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var consensuses = new List<SubscriberConsensus>
            {
                typeof(SubscriberConsensus)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return consensuses.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetDemographics(Client, EmailAddress);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(EmailAddress),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDemographics),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(consensuses).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetDimensions_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var dimensions = new List<Dimension>
            {
                typeof(Dimension)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return dimensions.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetDimensions(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetDimensions),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(dimensions).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFieldsProduct_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomField>
            {
                typeof(CustomField)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFields_Product(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldsProduct),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFields_AdHoc_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomField>
            {
                typeof(CustomField)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFields_AdHoc(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldsAdHoc),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFields_Consensus_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomField>
            {
                typeof(CustomField)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFields_Consensus(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldsConsensus),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFields_ConsensusAdHoc_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomField>
            {
                typeof(CustomField)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFields_ConsensusAdHoc(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldsConsensusAdHoc),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFieldsBrand_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomFieldBrand>
            {
                typeof(CustomFieldBrand)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFieldsBrand(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldsBrand),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCustomFieldValues_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomFieldValue>
            {
                typeof(CustomFieldValue)
                .CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetCustomFieldValues(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCustomFieldValues),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetConsensusCustomFieldValues_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var customFields = new List<CustomFieldValue>
            {
                typeof(CustomFieldValue).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return customFields.GetSqlDataReader();
            };

            // Act
            var result = Objects.GetConsensusCustomFieldValues(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcGetConsensusCustomFieldValues),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(customFields).ShouldBeTrue(),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
        }
    }
}