using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Fakes;
using System.Collections.Specialized;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Service;
using FileMapperWizard.Controls;
using FrameworkServices.Fakes;
using Shouldly;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS.UnitTests.Helpers;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Fakes;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class MapColumnsTest
    {
        [Test]
        public void LoadData_IsEditisFalse_SetsContainerValuesAndControls()
        {
            // Arrange
            ShimColumnMapper.ConstructorStringAppDataClientListOfFileMappingColumnStringStringStringListOfCodeListOfStringListOfStringCodeStringStringStringBoolean =
                (q, e, r, t, y, u, i, o, a, s, d, f, g, h, k, z) => { };
            ShimMapColumns.AllInstances.LoadData = null;
            SetLoadDataFakes();

            // Act
            _testEntity.LoadData();
            AsyncHelper.Wait(3000);
            var resultContainer = Get<FMUniversal>(_privateTestObj, ContainerField);
            var comboBoxDelimiter = Get<RadComboBox>(_privateTestObj, "ComboBoxDelimiter");
            var comboBoxQualifier = Get<RadComboBox>(_privateTestObj, "ComboBoxQualifier");
            var requiredFields = Get<List<string>>(_privateTestObj, "isRequiredFields");
            var adhocBitFields = Get<List<string>>(_privateTestObj, "adhocBitFields");
            var fieldMappingTypes = Get<List<Code>>(_privateTestObj, "fieldMappingTypes");
            var demoUpdateOptions = Get<List<Code>>(_privateTestObj, "DemoUpdateOptions");

            // Assert
            fieldMappingTypes.ShouldNotBeEmpty();
            fieldMappingTypes.Count.ShouldBe(7);
            demoUpdateOptions.ShouldNotBeEmpty();
            demoUpdateOptions.Count.ShouldBe(7);
            adhocBitFields.ShouldNotBeEmpty();
            adhocBitFields.Count.ShouldBe(2);
            requiredFields.ShouldNotBeEmpty();
            requiredFields.Count.ShouldBe(1);
            comboBoxDelimiter.ShouldNotBeNull();
            comboBoxDelimiter.Items.Count.ShouldBeGreaterThan(0);
            comboBoxQualifier.ShouldNotBeNull();
            comboBoxQualifier.Items.Count.ShouldBeGreaterThan(0);
            resultContainer.ShouldNotBeNull();
            resultContainer.columns.ShouldNotBeNull();
            resultContainer.columns.Count.ShouldBe(1);
            resultContainer.fileData.ShouldNotBeNull();
            resultContainer.fileData.Rows.Count.ShouldBe(0);
            resultContainer.pubCode.ShouldNotBeEmpty();
            resultContainer.pubCode.ShouldContainKeyAndValue(1, "TestPubCode");
            resultContainer.uadColumns.ShouldNotBeEmpty();
            resultContainer.uadColumns.Count.ShouldBe(1);
            _messageBoxMessage.ShouldBeNullOrWhiteSpace();
            _isbusyIconBusy.ShouldBeFalse();
        }

        [Test]
        public void LoadData_IsEditTrue_SetsContainerValuesAndControls()
        {
            // Arrange
            ShimColumnMapper.ConstructorStringAppDataClientListOfFileMappingColumnStringStringStringListOfCodeListOfStringListOfStringCodeStringStringStringBoolean =
                (q, e, r, t, y, u, i, o, a, s, d, f, g, h, k, z) => { };
            ShimMapColumns.AllInstances.LoadData = null;
            SetLoadDataFakes();
            var container = (FMUniversal)_privateTestObj.GetFieldOrProperty("thisContainer");
            container.isEdit = true;
            container.selectedProduct = null;

            // Act
            _testEntity.LoadData();
            AsyncHelper.Wait(3000);
            var resultContainer = Get<FMUniversal>(_privateTestObj, ContainerField);
            var comboBoxDelimiter = Get<RadComboBox>(_privateTestObj, "ComboBoxDelimiter");
            var comboBoxQualifier = Get<RadComboBox>(_privateTestObj, "ComboBoxQualifier");
            var requiredFields = Get<List<string>>(_privateTestObj, "isRequiredFields");
            var adhocBitFields = Get<List<string>>(_privateTestObj, "adhocBitFields");
            var fieldMappingTypes = Get<List<Code>>(_privateTestObj, "fieldMappingTypes");
            var demoUpdateOptions = Get<List<Code>>(_privateTestObj, "DemoUpdateOptions");

            // Assert
            fieldMappingTypes.ShouldNotBeEmpty();
            fieldMappingTypes.Count.ShouldBe(7);
            demoUpdateOptions.ShouldNotBeEmpty();
            demoUpdateOptions.Count.ShouldBe(7);
            adhocBitFields.ShouldNotBeEmpty();
            adhocBitFields.Count.ShouldBe(2);
            requiredFields.ShouldNotBeEmpty();
            requiredFields.Count.ShouldBe(1);
            comboBoxDelimiter.ShouldNotBeNull();
            comboBoxDelimiter.Items.Count.ShouldBeGreaterThan(0);
            comboBoxQualifier.ShouldNotBeNull();
            comboBoxQualifier.Items.Count.ShouldBeGreaterThan(0);
            resultContainer.ShouldNotBeNull();
            resultContainer.columns.ShouldBeNull();
            resultContainer.fileData.ShouldBeNull();
            resultContainer.pubCode.ShouldNotBeEmpty();
            resultContainer.pubCode.ShouldContainKeyAndValue(1, "TestPubCode");
            resultContainer.uadColumns.ShouldNotBeEmpty();
            resultContainer.uadColumns.Count.ShouldBe(1);
            _messageBoxMessage.ShouldBeNullOrWhiteSpace();
            _isbusyIconBusy.ShouldBeFalse();
        }

        [Test]
        public void LoadData_WIthIsEditFalseAndEmptyContainerUADColumns_ReturnsWithMessage()
        {
            // Arrange
            ShimColumnMapper.ConstructorStringAppDataClientListOfFileMappingColumnStringStringStringListOfCodeListOfStringListOfStringCodeStringStringStringBoolean =
                (q, e, r, t, y, u, i, o, a, s, d, f, g, h, k, z) => { };
            SetLoadDataFakes();
            CreateUADFileMappingColumnClient(); // hides the base class call
            _testEntity = new MapColumns(_container);
            ShimMapColumns.AllInstances.LoadData = null; // So that actual call to LoadData is made
            _privateTestObj = new PrivateObject(_testEntity);

            // Act
            _testEntity.LoadData();
            AsyncHelper.Wait(3000);
            var resultContainer = Get<FMUniversal>(_privateTestObj, ContainerField);

            // Assert
            resultContainer.ShouldNotBeNull();
            resultContainer.columns.Count.ShouldBe(1);
            resultContainer.fileData.ShouldNotBeNull();
            resultContainer.fileData.Rows.Count.ShouldBe(0);
            resultContainer.pubCode.ShouldNotBeEmpty();
            resultContainer.pubCode.ShouldContainKeyAndValue(1, "TestPubCode");
            resultContainer.uadColumns.ShouldBeEmpty();
            _messageBoxMessage.ShouldNotBeNullOrWhiteSpace();
            _messageBoxMessage.ShouldContain("Data is missing. No columns were found.");
            _isbusyIconBusy.ShouldBeFalse();
        }

        [Test]
        public void LoadData_WIthEmptyFileHeaders_ReturnsWithMessage()
        {
            // Arrange
            ShimColumnMapper.ConstructorStringAppDataClientListOfFileMappingColumnStringStringStringListOfCodeListOfStringListOfStringCodeStringStringStringBoolean =
                (q, e, r, t, y, u, i, o, a, s, d, f, g, h, k, z) => { };
            SetLoadDataFakes();
            var shimContainer = GetShimFMUniversalContainer();
            shimContainer.fwGet = () => new ShimFileWorker
            {
                IsExcelFileFileInfo = (w) => true,
                IsZipFileFileInfo = (w) => true,
                GetDuplicateColumnsFileInfoFileConfiguration = (f, c) => new List<string> { "SomeConfig" },
                GetFileHeadersFileInfoFileConfigurationBoolean = (f, c, b) => new StringDictionary(),
                GetRowCountFileInfo = (f) => 1,
                GetDataFileInfoFileConfiguration = (f, c) => new System.Data.DataTable()
            };
            shimContainer.MapToSetupColumns = () => { };
            _testEntity = new MapColumns(shimContainer.Instance);
            _privateTestObj = new PrivateObject(_testEntity);
            ShimMapColumns.AllInstances.LoadData = null; // So that actual call to LoadData is made

            // Act
            _testEntity.LoadData();
            AsyncHelper.Wait(3000);
            var resultContainer = Get<FMUniversal>(_privateTestObj, ContainerField);

            // Assert
            resultContainer.ShouldNotBeNull();
            resultContainer.columns.Count.ShouldBe(0);
            resultContainer.fileData.ShouldNotBeNull();
            resultContainer.fileData.Rows.Count.ShouldBe(0);
            resultContainer.pubCode.ShouldNotBeEmpty();
            resultContainer.pubCode.ShouldContainKeyAndValue(1, "TestPubCode");
            resultContainer.uadColumns.ShouldNotBeEmpty();
            resultContainer.uadColumns.Count.ShouldBe(1);
            _messageBoxMessage.ShouldNotBeNullOrWhiteSpace();
            _messageBoxMessage.ShouldContain("Issue gathering file columns. Please check file is not open or file has no duplicate columns.");
            _isbusyIconBusy.ShouldBeFalse();
        }

        [Test]
        public void LoadData_WIthIsEditTrueAndEmptyContainerUADColumns_ReturnsWithMessage()
        {
            // Arrange
            ShimColumnMapper.ConstructorStringAppDataClientListOfFileMappingColumnStringStringStringListOfCodeListOfStringListOfStringCodeStringStringStringBoolean =
                (q, e, r, t, y, u, i, o, a, s, d, f, g, h, k, z) => { };
            SetLoadDataFakes();
            CreateUADFileMappingColumnClient(); // hides the base class call;
            _testEntity = new MapColumns(_container);
            _privateTestObj = new PrivateObject(_testEntity);
            var container = Get<FMUniversal>(_privateTestObj, ContainerField);
            container.isEdit = true;
            ShimMapColumns.AllInstances.LoadData = null;  // So that actual call to LoadData is made

            // Act
            _testEntity.LoadData();
            AsyncHelper.Wait(3000);
            var resultContainer = Get<FMUniversal>(_privateTestObj, ContainerField);

            // Assert
            resultContainer.ShouldNotBeNull();
            resultContainer.pubCode.ShouldNotBeEmpty();
            resultContainer.pubCode.ShouldContainKeyAndValue(1, "TestPubCode");
            resultContainer.uadColumns.ShouldBeEmpty();
            _messageBoxMessage.ShouldNotBeNullOrWhiteSpace();
            _messageBoxMessage.ShouldContain("Data is missing. No columns were found.");
            _isbusyIconBusy.ShouldBeFalse();
        }

        private void SetLoadDataFakes()
        {
            ShimWPF.FindControlOf1DependencyObjectString<RadBusyIndicator>((d, name) =>
            {
                if (name == "busyIcon")
                {
                    return new RadBusyIndicator();
                }
                return null;
            });

            ShimPanel.AllInstances.ChildrenGet = (p) => new ShimUIElementCollection()
            {
                AddUIElement = (x) => 1
            };

            var foo = (StackPanel)_privateTestObj.GetFieldOrProperty("flowLayout");
            foo = new ShimStackPanel();
            ShimRadBusyIndicator.AllInstances.IsBusySetBoolean = (x, isbusy) => { _isbusyIconBusy = isbusy; };
        }

        protected new void CreateUADFileMappingColumnClient()
        {
            ShimServiceClient.UAD_FileMappingColumnClient = () =>
            {
                return new ShimServiceClient<IFileMappingColumn>
                {
                    ProxyGet = () =>
                    {
                        return new StubIFileMappingColumn
                        {
                            SelectGuidClientConnections = (g, c) =>
                            {
                                return new Response<List<FileMappingColumn>>
                                {
                                    Result = new List<FileMappingColumn>()
                                };
                            }
                        };
                    }
                };
            };
        }

        private T Get<T>(PrivateObject privateObj, string propName)
        {
            return (T)privateObj.GetFieldOrProperty(propName);
        }
    }
}
