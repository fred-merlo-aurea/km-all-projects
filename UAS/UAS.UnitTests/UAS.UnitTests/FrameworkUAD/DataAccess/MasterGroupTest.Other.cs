using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAD.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="MasterGroup"/>
    /// </summary>
    [TestFixture]
    public partial class MasterGroupTest
    {
        private const int Rows = 5;
        private const string DisplayName = "display-name";
        private const int MasterGroupId = 2;
        private const string Name = "name";
        private const int BrandId = 4;
        private const string SearchCriteria = "Search-criteria";
        private const int CurrentPage = 6;
        private const int PageSize = 7;
        private const string SortDirection = "Sort-direction";
        private const string SortColumn = "Sort-column";
        private const string MastergroupXml = "mastergroup-xml";
        private const string ProcExistsByName = "SELECT * FROM mastergroups with (nolock) where Displayname = @DisplayName";
        private const string ProcExistsByIdDisplayName = "SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Displayname = @DisplayName";
        private const string ProcExistsByIdName = "SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Name = @Name";
        private const string ProcValidationForDeleteorInActive = "e_MasterGroup_Validate_DeleteorInActive";
        private const string ProcSelect = "e_MasterGroup_Select";
        private const string ProcSelectByBrandId = "e_MasterGroup_Select_ByBrandID";
        private const string ProcSelectById = "e_MasterGroup_Select_ID";
        private const string ProcSelectByMasterGroupsSearch = "e_MasterGroup_Select_MasterGroupsBySearch";
        private const string ProcUpdateSort = "e_MasterGroup_Update_SortOrder";
        private const string ProcDelete = "e_MasterGroup_Delete";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private DataSet _dataSet;
        private IList<Entity.MasterGroup> _list;
        private Entity.MasterGroup _objWithRandomValues;
        private Entity.MasterGroup _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void ExistsByName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.ExistsByName(DisplayName, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@DisplayName"].Value.ShouldBe(DisplayName),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void ExistsByIDDisplayName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.ExistsByIDDisplayName(MasterGroupId, DisplayName, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => _sqlCommand.Parameters["@DisplayName"].Value.ShouldBe(DisplayName),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByIdDisplayName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void ExistsByIDName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.ExistsByIDName(MasterGroupId, Name, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByIdName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void ValidationForDeleteorInActive_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            MasterGroup.ValidationForDeleteorInActive(MasterGroupId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => _sqlCommand.CommandText.ShouldBe(ProcValidationForDeleteorInActive),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByBrandID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.SelectByBrandID(BrandId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@BrandID"].Value.ShouldBe(BrandId),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByBrandId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.SelectByID(MasterGroupId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectById),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByMasterGroupsSearch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.SelectByMasterGroupsSearch(Name, SearchCriteria, CurrentPage, PageSize, SortDirection, SortColumn, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => _sqlCommand.Parameters["@SearchCriteria"].Value.ShouldBe(SearchCriteria),
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(CurrentPage),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@SortDirection"].Value.ShouldBe(SortDirection),
                () => _sqlCommand.Parameters["@SortColumn"].Value.ShouldBe(SortColumn),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByMasterGroupsSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => result.ShouldBe(_dataSet));
        }

        [Test]
        public void UpdateSort_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            MasterGroup.UpdateSort(MastergroupXml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupXml"].Value.ShouldBe(MastergroupXml),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateSort),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Delete_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = MasterGroup.Delete(Client, MasterGroupId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@MasterGroupID"].Value.ShouldBe(MasterGroupId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDelete),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = MasterGroup.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }
    }
}