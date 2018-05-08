using ECN_Framework_Entities.Accounts;
using NUnit.Framework;
using Shouldly;

namespace ECN.Accounts.Tests.main.billingSystem
{
    public partial class BillingReportsTest
    {
        [Test]
        public void BtnSaveReportClick_WithReportId()
        {
            // Arrange
            InitilizeTest();
            _testType.SetStaticField("currentReport", new BillingReport { BillingReportID = 1 });

            // Act
            _privateTestObject.Invoke("btnSaveReport_Click", new object[] { null, null });

            // Assert
            _listBillingReport.ShouldNotBeNull();
            _listBillingReport.Count.ShouldBe(1);
            _listBillingReportItem.ShouldNotBeNull();
            _listBillingReportItem.Count.ShouldBe(3);
        }

        [Test]
        public void BtnSaveReportClick_WtihReportId_AllCustomers()
        {
            // Arrange
            InitilizeTest();
            _testType.SetStaticField("currentReport", new BillingReport { BillingReportID = 1 });
            _rblCustomer.SelectedIndex = 0;
            _txtRunToDate.Text = string.Empty;
            _txtRunFromDate.Text = string.Empty;

            // Act
            _privateTestObject.Invoke("btnSaveReport_Click", new object[] { null, null });

            // Assert
            _listBillingReport.ShouldNotBeNull();
            _listBillingReport.Count.ShouldBe(1);
            _listBillingReportItem.ShouldNotBeNull();
            _listBillingReportItem.Count.ShouldBe(3);
        }

        [Test]
        public void BtnSaveReportClick_NoReportId()
        {
            // Arrange
            InitilizeTest();
            
            // Act
            _privateTestObject.Invoke("btnSaveReport_Click", new object[] { null, null });

            // Assert
            _listBillingReport.ShouldNotBeNull();
            _listBillingReport.Count.ShouldBe(1);
            _listBillingReportItem.ShouldNotBeNull();
            _listBillingReportItem.Count.ShouldBe(3);
        }

        [Test]
        public void BtnSaveReportClick_NoReportId_AllCustomers()
        {
            // Arrange
            InitilizeTest();
            _rblCustomer.SelectedIndex = 0;
            _txtRunToDate.Text = string.Empty;
            _txtRunFromDate.Text = string.Empty;

            // Act
            _privateTestObject.Invoke("btnSaveReport_Click", new object[] { null, null });

            // Assert
            _listBillingReport.ShouldNotBeNull();
            _listBillingReport.Count.ShouldBe(1);
            _listBillingReportItem.ShouldNotBeNull();
            _listBillingReportItem.Count.ShouldBe(3);
        }
    }
}
