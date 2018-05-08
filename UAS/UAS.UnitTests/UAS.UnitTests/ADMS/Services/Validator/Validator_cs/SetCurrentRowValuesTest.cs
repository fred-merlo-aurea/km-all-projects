using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAS.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using LookupEnums = FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SetCurrentRowValuesTest
    {
        private const string IncomingFieldKey = "IncomingField";
        private const string SourceDataKey = "SourceData";
        private const string SampleKey = "SampleKey";

        private PrivateObject _validatorPrivateObject;
        private LookupEnums.MatchTypes _matchType;
        private StringDictionary _row;
        private TransformDataMap _dataMap;
        private FieldMapping _fieldMap;
        private bool? _dmRan;

        [SetUp]
        public void Initialize()
        {
            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
            _row = CreateRowsForDictionary();
            _dataMap = new TransformDataMap();
            _fieldMap = CreateFieldMapping();
            _dmRan = false;
        }

        [Test]
        public void SetCurrentRowValues_WhenSubscriberTransformedIsNull_ThrowsException()
        {
            // Arrange
            _row = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan));
        }

        [Test]
        public void SetCurrentRowValues_WhenTransformDataMapIsNull_ThrowsException()
        {
            // Arrange
            _dataMap = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan));
        }

        [Test]
        public void SetCurrentRowValues_WhenFieldMappingIsNull_ThrowsException()
        {
            // Arrange
            _fieldMap = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsAnyCharacter_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Any_Character;
            _dataMap = CreateDataMap(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsEquals_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Equals;
            _dataMap = CreateDataMap(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsLike_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Like;
            _dataMap = CreateDataMap(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsNotEquals_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Not_Equals;
            _dataMap = CreateDataMapWithDifferentSourceData(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsHasData_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Has_Data;
            _dataMap = CreateDataMap(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsNullOrEmpty_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Is_Null_or_Empty;
            _dataMap = CreateDataMap(_matchType.ToString());
            _row = new StringDictionary
            {
                [IncomingFieldKey] = string.Empty
            };

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsFindAndReplace_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Find_and_Replace;
            _dataMap = CreateDataMapWithDifferentSourceData(_matchType.ToString());
            var rowResult = _row[_fieldMap.IncomingField].Replace(_dataMap.SourceData, _dataMap.DesiredData).Trim();

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(rowResult));
        }

        [Test]
        public void SetCurrentRowValues_WhenMatchTypesIsDefault_ReturnsTrue()
        {
            // Arrange
            _matchType = LookupEnums.MatchTypes.Default;
            _dataMap = CreateDataMap(_matchType.ToString());

            // Act
            var result = _validatorPrivateObject.Invoke(
                SetCurrentRowValuesMethod, _row, _dataMap, _fieldMap, _matchType, _dmRan) as bool?;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<bool>(),
                () => result.Value.ShouldBeTrue(),
                () => _row[IncomingFieldKey].ShouldBe(_dataMap.DesiredData));
        }

        private static StringDictionary CreateRowsForDictionary()
        {
            return new StringDictionary
            {
                [IncomingFieldKey] = SourceDataKey
            };
        }

        private static TransformDataMap CreateDataMap(string desiredData)
        {
            return new TransformDataMap
            {
                SourceData = SourceDataKey,
                DesiredData = desiredData
            };
        }

        private static TransformDataMap CreateDataMapWithDifferentSourceData(string desiredData)
        {
            return new TransformDataMap
            {
                SourceData = SampleKey,
                DesiredData = desiredData
            };
        }

        private static FieldMapping CreateFieldMapping()
        {
            return new FieldMapping
            {
                IncomingField = IncomingFieldKey
            };
        }
    }
}
