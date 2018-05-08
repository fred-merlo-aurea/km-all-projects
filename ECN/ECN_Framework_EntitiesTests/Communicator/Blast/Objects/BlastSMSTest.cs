using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Blast.Objects
{
    [TestFixture]
    public class BlastSMSTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (BlastSMS) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSMS_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastSMS());
        }

        #endregion

        #region General Constructor : Class (BlastSMS) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSMS_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastSMS = Fixture.CreateMany<BlastSMS>(2).ToList();
            var firstBlastSMS = instancesOfBlastSMS.FirstOrDefault();
            var lastBlastSMS = instancesOfBlastSMS.Last();

            // Act, Assert
            firstBlastSMS.ShouldNotBeNull();
            lastBlastSMS.ShouldNotBeNull();
            firstBlastSMS.ShouldNotBeSameAs(lastBlastSMS);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSMS_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastSMS = new BlastSMS();
            var secondBlastSMS = new BlastSMS();
            var thirdBlastSMS = new BlastSMS();
            var fourthBlastSMS = new BlastSMS();
            var fifthBlastSMS = new BlastSMS();
            var sixthBlastSMS = new BlastSMS();

            // Act, Assert
            firstBlastSMS.ShouldNotBeNull();
            secondBlastSMS.ShouldNotBeNull();
            thirdBlastSMS.ShouldNotBeNull();
            fourthBlastSMS.ShouldNotBeNull();
            fifthBlastSMS.ShouldNotBeNull();
            sixthBlastSMS.ShouldNotBeNull();
            firstBlastSMS.ShouldNotBeSameAs(secondBlastSMS);
            thirdBlastSMS.ShouldNotBeSameAs(firstBlastSMS);
            fourthBlastSMS.ShouldNotBeSameAs(firstBlastSMS);
            fifthBlastSMS.ShouldNotBeSameAs(firstBlastSMS);
            sixthBlastSMS.ShouldNotBeSameAs(firstBlastSMS);
            sixthBlastSMS.ShouldNotBeSameAs(fourthBlastSMS);
        }

        #endregion

        #endregion

        #endregion
    }
}