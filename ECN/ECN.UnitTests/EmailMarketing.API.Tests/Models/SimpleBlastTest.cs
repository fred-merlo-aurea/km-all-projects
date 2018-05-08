using System;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Models
{
    [TestFixture]
    public class SimpleBlastTest
    {
        [Test]
        public void Properties_SetValues_ExpectSetValues()
        {
            // Arrange
            var testValue = Tuple.Create(true, 1, DateTime.Now, 1, DateTime.Now, 1, DateTime.Now);
            var testCommonValue = Tuple.Create(1, "Status1", "CommonBlast", 1, 1, 1);
            var testEmailValue = Tuple.Create("Subject", "From", "Name", "ReplyTo");

			// Act
            var testObject = new SimpleBlast
            {
                IsTestBlast = testValue.Item1,
                SmartSegmentID = testValue.Item2,
                SendTime = testValue.Item3,
                CreatedUserID = testValue.Item4,
                CreatedDate = testValue.Item5,
                UpdatedUserID = testValue.Item6,
                UpdatedDate = testValue.Item7,
                BlastID = testCommonValue.Item1,
                StatusCode = testCommonValue.Item2,
                BlastType = testCommonValue.Item3,
                LayoutID = testCommonValue.Item4,
                GroupID = testCommonValue.Item5,
                FilterID = testCommonValue.Item6,
                EmailSubject = testEmailValue.Item1,
                EmailFrom = testEmailValue.Item2,
                EmailFromName = testEmailValue.Item3,
                ReplyTo = testEmailValue.Item4
            };

			// Assert
            testObject.ShouldSatisfyAllConditions(
                () => testObject.IsTestBlast.ShouldBe(testValue.Item1),
                () => testObject.SmartSegmentID.ShouldBe(testValue.Item2),
                () => testObject.SendTime.ShouldBe(testValue.Item3),
                () => testObject.CreatedUserID.ShouldBe(testValue.Item4),
                () => testObject.CreatedDate.ShouldBe(testValue.Item5),
                () => testObject.UpdatedUserID.ShouldBe(testValue.Item6),
                () => testObject.UpdatedDate.ShouldBe(testValue.Item7),
                () => testObject.BlastID.ShouldBe(testCommonValue.Item1),
                () => testObject.StatusCode.ShouldBe(testCommonValue.Item2),
                () => testObject.BlastType.ShouldBe(testCommonValue.Item3),
                () => testObject.LayoutID.ShouldBe(testCommonValue.Item4),
                () => testObject.GroupID.ShouldBe(testCommonValue.Item5),
                () => testObject.FilterID.ShouldBe(testCommonValue.Item6),
                () => testObject.EmailSubject.ShouldBe(testEmailValue.Item1),
                () => testObject.EmailFrom.ShouldBe(testEmailValue.Item2),
                () => testObject.EmailFromName.ShouldBe(testEmailValue.Item3),
                () => testObject.ReplyTo.ShouldBe(testEmailValue.Item4));
        }
    }
}
