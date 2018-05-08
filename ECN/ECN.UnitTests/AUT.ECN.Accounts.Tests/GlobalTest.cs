using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests
{
    [TestFixture]
    public class GlobalTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (Global) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Global_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Global());
        }

        #endregion

        #region General Constructor : Class (Global) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Global_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGlobal = new Global();
            var secondGlobal = new Global();
            var thirdGlobal = new Global();
            var fourthGlobal = new Global();
            var fifthGlobal = new Global();
            var sixthGlobal = new Global();

            // Act, Assert
            firstGlobal.ShouldNotBeNull();
            secondGlobal.ShouldNotBeNull();
            thirdGlobal.ShouldNotBeNull();
            fourthGlobal.ShouldNotBeNull();
            fifthGlobal.ShouldNotBeNull();
            sixthGlobal.ShouldNotBeNull();
            firstGlobal.ShouldNotBeSameAs(secondGlobal);
            thirdGlobal.ShouldNotBeSameAs(firstGlobal);
            fourthGlobal.ShouldNotBeSameAs(firstGlobal);
            fifthGlobal.ShouldNotBeSameAs(firstGlobal);
            sixthGlobal.ShouldNotBeSameAs(firstGlobal);
            sixthGlobal.ShouldNotBeSameAs(fourthGlobal);
        }

        #endregion

        #endregion

        #endregion
    }
}