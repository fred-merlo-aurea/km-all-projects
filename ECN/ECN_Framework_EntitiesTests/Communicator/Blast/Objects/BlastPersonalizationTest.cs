using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastPersonalizationTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastPersonalization) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPersonalization_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastPersonalization());
        }

        #endregion

        #region General Constructor : Class (BlastPersonalization) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPersonalization_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastPersonalization = Fixture.CreateMany<BlastPersonalization>(2).ToList();
            var firstBlastPersonalization = instancesOfBlastPersonalization.FirstOrDefault();
            var lastBlastPersonalization = instancesOfBlastPersonalization.Last();

            // Act, Assert
            firstBlastPersonalization.ShouldNotBeNull();
            lastBlastPersonalization.ShouldNotBeNull();
            firstBlastPersonalization.ShouldNotBeSameAs(lastBlastPersonalization);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastPersonalization_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastPersonalization = new BlastPersonalization();
            var secondBlastPersonalization = new BlastPersonalization();
            var thirdBlastPersonalization = new BlastPersonalization();
            var fourthBlastPersonalization = new BlastPersonalization();
            var fifthBlastPersonalization = new BlastPersonalization();
            var sixthBlastPersonalization = new BlastPersonalization();

            // Act, Assert
            firstBlastPersonalization.ShouldNotBeNull();
            secondBlastPersonalization.ShouldNotBeNull();
            thirdBlastPersonalization.ShouldNotBeNull();
            fourthBlastPersonalization.ShouldNotBeNull();
            fifthBlastPersonalization.ShouldNotBeNull();
            sixthBlastPersonalization.ShouldNotBeNull();
            firstBlastPersonalization.ShouldNotBeSameAs(secondBlastPersonalization);
            thirdBlastPersonalization.ShouldNotBeSameAs(firstBlastPersonalization);
            fourthBlastPersonalization.ShouldNotBeSameAs(firstBlastPersonalization);
            fifthBlastPersonalization.ShouldNotBeSameAs(firstBlastPersonalization);
            sixthBlastPersonalization.ShouldNotBeSameAs(firstBlastPersonalization);
            sixthBlastPersonalization.ShouldNotBeSameAs(fourthBlastPersonalization);
        }

        #endregion

        #endregion

        #endregion
    }
}