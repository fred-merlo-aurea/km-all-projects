using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Fakes;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules.Fakes;
using FrameworkUAD_Lookup.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;
using WpfControls.Helpers;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class DcDemoAttributesTest
    {
        private const string CheckboxChecked = "CheckBox_Checked";
        private const string DemoTypeAll = "All";
        private const string DemoTypeCustom = "Custom";
        private const string SpStandard = "spStandard";
        private const string SpPremium = "spPremium";
        private const string BtnSave = "btnSave";
        private const string DemoStandards = "demoStandards";
        private const string DemoPremiums = "demoPremiums";
        private const string DemoTypeNone = "None";
        private const string ListBoxStandard = "ListBoxStandard";
        private const string ListBoxPremium = "ListBoxPremium";

        private PrivateObject _privateObject;
        private CheckBox _sender;
        private DCDemoAttributes _dcDemoAttributes;
        private ShimDataCompareSteps _compareSteps;
        private List<Code> _profileCodes;
        private bool _loadAttributesCalled;
        private bool _loadCheckBoxListsCalled;
        private bool _saveCalled;
        private string _demoSelection;
        private RoutedEventArgs _routedEventArgs;
        private object[] _args;

        [Test]
        public void CheckBoxChecked_All_SetsProfileCodes()
        {
            // Arrange
            ArrangeControl(DemoTypeAll);
            var listBoxStandard = new RadListBox();
            var checkStandardList = new ObservableCollection<CheckBoxItem>
            {
                new CheckBoxItem(),
                new CheckBoxItem()
            };
            listBoxStandard.ItemsSource = checkStandardList;

            var listBoxPremium = new RadListBox();
            var checkPremiumBoxList = new ObservableCollection<CheckBoxItem>
            {
                new CheckBoxItem(),
                new CheckBoxItem()
            };
            listBoxPremium.ItemsSource = checkPremiumBoxList;

            _privateObject.SetFieldOrProperty(ListBoxStandard, listBoxStandard);
            _privateObject.SetFieldOrProperty(ListBoxPremium, listBoxPremium);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _demoSelection.ShouldBe(DemoTypeAll),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpStandard)).Visibility.ShouldBe(Visibility
                    .Collapsed),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpPremium)).Visibility.ShouldBe(Visibility
                    .Collapsed),
                () => ((RadButton)_privateObject.GetFieldOrProperty(BtnSave)).Visibility
                    .ShouldBe(Visibility.Collapsed),
                () => _loadAttributesCalled.ShouldBe(true),
                () => checkStandardList.ShouldAllBe(c => c.IsChecked),
                () => checkPremiumBoxList.ShouldAllBe(c => c.IsChecked),
                () => _saveCalled.ShouldBe(true));
        }

        [Test]
        public void CheckBoxChecked_CustomCountGreatenThanZero_ProfileAttributesVisible()
        {
            // Arrange
            ArrangeControl(DemoTypeCustom);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _demoSelection.ShouldBe(DemoTypeCustom),
                () => _loadAttributesCalled.ShouldBeTrue(),
                () => _loadCheckBoxListsCalled.ShouldBeTrue());
        }

        [TestCase(0, 0, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed)]
        [TestCase(0, 1, Visibility.Collapsed, Visibility.Visible, Visibility.Visible)]
        [TestCase(1, 0, Visibility.Visible, Visibility.Collapsed, Visibility.Visible)]
        [TestCase(1, 1, Visibility.Visible, Visibility.Visible, Visibility.Visible)]
        public void CheckBoxChecked_CustomDifferentCounts_DifferentVisibilities(
            int standartCount,
            int premiumCount,
            Visibility standardVisibility,
            Visibility premiumVisibility,
            Visibility saveButtonVisibility)
        {
            // Arrange
            ArrangeControl(DemoTypeCustom);
            SetCode(standartCount, premiumCount);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpStandard)).Visibility.ShouldBe(standardVisibility),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpPremium)).Visibility.ShouldBe(premiumVisibility),
                () => ((RadButton)_privateObject.GetFieldOrProperty(BtnSave)).Visibility.ShouldBe(saveButtonVisibility));
        }

        [Test]
        public void CheckBoxChecked_CustomCountIsZero_NoLoadCheckBoxes()
        {
            // Arrange
            ArrangeControl(DemoTypeCustom);
            var standartCount = 0;
            var premiumCount = 0;
            SetCode(standartCount, premiumCount);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _loadCheckBoxListsCalled.ShouldBeFalse());
        }

        [Test]
        public void CheckBoxChecked_None_ProfileAttributesVisible()
        {
            // Arrange
            ArrangeControl(DemoTypeNone);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _demoSelection.ShouldBe(DemoTypeNone),
                () => _loadAttributesCalled.ShouldBeTrue(),
                () => ((bool)_privateObject.GetFieldOrProperty("didSave")).ShouldBeTrue(),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpStandard)).Visibility.ShouldBe(Visibility
                    .Collapsed),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpPremium)).Visibility.ShouldBe(
                    Visibility.Collapsed),
                () => ((RadButton)_privateObject.GetFieldOrProperty(BtnSave)).Visibility.ShouldBe(Visibility.Collapsed));
        }

        private void ArrangeControl(string textBlock)
        {
            ShimDCDemoAttributes.AllInstances.LoadAttributes = m => { _loadAttributesCalled = true; };
            ShimDCDemoAttributes.AllInstances.LoadCheckBoxLists = m => { _loadCheckBoxListsCalled = true; };
            ShimDCDemoAttributes.AllInstances.Save = m => { _saveCalled = true; };
            ShimWPF.FindVisualChildrenOf1DependencyObject(a => new List<TextBlock>
            {
                new TextBlock
                {
                    Text = textBlock
                }
            });

            ShimWPF.FindVisualChildrenOf1DependencyObject(a => new List<CheckBox>
            {
                _sender,
                new CheckBox()
            });

            CreateDemoAttributes();
            CreateArgs();
        }

        private void CreateDemoAttributes()
        {
            _compareSteps = new ShimDataCompareSteps();
            _profileCodes = new List<Code>();
            _compareSteps.demoSelectionSetString = profileSelection => _demoSelection = profileSelection;
            _compareSteps.demoCodesGet = () => _profileCodes;

            _dcDemoAttributes = new DCDemoAttributes(_compareSteps);

            ShimFrameworkElement.AllInstances.ParentGet = a => _dcDemoAttributes;

            _privateObject = new PrivateObject(_dcDemoAttributes);

            var standardCount = 1;
            var premiumCount = 1;
            SetCode(standardCount, premiumCount);
        }

        private void SetCode(int standartCount, int premiumCount)
        {
            var codesStandard = new List<Code>();
            for (var standardIndex = 0; standardIndex < standartCount; standardIndex++)
            {
                codesStandard.Add(new Code());
            }

            var codesPremium = new List<Code>();
            for (var premiumIndex = 0; premiumIndex < premiumCount; premiumIndex++)
            {
                codesPremium.Add(new Code());
            }
            _privateObject.SetFieldOrProperty(DemoStandards, codesStandard);
            _privateObject.SetFieldOrProperty(DemoPremiums, codesPremium);
        }

        private void CreateArgs()
        {
            _routedEventArgs = new RoutedEventArgs();
            _sender = new CheckBox { IsChecked = true };
            _args = new object[2] { _sender, _routedEventArgs };
        }

        private void CheckBoxCheckInit()
        {
            _loadAttributesCalled = false;
            _loadCheckBoxListsCalled = false;
            _saveCalled = false;
            _demoSelection = null;
            _args = null;
            _sender = null;
            _privateObject = null;
        }
    }
}
