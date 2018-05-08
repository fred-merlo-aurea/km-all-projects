using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KMPS.MD.Helpers
{
    public class AdhocDataListItemControlSet
    {
        public const string LbAdhocColumnValueId = "lbAdhocColumnValue";
        public const string DrpAdhocSearchId = "drpAdhocSearch";
        public const string TxtAdhocSearchValueId = "txtAdhocSearchValue";
        public const string TxtAdhocRangeFromId = "txtAdhocRangeFrom";
        public const string TxtAdhocRangeToId = "txtAdhocRangeTo";
        public const string LblToId = "lblTo";
        public const string DivAdhocRangeId = "divAdhocRange";
        public const string DrpDateRangeId = "drpDateRange";
        public const string DivAdhocDateRangeId = "divAdhocDateRange";
        public const string TxtAdhocDateRangeFromId = "txtAdhocDateRangeFrom";
        public const string TxtAdhocDateRangeToId = "txtAdhocDateRangeTo";
        public const string DrpAdhocDaysId = "drpAdhocDays";
        public const string DivAdhocDateYearId = "divAdhocDateYear";
        public const string TxtAdhocDateYearFromId = "txtAdhocDateYearFrom";
        public const string TxtAdhocDateYearToId = "txtAdhocDateYearTo";
        public const string DivAdhocDateMonthId = "divAdhocDateMonth";
        public const string TxtAdhocDateMonthFromId = "txtAdhocDateMonthFrom";
        public const string TxtAdhocDateMonthToId = "txtAdhocDateMonthTo";
        public const string DivCustomAdhocDaysId = "divCustomAdhocDays";
        public const string RfvCustomAdhocDaysId = "rfvCustomAdhocDays";
        public const string DrpAdhocIntId = "drpAdhocInt";
        public const string TxtAdhocIntFromId = "txtAdhocIntFrom";
        public const string TxtAdhocIntToId = "txtAdhocIntTo";
        public const string RblAdhocBitId = "rblAdhocBit";
        public const string DrpSubscribedId = "drpSubscribed";
        public const string TxtPubCountId = "txtPubCount";
        public const string LbAdhocColumnTypeId = "lbAdhocColumnType";
        public const string LblAdhocDisplayNameId = "lblAdhocDisplayName";
        public const string TxtCustomAdhocDaysId = "txtCustomAdhocDays";

        public AdhocDataListItemControlSet(DataListItem dataListItem)
        {
            LbAdhocColumnValue = dataListItem.FindControl(LbAdhocColumnValueId) as Label;
            DrpAdhocSearch = dataListItem.FindControl(DrpAdhocSearchId) as DropDownList;
            TxtAdhocSearchValue = dataListItem.FindControl(TxtAdhocSearchValueId) as TextBox;
            TxtAdhocRangeFrom = dataListItem.FindControl(TxtAdhocRangeFromId) as TextBox;
            TxtAdhocRangeTo = dataListItem.FindControl(TxtAdhocRangeToId) as TextBox;
            LblTo = dataListItem.FindControl(LblToId) as Label;
            DivAdhocRange = dataListItem.FindControl(DivAdhocRangeId) as HtmlGenericControl;
            DrpDateRange = dataListItem.FindControl(DrpDateRangeId) as DropDownList;
            DivAdhocDateRange = dataListItem.FindControl(DivAdhocDateRangeId) as HtmlGenericControl;
            TxtAdhocDateRangeFrom = dataListItem.FindControl(TxtAdhocDateRangeFromId) as TextBox;
            TxtAdhocDateRangeTo = dataListItem.FindControl(TxtAdhocDateRangeToId) as TextBox;
            DrpAdhocDays = dataListItem.FindControl(DrpAdhocDaysId) as DropDownList;
            DivAdhocDateYear = dataListItem.FindControl(DivAdhocDateYearId) as HtmlGenericControl;
            TxtAdhocDateYearFrom = dataListItem.FindControl(TxtAdhocDateYearFromId) as TextBox;
            TxtAdhocDateYearTo = dataListItem.FindControl(TxtAdhocDateYearToId) as TextBox;
            DivAdhocDateMonth = dataListItem.FindControl(DivAdhocDateMonthId) as HtmlGenericControl;
            TxtAdhocDateMonthFrom = dataListItem.FindControl(TxtAdhocDateMonthFromId) as TextBox;
            TxtAdhocDateMonthTo = dataListItem.FindControl(TxtAdhocDateMonthToId) as TextBox;
            DivCustomAdhocDays = dataListItem.FindControl(DivCustomAdhocDaysId) as HtmlGenericControl;
            RfvCustomAdhocDays = dataListItem.FindControl(RfvCustomAdhocDaysId) as RequiredFieldValidator;
            DrpAdhocInt = dataListItem.FindControl(DrpAdhocIntId) as DropDownList;
            TxtAdhocIntFrom = dataListItem.FindControl(TxtAdhocIntFromId) as TextBox;
            TxtAdhocIntTo = dataListItem.FindControl(TxtAdhocIntToId) as TextBox;
            RblAdhocBit = dataListItem.FindControl(RblAdhocBitId) as RadioButtonList;
            DrpSubscribed = dataListItem.FindControl(DrpSubscribedId) as DropDownList;
            TxtPubCount = dataListItem.FindControl(TxtPubCountId) as TextBox;
            LbAdhocColumnType = dataListItem.FindControl(LbAdhocColumnTypeId) as Label;
            LblAdhocDisplayName = dataListItem.FindControl(LblAdhocDisplayNameId) as Label;
            TxtCustomAdhocDays = dataListItem.FindControl(TxtCustomAdhocDaysId) as TextBox;
        }

        public Label LbAdhocColumnValue { get; }
        public DropDownList DrpAdhocSearch { get; }
        public TextBox TxtAdhocSearchValue { get; }
        public TextBox TxtAdhocRangeFrom { get; }
        public TextBox TxtAdhocRangeTo { get; }
        public Label LblTo { get; }
        public HtmlGenericControl DivAdhocRange { get; }
        public DropDownList DrpDateRange { get; }
        public HtmlGenericControl DivAdhocDateRange { get; }
        public TextBox TxtAdhocDateRangeFrom { get; }
        public TextBox TxtAdhocDateRangeTo { get; }
        public DropDownList DrpAdhocDays { get; }
        public HtmlGenericControl DivAdhocDateYear { get; }
        public TextBox TxtAdhocDateYearFrom { get; }
        public TextBox TxtAdhocDateYearTo { get; }
        public HtmlGenericControl DivAdhocDateMonth { get; }
        public TextBox TxtAdhocDateMonthFrom { get; }
        public TextBox TxtAdhocDateMonthTo { get; }
        public HtmlGenericControl DivCustomAdhocDays { get; }
        public RequiredFieldValidator RfvCustomAdhocDays { get; }
        public DropDownList DrpAdhocInt { get; }
        public TextBox TxtAdhocIntFrom { get; }
        public TextBox TxtAdhocIntTo { get; }
        public RadioButtonList RblAdhocBit { get; }
        public DropDownList DrpSubscribed { get; }
        public TextBox TxtPubCount { get; }
        public Label LbAdhocColumnType { get; }
        public Label LblAdhocDisplayName { get; }
        public TextBox TxtCustomAdhocDays { get; }
    }
}