using System;
using EmailMarketing.API.Models;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests
{
	[TestFixture]
	public class SimpleBlastV2Test
	{
		[Test]
		public void Properties_SetValues_ExpectSetValues()
		{
			// Arrange, Act
			var testValue = Tuple.Create("Test1", "Test2", 1, DateTime.Now, 2, DateTime.Today);
			var testObject = new SimpleBlastV2
			{
				CampaignName = testValue.Item1,
				CampaignItemName = testValue.Item2,
				CreatedUserID = testValue.Item3,
				CreatedDate = testValue.Item4,
				UpdatedUserID = testValue.Item5,
				UpdatedDate = testValue.Item6
			};

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => testObject.CampaignName.ShouldBe(testValue.Item1),
				() => testObject.CampaignItemName.ShouldBe(testValue.Item2),
				() => testObject.CreatedUserID.ShouldBe(testValue.Item3),
				() => testObject.CreatedDate.ShouldBe(testValue.Item4),
				() => testObject.UpdatedUserID.ShouldBe(testValue.Item5),
				() => testObject.UpdatedDate.ShouldBe(testValue.Item6)
				);
		}
	}
}
