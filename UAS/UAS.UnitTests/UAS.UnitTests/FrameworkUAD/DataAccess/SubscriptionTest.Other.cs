using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using EntityObject = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="Subscription"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriptionTests
    {
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.Subscription> _list;
        private Entity.Subscription _objWithRandomValues;
        private Entity.Subscription _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.Subscription)
                .CreateInstance();
            _objWithDefaultValues = typeof(Entity.Subscription)
                .CreateInstance(true);

            _list = new List<Entity.Subscription>
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
        public void ExistsStandardFieldName_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.ExistsStandardFieldName(Name, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Name"].Value.ShouldBe(Name),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcExistsStandardFieldName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void SelectSubscriptionID_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.Select(SubscriptionID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionID),
                () => result.ShouldBe(_objWithDefaultValues),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriptionId),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectEmail_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.Select(Email, Client, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(Email),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectEmail),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectInValidAddresses_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SelectInValidAddresses(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectInValidAddresses),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectIDs_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var intList = new List<int> { 1, 2, 3 };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Arrange, Act
            var result = Subscription.SelectIDs(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(new List<int> { 0, 0, 0 }),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectIDs),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForFileAudit_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = Subscription.SelectForFileAudit(ProcessCode, SourceFileId, startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => _sqlCommand.Parameters["@StartDate"].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters["@EndDate"].Value.ShouldBe(endDate),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForFileAudit),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void FindMatches_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.FindMatches(ProductId, Fname, Lname, Company, Address, State, Zip, Phone, Email, Title, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@Firstname"].Value.ShouldBe(Fname),
                () => _sqlCommand.Parameters["@Lastname"].Value.ShouldBe(Lname),
                () => _sqlCommand.Parameters["@Company"].Value.ShouldBe(Company),
                () => _sqlCommand.Parameters["@Address"].Value.ShouldBe(Address),
                () => _sqlCommand.Parameters["@State"].Value.ShouldBe(State),
                () => _sqlCommand.Parameters["@zip"].Value.ShouldBe(Zip),
                () => _sqlCommand.Parameters["@Phone"].Value.ShouldBe(Phone),
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(Email),
                () => _sqlCommand.Parameters["@Title"].Value.ShouldBe(ProductId),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcFindMatches),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void AddressUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.AddressUpdate(Xml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcAddressUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void NcoaUpdateAddress_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.NcoaUpdateAddress(Xml, Client, UserId, SourceFileId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters["@userId"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcNcoaUpdateAddress),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateQDate_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var qSourceDate = DateTime.Now;

            // Act
            var result = Subscription.UpdateQDate(SubscriptionID, qSourceDate, UpdatedByUserId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionID),
                () => _sqlCommand.Parameters["@QSourceDate"].Value.ShouldBe(qSourceDate),
                () => _sqlCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(UpdatedByUserId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateQDate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Search_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.Search(
                Client,
                ClientDisplayName,
                FirstName,
                LastName,
                Company,
                Title,
                Add1,
                City,
                RegionCode,
                Zip,
                Country,
                Email,
                Phone,
                SequenceId,
                Account,
                PublisherId,
                PublicationId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@fName"].Value.ShouldBe(FirstName),
                () => _sqlCommand.Parameters["@lName"].Value.ShouldBe(LastName),
                () => _sqlCommand.Parameters["@Company"].Value.ShouldBe(Company),
                () => _sqlCommand.Parameters["@Title"].Value.ShouldBe(Title),
                () => _sqlCommand.Parameters["@Add1"].Value.ShouldBe(Add1),
                () => _sqlCommand.Parameters["@City"].Value.ShouldBe(City),
                () => _sqlCommand.Parameters["@RegionCode"].Value.ShouldBe(RegionCode),
                () => _sqlCommand.Parameters["@Zip"].Value.ShouldBe(Zip),
                () => _sqlCommand.Parameters["@Country"].Value.ShouldBe(Country),
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(Email),
                () => _sqlCommand.Parameters["@Phone"].Value.ShouldBe(Phone),
                () => _sqlCommand.Parameters["@SequenceID"].Value.ShouldBe(SequenceId),
                () => _sqlCommand.Parameters["@AccountID"].Value.ShouldBe(Account),
                () => _sqlCommand.Parameters["@PublisherID"].Value.ShouldBe(PublisherId),
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => _sqlCommand.Parameters["@clientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SearchSuggestMatch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SearchSuggestMatch(Client, PublisherId, PublicationId, FirstName, LastName, Email);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublisherID"].Value.ShouldBe(PublisherId),
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => _sqlCommand.Parameters["@FirstName"].Value.ShouldBe(FirstName),
                () => _sqlCommand.Parameters["@LastName"].Value.ShouldBe(LastName),
                () => _sqlCommand.Parameters["@Email"].Value.ShouldBe(Email),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearchSuggestMatch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPaging_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SelectPaging(Page, PageSize, ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CurrentPage"].Value.ShouldBe(Page),
                () => _sqlCommand.Parameters["@PageSize"].Value.ShouldBe(PageSize),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPaging),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.UpdateSubscription(SubscriptionID, true, UserId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionID),
                () => _sqlCommand.Parameters["@IsLocked"].Value.ShouldBe(IsLocked),
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DeleteSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.DeleteSubscription(SubscriptionID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionID),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublication_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SelectPublication(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(ProductId),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublication),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ClearWaveMailingInfo_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.ClearWaveMailingInfo(WaveMailingID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@WaveMailingID"].Value.ShouldBe(WaveMailingID),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcClearWaveMailingInfo),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkWaveMailing_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SaveBulkWaveMailing(Xml, WaveMailingID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionXML"].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters["@WaveMailingID"].Value.ShouldBe(WaveMailingID),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkWaveMailing),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSequence_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SelectSequence(SequenceId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SequenceID"].Value.ShouldBe(SequenceId),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSequence),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SelectCount(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SearchAddressZip_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SearchAddressZip(Address1, ZipCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Address1"].Value.ShouldBe(Address1),
                () => _sqlCommand.Parameters["@ZipCode"].Value.ShouldBe(ZipCode),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearchAddressZip),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkActionIDUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Subscription.SaveBulkActionIDUpdate(Xml);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionXML"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkActionIDUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Subscription.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Subscription.GetList(new SqlCommand(), true);

            // Assert
            result.IsListContentMatched(_list.ToList()).ShouldBeTrue();
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
            var subscriptions = new List<EntityObject.Subscription> { typeof(EntityObject.Subscription).CreateInstance() };
            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
           {
               _sqlCommand = cmd;
               return subscriptions.GetSqlDataReader();
           };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}