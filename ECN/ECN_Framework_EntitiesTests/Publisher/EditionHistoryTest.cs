using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class EditionHistoryTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int editionHistoryID = -1;        

            // Act
            EditionHistory editionHistory = new EditionHistory();    

            // Assert
            editionHistory.EditionHistoryID.ShouldBe(editionHistoryID);
            editionHistory.EditionID.ShouldBeNull();
            editionHistory.ActivatedDate.ShouldBeNull();
            editionHistory.ArchievedDate.ShouldBeNull();
            editionHistory.DeActivatedDate.ShouldBeNull();
            editionHistory.CreatedUserID.ShouldBeNull();
            editionHistory.CreatedDate.ShouldBeNull();
            editionHistory.UpdatedUserID.ShouldBeNull();
            editionHistory.UpdatedDate.ShouldBeNull();
            editionHistory.IsDeleted.ShouldBeNull();
            editionHistory.CustomerID.ShouldBeNull();
        }
    }
}