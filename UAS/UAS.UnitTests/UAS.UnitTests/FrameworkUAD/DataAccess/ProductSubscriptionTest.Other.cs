using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class ProductSubscriptionTest
    {
        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = ProductSubscription.Get(new SqlCommand());

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = ProductSubscription.GetList(new SqlCommand());

            // Assert
            result.IsListContentMatched(_list.ToList()).ShouldBeTrue();
        }

        [Test]
        public void GetIntList_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var expectedList = new List<int> {0, 0, 0};
            var intList = new List<int> {1, 2, 3};
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Act
            var result = typeof(ProductSubscription).CallMethod(MethodGetIntList, new object[] {new SqlCommand()});

            // Assert
            result.ShouldBe(expectedList);
        }

        [Test]
        public void UpdateRequesterFlags_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Update_Requester_Flags(Client, ProductId, IssueId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateRequesterFlags),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateQDate_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var qSourceDate = DateTime.Now;

            // Arrange, Act
            var result = ProductSubscription.UpdateQDate(SubscriptionId, qSourceDate, UpdatedByUserId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionId),
                () => _sqlCommand.Parameters["@QSourceDate"].Value.ShouldBe(qSourceDate),
                () => _sqlCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(UpdatedByUserId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateQDate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void UpdateSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.UpdateSubscription(PubSubscriptionId, IsLocked, UserId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(PubSubscriptionId),
                () => _sqlCommand.Parameters["@IsLocked"].Value.ShouldBe(IsLocked),
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Search_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.Search(Client,
                ClientDisplayName,
                FName,
                LName,
                Company,
                Title,
                Address1,
                City,
                RegionCode,
                Zip,
                Country,
                Email,
                Phone,
                SequenceId,
                Account,
                PublisherId,
                PublicationId,
                SubscriptionId);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@fName"].Value.ShouldBe(FName),
                () => _sqlCommand.Parameters["@lName"].Value.ShouldBe(LName),
                () => _sqlCommand.Parameters["@Company"].Value.ShouldBe(Company),
                () => _sqlCommand.Parameters["@Title"].Value.ShouldBe(Title),
                () => _sqlCommand.Parameters["@Add1"].Value.ShouldBe(Address1),
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
                () => _sqlCommand.Parameters["@ClientDisplayName"].Value.ShouldBe(ClientDisplayName),
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearch),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SearchSuggestMatch_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result =
                ProductSubscription.SearchSuggestMatch(Client, PublisherId, PublicationId, FirstName, LastName, Email);

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
        public void DeleteSubscription_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.DeleteSubscription(SubscriptionId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionID"].Value.ShouldBe(SubscriptionId),
                () => result.ShouldBe(Rows),
                () => _sqlCommand.CommandText.ShouldBe(ProcDeleteSubscription),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ClearWaveMailingInfo_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.ClearWaveMailingInfo(WaveMailingID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@WaveMailingID"].Value.ShouldBe(WaveMailingID),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcClearWaveMailingInfo),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ClearIMBSeq_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.ClearIMBSeq(ProductId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcClearImbSeq),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkWaveMailing_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SaveBulkWaveMailing(Xml, WaveMailingID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionXML"].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters["@WaveMailingID"].Value.ShouldBe(WaveMailingID),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkWaveMailing),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SearchAddressZip_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SearchAddressZip(Address1, ZipCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Address1"].Value.ShouldBe(Address1),
                () => _sqlCommand.Parameters["@ZipCode"].Value.ShouldBe(ZipCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSearchAddressZip),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SaveBulkActionIDUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.SaveBulkActionIDUpdate(Xml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriptionXML"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkActionIdUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void RecordUpdate_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.RecordUpdate(PubSubs, Changes, IssueId, ProductId, UserId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PubSubs"].Value.ShouldBe(PubSubs),
                () => _sqlCommand.Parameters["@Changes"].Value.ShouldBe(Changes),
                () => _sqlCommand.Parameters["@IssueID"].Value.ShouldBe(IssueId),
                () => _sqlCommand.Parameters["@ProductID"].Value.ShouldBe(ProductId),
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcRecordUpdate),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetListSimple_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var expectedList = new List<global::FrameworkUAD.Entity.ActionProductSubscription>
            {
                typeof(global::FrameworkUAD.Entity.ActionProductSubscription).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Act
            var result = ProductSubscription.GetListSimple(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetListSimpleCopies_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var expectedList = new List<global::FrameworkUAD.Entity.CopiesProductSubscription>
            {
                typeof(global::FrameworkUAD.Entity.CopiesProductSubscription).CreateInstance()
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return expectedList.GetSqlDataReader();
            };

            // Arrange
            var result = ProductSubscription.GetListSimpleCopies(new SqlCommand());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(expectedList.ToList()).ShouldBeTrue());
        }

        [Test]
        public void UpdateMasterDB_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteNonQuery = cmd =>
            {
                _sqlCommand = cmd;
                return 1;
            };

            // Act
            var result = ProductSubscription.UpdateMasterDB(Client, ProcessCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcUpdateMasterDb),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void DedupeMasterDB_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            ShimSqlCommand.AllInstances.ExecuteNonQuery = cmd =>
            {
                _sqlCommand = cmd;
                return 1;
            };

            // Act
            var result = ProductSubscription.DedupeMasterDB(Client, ProcessCode);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void CountryRegionCleanse_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = ProductSubscription.CountryRegionCleanse(SourceFileId, ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcCountryRegionCleanse),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}
