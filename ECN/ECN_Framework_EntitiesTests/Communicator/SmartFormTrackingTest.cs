using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SmartFormTrackingTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int sALID = -1;
            string referringUrl = string.Empty;        

            // Act
            SmartFormTracking smartFormTracking = new SmartFormTracking();    

            // Assert
            smartFormTracking.SALID.ShouldBe(sALID);
            smartFormTracking.BlastID.ShouldBeNull();
            smartFormTracking.CustomerID.ShouldBeNull();
            smartFormTracking.SFID.ShouldBeNull();
            smartFormTracking.GroupID.ShouldBeNull();
            smartFormTracking.ReferringUrl.ShouldBe(referringUrl);
            smartFormTracking.ActivityDate.ShouldBeNull();
        }
    }
}