using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ControlCenter.Controls;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ControlCenter.Controls
{
    public partial class ResponseGroupsTest
    {
        private const string TbxGroupConrtol = "tbxGroup";
        private const string BtnSaveConrtol = "btnSave";
        private const string TbxNameControl = "tbxName";
        private const string CbProductControl = "cbProduct";
        private const string TbxDisplayControl = "tbxDisplay";
        private const string RcbKMProductControl = "rcbKMProduct";
        private const string CbxMultiControl = "cbxMulti";
        private const string CbxReqControl = "cbxReq";
        private const string CbxIsActiveControl = "cbxIsActive";
        private const string SampleGroup = "SampleGroup";
        private const string SomeOtherGroupName = "SomeOtherGroupName";
        private const string SampleName = "SampleName";
        private const string CurrentResponseGroupField = "currentResponseGroup";
        private const string ResponseGroupsField = "responseGroups";
        private const string ResponseGroupContainerTypeName = "ResponseGroupContainer";
        private const string GroupNameWithSpace = "Sample Group";
        private const string BtnSaveClickMethodName = "btnSave_Click";
        
        [Test]
        public void BtnSave_Click_WhenGroupTextBoxIsEmpty_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            Get<TextBox>(TbxGroupConrtol).Text = string.Empty;
            
            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () =>_messageBoxText.ShouldBe("Please provide a Group."));
        }

        [Test]
        public void BtnSave_Click_WhenGroupTextBoxContainsSpace_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            Get<TextBox>(TbxGroupConrtol).Text = GroupNameWithSpace;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () =>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () =>_messageBoxText.ShouldBe("Group cannot contain space characters."));
        }

        [Test]
        public void BtnSave_Click_WhenDisplayNameTextBoxEmpty_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            Get<TextBox>(TbxNameControl).Text = string.Empty;
            
            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                ()=>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                ()=>_messageBoxText.ShouldBe("Please provide a Display Name."));
        }

        [Test]
        public void BtnSave_Click_WhenProductComboxSelectedValueIsNotCorrect_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var cbProduct = Get<RadComboBox>(CbProductControl);
            cbProduct.SelectedIndex = 0;
            
            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () =>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () =>_messageBoxText.ShouldBe("Error loading product data. Please contact us if problem persists."));
        }

        [Test]
        public void BtnSave_Click_WhenProductComboxEmpty_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var cbProduct = Get<RadComboBox>(CbProductControl);
            cbProduct.SelectedIndex = -1;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please select a product."));
        }

        [Test]
        public void BtnSave_Click_WhenKMProductComboxSelectedValueIsNotCorrect_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var kmProduct = Get<RadComboBox>(RcbKMProductControl);
            kmProduct.SelectedIndex = 0;
            
            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Error loading product data. Please contact us if problem persists."));
        }

        [Test]
        public void BtnSave_Click_WhenKMProductComboxEmpty_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var kmProduct = Get<RadComboBox>(RcbKMProductControl);
            kmProduct.SelectedIndex = -1;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () =>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () =>_messageBoxText.ShouldBe("Please select KM Product."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void BtnSave_Click_WhenCurrentGroupAlreadyExists_ShowsMessageBoxText(bool isChecked)
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls(isChecked);
            var responseGroups = new List<ResponseGroup>
            {
                new ResponseGroup {PubID = 1, ResponseGroupName = SomeOtherGroupName }
            };
            var instance = GetNestedType(ResponseGroupContainerTypeName, responseGroups[0]);
            SetCurrentResponseGroupField(instance);
            responseGroups[0].ResponseGroupName = SampleGroup;
            SetResponseGroupsListField(responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () =>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () =>_messageBoxText.ShouldBe("Group already exists."));
        }

        [Test]
        public void BtnSave_Click_WhenCurrentResponseGroupIsNull_ShowsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var responseGroups = GetResponseGroupsList();
            SetCurrentResponseGroupField(null);
            SetResponseGroupsListField(responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                ()=>_messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                ()=>_messageBoxText.ShouldBe("Group already exists."));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValues_ResponseGroupIsSaved()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();
            var responseGroups = GetResponseGroupsList();
            var instance = GetNestedType(ResponseGroupContainerTypeName, responseGroups[0]);
            SetCurrentResponseGroupField(instance);
            responseGroups[0].PubID = 2;
            SetResponseGroupsListField(responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _privateTestEntity.ShouldSatisfyAllConditions(
                () => _isDataLoaded.ShouldBeTrue(),
                () => _isDataRefreshed.ShouldBeTrue(),
                () => _isSavedMessageBoxShown.ShouldBeTrue(),
                () => _savedGroup.ShouldNotBeNull(),
                () => _savedGroup.PubID.ShouldBe(PubId),
                () => _savedGroup.IsActive.Value.ShouldBeTrue(),
                () => _savedGroup.IsRequired.Value.ShouldBeTrue(),
                () => _savedGroup.IsMultipleValue.Value.ShouldBeTrue(),
                () => _savedGroup.ResponseGroupTypeId.ShouldBe(1),
                () => _savedGroup.DisplayOrder.ShouldBe(1),
                () => _savedGroup.DisplayName.ShouldBe(SampleName),
                () => _savedGroup.ResponseGroupName.ShouldBe(SampleGroup),
                () => _savedGroup.ResponseGroupID.ShouldBe(1));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValuesButSaveFails_ShowsMessageBoxText()
        {
            // Arrange
            InitializeCommonFakes(ServiceResponseStatusTypes.Error);
            _testEntity = new ResponseGroups(PubId);
            _privateTestEntity = new PrivateObject(_testEntity);
            SetFakesForBtnSaveClickMethod();
            InitializeUserControlControls();

            var responseGroups = GetResponseGroupsList();
            var instance = GetNestedType(ResponseGroupContainerTypeName, responseGroups[0]);
            SetCurrentResponseGroupField(instance);
            responseGroups[0].PubID = 2;
            SetResponseGroupsListField(responseGroups);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("There was an error saving the data. If this problem persists please contact us."));
        }

        private static List<ResponseGroup> GetResponseGroupsList()
        {
            return new List<ResponseGroup>
            {
                new ResponseGroup { ResponseGroupName = SampleGroup, PubID = 1 }
            };
        }

        private void InitializeUserControlControls(bool isChecked = true)
        {
            var btnSave = Get<RadButton>(BtnSaveConrtol);
            btnSave.Tag = 1;

            var tbxGroup = Get<TextBox>(TbxGroupConrtol);
            tbxGroup.Text = SampleGroup;

            var tbxDisplay = Get<TextBox>(TbxDisplayControl);
            tbxDisplay.Text = "1";

            var tbxName = Get<TextBox>(TbxNameControl);
            tbxName.Text = SampleName;

            var cbProduct = Get<RadComboBox>(CbProductControl);
            cbProduct.ItemsSource = new List<int> { 0, 1 };
            cbProduct.SelectedIndex = 1;

            var rcbKMProduct = Get<RadComboBox>(RcbKMProductControl);
            rcbKMProduct.ItemsSource = new List<int> { 0, 1 };
            rcbKMProduct.SelectedIndex = 1;

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
            ShimWPF.MessageSaveComplete = () => { _isSavedMessageBoxShown = true; };
        }
    }
}
