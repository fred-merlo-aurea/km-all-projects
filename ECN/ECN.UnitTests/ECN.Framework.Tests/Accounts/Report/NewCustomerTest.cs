using ECN_Framework.Accounts.Report;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.Tests.Accounts.Report
{
    [TestFixture]
    public class NewCustomerTest : ReportBaseTest
    {
        [Test]
        public void Get_ValidData_ReturnsList()
        {
            // Arrange
            _counter = 0;
            MaxRowCount = 3;

            // Act
            var customerResults = NewCustomer.Get(1, 1, true);

            // Assert
            customerResults.Count.ShouldBe(MaxRowCount);
        }
    }
}
