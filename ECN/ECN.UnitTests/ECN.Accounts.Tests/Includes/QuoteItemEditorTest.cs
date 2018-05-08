using System;
using System.Linq;
using System.Web.UI.WebControls;
using ecn.accounts.includes;
using ecn.accounts.includes.Fakes;
using ecn.common.classes.billing;
using ecn.common.classes.billing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Accounts.Tests.Includes
{
    [TestFixture]
    public class QuoteItemEditorTest : QuoteItemEditorTestsBase<QuoteItemEditor>
    {
        [Test]
        public void CurrentQuoteStatusGetter_IfQueryStringHasValue_ReturnsQuoteStatus()
        {
            // Arrange
            var quote = new Quote(QuoteId)
            {
                Status = QuoteStatusEnum.Approved
            };
            _httpSessionState[QuoteSessionStateKey] = quote;

            // Act
            var returnedValue = _testEntity.CurrentQuoteStatus;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldNotBeEmpty(),
                () => returnedValue.ShouldBe(quote.Status.ToString()));
        }

        [Test]
        public void CurrentQuoteStatusGetter_IfQueryStringDoesNotHaveValue_ReturnsEmptyString()
        {
            // Arrange
            _httpSessionState[QuoteSessionStateKey] = null;

            // Act
            var returnedValue = _testEntity.CurrentQuoteStatus;

            // Assert
            returnedValue.ShouldBeEmpty();
        }

        [Test]
        public void BtnAddClick_IfQuoteStatusDoesNotEqualToApproved_AddsQuoteItem()
        {
            // Arrange
            var optionsListItem02 = _ddlAddOptions.Items.FindByValue(_quoteOption02.Code);
            if (optionsListItem02 != null)
            {
                optionsListItem02.Selected = true;
            }

            var frequencyListItem02 = _ddlAddFrequency.Items.FindByValue(((int)Frequency02).ToString());
            if (frequencyListItem02 != null)
            {
                frequencyListItem02.Selected = true;
            }

            _txtAddRate.Text = Rate02.ToString();
            _txtAddDiscountRate.Text = DiscountRate02.ToString();

            var loadQuoteItemsCalled = false;
            ShimQuoteItemEditorBase.AllInstances.LoadQuoteItemsQuoteItemCollection = (_, __) =>
            {
                loadQuoteItemsCalled = true;
            };

            var fireQuoteItemChangeEventCalled = false;
            ShimQuoteItemEditorBase.AllInstances.FireQuoteItemChangeEvent = (_) =>
            {
                fireQuoteItemChangeEventCalled = true;
            };

            var pendingQuote = new Quote(QuoteId) { Status = QuoteStatusEnum.Pending };
            _httpSessionState[QuoteSessionStateKey] = pendingQuote;

            var expectedCount = _quoteItemCollection.Count + 1;
            var expectedIndex = expectedCount - 1;

            // Act
            _testEntityPrivate.Invoke(MethodNameBtnAddClick, _btnAdd, EventArgs.Empty);

            // Assert
            _quoteItemCollection.ShouldSatisfyAllConditions(
                () => _quoteItemCollection.Count.ShouldBe(expectedCount),
                () => _quoteItemCollection[expectedIndex].Name.ShouldBe(_quoteOption02.Name),
                () => _quoteItemCollection[expectedIndex].Code.ShouldBe(_quoteOption02.Code),
                () => _quoteItemCollection[expectedIndex].Description.ShouldBe(_quoteOption02.Description),
                () => _quoteItemCollection[expectedIndex].Quantity.ShouldBe(_quoteOption02.Quantity),
                () => _quoteItemCollection[expectedIndex].LicenseType.ShouldBe(_quoteOption02.LicenseType),
                () => _quoteItemCollection[expectedIndex].PriceType.ShouldBe(_quoteOption02.PriceType),
                () => _quoteItemCollection[expectedIndex].Rate.ShouldBe(Rate02),
                () => _quoteItemCollection[expectedIndex].Frequency.ShouldBe(Frequency02),
                () => _quoteItemCollection[expectedIndex].DiscountRate.ShouldBe(DiscountRate02 / 100),
                () => fireQuoteItemChangeEventCalled.ShouldBeTrue(),
                () => loadQuoteItemsCalled.ShouldBeTrue());
        }

        [Test]
        public void DltEmailOptionsItemCommand_IfQuoteStatusDoesNotEqualToApprovedAndCommandNameIsEdit_SetsItemIndex()
        {
            // Arrange
            var loadQuoteItemsCalled = false;
            ShimQuoteItemEditorBase.AllInstances.LoadQuoteItemsQuoteItemCollection = (_, __) =>
            {
                loadQuoteItemsCalled = true;
            };

            var originalEventArgs = new CommandEventArgs(CommandNameEdit, null);
            var eventArgs = new DataListCommandEventArgs(_dataListEditItem, null, originalEventArgs);

            // Act
            _testEntityPrivate.Invoke(MethodNameDltEmailOptionsItemCommand, null, eventArgs);

            // Assert
            _dltEmailOptions.ShouldSatisfyAllConditions(
                () => _dltEmailOptions.EditItemIndex.ShouldBe(_dataListEditItem.ItemIndex),
                () => loadQuoteItemsCalled.ShouldBeTrue());
        }

        [Test]
        public void DltEmailOptionsItemCommand_IfQuoteStatusDoesNotEqualToApprovedAndCommandNameIsUpdate_UpdatesQuoteItemFieldsAndFiresEvent()
        {
            // Arrange
            var editOptionsListItem02 = _ddlEditType.Items.FindByValue(_quoteOption02.Code);
            if (editOptionsListItem02 != null)
            {
                editOptionsListItem02.Selected = true;
            }

            var editFrequencyListItem02 = _ddlEditFrequency.Items.FindByValue(((int)Frequency02).ToString());
            if (editFrequencyListItem02 != null)
            {
                editFrequencyListItem02.Selected = true;
            }

            _txtEditRate.Text = Rate02.ToString();
            _txtEditDiscountRate.Text = DiscountRate02.ToString();

            var loadQuoteItemsCalled = false;
            ShimQuoteItemEditorBase.AllInstances.LoadQuoteItemsQuoteItemCollection = (_, __) =>
            {
                loadQuoteItemsCalled = true;
            };

            var fireQuoteItemChangeEventCalled = false;
            ShimQuoteItemEditorBase.AllInstances.FireQuoteItemChangeEvent = (_) =>
            {
                fireQuoteItemChangeEventCalled = true;
            };

            var expectedCount = _quoteItemCollection.Count;
            var expectedIndex = _dataListEditItem.ItemIndex;
            var originalEventArgs = new CommandEventArgs(CommandNameUpdate, null);
            var eventArgs = new DataListCommandEventArgs(_dataListEditItem, null, originalEventArgs);

            // Act
            _testEntityPrivate.Invoke(MethodNameDltEmailOptionsItemCommand, null, eventArgs);

            // Assert
            _quoteItemCollection.ShouldSatisfyAllConditions(
                () => _quoteItemCollection.Count.ShouldBe(expectedCount),
                () => _quoteItemCollection[expectedIndex].Name.ShouldBe(_quoteOption02.Name),
                () => _quoteItemCollection[expectedIndex].Code.ShouldBe(_quoteOption02.Code),
                () => _quoteItemCollection[expectedIndex].Description.ShouldBe(_quoteOption02.Description),
                () => _quoteItemCollection[expectedIndex].Rate.ShouldBe(Rate02),
                () => _quoteItemCollection[expectedIndex].Frequency.ShouldBe(Frequency02),
                () => _quoteItemCollection[expectedIndex].DiscountRate.ShouldBe(DiscountRate02 / 100),
                () => _dltEmailOptions.EditItemIndex.ShouldBe(-1),
                () => fireQuoteItemChangeEventCalled.ShouldBeTrue(),
                () => loadQuoteItemsCalled.ShouldBeTrue());
        }

        [Test]
        public void DltEmailOptionsItemCommand_IfQuoteStatusDoesNotEqualToApprovedAndCommandNameIsCancel_SetsItemIndex()
        {
            // Arrange
            var loadQuoteItemsCalled = false;
            ShimQuoteItemEditorBase.AllInstances.LoadQuoteItemsQuoteItemCollection = (_, __) =>
            {
                loadQuoteItemsCalled = true;
            };

            var originalEventArgs = new CommandEventArgs(CommandNameCancel, null);
            var eventArgs = new DataListCommandEventArgs(_dataListEditItem, null, originalEventArgs);

            // Act
            _testEntityPrivate.Invoke(MethodNameDltEmailOptionsItemCommand, null, eventArgs);

            // Assert
            _dltEmailOptions.ShouldSatisfyAllConditions(
                () => _dltEmailOptions.EditItemIndex.ShouldBe(-1),
                () => loadQuoteItemsCalled.ShouldBeTrue());
        }

        [Test]
        public void DltEmailOptionsItemCommand_IfQuoteStatusDoesNotEqualToApprovedAndCommandNameIsDelete_RemovesQuoteItemAndFiresEvent()
        {
            // Arrange
            var deleteCalled = false;
            ShimQuoteItem.AllInstances.DeleteInt32 = (_, __) =>
            {
                deleteCalled = true;
            };

            var loadQuoteItemsCalled = false;
            ShimQuoteItemEditorBase.AllInstances.LoadQuoteItemsQuoteItemCollection = (_, __) =>
            {
                loadQuoteItemsCalled = true;
            };

            var fireQuoteItemChangeEventCalled = false;
            ShimQuoteItemEditorBase.AllInstances.FireQuoteItemChangeEvent = (_) =>
            {
                fireQuoteItemChangeEventCalled = true;
            };

            var initialQuoteItemCount = _quoteItemCollection.Count;
            var originalEventArgs = new CommandEventArgs(CommandNameDelete, null);
            var eventArgs = new DataListCommandEventArgs(_dataListEditItem, null, originalEventArgs);

            // Act
            _testEntityPrivate.Invoke(MethodNameDltEmailOptionsItemCommand, null, eventArgs);

            // Assert
            _quoteItemCollection.ShouldSatisfyAllConditions(
                () => _quoteItemCollection.Count.ShouldBe(initialQuoteItemCount - 1),
                () => deleteCalled.ShouldBeTrue(),
                () => fireQuoteItemChangeEventCalled.ShouldBeTrue(),
                () => loadQuoteItemsCalled.ShouldBeTrue());
        }

        [Test]
        public void InitializeFrequencyDropDownList_IfAnnualFrequencyIsAllowed_AddsAnnualFrequencyToDropDownList()
        {
            // Arrange
            var shimQuoteOption = new ShimQuoteOption();
            shimQuoteOption.IsAllowedFrequencyEnum = (argFrequency) =>
            {
                return argFrequency == FrequencyEnum.Annual;
            };

            var quoteOption = shimQuoteOption.Instance;
            var expectedCount = 1;
            var expectedIndex = 0;

            // Act
            _testEntityPrivate.Invoke(MethodNameInitializeFrequencyDropDownList, _ddlAddFrequency, quoteOption);

            // Assert
            _ddlAddFrequency.ShouldSatisfyAllConditions(
                () => _ddlAddFrequency.Items.Count.ShouldBe(expectedCount),
                () => _ddlAddFrequency.Items[expectedIndex].Text.ShouldBe(FrequencyNameAnnual),
                () => _ddlAddFrequency.Items[expectedIndex].Value.ShouldBe(((int)FrequencyEnum.Annual).ToString()));
        }

        [Test]
        public void InitializeFrequencyDropDownList_IfQuarterlyFrequencyIsAllowed_AddsQuarterlyFrequencyToDropDownList()
        {
            // Arrange
            var shimQuoteOption = new ShimQuoteOption();
            shimQuoteOption.IsAllowedFrequencyEnum = (argFrequency) =>
            {
                return argFrequency == FrequencyEnum.Quarterly;
            };

            var quoteOption = shimQuoteOption.Instance;
            var expectedCount = 1;
            var expectedIndex = 0;

            // Act
            _testEntityPrivate.Invoke(MethodNameInitializeFrequencyDropDownList, _ddlAddFrequency, quoteOption);

            // Assert
            _ddlAddFrequency.ShouldSatisfyAllConditions(
                () => _ddlAddFrequency.Items.Count.ShouldBe(expectedCount),
                () => _ddlAddFrequency.Items[expectedIndex].Text.ShouldBe(FrequencyNameQuarterly),
                () => _ddlAddFrequency.Items[expectedIndex].Value.ShouldBe(((int)FrequencyEnum.Quarterly).ToString()));
        }

        [Test]
        public void InitializeFrequencyDropDownList_IfMonthlyFrequencyIsAllowed_AddsMonthlyFrequencyToDropDownList()
        {
            // Arrange
            var shimQuoteOption = new ShimQuoteOption();
            shimQuoteOption.IsAllowedFrequencyEnum = (argFrequency) =>
            {
                return argFrequency == FrequencyEnum.Monthly;
            };

            var quoteOption = shimQuoteOption.Instance;
            var expectedCount = 1;
            var expectedIndex = 0;

            // Act
            _testEntityPrivate.Invoke(MethodNameInitializeFrequencyDropDownList, _ddlAddFrequency, quoteOption);

            // Assert
            _ddlAddFrequency.ShouldSatisfyAllConditions(
                () => _ddlAddFrequency.Items.Count.ShouldBe(expectedCount),
                () => _ddlAddFrequency.Items[expectedIndex].Text.ShouldBe(FrequencyNameMonthly),
                () => _ddlAddFrequency.Items[expectedIndex].Value.ShouldBe(((int)FrequencyEnum.Monthly).ToString()));
        }

        [Test]
        public void InitializeFrequencyDropDownList_IfOneTimeFrequencyIsAllowed_AddsOneTimeFrequencyToDropDownList()
        {
            // Arrange
            var shimQuoteOption = new ShimQuoteOption();
            shimQuoteOption.IsAllowedFrequencyEnum = (argFrequency) =>
            {
                return argFrequency == FrequencyEnum.OneTime;
            };

            var quoteOption = shimQuoteOption.Instance;
            var expectedCount = 1;
            var expectedIndex = 0;

            // Act
            _testEntityPrivate.Invoke(MethodNameInitializeFrequencyDropDownList, _ddlAddFrequency, quoteOption);

            // Assert
            _ddlAddFrequency.ShouldSatisfyAllConditions(
                () => _ddlAddFrequency.Items.Count.ShouldBe(expectedCount),
                () => _ddlAddFrequency.Items[expectedIndex].Text.ShouldBe(FrequencyNameOneTime),
                () => _ddlAddFrequency.Items[expectedIndex].Value.ShouldBe(((int)FrequencyEnum.OneTime).ToString()));
        }

        [Test]
        public void InitializeFrequencyDropDownList_IfAllFrequenciesAreAllowed_Adds4FrequenciesToDropDownList()
        {
            // Arrange
            var shimQuoteOption = new ShimQuoteOption();
            shimQuoteOption.IsAllowedFrequencyEnum = (argFrequency) =>
            {
                return true;
            };

            var quoteOption = shimQuoteOption.Instance;
            var expectedCount = 4;

            // Act
            _testEntityPrivate.Invoke(MethodNameInitializeFrequencyDropDownList, _ddlAddFrequency, quoteOption);

            // Assert
            _ddlAddFrequency.ShouldSatisfyAllConditions(
                () => _ddlAddFrequency.Items.Count.ShouldBe(expectedCount),
                () => _ddlAddFrequency.Items.Cast<ListItem>().ShouldContain(item => item.Text == FrequencyNameAnnual),
                () => _ddlAddFrequency.Items.Cast<ListItem>().ShouldContain(item => item.Text == FrequencyNameQuarterly),
                () => _ddlAddFrequency.Items.Cast<ListItem>().ShouldContain(item => item.Text == FrequencyNameMonthly),
                () => _ddlAddFrequency.Items.Cast<ListItem>().ShouldContain(item => item.Text == FrequencyNameOneTime));
        }

        [Test]
        public void FireQuoteItemChangeEvent_IfOnQuoteItemAddedEventIsNotNull_TriggersEvent()
        {
            // Arrange
            var eventMethodCalled = false;
            var eventMethod = new EventHandler((sender, eventArgs) => { eventMethodCalled = true; });
            _testEntity.OnQuoteItemAdded += eventMethod;

            // Act
            _testEntityPrivate.Invoke(MethodNameFireQuoteItemChangeEvent);

            // Assert
            eventMethodCalled.ShouldBeTrue();
        }

        [Test]
        public void QuoteOptionsKeyGetter_ReturnsQuoteOptionsKey()
        {
            // Arrange
            const string quoteOptionsKeyPattern = "{0}_QuoteOptionsKey";
            var expectedKey = string.Format(quoteOptionsKeyPattern, _testEntity.ID);

            // Act
            var returnedValue = _testEntityPrivate.GetProperty(PropertyNameQuoteOptionsKey) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBe(expectedKey));
        }

        [Test]
        public void QuoteItemsKeyGetter_ReturnsQuoteItemsKey()
        {
            // Arrange
            const string quoteItemsKeyPattern = "{0}_QuoteItemsKey";
            var expectedKey = string.Format(quoteItemsKeyPattern, _testEntity.ID);

            // Act
            var returnedValue = _testEntityPrivate.GetProperty(PropertyNameQuoteItemsKey) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBe(expectedKey));
        }

        [Test]
        public void LoadQuoteItems_WhenCalled_LoadsDataList()
        {
            // Arrange & Act
            _testEntityPrivate.Invoke(MethodNameLoadQuoteItems, _quoteItemCollection);

            // Assert
            _dltEmailOptions.DataSource.ShouldBeSameAs(_quoteItemCollection);
        }

        [Test]
        public void ConvertQuoteOptionToQuoteItem_WhenCalled_CreatesQuoteItemFromQuoteOption()
        {
            // Arrange & Act
            var returnedValue = _testEntityPrivate.Invoke(MethodNameCreateQuoteItem, _quoteOption01) as QuoteItem;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<QuoteItem>(),
                () => returnedValue.Code.ShouldBe(_quoteOption01.Code),
                () => returnedValue.Name.ShouldBe(_quoteOption01.Name),
                () => returnedValue.Description.ShouldBe(_quoteOption01.Description),
                () => returnedValue.Quantity.ShouldBe(_quoteOption01.Quantity),
                () => returnedValue.LicenseType.ShouldBe(_quoteOption01.LicenseType),
                () => returnedValue.PriceType.ShouldBe(_quoteOption01.PriceType));
        }

        [Test]
        public void BindQuoteOptionDDL_IfSelectedValueDoesNotExist_BindsQuoteItems()
        {
            // Arrange & Act
            _testEntityPrivate.Invoke(MethodNameBindQuoteOptionDDL, _ddlAddOptions, string.Empty);

            // Assert
            _ddlAddOptions.ShouldSatisfyAllConditions(
                () => _ddlAddOptions.DataSource.ShouldBeSameAs(_quoteOptionCollection),
                () => _ddlAddOptions.DataTextField.ShouldBe(DataTextField),
                () => _ddlAddOptions.DataValueField.ShouldBe(DataValueField));
        }

        [Test]
        public void BindQuoteOptionDDL_IfSelectedValueExists_BindsQuoteItemsAndSelectsPreferred()
        {
            // Arrange & Act
            _testEntityPrivate.Invoke(MethodNameBindQuoteOptionDDL, _ddlAddOptions, _quoteOption01.Code);

            // Assert
            _ddlAddOptions.ShouldSatisfyAllConditions(
                () => _ddlAddOptions.DataSource.ShouldBeSameAs(_quoteOptionCollection),
                () => _ddlAddOptions.DataTextField.ShouldBe(DataTextField),
                () => _ddlAddOptions.DataValueField.ShouldBe(DataValueField),
                () => _ddlAddOptions.SelectedItem.ShouldNotBeNull(),
                () => _ddlAddOptions.SelectedItem.Value.ShouldBe(_quoteOption01.Code));
        }

        [Test]
        public void CopyQuoteOptionToQuoteItem_WhenCalled_CopiesFieldsFromQuoteOptionToQuoteItem()
        {
            // Arrange
            var quoteItem = new QuoteItem(
                FrequencyEnum.Annual,
                string.Empty,
                string.Empty,
                string.Empty,
                long.MinValue,
                double.MinValue,
                LicenseTypeEnum.AnnualTechAccess,
                PriceTypeEnum.OneTime);

            // Act
            _testEntityPrivate.Invoke(MethodNameGetQuoteItem, quoteItem, _quoteOption01);

            // Assert
            quoteItem.ShouldSatisfyAllConditions(
                () => quoteItem.Code.ShouldBe(_quoteOption01.Code),
                () => quoteItem.Name.ShouldBe(_quoteOption01.Name),
                () => quoteItem.Description.ShouldBe(_quoteOption01.Description));
        }

        protected override void SetupQuoteItemCollection()
        {
            base.SetupQuoteItemCollection();

            ShimQuoteItemEditorBase.AllInstances.QuoteItemsGet = (_) => { return _quoteItemCollection; };
        }

        protected override void SetupQuoteOptionCollection()
        {
            base.SetupQuoteOptionCollection();

            ShimQuoteItemEditorBase.AllInstances.QuoteOptionsGet = (_) => { return _quoteOptionCollection; };
        }
    }
}
