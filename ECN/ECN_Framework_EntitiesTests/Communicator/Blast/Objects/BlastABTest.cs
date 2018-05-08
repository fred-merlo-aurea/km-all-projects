using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastABTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastAB) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastAB_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastAB());
        }

        #endregion

        #region General Constructor : Class (BlastAB) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastAB_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastAB = Fixture.CreateMany<BlastAB>(2).ToList();
            var firstBlastAB = instancesOfBlastAB.FirstOrDefault();
            var lastBlastAB = instancesOfBlastAB.Last();

            // Act, Assert
            firstBlastAB.ShouldNotBeNull();
            lastBlastAB.ShouldNotBeNull();
            firstBlastAB.ShouldNotBeSameAs(lastBlastAB);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastAB_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastAB = new BlastAB();
            var secondBlastAB = new BlastAB();
            var thirdBlastAB = new BlastAB();
            var fourthBlastAB = new BlastAB();
            var fifthBlastAB = new BlastAB();
            var sixthBlastAB = new BlastAB();

            // Act, Assert
            firstBlastAB.ShouldNotBeNull();
            secondBlastAB.ShouldNotBeNull();
            thirdBlastAB.ShouldNotBeNull();
            fourthBlastAB.ShouldNotBeNull();
            fifthBlastAB.ShouldNotBeNull();
            sixthBlastAB.ShouldNotBeNull();
            firstBlastAB.ShouldNotBeSameAs(secondBlastAB);
            thirdBlastAB.ShouldNotBeSameAs(firstBlastAB);
            fourthBlastAB.ShouldNotBeSameAs(firstBlastAB);
            fifthBlastAB.ShouldNotBeSameAs(firstBlastAB);
            sixthBlastAB.ShouldNotBeSameAs(firstBlastAB);
            sixthBlastAB.ShouldNotBeSameAs(fourthBlastAB);
        }

        #endregion

        #endregion

        #endregion
    }
}