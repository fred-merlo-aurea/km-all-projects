using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.Entity;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ControlCenter.Controls.CircCodesheet
{
    public partial class GroupTest
    {
        private const string TbxGroupConrtol = "tbxGroup";
        private const string BtnSaveConrtol = "btnSave";
        private const string CbMagazineControl = "cbMagazine";
        private const string TbxDisplayControl = "tbxDisplayName";
        private const string CbxMultiControl = "cbxIsMultipleValue";
        private const string CbxReqControl = "cbxIsRequired";
        private const string CbxIsActiveControl = "cbxIsActive";
        private const string RadBusyControl = "busy";
        private const string SampleGroup = "SampleGroup";
        private const string SomeOtherGroupName = "SomeOtherGroupName";
        private const string SampleName = "SampleName";
        private const string AllResponseGroupField = "allResponseGroup";
        private const string BtnSaveClickMethodName = "btnSave_Click";

        [Test]
        public void BtnSave_Click_WhenNoMagazineSelected_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            var cbMagazine = Get<RadComboBox>(CbMagazineControl);
            cbMagazine.SelectedIndex = -1;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("No magazine was selected. Please select a magazine."));
        }

        [Test]
        public void BtnSave_Click_WhenTbxGroupIsEmpty_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<TextBox>(TbxGroupConrtol).Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("No group was provided. Please provide a group."));
        }

        [Test]
        public void BtnSave_Click_WhenTbxNameIsEmpty_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<TextBox>(TbxDisplayControl).Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("No name was provided. Please provide a name."));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void BtnSave_Click_WhenGroupAlreadyExists_SetsMessageBoxText(int tag)
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<RadButton>(BtnSaveConrtol).Tag = tag;
            var responseGroups = new List<ResponseGroup>
            {
                new ResponseGroup{ PubID = 1, ResponseGroupID = 2, ResponseGroupName = SampleGroup }
            };
            ReflectionHelper.SetField(_testEntity, AllResponseGroupField, responseGroups);
            
            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Group currently exists."));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValues_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            var responseGroups = new List<ResponseGroup>
            {
                new ResponseGroup{ PubID = 1, ResponseGroupID = 2, ResponseGroupName = SomeOtherGroupName }
            };
            ReflectionHelper.SetField(_testEntity, AllResponseGroupField, responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());
            AsyncHelper.Wait(3000);
            
            // Assert
            _privateTestEntity.ShouldSatisfyAllConditions(
                () => _isDataLoaded.ShouldBeTrue(),
                () => _savedGroup.ShouldNotBeNull(),
                () => _savedGroup.PubID.ShouldBe(0),
                () => _savedGroup.IsActive.Value.ShouldBeTrue(),
                () => _savedGroup.IsRequired.Value.ShouldBeTrue(),
                () => _savedGroup.IsMultipleValue.Value.ShouldBeTrue(),
                () => _savedGroup.DisplayOrder.ShouldBeNull(),
                () => _savedGroup.DisplayName.ShouldBe(SampleName),
                () => _savedGroup.ResponseGroupName.ShouldBe(SampleGroup),
                () => _savedGroup.ResponseGroupID.ShouldBe(1));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValuesAndTagZero_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<RadButton>(BtnSaveConrtol).Tag = 0;
            var responseGroups = new List<ResponseGroup>
            {
                new ResponseGroup{ PubID = 1, ResponseGroupID = 2, ResponseGroupName = SomeOtherGroupName }
            };
            ReflectionHelper.SetField(_testEntity, AllResponseGroupField, responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());
            AsyncHelper.Wait(3000);

            // Assert
            _privateTestEntity.ShouldSatisfyAllConditions(
                () => _isDataLoaded.ShouldBeTrue(),
                () => _savedGroup.ShouldNotBeNull(),
                () => _savedGroup.PubID.ShouldBe(1),
                () => _savedGroup.IsActive.Value.ShouldBeTrue(),
                () => _savedGroup.IsRequired.Value.ShouldBeTrue(),
                () => _savedGroup.IsMultipleValue.Value.ShouldBeTrue(),
                () => _savedGroup.DisplayOrder.ShouldBe(1),
                () => _savedGroup.DisplayName.ShouldBe(SampleName),
                () => _savedGroup.ResponseGroupName.ShouldBe(SampleGroup),
                () => _savedGroup.ResponseGroupID.ShouldBe(0));
        }

        private void InitializeChildControls(bool isChecked = true)
        {
            var btnSave = Get<RadButton>(BtnSaveConrtol);
            btnSave.Tag = 1;

            var tbxGroup = Get<TextBox>(TbxGroupConrtol);
            tbxGroup.Text = SampleGroup;

            var tbxDisplay = Get<TextBox>(TbxDisplayControl);
            tbxDisplay.Text = SampleName;
            
            var cbMagazine = Get<RadComboBox>(CbMagazineControl);
            cbMagazine.ItemsSource = new List<int> { 0, 1 };
            cbMagazine.SelectedIndex = 1;

            var busy = Get<RadBusyIndicator>(RadBusyControl);
            busy.IsBusy = false;

            Get<CheckBox>(CbxMultiControl).IsChecked = isChecked;
            Get<CheckBox>(CbxReqControl).IsChecked = isChecked;
            Get<CheckBox>(CbxIsActiveControl).IsChecked = isChecked;
        }

        private void SetFakesForBtnSaveClickMethod()
        {
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = (msg, m, mi, r, s) =>
            {
                _messageBoxText += msg;
            };
        }
    }
}
