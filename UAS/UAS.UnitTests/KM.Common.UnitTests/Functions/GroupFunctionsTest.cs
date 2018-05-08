using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using KM.Common.Fakes;
using KM.Common.Functions;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KM.Common.UnitTests.Functions
{
    [TestFixture]
    public class GroupFunctionsTest
    {
        private const int TestGroupId = 12345;
        private const string TestName = "MyUd fTes tN ame";
        private const string TestNameShort = "MyUd_fTes_tN_ame";
        private const string TestConnectionString = "MyConnectionString";
        private const int TestResult = 1142;
        private const string InsertUdfCommandText = "INSERT INTO GroupDatafields (ShortName, longname, GroupID,IsPublic) VALUES (@ShortName, @LongName, @GroupID, @IsPublic); select @@identity";
        private const string InsertUdfParameterShortName = "@ShortName";
        private const string InsertUdfParameterLongName = "@LongName";
        private const string InsertUdfParameterGroupId = "@GroupID";
        private const string InsertUdfParameterIsPublic = "@IsPublic";
        private const string Yes = "Y";
        private const string UdfExistsCommandText = "SELECT groupdatafieldsID FROM GroupDatafields WHERE isDeleted=0 and  GroupID = 12345 AND replace(ShortName, ' ', '_') = 'MyUd_fTes_tN_ame'";
        private const string GroupName = "SuperGroup";
        private const int CustomerId = 55;

        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void InsertUdf_ParameterSet_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            string actualConnectionString = null;

            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
            {
                actualConnectionString = connectionString;
            };

            ShimDataFunctions.ExecuteScalarSqlCommandBoolean = (command, _) =>
            {
                actualSqlCommand = command.Clone();
                return TestResult;
            };

            // Act
            var result = GroupFunctions.InsertUdf(TestGroupId, TestName, TestConnectionString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(TestResult),
                () => actualConnectionString.ShouldBe(TestConnectionString),
                () => actualSqlCommand.CommandText.ShouldBe(InsertUdfCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => actualSqlCommand.Parameters.Count.ShouldBe(4),
                () => actualSqlCommand.Parameters[InsertUdfParameterShortName].Value.ShouldBe(TestNameShort),
                () => actualSqlCommand.Parameters[InsertUdfParameterLongName].Value.ShouldBe(TestName),
                () => actualSqlCommand.Parameters[InsertUdfParameterGroupId].Value.ShouldBe(TestGroupId),
                () => actualSqlCommand.Parameters[InsertUdfParameterIsPublic].Value.ShouldBe(Yes));
        }

        [Test]
        public void UdfExists_ParameterSet_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            string actualConnectionString = null;

            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
            {
                actualConnectionString = connectionString;
            };

            ShimDataFunctions.ExecuteScalarSqlCommandBoolean = (command, _) =>
            {
                actualSqlCommand = command.Clone();
                return TestResult;
            };

            // Act
            var result = GroupFunctions.UdfExists(TestGroupId, TestName, TestConnectionString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(TestResult),
                () => actualConnectionString.ShouldBe(TestConnectionString),
                () => actualSqlCommand.CommandText.ShouldBe(UdfExistsCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void InsertGroup_FolderId0ParameterSet_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            string actualConnectionString = null;
            const string expectedCommandText = "INSERT INTO Groups (GroupName, GroupDescription, CustomerID, OwnerTypeCode, PublicFolder ) values ('SuperGroup', 'SuperGroup', 55, 'customer' , 0); select @@identity";

            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
            {
                actualConnectionString = connectionString;
            };

            ShimDataFunctions.ExecuteScalarSqlCommandBoolean = (command, _) =>
            {
                actualSqlCommand = command.Clone();
                return TestResult;
            };

            // Act
            var result = GroupFunctions.InsertGroup(GroupName, CustomerId, 0, TestConnectionString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(TestResult),
                () => actualConnectionString.ShouldBe(TestConnectionString),
                () => actualSqlCommand.CommandText.ShouldBe(expectedCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void InsertGroup_FolderIdNot0ParameterSet_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            string actualConnectionString = null;
            const string expectedCommandText = "INSERT INTO Groups (GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder ) values ('SuperGroup', 'SuperGroup', 55, 42, 'customer' , 0); select @@identity";

            ShimSqlConnection.AllInstances.Open = connection => { };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
            {
                actualConnectionString = connectionString;
            };

            ShimDataFunctions.ExecuteScalarSqlCommandBoolean = (command, _) =>
            {
                actualSqlCommand = command.Clone();
                return TestResult;
            };

            // Act
            var result = GroupFunctions.InsertGroup(GroupName, CustomerId, 42, TestConnectionString);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(TestResult),
                () => actualConnectionString.ShouldBe(TestConnectionString),
                () => actualSqlCommand.CommandText.ShouldBe(expectedCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text));
        }
    }
}
