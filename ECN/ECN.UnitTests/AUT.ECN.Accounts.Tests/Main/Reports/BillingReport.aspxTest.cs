using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public class BillingReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BillingReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BillingReport());
        }

        #endregion

        #region General Constructor : Class (BillingReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBillingReport = new BillingReport();
            var secondBillingReport = new BillingReport();
            var thirdBillingReport = new BillingReport();
            var fourthBillingReport = new BillingReport();
            var fifthBillingReport = new BillingReport();
            var sixthBillingReport = new BillingReport();

            // Act, Assert
            firstBillingReport.ShouldNotBeNull();
            secondBillingReport.ShouldNotBeNull();
            thirdBillingReport.ShouldNotBeNull();
            fourthBillingReport.ShouldNotBeNull();
            fifthBillingReport.ShouldNotBeNull();
            sixthBillingReport.ShouldNotBeNull();
            firstBillingReport.ShouldNotBeSameAs(secondBillingReport);
            thirdBillingReport.ShouldNotBeSameAs(firstBillingReport);
            fourthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            fifthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            sixthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            sixthBillingReport.ShouldNotBeSameAs(fourthBillingReport);
        }

        #endregion

        #endregion

        #endregion
    }
}