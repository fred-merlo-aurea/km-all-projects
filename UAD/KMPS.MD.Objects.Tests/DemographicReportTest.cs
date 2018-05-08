using System;
using System.Collections.Generic;
using System.Data.SqlClient.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAD.DataAccess.Fakes;
using KM.Common.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    public class DemographicReportTest
    {

        private readonly ClientConnections _connection = new ClientConnections();
        private IDisposable _shims;
        private bool _wasReadExecuted;
        private ShimSqlDataReader _reader;
        private const string ExpectedDescription = "Description";
        private readonly StringBuilder ExpectedQueries = new StringBuilder("Queries");
        private const int ExpectedMasterGroup = 40;
        private const int ExpectedBrandId = 30;
        private const bool ExpectedIsRecentlyViewed = true;
        private const int ExpectedPubId = 20;
        private const int ExpectedResponseGroups = 10;
        private readonly object Result = new object();


        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            Fakes.ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();

            ShimDynamicBuilder<DemographicReport>.CreateBuilderIDataRecord = (_) =>
               new ShimDynamicBuilder<DemographicReport>();

            _reader = new ShimSqlDataReader();
            ShimSqlCommand.AllInstances.ExecuteReader = (_) => _reader;
            
            _wasReadExecuted = false;
            ShimSqlDataReader.AllInstances.Read = (_) =>
            {
                if (!_wasReadExecuted)
                {
                    _wasReadExecuted = true;
                    return true;
                }
                return false;
            };
        }

        [TearDown]
        public void TearDown()
        {
            _reader?.Instance?.Dispose();
            _shims?.Dispose();
        }

        [Test]
        public void Get_ParametersProvided_DemographicReportList()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;
           
            // Act
            var actualDemographicReport = DemographicReport.Get(_connection, new StringBuilder(), 10, "Descr", 20);

            // Assert
            actualDemographicReport.ShouldSatisfyAllConditions(
                () => actualDemographicReport.ShouldHaveSingleItem(),
                () => actualDemographicReport.ShouldContain(expectedReport));
        }

        [Test]
        public void Get_ParametersProvided_CallsExecuteReaderWithExpectedParameters()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            var queries = string.Empty;
            var responseGroup = 0;
            var description = string.Empty;
            var pubId = 0;
            var commandText = String.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                queries = command.Parameters["@Queries"].Value.ToString();
                responseGroup = (int)command.Parameters["@ResponseGroupID"].Value;
                description = command.Parameters["@Description"].Value.ToString();
                pubId = (int)command.Parameters["@PubID"].Value;
                commandText = command.CommandText;
                return _reader;
            };

            // Act
            DemographicReport.Get(_connection, ExpectedQueries, ExpectedResponseGroups, ExpectedDescription, ExpectedPubId );

            // Assert
            Result.ShouldSatisfyAllConditions(
                () => queries.ShouldBe(ExpectedQueries.ToString()),
                () => responseGroup.ShouldBe(ExpectedResponseGroups),
                () => description.ShouldBe(ExpectedDescription),
                () => pubId.ShouldBe(ExpectedPubId),
                () => commandText.ShouldBe("sp_Subscriber_Codesheet_counts"));
        }

        [Test]
        public void GetGetWithPermission_ParametersProvided_DemographicReportList()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            // Act
            var actualDemographicReport = DemographicReport.GetWithPermission(_connection, new StringBuilder(), 10, "Descr", 20);

            // Assert
            actualDemographicReport.ShouldSatisfyAllConditions(
                () => actualDemographicReport.ShouldHaveSingleItem(),
                () => actualDemographicReport.ShouldContain(expectedReport));
        }

        [Test]
        public void GetGetWithPermission_ParametersProvided_CallsExecuteReaderWithExpectedParameters()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            var queries = string.Empty;
            var responseGroup = 0;
            var description = string.Empty;
            var pubId = 0;
            var commandText = String.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                queries = command.Parameters["@Queries"].Value.ToString();
                responseGroup = (int)command.Parameters["@ResponseGroupID"].Value;
                description = command.Parameters["@Description"].Value.ToString();
                pubId = (int)command.Parameters["@PubID"].Value;
                commandText = command.CommandText;
                return _reader;
            };

            // Act
            DemographicReport.GetWithPermission(_connection, ExpectedQueries, ExpectedResponseGroups, ExpectedDescription, ExpectedPubId);

            // Assert
            Result.ShouldSatisfyAllConditions(
                () => queries.ShouldBe(ExpectedQueries.ToString()),
                () => responseGroup.ShouldBe(ExpectedResponseGroups),
                () => description.ShouldBe(ExpectedDescription),
                () => pubId.ShouldBe(ExpectedPubId),
                () => commandText.ShouldBe("sp_Subscriber_Codesheet_counts_With_Permissions"));
        }


        [Test]
        public void GetData_ParametersProvided_DemographicReportList()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            // Act
            var actualDemographicReport = DemographicReport.GetData(_connection, new StringBuilder(), 10, "Descr", 20, false);

            // Assert
            actualDemographicReport.ShouldSatisfyAllConditions(
                () => actualDemographicReport.ShouldHaveSingleItem(),
                () => actualDemographicReport.ShouldContain(expectedReport));
        }

        [Test]
        public void GetData_ParametersProvided_CallsExecuteReaderWithExpectedParameters()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            var queries = string.Empty;
            var masterGroup = 0;
            var description = string.Empty;
            var brandId = 0;
            bool? isRecentlyViewed = null;
            var commandText = String.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                queries = command.Parameters["@Queries"].Value.ToString();
                masterGroup = (int)command.Parameters["@masterGroup"].Value;
                description = command.Parameters["@Description"].Value.ToString();
                brandId = (int)command.Parameters["@BrandID"].Value;
                isRecentlyViewed = (bool)command.Parameters["@IsRecencyView"].Value;
                commandText = command.CommandText;
                return _reader;
            };          

            // Act
            DemographicReport.GetData(_connection, ExpectedQueries, ExpectedMasterGroup, ExpectedDescription, ExpectedBrandId, ExpectedIsRecentlyViewed);

            // Assert
            Result.ShouldSatisfyAllConditions(
                ()=>queries.ShouldBe(ExpectedQueries.ToString()),
                ()=>masterGroup.ShouldBe(ExpectedMasterGroup),
                ()=>description.ShouldBe(ExpectedDescription),
                ()=>brandId.ShouldBe(ExpectedBrandId),
                ()=>isRecentlyViewed.ShouldBe(ExpectedIsRecentlyViewed),
                ()=>commandText.ShouldBe("sp_Subscriber_MasterCodesheet_counts"));
        }

        [Test]
        public void GetDataWithPermissions_ParametersProvided_DemographicReportList()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            // Act
            var actualDemographicReport = DemographicReport.GetDataWithPermission(_connection, new StringBuilder(), 10, "Descr", 20, false);

            // Assert
            actualDemographicReport.ShouldSatisfyAllConditions(
                () => actualDemographicReport.ShouldHaveSingleItem(),
                () => actualDemographicReport.ShouldContain(expectedReport));
        }

        [Test]
        public void GetDataWithPermissions_ParametersProvided_CallsExecuteReaderWithExpectedParameters()
        {
            // Arrange            
            var expectedReport = new DemographicReport { };
            ShimDynamicBuilder<DemographicReport>.AllInstances.BuildIDataRecord = (_, __) => expectedReport;

            var queries = string.Empty;
            var masterGroup = 0;
            var description = string.Empty;
            var brandId = 0;
            bool? isRecentlyViewed = null;
            var commandText = String.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                queries = command.Parameters["@Queries"].Value.ToString();
                masterGroup = (int)command.Parameters["@masterGroup"].Value;
                description = command.Parameters["@Description"].Value.ToString();
                brandId = (int)command.Parameters["@BrandID"].Value;
                isRecentlyViewed = (bool)command.Parameters["@IsRecencyView"].Value;
                commandText = command.CommandText;
                return _reader;
            };

            // Act
            DemographicReport.GetDataWithPermission(_connection, ExpectedQueries, ExpectedMasterGroup, ExpectedDescription, ExpectedBrandId, ExpectedIsRecentlyViewed);

            // Assert
            Result.ShouldSatisfyAllConditions(
                () => queries.ShouldBe(ExpectedQueries.ToString()),
                () => masterGroup.ShouldBe(ExpectedMasterGroup),
                () => description.ShouldBe(ExpectedDescription),
                () => brandId.ShouldBe(ExpectedBrandId),
                () => isRecentlyViewed.ShouldBe(ExpectedIsRecentlyViewed),
                () => commandText.ShouldBe("sp_Subscriber_MasterCodesheet_counts_With_Permissions"));
        }
    }
}
