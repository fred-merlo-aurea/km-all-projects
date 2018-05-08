using System;
using KMPS.MD.Helpers;
using KMPS.MD.Helpers.Fakes;
using KMPS.MD.Objects;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    public partial class AdhocHelperTests
    {
        [Test]
        public void SetDefaultSettings_NullControlSet_ThrowsArgumentNull()
        {
            // Arrange
            AdhocDataListItemControlSet controlSetNull = null;

            //Act, Assert
            Should.Throw<ArgumentNullException>(() => AdhocHelper.SetDefaultSettings(controlSetNull, _field));
        }

        [Test]
        public void SetDefaultSettings_NullField_ThrowsArgumentNull()
        {
            // Arrange
            Field fieldNull = null;

            //Act, Assert
            Should.Throw<ArgumentNullException>(() => AdhocHelper.SetDefaultSettings(_controlSet, fieldNull));
        }

        [TestCase(AdhocHelper.ConditionContains, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.ConditionNotContains, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.Equal, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.StartsWith, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.EndsWith, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.Empty, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.NotEmpty, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.Empty, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase(AdhocHelper.NotEmpty, AdhocHelper.DisplayInlineValue, AdhocHelper.DisplayNoneValue)]
        [TestCase("", AdhocHelper.DisplayNoneValue, AdhocHelper.DisplayInlineValue)]
        public void SetDefaultSettings_ValidInput_DisplayStyleChanges(string condition, string txtAdhocSearchValueStyle,
            string divAdhocRangeStyle)
        {
            // Arrange
            _field.Values = "";
            _field.SearchCondition = condition;
            InitShimForSetSelectedTrue();

            //Act
            AdhocHelper.SetDefaultSettings(_controlSet, _field);

            //Assert
            AdhocHelper.SetDefaultSettings(_controlSet, _field);
            _controlSet.ShouldSatisfyAllConditions(
                () => _drpAdhocSearchSelectedSetCalled.ShouldBeTrue(),
                () => _controlSet.TxtAdhocSearchValue.Style[AdhocHelper.DisplayKey].ShouldBe(txtAdhocSearchValueStyle),
                () => _controlSet.DivAdhocRange.Style[AdhocHelper.DisplayKey].ShouldBe(divAdhocRangeStyle));
        }


        [TestCase(AdhocHelper.Empty)]
        [TestCase(AdhocHelper.NotEmpty)]
        public void SetDefaultSettings_ConditionEmptyorNonEmpty_TxtAdhocSearchValueHasDisabledAttribute(
            string condition)
        {
            // Arrange
            _field.Values = "";
            _field.SearchCondition = condition;
            InitShimForSetSelectedTrue();

            //Act
            AdhocHelper.SetDefaultSettings(_controlSet, _field);

            //Assert
            _controlSet.ShouldSatisfyAllConditions(
                () => _drpAdhocSearchSelectedSetCalled.ShouldBeTrue(),
                () => _controlSet.TxtAdhocSearchValue.Attributes[AdhocHelper.AttributeDisabledKey]
                    .ShouldBe(AdhocHelper.AttributeDisabledValue));
        }

        [Test]
        public void SetDefaultSettings_FieldValueHasRangeSeperator_AdhocRangeIsSet()
        {
            // Arrange
            var adhocValue0 = "a";
            var adhocValue1 = "b";
            _field.Values = $"{adhocValue0}{AdhocHelper.RangeSeperator}{adhocValue1}";
            InitShimForSetSelectedTrue();

            //Act
            AdhocHelper.SetDefaultSettings(_controlSet, _field);

            //Assert
            _controlSet.ShouldSatisfyAllConditions(
                () => _drpAdhocSearchSelectedSetCalled.ShouldBeTrue(),
                () => _controlSet.TxtAdhocRangeFrom.Text.ShouldBe(adhocValue0),
                () => _controlSet.TxtAdhocRangeTo.Text.ShouldBe(adhocValue1));
        }

        [Test]
        public void SetDefaultSettings_FieldValueHasNotRangeSeperator_TxtAdhocSearchValueIsSet()
        {
            // Arrange
            _field.Values = SampleFieldValue;
            InitShimForSetSelectedTrue();

            //Act
            AdhocHelper.SetDefaultSettings(_controlSet, _field);

            //Assert
            _controlSet.ShouldSatisfyAllConditions(
                () => _drpAdhocSearchSelectedSetCalled.ShouldBeTrue(),
                () => _controlSet.TxtAdhocSearchValue.Text.ShouldBe(_field.Values));
        }

        private void InitShimForSetSelectedTrue()
        {
            ShimAdhocHelper.SetSelectedTrueListControlString =
                (control, value) =>
                    _drpAdhocSearchSelectedSetCalled =
                        control == _controlSet.DrpAdhocSearch && value == _field.SearchCondition;
        }
    }
}