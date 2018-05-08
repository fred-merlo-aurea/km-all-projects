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
    /// Unit tests for <see cref="HistoryPaid"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryPaidTest
    {
        private const string CommandText = "e_HistoryPaid_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.HistoryPaid _historyPaid;

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
            _historyPaid = typeof(Entity.HistoryPaid).CreateInstance();

            // Act
            HistoryPaid.Save(_historyPaid, new ClientConnections());

            // Assert
            _historyPaid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _historyPaid = typeof(Entity.HistoryPaid).CreateInstance(true);

            // Act
            HistoryPaid.Save(_historyPaid, new ClientConnections());

            // Assert
            _historyPaid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriptionPaidID"].Value.ShouldBe(_historyPaid.SubscriptionPaidID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_historyPaid.PubSubscriptionID),
                () => _saveCommand.Parameters["@PriceCodeID"].Value.ShouldBe(_historyPaid.PriceCodeID),
                () => _saveCommand.Parameters["@StartIssueDate"].Value.ShouldBe(_historyPaid.StartIssueDate),
                () => _saveCommand.Parameters["@ExpireIssueDate"].Value.ShouldBe(_historyPaid.ExpireIssueDate),
                () => _saveCommand.Parameters["@CPRate"].Value.ShouldBe(_historyPaid.CPRate),
                () => _saveCommand.Parameters["@Amount"].Value.ShouldBe(_historyPaid.Amount),
                () => _saveCommand.Parameters["@AmountPaid"].Value.ShouldBe(_historyPaid.AmountPaid),
                () => _saveCommand.Parameters["@BalanceDue"].Value.ShouldBe(_historyPaid.BalanceDue),
                () => _saveCommand.Parameters["@PaidDate"].Value.ShouldBe(_historyPaid.PaidDate),
                () => _saveCommand.Parameters["@TotalIssues"].Value.ShouldBe(_historyPaid.TotalIssues),
                () => _saveCommand.Parameters["@CheckNumber"].Value.ShouldBe(_historyPaid.CheckNumber),
                () => _saveCommand.Parameters["@CCNumber"].Value.ShouldBe(_historyPaid.CCNumber),
                () => _saveCommand.Parameters["@CCExpirationMonth"].Value.ShouldBe(_historyPaid.CCExpirationMonth),
                () => _saveCommand.Parameters["@CCExpirationYear"].Value.ShouldBe(_historyPaid.CCExpirationYear),
                () => _saveCommand.Parameters["@CCHolderName"].Value.ShouldBe(_historyPaid.CCHolderName),
                () => _saveCommand.Parameters["@CreditCardTypeID"].Value.ShouldBe(_historyPaid.CreditCardTypeID),
                () => _saveCommand.Parameters["@PaymentTypeID"].Value.ShouldBe(_historyPaid.PaymentTypeID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_historyPaid.DateCreated),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_historyPaid.CreatedByUserID));
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