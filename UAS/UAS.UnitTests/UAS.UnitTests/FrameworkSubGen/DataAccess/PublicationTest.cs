using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using FrameworkSubGen.DataAccess;
using FrameworkSubGen.DataAccess.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;

using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkSubGen.DataAccess
{
    [TestFixture]
    public class PublicationTest : Fakes
    {
        private Mocks _mocks;
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            SetupFakes(_mocks);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCase("dummy")]
        [TestCase("")]
        public void PublicationTest_SaveBulkXML(string xml)
        {
            // Arrange
            var commandData = new SqlCommandData();
            ShimKMCommonDataFunctions.GetSqlConnectionString = connectionName => new SqlConnection();
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandData.CommandType = command.CommandType;
                commandData.CommandText = command.CommandText;

                commandData.Parameters.Add(command.Parameters[0].ParameterName, command.Parameters[0].Value);
                commandData.ConnectionString = command.Connection.ConnectionString;

                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            var success = Publication.SaveBulkXml(xml);

            // Assert
            success.ShouldBe(true);
            commandData.ShouldSatisfyAllConditions(
                () => commandData.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => commandData.CommandText.ShouldBe("e_Publication_SaveBulkXml"),
                () => commandData.Parameters.Keys.ShouldContain("@xml"),
                () => commandData.Parameters.Count.ShouldBe(1),
                () => commandData.Parameters.Values.First().ShouldBe(xml),
                () => commandData.Parameters.Values.First().GetType().ShouldBe(typeof(string))
             );
        }

        [Test]
        public void PublicationTest_SaveBulkXMLException()
        {
            // Arrange
            ApiLog apiLog = null;

            ShimKMCommonDataFunctions.ExecuteScalarSqlCommandString = (command, connectionString) => 0;
            ShimKMCommonDataFunctions.GetSqlConnectionString = connectionName => new SqlConnection();
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                command.Connection.Dispose();
                command.Dispose();

                throw new InvalidOperationException("PublicationTest_SaveBulkXMLTestApiLog");
            };

            ShimAuthentication.SaveApiLogExceptionStringString = (ex, myClass, myMethod) =>
            {
                apiLog = new ApiLog
                {
                    Entity = myClass,
                    Method = myMethod
                };
            };

            // Act
            var success = Publication.SaveBulkXml(string.Empty);

            // Assert
            success.ShouldBe(false);
            apiLog.ShouldSatisfyAllConditions(
                () => apiLog.Entity.ShouldBe("FrameworkSubGen.DataAccess.Publication"),
                () => apiLog.Method.ShouldBe("SaveBulkXml")
            );
        }
    }
}
