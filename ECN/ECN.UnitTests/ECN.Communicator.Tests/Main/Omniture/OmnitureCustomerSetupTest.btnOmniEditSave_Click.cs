using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Omniture;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN.Communicator.Tests.Main.Omniture
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class OmnitureCustomerSetupTestbtnOmniEditSave_Click
    {
        private const int Zero = 0;
        private const string LTPOIDColumn = "LTPOID";
        private const string DisplayNameColumn = "DisplayName";
        private const string ValueColumn = "Value";
        private const string IsDeletedColumn = "IsDeleted";
        private const string IsDynamicColumn = "IsDynamic";
        private const string IsDefaultColumn = "IsDefault";
        private IDisposable _shimsContext;
        private OmnitureCustomerSetup _page;
        private PrivateObject _pagePrivate;
        private LinkTrackingParamSettings _linkTrackingParamSettingsGetResult;
        private User _currentUser;
        private Customer _currentCustomer;
        private TextBox _txtOmniDisplayName;
        private CheckBoxList _chklstDynamicFields;
        private StateBag _viewState;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;
        private ECNException _linkTrackingParamSettingsUpdateException;
        private LinkTrackingParamSettings _linkTrackingParamSettingsUpdated;
        private LinkTrackingParamSettings _linkTrackingParamSettingsInserted;
        private LinkTrackingParamOption _linkTrackingParamOptionInserted;
        private LinkTrackingParamOption _linkTrackingParamOptionGetResult;
        private LinkTrackingParamOption _linkTrackingParamOptionDeleted;
        private List<LinkTrackingParamOption> _linkTrackingParamOptionGetByLTPIDResult;
        private ECNException _linkTrackingParamSettingsInsertException;
        private ECNException _linkTrackingParamOptionInsertException;
        private ECNException _linkTrackingParamOptionDeleteException;
        private readonly BindingFlags _privateFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _page = new OmnitureCustomerSetup();
            _pagePrivate = new PrivateObject(_page);
            _shimsContext = ShimsContext.Create();
            InitializeFields();
            CommonShims();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }
        
        [Test]
        public void btnOmniEditSave_Click_LinkTrackingParamSettingsHasId_ShouldUpdate()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            _currentUser.UserID = GetNumber();
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            _linkTrackingParamSettingsUpdated.ShouldSatisfyAllConditions(
                () => _linkTrackingParamSettingsUpdated.ShouldNotBeNull(),
                () => _linkTrackingParamSettingsUpdated.ShouldBe(_linkTrackingParamSettingsGetResult),
                () => _linkTrackingParamSettingsUpdated.UpdatedUserID.ShouldBe(_currentUser.UserID),
                () => _linkTrackingParamSettingsUpdated.DisplayName.ShouldBe(_txtOmniDisplayName.Text));
        }

        [Test]
        public void btnOmniEditSave_Click_LinkTrackingParamSettingsHasIdAndExceptionThrown_ShouldShowError()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            var exception = GetECNException();
            _linkTrackingParamSettingsUpdateException = exception;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            AssertExceptionDisplayed(exception);
        }

        [Test]
        public void btnOmniEditSave_Click_LinkTrackingParamSettingsHasNoId_ShouldInsert()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = Zero;
            _currentUser.UserID = GetNumber();
            var paramId = GetNumber();
            _viewState["ParamOptionID"] = paramId;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            _linkTrackingParamSettingsInserted.ShouldSatisfyAllConditions(
                () => _linkTrackingParamSettingsInserted.ShouldNotBeNull(),
                () => _linkTrackingParamSettingsInserted.CreatedUserID.ShouldBe(_currentUser.UserID),
                () => _linkTrackingParamSettingsInserted.DisplayName.ShouldBe(_txtOmniDisplayName.Text),
                () => _linkTrackingParamSettingsInserted.LTPID.ShouldBe(paramId));
        }

        [Test]
        public void btnOmniEditSave_Click_LinkTrackingParamSettingsHasNoIdAndExceptionThrown_ShouldShowError()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = Zero;
            var exception = GetECNException();
            _linkTrackingParamSettingsInsertException = exception;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            AssertExceptionDisplayed(exception);
        }

        [Test]
        public void btnOmniEditSave_Click_DynamicFieldsSelected_ShouldAddRow()
        {
            //Arrange
            var item = new ListItem(GetString(), GetString());
            _chklstDynamicFields.Items.Add(item);
            item.Selected = true;
            var table = GetParamOptionsTable();
            _viewState["ParamOptionsDT"] = table;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            table.Rows.Count.ShouldBe(1);
            var row = table.Rows[0];
            row.ShouldSatisfyAllConditions(
                () => row[DisplayNameColumn].ShouldBe(item.Text),
                () => row[ValueColumn].ShouldBe(item.Value));
        }

        [Test]
        public void btnOmniEditSave_Click_DynamicFieldsNotSelected_ShouldSetRowTodDeleted()
        {
            //Arrange
            var item = new ListItem(GetString(), GetString());
            _chklstDynamicFields.Items.Add(item);
            item.Selected = false;
            var table = GetParamOptionsTable();
            var row = table.NewRow();
            row[ValueColumn] = item.Value;
            row[IsDynamicColumn] = true;
            table.Rows.Add(row);
            _viewState["ParamOptionsDT"] = table;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            row[IsDeletedColumn].ShouldBe(bool.TrueString);
        }

        [Test]
        public void btnOmniEditSave_Click_RowsNeedInsert_ShouldBeInserted()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            var table = GetParamOptionsTable();
            var row = table.NewRow();
            row[IsDeletedColumn] = false;
            row[IsDefaultColumn] = true;
            row[LTPOIDColumn] = GetString();
            row[ValueColumn] = GetString();
            row[DisplayNameColumn] = GetString();
            row[IsDynamicColumn] = true;
            table.Rows.Add(row);
            _viewState["ParamOptionsDT"] = table;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            _linkTrackingParamOptionInserted.ShouldSatisfyAllConditions(
                () => _linkTrackingParamOptionInserted.ShouldNotBeNull(),
                () => _linkTrackingParamOptionInserted.Value.ShouldBe(row[ValueColumn]),
                () => _linkTrackingParamOptionInserted.DisplayName.ShouldBe(row[DisplayNameColumn]),
                () => _linkTrackingParamOptionInserted.IsDefault.ToString().ShouldBe(row[IsDefaultColumn]),
                () => _linkTrackingParamOptionInserted.IsDynamic.ToString().ShouldBe(row[IsDynamicColumn]),
                () => _linkTrackingParamOptionInserted.CustomerID.ShouldBe(_currentUser.CustomerID),
                () => _linkTrackingParamOptionInserted.CreatedUserID.ShouldBe(_currentUser.UserID),
                () => _linkTrackingParamOptionInserted.IsActive.ShouldBe(true));
        }

        [Test]
        public void btnOmniEditSave_Click_RowsNeedInsertWhenException_ShouldShowError()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            var table = GetParamOptionsTable();
            var row = table.NewRow();
            row[IsDeletedColumn] = false;
            row[IsDefaultColumn] = true;
            row[LTPOIDColumn] = GetString();
            row[ValueColumn] = GetString();
            row[DisplayNameColumn] = GetString();
            row[IsDynamicColumn] = true;
            table.Rows.Add(row);
            _viewState["ParamOptionsDT"] = table;
            var exception = GetECNException();
            _linkTrackingParamOptionInsertException = exception;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            AssertExceptionDisplayed(exception);
        }

        [Test]
        public void btnOmniEditSave_Click_RowsNeedDelete_ShouldBeDeleted()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            var table = GetParamOptionsTable();
            var row = table.NewRow();
            row[IsDeletedColumn] = true;
            row[LTPOIDColumn] = GetNumber();
            table.Rows.Add(row);
            _viewState["ParamOptionsDT"] = table;
            _currentUser.UserID = GetNumber();
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            _linkTrackingParamOptionDeleted.ShouldSatisfyAllConditions(
                () => _linkTrackingParamOptionDeleted.ShouldBe(_linkTrackingParamOptionGetResult),
                () => _linkTrackingParamOptionDeleted.IsDeleted.ShouldBeTrue(),
                () => _linkTrackingParamOptionDeleted.IsActive.GetValueOrDefault().ShouldBeFalse(),
                () => _linkTrackingParamOptionDeleted.UpdatedUserID.ShouldBe(_currentUser.UserID));
        }

        [Test]
        public void btnOmniEditSave_Click_RowsNeedDeleteWhenException_ShouldShowError()
        {
            //Arrange
            _linkTrackingParamSettingsGetResult.LTPSID = GetNumber();
            var table = GetParamOptionsTable();
            var row = table.NewRow();
            row[IsDeletedColumn] = true;
            row[LTPOIDColumn] = GetNumber();
            table.Rows.Add(row);
            _viewState["ParamOptionsDT"] = table;
            _currentUser.UserID = GetNumber();
            var exception = GetECNException();
            _linkTrackingParamOptionDeleteException = exception;
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            AssertExceptionDisplayed(exception);
        }

        [Test]
        [TestCaseSource(nameof(GetLabels))]
        public void btnOmniEditSave_Click_LabelId_ShouldUpdateRelevantControls(string label)
        {
            //Arrange
            const string NumberGroup = "Number";
            _viewState["LabelID"] = label;
            var numberGroup = Regex
                .Match(label, $@"imgbtnOmni(?<{NumberGroup}>[\d]+)")
                .Groups[NumberGroup];
            numberGroup.Success.ShouldBeTrue();
            var number = numberGroup.Value;
            var lblOmniture = GetRefrenceField<Label>($"lblOmniture{number}");
            var ddlOmniDefault = GetRefrenceField<DropDownList>($"ddlOmniDefault{number}");
            var selected = new LinkTrackingParamOption
            {
                IsDefault = true,
                LTPOID = GetNumber()
            };
            _linkTrackingParamOptionGetByLTPIDResult.Add(selected);
            //Act
            CallbtnOmniEditSave_Click();
            //Assert
            ddlOmniDefault.ShouldSatisfyAllConditions(
                () => ddlOmniDefault.Items
                    .OfType<ListItem>()
                    .ShouldContain(item =>
                        item.Text == "-Select-" &&
                        item.Value == "-1"),
                () => ddlOmniDefault.DataSource.ShouldBe(_linkTrackingParamOptionGetByLTPIDResult),
                () => ddlOmniDefault.DataTextField.ShouldBe("DisplayName"),
                () => ddlOmniDefault.DataValueField.ShouldBe("LTPOID"),
                () => ddlOmniDefault.SelectedValue.ShouldBe(selected.LTPOID.ToString()));
        }

        private static IEnumerable<string> GetLabels()
        {
            return Enumerable.Range(1, 10)
                .Select(i => $"imgbtnOmni{i}");
        }

        private ECNException GetECNException()
        {
            var error = new ECNError
            {
                ErrorMessage = GetString(),
                Entity = Entity.BlastABMaster
            };
            var errors = new List<ECNError> { error };
            var exception = new ECNException(errors);
            return exception;
        }

        private DataTable GetParamOptionsTable()
        {
            var result = new DataTable();
            var columns = new[]
            {
                LTPOIDColumn,
                DisplayNameColumn,
                ValueColumn,
                IsDeletedColumn,
                IsDynamicColumn,
                IsDefaultColumn
            };
            foreach (var column in columns)
            {
                result.Columns.Add(column);
            }
            return result;
        }

        private void AssertExceptionDisplayed(ECNException linkTrackingParamSettingsUpdateException)
        {
            _phError.Visible.ShouldBeTrue();
            var messageBuilder = new StringBuilder();
            foreach (var error in linkTrackingParamSettingsUpdateException.ErrorList)
            {
                messageBuilder.Append($"<br/>{error.Entity}: {error.ErrorMessage}");
            }
            _lblErrorMessage.Text.ShouldBe(messageBuilder.ToString());
        }

        private void ThrowExceptionAndReset(ref ECNException exception)
        {
            if (exception != null)
            {
                var tempException = exception;
                exception = null;
                throw tempException;
            }
        }

        private void CommonShims()
        {
            ShimECNSession.Constructor = instance => { };
            var session = GetSession();
            ShimECNSession.CurrentSession = () => session;
            _linkTrackingParamSettingsGetResult = new LinkTrackingParamSettings();
            ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (ltpId, customerId) =>
            {
                return _linkTrackingParamSettingsGetResult;
            };
            ShimLinkTrackingParamSettings.InsertLinkTrackingParamSettings = linkTrackingParamSettings =>
            {
                ThrowExceptionAndReset(ref _linkTrackingParamSettingsInsertException);
                linkTrackingParamSettings.ShouldNotBeNull();
                _linkTrackingParamSettingsInserted = linkTrackingParamSettings;
                _linkTrackingParamSettingsInserted.LTPSID = GetNumber();
                return _linkTrackingParamSettingsInserted.LTPSID;
            };
            ShimLinkTrackingParamSettings.UpdateLinkTrackingParamSettings = linkTrackingParamSettigns =>
            {
                ThrowExceptionAndReset(ref _linkTrackingParamSettingsUpdateException);
                _linkTrackingParamSettingsUpdated = linkTrackingParamSettigns;
            };
            ShimLinkTrackingParamOption.InsertLinkTrackingParamOption = linkTrackingParamOption =>
            {
                ThrowExceptionAndReset(ref _linkTrackingParamOptionInsertException);
                linkTrackingParamOption.ShouldNotBeNull();
                _linkTrackingParamOptionInserted = linkTrackingParamOption;
                _linkTrackingParamOptionInserted.LTPID = GetNumber();
                return _linkTrackingParamOptionInserted.LTPID;
            };
            _linkTrackingParamOptionGetResult = new LinkTrackingParamOption();
            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = ltpoId =>
            {
                return _linkTrackingParamOptionGetResult;
            };
            ShimLinkTrackingParamOption.DeleteLinkTrackingParamOption = linkTrackingParamOption =>
            {
                ThrowExceptionAndReset(ref _linkTrackingParamOptionDeleteException);
                _linkTrackingParamOptionDeleted = linkTrackingParamOption;
            };
            _linkTrackingParamOptionGetByLTPIDResult = new List<LinkTrackingParamOption>();
            ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (paramId, customerId) =>
            {
                return _linkTrackingParamOptionGetByLTPIDResult;
            };
        }

        private ECNSession GetSession()
        {
            var result = CreateInstance(typeof(ECNSession)) as ECNSession;
            _currentUser = new User();
            result.CurrentUser = _currentUser;
            _currentCustomer = new Customer();
            result.CurrentCustomer = _currentCustomer;
            return result;
        }

        private void InitializeFields()
        {
            var fields = _page.GetType()
                .GetFields(_privateFlags)
                .Where(field => field.GetValue(_page) == null)
                .ToList();
            foreach (var field in fields)
            {
                var value = CreateInstance(field.FieldType);
                field.SetValue(_page, value);
            }
            _txtOmniDisplayName = GetRefrenceField<TextBox>("txtOmniDisplayName");
            _chklstDynamicFields = GetRefrenceField<CheckBoxList>("chklstDynamicFields");
            _viewState = GetRefrenceProperty<StateBag>("ViewState");
            _lblErrorMessage = GetRefrenceField<Label>("lblErrorMessage");
            _phError = GetRefrenceField<PlaceHolder>("phError");
        }

        private T GetRefrenceField<T>(string fieldName) where T : class
        {
            var result = _pagePrivate.GetField(fieldName) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private T GetRefrenceProperty<T>(string fieldName) where T : class
        {
            var result = _pagePrivate.GetProperty(fieldName) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private object CreateInstance(Type type)
        {
            return type
                .GetConstructor(_privateFlags, null, new Type[0], null)
                ?.Invoke(new object[0]);
        }

        private void CallbtnOmniEditSave_Click()
        {
            _pagePrivate.Invoke("btnOmniEditSave_Click", new object[] { null, null });
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }
    } 
}
