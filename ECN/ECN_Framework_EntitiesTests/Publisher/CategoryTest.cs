using System;
using NUnit.Framework;
using ECN_Framework_Entities.Publisher;
using Shouldly;

namespace ECN_Framework_Entities.Publisher.Tests
{
    [TestFixture]
    public class CategoryTest
    {
        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            int categoryID = -1;
            string categoryName = string.Empty;        

            // Act
            Category category = new Category();    

            // Assert
            category.CategoryID.ShouldBe(categoryID);
            category.CategoryName.ShouldBe(categoryName);
            category.IsDeleted.ShouldBeNull();
        }
    }
}