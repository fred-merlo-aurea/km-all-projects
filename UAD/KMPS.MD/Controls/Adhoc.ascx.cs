using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KMPS.MD.Helpers;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public partial class Adhoc : BaseControl
    {
        public Delegate hideAdhocPopup;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.mpeDownloads.Show();
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
            }
        }

        public void LoadControls()
        {
            foreach (GridViewRow gv in gvCategory.Rows)
            {
                DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                if (dl != null)
                {
                    foreach (DataListItem di in dl.Items)
                    {
                        var controlSet = new Helpers.AdhocDataListItemControlSet(di);

                        if (controlSet.LbAdhocColumnType.Text.Contains("date"))
                        {
                            if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "daterange")
                            {
                                controlSet.DivAdhocDateRange.Style.Add("display", "inline");
                                controlSet.DivAdhocDateYear.Style.Add("display", "none");
                                controlSet.DivAdhocDateMonth.Style.Add("display", "none");
                                controlSet.DrpAdhocDays.Style.Add("display", "none");
                            }
                            else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "xdays")
                            {
                                controlSet.DivAdhocDateRange.Style.Add("display", "none");
                                controlSet.DivAdhocDateYear.Style.Add("display", "none");
                                controlSet.DivAdhocDateMonth.Style.Add("display", "none");
                                controlSet.DrpAdhocDays.Style.Add("display", "inline");

                                if (controlSet.DrpAdhocDays.SelectedItem.Value.Equals("Custom", StringComparison.OrdinalIgnoreCase))
                                {
                                    controlSet.DivCustomAdhocDays.Style.Add("display", "inline");
                                    controlSet.RfvCustomAdhocDays.Enabled = true;
                                }
                                else
                                {
                                    controlSet.DivCustomAdhocDays.Style.Add("display", "none");
                                    controlSet.RfvCustomAdhocDays.Enabled = false;
                                }
                            }
                            else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "year")
                            {
                                controlSet.DivAdhocDateRange.Style.Add("display", "none");
                                controlSet.DivAdhocDateYear.Style.Add("display", "inline");
                                controlSet.DivAdhocDateMonth.Style.Add("display", "none");
                                controlSet.DrpAdhocDays.Style.Add("display", "none");
                            }
                            else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "month")
                            {
                                controlSet.DivAdhocDateRange.Style.Add("display", "none");
                                controlSet.DivAdhocDateYear.Style.Add("display", "none");
                                controlSet.DivAdhocDateMonth.Style.Add("display", "inline");
                                controlSet.DrpAdhocDays.Style.Add("display", "none");
                            }
                        }
                        else if (controlSet.DrpAdhocSearch.SelectedItem.Value.Equals("Is Empty", StringComparison.OrdinalIgnoreCase) || controlSet.DrpAdhocSearch.SelectedItem.Value.Equals("Is Not Empty", StringComparison.OrdinalIgnoreCase))
                        {
                            controlSet.TxtAdhocSearchValue.Attributes.Add("disabled", "disabled");
                        }
                    }
                }
            }
        }

        protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataList dlAdhocFilter = (DataList)e.Row.FindControl("dlAdhocFilter");

                dlAdhocFilter.DataSource = KMPS.MD.Objects.Adhoc.GetByCategoryID(clientconnections, Convert.ToInt32(gvCategory.DataKeys[e.Row.RowIndex].Value), BrandID, PubID);
                dlAdhocFilter.DataBind();

            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < gvCategory.Rows.Count; i++)
            {
                GridViewRow row = gvCategory.Rows[i];
                DataList dlAdhocFilter = (DataList)row.Cells[0].FindControl("dlAdhocFilter");

                foreach (DataListItem di in dlAdhocFilter.Items)
                {
                    TextBox txtAdhocSearchValue = (TextBox)di.FindControl("txtAdhocSearchValue");
                    txtAdhocSearchValue.Text = string.Empty;
                    txtAdhocSearchValue.Attributes.Remove("disabled");

                    HtmlGenericControl divAdhocRange = (HtmlGenericControl)di.FindControl("divAdhocRange");

                    divAdhocRange.Style.Add("display", "none");

                    TextBox txtAdhocRangeFrom = (TextBox)di.FindControl("txtAdhocRangeFrom");
                    txtAdhocRangeFrom.Text = string.Empty;
                    TextBox txtAdhocRangeTo = (TextBox)di.FindControl("txtAdhocRangeTo");
                    txtAdhocRangeTo.Text = string.Empty;

                    DropDownList drpAdhoc = (DropDownList)di.FindControl("drpAdhocSearch");
                    drpAdhoc.ClearSelection();

                    DropDownList drpDateRange = (DropDownList)di.FindControl("drpDateRange");
                    drpDateRange.ClearSelection();

                    HtmlGenericControl divAdhocDateRange = (HtmlGenericControl)di.FindControl("divAdhocDateRange");
                    TextBox txtAdhocDateRangeFrom = (TextBox)di.FindControl("txtAdhocDateRangeFrom");
                    TextBox txtAdhocDateRangeTo = (TextBox)di.FindControl("txtAdhocDateRangeTo");
                    divAdhocDateRange.Style.Add("display", "inline");
                    txtAdhocDateRangeFrom.Text = string.Empty;
                    txtAdhocDateRangeTo.Text = string.Empty;

                    DropDownList drpAdhocDays = (DropDownList)di.FindControl("drpAdhocDays");
                    drpAdhocDays.Style.Add("display", "none");
                    drpAdhocDays.ClearSelection();

                    HtmlGenericControl divCustomAdhocDays = (HtmlGenericControl)di.FindControl("divCustomAdhocDays");
                    divCustomAdhocDays.Style.Add("display", "none");
                    TextBox txtCustomAdhocDays = (TextBox)di.FindControl("txtCustomAdhocDays");
                    txtCustomAdhocDays.Text = string.Empty;
                    RequiredFieldValidator rfvCustomAdhocDays = (RequiredFieldValidator)di.FindControl("rfvCustomAdhocDays");
                    rfvCustomAdhocDays.Enabled = false;

                    HtmlGenericControl divAdhocDateYear = (HtmlGenericControl)di.FindControl("divAdhocDateYear");
                    TextBox txtAdhocDateYearFrom = (TextBox)di.FindControl("txtAdhocDateYearFrom");
                    TextBox txtAdhocDateYearTo = (TextBox)di.FindControl("txtAdhocDateYearTo");
                    divAdhocDateYear.Style.Add("display", "none"); 
                    txtAdhocDateYearFrom.Text = string.Empty;
                    txtAdhocDateYearTo.Text = string.Empty;

                    HtmlGenericControl divAdhocDateMonth = (HtmlGenericControl)di.FindControl("divAdhocDateMonth");
                    TextBox txtAdhocDateMonthFrom = (TextBox)di.FindControl("txtAdhocDateMonthFrom");
                    TextBox txtAdhocDateMonthTo = (TextBox)di.FindControl("txtAdhocDateMonthTo");
                    divAdhocDateMonth.Style.Add("display", "none"); 
                    txtAdhocDateMonthFrom.Text = string.Empty;
                    txtAdhocDateMonthTo.Text = string.Empty;

                    DropDownList drpAdhocInt = (DropDownList)di.FindControl("drpAdhocInt");
                    drpAdhocInt.ClearSelection();
                    TextBox txtAdhocIntFrom = (TextBox)di.FindControl("txtAdhocIntFrom");
                    txtAdhocIntFrom.Text = string.Empty;
                    TextBox txtAdhocIntTo = (TextBox)di.FindControl("txtAdhocIntTo");
                    txtAdhocIntTo.Text = string.Empty;
                    RadioButtonList rblAdhocBit = (RadioButtonList)di.FindControl("rblAdhocBit");
                    rblAdhocBit.ClearSelection();
                    DropDownList drpSubscribed = (DropDownList)di.FindControl("drpSubscribed");
                    drpSubscribed.ClearSelection();
                    TextBox txtPubCount = (TextBox)di.FindControl("txtPubCount");
                    txtPubCount.Text = string.Empty;
                }
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvCategory.Rows.Count; i++)
            {
                GridViewRow row = gvCategory.Rows[i];
                DataList dlAdhocFilter = (DataList)row.Cells[0].FindControl("dlAdhocFilter");
                foreach (DataListItem di in dlAdhocFilter.Items)
                {
                    DropDownList drpDateRange = (DropDownList)di.FindControl("drpDateRange");
                    TextBox txtAdhocDateYearFrom = (TextBox)di.FindControl("txtAdhocDateYearFrom");
                    TextBox txtAdhocDateYearTo = (TextBox)di.FindControl("txtAdhocDateYearTo");
                    TextBox txtAdhocDateMonthFrom = (TextBox)di.FindControl("txtAdhocDateMonthFrom");
                    TextBox txtAdhocDateMonthTo = (TextBox)di.FindControl("txtAdhocDateMonthTo");
                    int integer;

                    if (drpDateRange.SelectedValue.ToUpper() == "YEAR")
                    {
                        if (!Int32.TryParse(txtAdhocDateYearFrom.Text, out integer) && txtAdhocDateYearTo.Text != "")
                        {
                            divError.Visible = true;
                            lblErrorMessage.Text = "Please enter valid year";
                            return;
                        }
                    }
                    else if (drpDateRange.SelectedValue.ToUpper() == "MONTH")
                    {
                        if (!Int32.TryParse(txtAdhocDateMonthFrom.Text, out integer) && txtAdhocDateMonthTo.Text != "")
                        {
                            divError.Visible = true;
                            lblErrorMessage.Text = "Please enter valid month";
                            return;
                        }
                    }
                }
            }

            hideAdhocPopup.DynamicInvoke();
            this.mpeDownloads.Hide();
        }

        protected void dlAdhocFilter_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbAdhocColumnValue = (Label)e.Item.FindControl("lbAdhocColumnValue");
                if (lbAdhocColumnValue.Text == "[ZIP]" || lbAdhocColumnValue.Text == "[ZIPCODE]")
                {
                    DropDownList drpAdhocSearch = (DropDownList)e.Item.FindControl("drpAdhocSearch");
                    drpAdhocSearch.Items.Add(new ListItem("RANGE", "RANGE"));
                }

                Label lblAdhocDisplayName = (Label)e.Item.FindControl("lblAdhocDisplayName");
                ImageButton ibChooseFromDate = (ImageButton)e.Item.FindControl("ibChooseFromDate");
                ibChooseFromDate.CommandArgument = lblAdhocDisplayName.Text + "|FROM";
                ImageButton ibChooseToDate = (ImageButton)e.Item.FindControl("ibChooseToDate");
                ibChooseToDate.CommandArgument = lblAdhocDisplayName.Text + "|TO";
            }
        }

        public void LoadAdhocGrid()
        {
            List<AdhocCategory> acat = AdhocCategory.Get(clientconnections);

            AdhocCategory ac = new AdhocCategory();
            ac.CategoryID = 0;
            ac.CategoryName = "Others";
            acat.Add(ac);

            gvCategory.DataSource = acat;
            gvCategory.DataBind();
        }

        protected void ibChooseDate_Command(object sender, CommandEventArgs e)
        {
            LoadControls();

            rbToday.Checked = false;
            rbTodayPlusMinus.Checked = false;
            ddlPlusMinus.SelectedValue = "Plus";
            txtDays.Text = "";
            rbOther.Checked = false;
            txtDatePicker.Text = "";
            divTodayPlusMinus.Style.Add("Display", "none");
            divOther.Style.Add("Display", "none");

            string[] strValue = e.CommandArgument.ToString().Split('|');

            foreach (GridViewRow gv in gvCategory.Rows)
            {
                DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                if (dl != null)
                {
                    foreach (DataListItem di in dl.Items)
                    {
                        Label lblAdhocDisplayName = (Label)di.FindControl("lblAdhocDisplayName");

                        if (lblAdhocDisplayName.Text == strValue[0])
                        {
                            if (strValue[1].Equals("FROM", StringComparison.OrdinalIgnoreCase))
                            {
                                TextBox txtAdhocDateRangeFrom = (TextBox)di.FindControl("txtAdhocDateRangeFrom");

                                if (txtAdhocDateRangeFrom.Text.Equals("EXP:Today", StringComparison.OrdinalIgnoreCase))
                                    rbToday.Checked = true;
                                else if (txtAdhocDateRangeFrom.Text.Contains("EXP:Today["))
                                {
                                    rbTodayPlusMinus.Checked = true;
                                    divTodayPlusMinus.Style.Add("Display", "inline");

                                    if (txtAdhocDateRangeFrom.Text.Substring(txtAdhocDateRangeFrom.Text.IndexOf("[") + 1, 1) == "+")
                                        ddlPlusMinus.SelectedValue = "Plus";
                                    else
                                        ddlPlusMinus.SelectedValue = "Minus";

                                    txtDays.Text = txtAdhocDateRangeFrom.Text.Substring(txtAdhocDateRangeFrom.Text.IndexOf("[") + 2, txtAdhocDateRangeFrom.Text.IndexOf("]") - (txtAdhocDateRangeFrom.Text.IndexOf("[") + 2));
                                }
                                else
                                {
                                    rbOther.Checked = true;
                                    txtDatePicker.Text = txtAdhocDateRangeFrom.Text;
                                    divOther.Style.Add("Display", "inline");
                                }
                            }
                            else
                            {
                                TextBox txtAdhocDateRangeTo = (TextBox)di.FindControl("txtAdhocDateRangeTo");

                                if (txtAdhocDateRangeTo.Text.Equals("EXP:Today", StringComparison.OrdinalIgnoreCase))
                                    rbToday.Checked = true;
                                else if (txtAdhocDateRangeTo.Text.Contains("EXP:Today["))
                                {
                                    rbTodayPlusMinus.Checked = true;
                                    divTodayPlusMinus.Style.Add("Display", "inline");

                                    if (txtAdhocDateRangeTo.Text.Substring(txtAdhocDateRangeTo.Text.IndexOf("[") + 1, 1) == "+")
                                        ddlPlusMinus.SelectedValue = "Plus";
                                    else
                                        ddlPlusMinus.SelectedValue = "Minus";

                                    txtDays.Text = txtAdhocDateRangeTo.Text.Substring(txtAdhocDateRangeTo.Text.IndexOf("[") + 2, txtAdhocDateRangeTo.Text.IndexOf("]") - (txtAdhocDateRangeTo.Text.IndexOf("[") + 2));
                                }
                                else
                                {
                                    rbOther.Checked = true;
                                    txtDatePicker.Text = txtAdhocDateRangeTo.Text;
                                    divOther.Style.Add("Display", "inline");
                                }
                            }
                        }
                    }
                }
            }

            lblID.Text = e.CommandArgument.ToString();
            LoadControls();
            mpeCalendar.Show();
        }

        public void LoadAdhocFilters(Field field)
        {
            foreach (GridViewRow gv in gvCategory.Rows)
            {
                DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                if (dl != null)
                {
                    foreach (DataListItem di in dl.Items)
                    {
                        var controlSet = new Helpers.AdhocDataListItemControlSet(di);

                        if (controlSet.LbAdhocColumnValue.Text == field.Group)
                        {
                            string[] strID = field.Group.Split('|');

                            switch (strID[0].ToLower())
                            {
                                case "e":
                                    switch (strID[2].ToLower())
                                    {
                                        case "d":
                                            #region Adhoc Date
                                            string[] strValue = field.Values.Split('|');

                                            if (field.SearchCondition.ToLower() == "daterange")
                                            {
                                                controlSet.DivAdhocDateRange.Style.Add("display", "inline");
                                                controlSet.DivAdhocDateYear.Style.Add("display", "none");
                                                controlSet.DivAdhocDateMonth.Style.Add("display", "none");

                                                controlSet.TxtAdhocDateRangeFrom.Text = strValue[0];
                                                controlSet.TxtAdhocDateRangeTo.Text = strValue[1];
                                            }
                                            else if (field.SearchCondition.ToLower() == "xdays")
                                            {
                                                controlSet.DrpAdhocDays.Style.Add("display", "inline");
                                                controlSet.DrpAdhocDays.SelectedValue = strValue[0];
                                            }
                                            else if (field.SearchCondition.ToLower() == "year")
                                            {
                                                controlSet.DivAdhocDateRange.Style.Add("display", "none");
                                                controlSet.DivAdhocDateYear.Style.Add("display", "inline");
                                                controlSet.DivAdhocDateMonth.Style.Add("display", "none");

                                                controlSet.TxtAdhocDateYearFrom.Text = strValue[0];
                                                controlSet.TxtAdhocDateYearTo.Text = strValue[1];
                                            }
                                            else if (field.SearchCondition.ToLower() == "month")
                                            {
                                                AdhocHelper.SetDateStyleForMonth(controlSet, strValue); 
                                            }

                                            AdhocHelper.SetSelectedTrue(controlSet.DrpDateRange, field.SearchCondition);
                                            break;
                                            #endregion
                                        case "b":
                                            AdhocHelper.SetSelectedTrue(controlSet.RblAdhocBit, field.Values);
                                            break;
                                        case "i":
                                        case "f":
                                            string[] strValues = field.Values.Split('|');

                                            controlSet.TxtAdhocIntFrom.Text = strValues[0];
                                            controlSet.TxtAdhocIntTo.Text = strValues[1];

                                            if (field.SearchCondition.ToUpper() == "EQUAL" || field.SearchCondition.ToUpper() == "GREATER" || field.SearchCondition.ToUpper() == "LESSER")
                                            {
                                                controlSet.TxtAdhocIntTo.Style.Add("display", "none");
                                            }
                                            else
                                            {
                                                controlSet.TxtAdhocIntTo.Style.Add("display", "inline");
                                            }

                                            controlSet.DrpAdhocInt.SelectedIndex = -1;

                                            if (controlSet.DrpAdhocInt.Items.FindByValue(field.SearchCondition) != null)
                                            {
                                                controlSet.DrpAdhocInt.Items.FindByValue(field.SearchCondition).Selected = true;
                                                break;
                                            }
                                            break;
                                        default:
                                            AdhocHelper.SetDefaultSettings(controlSet, field);
                                            break;
                                    }
                                    break;
                                case "d":
                                    #region Adhoc Date
                                    string[] strDateValue = field.Values.Split('|');

                                    if (field.SearchCondition.ToLower() == "daterange")
                                    {
                                        controlSet.DivAdhocDateRange.Style.Add("display", "inline");
                                        controlSet.DivAdhocDateYear.Style.Add("display", "none");
                                        controlSet.DivAdhocDateMonth.Style.Add("display", "none");

                                        controlSet.TxtAdhocDateRangeFrom.Text = strDateValue[0];
                                        controlSet.TxtAdhocDateRangeTo.Text = strDateValue[1];
                                    }
                                    else if (field.SearchCondition.ToLower() == "xdays")
                                    {
                                        controlSet.DrpAdhocDays.Style.Add("display", "inline");

                                        List<string> days = new List<string>(new string[] {"7", "14", "21", "30", "60", "90", "120", "150", "6mon", "1yr" });

                                        if (days.Contains(field.Values))
                                            controlSet.DrpAdhocDays.SelectedValue = strDateValue[0];
                                        else
                                        {
                                            controlSet.DrpAdhocDays.SelectedValue = "Custom";
                                            controlSet.TxtCustomAdhocDays.Style.Add("display", "inline");
                                            controlSet.TxtCustomAdhocDays.Text = strDateValue[0];
                                            controlSet.RfvCustomAdhocDays.Enabled = true;
                                        }
                                    }
                                    else if (field.SearchCondition.ToLower() == "year")
                                    {
                                        controlSet.DivAdhocDateRange.Style.Add("display", "none");
                                        controlSet.DivAdhocDateYear.Style.Add("display", "inline");
                                        controlSet.DivAdhocDateMonth.Style.Add("display", "none");

                                        controlSet.TxtAdhocDateYearFrom.Text = strDateValue[0];
                                        controlSet.TxtAdhocDateYearTo.Text = strDateValue[1];
                                    }
                                    else if (field.SearchCondition.ToLower() == "month")
                                    {
                                        AdhocHelper.SetDateStyleForMonth(controlSet, strDateValue);
                                    }

                                    AdhocHelper.SetSelectedTrue(controlSet.DrpDateRange, field.SearchCondition);
                                    break;
                                    #endregion
                                case "b":
                                    AdhocHelper.SetSelectedTrue(controlSet.RblAdhocBit, field.Values);
                                    break;
                                case "i":
                                case "f":
                                    if (strID[1].Equals("[PRODUCT COUNT]", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        controlSet.TxtPubCount.Text = field.Values;

                                        controlSet.DrpSubscribed.SelectedIndex = -1;

                                        if (controlSet.DrpSubscribed.Items.FindByValue(field.SearchCondition) != null)
                                        {
                                            controlSet.DrpSubscribed.Items.FindByValue(field.SearchCondition).Selected = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        string[] strVal1 = field.Values.Split('|');
                                        controlSet.TxtAdhocIntFrom.Text = strVal1[0];
                                        controlSet.TxtAdhocIntTo.Text = strVal1[1];

                                        if (field.SearchCondition.ToUpper() == "EQUAL" || field.SearchCondition.ToUpper() == "GREATER" || field.SearchCondition.ToUpper() == "LESSER")
                                        {
                                            controlSet.TxtAdhocIntTo.Style.Add("display", "none");
                                        }
                                        else
                                        {
                                            controlSet.TxtAdhocIntTo.Style.Add("display", "inline");
                                        }

                                        controlSet.DrpAdhocInt.SelectedIndex = -1;

                                        if (controlSet.DrpAdhocInt.Items.FindByValue(field.SearchCondition) != null)
                                        {
                                            controlSet.DrpAdhocInt.Items.FindByValue(field.SearchCondition).Selected = true;
                                            break;
                                        }
                                    }
                                    break;
                                default:
                                    AdhocHelper.SetDefaultSettings(controlSet, field); 
                                    break;
                            }
                        }

                    }
                }
            }
        }

        public List<Field> GetAdhocFilters()
        {
            List<Field> fields = new List<Field>();

            foreach (GridViewRow gv in gvCategory.Rows)
            {
                DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                if (dl != null)
                {
                    foreach (DataListItem di in dl.Items)
                    {

                        var controlSet = new Helpers.AdhocDataListItemControlSet(di);

                        if (controlSet.TxtAdhocSearchValue.Text != string.Empty || controlSet.TxtAdhocRangeFrom.Text != string.Empty || controlSet.TxtAdhocRangeTo.Text != string.Empty || controlSet.TxtAdhocDateRangeFrom.Text != string.Empty || controlSet.TxtAdhocDateRangeTo.Text != string.Empty || controlSet.TxtAdhocDateYearFrom.Text != string.Empty || controlSet.TxtAdhocDateYearTo.Text != string.Empty || controlSet.TxtAdhocDateMonthFrom.Text != string.Empty || controlSet.TxtAdhocDateMonthTo.Text != string.Empty || controlSet.TxtAdhocIntFrom.Text != string.Empty || controlSet.TxtAdhocIntTo.Text != string.Empty || controlSet.RblAdhocBit.SelectedIndex != -1 || controlSet.DrpAdhocSearch.SelectedItem.Value.Equals("Is Empty", StringComparison.OrdinalIgnoreCase) || controlSet.DrpAdhocSearch.SelectedItem.Value.Equals("Is Not Empty", StringComparison.OrdinalIgnoreCase) || controlSet.TxtPubCount.Text != string.Empty || (controlSet.DrpAdhocDays.SelectedValue != "0" && controlSet.DrpDateRange.SelectedItem.Value.Equals("xdays", StringComparison.OrdinalIgnoreCase)))
                        {
                            if (controlSet.LbAdhocColumnType.Text.Contains("date"))
                            {
                                if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "daterange")
                                {
                                    fields.Add(new Field("Adhoc", controlSet.TxtAdhocDateRangeFrom.Text + "|" + controlSet.TxtAdhocDateRangeTo.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpDateRange.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                }
                                else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "xdays")
                                {
                                    if (controlSet.DrpAdhocDays.SelectedItem.Value.ToLower() == "custom")
                                        fields.Add(new Field("Adhoc", controlSet.TxtCustomAdhocDays.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpDateRange.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                    else
                                        fields.Add(new Field("Adhoc", controlSet.DrpAdhocDays.SelectedValue, controlSet.LblAdhocDisplayName.Text, controlSet.DrpDateRange.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                }
                                else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "year")
                                {
                                    fields.Add(new Field("Adhoc", controlSet.TxtAdhocDateYearFrom.Text + "|" + controlSet.TxtAdhocDateYearTo.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpDateRange.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                }
                                else if (controlSet.DrpDateRange.SelectedItem.Value.ToLower() == "month")
                                {
                                    fields.Add(new Field("Adhoc", controlSet.TxtAdhocDateMonthFrom.Text + "|" + controlSet.TxtAdhocDateMonthTo.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpDateRange.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                }
                            }
                            else if (controlSet.LbAdhocColumnType.Text.Contains("varchar") || controlSet.LbAdhocColumnType.Text.Contains("uniqueidentifier"))
                            {
                                if (controlSet.DrpAdhocSearch.SelectedItem.Value == "RANGE")
                                    fields.Add(new Field("Adhoc", controlSet.TxtAdhocRangeFrom.Text + "|" + controlSet.TxtAdhocRangeTo.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpAdhocSearch.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                                else
                                    fields.Add(new Field("Adhoc", controlSet.TxtAdhocSearchValue.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpAdhocSearch.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                            }
                            else if ((controlSet.LbAdhocColumnType.Text.Contains("int") || controlSet.LbAdhocColumnType.Text.Contains("float")) && !controlSet.LblAdhocDisplayName.Text.ToString().Contains("PRODUCT COUNT"))
                                fields.Add(new Field("Adhoc", controlSet.TxtAdhocIntFrom.Text + "|" + controlSet.TxtAdhocIntTo.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpAdhocInt.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                            else if (controlSet.LbAdhocColumnType.Text.Contains("int") && controlSet.LblAdhocDisplayName.Text.ToString().Contains("PRODUCT COUNT"))
                                fields.Add(new Field("Adhoc", controlSet.TxtPubCount.Text, controlSet.LblAdhocDisplayName.Text, controlSet.DrpSubscribed.SelectedItem.Value, Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                            else if (controlSet.LbAdhocColumnType.Text.Contains("bit"))
                                fields.Add(new Field("Adhoc", controlSet.RblAdhocBit.SelectedItem.Value, controlSet.LblAdhocDisplayName.Text, "", Enums.FiltersType.Adhoc, controlSet.LbAdhocColumnValue.Text));
                        }
                    }
                }
            }

            return fields;
        }

        #region choosedate
        protected void btnSelectDate_Click(object sender, EventArgs e)
        {
            int i = 0;

            string[] strValue = lblID.Text.Split('|');

            foreach (GridViewRow gv in gvCategory.Rows)
            {
                DataList dl = (DataList)gv.FindControl("dlAdhocFilter");

                if (dl != null)
                {
                    foreach (DataListItem di in dl.Items)
                    {
                        Label lblAdhocDisplayName = (Label)di.FindControl("lblAdhocDisplayName");

                        if (lblAdhocDisplayName.Text == strValue[0])
                        {
                            if (strValue[1].Equals("FROM", StringComparison.OrdinalIgnoreCase))
                            {
                                TextBox txtAdhocDateRangeFrom = (TextBox)di.FindControl("txtAdhocDateRangeFrom");
                                if (rbToday.Checked)
                                    txtAdhocDateRangeFrom.Text = "EXP:Today";
                                else if (rbTodayPlusMinus.Checked)
                                {
                                    try
                                    {
                                        Convert.ToInt32(txtDays.Text);
                                        txtAdhocDateRangeFrom.Text = "EXP:Today[";
                                        if (ddlPlusMinus.SelectedValue == "Plus")
                                            txtAdhocDateRangeFrom.Text += "+";
                                        else
                                            txtAdhocDateRangeFrom.Text += "-";

                                        txtAdhocDateRangeFrom.Text += txtDays.Text.Trim() + "]";
                                    }
                                    catch (Exception)
                                    {
                                        txtAdhocDateRangeFrom.Text = "";
                                    }
                                }
                                else if (rbOther.Checked)
                                    txtAdhocDateRangeFrom.Text = txtDatePicker.Text;
                            }
                            else if (strValue[1].Equals("TO", StringComparison.OrdinalIgnoreCase))
                            {
                                TextBox txtAdhocDateRangeTo = (TextBox)di.FindControl("txtAdhocDateRangeTo");
                                if (rbToday.Checked)
                                    txtAdhocDateRangeTo.Text = "EXP:Today";
                                else if (rbTodayPlusMinus.Checked)
                                {
                                    try
                                    {
                                        Convert.ToInt32(txtDays.Text);
                                        txtAdhocDateRangeTo.Text = "EXP:Today[";
                                        if (ddlPlusMinus.SelectedValue == "Plus")
                                            txtAdhocDateRangeTo.Text += "+";
                                        else
                                            txtAdhocDateRangeTo.Text += "-";

                                        txtAdhocDateRangeTo.Text += txtDays.Text.Trim() + "]";
                                    }
                                    catch (Exception)
                                    {
                                        txtAdhocDateRangeTo.Text = "";
                                    }
                                }
                                else if (rbOther.Checked)
                                    txtAdhocDateRangeTo.Text = txtDatePicker.Text;
                            }
                        }
                        i++;
                    }
                }
            }
        }
        #endregion
    }
}