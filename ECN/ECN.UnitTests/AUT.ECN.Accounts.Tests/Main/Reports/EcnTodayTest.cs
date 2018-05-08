using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public class EcnTodayTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (ECNToday) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ECNToday_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ECNToday());
        }

        #endregion

        #region General Constructor : Class (ECNToday) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ECNToday_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEcnToday = new ECNToday();
            var secondEcnToday = new ECNToday();
            var thirdEcnToday = new ECNToday();
            var fourthEcnToday = new ECNToday();
            var fifthEcnToday = new ECNToday();
            var sixthEcnToday = new ECNToday();

            // Act, Assert
            firstEcnToday.ShouldNotBeNull();
            secondEcnToday.ShouldNotBeNull();
            thirdEcnToday.ShouldNotBeNull();
            fourthEcnToday.ShouldNotBeNull();
            fifthEcnToday.ShouldNotBeNull();
            sixthEcnToday.ShouldNotBeNull();
            firstEcnToday.ShouldNotBeSameAs(secondEcnToday);
            thirdEcnToday.ShouldNotBeSameAs(firstEcnToday);
            fourthEcnToday.ShouldNotBeSameAs(firstEcnToday);
            fifthEcnToday.ShouldNotBeSameAs(firstEcnToday);
            sixthEcnToday.ShouldNotBeSameAs(firstEcnToday);
            sixthEcnToday.ShouldNotBeSameAs(fourthEcnToday);
        }

        #endregion

        #endregion

        #endregion
    }
}