using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="SubscriberArchive"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberArchiveTest
    {
        private const string CommandText = "e_SubscriberArchive_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberArchive _archive;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.SubscriberArchive).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriberArchive).CreateInstance();

            _list = new List<Entity.SubscriberArchive>
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
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _archive = typeof(Entity.SubscriberArchive).CreateInstance();

            // Act
            SubscriberArchive.Save(_archive, new ClientConnections());

            // Assert
            _archive.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _archive = typeof(Entity.SubscriberArchive).CreateInstance(true);

            // Act
            SubscriberArchive.Save(_archive, new ClientConnections());

            // Assert
            _archive.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriberArchiveID"].Value.ShouldBe(_archive.SubscriberArchiveID),
                () => _saveCommand.Parameters["@SourceFileID"].Value.ShouldBe(_archive.SourceFileID),
                () => _saveCommand.Parameters["@PubCode"].Value.ShouldBe((object)_archive.PubCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sequence"].Value.ShouldBe(_archive.Sequence),
                () => _saveCommand.Parameters["@FName"].Value.ShouldBe((object)_archive.FName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LName"].Value.ShouldBe((object)_archive.LName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_archive.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_archive.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address"].Value.ShouldBe((object)_archive.Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailStop"].Value.ShouldBe((object)_archive.MailStop ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_archive.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@State"].Value.ShouldBe((object)_archive.State ?? DBNull.Value),
                () => _saveCommand.Parameters["@Zip"].Value.ShouldBe((object)_archive.Zip ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_archive.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@ForZip"].Value.ShouldBe((object)_archive.ForZip ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_archive.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_archive.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_archive.CountryID),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_archive.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhoneExists"].Value.ShouldBe((object)_archive.PhoneExists ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe(_archive.Fax),
                () => _saveCommand.Parameters["@FaxExists"].Value.ShouldBe((object)_archive.FaxExists ?? DBNull.Value),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_archive.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailExists"].Value.ShouldBe((object)_archive.EmailExists ?? DBNull.Value),
                () => _saveCommand.Parameters["@CategoryID"].Value.ShouldBe(_archive.CategoryID),
                () => _saveCommand.Parameters["@TransactionID"].Value.ShouldBe(_archive.TransactionID),
                () => _saveCommand.Parameters["@TransactionDate"].Value.ShouldBe((object)_archive.TransactionDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QDate"].Value.ShouldBe((object)_archive.QDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QSourceID"].Value.ShouldBe(_archive.QSourceID),
                () => _saveCommand.Parameters["@RegCode"].Value.ShouldBe((object)_archive.RegCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Verified"].Value.ShouldBe((object)_archive.Verified ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubSrc"].Value.ShouldBe((object)_archive.SubSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@OrigsSrc"].Value.ShouldBe((object)_archive.OrigsSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@Par3C"].Value.ShouldBe((object)_archive.Par3C ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailPermission"].Value.ShouldBe((object)_archive.MailPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@FaxPermission"].Value.ShouldBe((object)_archive.FaxPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhonePermission"].Value.ShouldBe((object)_archive.PhonePermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@OtherProductsPermission"].Value.ShouldBe((object)_archive.OtherProductsPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@ThirdPartyPermission"].Value.ShouldBe((object)_archive.ThirdPartyPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailRenewPermission"].Value.ShouldBe((object)_archive.EmailRenewPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@TextPermission"].Value.ShouldBe((object)_archive.TextPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@Source"].Value.ShouldBe((object)_archive.Source ?? DBNull.Value),
                () => _saveCommand.Parameters["@Priority"].Value.ShouldBe((object)_archive.Priority ?? DBNull.Value),
                () => _saveCommand.Parameters["@IGrp_No"].Value.ShouldBe(_archive.IGrp_No),
                () => _saveCommand.Parameters["@IGrp_Cnt"].Value.ShouldBe(_archive.IGrp_Cnt),
                () => _saveCommand.Parameters["@CGrp_No"].Value.ShouldBe(_archive.CGrp_No),
                () => _saveCommand.Parameters["@CGrp_Cnt"].Value.ShouldBe(_archive.CGrp_Cnt),
                () => _saveCommand.Parameters["@StatList"].Value.ShouldBe((object)_archive.StatList ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sic"].Value.ShouldBe((object)_archive.Sic ?? DBNull.Value),
                () => _saveCommand.Parameters["@SicCode"].Value.ShouldBe((object)_archive.SicCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Gender"].Value.ShouldBe((object)_archive.Gender ?? DBNull.Value),
                () => _saveCommand.Parameters["@IGrp_Rank"].Value.ShouldBe((object)_archive.IGrp_Rank ?? DBNull.Value),
                () => _saveCommand.Parameters["@CGrp_Rank"].Value.ShouldBe((object)_archive.CGrp_Rank ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_archive.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Home_Work_Address"].Value.ShouldBe((object)_archive.Home_Work_Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@Demo7"].Value.ShouldBe((object)_archive.Demo7 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_archive.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_archive.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_archive.Longitude),
                () => _saveCommand.Parameters["@EmailStatusID"].Value.ShouldBe((object)_archive.EmailStatusID ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsMailable"].Value.ShouldBe((object)_archive.IsMailable ?? DBNull.Value),
                () => _saveCommand.Parameters["@SARecordIdentifier"].Value.ShouldBe(_archive.SARecordIdentifier),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_archive.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_archive.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_archive.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_archive.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ImportRowNumber"].Value.ShouldBe(_archive.ImportRowNumber),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe((object)_archive.ProcessCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_archive.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@ExternalKeyId"].Value.ShouldBe(_archive.ExternalKeyId),
                () => _saveCommand.Parameters["@AccountNumber"].Value.ShouldBe((object)_archive.AccountNumber ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailID"].Value.ShouldBe(_archive.EmailID),
                () => _saveCommand.Parameters["@Copies"].Value.ShouldBe(_archive.Copies),
                () => _saveCommand.Parameters["@GraceIssues"].Value.ShouldBe(_archive.GraceIssues),
                () => _saveCommand.Parameters["@IsComp"].Value.ShouldBe((object)_archive.IsComp ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsPaid"].Value.ShouldBe((object)_archive.IsPaid ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsSubscribed"].Value.ShouldBe((object)_archive.IsSubscribed ?? DBNull.Value),
                () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe((object)_archive.Occupation ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_archive.SubscriptionStatusID),
                () => _saveCommand.Parameters["@SubsrcID"].Value.ShouldBe(_archive.SubsrcID),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object)_archive.Website ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                _sqlCommand = cmd;
                return -1;
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
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}
