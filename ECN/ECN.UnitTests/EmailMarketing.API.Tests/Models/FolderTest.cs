using System;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Models
{
    [TestFixture]
    public class FolderTest
    {
        [Test]
        public void Properties_SetValues_ExpectSetValues()
        {
            // Arrange, Act
            var folderValue1 = Tuple.Create(1, "Name", 2, "Desc", "Type", 3);
            var folderValue2 = Tuple.Create(DateTime.Today, 1, DateTime.Today, 2);
            var testObject = new Folder
            {
                FolderID = folderValue1.Item1,
                FolderName = folderValue1.Item2,
                ParentID = folderValue1.Item3,
                FolderDescription = folderValue1.Item4,
                FolderType = folderValue1.Item5,
                CustomerID = folderValue1.Item6,
                CreatedDate = folderValue2.Item1,
                CreatedUserID =folderValue2.Item2,
                UpdatedDate = folderValue2.Item3,
                UpdatedUserID = folderValue2.Item4
            };

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => testObject.FolderID.ShouldBe(folderValue1.Item1),
                () => testObject.FolderName.ShouldBe(folderValue1.Item2),
                () => testObject.ParentID.ShouldBe(folderValue1.Item3),
                () => testObject.FolderDescription.ShouldBe(folderValue1.Item4),
                () => testObject.FolderType.ShouldBe(folderValue1.Item5),
                () => testObject.CustomerID.ShouldBe(folderValue1.Item6),
                () => testObject.CreatedDate.ShouldBe(folderValue2.Item1),
                () => testObject.CreatedUserID.ShouldBe(folderValue2.Item2),
                () => testObject.UpdatedDate.ShouldBe(folderValue2.Item3),
                () => testObject.UpdatedUserID.ShouldBe(folderValue2.Item4));
        }
    }
}
