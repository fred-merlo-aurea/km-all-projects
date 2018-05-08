using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public  class TotalBlastsForDayTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (TotalBlastsForDay) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_TotalBlastsForDay_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new TotalBlastsForDay());
        }

        #endregion

        #region General Constructor : Class (TotalBlastsForDay) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_TotalBlastsForDay_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstTotalBlastsForDay = new TotalBlastsForDay();
            var secondTotalBlastsForDay = new TotalBlastsForDay();
            var thirdTotalBlastsForDay = new TotalBlastsForDay();
            var fourthTotalBlastsForDay = new TotalBlastsForDay();
            var fifthTotalBlastsForDay = new TotalBlastsForDay();
            var sixthTotalBlastsForDay = new TotalBlastsForDay();

            // Act, Assert
            firstTotalBlastsForDay.ShouldNotBeNull();
            secondTotalBlastsForDay.ShouldNotBeNull();
            thirdTotalBlastsForDay.ShouldNotBeNull();
            fourthTotalBlastsForDay.ShouldNotBeNull();
            fifthTotalBlastsForDay.ShouldNotBeNull();
            sixthTotalBlastsForDay.ShouldNotBeNull();
            firstTotalBlastsForDay.ShouldNotBeSameAs(secondTotalBlastsForDay);
            thirdTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            fourthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            fifthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            sixthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            sixthTotalBlastsForDay.ShouldNotBeSameAs(fourthTotalBlastsForDay);
        }

        #endregion

        #endregion

        #endregion
    }
}