using System;
using KMPS.MD.Helpers;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    public partial class AdhocHelperTests
    {
        private const string FromDate = "2000/1/1";
        private const string ToDate = "2000/2/2";

        [Test]
        public void SetDateStyleForMonth_NullControlSet_ThrowsArgumentNull()
        {
            // Arrange
            AdhocDataListItemControlSet controlSetNull = null;
            var fromDate = string.Empty;
            var toDate = string.Empty;
            string[] dates = {fromDate, toDate};
            //Act, Assert
            Should.Throw<ArgumentNullException>(() => AdhocHelper.SetDateStyleForMonth(controlSetNull, dates));
        }

        [Test]
        public void SetDateStyleForMonth_NullDates_ThrowsArgumentNull()
        {
            // Arrange
            string[] datesNull = null;

            //Act, Assert
            Should.Throw<ArgumentNullException>(() => AdhocHelper.SetDateStyleForMonth(_controlSet, datesNull));
        }

        [Test]
        public void SetDateStyleForMonth_IncompleteDates_ThrowsIndexOutOfRange()
        {
            // Arrange
            string[] datesIncomplete = {string.Empty};

            //Act, Assert
            Should.Throw<IndexOutOfRangeException>(() =>
                AdhocHelper.SetDateStyleForMonth(_controlSet, datesIncomplete));
        }

        [Test]
        public void SetDateStyleForMonth_ValidInput_Succeeds()
        {
            //Arrange, Act
            AdhocHelper.SetDateStyleForMonth(_controlSet, new[] {FromDate, ToDate});

            //Assert
            _controlSet.ShouldSatisfyAllConditions(
                () => _controlSet.DivAdhocDateRange.Style[AdhocHelper.DisplayKey]
                    .ShouldBe(AdhocHelper.DisplayNoneValue),
                () => _controlSet.DivAdhocDateYear.Style[AdhocHelper.DisplayKey].ShouldBe(AdhocHelper.DisplayNoneValue),
                () => _controlSet.DivAdhocDateMonth.Style[AdhocHelper.DisplayKey]
                    .ShouldBe(AdhocHelper.DisplayInlineValue),
                () => _controlSet.TxtAdhocDateMonthFrom.Text.ShouldBe(FromDate),
                () => _controlSet.TxtAdhocDateMonthTo.Text.ShouldBe(ToDate));
        }
    }
}