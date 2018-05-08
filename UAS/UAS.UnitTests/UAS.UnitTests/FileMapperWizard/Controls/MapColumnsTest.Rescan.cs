using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FrameworkUAD.Object;
using FrameworkUAS.Object;
using FrameworkUAD_Lookup.Entity;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using UAS.UnitTests.Helpers;
using static FrameworkUAD_Lookup.Enums;
using Label = System.Windows.Controls.Label;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class MapColumnsTest
    {
        private const string LabelDelimiter = "LabelDelimiter";
        private const string LabelTextQualifier = "LabelTextQualifier";
        private const string ComboBoxDelimiter = "ComboBoxDelimiter";
        private const string ComboBoxQualifier = "ComboBoxQualifier";
        private const string ButtonStart = "ButtonStart";
        private const string FlowLayout = "flowLayout";
        private const string DummyString1 = "DummyString1";
        private const string DummyString2 = "DummyString2";
        private const string DummyString3 = "DummyString3";
        private const string EmptyString = "";

        private Label _labelDelimiter;
        private Label _labelTextQualifier;
        private RadComboBox _comboBoxDelimiter;
        private RadComboBox _comboBoxQualifier;
        private RadButton _buttonStart;
        private StackPanel _flowLayout;

        [Test]
        public void MapColumns_Rescan_DuplicateColumns()
        {
            ShimFileWorker.AllInstances.GetDuplicateColumnsFileInfoFileConfiguration = (p1, p2, p3) =>
            {
                return new List<string>
                {
                    "file1", "file2"
                };
            };

            _labelDelimiter = ReflectionHelper.GetFieldValueAs<Label>(_testEntity, LabelDelimiter);
            _labelTextQualifier = ReflectionHelper.GetFieldValueAs<Label>(_testEntity, LabelTextQualifier);
            _comboBoxDelimiter = ReflectionHelper.GetFieldValueAs<RadComboBox>(_testEntity, ComboBoxDelimiter);
            _comboBoxQualifier = ReflectionHelper.GetFieldValueAs<RadComboBox>(_testEntity, ComboBoxQualifier);
            _buttonStart = ReflectionHelper.GetFieldValueAs<RadButton>(_testEntity, ButtonStart);

            _labelDelimiter.Visibility = Visibility.Visible;
            _labelTextQualifier.Visibility = Visibility.Visible;
            _comboBoxDelimiter.Visibility = Visibility.Visible;
            _comboBoxQualifier.Visibility = Visibility.Visible;
            _buttonStart.Visibility = Visibility.Visible;

            _testEntity.Rescan("SampleFile");

            _labelDelimiter.ShouldSatisfyAllConditions(
                () => _labelDelimiter.Visibility.ShouldBe(Visibility.Collapsed),
                () => _labelTextQualifier.Visibility.ShouldBe(Visibility.Collapsed),
                () => _comboBoxDelimiter.Visibility.ShouldBe(Visibility.Collapsed),
                () => _comboBoxQualifier.Visibility.ShouldBe(Visibility.Collapsed),
                () => _buttonStart.Visibility.ShouldBe(Visibility.Collapsed));
        }

        [Test]
        public void MapColumns_Rescan_Success()
        {
            ShimFileWorker.AllInstances.GetDuplicateColumnsFileInfoFileConfiguration = (p1, p2, p3) =>
            {
                return new List<string>();
            };

            ShimFileWorker.AllInstances.GetFileHeadersFileInfoFileConfigurationBoolean = (p1, p2, p3, p4) =>
            {
                return new StringDictionary()
                {
                    { DummyString1, "1" },
                    { DummyString2, "2" }
                };
            };

            ShimColumnMapper.AllInstances.DemographicUpdateCodeIdGet = (p1) => { return 1; };

            _labelDelimiter = ReflectionHelper.GetFieldValueAs<Label>(_testEntity, LabelDelimiter);
            _labelTextQualifier = ReflectionHelper.GetFieldValueAs<Label>(_testEntity, LabelTextQualifier);
            _comboBoxDelimiter = ReflectionHelper.GetFieldValueAs<RadComboBox>(_testEntity, ComboBoxDelimiter);
            _comboBoxQualifier = ReflectionHelper.GetFieldValueAs<RadComboBox>(_testEntity, ComboBoxQualifier);
            _buttonStart = ReflectionHelper.GetFieldValueAs<RadButton>(_testEntity, ButtonStart);
            _flowLayout = ReflectionHelper.GetFieldValueAs<StackPanel>(_testEntity, FlowLayout);

            _labelDelimiter.Visibility = Visibility.Visible;
            _labelTextQualifier.Visibility = Visibility.Visible;
            _comboBoxDelimiter.Visibility = Visibility.Visible;
            _comboBoxQualifier.Visibility = Visibility.Visible;
            _buttonStart.Visibility = Visibility.Visible;
            _flowLayout.IsEnabled = false;

            _container.uadColumns = new List<FileMappingColumn>()
            {
                new FileMappingColumn() { ColumnName = DummyString1 }
            };

            AddChildrenToFlowLayout();

            _testEntity.Rescan("SampleFile");

            _labelDelimiter.ShouldSatisfyAllConditions(
                () => _flowLayout.IsEnabled.ShouldBeTrue(),
                () => _labelDelimiter.Visibility.ShouldBe(Visibility.Collapsed),
                () => _labelTextQualifier.Visibility.ShouldBe(Visibility.Collapsed),
                () => _comboBoxDelimiter.Visibility.ShouldBe(Visibility.Collapsed),
                () => _comboBoxQualifier.Visibility.ShouldBe(Visibility.Collapsed),
                () => _buttonStart.Visibility.ShouldBe(Visibility.Collapsed));
        }

        private void AddChildrenToFlowLayout()
        {
            var columnMapper = new ColumnMapper(
                "Normal",
                AppData.myAppData,
                _container.myClient,
                _container.uadColumns,
                ColumnMapperControlType.New.ToString(),
                DummyString1,
                DummyString1,
                new List<Code>(),
                new List<string>(),
                new List<string>(),
                new Code(),
                DummyString1,
                FieldMappingTypes.Demographic.ToString());

            _flowLayout.Children.Add(columnMapper);

            columnMapper = new ColumnMapper(
                "Normal",
                AppData.myAppData,
                _container.myClient,
                _container.uadColumns,
                ColumnMapperControlType.New.ToString(),
                DummyString3,
                DummyString3,
                new List<Code>(),
                new List<string>(),
                new List<string>(),
                new Code(),
                DummyString3,
                FieldMappingTypes.Demographic.ToString());

            _flowLayout.Children.Add(columnMapper);

            columnMapper = new ColumnMapper(
                "Normal",
                AppData.myAppData,
                _container.myClient,
                _container.uadColumns,
                ColumnMapperControlType.New.ToString(),
                EmptyString,
                DummyString3,
                new List<Code>(),
                new List<string>(),
                new List<string>(),
                new Code(),
                DummyString3,
                FieldMappingTypes.Demographic.ToString());

            _flowLayout.Children.Add(columnMapper);
        }
    }
}
