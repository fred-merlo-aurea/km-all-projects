using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastSocialTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastSocial) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSocial_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastSocial());
        }

        #endregion

        #region General Constructor : Class (BlastSocial) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSocial_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastSocial = Fixture.CreateMany<BlastSocial>(2).ToList();
            var firstBlastSocial = instancesOfBlastSocial.FirstOrDefault();
            var lastBlastSocial = instancesOfBlastSocial.Last();

            // Act, Assert
            firstBlastSocial.ShouldNotBeNull();
            lastBlastSocial.ShouldNotBeNull();
            firstBlastSocial.ShouldNotBeSameAs(lastBlastSocial);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSocial_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastSocial = new BlastSocial();
            var secondBlastSocial = new BlastSocial();
            var thirdBlastSocial = new BlastSocial();
            var fourthBlastSocial = new BlastSocial();
            var fifthBlastSocial = new BlastSocial();
            var sixthBlastSocial = new BlastSocial();

            // Act, Assert
            firstBlastSocial.ShouldNotBeNull();
            secondBlastSocial.ShouldNotBeNull();
            thirdBlastSocial.ShouldNotBeNull();
            fourthBlastSocial.ShouldNotBeNull();
            fifthBlastSocial.ShouldNotBeNull();
            sixthBlastSocial.ShouldNotBeNull();
            firstBlastSocial.ShouldNotBeSameAs(secondBlastSocial);
            thirdBlastSocial.ShouldNotBeSameAs(firstBlastSocial);
            fourthBlastSocial.ShouldNotBeSameAs(firstBlastSocial);
            fifthBlastSocial.ShouldNotBeSameAs(firstBlastSocial);
            sixthBlastSocial.ShouldNotBeSameAs(firstBlastSocial);
            sixthBlastSocial.ShouldNotBeSameAs(fourthBlastSocial);
        }

        #endregion

        #endregion

        #endregion
    }
}