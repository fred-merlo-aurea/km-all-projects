using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public  class DiskSpaceTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region General Constructor : Class (DiskSpace) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DiskSpace_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DiskSpace());
        }

        #endregion

        #region General Constructor : Class (DiskSpace) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DiskSpace_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDiskSpace = new DiskSpace();
            var secondDiskSpace = new DiskSpace();
            var thirdDiskSpace = new DiskSpace();
            var fourthDiskSpace = new DiskSpace();
            var fifthDiskSpace = new DiskSpace();
            var sixthDiskSpace = new DiskSpace();

            // Act, Assert
            firstDiskSpace.ShouldNotBeNull();
            secondDiskSpace.ShouldNotBeNull();
            thirdDiskSpace.ShouldNotBeNull();
            fourthDiskSpace.ShouldNotBeNull();
            fifthDiskSpace.ShouldNotBeNull();
            sixthDiskSpace.ShouldNotBeNull();
            firstDiskSpace.ShouldNotBeSameAs(secondDiskSpace);
            thirdDiskSpace.ShouldNotBeSameAs(firstDiskSpace);
            fourthDiskSpace.ShouldNotBeSameAs(firstDiskSpace);
            fifthDiskSpace.ShouldNotBeSameAs(firstDiskSpace);
            sixthDiskSpace.ShouldNotBeSameAs(firstDiskSpace);
            sixthDiskSpace.ShouldNotBeSameAs(fourthDiskSpace);
        }

        #endregion

        #endregion

        #endregion
    }
}