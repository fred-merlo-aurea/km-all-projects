using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class PageTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int pageID = -1;
            int editionID = -1;
            int pageNumber = -1;
            string displayNumber = string.Empty;
            string textContent = string.Empty;        

            // Act
            Page page = new Page();    

            // Assert
            page.PageID.ShouldBe(pageID);
            page.EditionID.ShouldBe(editionID);
            page.PageNumber.ShouldBe(pageNumber);
            page.DisplayNumber.ShouldBe(displayNumber);
            page.Width.ShouldBeNull();
            page.Height.ShouldBeNull();
            page.TextContent.ShouldBe(textContent);
            page.CreatedUserID.ShouldBeNull();
            page.CreatedDate.ShouldBeNull();
            page.UpdatedUserID.ShouldBeNull();
            page.UpdatedDate.ShouldBeNull();
            page.IsDeleted.ShouldBeNull();
            page.LinkList.ShouldBeEmpty();
            page.CustomerID.ShouldBeNull();
        }
    }
}