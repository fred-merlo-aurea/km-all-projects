using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="MasterCodeSheet"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MasterCodeSheetTests
    {
        private const int Rows = 5;
        private const int MasterId = 2;
        private const int MasterGroupId = 1;
        private const string MasterValue = "master-value";
        private const int BrandId = 4;
        private const string Name = "Name";
        private const string SearchCriteria = "Search-criteria";
        private const int CurrentPage = 7;
        private const int PageSize = 8;
        private const string SortDirection = "Sort-direction";
        private const string SortColumn = "Sort-column";
        private const int CodeSheetId = 11;
        private const string MastercodesheetXml = "mastercodesheet-xml";
        private const string ProcExistsByIdMasterValueMasterGroupId = "e_MasterCodeSheet_Exists_ByIDMasterValueMasterGroupID";
        private const string ProcSelect = "e_MasterCodeSheet_Select";
        private const string ProcSelectById = "e_MasterCodeSheet_Select_ID";
        private const string ProcSelectMasterGroupId = "e_MasterCodeSheet_Select_MasterGroupID";
        private const string ProcSelectMasterGroupBrandId = "e_MasterCodeSheet_Select_ByBrandID";
        private const string ProcSelectByMasterCodeSheetSearch = "e_MasterCodeSheet_Select_Search";
        private const string ProcSelectByCodeSheetId = "e_MasterCodeSheet_Select_CodeSheetID";
        private const string ProcUpdateSort = "e_MasterCodeSheet_Update_SortOrder";
        private const string ProcImportSubscriber = "e_MasterCodeSheet_Import_Subscriber";
        private const string ProcDeleteMasterId = "e_MasterCodeSheet_Delete_MasterID";
        private const string ProcCreateNoValueRespones = "job_MasterCodeSheet_AddNoValue";
        private const string ConnectionString = "connection-string";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.MasterCodeSheet> _list;
        private Entity.MasterCodeSheet _objWithRandomValues;
        private Entity.MasterCodeSheet _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private DataSet _dataSet;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataSet = new DataSet();

            _objWithRandomValues = typeof(Entity.MasterCodeSheet).CreateInstance();
            _objWithDefaultValues = typeof(Entity.MasterCodeSheet).CreateInstance(true);

            _list = new List<Entity.MasterCodeSheet>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void ExistsByIDMasterValueMasterGroupID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.ExistsByIDMasterValueMasterGroupID(MasterId, MasterGroupId, MasterValue, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterID"].Value.ShouldBe(MasterId),
                () => _sqlCommand.Parameters["@MasterValue"].Value.ShouldBe(MasterValue),
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByIdMasterValueMasterGroupId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.SelectByID(MasterId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterID"].Value.ShouldBe(MasterId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectById),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectMasterGroupID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.SelectMasterGroupID(Client, MasterGroupId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectMasterGroupId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectMasterGroupBrandID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.SelectMasterGroupBrandID(Client, BrandId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@BrandID"].Value.ShouldBe(BrandId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectMasterGroupBrandId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByMasterCodeSheetSearch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.SelectByMasterCodeSheetSearch(MasterGroupId, Name, SearchCriteria, CurrentPage, PageSize, SortDirection, SortColumn, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => _sqlCommand.Parameters["@SearchCriteria"].Value.ShouldBe(SearchCriteria),
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(CurrentPage),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@SortDirection"].Value.ShouldBe(SortDirection),
                () => _sqlCommand.Parameters["@SortColumn"].Value.ShouldBe(SortColumn),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByMasterCodeSheetSearch),
                () => result.ShouldBe(_dataSet),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByCodeSheetID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.SelectByCodeSheetID(CodeSheetId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CodeSheetID"].Value.ShouldBe(CodeSheetId),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByCodeSheetId),
                () => result.ShouldBe(_dataSet),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateSort_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            MasterCodeSheet.UpdateSort(MastercodesheetXml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterCodeSheetXml"].Value.ShouldBe(MastercodesheetXml),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateSort),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ImportSubscriber_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var xDoc = new XDocument();

            // Act
            var result = MasterCodeSheet.ImportSubscriber(MasterId, xDoc, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MASTERGROUPID"].Value.ShouldBe(MasterId),
                () => _sqlCommand.Parameters["@IMPORTXML"].Value.ShouldBe(xDoc.ToString(SaveOptions.DisableFormatting)),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcImportSubscriber),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DeleteMasterID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterCodeSheet.DeleteMasterID(Client, MasterId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterID"].Value.ShouldBe(MasterId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteMasterId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void CreateNoValueRespones_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            MasterCodeSheet.CreateNoValueRespones(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandText.ShouldBe(ProcCreateNoValueRespones),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection
            {
                ConnectionStringGet = () => ConnectionString
            }.Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataSetSqlCommandString = (cmd, _) =>
            {
                _sqlCommand = cmd;
                return _dataSet;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}