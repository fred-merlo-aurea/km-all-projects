using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture]
    public class SmartSegmentTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int smartSegmentID = -1;
            string smartSegmentName = string.Empty;
            int smartSegmentOldID = -1;        

            // Act
            SmartSegment smartSegment = new SmartSegment();    

            // Assert
            smartSegment.SmartSegmentID.ShouldBe(smartSegmentID);
            smartSegment.SmartSegmentName.ShouldBe(smartSegmentName);
            smartSegment.SmartSegmentOldID.ShouldBe(smartSegmentOldID);
            smartSegment.CreatedUserID.ShouldBeNull();
            smartSegment.CreatedDate.ShouldBeNull();
            smartSegment.UpdatedUserID.ShouldBeNull();
            smartSegment.UpdatedDate.ShouldBeNull();
            smartSegment.IsDeleted.ShouldBeNull();
        }
    }
}