using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using Core_AMS.Utilities;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Modules;
using FileMapperWizard.Modules.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KM.Common.Import;
using KM.Common.Functions;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using UASServiceFakes = FrameworkUAS.Service.Fakes;
using CommonEnums = KM.Common.Enums;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class EditSetupTest
    {
        [Test]
        public void btnStep1Next_Click_WithDemiliterAndFileType_SetsContainerValues()
        {
            // Arrange
            SetEditSetUpControls();
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            container.myFileConfig.ShouldNotBeNull();
            container.extension.ShouldContain(Enums.FileExtensions.txt.ToString());
            container.myTextQualifier.ShouldBeTrue();
            container.srcFileTypeID.ShouldBe(1);
            container.batchSize.ShouldBe(2500);
            container.myFeatureID.ShouldBe(1);
            container.qDateFormat.ShouldContain(DateFormat.DDMMYYYY.ToString());
            container.DatabaseFileType.ShouldBe(1);
        }

        [Test]
        public void btnStep1Next_Click_WithMyFileConfig_SetsContainerValues()
        {
            // Arrange
            SetEditSetUpControls(quotationSelectedIndex: 0);
            _privateTestObj.SetFieldOrProperty(ContainerField, GetShimFMUniversalContainer(isExcelZipFile: false).Instance);
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            container.myFileConfig.ShouldNotBeNull();
            container.myFileConfig.FileColumnDelimiter.ShouldBeNull();
            container.myFileConfig.IsQuoteEncapsulated.ShouldBeFalse();
            container.extension.ShouldContain(Enums.FileExtensions.txt.ToString());
            container.myTextQualifier.ShouldBeTrue();
            container.srcFileTypeID.ShouldBe(1);
            container.batchSize.ShouldBe(2500);
            container.myFeatureID.ShouldBe(1);
            container.qDateFormat.ShouldContain(DateFormat.DDMMYYYY.ToString());
            container.DatabaseFileType.ShouldBe(1);
        }

        [Test]
        public void btnStep1Next_Click_SourceFileEmpty_SetsContainerValues()
        {
            // Arrange
            SetEditSetUpControls(quotationSelectedIndex: 1);
            _privateTestObj.SetFieldOrProperty(ContainerField, GetShimFMUniversalContainer(isExcelZipFile: true).Instance);
            UASServiceFakes.ShimResponse<List<FieldMapping>>.AllInstances.ResultGet = (m) => new List<FieldMapping>();
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            container.myFileConfig.ShouldNotBeNull();
            container.myFileConfig.FileColumnDelimiter.ShouldBeNull();
            container.myFileConfig.IsQuoteEncapsulated.ShouldBeFalse();
            container.extension.ShouldContain(Enums.FileExtensions.txt.ToString());
            container.myTextQualifier.ShouldBeFalse();
            container.srcFileTypeID.ShouldBe(1);
            container.batchSize.ShouldBe(2500);
            container.myFeatureID.ShouldBe(1);
            container.qDateFormat.ShouldContain(DateFormat.DDMMYYYY.ToString());
            container.DatabaseFileType.ShouldBe(1);
        }

        [Test]
        public void btnStep1Next_Click_CodeFileOtherThanACS_SetsContainerValues()
        {
            // Arrange
            SetEditSetUpControls(quotationSelectedIndex: 1, defultFeatureId: 2);
            _privateTestObj.SetFieldOrProperty(ContainerField, GetShimFMUniversalContainer(isExcelZipFile: false).Instance);
            UASServiceFakes.ShimResponse<List<FieldMapping>>.AllInstances.ResultGet = (m) => new List<FieldMapping>();
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            container.myFileConfig.ShouldNotBeNull();
            container.myFileConfig.FileColumnDelimiter.ShouldBeNull();
            container.myFileConfig.IsQuoteEncapsulated.ShouldBeFalse();
            container.extension.ShouldContain(Enums.FileExtensions.txt.ToString());
            container.myTextQualifier.ShouldBeFalse();
            container.srcFileTypeID.ShouldBe(1);
            container.batchSize.ShouldBe(2500);
            container.myFeatureID.ShouldBe(2);
            container.qDateFormat.ShouldContain(DateFormat.DDMMYYYY.ToString());
            container.DatabaseFileType.ShouldBe(1);
        }

        [Test]
        public void btnStep1Next_Click_NoSelectedDimileter_ReturnsWithMessage()
        {
            // Arrange
            _privateTestObj.SetFieldOrProperty(ContainerField, GetShimFMUniversalContainer(isExcelZipFile: false).Instance);
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("File Delimiter and/or File contains double quotation marks");
        }

        [Test]
        public void btnStep1Next_Click_TextSaveNameEmpty_ReturnsWithMessage()
        {
            // Arrange
            SetEditSetUpControls();
            var txtSaveName = (TextBox)_privateTestObj.GetFieldOrProperty("txtSaveName");
            txtSaveName.Text = string.Empty;
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("Data is missing. Please provide a 'Save Filename As");
        }

        [Test]
        public void btnStep1Next_Click_NoMoreThanFifty_ReturnsWithMessage()
        {
            // Arrange
            SetEditSetUpControls();
            var txtSaveName = (TextBox)_privateTestObj.GetFieldOrProperty("txtSaveName");
            txtSaveName.Text = new string('A', 51);
            var parameters = new object[]
            {
                this,
                new RoutedEventArgs()
            };

            // Act
            _privateTestObj.Invoke(BtnStep1NextClickMethodName, parameters);
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty(ContainerField);

            // Assert
            container.ShouldNotBeNull();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("Data invalid. 'Save Filename As' cannot exceed 50 characters in length");
        }

        private void SetEditSetUpControls(int quotationSelectedIndex = 0,int defultFeatureId = 1)
        {
            var rcbDelimiters = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbDelimiters");
            rcbDelimiters.Items.Add(CommonEnums.ColumnDelimiter.comma.ToString().Replace("_", " "));
            rcbDelimiters.SelectedIndex = 0;

            var txtSaveName = (TextBox)_privateTestObj.GetFieldOrProperty("txtSaveName");
            txtSaveName.Text = "TestFile";

            var rcbExtension = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbExtension");
            rcbExtension.Items.Add("." + Enums.FileExtensions.txt.ToString().Replace("_", " ").ToUpper());
            rcbExtension.SelectedIndex = 0;

            var rcbQuotations = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbQuotations");
            rcbQuotations.Items.Add(Enums.YesNo.Yes.ToString().Replace("_", " "));
            rcbQuotations.Items.Add(Enums.YesNo.No.ToString().Replace("_", " "));
            rcbQuotations.SelectedIndex = quotationSelectedIndex;

            var rcbFrequency = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbFrequency");
            rcbFrequency.Items.Add(1);
            rcbFrequency.SelectedIndex = 0;

            var rcbFeatures = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbFeatures");
            rcbFeatures.Items.Add(defultFeatureId);
            rcbFeatures.SelectedIndex = 0;

            var rcbDateFormat = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbDateFormat");
            rcbDateFormat.Items.Add(DateFormat.DDMMYYYY.ToString().Replace("_", " "));
            rcbDateFormat.SelectedIndex = 0;

            var rcbDatabaseFileType = (RadComboBox)_privateTestObj.GetFieldOrProperty("rcbDatabaseFileType");
            rcbDatabaseFileType.Items.Add(1);
            rcbDatabaseFileType.SelectedIndex = 0;

        }

        private ShimFMUniversal GetShimFMUniversalContainer(bool isExcelZipFile = true)
        {
            var shim = new ShimFMUniversal();
            
            shim.InstanceBehavior = ShimBehaviors.Fallthrough;
            shim.Instance.AllPublishers = new List<Client>();
            shim.Instance.AllFeatures = new List<ServiceFeature> { new ServiceFeature { SFName = "Special Files", ServiceFeatureID = 1 } };
            shim.Instance.DatabaseFileTypeList = new List<Code> { new Code { CodeId = 1, CodeName = FrameworkUAD_Lookup.Enums.FileTypes.ACS.ToString() } };
            shim.Instance.myClient = new Client() { ClientID = 1 };
            shim.Instance.FileName = "SampleFile";
            shim.Instance.saveFileName = "SampleFile";
            shim.Instance.sourceFileID = 1;
            shim.Instance.myFeatureID = 1;
            shim.fwGet = () => new ShimFileWorker
            {
                IsExcelFileFileInfo = (w) => isExcelZipFile,
                IsZipFileFileInfo = (w) => isExcelZipFile
            };
            shim.SetupToMapColumns = () => { };
            shim.SetupToSpecialFile = () => { };
            shim.ShowRules = () => { };
            shim.myFileConfigGet = () => new FileConfiguration();
            return shim;
        }

    }
}
