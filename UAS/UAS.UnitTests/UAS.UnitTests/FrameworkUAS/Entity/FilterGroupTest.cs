using System;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAS.Entity
{
	[TestFixture]
	public class FilterGroupTest
	{
		private const string DateCreatedPropertyName = "DateCreated";
		private const string DateUpdatedPropertyName = "DateUpdated";
		private const string CreatedByUserIdPropertyName = "CreatedByUserID";
		private const string UpdatedByUserIdPropertyName = "UpdatedByUserID";
		private const int DefaultUserId = 0;

		[TestCase(typeof(FilterGroup))]
		[TestCase(typeof(FilterExportField))]
		[TestCase(typeof(FilterDetailSelectedValue))]
		public void Filter_SetAndGetValue_ReturnsDefaultValue(Type type)
		{
			// Arrange, Act
			var typeInstance  = Activator.CreateInstance(type);

			// Assert
			var dateCreatedValue = (DateTime)typeInstance.GetPropertyValue(DateCreatedPropertyName);

			typeInstance.ShouldSatisfyAllConditions(
				() => dateCreatedValue.Date.ShouldBe(DateTime.Now.Date),
				() => typeInstance.GetPropertyValue(DateUpdatedPropertyName).ShouldBeNull(),
				() => typeInstance.GetPropertyValue(CreatedByUserIdPropertyName).ShouldBe(DefaultUserId),
				() => typeInstance.GetPropertyValue(UpdatedByUserIdPropertyName).ShouldBeNull());
		}

		[TestCase(typeof(FilterGroup))]
		[TestCase(typeof(FilterExportField))]
		[TestCase(typeof(FilterDetailSelectedValue))]
		public void Filter_SetAndGetValue_ReturnsSetValue(Type type)
		{
			// Arrange
			var typeInstance = Activator.CreateInstance(type);
			var date = DateTime.Now.Date;

			//userId is defined as object as SetProperty has 2 overload method
			//one having object and other having int as parameter. It is required to pass
			//userId as object to assign property value.
			object userId = 1;

			// Act
			typeInstance.SetProperty(DateCreatedPropertyName, date);
			typeInstance.SetProperty(DateUpdatedPropertyName, date);
			typeInstance.SetProperty(CreatedByUserIdPropertyName, userId);
			typeInstance.SetProperty(UpdatedByUserIdPropertyName, userId);

			// Assert
			typeInstance.ShouldSatisfyAllConditions(
				() => typeInstance.GetPropertyValue(DateCreatedPropertyName).ShouldBe(date),
				() => typeInstance.GetPropertyValue(DateUpdatedPropertyName).ShouldBe(date),
				() => typeInstance.GetPropertyValue(CreatedByUserIdPropertyName).ShouldBe(userId),
				() => typeInstance.GetPropertyValue(UpdatedByUserIdPropertyName).ShouldBe(userId));
		}
	}
}