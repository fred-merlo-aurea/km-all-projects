using System;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Models
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        public void Properties_SetValues_ExpectSetValues()
        {
            // Arrange, Act
            var folderValue1 = Tuple.Create(1, 2, 3, DateTime.Today, "Options", "Address");
            var folderValue2 = Tuple.Create(1, DateTime.Today, 2, 3, true);
            var testObject = new Message
            {
                LayoutID = folderValue1.Item1,
                TemplateID = folderValue1.Item2,
                FolderID = folderValue1.Item3,
                UpdatedDate = folderValue1.Item4,
                TableOptions = folderValue1.Item5,
                DisplayAddress = folderValue1.Item6,
                MessageTypeID = folderValue2.Item1,
                CreatedDate = folderValue2.Item2,
                UpdatedUserID = folderValue2.Item3,
                CreatedUserID = folderValue2.Item4,
                Archived = folderValue2.Item5
            };

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => testObject.LayoutID.ShouldBe(folderValue1.Item1),
                () => testObject.TemplateID.ShouldBe(folderValue1.Item2),
                () => testObject.FolderID.ShouldBe(folderValue1.Item3),
                () => testObject.UpdatedDate.ShouldBe(folderValue1.Item4),
                () => testObject.TableOptions.ShouldBe(folderValue1.Item5),
                () => testObject.DisplayAddress.ShouldBe(folderValue1.Item6),
                () => testObject.MessageTypeID.ShouldBe(folderValue2.Item1),
                () => testObject.CreatedDate.ShouldBe(folderValue2.Item2),
                () => testObject.UpdatedUserID.ShouldBe(folderValue2.Item3),
                () => testObject.CreatedUserID.ShouldBe(folderValue2.Item4),
                () => testObject.Archived.ShouldBe(folderValue2.Item5));
        }

    }
}
