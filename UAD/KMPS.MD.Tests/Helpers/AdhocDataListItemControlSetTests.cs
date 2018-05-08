using System.Diagnostics.CodeAnalysis;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KMPS.MD.Helpers;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdhocDataListItemControlSetTests
    {
        [Test]
        public void ConstructorDataListItem_EmptyControlSet_ControlsAreNull()
        {
            // Arrange, Act
            var dataListItem = new DataListItem(0, ListItemType.Item);
            var controlSet = new AdhocDataListItemControlSet(dataListItem);

            // Assert
            controlSet.ShouldSatisfyAllConditions(
                () => controlSet.LbAdhocColumnValue.ShouldBeNull(),
                () => controlSet.DrpAdhocSearch.ShouldBeNull(),
                () => controlSet.TxtAdhocSearchValue.ShouldBeNull(),
                () => controlSet.TxtAdhocRangeFrom.ShouldBeNull(),
                () => controlSet.TxtAdhocRangeTo.ShouldBeNull(),
                () => controlSet.LblTo.ShouldBeNull(),
                () => controlSet.DivAdhocRange.ShouldBeNull(),
                () => controlSet.DrpDateRange.ShouldBeNull(),
                () => controlSet.DivAdhocDateRange.ShouldBeNull(),
                () => controlSet.TxtAdhocDateRangeFrom.ShouldBeNull(),
                () => controlSet.TxtAdhocDateRangeTo.ShouldBeNull(),
                () => controlSet.DrpAdhocDays.ShouldBeNull(),
                () => controlSet.DivAdhocDateYear.ShouldBeNull(),
                () => controlSet.TxtAdhocDateYearFrom.ShouldBeNull(),
                () => controlSet.TxtAdhocDateYearTo.ShouldBeNull(),
                () => controlSet.DivAdhocDateMonth.ShouldBeNull(),
                () => controlSet.TxtAdhocDateMonthFrom.ShouldBeNull(),
                () => controlSet.TxtAdhocDateMonthTo.ShouldBeNull(),
                () => controlSet.DivCustomAdhocDays.ShouldBeNull(),
                () => controlSet.RfvCustomAdhocDays.ShouldBeNull(),
                () => controlSet.DrpAdhocInt.ShouldBeNull(),
                () => controlSet.TxtAdhocIntFrom.ShouldBeNull(),
                () => controlSet.TxtAdhocIntTo.ShouldBeNull(),
                () => controlSet.RblAdhocBit.ShouldBeNull(),
                () => controlSet.DrpSubscribed.ShouldBeNull(),
                () => controlSet.TxtPubCount.ShouldBeNull(),
                () => controlSet.LbAdhocColumnType.ShouldBeNull(),
                () => controlSet.LblAdhocDisplayName.ShouldBeNull(),
                () => controlSet.TxtCustomAdhocDays.ShouldBeNull());
        }

        [Test]
        public void ConstructorDataListItem_ControlsAreInDataListItem_ControlsAreNotNull()
        {
            // Arrange, Act
            var controlSet = CreateNonEmptyControlSet();

            // Assert
            controlSet.ShouldSatisfyAllConditions(
                () => controlSet.LbAdhocColumnValue.ShouldNotBeNull(),
                () => controlSet.DrpAdhocSearch.ShouldNotBeNull(),
                () => controlSet.TxtAdhocSearchValue.ShouldNotBeNull(),
                () => controlSet.TxtAdhocRangeFrom.ShouldNotBeNull(),
                () => controlSet.TxtAdhocRangeTo.ShouldNotBeNull(),
                () => controlSet.LblTo.ShouldNotBeNull(),
                () => controlSet.DivAdhocRange.ShouldNotBeNull(),
                () => controlSet.DrpDateRange.ShouldNotBeNull(),
                () => controlSet.DivAdhocDateRange.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateRangeFrom.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateRangeTo.ShouldNotBeNull(),
                () => controlSet.DrpAdhocDays.ShouldNotBeNull(),
                () => controlSet.DivAdhocDateYear.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateYearFrom.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateYearTo.ShouldNotBeNull(),
                () => controlSet.DivAdhocDateMonth.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateMonthFrom.ShouldNotBeNull(),
                () => controlSet.TxtAdhocDateMonthTo.ShouldNotBeNull(),
                () => controlSet.DivCustomAdhocDays.ShouldNotBeNull(),
                () => controlSet.RfvCustomAdhocDays.ShouldNotBeNull(),
                () => controlSet.DrpAdhocInt.ShouldNotBeNull(),
                () => controlSet.TxtAdhocIntFrom.ShouldNotBeNull(),
                () => controlSet.TxtAdhocIntTo.ShouldNotBeNull(),
                () => controlSet.RblAdhocBit.ShouldNotBeNull(),
                () => controlSet.DrpSubscribed.ShouldNotBeNull(),
                () => controlSet.TxtPubCount.ShouldNotBeNull(),
                () => controlSet.LbAdhocColumnType.ShouldNotBeNull(),
                () => controlSet.LblAdhocDisplayName.ShouldNotBeNull(),
                () => controlSet.TxtCustomAdhocDays.ShouldNotBeNull());
        }

        public static AdhocDataListItemControlSet CreateNonEmptyControlSet()
        {
            var dataListItem = new DataListItem(0, ListItemType.Item);

            dataListItem.Controls.Add(new Label {ID = AdhocDataListItemControlSet.LbAdhocColumnValueId});
            dataListItem.Controls.Add(new DropDownList {ID = AdhocDataListItemControlSet.DrpAdhocSearchId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocSearchValueId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocRangeFromId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocRangeToId});
            dataListItem.Controls.Add(new Label {ID = AdhocDataListItemControlSet.LblToId});
            dataListItem.Controls.Add(new HtmlGenericControl {ID = AdhocDataListItemControlSet.DivAdhocRangeId});
            dataListItem.Controls.Add(new DropDownList {ID = AdhocDataListItemControlSet.DrpDateRangeId});
            dataListItem.Controls.Add(new HtmlGenericControl {ID = AdhocDataListItemControlSet.DivAdhocDateRangeId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateRangeFromId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateRangeToId});
            dataListItem.Controls.Add(new DropDownList {ID = AdhocDataListItemControlSet.DrpAdhocDaysId});
            dataListItem.Controls.Add(new HtmlGenericControl {ID = AdhocDataListItemControlSet.DivAdhocDateYearId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateYearFromId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateYearToId});
            dataListItem.Controls.Add(new HtmlGenericControl {ID = AdhocDataListItemControlSet.DivAdhocDateMonthId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateMonthFromId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocDateMonthToId});
            dataListItem.Controls.Add(new HtmlGenericControl {ID = AdhocDataListItemControlSet.DivCustomAdhocDaysId});
            dataListItem.Controls.Add(
                new RequiredFieldValidator {ID = AdhocDataListItemControlSet.RfvCustomAdhocDaysId});
            dataListItem.Controls.Add(new DropDownList {ID = AdhocDataListItemControlSet.DrpAdhocIntId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocIntFromId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtAdhocIntToId});
            dataListItem.Controls.Add(new RadioButtonList {ID = AdhocDataListItemControlSet.RblAdhocBitId});
            dataListItem.Controls.Add(new DropDownList {ID = AdhocDataListItemControlSet.DrpSubscribedId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtPubCountId});
            dataListItem.Controls.Add(new Label {ID = AdhocDataListItemControlSet.LbAdhocColumnTypeId});
            dataListItem.Controls.Add(new Label {ID = AdhocDataListItemControlSet.LblAdhocDisplayNameId});
            dataListItem.Controls.Add(new TextBox {ID = AdhocDataListItemControlSet.TxtCustomAdhocDaysId});

            var controlSet = new AdhocDataListItemControlSet(dataListItem);
            return controlSet;
        }
    }
}