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
    /// Unit Tests for <see cref="ResponseGroup"/>
    /// </summary>
    [TestFixture]
    public partial class ResponseGroupTest
    {
        private const int Rows = 5;
        private const string DisplayName = "display-name";
        private const int PubId = 2;
        private const int ResponseGroupId = 3;
        private const string ResponseGroupName = "response-group-name";
        private const string PubCode = "pub-code";
        private const string Name = "Name";
        private const string SearchCriteria = "Search-criteria";
        private const int CurrentPage = 8;
        private const int PageSize = 9;
        private const string SortDirection = "Sort-direction";
        private const string SortColumn = "Sort-column";
        private const string DestPubsXml = "dest-pubs-xml";

        private const string ProcExistsByName = "e_ResponseGroup_Exists_ByDisplayName";
        private const string ProcExistsByIdDisplayNamePubId = "e_ResponseGroup_Exists_ByIDDisplayNamePubID";
        private const string ProcExistsByIdResponseGroupNamePubId = "e_ResponseGroup_Exists_ByIDResponseGroupNamePubID";
        private const string ProcValidationForDeleteorInActive = "e_ResponseGroup_Validate_DeleteorInActive";
        private const string ProcSelect = "e_ResponseGroup_Select";
        private const string ProcSelectPubId = "e_ResponseGroup_Select_PubID";
        private const string ProcSelectPubCode = "e_ResponseGroup_Select_PubCode";
        private const string ProcSelectById = "e_ResponseGroup_Select_ID";
        private const string ProcSelectByResponseGroupsSearch = "e_ResponseGroup_Select_ResponseGroupsBySearch";
        private const string ProcCopy = "e_ResponseGroup_Copy";
        private const string ProcDelete = "e_ResponseGroup_Delete";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private DataSet _dataSet;
        private IList<Entity.ResponseGroup> _list;
        private Entity.ResponseGroup _objWithRandomValues;
        private Entity.ResponseGroup _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void ExistsByName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.ExistsByName(DisplayName, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@DisplayName"].Value.ShouldBe(DisplayName),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ExistsByIdDisplayNamePubID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.ExistsByIDDisplayNamePubID(PubId, ResponseGroupId, DisplayName, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _sqlCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => _sqlCommand.Parameters["@DisplayName"].Value.ShouldBe(DisplayName),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByIdDisplayNamePubId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ExistsByIDResponseGroupNamePubID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.ExistsByIDResponseGroupNamePubID(PubId, ResponseGroupId, ResponseGroupName, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _sqlCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => _sqlCommand.Parameters["@ResponseGroupName"].Value.ShouldBe(ResponseGroupName),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsByIdResponseGroupNamePubId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ValidationForDeleteorInActive_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            ResponseGroup.ValidationForDeleteorInActive(PubId, ResponseGroupId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _sqlCommand.CommandText.ShouldBe(ProcValidationForDeleteorInActive),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithPubId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.Select(PubId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPubId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithPubCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.Select(PubCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubCode"].Value.ShouldBe(PubCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPubCode),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.SelectByID(ResponseGroupId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectById),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectByResponseGroupsSearch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.SelectByResponseGroupsSearch(PubId, Name, SearchCriteria, CurrentPage, PageSize, SortDirection, SortColumn, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(PubId),
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => _sqlCommand.Parameters["@SearchCriteria"].Value.ShouldBe(SearchCriteria),
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(CurrentPage),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@SortDirection"].Value.ShouldBe(SortDirection),
                () => _sqlCommand.Parameters["@SortColumn"].Value.ShouldBe(SortColumn),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectByResponseGroupsSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => result.ShouldBe(_dataSet));
        }

        [Test]
        public void Copy_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.Copy(Client, ResponseGroupId, DestPubsXml);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@srcResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => _sqlCommand.Parameters["@destPubsXML"].Value.ShouldBe(DestPubsXml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcCopy),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Delete_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ResponseGroup.Delete(Client, ResponseGroupId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(ResponseGroupId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcDelete),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}