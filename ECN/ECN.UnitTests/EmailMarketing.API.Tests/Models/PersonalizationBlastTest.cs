using System;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Models
{
	[TestFixture]
	public class PersonalizationBlastTest
	{
		[Test]
		public void Properties_SetValues_ExpectSetValues()
		{
			// Arrange
			var testValue = Tuple.Create("Test1", "Test2", 1, DateTime.Now, 2, DateTime.Today);
			var testCommonValue = Tuple.Create(1, "Status1", "CommonBlast", 1, 1, 1);
			var testEmailValue = Tuple.Create("Subject", "From", "Name", "ReplyTo");

			// Act
			var testObject = new PersonalizationBlast
			{
				CampaignName = testValue.Item1,
				CampaignItemName = testValue.Item2,
				CreatedUserID = testValue.Item3,
				CreatedDate = testValue.Item4,
				UpdatedUserID = testValue.Item5,
				UpdatedDate = testValue.Item6,
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
				() => testObject.CampaignName.ShouldBe(testValue.Item1),
				() => testObject.CampaignItemName.ShouldBe(testValue.Item2),
				() => testObject.CreatedUserID.ShouldBe(testValue.Item3),
				() => testObject.CreatedDate.ShouldBe(testValue.Item4),
				() => testObject.UpdatedUserID.ShouldBe(testValue.Item5),
				() => testObject.UpdatedDate.ShouldBe(testValue.Item6),
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

