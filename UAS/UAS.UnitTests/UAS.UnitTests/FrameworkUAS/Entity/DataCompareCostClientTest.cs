using System;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAS.Entity
{
	[TestFixture]
	public class DataCompareCostClientTest
	{
		private const string CodeTypeIdPropertyName = "CodeTypeId";
		private const string CodeTypeCostModifierPropertyName = "CodeTypeCostModifier";
		private const string DateCreatedPropertyName = "DateCreated";
		private const string CreatedByUserIdPropertyName = "CreatedByUserId";
		private const string DateUpdatedPropertyName = "DateUpdated";
		private const string UpdatedByUserIdPropertyName = "UpdatedByUserId";
		private const string ClientIdPropertyName = "ClientId";
		private const int DefaultIntValue = 0;
		private const decimal DummyDecimal = 10;

		/// <summary>
		/// intPropertyValue is defined as object as SetProperty has 2 overload method
		/// one having object and other having int as parameter. It is required to pass
		/// intPropertyValue as object to assign property value.
		/// </summary>
		private object _intPropertyValue = 1;

		[TestCase(typeof(DataCompareCostClient))]
		[TestCase(typeof(DataCompareCostUser))]
		[TestCase(typeof(DataCompareCostThirdParty))]
		public void DataCompare_SetAndGetValue_ReturnsDefaultValue(Type type)
		{
			// Arrange, Act
			var typeInstance = Activator.CreateInstance(type);

			// Assert
			var dateCreatedValue = (DateTime)typeInstance.GetPropertyValue(DateCreatedPropertyName);

			typeInstance.ShouldSatisfyAllConditions(
				() => typeInstance.GetPropertyValue(CodeTypeIdPropertyName).ShouldBe(DefaultIntValue),
				() => typeInstance.GetPropertyValue(CodeTypeCostModifierPropertyName).ShouldBe(DefaultIntValue),
				() => dateCreatedValue.Date.ShouldBe(DateTime.Now.Date),
				() => typeInstance.GetPropertyValue(CreatedByUserIdPropertyName).ShouldBe(DefaultIntValue),
				() => typeInstance.GetPropertyValue(DateUpdatedPropertyName).ShouldBeNull(),
				() => typeInstance.GetPropertyValue(UpdatedByUserIdPropertyName).ShouldBeNull());
		}

		[TestCase(typeof(DataCompareCostClient))]
		[TestCase(typeof(DataCompareCostUser))]
		[TestCase(typeof(DataCompareCostThirdParty))]
		public void DataCompare_SetAndGetValue_ReturnsSetValue(Type type)
		{
			// Arrange
			var typeInstance = Activator.CreateInstance(type);
			var date = DateTime.Now.Date;

			// Act
			typeInstance.SetProperty(CodeTypeIdPropertyName, _intPropertyValue);
			typeInstance.SetProperty(CodeTypeCostModifierPropertyName, DummyDecimal);
			typeInstance.SetProperty(DateCreatedPropertyName, date);
			typeInstance.SetProperty(CreatedByUserIdPropertyName, _intPropertyValue);
			typeInstance.SetProperty(DateUpdatedPropertyName, date);
			typeInstance.SetProperty(UpdatedByUserIdPropertyName, _intPropertyValue);

			// Assert
			typeInstance.ShouldSatisfyAllConditions(
				() => typeInstance.GetPropertyValue(CodeTypeIdPropertyName).ShouldBe(_intPropertyValue),
				() => typeInstance.GetPropertyValue(CodeTypeCostModifierPropertyName).ShouldBe(DummyDecimal),
				() => typeInstance.GetPropertyValue(DateCreatedPropertyName).ShouldBe(date),
				() => typeInstance.GetPropertyValue(CreatedByUserIdPropertyName).ShouldBe(_intPropertyValue),
				() => typeInstance.GetPropertyValue(DateUpdatedPropertyName).ShouldBe(date),
				() => typeInstance.GetPropertyValue(UpdatedByUserIdPropertyName).ShouldBe(_intPropertyValue));
		}

		[TestCase(typeof(DataCompareCostClient))]
		[TestCase(typeof(DataCompareCostThirdParty))]
		public void DataCompareClientId_SetAndGetValue_ReturnsDefaultValue(Type type)
		{
			// Arrange, Act
			var typeInstance = Activator.CreateInstance(type);

			// Assert
			typeInstance.GetPropertyValue(ClientIdPropertyName).ShouldBe(DefaultIntValue);
		}

		[TestCase(typeof(DataCompareCostClient))]
		[TestCase(typeof(DataCompareCostThirdParty))]
		public void DataCompareClientId_SetAndGetValue_ReturnsSetValue(Type type)
		{
			// Arrange
			var typeInstance = Activator.CreateInstance(type);

			// Act
			typeInstance.SetProperty(ClientIdPropertyName, _intPropertyValue);

			// Assert
			typeInstance.GetPropertyValue(ClientIdPropertyName).ShouldBe(_intPropertyValue);
		}
	}
}