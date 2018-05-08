using System;
using System.IO.Fakes;
using ecn.accounts.main.billingSystem;
using NUnit.Framework;
using Shouldly;
namespace ECN.Accounts.Tests.main.billingSystem
{
    public partial class BillingReportsTest
    {
        private string _result;

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void BtnRunReportClick_BlastColumns_Success(int blastColumn)
        {
            // Arrange
            InitilizeTest();
            _result = string.Empty;
            _lstbxBlastColumns.SelectedIndex = blastColumn;
            BillingReports.dtFlatRateItems.Rows.Clear();

            // Act
            _privateTestObject.Invoke("btnRunReport_Click", new object[] { null, null });

            // Assert
            _result.ShouldContain("BaseChannel\t");
            _result.ShouldContain("CustomerName\t");
            _result.ShouldContain("SendTime\t");
            _phError.Visible.ShouldBeFalse();
            _lblErrorMessage.Text.ShouldBeEmpty();
        }

        [Test]
        public void BtnRunReportClick_AllCustomers_Success()
        {
            // Arrange
            InitilizeTest();
            _result = string.Empty;
            _rblCustomer.SelectedIndex = 0;

            // Act
            _privateTestObject.Invoke("btnRunReport_Click", new object[] { null, null });

            // Assert
            _result.ShouldContain("BaseChannel\t");
            _result.ShouldContain("CustomerName\t");
            _result.ShouldContain("SendTime\t");
            _phError.Visible.ShouldBeFalse();
            _lblErrorMessage.Text.ShouldBeEmpty();
        }

        [Test]
        public void BtnRunReportClick_Exception()
        {
            // Arrange
            InitilizeTest();
            ShimDirectory.ExistsString = (path) => throw new Exception("Test Exception");

            // Act
            _privateTestObject.Invoke("btnRunReport_Click", new object[] { null, null });

            // Assert
            _phError.Visible.ShouldBeTrue();
            _lblErrorMessage.Text.ShouldBe("Test Exception");
        }
    }
}
