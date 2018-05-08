using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using DQM.Helpers.Validation;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class SetAdHocTest : Fakes
    {
        private const string SampleStandardField = "SampleStandardField";
        private const string SampleDimension = "SampleDimension";
        private const string SampleSourceValue = "5";
        private const string SampleDimensionValue = "10";
        private const string GreaterThanOperator = "greater_than";
        private const string LessThanOperator = "less_than";
        private const string GreaterThanOrEqualOperator = "greater_than_or_equal_to";
        private const string LessThanOrEqualOperator = "less_than_or_equal_to";
        private const string IsNotLessThanOperator = "is_not_less_than";
        private const string IsNotGreaterThanOperator = "is_not_greater_than";
        private const string EqualOperator = "equal";
        private const string NotEqualOperator = "not_equal";
        private const string ContainsOperator = "contains";
        private const string StartsWithOperator = "starts_with";
        private const string EndsWithOperator = "ends_with";
        private const string SetAdHocMethodName = "SetAdHoc";
        private FileValidator testEntity;
        private AdHocDimensionGroup dimensionGroup;
        private StringDictionary stringDictionary;
        private List<AdHocDimension> adList;

        [SetUp]
        public void SetUp()
        {
            testEntity = new FileValidator();
            SetupFakes(new Mocks());
            dimensionGroup = new AdHocDimensionGroup();
            dimensionGroup.StandardField = SampleStandardField;
            dimensionGroup.DefaultValue = SampleSourceValue;
            dimensionGroup.CreatedDimension = SampleDimension;
            stringDictionary = new StringDictionary();
            stringDictionary.Add(SampleStandardField, SampleSourceValue);
            stringDictionary.Add(SampleDimension, string.Empty);

            adList = new List<AdHocDimension>();
        }

        [Test]
        public void SetAdHoc_WithGreaterThanOperator_SetsAdHocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension {
                MatchValue = "0",
                Operator = GreaterThanOperator,
                DimensionValue = SampleDimension
            });
            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
           var result = ReflectionHelper.CallMethod(
               testEntity.GetType(), 
               SetAdHocMethodName, 
               parameters, 
               testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WIthLessThanOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "10",
                Operator = LessThanOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithGreaterThanOrEqualOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = GreaterThanOrEqualOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithLessThanOrEqualOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = LessThanOrEqualOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithIsNotLessThanOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = IsNotLessThanOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithIsNotGreaterThanOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = IsNotGreaterThanOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithEqualOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = EqualOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithNotEqualOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "10",
                Operator = NotEqualOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithContainsOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = ContainsOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithStartsWithOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = StartsWithOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithEndsWithOperator_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = EndsWithOperator,
                DimensionValue = SampleDimensionValue
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(adList[0].DimensionValue);
        }

        [Test]
        public void SetAdHoc_WithEmptyDimensionValue_SetsAdhocDimension()
        {
            // Arrange
            adList.Add(new AdHocDimension
            {
                MatchValue = "5",
                Operator = "ends_with",
                DimensionValue = null
            });

            var parameters = new object[] { dimensionGroup, stringDictionary, adList };

            // Act
            var result = ReflectionHelper.CallMethod(
                testEntity.GetType(),
                SetAdHocMethodName,
                parameters,
                testEntity) as StringDictionary;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result[dimensionGroup.CreatedDimension].ShouldBe(string.Empty);
        }
    }
}
