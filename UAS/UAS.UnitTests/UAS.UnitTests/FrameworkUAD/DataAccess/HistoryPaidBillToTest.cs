using System;
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

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="HistoryPaidBillTo"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryPaidBillToTest
    {
        private const string CommandText = "e_HistoryPaidBillTo_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.HistoryPaidBillTo _bill;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
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
            _bill = typeof(Entity.HistoryPaidBillTo).CreateInstance();

            // Act
            HistoryPaidBillTo.Save(_bill, new ClientConnections());

            // Assert
            _bill.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _bill = typeof(Entity.HistoryPaidBillTo).CreateInstance(true);

            // Act
            HistoryPaidBillTo.Save(_bill, new ClientConnections());

            // Assert
            _bill.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PaidBillToID"].Value.ShouldBe(_bill.PaidBillToID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_bill.PubSubscriptionID),
                () => _saveCommand.Parameters["@SubscriptionPaidID"].Value.ShouldBe(_bill.SubscriptionPaidID),
                () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe((object) _bill.FirstName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LastName"].Value.ShouldBe((object) _bill.LastName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object) _bill.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object) _bill.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressTypeID"].Value.ShouldBe(_bill.AddressTypeID),
                () => _saveCommand.Parameters["@Address1"].Value.ShouldBe((object) _bill.Address1 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address2"].Value.ShouldBe((object) _bill.Address2 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object) _bill.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object) _bill.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionCode"].Value.ShouldBe((object) _bill.RegionCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionID"].Value.ShouldBe(_bill.RegionID),
                () => _saveCommand.Parameters["@ZipCode"].Value.ShouldBe((object) _bill.ZipCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object) _bill.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@CarrierRoute"].Value.ShouldBe((object) _bill.CarrierRoute ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object) _bill.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object) _bill.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_bill.CountryID),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_bill.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_bill.Longitude),
                () => _saveCommand.Parameters["@IsAddressValidated"].Value.ShouldBe(_bill.IsAddressValidated),
                () => _saveCommand.Parameters["@AddressValidationDate"].Value.ShouldBe((object) _bill.AddressValidationDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressValidationSource"].Value.ShouldBe((object) _bill.AddressValidationSource ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressValidationMessage"].Value.ShouldBe((object) _bill.AddressValidationMessage ?? DBNull.Value),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object) _bill.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object) _bill.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe((object) _bill.Fax ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object) _bill.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object) _bill.Website ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_bill.DateCreated),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_bill.CreatedByUserID));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}
