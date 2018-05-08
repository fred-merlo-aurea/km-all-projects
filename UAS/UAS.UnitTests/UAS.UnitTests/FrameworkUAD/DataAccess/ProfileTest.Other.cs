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
    /// Unit Tests for <see cref="Profile"/>
    /// </summary>
    [TestFixture]
    public partial class ProfileTest
    {
        private const int Rows = 5;
        private const string SearchValue = "search-value";
        private const string SearchFields = "search-fields";
        private const string OrderBy = "order-by";
        private const int ProfileId = 5;
        private const int PublisherId = 6;
        private const int PublicationId = 7;
        private const bool IsSubscribed = true;
        private const bool IsProspect = true;
        private const string ProcSearch = "e_Profile_Search";
        private const string ProcSelect = "e_Profile_Select_ProfileID";
        private const string ProcSelectPublisher = "e_Subscriber_Select_PublisherID";
        private const string ProcSelectProspect = "e_Subscriber_Select_Prospect_PublicationID";
        private const string ProcSelectPublicationSubscribed = "e_Subscriber_Select_PublicationID_IsSubscribed";
        private const string ProcSelectPublicationProspect = "e_Subscriber_Select_PublicationID_IsProspect";
        private const string ProcSelectPublicationPublicationId = "e_Subscriber_Select_PublicationID_IsSubscribed_IsProspect";
        private const string ProcSelectPublication = "e_Subscriber_SelectPublication";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.Profile> _list;
        private Entity.Profile _objWithRandomValues;
        private Entity.Profile _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void Search_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Profile.Search(SearchValue, new List<Entity.Profile>());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void Search_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.Search(SearchValue, SearchFields, OrderBy, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Search"].Value.ShouldBe(SearchValue),
                () => _sqlCommand.Parameters["@SearchFields"].Value.ShouldBe(SearchFields),
                () => _sqlCommand.Parameters["@OrderBy"].Value.ShouldBe(OrderBy),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.Select(ProfileId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProfileID"].Value.ShouldBe(ProfileId),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublisher_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectPublisher(PublisherId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublisherID"].Value.ShouldBe(PublisherId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublisher),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectProspect_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectProspect(PublicationId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectProspect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublicationSubscribed_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectPublicationSubscribed(PublicationId, true, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => _sqlCommand.Parameters["@IsSubscribed"].Value.ShouldBe(IsSubscribed),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublicationSubscribed),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublicationProspect_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectPublicationProspect(PublicationId, true, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => _sqlCommand.Parameters["@IsProspect"].Value.ShouldBe(IsProspect),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublicationProspect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublication_WhenCalledWithPublicationId_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectPublication(PublicationId, true, true, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => _sqlCommand.Parameters["@IsSubscribed"].Value.ShouldBe(IsSubscribed),
                () => _sqlCommand.Parameters["@IsProspect"].Value.ShouldBe(IsProspect),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublicationPublicationId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublication_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Profile.SelectPublication(PublicationId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublication),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}