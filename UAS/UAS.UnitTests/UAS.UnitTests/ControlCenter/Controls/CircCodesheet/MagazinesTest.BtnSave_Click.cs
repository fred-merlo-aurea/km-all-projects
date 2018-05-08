using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.Entity;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ControlCenter.Controls.CircCodesheet
{
    public partial class MagazinesTest
    {
        private const string BtnSaveControl = "btnSave";
        private const string TbxMagazineNameControl = "tbxMagazineName";
        private const string TbxMagazineCodeControl = "tbxMagazineCode";
        private const string CbxAllowDataEntryControl = "cbxAllowDataEntry";
        private const string CbFrequencyControl = "cbFrequency";
        private const string TbxYearStartDateControl = "tbxYearStartDate";
        private const string TbxYearEndDateControl = "tbxYearEndDate";
        private const string RadBusyControl = "busy";
        private const string SampleName = "SampleName";
        private const string SampleCode = "SampleCode";
        private const string AllPublicationsField = "allPublications";
        private const string BtnSaveClickMethodName = "btnSave_Click";

        [Test]
        public void BtnSave_Click_WhenNoMagazineSelected_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            var cbMagazine = Get<TextBox>(TbxMagazineNameControl);
            cbMagazine.Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a name."));
        }
        
        [Test]
        public void BtnSave_Click_WhenTbxGroupIsEmpty_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<TextBox>(TbxMagazineCodeControl).Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a code."));
        }

        [Test]
        public void BtnSave_Click_WhenTbxNameIsEmpty_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<TextBox>(TbxYearStartDateControl).Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a year start date."));
        }

        [Test]
        public void BtnSave_Click_WhenTbxEndDateIsEmpty_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<TextBox>(TbxYearEndDateControl).Text = string.Empty;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a year end date."));
        }

        [Test]
        public void BtnSave_Click_WhenFrequencyNotSelected_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            var cbFrequency = Get<RadComboBox>(CbFrequencyControl);
            cbFrequency.SelectedIndex = 0;

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a frequency."));
        }

        [Test]
        public void BtnSave_Click_WhenCurrentPublisherNotSelected_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            ReflectionHelper.SetField(_testEntity, "currentPublisher", new Client { ClientID = 0 });

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please provide a publisher."));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void BtnSave_Click_WhenNameAlreadyExists_SetsMessageBoxText(int tag)
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<RadButton>(BtnSaveControl).Tag = tag;
            var products = new List<Product>
            {
                new Product{ PubID = 2, PubName = SampleName }
            };
            ReflectionHelper.SetField(_testEntity, AllPublicationsField, products);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Name currently exists."));
        }
        
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void BtnSave_Click_WhenCodeAlreadyExists_SetsMessageBoxText(int tag)
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<RadButton>(BtnSaveControl).Tag = tag;
            var products = new List<Product>
            {
                new Product{ PubID = 2, PubCode = SampleCode }
            };
            ReflectionHelper.SetField(_testEntity, AllPublicationsField, products);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Code currently exists."));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValues_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            var products = new List<Product>
            {
                new Product{ PubID = 1, PubCode = SampleCode }
            };
            ReflectionHelper.SetField(_testEntity, AllPublicationsField, products);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());
            AsyncHelper.Wait(3000);

            // Assert
            _privateTestEntity.ShouldSatisfyAllConditions(
                () => _isDataLoaded.ShouldBeTrue(),
                () => _savedProduct.ShouldNotBeNull(),
                () => _savedProduct.PubID.ShouldBe(1),
                () => _savedProduct.FrequencyID.ShouldBe(1),
                () => _savedProduct.PubName.ShouldBe(SampleName),
                () => _savedProduct.PubCode.ShouldBe(SampleCode),
                () => _savedProduct.YearStartDate.ShouldContain(DateTime.UtcNow.ToShortDateString()),
                () => _savedProduct.YearEndDate.ShouldContain(DateTime.UtcNow.ToShortDateString()),
                () => _savedProduct.AllowDataEntry.ShouldBeTrue());
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValuesAndTagZero_SetsMessageBoxText()
        {
            // Arrange
            SetFakesForBtnSaveClickMethod();
            InitializeChildControls();
            Get<RadButton>(BtnSaveControl).Tag = 0;
            var products = new List<Product>
            {
                new Product{ PubID = 1, PubCode = string.Empty }
            };
            ReflectionHelper.SetField(_testEntity, AllPublicationsField, products);

            // Act
            _privateTestEntity.Invoke(BtnSaveClickMethodName, this, new RoutedEventArgs());
            AsyncHelper.Wait(3000);

            // Assert
            _privateTestEntity.ShouldSatisfyAllConditions(
                () => _isDataLoaded.ShouldBeTrue(),
                () => _savedProduct.ShouldNotBeNull(),
                () => _savedProduct.PubID.ShouldBe(0),
                () => _savedProduct.ClientID.ShouldBe(1),
                () => _savedProduct.IsImported.Value.ShouldBeFalse(),
                () => _savedProduct.IsActive.ShouldBeTrue(),
                () => _savedProduct.FrequencyID.ShouldBe(1),
                () => _savedProduct.PubName.ShouldBe(SampleName),
                () => _savedProduct.PubCode.ShouldBe(SampleCode),
                () => _savedProduct.YearStartDate.ShouldContain(DateTime.UtcNow.ToShortDateString()),
                () => _savedProduct.YearEndDate.ShouldContain(DateTime.UtcNow.ToShortDateString()),
                () => _savedProduct.AllowDataEntry.ShouldBeTrue());
        }
       
        private void InitializeChildControls(bool isChecked = true)
        {
            ReflectionHelper.SetField(_testEntity, "currentPublisher", new Client { ClientID = 1 });

            var btnSave = Get<RadButton>(BtnSaveControl);
            btnSave.Tag = 1;
            
            var tbxCode = Get<TextBox>(TbxMagazineCodeControl);
            tbxCode.Text = SampleCode;

            var tbxMagazineName = Get<TextBox>(TbxMagazineNameControl);
            tbxMagazineName.Text = SampleName;

            var tbxYearStartDate = Get<TextBox>(TbxYearStartDateControl);
            tbxYearStartDate.Text = DateTime.UtcNow.ToString();

            var tbxYearEndDate = Get<TextBox>(TbxYearEndDateControl);
            tbxYearEndDate.Text = DateTime.UtcNow.ToString();

            var cbFrequency = Get<RadComboBox>(CbFrequencyControl);
            cbFrequency.ItemsSource = new List<int> { 0, 1 };
            cbFrequency.SelectedIndex = 1;

            var busy = Get<RadBusyIndicator>(RadBusyControl);
            busy.IsBusy = false;

            Get<CheckBox>(CbxAllowDataEntryControl).IsChecked = isChecked;
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