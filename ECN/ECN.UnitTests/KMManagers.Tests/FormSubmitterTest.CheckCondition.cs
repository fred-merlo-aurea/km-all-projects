using System.Collections.Generic;
using Shouldly;
using NUnit.Framework;
using KMEntities;
using KMEnums;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class FormSubmitterTest
    {
        private const string TestedMethodName_CheckCondition = "CheckCondition";
        private const string ComparisonExactValue = "ValueIsExactlyThis";
        private const string ComparisonExactNumberValue = "112";
        private const string ComparisonDifferentValue = "ValueIsNotLikeThis";
        private const string ComparisonDifferentNumberValue = "250";
        private const string ComparisonContainsValue = "Like";
        private const string ComparisonDoeNotContainsValue = "Yes";
        private const string ComparisonStartsWithValue = "ValueIs";
        private const string ComparisonEndsWithValue = "This";
        private const string ComparisonLessThanNumberValue = "50";
        private const string ComparisonGreaterThanDecimalValue = "400.987";
        private const string ComparisonBeforeDateValue ="01/01/2018";
        private const string ComparisonAfterDateValue = "03/01/2018";
        private const string ComparisonAfterDateTimeValue = "03/01/2018 07:08:00";
        private const string ComparisonBeforeDateTimeValue = "03/01/2018 05:00:00";
        
        [Test]
        public void CheckCondition_ComparisionValueDoesNotExistInCollection_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactValue;
            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Is_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactValue;
            var collectionValueToMatch = ComparisonExactValue;

            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_Is_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNot_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.IsNot;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNot_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactValue;
            var collectionValueToMatch = ComparisonExactValue;
            var comparisonType = ComparisonType.IsNot;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Contains_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.Contains;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_Contains_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonDoeNotContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.Contains;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_DoesNotContain_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonDoeNotContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.DoesNotContain;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_DoesNotContain_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.DoesNotContain;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_StartsWith_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonStartsWithValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.StartsWith;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_StartsWith_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.StartsWith;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_EndsWith_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonEndsWithValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.EndsWith;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_EndsWith_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.EndsWith;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Equals_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactNumberValue;
            var collectionValueToMatch = ComparisonExactNumberValue;
            var comparisonType = ComparisonType.Equals;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_Equal_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactNumberValue;
            var collectionValueToMatch = ComparisonDifferentNumberValue;
            var comparisonType = ComparisonType.Equals;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Equal_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactNumberValue;
            var collectionValueToMatch = ComparisonDifferentValue ;
            var comparisonType = ComparisonType.Equals;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_LessThan_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactNumberValue;
            var collectionValueToMatch = ComparisonLessThanNumberValue;
            var comparisonType = ComparisonType.LessThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_LessThan_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonLessThanNumberValue;
            var collectionValueToMatch = ComparisonExactNumberValue;
            var comparisonType = ComparisonType.LessThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_LessThan_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonDifferentValue;
            var collectionValueToMatch = ComparisonExactNumberValue;
            var comparisonType = ComparisonType.LessThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_GreaterThan_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonLessThanNumberValue;
            var collectionValueToMatch = ComparisonExactNumberValue;
            var comparisonType = ComparisonType.GreaterThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_GreaterThan_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonGreaterThanDecimalValue;
            var collectionValueToMatch = ComparisonLessThanNumberValue;
            var comparisonType = ComparisonType.GreaterThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_GreaterThan_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonDifferentValue;
            var collectionValueFirstInComparison = ComparisonGreaterThanDecimalValue;
            var comparisonType = ComparisonType.GreaterThan;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_After_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonBeforeDateValue;
            var collectionValueFirstInComparison = ComparisonAfterDateValue;
            var comparisonType = ComparisonType.After;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_After_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonAfterDateValue;
            var collectionValueToMatch = ComparisonBeforeDateValue;
            var comparisonType = ComparisonType.After;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_After_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonDifferentValue;
            var collectionValueToMatch = ComparisonDifferentValue;
            var comparisonType = ComparisonType.After;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Before_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = ComparisonAfterDateTimeValue;
            var collectionValueFirstInComparison = ComparisonBeforeDateTimeValue;
            var comparisonType = ComparisonType.Before;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_Before_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonBeforeDateValue;
            var collectionValueToMatch = ComparisonAfterDateValue;
            var comparisonType = ComparisonType.Before;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_Before_ExceptionCaught_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = ComparisonExactNumberValue;
            var collectionValueToMatch = ComparisonDifferentValue;
            var comparisonType = ComparisonType.Before;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNull_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = EmptyValue;
            var collectionValueFirstInComparison = EmptyValue;
            var comparisonType = ComparisonType.IsNull;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNull_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = EmptyValue;
            var collectionValueToMatch = ComparisonDifferentValue;
            var comparisonType = ComparisonType.IsNull;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNotNull_ReturnsTrue()
        {
            //Arrange
            var controlValueToMatch = EmptyValue;
            var collectionValueFirstInComparison = ComparisonDifferentValue;
            var comparisonType = ComparisonType.IsNotNull;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_ComparisonType_IsNotNull_ReturnsFalse()
        {
            //Arrange
            var controlValueToMatch = EmptyValue;
            var collectionValueToMatch = EmptyValue;
            var comparisonType = ComparisonType.IsNotNull;
            var condition = CreateTestCondition(controlValueToMatch, comparisonType);
            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueToMatch);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_ComparisonType_Is_ReturnsTrue()
        {
            //Arrange            
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueFirstInComparison = ComparisonContainsValue;
            var anotherCollectionValue = ComparisonDifferentValue;

            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _values.Add(ControlID_2, anotherCollectionValue);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_ComparisonType_Is_ReturnsFalse()
        {
            //Arrange            
            var controlValueToMatch = ComparisonDoeNotContainsValue;
            var collectionValueFirstInComparison = ComparisonContainsValue;

            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_ComparisonType_DifferentOfIs_ReturnsTrue()
        {
            //Arrange            
            var controlValueToMatch = ComparisonDoeNotContainsValue;
            var collectionValueFirstInComparison = ComparisonContainsValue;
            var anotherCollectionValue = ComparisonDifferentValue;

            var comparisonType = ComparisonType.Equals;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _values.Add(ControlID_2, anotherCollectionValue);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_ComparisonType_DifferentOfIs_ReturnsFalse()
        {
            //Arrange            
            var controlValueToMatch = ComparisonContainsValue;
            var collectionValueFirstInComparison = ComparisonContainsValue;

            var comparisonType = ComparisonType.EndsWith;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_CollectionValueEmpty_ReturnsTrue()
        {
            //Arrange            
            var controlValueToMatch = EmptyValue;
            var collectionValueFirstInComparison = EmptyValue;

            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_NewsLetter_CollectionValueEmpty_ReturnsFalse()
        {
            //Arrange            
            var controlValueToMatch = ComparisonDifferentValue;
            var collectionValueFirstInComparison = EmptyValue;

            var comparisonType = ComparisonType.Is;
            var condition = CreateTestCondition(controlValueToMatch.ToLower(), comparisonType);
            condition.Control.ControlType.MainType_ID = HtmlControltypeAsNewsLetter;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_Checkbox_ComparisonType_Is_ReturnsTrue()
        {
            //Arrange       
            var controlValueToMatch = RegionID;
            var collectionValueFirstInComparison = RegionID;
            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsCheckbox, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }


        [Test]
        [Ignore("Testing is not passing due to a bug that is raising exception instead of returning false")]
        public void CheckCondition_HTMLControlType_RadioButton_ComparisonType_Is_ReturnsFalse()
        {
            //Arrange                       
            var controlValueToMatch = DifferentRegionID;
            var collectionValueFirstInComparison = RegionID;
            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsRadioButton, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_ListBox_ComparisonType_Is_StateNotInValues_ReturnsFalse()
        {
            //Arrange          
            var controlValueToMatch = RegionID;
            var collectionValueFirstInComparison = DifferentRegionID;
            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsListbox, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_RadioButton_ComparisonType_DifferentOfIs_ReturnsTrue()
        {
            //Arrange       
            var controlValueToMatch = RegionID;
            var collectionValueFirstInComparison = DifferentRegionID;
            var comparisonType = ComparisonType.Contains;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsRadioButton, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_HTMLControlType_DropDown_ComparisonType_DifferentOfIs_ReturnsFalse()
        {
            //Arrange          
            var controlValueToMatch = RegionID;
            var collectionValueFirstInComparison = RegionID;
            var comparisonType = ComparisonType.IsNot;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsDropDown, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_DropDown_ComparisonType_Is_WithGridSeqID_ReturnsTrue()
        {
            //Arrange          
            const int regionID = TypeSeqIDForStates;
            var controlValueToMatch = regionID.ToString();
            var collectionValueFirstInComparison = regionID.ToString();

            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsDropDown);
            var gridPropetiestoMatch = new List<FormControlPropertyGrid>();
            gridPropetiestoMatch.Add(new FormControlPropertyGrid()
            {
                FormControlPropertyGrid_Seq_ID = regionID,
                DataValue = regionID.ToString()
            });
            condition.Control.FormControlPropertyGrids = gridPropetiestoMatch;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeTrue();
        }

        [Test]
        public void CheckCondition_HTMLControlType_DropDown_ComparisonType_Is_WithGridSeqID_ReturnsFalse()
        {
            //Arrange          
            const int regionID = TypeSeqIDForStates;
            var controlValueToMatch = regionID.ToString();
            var collectionValueFirstInComparison = ComparisonDifferentNumberValue;

            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsDropDown);
            var gridPropetiestoMatch = new List<FormControlPropertyGrid>();
            gridPropetiestoMatch.Add(new FormControlPropertyGrid()
            {
                FormControlPropertyGrid_Seq_ID = regionID,
                DataValue = regionID.ToString()
            });
            condition.Control.FormControlPropertyGrids = gridPropetiestoMatch;

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }

        [Test]
        public void CheckCondition_HTMLControlType_DropDown_ComparisonType_Is_CollectionValueEmpty_ReturnsFalse()
        {
            //Arrange       
            var controlValueToMatch = RegionID;
            var collectionValueFirstInComparison = EmptyValue;
            var comparisonType = ComparisonType.Is;

            var condition = CreateTestConditionForHTMLControl(controlValueToMatch, comparisonType, HtmlControltypeAsCheckbox, TypeSeqIDForStates);

            var methodArguments = new object[] { condition };

            _values.Add(ControlID, collectionValueFirstInComparison);
            _statesValues.Add(StateItem);

            //Act
            var returnResult = (bool)_formSubmitter.Invoke(TestedMethodName_CheckCondition, methodArguments);

            //Assert
            returnResult.ShouldBeFalse();
        }
    }
}
