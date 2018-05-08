using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using KmpsEnums = KMPS.MD.Objects.Enums;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DownloadPanelVTests : BaseControlTests
    {
        private const string MethodBtnAdd = "BtnAdd_Click";
        private const string MethodBtnRemove = "BtnRemove_Click";
        private const string MethodBtnUp = "btnUp_Click";
        private const string MethodBtnDown = "btndown_Click";
        private const string MethodDrpIsBillableOnSelectedIndexChanged = "DrpIsBillable_OnSelectedIndexChanged";
        private const string MethodBtnCloseExportClick = "btnCloseExport_Click";
        private const string MethodResetPopupControls = "ResetPopupControls";
        private const string MethodRbDownloadCheckedChanged = "rbDownload_CheckedChanged";

        private const string ListAvailableProfileFields = "lstAvailableProfileFields";
        private const string ListAvailableDemoFields = "lstAvailableDemoFields";
        private const string ListAvailableAdhocFields = "lstAvailableAdhocFields";
        private const string ItemTextDefault = "TEST(Default)";
        private const string ItemText = "TEST";
        private const string ItemValueDefault = "Test|Varchar|Default";
        private const string ItemValue = "Fake|Int|None";
        private const string FieldValue1 = "1";
        private const string FieldValue2 = "2";
        private const string ItemValueVarchar = "Test|Varchar";
        private const string ItemValueInt = "Fake|Int";
        private const string FieldTestValue = "Test";
        private const string PhProfileFields = "phProfileFields";
        private const string PhDemoFields = "phDemoFields";
        private const string ListSelectedFields = "lstSelectedFields";
        private const string ItemValueDefaultUpper = "TEST|Varchar|Default";
        private const string ItemValueNoTestUpper = "NOTEST|Varchar|Default";
        private const string ItemSampleName0 = "item0";
        private const string ItemSampleValue0 = "0";
        private const string ItemSampleName1 = "item1";
        private const string DropDownIsBillable = "drpIsBillable";
        private const string ButtonExports = "btnExport";
        private const string PlaceHolderNotes = "plNotes";
        private const string ScriptReturn = "return confirmPopupPurchase();";
        private const string ModulePopupExtenderDownloads = "mdlDownloads";
        private const string PlaceHolderShowHeader = "phShowHeader";
        private const string ChkBoxShowHeader = "cbShowHeader";
        private const string MessageDownload = "Download";
        private const string BtnExportTestClick = "BtnExport_click";
        private const string TxtBoxNotes = "txtNotes";
        private const string MethodDisplayError = "DisplayError";
        private const string ErrorSample = "Error123";
        private const string LblErrorMessage = "lblErrorMessage";
        private const string DivError = "divError";
        private const string SessionPropertyUserId = "UserID";
        private const int UserIdSampleValue = 1;

        private DownloadPanelV _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new DownloadPanel_CLV();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [TestCase(ListAvailableProfileFields, true, false, true, ItemTextDefault, ItemValueDefault)]
        [TestCase(ListAvailableDemoFields, false, true, true, ItemTextDefault, ItemValueDefault)]
        [TestCase(ListAvailableAdhocFields, false, false, true, ItemTextDefault, ItemValueDefault)]
        [TestCase(ListAvailableProfileFields, true, false, false, ItemText, ItemValue)]
        [TestCase(ListAvailableDemoFields, false, true, false, ItemText, ItemValue)]
        [TestCase(ListAvailableAdhocFields, false, false, false, ItemText, ItemValue)]
        public void BtnAddClick_SrcList_DstList(
            string srcListBoxName, 
            bool profileFieldsVisible,
            bool demoFieldsVisible,
            bool isValueVarchar,
            string expectedText,
            string expectedValue)
        {
            // Arrange
            using (var listBoxSrc = new ListBox())
            {
                var itemValue = isValueVarchar ? ItemValueVarchar : ItemValueInt;
                var itemSrcFields = new ListItem(FieldTestValue, itemValue, true);
                itemSrcFields.Selected = true;
                listBoxSrc.Items.Add(itemSrcFields);
                PrivateControl.SetField(
                    srcListBoxName,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    listBoxSrc);

                var placeHolderProfileFields = (PlaceHolder) PrivateControl.GetField(
                    PhProfileFields,
                    BindingFlags.Instance | BindingFlags.NonPublic);
                placeHolderProfileFields.Visible = profileFieldsVisible;
                var placeHolderDemoFields = (PlaceHolder) PrivateControl.GetField(
                    PhDemoFields,
                    BindingFlags.Instance | BindingFlags.NonPublic);
                placeHolderDemoFields.Visible = demoFieldsVisible;

                // Act
                ReflectionHelper.CallMethod(_testEntity, MethodBtnAdd, null, null);

                // Assert
                var lstSelectedFields = PrivateControl.GetField(
                    ListSelectedFields,
                    BindingFlags.Instance | BindingFlags.NonPublic) as ListBox;

                lstSelectedFields.ShouldNotBeNull();

                listBoxSrc.Items.Count.ShouldBe(0);
                lstSelectedFields.ShouldNotBeNull();
                var items = lstSelectedFields.Items;
                items.ShouldNotBeNull();
                items.ShouldSatisfyAllConditions(
                    () => items.Count.ShouldBe(UserIdSampleValue),
                    () => items[0].Text.ShouldBe(expectedText),
                    () => items[0].Value.ShouldBe(expectedValue));
            }
        }

        [TestCase(true, false, true, ListAvailableDemoFields)]
        [TestCase(false, true, true, ListAvailableAdhocFields)]
        [TestCase(false, false, true, ListAvailableProfileFields)]
        [TestCase(true, false, false, ListAvailableProfileFields)]
        [TestCase(false, true, false, ListAvailableProfileFields)]
        [TestCase(false, false, false, ListAvailableProfileFields)]
        public void BtnRemoveClick_SrcList_DstList(bool isDemo, bool isAdhoc, bool isMatch, string dstLisBoxName)
        {
            // Arrange
            ShimForEcnSession();

            ShimUtilities.GetExportFieldsClientConnectionsEnumsViewTypeInt32IListOfInt32EnumsExportTypeInt32EnumsExportFieldTypeBoolean = 
                (connections, type, _, __, ___, ____, exportFieldType, _____) =>
                {
                    var fields = new Dictionary<string, string>();
                    if (exportFieldType == KmpsEnums.ExportFieldType.Demo && isDemo)
                    {
                        fields[ItemText] = FieldValue1;
                    }
                    else if (exportFieldType == KmpsEnums.ExportFieldType.Adhoc && isAdhoc)
                    {
                        fields[ItemText] = FieldValue2;
                    }
                    return fields;
                };

            ShimBaseControl.AllInstances.clientconnectionsGet = control => null;

            var listBoxSrc = new ListBox();
            var itemText = ItemTextDefault;
            var itemValue = isMatch ? ItemValueDefaultUpper : ItemValueNoTestUpper;
            var itemSrcFields = new ListItem(itemText, itemValue, true);
            itemSrcFields.Selected = true;
            listBoxSrc.Items.Add(itemSrcFields);
            PrivateControl.SetField(
                ListSelectedFields,
                BindingFlags.Instance | BindingFlags.NonPublic,
                listBoxSrc);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodBtnRemove, null, null);

            // Assert
            var lstSelectedFields = (ListBox)_testEntity.GetFieldValue(ListSelectedFields);
            lstSelectedFields.Items.Count.ShouldBe(0);

            var destinationListBox = (ListBox)_testEntity.GetFieldValue(dstLisBoxName);
            destinationListBox.ShouldSatisfyAllConditions(
                () => destinationListBox.Items.Count.ShouldBe(UserIdSampleValue),
                () => destinationListBox.Items[0].Text.ShouldBe(ItemText),
                () => destinationListBox.Items[0].Value.ShouldBe(isMatch ? "TEST|Varchar" : "NOTEST|Varchar"));
        }

        [Test]
        public void BtnUp_Click_2Items_MovedUp()
        {
            // Arrange
            var listBoxSrc = new ListBox();
            var itemSrcFields0 = new ListItem(ItemSampleName0, ItemSampleValue0, true);
            listBoxSrc.Items.Add(itemSrcFields0);
            var itemSrcFields1 = new ListItem(ItemSampleName1, FieldValue1, true);
            itemSrcFields1.Selected = true;
            listBoxSrc.Items.Add(itemSrcFields1);
            PrivateControl.SetField(
                ListSelectedFields,
                BindingFlags.Instance | BindingFlags.NonPublic,
                listBoxSrc);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodBtnUp, null, null);

            // Assert
            var lstSelectedFields = (ListBox)_testEntity.GetFieldValue(ListSelectedFields);
            lstSelectedFields.ShouldSatisfyAllConditions(
                () => lstSelectedFields.Items[0].ShouldBe(itemSrcFields1),
                () => lstSelectedFields.Items[UserIdSampleValue].ShouldBe(itemSrcFields0));
        }

        [Test]
        public void Btndown_Click_2Items_MovedDown()
        {
            // Arrange
            var listBoxSrc = new ListBox();
            var itemSrcFields0 = new ListItem(ItemSampleName0, ItemSampleValue0, true);
            itemSrcFields0.Selected = true;
            listBoxSrc.Items.Add(itemSrcFields0);
            var itemSrcFields1 = new ListItem(ItemSampleName1, FieldValue1, true);
            listBoxSrc.Items.Add(itemSrcFields1);
            PrivateControl.SetField(
                ListSelectedFields,
                BindingFlags.Instance | BindingFlags.NonPublic,
                listBoxSrc);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodBtnDown, null, null);

            // Assert
            var lstSelectedFields = (ListBox)_testEntity.GetFieldValue(ListSelectedFields);
            lstSelectedFields.ShouldSatisfyAllConditions(
                () => lstSelectedFields.Items[0].ShouldBe(itemSrcFields1),
                () => lstSelectedFields.Items[UserIdSampleValue].ShouldBe(itemSrcFields0));
        }

        [Test]
        public void Btndown_ClickDrpIsBillable_OnSelectedIndexChanged_2Items_MovedDown(
            [Values(true, false)]bool selectedValue)
        {
            // Arrange
            var drpIsBillable = new ShimDropDownList();
            var lstIsBillable = new ShimListControl(drpIsBillable);
            lstIsBillable.SelectedValueGet = selectedValue.ToString;
            _testEntity.SetField(DropDownIsBillable, (DropDownList)drpIsBillable);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodDrpIsBillableOnSelectedIndexChanged, null, null);

            // Assert
            var btnExport = (Button)_testEntity.GetFieldValue(ButtonExports);
            var plNotes = (PlaceHolder)_testEntity.GetFieldValue(PlaceHolderNotes);
            btnExport.ShouldSatisfyAllConditions(
                () => btnExport.OnClientClick.ShouldBe(selectedValue ? ScriptReturn : string.Empty),
                () => plNotes.Visible.ShouldBe(!selectedValue));
        }

        [Test]
        public void RbDownloadCheckedChanged_ControlsSetup(
            [Values(true, false)]bool selectedValue)
        {
            // Arrange
            ShimDownloadPanelBase.AllInstances.ShowHeaderCheckBoxGet = _ => selectedValue;

            var mdlDownloads = new ShimModalPopupExtender();
            var mdlDownloadCalled = false;
            mdlDownloads.Show = () =>
            {
                mdlDownloadCalled = true;
            };
            _testEntity.SetField(ModulePopupExtenderDownloads, (ModalPopupExtender)mdlDownloads);

            var phShowHeader = (PlaceHolder)_testEntity.GetFieldValue(PlaceHolderShowHeader);
            phShowHeader.Visible = false;

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodRbDownloadCheckedChanged, null, null);

            // Assert
            var btnExport = (Button)_testEntity.GetFieldValue(ButtonExports);
            var cbShowHeader = (CheckBox)_testEntity.GetFieldValue(ChkBoxShowHeader);
            btnExport.ShouldSatisfyAllConditions(
                () => btnExport.Text.ShouldBe(MessageDownload),
                () => phShowHeader.Visible.ShouldBe(selectedValue),
                () => cbShowHeader.Checked.ShouldBe(false),
                () => mdlDownloadCalled.ShouldBeTrue());
        }

        [Test]
        public void BtnExportClick_ControlsSetup(
            [Values(true, false)]bool pageIsValid, [Values(true, false)]bool listHasItems)
        {
            // Arrange
            ShimPage.AllInstances.IsValidGet = page => pageIsValid;

            var lstSelectedFields = new ListBox();
            if (listHasItems)
            {
                lstSelectedFields.Items.Add(new ListItem());
            }
            _testEntity.SetField(ListSelectedFields, lstSelectedFields);

            var detailsDownloadCalled = false;
            ShimDownloadPanel_CLV.AllInstances.DetailsDownload = _ =>
            {
                detailsDownloadCalled = true;
            };
            var errorSet = false;
            ShimDownloadPanelV.AllInstances.DisplayErrorString = (_, __) =>
            {
                errorSet = true;
            };

            // Act
            ReflectionHelper.CallMethod(_testEntity, BtnExportTestClick, null, null);

            // Assert
            if (pageIsValid)
            {
                if (listHasItems)
                {
                    errorSet.ShouldBeFalse();
                    detailsDownloadCalled.ShouldBeTrue();
                }
                else
                {
                    errorSet.ShouldBeTrue();
                    detailsDownloadCalled.ShouldBeFalse();
                }
            }
            else
            {
                detailsDownloadCalled.ShouldBeFalse();
                errorSet.ShouldBeFalse();
            }
        }

        [Test]
        public void BtnCloseExport_Click_ControlsSetup()
        {
            // Arrange
            var resetPopupControlsCalled = false;
            ShimDownloadPanelV.AllInstances.ResetPopupControls = _ =>
            {
                resetPopupControlsCalled = true;
            };
            var hideDownloadPopupCalled = false;
            Action hideDownloadPopup = () => { hideDownloadPopupCalled = true; };
            _testEntity.HideDownloadPopup = hideDownloadPopup;

            var mdlDownloads = new ShimModalPopupExtender();
            var mdlDownloadsHideCalled = false;
            mdlDownloads.Hide = () => { mdlDownloadsHideCalled = true; };
            _testEntity.SetField(ModulePopupExtenderDownloads, (ModalPopupExtender)mdlDownloads);

            // Act
            ReflectionHelper.CallMethod(_testEntity, MethodBtnCloseExportClick, null, null);

            // Assert
            resetPopupControlsCalled.ShouldSatisfyAllConditions(
                () => resetPopupControlsCalled.ShouldBeTrue(),
                () => hideDownloadPopupCalled.ShouldBeTrue(),
                () => mdlDownloadsHideCalled.ShouldBeTrue());
        }

        [Test]
        public void ResetPopupControls_ControlsSetup()
        {
            // Arrange, Act
            ReflectionHelper.CallMethod(_testEntity, MethodResetPopupControls);

            // Assert
            var plNotes = (PlaceHolder) _testEntity.GetFieldValue(PlaceHolderNotes);
            var txtNotes = (TextBox)_testEntity.GetFieldValue(TxtBoxNotes);

            plNotes.ShouldSatisfyAllConditions(
                () => plNotes.Visible.ShouldBeFalse(),
                () => txtNotes.Text.ShouldBeNullOrEmpty());
        }

        [Test]
        public void DisplayError_ControlsSetup()
        {
            // Arrange, Act
            ReflectionHelper.CallMethod(_testEntity, MethodDisplayError, ErrorSample);

            // Assert
            var lblErrorMessage = (Label)_testEntity.GetFieldValue(LblErrorMessage);
            var divError = (HtmlGenericControl)_testEntity.GetFieldValue(DivError);

            lblErrorMessage.ShouldSatisfyAllConditions(
                () => lblErrorMessage.Text.ShouldBe(ErrorSample),
                () => divError.Visible.ShouldBeTrue());
        }

        private static void ShimForEcnSession()
        {
            ShimECNSession.AllInstances.RefreshSession = session => { };
            var ecnSession = ReflectionHelper.CreateInstance<ECNSession>();
            ReflectionHelper.SetPropertyValue(ecnSession, SessionPropertyUserId, UserIdSampleValue);
            ShimECNSession.CurrentSession = () => ecnSession;
        }
    }
}
