using System;
using System.Linq;
using System.Web.UI.WebControls;
using KM.Common.Extensions;
using KMPS.MD.Objects;

namespace KMPS.MD.Helpers
{
    public class AdhocHelper
    {
        public const string DisplayKey = "display";
        public const string DisplayNoneValue = "none";
        public const string DisplayInlineValue = "inline";
        public const string AttributeDisabledKey = "disabled";
        public const string AttributeDisabledValue = "disabled";
        public const char RangeSeperator = '|';

        public const string ConditionContains = "CONTAINS";
        public const string ConditionNotContains = "DOES NOT CONTAIN";
        public const string Equal = "EQUAL";
        public const string StartsWith = "START WITH";
        public const string EndsWith = "END WITH";
        public const string Empty = "IS EMPTY";
        public const string NotEmpty = "IS NOT EMPTY";

        public static void SetDefaultSettings(AdhocDataListItemControlSet controlSet, Field field)
        {
            if (controlSet == null)
            {
                throw new ArgumentNullException(nameof(controlSet));
            }

            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!string.IsNullOrWhiteSpace(field.Values) && field.Values.Contains(RangeSeperator))
            {
                var strAdhocValue = field.Values.Split(RangeSeperator);
                controlSet.TxtAdhocRangeFrom.Text = strAdhocValue[0];
                controlSet.TxtAdhocRangeTo.Text = strAdhocValue[1];
            }
            else
            {
                controlSet.TxtAdhocSearchValue.Text = field.Values;
            }

            if (!string.IsNullOrWhiteSpace(field.SearchCondition) && 
                field.SearchCondition.EqualsAnyIgnoreCase(
                    ConditionContains,
                    ConditionNotContains,
                    Equal,
                    StartsWith,
                    EndsWith,
                    Empty,
                    NotEmpty))
            {
                if (field.SearchCondition.EqualsAnyIgnoreCase(Empty, NotEmpty))
                {
                    controlSet.TxtAdhocSearchValue.Attributes.Add(AttributeDisabledKey, AttributeDisabledValue);
                }

                controlSet.TxtAdhocSearchValue.Style.Add(DisplayKey, DisplayInlineValue);
                controlSet.DivAdhocRange.Style.Add(DisplayKey, DisplayNoneValue);
            }
            else
            {
                controlSet.TxtAdhocSearchValue.Style.Add(DisplayKey, DisplayNoneValue);
                controlSet.DivAdhocRange.Style.Add(DisplayKey, DisplayInlineValue);
            }

            SetSelectedTrue(controlSet.DrpAdhocSearch, field.SearchCondition);
        }

        public static void SetSelectedTrue(ListControl listControl, string controlValue)
        {
            if (listControl == null)
            {
                throw new ArgumentNullException(nameof(listControl));
            }

            listControl.SelectedIndex = -1;
            var listItem = listControl.Items.FindByValue(controlValue);
            if (listItem != null)
            {
                listItem.Selected = true;
            }
        }

        public static void SetDateStyleForMonth(AdhocDataListItemControlSet diControls, string[] dates)
        {
            if (diControls == null)
            {
                throw new ArgumentNullException(nameof(diControls));
            }

            if (dates == null)
            {
                throw new ArgumentNullException(nameof(dates));
            }

            if (dates.Length < 2)
            {
                throw new IndexOutOfRangeException(
                    $"Length of {nameof(dates)} array must be at least 2:[0] fromDate and [1] toDate");
            }

            diControls.DivAdhocDateRange?.Style.Add(DisplayKey, DisplayNoneValue);
            diControls.DivAdhocDateYear?.Style.Add(DisplayKey, DisplayNoneValue);
            diControls.DivAdhocDateMonth?.Style.Add(DisplayKey, DisplayInlineValue);

            var fromDate = dates[0];
            var toDate = dates[1];

            if (diControls.TxtAdhocDateMonthFrom != null)
            {
                diControls.TxtAdhocDateMonthFrom.Text = fromDate;
            }

            if (diControls.TxtAdhocDateMonthTo != null)
            {
                diControls.TxtAdhocDateMonthTo.Text = toDate;
            }
        }
    }
}