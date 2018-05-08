using System;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Web.SessionState.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.MasterPages.Fakes;
using ecn.common.classes.billing;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Accounts.Tests.Includes
{
    [TestFixture]
    public abstract class QuoteItemEditorTestsBase<T> where T : UserControl, new()
    {
        protected internal const string DataTextField = "Name";
        protected internal const string DataValueField = "Code";
        protected internal const string MethodNameBtnAddClick = "btnAdd_Click";
        protected internal const string MethodNameDltEmailOptionsItemCommand = "dltEmailOptions_ItemCommand";
        protected internal const string MethodNameInitializeFrequencyDropDownList = "InitializeFrequencyDropDownList";
        protected internal const string MethodNameFireQuoteItemChangeEvent = "FireQuoteItemChangeEvent";
        protected internal const string MethodNameLoadQuoteItems = "LoadQuoteItems";
        protected internal const string MethodNameCreateQuoteItem = "ConvertQuoteOptionToQuoteItem";
        protected internal const string MethodNameGetQuoteItem = "CopyQuoteOptionToQuoteItem";
        protected internal const string MethodNameBindQuoteOptionDDL = "BindQuoteOptionDDL";
        protected internal const string PropertyNameQuoteOptionsKey = "QuoteOptionsKey";
        protected internal const string PropertyNameQuoteItemsKey = "QuoteItemsKey";
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
        protected internal const string FrequencyNameAnnual = "Annual";
        protected internal const string FrequencyNameQuarterly = "Quarterly";
        protected internal const string FrequencyNameMonthly = "Monthly";
        protected internal const string FrequencyNameOneTime = "One Time";
        protected internal const string CommandNameEdit = "Edit";
        protected internal const string CommandNameUpdate = "Update";
        protected internal const string CommandNameCancel = "Cancel";
        protected internal const string CommandNameDelete = "Delete";

        protected internal const int UserId = 1;
        protected internal const string TestEntityId = "Item Editor ID";
        protected internal const string QuoteSessionStateKey = "QuoteKey";
        protected internal const int QuoteId = 1;
        protected internal const double Rate01 = 3.5;
        protected internal const double Rate02 = 6.5;
        protected internal const double DiscountRate01 = 4.5;
        protected internal const double DiscountRate02 = 9.5;
        protected internal const FrequencyEnum Frequency01 = FrequencyEnum.Weekly;
        protected internal const FrequencyEnum Frequency02 = FrequencyEnum.Monthly;

        protected IDisposable _shimsContext;
        protected T _testEntity;
        protected PrivateObject _testEntityPrivate;
        protected HttpSessionState _httpSessionState;

        protected QuoteItemCollection _quoteItemCollection;
        protected QuoteItem _quoteItem01;
        protected QuoteOptionCollection _quoteOptionCollection;
        protected QuoteOption _quoteOption01;
        protected QuoteOption _quoteOption02;

        protected DataList _dltEmailOptions;
        protected DataListItem _dataListFooterItem;
        protected DataListItem _dataListEditItem;
        protected DropDownList _ddlAddOptions;
        protected LinkButton _btnAdd;
        protected TextBox _txtAddRate;
        protected TextBox _txtAddDiscountRate;
        protected DropDownList _ddlAddFrequency;
        protected DropDownList _ddlEditType;
        protected TextBox _txtEditRate;
        protected TextBox _txtEditDiscountRate;
        protected DropDownList _ddlEditFrequency;

        [SetUp]
        public virtual void SetUp()
        {
            _shimsContext = ShimsContext.Create();

            SetupWebRequirements();
            SetupQuoteOptionCollection();
            SetupQuoteItemCollection();
            SetupTestEntity();
            SetupPageControls();
        }

        [TearDown]
        public virtual void TearDown()
        {
            DisposePageControls();
            _testEntity?.Dispose();
            _shimsContext?.Dispose();
        }

        protected virtual void SetupWebRequirements()
        {
            var sessionStateCollection = new Dictionary<string, object>();
            var shimSessionState = new ShimHttpSessionState();
            shimSessionState.ItemGetString = (argKey) =>
            {
                return sessionStateCollection.ContainsKey(argKey) ? sessionStateCollection[argKey] : null;
            };
            shimSessionState.ItemSetStringObject = (argKey, argValue) =>
            {
                sessionStateCollection[argKey] = argValue;
            };
            _httpSessionState = shimSessionState.Instance;

            ShimUserControl.AllInstances.SessionGet = (userControl) => { return _httpSessionState; };
        }

        protected virtual void SetupQuoteOptionCollection()
        {
            const string quoteOption01Code = "01";
            const string quoteOption01Name = "Option 01";
            const string quoteOption01Description = "Option Description 01";
            const LicenseTypeEnum quoteOption01LicenseType = LicenseTypeEnum.Option;
            const int quoteOption01Quantity = 10;
            const double quoteOption01Rate = 2.5;
            const PriceTypeEnum quoteOption01PriceType = PriceTypeEnum.Recurring;
            const FrequencyEnum quoteOption01Frequency = FrequencyEnum.Monthly;
            const string quoteOption02Code = "02";
            const string quoteOption02Name = "Option 02";
            const string quoteOption02Description = "Option Description 02";
            const LicenseTypeEnum quoteOption02LicenseType = LicenseTypeEnum.AnnualTechAccess;
            const int quoteOption02Quantity = 20;
            const double quoteOption02Rate = 4.5;
            const PriceTypeEnum quoteOption02PriceType = PriceTypeEnum.OneTime;
            const FrequencyEnum quoteOption02Frequency = FrequencyEnum.Annual;

            _quoteOption01 = new QuoteOption(
                quoteOption01Code,
                quoteOption01Name,
                quoteOption01Description,
                quoteOption01LicenseType,
                quoteOption01Quantity,
                quoteOption01Rate,
                quoteOption01PriceType,
                quoteOption01Frequency);

            _quoteOption02 = new QuoteOption(
                quoteOption02Code,
                quoteOption02Name,
                quoteOption02Description,
                quoteOption02LicenseType,
                quoteOption02Quantity,
                quoteOption02Rate,
                quoteOption02PriceType,
                quoteOption02Frequency);

            _quoteOptionCollection = new QuoteOptionCollection();
            _quoteOptionCollection.Add(_quoteOption01);
            _quoteOptionCollection.Add(_quoteOption02);
        }

        protected virtual void SetupQuoteItemCollection()
        {
            const string quoteItem01Code = "01";
            const string quoteItem01Name = "Item 01";
            const string quoteItem01Description = "Item Description 01";
            const LicenseTypeEnum quoteItem01LicenseType = LicenseTypeEnum.EmailBlock;
            const int quoteItem01Quantity = 30;
            const double quoteItem01Rate = 13.5;
            const PriceTypeEnum quoteItem01PriceType = PriceTypeEnum.Usage;
            const FrequencyEnum quoteItem01Frequency = FrequencyEnum.Quarterly;

            _quoteItem01 = new QuoteItem(
                    quoteItem01Frequency,
                    quoteItem01Code,
                    quoteItem01Name,
                    quoteItem01Description,
                    quoteItem01Quantity,
                    quoteItem01Rate,
                    quoteItem01LicenseType,
                    quoteItem01PriceType);

            _quoteItemCollection = new QuoteItemCollection();
            _quoteItemCollection.Add(_quoteItem01);
        }

        protected virtual void SetupTestEntity()
        {
            var shimUser = new ShimUser();
            shimUser.UserIDGet = () => { return UserId; };
            var user = shimUser.Instance;

            var shimEcnSession = new ShimECNSession();
            var ecnSession = shimEcnSession.Instance;
            ecnSession.CurrentUser = user;

            var shimMasterPage = new ShimAccounts();
            shimMasterPage.UserSessionGet = () => { return ecnSession; };
            var masterPage = shimMasterPage.Instance;

            var shimPage = new ShimPage();
            shimPage.MasterGet = () => { return masterPage; };
            var page = shimPage.Instance;

            _testEntity = new T();
            _testEntity.ID = TestEntityId;
            _testEntity.Page = page;
            _testEntityPrivate = new PrivateObject(_testEntity);
        }

        protected virtual void SetupPageControls()
        {
            const int editItemIndex = 0;
            const int footerItemIndex = -1;

            _dltEmailOptions = new DataList();
            _btnAdd = new LinkButton() { ID = ControlNameBtnAdd };
            _ddlAddOptions = new DropDownList() { ID = ControlNameDdlAddOptions };
            _txtAddRate = new TextBox() { ID = ControlNameTxtAddRate };
            _txtAddDiscountRate = new TextBox() { ID = ControlNameTxtAddDiscountRate };
            _ddlAddFrequency = new DropDownList() { ID = ControlNameDdlAddFrequency };
            _ddlEditType = new DropDownList() { ID = ControlNameDdlEditType };
            _txtEditRate = new TextBox() { ID = ControlNameTxtEditRate };
            _txtEditDiscountRate = new TextBox() { ID = ControlNameTxtEditDiscountRate };
            _ddlEditFrequency = new DropDownList() { ID = ControlNameDdlEditFrequency };

            _dataListFooterItem = new DataListItem(footerItemIndex, ListItemType.Footer);
            _dataListFooterItem.Controls.Add(_btnAdd);
            _dataListFooterItem.Controls.Add(_ddlAddOptions);
            _dataListFooterItem.Controls.Add(_ddlAddFrequency);
            _dataListFooterItem.Controls.Add(_txtAddRate);
            _dataListFooterItem.Controls.Add(_txtAddDiscountRate);

            _dataListEditItem = new DataListItem(editItemIndex, ListItemType.EditItem);
            _dataListEditItem.Controls.Add(_ddlEditType);
            _dataListEditItem.Controls.Add(_txtEditRate);
            _dataListEditItem.Controls.Add(_txtEditDiscountRate);
            _dataListEditItem.Controls.Add(_ddlEditFrequency);

            _dltEmailOptions.Controls.Add(_dataListFooterItem);
            _dltEmailOptions.Controls.Add(_dataListEditItem);

            _ddlAddOptions.DataSource = _quoteOptionCollection;
            _ddlAddOptions.DataTextField = DataTextField;
            _ddlAddOptions.DataValueField = DataValueField;
            _ddlAddOptions.DataBind();

            _ddlEditType.DataSource = _quoteOptionCollection;
            _ddlEditType.DataTextField = DataTextField;
            _ddlEditType.DataValueField = DataValueField;
            _ddlEditType.DataBind();

            var addFrequencyListItem01 = new ListItem(Frequency01.ToString(), ((int)Frequency01).ToString());
            var addFrequencyListItem02 = new ListItem(Frequency02.ToString(), ((int)Frequency02).ToString());
            _ddlAddFrequency.Items.Add(addFrequencyListItem01);
            _ddlAddFrequency.Items.Add(addFrequencyListItem02);

            var editFrequencyListItem01 = new ListItem(Frequency01.ToString(), ((int)Frequency01).ToString());
            var editFrequencyListItem02 = new ListItem(Frequency02.ToString(), ((int)Frequency02).ToString());
            _ddlEditFrequency.Items.Add(editFrequencyListItem01);
            _ddlEditFrequency.Items.Add(editFrequencyListItem02);

            _testEntityPrivate.SetField(ControlNameDltEmailOptions, _dltEmailOptions);
        }

        protected virtual void DisposePageControls()
        {
            _dltEmailOptions?.Dispose();
            _dataListFooterItem?.Dispose();
            _dataListEditItem?.Dispose();
            _ddlAddOptions?.Dispose();
            _btnAdd?.Dispose();
            _txtAddRate?.Dispose();
            _txtAddDiscountRate?.Dispose();
            _ddlAddFrequency?.Dispose();
            _ddlEditType?.Dispose();
            _txtEditRate?.Dispose();
            _txtEditDiscountRate?.Dispose();
            _ddlEditFrequency?.Dispose();
        }
    }
}
