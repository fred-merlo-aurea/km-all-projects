using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using WpfControls;
using WpfControls.WindowsAndDialogs;
using WpfControls.Fakes; 
using static WpfControls.WindowsAndDialogs.RecordUpdateChangeSelector;

namespace UAS.UnitTests.WpfControls.WindowsAndDialogs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public partial class RecordUpdateChangeSelectorTest
    {
        private const string SampleName = "SampleName";
        private const string SampleText = "SampleText";
        private const string DisplayNameDemo7 = "Demo7";
        private const string QualificationDateName = "QualificationDate";
        private const string SampleKey = "SampleKey";
        private const string SampleValue = "SampleValue";
        private const string SpChangesControl = "spChanges";
        private const string CbOptionControl = "cbOption";
        private const string RdpDateControl = "rdpDate";
        private const string RlbOptionControl = "rlbOption";
        private const string TbOptionControl = "tbOption";
        private const string SampleCodeValue = "SampleCodeValue";
        private const string SampleAdhoc = "SampleAdhoc";
        private const string SamplePubProp = "SamplePubProp";
        private const string BtnApplyClickMethodName = "btnApply_Click";

        private IDisposable _shimsObject;
        private RecordUpdateChangeSelector _testEntity;
        private PrivateObject _privateTestObject;
        private List<AppliedChanges> _appliedChangesResult;
        private RecordUpdate.BulkRecordUpdateDetail _bulkRecordUpdateResult;

        [SetUp]
        public void SetUp()
        {
            _shimsObject = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _shimsObject.Dispose();
        }

        [Test]
        [TestCase(SampleName)]
        [TestCase(DisplayNameDemo7)]
        [TestCase(QualificationDateName)]
        public void BtnApply_Click_WithAnswersAndDateTimeDisplayName_ResultResponseUpdated(string displayName)
        {
            // Arrange
            var args = new RoutedEventArgs();
            _testEntity = GetRecordUpdateChangeSelector(displayName);
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeControls();

            // Act
            _privateTestObject.Invoke(BtnApplyClickMethodName, this, args);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                    () => _appliedChangesResult.ShouldNotBeNull(),
                    () => _appliedChangesResult.ShouldNotBeEmpty(),
                    () => _appliedChangesResult.Count.ShouldBe(1),
                    () => _bulkRecordUpdateResult.ShouldNotBeNull());
        }

        [Test]
        [TestCase(DisplayNameDemo7)]
        [TestCase(QualificationDateName)]
        public void BtnApply_Click_WhenNoDefaultSelected_MessageBoxShowsMessage(string displayName)
        {
            // Arrange
            var args = new RoutedEventArgs();
            _testEntity = GetRecordUpdateChangeSelector(displayName);
            _privateTestObject = new PrivateObject(_testEntity);
            var messageString = string.Empty;
            ShimMessageBox.ShowString = (msg) =>
            {
                messageString = msg;
                return MessageBoxResult.OK;
            };
            var stackpnl = _privateTestObject.GetFieldOrProperty(SpChangesControl) as StackPanel;
            var stackPanel = stackpnl.Children[0] as StackPanel;
            var datePicker = stackPanel.Children.OfType<RadDatePicker>().
                FirstOrDefault(x => x.Name.Equals(RdpDateControl, StringComparison.CurrentCultureIgnoreCase));
            if (datePicker != null)
            {
                datePicker.SelectedDate = null;
            }

            // Act
            _privateTestObject.Invoke(BtnApplyClickMethodName, this, args);

            // Assert
            messageString.ShouldSatisfyAllConditions(
                () => messageString.ShouldNotBeNullOrWhiteSpace(),
                () => messageString.ShouldContain($"{displayName} missing a response. Please select an response."));
        }

        [Test]
        [TestCase(SampleText)]
        [TestCase("")]
        public void BtnApply_Click_WhenIsNotDemographic_UpdatesResult(string text)
        {
            // Arrange
            var args = new RoutedEventArgs();
            _testEntity = GetRecordUpdateChangeSelector(SampleName, ColumnSelectionType.Adhoc);
            _privateTestObject = new PrivateObject(_testEntity);
            var messageString = string.Empty;
            ShimMessageBox.ShowString = (msg) =>
            {
                messageString = msg;
                return MessageBoxResult.OK;
            };
            var stackpnl = _privateTestObject.GetFieldOrProperty(SpChangesControl) as StackPanel;
            var stackPanel = stackpnl.Children[0] as StackPanel;
            var textBox = stackPanel.Children.OfType<TextBox>().
                FirstOrDefault(x => x.Name.Equals(TbOptionControl, StringComparison.CurrentCultureIgnoreCase));
            if (textBox != null)
            {
                textBox.Text = text;
            }

            // Act
            _privateTestObject.Invoke(BtnApplyClickMethodName, this, args);

            // Assert
            if (string.IsNullOrWhiteSpace(text))
            {
                messageString.ShouldSatisfyAllConditions(
                    () => messageString.ShouldNotBeNullOrWhiteSpace(),
                    () => messageString.ShouldContain($"Please provide a response for {SampleName}"));
            }
            else
            {
                _privateTestObject.ShouldSatisfyAllConditions(
                    () => _appliedChangesResult.ShouldNotBeNull(),
                    () => _appliedChangesResult.ShouldNotBeEmpty(),
                    () => _appliedChangesResult.Count.ShouldBe(1),
                    () => _bulkRecordUpdateResult.ShouldNotBeNull());
            }
        }

        private RecordUpdateChangeSelector GetRecordUpdateChangeSelector(
            string displayName,
            ColumnSelectionType type = ColumnSelectionType.Demographic)
        {
            _appliedChangesResult = new List<AppliedChanges>();
            _bulkRecordUpdateResult = null;
            var parent = new ShimRecordUpdate();
            parent.ReturnResultsListOfRecordUpdateChangeSelectorAppliedChangesRecordUpdateBulkRecordUpdateDetail =
                (changes, bulkRecordUpdate) =>
                {
                    _appliedChangesResult = changes;
                    _bulkRecordUpdateResult = bulkRecordUpdate;
                };
            var columnType = type.ToString();
            var recordUpdateDetails = new RecordUpdate.BulkRecordUpdateDetail();
            var pubId = 1;
            var appliedChanges = new List<AppliedChanges>
            {
                new AppliedChanges
                {
                    AppliedChange = new ColumnSelectionTemplate
                    {
                        DisplayName = displayName,
                        Type = columnType,
                        ResponseGroupID = 1
                    }
                }
            };
            var pubSubProps = new List<string> { SamplePubProp };
            var codeSheets = new List<CodeSheet> { new CodeSheet() };
            var rgList = new List<ResponseGroup> { new ResponseGroup() };
            var cList = new List<Code>
            {
                new Code
                {
                    CodeTypeId = 1,
                    CodeId = 1,
                    CodeValue = SampleCodeValue
                }
            };
            var ccList = new List<CategoryCode> { new CategoryCode() };
            var tcList = new List<TransactionCode> { new TransactionCode() };
            var ahcList = new List<string> { SampleAdhoc };
            var esList = new List<EmailStatus> { new EmailStatus() };
            var ctList = new List<CodeType>
            {
                new CodeType
                {
                    CodeTypeName = FrameworkUAD_Lookup.Enums.CodeType.Deliver.ToString(),
                    CodeTypeId = 1
                }
            };

            return new RecordUpdateChangeSelector(
                parent,
                recordUpdateDetails,
                pubId,
                appliedChanges,
                pubSubProps,
                codeSheets,
                rgList,
                cList,
                ccList,
                tcList,
                ahcList,
                esList,
                ctList);
        }

        private void InitializeControls()
        {
            if (_privateTestObject.GetFieldOrProperty(SpChangesControl) is StackPanel stackpnl)
            {
                if (stackpnl.Children[0] is StackPanel stackPanel)
                {
                    var child = stackPanel.Children.OfType<ComboBox>().
                        FirstOrDefault(x => x.Name.Equals(CbOptionControl, StringComparison.CurrentCultureIgnoreCase));
                    if (child != null)
                    {
                        child.ItemsSource = new Dictionary<string, string> { { SampleKey, SampleValue } };
                        child.SelectedIndex = 0;
                    }
                    var datePicker = stackPanel.Children.OfType<RadDatePicker>().
                        FirstOrDefault(x => x.Name.Equals(RdpDateControl, StringComparison.CurrentCultureIgnoreCase));
                    if (datePicker != null)
                    {
                        datePicker.SelectedDate = DateTime.UtcNow;
                    }
                    var stackChild = stackPanel.Children.OfType<StackPanel>().FirstOrDefault();
                    if (stackChild != null)
                    {
                        var radListBox = stackChild.Children.OfType<RadListBox>().
                            FirstOrDefault(x => x.Name.Equals(RlbOptionControl, StringComparison.CurrentCultureIgnoreCase));
                        if (radListBox != null)
                        {
                            radListBox.ItemsSource = new Dictionary<string, string> { { SampleKey, SampleValue } };
                            radListBox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }
    }
}
