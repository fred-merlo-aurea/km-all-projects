using ecn.common.classes.billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Diag = System.Diagnostics;

namespace ecn.accounts.includes
{
    public abstract class QuoteItemEditorBase : UserControl
    {
        protected internal const string QuoteKeySessionKey = "QuoteKey";
        protected internal const string ControlNameDltEmailOptions = "dltEmailOptions";
        protected internal const string ControlNameBtnAdd = "btnAdd";
        protected internal const string ControlNameDdlAddOptions = "ddlAddOptions";
        protected internal const string ControlNameDdlAddFrequency = "ddlAddFrequency";
        protected internal const string ControlNameTxtAddRate = "txtAddRate";
        protected internal const string ControlNameTxtAddDiscountRate = "txtAddDiscountRate";
        protected internal const string ControlNameDdlEditType = "ddlEditType";
        protected internal const string ControlNameTxtEditRate = "txtEditRate";
        protected internal const string ControlNameTxtEditDiscountRate = "txtEditDiscountRate";
        protected internal const string ControlNameDdlEditFrequency = "ddlEditFrequency";
        protected internal const string CommandNameEdit = "Edit";
        protected internal const string CommandNameUpdate = "Update";
        protected internal const string CommandNameCancel = "Cancel";
        protected internal const string CommandNameDelete = "Delete";
        protected internal const string FrequencyNameAnnual = "Annual";
        protected internal const string FrequencyNameQuarterly = "Quarterly";
        protected internal const string FrequencyNameMonthly = "Monthly";
        protected internal const string FrequencyNameOneTime = "One Time";
        protected static readonly string FrequencyValueAnnual = ((int)FrequencyEnum.Annual).ToString();
        protected static readonly string FrequencyValueQuarterly = ((int)FrequencyEnum.Quarterly).ToString();
        protected static readonly string FrequencyValueMonthly = ((int)FrequencyEnum.Monthly).ToString();
        protected static readonly string FrequencyValueOneTime = ((int)FrequencyEnum.OneTime).ToString();

        public event EventHandler OnQuoteItemAdded;

        protected DataList dltEmailOptions;

        public virtual QuoteOptionCollection QuoteOptions
        {
            get
            {
                return Session[QuoteOptionsKey] as QuoteOptionCollection;
            }
            set
            {
                Session[QuoteOptionsKey] = value;
            }
        }

        public virtual QuoteItemCollection QuoteItems
        {
            get
            {
                return Session[QuoteItemsKey] as QuoteItemCollection;
            }
            set
            {
                Session[QuoteItemsKey] = value;
                LoadQuoteItems(value);
            }
        }

        public virtual string CurrentQuoteStatus
        {
            get
            {
                var status = string.Empty;

                if (Session[QuoteKeySessionKey] != null)
                {
                    var quote = Session[QuoteKeySessionKey] as Quote;
                    status = quote != null ? quote.Status.ToString() : string.Empty;
                }

                return status;
            }
        }

        protected virtual void btnAdd_Click(object sender, EventArgs e)
        {
            var approvedText = QuoteStatusEnum.Approved.ToString();

            if (!CurrentQuoteStatus.Equals(approvedText, StringComparison.OrdinalIgnoreCase))
            {
                var btnAdd = sender as LinkButton;

                if (btnAdd != null && btnAdd.Parent != null)
                {
                    var parentControl = btnAdd.Parent;

                    var ddlAddOptions = parentControl.FindControl(ControlNameDdlAddOptions) as DropDownList;
                    var txtAddRate = parentControl.FindControl(ControlNameTxtAddRate) as TextBox;
                    var txtAddDiscountRate = parentControl.FindControl(ControlNameTxtAddDiscountRate) as TextBox;
                    var ddlAddFrequency = parentControl.FindControl(ControlNameDdlAddFrequency) as DropDownList;

                    if (ddlAddOptions != null &&
                        txtAddRate != null &&
                        txtAddDiscountRate != null &&
                        ddlAddFrequency != null)
                    {
                        var quoteOption = QuoteOptions.FindByCode(ddlAddOptions.SelectedValue);
                        var quoteItem = ConvertQuoteOptionToQuoteItem(quoteOption);

                        const int percentageDivider = 100;
                        double rate;
                        double discountRate;
                        int frequencyValue;

                        double.TryParse(txtAddRate.Text, out rate);
                        double.TryParse(txtAddDiscountRate.Text, out discountRate);
                        int.TryParse(ddlAddFrequency.SelectedValue, out frequencyValue);

                        quoteItem.Rate = rate;
                        quoteItem.DiscountRate = discountRate / percentageDivider;
                        quoteItem.Frequency = (FrequencyEnum)frequencyValue;
                        QuoteItems.Add(quoteItem);

                        LoadQuoteItems(QuoteItems);
                        FireQuoteItemChangeEvent();
                    }
                }
            }
        }

        protected virtual void dltEmailOptions_ItemCommand(object source, DataListCommandEventArgs e)
        {
            var master = (Page.Master as MasterPages.Accounts);
            var approvedText = QuoteStatusEnum.Approved.ToString();

            if (!CurrentQuoteStatus.Equals(approvedText, StringComparison.OrdinalIgnoreCase))
            {
                switch (e.CommandName)
                {
                    case CommandNameEdit:
                        dltEmailOptions.EditItemIndex = e.Item.ItemIndex;
                        break;
                    case CommandNameUpdate:
                        var ddlOptions = e.Item.FindControl(ControlNameDdlEditType) as DropDownList;
                        var txtEditRate = e.Item.FindControl(ControlNameTxtEditRate) as TextBox;
                        var txtEditDiscountRate = e.Item.FindControl(ControlNameTxtEditDiscountRate) as TextBox;
                        var ddlEditFrequency = e.Item.FindControl(ControlNameDdlEditFrequency) as DropDownList;

                        if (ddlOptions != null &&
                            txtEditRate != null &&
                            txtEditDiscountRate != null &&
                            ddlEditFrequency != null)
                        {
                            var quoteOption = QuoteOptions.FindByCode(ddlOptions.SelectedValue);
                            var quoteItem = QuoteItems[e.Item.ItemIndex];
                            CopyQuoteOptionToQuoteItem(quoteItem, quoteOption);

                            const int percentageDivider = 100;
                            double rate;
                            double discountRate;
                            int frequencyValue;

                            double.TryParse(txtEditRate.Text, out rate);
                            double.TryParse(txtEditDiscountRate.Text, out discountRate);
                            int.TryParse(ddlEditFrequency.SelectedValue, out frequencyValue);

                            quoteItem.Rate = rate;
                            quoteItem.DiscountRate = discountRate / percentageDivider;
                            quoteItem.Frequency = (FrequencyEnum)frequencyValue;

                            FireQuoteItemChangeEvent();
                            dltEmailOptions.EditItemIndex = -1;
                        }
                        break;
                    case CommandNameCancel:
                        dltEmailOptions.EditItemIndex = -1;
                        break;
                    case CommandNameDelete:
                        var item = QuoteItems[e.Item.ItemIndex];
                        item.Delete(master.UserSession.CurrentUser.UserID);
                        QuoteItems.RemoveAt(e.Item.ItemIndex);
                        FireQuoteItemChangeEvent();
                        break;
                }
                LoadQuoteItems(QuoteItems);
            }
        }

        protected void InitializeFrequencyDropDownList(DropDownList dropDownList, QuoteOption quoteOption)
        {
            ListItem frequencyItem;

            dropDownList.Items.Clear();
            if (quoteOption.IsAllowed(FrequencyEnum.Monthly))
            {
                frequencyItem = new ListItem(FrequencyNameMonthly, FrequencyValueMonthly);
                dropDownList.Items.Add(frequencyItem);
            }
            if (quoteOption.IsAllowed(FrequencyEnum.Quarterly))
            {
                frequencyItem = new ListItem(FrequencyNameQuarterly, FrequencyValueQuarterly);
                dropDownList.Items.Add(frequencyItem);
            }
            if (quoteOption.IsAllowed(FrequencyEnum.Annual))
            {
                frequencyItem = new ListItem(FrequencyNameAnnual, FrequencyValueAnnual);
                dropDownList.Items.Add(frequencyItem);
            }
            if (quoteOption.IsAllowed(FrequencyEnum.OneTime))
            {
                frequencyItem = new ListItem(FrequencyNameOneTime, FrequencyValueOneTime);
                dropDownList.Items.Add(frequencyItem);
            }
        }

        protected virtual void FireQuoteItemChangeEvent()
        {
            if (OnQuoteItemAdded != null)
            {
                OnQuoteItemAdded(this, EventArgs.Empty);
            }
        }

        protected virtual string QuoteOptionsKey
        {
            get
            {
                const string quoteOptionsKeySuffix = "_QuoteOptionsKey";
                return string.Format("{0}{1}", ID, quoteOptionsKeySuffix);
            }
        }

        protected virtual string QuoteItemsKey
        {
            get
            {
                const string quoteItemsKeySuffix = "_QuoteItemsKey";
                return string.Format("{0}{1}", ID, quoteItemsKeySuffix);
            }
        }

        protected virtual void LoadQuoteItems(QuoteItemCollection quoteItems)
        {
            dltEmailOptions.DataSource = quoteItems;
            dltEmailOptions.DataBind();
        }

        protected virtual QuoteItem ConvertQuoteOptionToQuoteItem(QuoteOption option)
        {
            var item = new QuoteItem(
                option.AllowedFrequency,
                option.Code,
                option.Name,
                option.Description,
                option.Quantity,
                option.Rate,
                option.LicenseType,
                option.PriceType,
                option.IsCustomerCredit);

            item.RemoveProducts();
            item.AddProductFromOption(option);
            item.RemoveProductFeatures();
            item.AddProductFeatureFromOption(option);
            item.RemoveServices();
            item.AddServiceFromOption(option);

            return item;
        }

        protected virtual void BindQuoteOptionDDL(DropDownList dropDownList, string selectedValue)
        {
            const string dataTextField = "Name";
            const string dataValueField = "Code";

            dropDownList.DataSource = QuoteOptions;
            dropDownList.DataTextField = dataTextField;
            dropDownList.DataValueField = dataValueField;
            dropDownList.DataBind();

            var selectedItem = dropDownList.Items.FindByValue(selectedValue);
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
            }
        }

        protected virtual void CopyQuoteOptionToQuoteItem(QuoteItem quoteItem, QuoteOption quoteOption)
        {
            quoteItem.Code = quoteOption.Code;
            quoteItem.Name = quoteOption.Name;
            quoteItem.Description = quoteOption.Description;
            quoteItem.Quantity = quoteOption.Quantity;
            quoteItem.RemoveProducts();
            quoteItem.AddProductFromOption(quoteOption);
            quoteItem.RemoveProductFeatures();
            quoteItem.AddProductFeatureFromOption(quoteOption);
            quoteItem.RemoveServices();
            quoteItem.AddServiceFromOption(quoteOption);
        }
    }
}