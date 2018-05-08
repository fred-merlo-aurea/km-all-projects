using Shouldly;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ECN_Framework_Entities.Salesforce.Sorting;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity.Sorting
{
    [TestFixture]
    public class EntitySortTest
    {
        [Test]
        public void Sort_ByAscending_ReturnsSortedCollection()
        {
            // Arrange
            const bool isAscending = true;
            const string propertyToSort = "Id";
            var utility = new EntitySort();
            var originalList = BuildUnsortedList();
            var expectedResult = originalList.OrderBy(x => x.Id).ToArray();

            // Act
            var result = utility.Sort(originalList, propertyToSort, isAscending).ToArray();

            // Assert
            result.ShouldBeInOrder(SortDirection.Ascending, new JustForTestCompare());
        }

        [Test]
        public void Sort_ByDescending_ReturnsSortedCollection()
        {
            // Arrange
            const bool isAscending = false;
            const string propertyToSort = "Id";
            var utility = new EntitySort();
            var originalList = BuildUnsortedList();
            var expectedResult = originalList.OrderByDescending(x => x.Id).ToArray();

            // Act
            var result = utility.Sort(originalList, propertyToSort, isAscending).ToArray();

            // Assert
            result.ShouldBeInOrder(SortDirection.Descending, new JustForTestCompare());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Sort_ByExcludedProperty_ReturnsOriginalCollection(bool isAscending)
        {
            // Arrange
            const string propertyToSort = "Id";
            var utility = new EntitySort();
            utility.ExcludeFromSorting<JustForUnitTest, int>(x => x.Id);
            var originalList = BuildUnsortedList();

            // Act
            var result = utility.Sort(originalList, propertyToSort, isAscending);

            // Assert
            result.ShouldBeSameAs(originalList);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Sort_ByNotExistProperty_ReturnsOriginalCollection(bool isAscending)
        {
            // Arrange
            const string notExistProperty = "notExistProperty";
            var utility = new EntitySort();
            var originalList = BuildUnsortedList();

            // Act
            var result = utility.Sort(originalList, notExistProperty, isAscending);

            // Assert
            result.ShouldBeSameAs(originalList);
        }

        private JustForUnitTest[] BuildUnsortedList()
        {
            return new[]
            {
                new JustForUnitTest(2),
                new JustForUnitTest(3),
                new JustForUnitTest(1)
            };

        }

        private class JustForUnitTest
        {
            public JustForUnitTest(int id)
            {
                Id = id;
            }
            public int Id { get; set; }
        }

        private class JustForTestCompare : IComparer<JustForUnitTest>
        {
            public int Compare(JustForUnitTest x, JustForUnitTest y)
            {
                return x.Id - y.Id;
            }
        }
    }
}
