using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FrameworkServices;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.Service;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS.UnitTests.Helpers;
using KMObject = KMPlatform.Object;

namespace UAS.UnitTests.ControlCenter.Modules
{
    public partial class PublicationManagementTest
    {
        private const string TbPubNameControl = "tbPubName";
        private const string CbisTradeShowControl = "cbistradeshow";
        private const string TbPubCodeControl = "tbPubCode";
        private const string RcbPubTypeIDControl = "rcbPubTypeID";
        private const string CbEnableSearchingControl = "cbEnableSearching";
        private const string NudScoreControl = "nudScore";
        private const string NudSortOrderControl = "nudSortOrder";
        private const string DtpYearStartDateControl = "dtpYearStartDate";
        private const string DtpYearEndDateControl = "dtpYearEndDate";
        private const string DtpIssueDateControl = "dtpIssueDate";
        private const string CbIsImportedControl = "cbIsImported";
        private const string CbIsActiveControl = "cbIsActive";
        private const string CbAllowDataEntryControl = "cbAllowDataEntry";
        private const string RcbFrequencyIDControl = "rcbFrequencyID";
        private const string CbKMImportAllowedControl = "cbKMImportAllowed";
        private const string CbClientImportAllowedControl = "cbClientImportAllowed";
        private const string CbAddRemoveAllowedControl = "cbAddRemoveAllowed";
        private const string CbIsUADControl = "cbIsUAD";
        private const string CbIsCircControl = "cbIsCirc";
        private const string SamplePubName = "SamplePubName";
        private const string SamplePubCode = "SamplePubCode";
        private const string GrdPublicationsField = "grdPublications";
        private const string MyClientProperty = "myClient";
        private const string PublicationWorkerProperty = "publicationWorker";
        private const string BtnNewSaveClickMethodName = "btnNewSave_Click";

        private Product _savedProduct;

        [Test]
        public void BtnSave_Click_WhenNameIsEmpty_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<TextBox>(TbPubNameControl).Text = string.Empty;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Current product name cannot be blank."));
        }

        [Test]
        public void BtnSave_Click_WhenProductIsEmpty_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<TextBox>(TbPubCodeControl).Text = string.Empty;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Current product code cannot be blank."));
        }

        [Test]
        public void BtnSave_Click_WhenProductTypeNotSelected_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<RadComboBox>(RcbPubTypeIDControl).SelectedIndex = 0;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Must select a product type."));
        }

        [Test]
        public void BtnSave_Click_WhenFrequencyTypeNotSelected_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<CheckBox>(CbIsCircControl).IsChecked = false;
            Get<RadComboBox>(RcbFrequencyIDControl).SelectedIndex = 0;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Must select a frequency type."));
        }

        [Test]
        public void BtnSave_Click_WhenYearStartDateIsEmpty_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<RadDateTimePicker>(DtpYearStartDateControl).CurrentDateTimeText = string.Empty;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please supply a Year Start Date."));
        }

        [Test]
        public void BtnSave_Click_WhenYearEndDateIsEmpty_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<RadDateTimePicker>(DtpYearEndDateControl).CurrentDateTimeText = string.Empty;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please supply a Year End Date."));
        }

        [Test]
        public void BtnSave_Click_WhenFrequencyNotSelected_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            Get<RadComboBox>(RcbFrequencyIDControl).SelectedIndex = 0;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Please supply a Frequency."));
        }

        [Test]
        public void BtnSave_Click_WhenPubNameAlreadyExists_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            ServiceClient<IProduct> shimServiceClient = GetShimServiceClientProduct(new Product { PubName = SamplePubName });
            _testEntity.SetProperty(PublicationWorkerProperty, shimServiceClient);

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Must provide a unique product name that hasn't been used."));
        }

        [Test]
        public void BtnSave_Click_WhenPubCodeAlreadyExists_ShowsMessageBoxErrorText()
        {
            // Arrange
            InitialChildControls();
            ServiceClient<IProduct> shimServiceClient = GetShimServiceClientProduct(new Product { PubCode = SamplePubCode });
            _testEntity.SetProperty(PublicationWorkerProperty, shimServiceClient);

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _messageBoxText.ShouldSatisfyAllConditions(
                () => _messageBoxText.ShouldNotBeNullOrWhiteSpace(),
                () => _messageBoxText.ShouldBe("Must provide a unique product code that hasn't been used."));
        }

        [Test]
        public void BtnSave_Click_WhenCorrectValues_SavesProduct()
        {
            // Arrange
            InitialChildControls();
            ServiceClient<IProduct> shimServiceClient = GetShimServiceClientProduct(new Product());
            _testEntity.SetProperty(PublicationWorkerProperty, shimServiceClient);
            _savedProduct = null;

            // Act
            CallBtnSaveClick(_testEntity, this, new RoutedEventArgs());

            // Assert
            _testEntity.ShouldSatisfyAllConditions(
                () => _savedProduct.ShouldNotBeNull(),
                () => _savedProduct.PubName.ShouldBe(SamplePubName),
                () => _savedProduct.PubCode.ShouldBe(SamplePubCode),
                () => _savedProduct.istradeshow.ShouldBeTrue(),
                () => _savedProduct.PubTypeID.ShouldBe(1),
                () => _savedProduct.GroupID.ShouldBe(0),
                () => _savedProduct.EnableSearching.ShouldBeTrue(),
                () => _savedProduct.score.ShouldBe(1),
                () => _savedProduct.ClientID.ShouldBe(1),
                () => _savedProduct.IsImported.Value.ShouldBeTrue(),
                () => _savedProduct.IsActive.ShouldBeTrue(),
                () => _savedProduct.AllowDataEntry.ShouldBeTrue(),
                () => _savedProduct.FrequencyID.ShouldBe(1),
                () => _savedProduct.KMImportAllowed.Value.ShouldBeTrue(),
                () => _savedProduct.ClientImportAllowed.Value.ShouldBeTrue(),
                () => _savedProduct.AddRemoveAllowed.Value.ShouldBeTrue(),
                () => _savedProduct.AcsMailerInfoId.ShouldBe(0),
                () => _savedProduct.IsUAD.Value.ShouldBeTrue(),
                () => _savedProduct.IsCirc.Value.ShouldBeTrue(),
                () => _isWindowClosed.ShouldBeTrue());
        }

        private void InitialChildControls(bool isChecked = true)
        {
            var dataControl = new Mock<RadGridView>();
            dataControl.SetupAllProperties();
            dataControl.Object.RowDetailsVisibilityMode = GridViewRowDetailsVisibilityMode.Visible;
            _testEntity.SetField(GrdPublicationsField, dataControl.Object);

            var client = new Client { ClientConnections = new KMObject.ClientConnections() };
            _testEntity.SetProperty(MyClientProperty, client);

            Get<TextBox>(TbPubNameControl).Text = SamplePubName;
            Get<CheckBox>(CbisTradeShowControl).IsChecked = isChecked;
            Get<TextBox>(TbPubCodeControl).Text = SamplePubCode;

            var radPubType = Get<RadComboBox>(RcbPubTypeIDControl);
            radPubType.ItemsSource = new List<int> { 0, 1 };
            radPubType.SelectedIndex = 1;

            Get<CheckBox>(CbEnableSearchingControl).IsChecked= isChecked;
            Get<RadNumericUpDown>(NudScoreControl).Value = 1;
            Get<RadNumericUpDown>(NudSortOrderControl).Value = 1;
            Get<RadDateTimePicker>(DtpYearStartDateControl).CurrentDateTimeText = DateTime.UtcNow.ToString();
            Get<RadDateTimePicker>(DtpYearEndDateControl).CurrentDateTimeText = DateTime.UtcNow.ToString();
            Get<RadDateTimePicker>(DtpIssueDateControl).SelectedDate = DateTime.UtcNow;
            Get<CheckBox>(CbIsImportedControl).IsChecked = isChecked;
            Get<CheckBox>(CbIsActiveControl).IsChecked = isChecked;
            Get<CheckBox>(CbAllowDataEntryControl).IsChecked = isChecked;
            
            var radFrequecnyId = Get<RadComboBox>(RcbFrequencyIDControl);
            radFrequecnyId.ItemsSource = new List<int> { 0, 1 };
            radFrequecnyId.SelectedIndex = 1;
            
            Get<CheckBox>(CbKMImportAllowedControl).IsChecked = isChecked;
            Get<CheckBox>(CbClientImportAllowedControl).IsChecked = isChecked;
            Get<CheckBox>(CbAddRemoveAllowedControl).IsChecked = isChecked;
            Get<CheckBox>(CbIsUADControl).IsChecked = isChecked;
            Get<CheckBox>(CbIsCircControl).IsChecked = isChecked;
        }

        private ShimServiceClient<IProduct> GetShimServiceClientProduct(Product product)
        {
            return new ShimServiceClient<IProduct>
            {
                ProxyGet = () => new StubIProduct
                {
                    SelectGuidClientConnections = (id, ccon) => 
                    {
                        return new Response<List<Product>>
                        {
                            Result = new List<Product>
                           {
                               product
                           }
                        };
                    },
                    SaveGuidProductClientConnections = (id, prod, ccon) => 
                    {
                        _savedProduct = prod;
                        return new Response<int>
                        {
                            Result = 1
                        };
                    }
                }
            };
        }
    }
}
