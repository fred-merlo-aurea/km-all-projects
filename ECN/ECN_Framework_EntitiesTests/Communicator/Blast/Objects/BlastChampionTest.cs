using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastChampionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastChampion) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastChampion_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastChampion());
        }

        #endregion

        #region General Constructor : Class (BlastChampion) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastChampion_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastChampion = Fixture.CreateMany<BlastChampion>(2).ToList();
            var firstBlastChampion = instancesOfBlastChampion.FirstOrDefault();
            var lastBlastChampion = instancesOfBlastChampion.Last();

            // Act, Assert
            firstBlastChampion.ShouldNotBeNull();
            lastBlastChampion.ShouldNotBeNull();
            firstBlastChampion.ShouldNotBeSameAs(lastBlastChampion);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastChampion_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastChampion = new BlastChampion();
            var secondBlastChampion = new BlastChampion();
            var thirdBlastChampion = new BlastChampion();
            var fourthBlastChampion = new BlastChampion();
            var fifthBlastChampion = new BlastChampion();
            var sixthBlastChampion = new BlastChampion();

            // Act, Assert
            firstBlastChampion.ShouldNotBeNull();
            secondBlastChampion.ShouldNotBeNull();
            thirdBlastChampion.ShouldNotBeNull();
            fourthBlastChampion.ShouldNotBeNull();
            fifthBlastChampion.ShouldNotBeNull();
            sixthBlastChampion.ShouldNotBeNull();
            firstBlastChampion.ShouldNotBeSameAs(secondBlastChampion);
            thirdBlastChampion.ShouldNotBeSameAs(firstBlastChampion);
            fourthBlastChampion.ShouldNotBeSameAs(firstBlastChampion);
            fifthBlastChampion.ShouldNotBeSameAs(firstBlastChampion);
            sixthBlastChampion.ShouldNotBeSameAs(firstBlastChampion);
            sixthBlastChampion.ShouldNotBeSameAs(fourthBlastChampion);
        }

        #endregion

        #endregion

        #endregion
    }
}