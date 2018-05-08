using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public class EmailDirectReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (EmailDirectReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDirectReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailDirectReport());
        }

        #endregion

        #region General Constructor : Class (EmailDirectReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDirectReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailDirectReport = new EmailDirectReport();
            var secondEmailDirectReport = new EmailDirectReport();
            var thirdEmailDirectReport = new EmailDirectReport();
            var fourthEmailDirectReport = new EmailDirectReport();
            var fifthEmailDirectReport = new EmailDirectReport();
            var sixthEmailDirectReport = new EmailDirectReport();

            // Act, Assert
            firstEmailDirectReport.ShouldNotBeNull();
            secondEmailDirectReport.ShouldNotBeNull();
            thirdEmailDirectReport.ShouldNotBeNull();
            fourthEmailDirectReport.ShouldNotBeNull();
            fifthEmailDirectReport.ShouldNotBeNull();
            sixthEmailDirectReport.ShouldNotBeNull();
            firstEmailDirectReport.ShouldNotBeSameAs(secondEmailDirectReport);
            thirdEmailDirectReport.ShouldNotBeSameAs(firstEmailDirectReport);
            fourthEmailDirectReport.ShouldNotBeSameAs(firstEmailDirectReport);
            fifthEmailDirectReport.ShouldNotBeSameAs(firstEmailDirectReport);
            sixthEmailDirectReport.ShouldNotBeSameAs(firstEmailDirectReport);
            sixthEmailDirectReport.ShouldNotBeSameAs(fourthEmailDirectReport);
        }

        #endregion

        #endregion

        #endregion
    }
}