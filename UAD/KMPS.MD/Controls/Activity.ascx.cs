using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using KM.Common;
using KM.Common.Extensions;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using Enums = KMPS.MD.Objects.Enums;

namespace KMPS.MD.Controls
{
    public partial class Activity : BaseControl
    {
        private const string OpenActivityFromDate = "OPENACTIVITYFROMDATE";
        private const string OpenActivityToDate = "OPENACTIVITYTODATE";
        private const string OpenEmailFromDate = "OPENEMAILFROMDATE";
        private const string OpenEmailToDate = "OPENEMAILTODATE";
        private const string ClickActivityFromDate = "CLICKACTIVITYFROMDATE";
        private const string ClickActivityToDate = "CLICKACTIVITYTODATE";
        private const string ClickEmailFromDate = "CLICKEMAILFROMDATE";
        private const string ClickEmailToDate = "CLICKEMAILTODATE";
        private const string VisitActivityFromDate = "VISITACTIVITYFROMDATE";
        private const string VisitActivityToDate = "VISITACTIVITYTODATE";
        private const string ExpToday = "EXP:Today";
        private const string ExpTodayTemplate = "EXP:Today[{0}{1}]";
        private const string PlusStr = "Plus";
        private const char SignPlus = '+';
        private const char SignMinus = '-';
        private const string StyleDisplay = "Display";
        private const string StyleDisplayValueNone = "none";
        private const string StyleDisplayValueInline = "inline";

        private const string FilterOpenCriteria = "OPEN CRITERIA";
        private const string FilterOpenActivity = "OPEN ACTIVITY";
        private const string FilterOpenBlastId = "OPEN BLASTID";
        private const string FilterOpenCampaigns = "OPEN CAMPAIGNS";
        private const string FilterOpenEmailSubject = "OPEN EMAIL SUBJECT";
        private const string FilterOpenEmailSendDate = "OPEN EMAIL SENT DATE";
        private const string FilterClickCriteria = "CLICK CRITERIA";
        private const string FilterLink = "LINK";
        private const string FilterClickActivity = "CLICK ACTIVITY";
        private const string FilterClickBlastId = "CLICK BLASTID";
        private const string FilterClickCampaigns = "CLICK CAMPAIGNS";
        private const string FilterClickEmailSubject = "CLICK EMAIL SUBJECT";
        private const string FilterClickEmailSendDate = "CLICK EMAIL SENT DATE";
        private const string FilterDomainTracking = "DOMAIN TRACKING";
        private const string FilterUrl = "URL";
        private const string FilterVisitCriteria = "VISIT CRITERIA";
        private const string FilterVisitActivity = "VISIT ACTIVITY";

        private const string SearchConditionDateRange = "DateRange";
        private const string SearchConditionXDays = "XDays";
        private const string SearchConditionYear = "YEAR";
        private const string SearchConditionMonth = "MONTH";

        private const char DelimiterPipeChar = '|';
        private const string DelimiterPipeStr = "|";
        private const string OpenCampaignValueFormat = ",{0}";
        private const string OpenCampaignTextFormat = ", {0}";

        private const int IndexFrom = 0;
        private const int IndexTo = 1;

        private const string DaysValueCustom = "Custom";
        private const string FieldNameOpenCriteria = "Open Criteria";
        private const string FieldNameOpenActivity = "Open Activity";
        private const string FieldNameOpenBlastId = "Open BlastID";
        private const string FieldNameOpenCampaigns = "Open Campaigns";
        private const string FieldNameOpenEmailSubject = "Open Email Subject";
        private const string FieldNameOpenEmailSentDate = "Open Email Sent Date";
        private const string FieldNameClickCriteria = "Click Criteria";
        private const string FieldNameClickActivity = "Click Activity";
        private const string FieldNameClickBlastId = "Click BlastID";
        private const string FieldNameClickCampaigns = "Click Campaigns";
        private const string FieldNameClickEmailSubject = "Click Email Subject";
        private const string FieldNameClickEmailSentDate = "Click Email Sent Date";
        private const string FieldNameVisitCriteria = "Visit Criteria";
        private const string FieldNameVisitActivity = "Visit Activity";
        private const string FieldNameLink = "Link";
        private const string FieldNameDomainTracking = "Domain Tracking";
        private const string FieldNameURrl= "URL";

        private const string ActivityDateRange = "DATERANGE";
        private const string ActivityXDays = "XDAYS";
        private const string ActivityDateRangeMonth = "MONTH";
        private const string ActivityDateRangeYear = "YEAR";

        private const string FieldGroupOpenActivity = "OPENACTIVITY";
        private const string FieldGroupOpenCriteria = "OPENCRITERIA";
        private const string FieldGroupOpenBlastId = "OPENBLASTID";
        private const string FieldGroupOpenCampaign = "OPENCAMPAIGNS";
        private const string FieldGroupOpenEmailSubject = "OPENEMAILSUBJECT";
        private const string FieldGroupOpenEmailSentDate = "OPENEMAILSENTDATE";
        private const string FieldGroupClickActivity = "CLICKACTIVITY";
        private const string FieldGroupClickCriteria = "CLICKCRITERIA";
        private const string FieldGroupClickBlastId = "CLICKBLASTID";
        private const string FieldGroupClickCampaign = "CLICKCAMPAIGNS";
        private const string FieldGroupClickEmailSubject = "CLICKEMAILSUBJECT";
        private const string FieldGroupClickEmailSentDate = "CLICKEMAILSENTDATE";
        private const string FieldGroupVisitCriteria = "VISITCRITERIA";
        private const string FieldGroupVisitActivity = "VISITACTIVITY";
        private const string FieldGroupLink = "LINK";
        private const string FieldGroupDomainTracking = "DOMAINTRACKING";
        private const string FieldGroupUrl = "URL";
        private const string ValueDateRange = "DateRange";
        private const string ValueXDays = "XDays";
        private const string ValueYear = "Year";
        private const string ValueMonth = "Month";
        private const string ValueZero = "0";

        private static readonly IReadOnlyList<string> Periods = 
            new List<string>(new[] { "7", "14", "21", "30", "60", "90", "120", "150", "6mon", "1yr" });

        public Delegate hideActivityPopup;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mdlActivitys.Show();
            divError.Visible = false;
            lblErrorMessage.Text = "";

            drpOpenActivity.Attributes.Add("onChange", "drpopenactivity_onchange('" + this.ClientID + "_');");
            drpClickActivity.Attributes.Add("onChange", "drpclickactivity_onchange('" + this.ClientID + "_');");
            drpVisitActivity.Attributes.Add("onChange", "drpvisitactivity_onchange('" + this.ClientID + "_');");

            if (!IsPostBack)
            {
                drpDomain.DataSource = DomainTracking.Get(clientconnections);
                drpDomain.DataBind();
                drpDomain.Items.Insert(0, new ListItem("", ""));

                List<ECNCampaign> ec = ECNCampaign.GetAll(clientconnections);

                RadCBOpenCampaigns.DataSource = ec;
                RadCBOpenCampaigns.DataBind();

                RadCBClickCampaigns.DataSource = ec;
                RadCBClickCampaigns.DataBind();
            }

            //FillLookup();
        }

        public void LoadControls()
        {
            LoadControlsForOpenActivity();
            LoadControlsForClickActivity();
            LoadControlsForVisitActivity();
        }

        private void LoadControlsForOpenActivity()
        {
            divOpenDate.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenActivityDateRange.Style.Add(StyleDisplay, StyleDisplayValueNone);
            drpOpenActivityDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenActivityYear.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenActivityMonth.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenEmail.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenEmailDateRange.Style.Add(StyleDisplay, StyleDisplayValueNone);
            drpOpenEmailDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenEmailYear.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOpenEmailMonth.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divCustomOpenActivityDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divCustomOpenEmailDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            rfvCustomOpenActivityDays.Enabled = false;
            rfvCustomOpenEmailDays.Enabled = false;

            if (drpOpenActivity.SelectedValue != string.Empty)
            {
                LoadDateControls(
                    divOpenDate,
                    drpOpenActivityDateRange,
                    divOpenActivityDateRange,
                    drpOpenActivityDays,
                    divCustomOpenActivityDays,
                    divOpenActivityYear,
                    divOpenActivityMonth,
                    rfvCustomOpenActivityDays);

                if (drpOpenActivity.SelectedValue != ValueZero)
                {
                    LoadDateControls(
                        divOpenEmail,
                        drpOpenEmailDateRange,
                        divOpenEmailDateRange,
                        drpOpenEmailDays,
                        divCustomOpenEmailDays,
                        divOpenEmailYear,
                        divOpenEmailMonth,
                        rfvCustomOpenEmailDays);
                }
            }
        }

        private void LoadControlsForClickActivity()
        {
            rfvCustomClickActivityDays.Enabled = false;
            rfvCustomClickEmailDays.Enabled = false;
            divClickDate.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickActivityDateRange.Style.Add(StyleDisplay, StyleDisplayValueNone);
            drpClickActivityDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickActivityYear.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickActivityMonth.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickEmail.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickEmailDateRange.Style.Add(StyleDisplay, StyleDisplayValueNone);
            drpClickEmailDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickEmailYear.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divClickEmailMonth.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divCustomClickActivityDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divCustomClickEmailDays.Style.Add(StyleDisplay, StyleDisplayValueNone);

            if (drpClickActivity.SelectedValue != string.Empty)
            {
                LoadDateControls(
                    divClickDate,
                    drpClickActivityDateRange,
                    divClickActivityDateRange,
                    drpClickActivityDays,
                    divCustomClickActivityDays,
                    divClickActivityYear,
                    divClickActivityMonth,
                    rfvCustomClickActivityDays);

                if (drpClickActivity.SelectedValue != ValueZero)
                {
                    LoadDateControls(
                        divClickEmail,
                        drpClickEmailDateRange,
                        divClickEmailDateRange,
                        drpClickEmailDays,
                        divCustomClickEmailDays,
                        divClickEmailYear,
                        divClickEmailMonth,
                        rfvCustomClickEmailDays);
                }
            }
        }

        private void LoadControlsForVisitActivity()
        {
            rfvCustomVisitActivityDays.Enabled = false;
            divVisitDate.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divVisitActivityDateRange.Style.Add(StyleDisplay, StyleDisplayValueNone);
            drpVisitActivityDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divVisitActivityYear.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divVisitActivityMonth.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divVisitDomain.Style.Add(StyleDisplay, StyleDisplayValueNone);

            if (drpVisitActivity.SelectedValue != string.Empty)
            {
                LoadDateControls(
                    divVisitDate,
                    drpVisitActivityDateRange,
                    divVisitActivityDateRange,
                    drpVisitActivityDays,
                    divCustomVisitActivityDays,
                    divVisitActivityYear,
                    divVisitActivityMonth,
                    rfvCustomVisitActivityDays);

                if (drpVisitActivity.SelectedValue != ValueZero)
                {
                    divVisitDomain.Style.Add(StyleDisplay, StyleDisplayValueInline);
                }
            }
        }

        private static void LoadDateControls(
            HtmlGenericControl divDate,
            DropDownList dropDownDateRange,
            HtmlGenericControl divDateRange,
            DropDownList dropDownDays,
            HtmlGenericControl divCustomDays,
            HtmlGenericControl divYear,
            HtmlGenericControl divMonth,
            RequiredFieldValidator customDaysValidator)
        {
            divDate.Style.Add(StyleDisplay, StyleDisplayValueInline);

            if (dropDownDateRange.SelectedValue == ValueDateRange)
            {
                divDateRange.Style.Add(StyleDisplay, StyleDisplayValueInline);
            }
            else if (dropDownDateRange.SelectedValue == ValueXDays)
            {
                dropDownDays.Style.Add(StyleDisplay, StyleDisplayValueInline);

                if (dropDownDays.SelectedValue.EqualsIgnoreCase(DaysValueCustom))
                {
                    divCustomDays.Style.Add(StyleDisplay, StyleDisplayValueInline);
                    customDaysValidator.Enabled = true;
                }
                else
                {
                    divCustomDays.Style.Add(StyleDisplay, StyleDisplayValueNone);
                    customDaysValidator.Enabled = false;
                }
            }
            else if (dropDownDateRange.SelectedValue == ValueYear)
            {
                divYear.Style.Add(StyleDisplay, StyleDisplayValueInline);
            }
            else if (dropDownDateRange.SelectedValue == ValueMonth)
            {
                divMonth.Style.Add(StyleDisplay, StyleDisplayValueInline);
            }
        }

        public List<Field> GetActivityFilters()
        {
            var fields = new List<Field>();

            FillOpenActivityFilter(fields);
            FillClickActivityFilter(fields);
            FillVisitActivityFilter(fields);

            return fields;
        }

        private void FillOpenActivityFilter(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(drpOpenActivity.SelectedItem.Value))
            {
                fields.Add(
                    new Field(
                        FieldNameOpenCriteria, 
                        drpOpenActivity.SelectedItem.Value, 
                        drpOpenActivity.SelectedItem.Text,
                        rblOpenSearchType.SelectedValue, 
                        Enums.FiltersType.Activity,
                        FieldGroupOpenCriteria));

                if (!string.IsNullOrWhiteSpace(drpOpenActivityDateRange.SelectedValue))
                {
                    FillOpenActivityFilterActivityDateRange(fields);
                }

                if (!string.IsNullOrWhiteSpace(txtOpenBlastID.Text))
                {
                    fields.Add(
                        new Field(
                            FieldNameOpenBlastId,
                            txtOpenBlastID.Text,
                            txtOpenBlastID.Text,
                            string.Empty,
                            Enums.FiltersType.Activity,
                            FieldGroupOpenBlastId));
                }

                if (RadCBOpenCampaigns.CheckedItems.Count > 0)
                {
                    FillOpenActivityFilterCampaigns(fields);
                }

                if (!string.IsNullOrWhiteSpace(txtOpenEmailSubject.Text))
                {
                    fields.Add(
                        new Field(
                            FieldNameOpenEmailSubject, 
                            txtOpenEmailSubject.Text, 
                            txtOpenEmailSubject.Text, 
                            string.Empty,
                            Enums.FiltersType.Activity,
                            FieldGroupOpenEmailSubject));
                }

                if (!string.IsNullOrWhiteSpace(drpOpenEmailDateRange.SelectedValue))
                {
                    FillOpenActivityFilterEmailDateRange(fields);
                }
            }
        }

        private void FillOpenActivityFilterActivityDateRange(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpOpenActivityDateRange.SelectedValue.Equals(ActivityDateRange, StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterActivityDateRangeActiityDateRange(fields);
            }
            else if (
                drpOpenActivityDateRange.SelectedValue.Equals(
                    ActivityXDays,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterActivityDateRangeXDays(fields);
            }
            else if (
                drpOpenActivityDateRange.SelectedValue.Equals(
                    ActivityDateRangeMonth,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterActivityDateRangeMonth(fields);
            }
            else if (
                drpOpenActivityDateRange.SelectedValue.Equals(
                    SearchConditionYear,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterActivityDateRangeYear(fields);
            }
        }

        private void FillOpenActivityFilterActivityDateRangeActiityDateRange(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (txtOpenActivityFrom.Text != string.Empty || txtOpenActivityTo.Text != string.Empty)
            {
                var searchTxt = string.Join(
                    DelimiterPipeStr, txtOpenActivityFrom.Text,
                    txtOpenActivityTo.Text);
                fields.Add(
                    new Field(
                        FieldNameOpenActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionDateRange,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenActivity));
            }
        }

        private void FillOpenActivityFilterActivityDateRangeXDays(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpOpenActivityDays.SelectedValue.Equals(
                DaysValueCustom,
                StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(
                    new Field(
                        FieldNameOpenActivity,
                        txtCustomOpenActivityDays.Text,
                        txtCustomOpenActivityDays.Text,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenActivity));
            }
            else
            {
                fields.Add(
                    new Field(
                        FieldNameOpenActivity,
                        drpOpenActivityDays.SelectedValue,
                        drpOpenActivityDays.SelectedValue,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenActivity));
            }
        }

        private void FillOpenActivityFilterActivityDateRangeMonth(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtOpenActivityFromMonth.Text) ||
                !string.IsNullOrWhiteSpace(txtOpenActivityToMonth.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtOpenActivityFromMonth.Text,
                        txtOpenActivityToMonth.Text);
                fields.Add(
                    new Field(
                        FieldNameOpenActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionMonth,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenActivity));
            }
        }

        private void FillOpenActivityFilterActivityDateRangeYear(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtOpenActivityFromYear.Text) ||
                !string.IsNullOrWhiteSpace(txtOpenActivityToYear.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtOpenActivityFromYear.Text,
                        txtOpenActivityToYear.Text);
                fields.Add(new Field(
                    FieldNameOpenActivity,
                    searchTxt,
                    searchTxt,
                    SearchConditionYear,
                    Enums.FiltersType.Activity,
                    FieldGroupOpenActivity));
            }
        }

        private void FillOpenActivityFilterCampaigns(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            var sbOpenCampaignsValue = new StringBuilder();
            var sbOpenCampaignsText = new StringBuilder();

            foreach (var item in RadCBOpenCampaigns.CheckedItems)
            {
                if (sbOpenCampaignsValue.ToString() == string.Empty)
                {
                    sbOpenCampaignsValue.Append(item.Value);
                    sbOpenCampaignsText.Append(item.Text);
                }
                else
                {
                    sbOpenCampaignsValue.AppendFormat(OpenCampaignValueFormat, item.Value);
                    sbOpenCampaignsText.AppendFormat(OpenCampaignTextFormat, item.Text);
                }
            }

            fields.Add(
                new Field(
                    FieldNameOpenCampaigns,
                    sbOpenCampaignsValue.ToString(),
                    sbOpenCampaignsText.ToString(),
                    string.Empty,
                    Enums.FiltersType.Activity,
                    FieldGroupOpenCampaign));
        }

        private void FillOpenActivityFilterEmailDateRange(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpOpenEmailDateRange.SelectedValue.Equals(ActivityDateRange, StringComparison.OrdinalIgnoreCase))
            {
                if (txtOpenEmailFromDate.Text != string.Empty || txtOpenEmailToDate.Text != string.Empty)
                {
                    FillOpenActivityFilterEmailDateRangeEmailFromTo(fields);
                }
            }
            else if (drpOpenEmailDateRange.SelectedValue.Equals(ActivityXDays, StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterEmailDateRangeActivityXDays(fields);
            }
            else if (drpOpenEmailDateRange.SelectedValue.Equals(ActivityDateRangeMonth, StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterEmailDateRangeEmailDateRange(fields);
            }
            else if (drpOpenEmailDateRange.SelectedValue.Equals(ActivityDateRangeYear, StringComparison.OrdinalIgnoreCase))
            {
                FillOpenActivityFilterEmailDateRangeYear(fields);
            }
        }

        private void FillOpenActivityFilterEmailDateRangeYear(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtOpenEmailFromYear.Text) ||
                !string.IsNullOrWhiteSpace(txtOpenEmailToYear.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtOpenEmailFromYear.Text,
                        txtOpenEmailToYear.Text);
                fields.Add(
                    new Field(
                        FieldNameOpenEmailSentDate,
                        searchTxt,
                        searchTxt,
                        SearchConditionYear,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenEmailSentDate));
            }
        }

        private void FillOpenActivityFilterEmailDateRangeEmailDateRange(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtOpenEmailFromMonth.Text) ||
                !string.IsNullOrWhiteSpace(txtOpenEmailToMonth.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtOpenEmailFromMonth.Text,
                        txtOpenEmailToMonth.Text);
                fields.Add(
                    new Field(
                        FieldNameOpenEmailSentDate,
                        searchTxt,
                        searchTxt,
                        SearchConditionMonth,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenEmailSentDate));
            }
        }

        private void FillOpenActivityFilterEmailDateRangeActivityXDays(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpOpenEmailDays.SelectedValue.Equals(DaysValueCustom, StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(
                    new Field(
                        FieldNameOpenEmailSentDate,
                        txtCustomOpenEmailDays.Text,
                        txtCustomOpenEmailDays.Text,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenEmailSentDate));
            }
            else
            {
                fields.Add(
                    new Field(
                        FieldNameOpenEmailSentDate,
                        drpOpenEmailDays.SelectedValue,
                        drpOpenEmailDays.SelectedValue,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupOpenEmailSentDate));
            }
        }

        private void FillOpenActivityFilterEmailDateRangeEmailFromTo(ICollection<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            var searchTxt =
                string.Join(
                    DelimiterPipeStr,
                    txtOpenEmailFromDate.Text,
                    txtOpenEmailToDate.Text);
            fields.Add(
                new Field(
                    FieldNameOpenEmailSentDate,
                    searchTxt,
                    searchTxt,
                    SearchConditionDateRange,
                    Enums.FiltersType.Activity,
                    FieldGroupOpenEmailSentDate));
        }

        private void FillClickActivityFilter(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(drpClickActivity.SelectedItem.Value))
            {
                fields.Add(
                    new Field(
                        FieldNameClickCriteria, 
                        drpClickActivity.SelectedItem.Value, 
                        drpClickActivity.SelectedItem.Text,
                        rblClickSearchType.SelectedValue, 
                        Enums.FiltersType.Activity,
                        FieldGroupClickCriteria));

                if (txtLink.Text.Length > 0)
                {
                    fields.Add(
                        new Field(
                            FieldNameLink, 
                            txtLink.Text, 
                            txtLink.Text, 
                            string.Empty, 
                            Enums.FiltersType.Activity,
                            FieldGroupLink));
                }

                if (!string.IsNullOrWhiteSpace(drpClickActivityDateRange.SelectedValue))
                {
                    FillClickActivityFilterDateRange(fields);
                }

                if (!string.IsNullOrWhiteSpace(txtClickBlastID.Text))
                {
                    fields.Add(
                        new Field(
                            FieldNameClickBlastId, 
                            txtClickBlastID.Text, 
                            txtClickBlastID.Text, 
                            String.Empty, 
                            Enums.FiltersType.Activity,
                            FieldGroupClickBlastId));
                }

                if (RadCBClickCampaigns.CheckedItems.Count > 0)
                {
                    FillClickActivityFilterCampaigns(fields);
                }

                FillClickActivityFilterClickEmailSubject(fields);

                if (!string.IsNullOrWhiteSpace(drpClickEmailDateRange.SelectedValue))
                {
                    FillClickActivityFilterEmailDateRange(fields);
                }
            }
        }

        private void FillClickActivityFilterClickEmailSubject(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtClickEmailSubject.Text))
            {
                fields.Add(
                    new Field(
                        FieldNameClickEmailSubject,
                        txtClickEmailSubject.Text,
                        txtClickEmailSubject.Text,
                        string.Empty,
                        Enums.FiltersType.Activity,
                        FieldGroupClickEmailSubject));
            }
        }

        private void FillClickActivityFilterCampaigns(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            var sbClickCampaignsValue = new StringBuilder();
            var sbClickCampaignsText = new StringBuilder();

            foreach (var item in RadCBClickCampaigns.CheckedItems)
            {
                if (string.IsNullOrWhiteSpace(sbClickCampaignsValue.ToString()))
                {
                    sbClickCampaignsValue.Append(item.Value);
                    sbClickCampaignsText.Append(item.Text);
                }
                else
                {
                    sbClickCampaignsValue.AppendFormat(OpenCampaignValueFormat, item.Value);
                    sbClickCampaignsText.AppendFormat(OpenCampaignTextFormat, item.Text);
                }
            }

            fields.Add(
                new Field(
                    FieldNameClickCampaigns,
                    sbClickCampaignsValue.ToString(),
                    sbClickCampaignsText.ToString(),
                    string.Empty,
                    Enums.FiltersType.Activity,
                    FieldGroupClickCampaign));
        }

        private void FillClickActivityFilterEmailDateRange(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpClickEmailDateRange.SelectedValue.Equals(
                ActivityDateRange, 
                StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(txtClickEmailFromDate.Text) ||
                    !string.IsNullOrWhiteSpace(txtClickEmailToDate.Text))
                {
                    var searchTxt =
                        string.Join(
                            DelimiterPipeStr,
                            txtClickEmailFromDate.Text,
                            txtClickEmailToDate.Text);
                    fields.Add(
                        new Field(
                            FieldNameClickEmailSentDate,
                            searchTxt,
                            searchTxt,
                            SearchConditionDateRange,
                            Enums.FiltersType.Activity,
                            FieldGroupClickEmailSentDate));
                }
            }
            else if (drpClickEmailDateRange.SelectedValue.Equals(
                ActivityXDays, 
                StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterEmailDateRangeActiveXDays(fields);
            }
            else if (drpClickEmailDateRange.SelectedValue.Equals(
                ActivityDateRangeMonth, 
                StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterEmailDateRangeMonth(fields);
            }
            else if (drpClickEmailDateRange.SelectedValue.Equals(
                SearchConditionYear, 
                StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterEmailDateRangeYear(fields);
            }
        }

        private void FillClickActivityFilterEmailDateRangeMonth(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtClickEmailFromMonth.Text) ||
                !string.IsNullOrWhiteSpace(txtClickEmailToMonth.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtClickEmailFromMonth.Text,
                        txtClickEmailToMonth.Text);
                fields.Add(
                    new Field(
                        FieldNameClickEmailSentDate,
                        searchTxt,
                        searchTxt,
                        SearchConditionMonth,
                        Enums.FiltersType.Activity,
                        FieldGroupClickEmailSentDate));
            }
        }

        private void FillClickActivityFilterEmailDateRangeYear(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtClickEmailFromYear.Text) ||
                !string.IsNullOrWhiteSpace(txtClickEmailToYear.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtClickEmailFromYear.Text,
                        txtClickEmailToYear.Text);
                fields.Add(
                    new Field(
                        FieldNameClickEmailSentDate,
                        searchTxt,
                        searchTxt,
                        SearchConditionYear,
                        Enums.FiltersType.Activity,
                        FieldGroupClickEmailSentDate));
            }
        }

        private void FillClickActivityFilterEmailDateRangeActiveXDays(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpClickEmailDays.SelectedValue.Equals(DaysValueCustom, StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(
                    new Field(
                        FieldNameClickEmailSentDate,
                        txtCustomClickEmailDays.Text,
                        txtCustomClickEmailDays.Text,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupClickEmailSentDate));
            }
            else
            {
                fields.Add(
                    new Field(
                        FieldNameClickEmailSentDate,
                        drpClickEmailDays.SelectedValue,
                        drpClickEmailDays.SelectedValue,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupClickEmailSentDate));
            }
        }

        private void FillClickActivityFilterDateRange(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpClickActivityDateRange.SelectedValue.Equals(ActivityDateRange, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(txtClickActivityFrom.Text) ||
                    !string.IsNullOrWhiteSpace(txtClickActivityTo.Text))
                {
                    var searchTxt =
                        string.Join(
                            DelimiterPipeStr,
                            txtClickActivityFrom.Text,
                            txtClickActivityTo.Text);
                    fields.Add(
                        new Field(
                            FieldNameClickActivity,
                            searchTxt,
                            searchTxt,
                            SearchConditionDateRange,
                            Enums.FiltersType.Activity,
                            FieldGroupClickActivity));
                }
            }
            else if (drpClickActivityDateRange.SelectedValue.Equals(ActivityXDays, StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterDateRangeDaysCustom(fields);
            }
            else if (
                drpClickActivityDateRange.SelectedValue.Equals(
                    ActivityDateRangeMonth,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterDateRangeMonth(fields);
            }
            else if (
                drpClickActivityDateRange.SelectedValue.Equals(
                    ActivityDateRangeYear,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillClickActivityFilterDateRangeYear(fields);
            }
        }

        private void FillClickActivityFilterDateRangeMonth(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtClickActivityFromMonth.Text) ||
                !string.IsNullOrWhiteSpace(txtClickActivityToMonth.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtClickActivityFromMonth.Text,
                        txtClickActivityToMonth.Text);
                fields.Add(
                    new Field(
                        FieldNameClickActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionMonth,
                        Enums.FiltersType.Activity,
                        FieldGroupClickActivity));
            }
        }

        private void FillClickActivityFilterDateRangeDaysCustom(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpClickActivityDays.SelectedValue.Equals(DaysValueCustom, StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(
                    new Field(
                        FieldNameClickActivity,
                        txtCustomClickActivityDays.Text,
                        txtCustomClickActivityDays.Text,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupClickActivity));
            }
            else
            {
                fields.Add(
                    new Field(
                        FieldNameClickActivity,
                        drpClickActivityDays.SelectedValue,
                        drpClickActivityDays.SelectedValue,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupClickActivity));
            }
        }

        private void FillClickActivityFilterDateRangeYear(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (txtClickActivityFromYear.Text != string.Empty ||
                txtClickActivityToYear.Text != string.Empty)
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtClickActivityFromYear.Text,
                        txtClickActivityToYear.Text);
                fields.Add(
                    new Field(
                        FieldNameClickActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionYear,
                        Enums.FiltersType.Activity,
                        FieldGroupClickActivity));
            }
        }

        private void FillVisitActivityFilter(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(drpVisitActivity.SelectedValue))
            {
                fields.Add(
                    new Field(
                        FieldNameVisitCriteria, 
                        drpVisitActivity.SelectedItem.Value, 
                        drpVisitActivity.SelectedItem.Text,
                        string.Empty, 
                        Enums.FiltersType.Activity,
                        FieldGroupVisitCriteria));

                if (!string.IsNullOrWhiteSpace(drpDomain.SelectedItem.Value))
                {
                    fields.Add(
                        new Field(
                            FieldNameDomainTracking, 
                            drpDomain.SelectedItem.Value, 
                            drpDomain.SelectedItem.Text,
                            string.Empty,
                            Enums.FiltersType.Activity,
                            FieldGroupDomainTracking));
                }

                if (txtURL.Text.Length > 0)
                {
                    fields.Add(
                        new Field(
                            FieldNameURrl, 
                            txtURL.Text, 
                            txtURL.Text, 
                            string.Empty, 
                            Enums.FiltersType.Activity,
                            FieldGroupUrl));
                }

                if (!string.IsNullOrWhiteSpace(drpVisitActivityDateRange.SelectedValue))
                {
                    FillVisitActivityFilterDateRange(fields);
                }
            }
        }

        private void FillVisitActivityFilterDateRange(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpVisitActivityDateRange.SelectedValue.Equals(ActivityDateRange, StringComparison.OrdinalIgnoreCase))
            {
                FillVisitActivityFilterDateRangeDateRange(fields);
            }
            else if (drpVisitActivityDateRange.SelectedValue.Equals(ActivityXDays, StringComparison.OrdinalIgnoreCase))
            {
                FillVisitActivityFilterDateRangeXDays(fields);
            }
            else if (
                drpVisitActivityDateRange.SelectedValue.Equals(
                    ActivityDateRangeMonth,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillVisitActivityFilterDateRangeMonth(fields);
            }
            else if (
                drpVisitActivityDateRange.SelectedValue.Equals(
                    SearchConditionYear,
                    StringComparison.OrdinalIgnoreCase))
            {
                FillVisitActivityFilterDateRangeYear(fields);
            }
        }

        private void FillVisitActivityFilterDateRangeYear(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtVisitActivityFromYear.Text) ||
                !string.IsNullOrWhiteSpace(txtVisitActivityToYear.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtVisitActivityFromYear.Text,
                        txtVisitActivityToYear.Text);
                fields.Add(
                    new Field(
                        FieldNameVisitActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionYear,
                        Enums.FiltersType.Activity,
                        FieldGroupVisitActivity));
            }
        }

        private void FillVisitActivityFilterDateRangeMonth(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtVisitActivityFromMonth.Text) ||
                !string.IsNullOrWhiteSpace(txtVisitActivityToMonth.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtVisitActivityFromMonth.Text,
                        txtVisitActivityToMonth.Text);
                fields.Add(
                    new Field(
                        FieldNameVisitActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionMonth,
                        Enums.FiltersType.Activity,
                        FieldGroupVisitActivity));
            }
        }

        private void FillVisitActivityFilterDateRangeXDays(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (drpVisitActivityDays.SelectedValue.Equals(DaysValueCustom, StringComparison.OrdinalIgnoreCase))
            {
                fields.Add(
                    new Field(
                        FieldNameVisitActivity,
                        txtCustomVisitActivityDays.Text,
                        txtCustomVisitActivityDays.Text,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupVisitActivity));
            }
            else
            {
                fields.Add(
                    new Field(
                        FieldNameVisitActivity,
                        drpVisitActivityDays.SelectedValue,
                        drpVisitActivityDays.SelectedValue,
                        SearchConditionXDays,
                        Enums.FiltersType.Activity,
                        FieldGroupVisitActivity));
            }
        }

        private void FillVisitActivityFilterDateRangeDateRange(List<Field> fields)
        {
            Guard.NotNull(fields, nameof(fields));

            if (!string.IsNullOrWhiteSpace(txtVisitActivityFrom.Text) ||
                !string.IsNullOrWhiteSpace(txtVisitActivityTo.Text))
            {
                var searchTxt =
                    string.Join(
                        DelimiterPipeStr,
                        txtVisitActivityFrom.Text,
                        txtVisitActivityTo.Text);
                fields.Add(
                    new Field(
                        FieldNameVisitActivity,
                        searchTxt,
                        searchTxt,
                        SearchConditionDateRange,
                        Enums.FiltersType.Activity,
                        FieldGroupVisitActivity));
            }
        }

        public void LoadActivityFilters(Field field)
        {
            Guard.NotNull(field, nameof(field));

            switch (field.Name.ToUpper())
            {
                case FilterOpenCriteria:
                    rblOpenSearchType.SelectedValue = field.SearchCondition;
                    Utilities.SelectFilterDropDowns(drpOpenActivity, field.Values);
                    break;
                case FilterOpenActivity:
                    LoadFilterOpenActivity(field);
                    break;
                case FilterOpenBlastId:
                    txtOpenBlastID.Text = field.Values;
                    break;
                case FilterOpenCampaigns:
                    LoadFilterSetCampaignCombo(field, RadCBOpenCampaigns);
                    break;
                case FilterOpenEmailSubject:
                    txtOpenEmailSubject.Text = field.Values;
                    break;
                case FilterOpenEmailSendDate:
                    LoadFilterOpenEmailSendDate(field);
                    break;
                case FilterClickCriteria:
                    rblClickSearchType.SelectedValue = field.SearchCondition;
                    Utilities.SelectFilterDropDowns(drpClickActivity, field.Values);
                    break;
                case FilterLink:
                    Utilities.SelectFilterTextBox(txtLink, field.Values);
                    break;
                case FilterClickActivity:
                    LoadFilterClickActivity(field);
                    break;
                case FilterClickBlastId:
                    txtClickBlastID.Text = field.Values;
                    break;
                case FilterClickCampaigns:
                    LoadFilterSetCampaignCombo(field, RadCBClickCampaigns);
                    break;
                case FilterClickEmailSubject:
                    txtClickEmailSubject.Text = field.Values;
                    break;
                case FilterClickEmailSendDate:
                    LoadFilterClickEmailSendDate(field);
                    break;
                case FilterDomainTracking:
                    Utilities.SelectFilterDropDowns(drpDomain, field.Values);
                    break;
                case FilterUrl:
                    Utilities.SelectFilterTextBox(txtURL, field.Values);
                    break;
                case FilterVisitCriteria:
                    Utilities.SelectFilterDropDowns(drpVisitActivity, field.Values);
                    break;
                case FilterVisitActivity:
                    LoadFilterVisitActivity(field);
                    break;
            }
        }

        private static void LoadFilterSetCampaignCombo(Field field, RadComboBox cmbBox)
        {
            Guard.NotNull(field, nameof(field));
            Guard.NotNull(cmbBox, nameof(cmbBox));

            if (!string.IsNullOrWhiteSpace(field.Values))
            {
                var items = field.Values.Split(',');
                foreach (RadComboBoxItem item in cmbBox.Items)
                {
                    item.Selected = items.Any(c => c.Equals(item.Value));
                    item.Checked = item.Selected;
                }
            }
        }

        private void LoadFilterVisitActivity(Field field)
        {
            Guard.NotNull(field, nameof(field));

            var filterVisitActivityArgs = new LoadActivityFilterArgs
            {
                Field = field,
                DrpDateRange = drpVisitActivityDateRange,
                DivDateRange = divVisitActivityDateRange,
                TxtFrom = txtVisitActivityFrom,
                TxtTo = txtVisitActivityTo,
                DrpDays = drpVisitActivityDays,
                TxtActivityDays = txtCustomVisitActivityDays,
                DivYear = divVisitActivityYear,
                TxtFromYear = txtVisitActivityFromYear,
                TxtToYear = txtVisitActivityToYear,
                DivMonth = divVisitActivityMonth,
                TxtFromMonth = txtVisitActivityFromMonth,
                TxtoMonth = txtVisitActivityToMonth
            };
            LoadActivityFilters(filterVisitActivityArgs);
        }

        private void LoadFilterClickEmailSendDate(Field field)
        {
            Guard.NotNull(field, nameof(field));

            var filterClickEmailArgs = new LoadActivityFilterArgs
            {
                Field = field,
                DrpDateRange = drpClickEmailDateRange,
                DivDateRange = divClickEmailDateRange,
                TxtFrom = txtClickEmailFromDate,
                TxtTo = txtClickEmailToDate,
                DrpDays = drpClickEmailDays,
                TxtActivityDays = txtCustomClickEmailDays,
                DivYear = divClickEmailYear,
                TxtFromYear = txtClickEmailFromYear,
                TxtToYear = txtClickEmailToYear,
                DivMonth = divClickEmailMonth,
                TxtFromMonth = txtClickEmailFromMonth,
                TxtoMonth = txtClickEmailToMonth
            };
            LoadActivityFilters(filterClickEmailArgs);
        }

        private void LoadFilterClickActivity(Field field)
        {
            Guard.NotNull(field, nameof(field));

            var filterClickActivityArgs = new LoadActivityFilterArgs
            {
                Field = field,
                DrpDateRange = drpClickActivityDateRange,
                DivDateRange = divClickActivityDateRange,
                TxtFrom = txtClickActivityFrom,
                TxtTo = txtClickActivityTo,
                DrpDays = drpClickActivityDays,
                TxtActivityDays = txtCustomClickActivityDays,
                DivYear = divClickActivityYear,
                TxtFromYear = txtClickActivityFromYear,
                TxtToYear = txtClickActivityToYear,
                DivMonth = divClickActivityMonth,
                TxtFromMonth = txtClickActivityFromMonth,
                TxtoMonth = txtClickActivityToMonth
            };
            LoadActivityFilters(filterClickActivityArgs);
        }

        private void LoadFilterOpenEmailSendDate(Field field)
        {
            Guard.NotNull(field, nameof(field));

            var filterOpenEmailSendDateArgs = new LoadActivityFilterArgs
            {
                Field = field,
                DrpDateRange = drpOpenEmailDateRange,
                DivDateRange = divOpenEmailDateRange,
                TxtFrom = txtOpenEmailFromDate,
                TxtTo = txtOpenEmailToDate,
                DrpDays = drpOpenEmailDays,
                TxtActivityDays = txtCustomOpenEmailDays,
                DivYear = divOpenEmailYear,
                TxtFromYear = txtOpenEmailFromYear,
                TxtToYear = txtOpenEmailToYear,
                DivMonth = divOpenEmailMonth,
                TxtFromMonth = txtOpenEmailFromMonth,
                TxtoMonth = txtOpenEmailToMonth
            };
            LoadActivityFilters(filterOpenEmailSendDateArgs);
        }

        private void LoadFilterOpenActivity(Field field)
        {
            Guard.NotNull(field, nameof(field));

            var filterOpenActivityArgs = new LoadActivityFilterArgs
            {
                Field = field,
                DrpDateRange = drpOpenActivityDateRange,
                DivDateRange = divOpenActivityDateRange,
                TxtFrom = txtOpenActivityFrom,
                TxtTo = txtOpenActivityTo,
                DrpDays = drpOpenActivityDays,
                TxtActivityDays = txtCustomOpenActivityDays,
                DivYear = divOpenActivityYear,
                TxtFromYear = txtOpenActivityFromYear,
                TxtToYear = txtOpenActivityToYear,
                DivMonth = divOpenActivityMonth,
                TxtFromMonth = txtOpenActivityFromMonth,
                TxtoMonth = txtOpenActivityToMonth
            };
            LoadActivityFilters(filterOpenActivityArgs);
        }

        private static void LoadActivityFilters(LoadActivityFilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            args.EnsureNotNull();

            string[] strvalues;

            args.DrpDateRange.SelectedValue = args.Field.SearchCondition;

            if (args.Field.SearchCondition.Equals(SearchConditionDateRange, StringComparison.OrdinalIgnoreCase))
            {
                args.DivDateRange.Visible = true;
                args.DrpDateRange.Enabled = true;
                strvalues = args.Field.Values.Split(DelimiterPipeChar);
                args.TxtFrom.Text = strvalues[IndexFrom];
                args.TxtTo.Text = strvalues[IndexTo];
            }
            else
            {
                
                if (args.Field.SearchCondition.Equals(SearchConditionXDays, StringComparison.OrdinalIgnoreCase))
                {
                    if (Periods.Contains(args.Field.Values))
                    {
                        args.DrpDays.SelectedValue = args.Field.Values;
                    }
                    else
                    {
                        args.DrpDays.SelectedValue = DaysValueCustom;
                        args.TxtActivityDays.Text = args.Field.Values;
                    }
                }
                else if (args.Field.SearchCondition.Equals(SearchConditionYear, StringComparison.OrdinalIgnoreCase))
                {
                    args.DivYear.Visible = true;
                    strvalues = args.Field.Values.Split(DelimiterPipeChar);
                    args.TxtFromYear.Text = strvalues[0];
                    args.TxtToYear.Text = strvalues[1];
                }
                else if (args.Field.SearchCondition.Equals(SearchConditionMonth, StringComparison.OrdinalIgnoreCase))
                {
                    args.DivMonth.Visible = true;
                    strvalues = args.Field.Values.Split(DelimiterPipeChar);
                    args.TxtFromMonth.Text = strvalues[0];
                    args.TxtoMonth.Text = strvalues[1];
                }
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            hideActivityPopup.DynamicInvoke();
            this.mdlActivitys.Hide();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            rblOpenSearchType.SelectedIndex = 0;
            drpOpenActivity.ClearSelection();

            divOpenDate.Style.Add("Display", "none");
            divOpenActivityDateRange.Style.Add("Display", "none");
            drpOpenActivityDays.Style.Add("Display", "none");
            divOpenActivityYear.Style.Add("Display", "none");
            divOpenActivityMonth.Style.Add("Display", "none");
            divCustomOpenActivityDays.Style.Add("Display", "none");
            divOpenEmail.Style.Add("Display", "none");
            divOpenEmailDateRange.Style.Add("Display", "none");
            drpOpenEmailDays.Style.Add("Display", "none");
            divOpenEmailYear.Style.Add("Display", "none");
            divOpenEmailMonth.Style.Add("Display", "none");
            divCustomOpenEmailDays.Style.Add("Display", "none");

            drpOpenActivityDateRange.ClearSelection();
            drpOpenActivityDays.ClearSelection();
            drpOpenEmailDateRange.ClearSelection();
            drpOpenEmailDays.ClearSelection();

            txtOpenActivityFrom.Text = string.Empty;
            txtOpenActivityTo.Text = string.Empty;
            txtOpenActivityFromMonth.Text = string.Empty;
            txtOpenActivityToMonth.Text = string.Empty;
            txtOpenActivityFromYear.Text = string.Empty;
            txtOpenActivityToYear.Text = string.Empty;
            txtCustomOpenActivityDays.Text = string.Empty;

            txtOpenBlastID.Text = string.Empty;
            RadCBOpenCampaigns.ClearCheckedItems();
            txtOpenEmailSubject.Text = string.Empty;
            txtOpenEmailFromDate.Text = string.Empty;
            txtOpenEmailToDate.Text = string.Empty;
            txtOpenEmailFromYear.Text = string.Empty;
            txtOpenEmailToYear.Text = string.Empty;
            txtOpenEmailFromMonth.Text = string.Empty;
            txtOpenEmailToMonth.Text = string.Empty;
            txtCustomOpenEmailDays.Text = string.Empty;

            rblClickSearchType.SelectedIndex = 0;
            drpClickActivity.ClearSelection();
            txtLink.Text = string.Empty;

            divClickDate.Style.Add("Display", "none");
            divClickActivityDateRange.Style.Add("Display", "none");
            drpClickActivityDays.Style.Add("Display", "none");
            divClickActivityYear.Style.Add("Display", "none");
            divClickActivityMonth.Style.Add("Display", "none");
            divCustomClickActivityDays.Style.Add("Display", "none");
            divClickEmail.Style.Add("Display", "none");
            divClickEmailDateRange.Style.Add("Display", "none");
            drpClickEmailDays.Style.Add("Display", "none");
            divClickEmailYear.Style.Add("Display", "none");
            divClickEmailMonth.Style.Add("Display", "none");
            divCustomClickEmailDays.Style.Add("Display", "none");

            drpClickActivityDateRange.ClearSelection();
            drpClickActivityDays.ClearSelection();
            drpClickEmailDateRange.ClearSelection();
            drpClickEmailDays.ClearSelection();

            txtClickActivityFrom.Text = string.Empty;
            txtClickActivityTo.Text = string.Empty;
            txtClickActivityFromYear.Text = string.Empty;
            txtClickActivityToYear.Text = string.Empty;
            txtClickActivityFromMonth.Text = string.Empty;
            txtClickActivityToMonth.Text = string.Empty;
            txtCustomClickActivityDays.Text = string.Empty;

            txtClickBlastID.Text = string.Empty;
            RadCBClickCampaigns.ClearCheckedItems();
            txtClickEmailSubject.Text = string.Empty;
            txtClickEmailFromDate.Text = string.Empty;
            txtClickEmailToDate.Text = string.Empty;
            txtClickEmailFromYear.Text = string.Empty;
            txtClickEmailToYear.Text = string.Empty;
            txtClickEmailFromMonth.Text = string.Empty;
            txtClickEmailToMonth.Text = string.Empty;
            txtCustomClickEmailDays.Text = string.Empty;

            divVisitDate.Style.Add("Display", "none");
            divVisitDomain.Style.Add("Display", "none");
            drpVisitActivity.ClearSelection();
            drpDomain.ClearSelection();
            txtURL.Text = string.Empty;

            divVisitActivityDateRange.Style.Add("Display", "inline");
            drpVisitActivityDays.Style.Add("Display", "none");
            divVisitActivityYear.Style.Add("Display", "none");
            divVisitActivityMonth.Style.Add("Display", "none");
            divCustomVisitActivityDays.Style.Add("Display", "none");

            drpVisitActivityDateRange.ClearSelection();
            drpVisitActivityDays.ClearSelection();
            txtVisitActivityFrom.Text = string.Empty;
            txtVisitActivityTo.Text = string.Empty;
            txtVisitActivityFromYear.Text = string.Empty;
            txtVisitActivityToYear.Text = string.Empty;
            txtVisitActivityToMonth.Text = string.Empty;
            txtCustomVisitActivityDays.Text = string.Empty;
        }

        protected void ibChooseDate_Command(object sender, CommandEventArgs e)
        {
            LoadControls();

            rbToday.Checked = false;
            rbTodayPlusMinus.Checked = false;
            ddlPlusMinus.SelectedValue = PlusStr;
            txtDays.Text = string.Empty;
            rbOther.Checked = false;
            txtDatePicker.Text = string.Empty;
            divTodayPlusMinus.Style.Add(StyleDisplay, StyleDisplayValueNone);
            divOther.Style.Add(StyleDisplay, StyleDisplayValueNone);

            RunActionOnTextBox(e.CommandArgument.ToString(), ChooseDateSetControl);

            lblID.Text = e.CommandArgument.ToString();
            mpeCalendar.Show();
        }

        private void ChooseDateSetControl(TextBox txtBox)
        {
            Guard.NotNull(txtBox, nameof(txtBox));

            if (txtBox.Text.Equals(ExpToday, StringComparison.OrdinalIgnoreCase))
            {
                rbToday.Checked = true;
            }
            else if (txtBox.Text.Contains("EXP:Today["))
            {
                rbTodayPlusMinus.Checked = true;
                divTodayPlusMinus.Style.Add(StyleDisplay, StyleDisplayValueInline);

                if (txtBox.Text.Substring(txtBox.Text.IndexOf("[") + 1, 1) == "+")
                {
                    ddlPlusMinus.SelectedValue = "Plus";
                }
                else
                {
                    ddlPlusMinus.SelectedValue = "Minus";
                }

                txtDays.Text = txtBox.Text.Substring(txtBox.Text.IndexOf("[") + 2, txtBox.Text.IndexOf("]") - (txtBox.Text.IndexOf("[") + 2));
            }
            else
            {
                rbOther.Checked = true;
                txtDatePicker.Text = txtBox.Text;
                divOther.Style.Add(StyleDisplay, StyleDisplayValueInline);
            }
        }

        #region choosedate
        protected void rbToday_CheckedChanged(object sender, EventArgs e)
        {
            if (rbToday.Checked)
            {
                divTodayPlusMinus.Style.Add("Display", "inline");
                divOther.Style.Add("Display", "none");
                txtDays.Text = "";
                txtDatePicker.Text = "";
                mpeCalendar.Show();
            }
        }

        protected void rbTodayPlusMinus_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodayPlusMinus.Checked)
            {
                divTodayPlusMinus.Style.Add("Display", "inline");
                divOther.Style.Add("Display", "none");                
                txtDatePicker.Text = "";
                txtDays.Text = "";
                mpeCalendar.Show();
            }
        }

        protected void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOther.Checked)
            {
                divTodayPlusMinus.Style.Add("Display", "none");
                divOther.Style.Add("Display", "inline"); 
                txtDays.Text = "";
                txtDatePicker.Text = "";
                mpeCalendar.Show();
            }
        }

        protected void btnSelectDate_Click(object sender, EventArgs e)
        {
            RunActionOnTextBox(lblID.Text, SelectDateSetControl);
        }

        private void SelectDateSetControl(TextBox txtBox)
        {
            Guard.NotNull(txtBox, nameof(txtBox));

            if (rbToday.Checked)
            {
                txtBox.Text = ExpToday;
            }
            else if (rbTodayPlusMinus.Checked)
            {
                int days;
                if (!int.TryParse(txtDays.Text, out days))
                {
                    txtBox.Text = String.Empty;
                    return;
                }

                var isPlus = string.Equals(ddlPlusMinus.SelectedValue, PlusStr, StringComparison.OrdinalIgnoreCase);
                var sign = isPlus ? SignPlus : SignMinus;
                var text = string.Format(ExpTodayTemplate, sign, days);

                txtBox.Text = text;
            }
            else if (rbOther.Checked)
            {
                txtBox.Text = txtDatePicker.Text;
            }
        }
        #endregion

        private static bool EqualsIgnoreCase(string strA, string strB)
        {
            var isEqual = string.Equals(strA, strB, StringComparison.OrdinalIgnoreCase);
            return isEqual;
        }

        private void RunActionOnTextBox(string dateKind, Action<TextBox> action)
        {
            var lookup = GetTextBoxLookup();
            var txtBox = lookup[dateKind];

            if (txtBox != null)
            {
                Guard.NotNull(action, nameof(action));
                action(txtBox);
            }
        }

        private Dictionary<string, TextBox> GetTextBoxLookup()
        {
            var dateKindTextBoxLookup = new Dictionary<string, TextBox>(StringComparer.OrdinalIgnoreCase)
            {
                [OpenActivityFromDate] = txtOpenActivityFrom,
                [OpenActivityToDate] = txtOpenActivityTo,
                [OpenEmailFromDate] = txtOpenEmailFromDate,
                [OpenEmailToDate] = txtOpenEmailToDate,

                [ClickActivityFromDate] = txtClickActivityFrom,
                [ClickActivityToDate] = txtClickActivityTo,
                [ClickEmailFromDate] = txtClickEmailFromDate,
                [ClickEmailToDate] = txtClickEmailToDate,

                [VisitActivityFromDate] = txtVisitActivityFrom,
                [VisitActivityToDate] = txtVisitActivityTo
            };

            return dateKindTextBoxLookup;
        }
    }
}