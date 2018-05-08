using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public class DigitalEditionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (DigitalEdition) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalEdition_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DigitalEdition());
        }

        #endregion

        #region General Constructor : Class (DigitalEdition) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DigitalEdition_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDigitalEdition = new DigitalEdition();
            var secondDigitalEdition = new DigitalEdition();
            var thirdDigitalEdition = new DigitalEdition();
            var fourthDigitalEdition = new DigitalEdition();
            var fifthDigitalEdition = new DigitalEdition();
            var sixthDigitalEdition = new DigitalEdition();

            // Act, Assert
            firstDigitalEdition.ShouldNotBeNull();
            secondDigitalEdition.ShouldNotBeNull();
            thirdDigitalEdition.ShouldNotBeNull();
            fourthDigitalEdition.ShouldNotBeNull();
            fifthDigitalEdition.ShouldNotBeNull();
            sixthDigitalEdition.ShouldNotBeNull();
            firstDigitalEdition.ShouldNotBeSameAs(secondDigitalEdition);
            thirdDigitalEdition.ShouldNotBeSameAs(firstDigitalEdition);
            fourthDigitalEdition.ShouldNotBeSameAs(firstDigitalEdition);
            fifthDigitalEdition.ShouldNotBeSameAs(firstDigitalEdition);
            sixthDigitalEdition.ShouldNotBeSameAs(firstDigitalEdition);
            sixthDigitalEdition.ShouldNotBeSameAs(fourthDigitalEdition);
        }

        #endregion

        #endregion

        #endregion
    }
}