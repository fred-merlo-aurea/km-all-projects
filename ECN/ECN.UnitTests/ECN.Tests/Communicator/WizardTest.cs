using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using ecn.communicator.classes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public class WizardTest
    {
        [Test]
        public void Save_WithDefaultValues_SavesEmptyString()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                var parameters = new[]
                {
                    "@CardHolderName",
                    "@CardNumber",
                    "@CardcvNumber",
                    "@CardExpiration",
                    "@TransactionNo"
                };
                ShimSqlConnection.ConstructorString = (connection, connectionString) => { };
                ShimSqlConnection.AllInstances.Open = connection => { };
                ShimSqlConnection.AllInstances.Close = connection => { };
                SqlCommand sqlCommand = null;
                ShimSqlCommand.AllInstances.ExecuteScalar = command =>
                {
                    sqlCommand = command;
                    return 1;
                };
                var wizard = new Wizard();

                // Act
                var result = wizard.Save();

                // Assert
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldBe(1),
                    () => parameters.ShouldAllBe(p => sqlCommand.Parameters[p].Value.ToString() == string.Empty));
            }
        }
    }
}
