using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class LinkTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int linkID = -1;
            int pageID = -1;
            string linkType = string.Empty;
            string linkURL = string.Empty;
            int x1 = -1;
            int y1 = -1;
            int x2 = -1;
            int y2 = -1;
            string alias = string.Empty;        

            // Act
            Link link = new Link();    

            // Assert
            link.LinkID.ShouldBe(linkID);
            link.PageID.ShouldBe(pageID);
            link.LinkType.ShouldBe(linkType);
            link.LinkURL.ShouldBe(linkURL);
            link.x1.ShouldBe(x1);
            link.y1.ShouldBe(y1);
            link.x2.ShouldBe(x2);
            link.y2.ShouldBe(y2);
            link.Alias.ShouldBe(alias);
            link.CreatedUserID.ShouldBeNull();
            link.CreatedDate.ShouldBeNull();
            link.UpdatedUserID.ShouldBeNull();
            link.UpdatedDate.ShouldBeNull();
            link.IsDeleted.ShouldBeNull();
            link.CustomerID.ShouldBeNull();
        }
    }
}