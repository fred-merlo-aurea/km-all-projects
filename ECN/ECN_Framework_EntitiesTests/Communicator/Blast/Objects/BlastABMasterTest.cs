using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastABMasterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastABMaster) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastABMaster_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastABMaster());
        }

        #endregion

        #region General Constructor : Class (BlastABMaster) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastABMaster_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastABMaster = new BlastABMaster();
            var secondBlastABMaster = new BlastABMaster();
            var thirdBlastABMaster = new BlastABMaster();
            var fourthBlastABMaster = new BlastABMaster();
            var fifthBlastABMaster = new BlastABMaster();
            var sixthBlastABMaster = new BlastABMaster();

            // Act, Assert
            firstBlastABMaster.ShouldNotBeNull();
            secondBlastABMaster.ShouldNotBeNull();
            thirdBlastABMaster.ShouldNotBeNull();
            fourthBlastABMaster.ShouldNotBeNull();
            fifthBlastABMaster.ShouldNotBeNull();
            sixthBlastABMaster.ShouldNotBeNull();
            firstBlastABMaster.ShouldNotBeSameAs(secondBlastABMaster);
            thirdBlastABMaster.ShouldNotBeSameAs(firstBlastABMaster);
            fourthBlastABMaster.ShouldNotBeSameAs(firstBlastABMaster);
            fifthBlastABMaster.ShouldNotBeSameAs(firstBlastABMaster);
            sixthBlastABMaster.ShouldNotBeSameAs(firstBlastABMaster);
            sixthBlastABMaster.ShouldNotBeSameAs(fourthBlastABMaster);
        }

        #endregion

        #region General Constructor : Class (BlastABMaster) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastABMaster_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange

            // Act
            var blastABMaster = new BlastABMaster();

            // Assert
            blastABMaster.BlastA.ShouldBeNull();
            blastABMaster.BlastB.ShouldBeNull();
            blastABMaster.BlastA.ShouldBeNull();
            blastABMaster.BlastB.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}