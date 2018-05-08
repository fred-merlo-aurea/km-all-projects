using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastRegularTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastRegular) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastRegular_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastRegular());
        }

        #endregion

        #region General Constructor : Class (BlastRegular) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastRegular_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastRegular = Fixture.CreateMany<BlastRegular>(2).ToList();
            var firstBlastRegular = instancesOfBlastRegular.FirstOrDefault();
            var lastBlastRegular = instancesOfBlastRegular.Last();

            // Act, Assert
            firstBlastRegular.ShouldNotBeNull();
            lastBlastRegular.ShouldNotBeNull();
            firstBlastRegular.ShouldNotBeSameAs(lastBlastRegular);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastRegular_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastRegular = new BlastRegular();
            var secondBlastRegular = new BlastRegular();
            var thirdBlastRegular = new BlastRegular();
            var fourthBlastRegular = new BlastRegular();
            var fifthBlastRegular = new BlastRegular();
            var sixthBlastRegular = new BlastRegular();

            // Act, Assert
            firstBlastRegular.ShouldNotBeNull();
            secondBlastRegular.ShouldNotBeNull();
            thirdBlastRegular.ShouldNotBeNull();
            fourthBlastRegular.ShouldNotBeNull();
            fifthBlastRegular.ShouldNotBeNull();
            sixthBlastRegular.ShouldNotBeNull();
            firstBlastRegular.ShouldNotBeSameAs(secondBlastRegular);
            thirdBlastRegular.ShouldNotBeSameAs(firstBlastRegular);
            fourthBlastRegular.ShouldNotBeSameAs(firstBlastRegular);
            fifthBlastRegular.ShouldNotBeSameAs(firstBlastRegular);
            sixthBlastRegular.ShouldNotBeSameAs(firstBlastRegular);
            sixthBlastRegular.ShouldNotBeSameAs(fourthBlastRegular);
        }

        #endregion

        #endregion

        #endregion
    }
}