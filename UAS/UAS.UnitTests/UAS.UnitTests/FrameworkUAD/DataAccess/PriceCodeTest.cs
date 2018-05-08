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
    /// Unit tests for <see cref="PriceCode"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PriceCodeTest
    {
        private const string CommandText = "e_PriceCode_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.PriceCode _price;

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
            _price = typeof(Entity.PriceCode).CreateInstance();

            // Act
            PriceCode.Save(_price, new ClientConnections());

            // Assert
            _price.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _price = typeof(Entity.PriceCode).CreateInstance(true);

            // Act
            PriceCode.Save(_price, new ClientConnections());

            // Assert
            _price.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PriceCodeID"].Value.ShouldBe(_price.PriceCodeID),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_price.PublicationID),
                () => _saveCommand.Parameters["@PriceCode"].Value.ShouldBe(_price.PriceCodes),
                () => _saveCommand.Parameters["@Term"].Value.ShouldBe(_price.Term),
                () => _saveCommand.Parameters["@USCopyRate"].Value.ShouldBe(_price.US_CopyRate),
                () => _saveCommand.Parameters["@CANCopyRate"].Value.ShouldBe(_price.CAN_CopyRate),
                () => _saveCommand.Parameters["@FORCopyRate"].Value.ShouldBe(_price.FOR_CopyRate),
                () => _saveCommand.Parameters["@USPrice"].Value.ShouldBe(_price.US_Price),
                () => _saveCommand.Parameters["@CANPrice"].Value.ShouldBe(_price.CAN_Price),
                () => _saveCommand.Parameters["@FORPrice"].Value.ShouldBe(_price.FOR_Price),
                () => _saveCommand.Parameters["@QFOfferCode"].Value.ShouldBe(_price.QFOfferCode),
                () => _saveCommand.Parameters["@FoxProPriceCode"].Value.ShouldBe(_price.FoxProPriceCode),
                () => _saveCommand.Parameters["@Description"].Value.ShouldBe(_price.Description),
                () => _saveCommand.Parameters["@DeliverabilityID"].Value.ShouldBe(_price.DeliverabilityID),
                () => _saveCommand.Parameters["@TotalIssues"].Value.ShouldBe(_price.TotalIssues),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_price.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_price.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_price.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_price.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_price.UpdatedByUserID ?? DBNull.Value));
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
