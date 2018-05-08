using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastLayoutTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastLayout) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLayout_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastLayout());
        }

        #endregion

        #region General Constructor : Class (BlastLayout) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLayout_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastLayout = Fixture.CreateMany<BlastLayout>(2).ToList();
            var firstBlastLayout = instancesOfBlastLayout.FirstOrDefault();
            var lastBlastLayout = instancesOfBlastLayout.Last();

            // Act, Assert
            firstBlastLayout.ShouldNotBeNull();
            lastBlastLayout.ShouldNotBeNull();
            firstBlastLayout.ShouldNotBeSameAs(lastBlastLayout);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastLayout_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastLayout = new BlastLayout();
            var secondBlastLayout = new BlastLayout();
            var thirdBlastLayout = new BlastLayout();
            var fourthBlastLayout = new BlastLayout();
            var fifthBlastLayout = new BlastLayout();
            var sixthBlastLayout = new BlastLayout();

            // Act, Assert
            firstBlastLayout.ShouldNotBeNull();
            secondBlastLayout.ShouldNotBeNull();
            thirdBlastLayout.ShouldNotBeNull();
            fourthBlastLayout.ShouldNotBeNull();
            fifthBlastLayout.ShouldNotBeNull();
            sixthBlastLayout.ShouldNotBeNull();
            firstBlastLayout.ShouldNotBeSameAs(secondBlastLayout);
            thirdBlastLayout.ShouldNotBeSameAs(firstBlastLayout);
            fourthBlastLayout.ShouldNotBeSameAs(firstBlastLayout);
            fifthBlastLayout.ShouldNotBeSameAs(firstBlastLayout);
            sixthBlastLayout.ShouldNotBeSameAs(firstBlastLayout);
            sixthBlastLayout.ShouldNotBeSameAs(fourthBlastLayout);
        }

        #endregion

        #endregion

        #endregion
    }
}