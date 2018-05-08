using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Fakes;
using System.Xml.Linq;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules.Fakes;
using FrameworkUAD_Lookup.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Controls;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class DcProfileAttributesTest
    {
        private const string CheckboxChecked = "CheckBox_Checked";
        private const string ProfileTypeAll = "All";
        private const string ProfileTypeCustom = "Custom";
        private const string SpStandard = "spStandard";
        private const string SpPremium = "spPremium";
        private const string BtnSave = "btnSave";
        private const string ProfileStandards = "profileStandards";
        private const string ProfilePremiums = "profilePremiums";
        private static readonly string AllProfileAttributes = "<List>" + Environment.NewLine +
                                                              "<Item>" + Environment.NewLine +
                                                              "<CodeTypeId>0</CodeTypeId>" + Environment.NewLine +
                                                              "<CodeId>0</CodeId>" + Environment.NewLine +
                                                              "</Item>" + Environment.NewLine +
                                                              "<Item>" + Environment.NewLine +
                                                              "<CodeTypeId>0</CodeTypeId>" + Environment.NewLine +
                                                              "<CodeId>0</CodeId>" + Environment.NewLine +
                                                              "</Item>" + Environment.NewLine +
                                                              "</List>";

        private PrivateObject _privateObject;
        private CheckBox _sender;
        private DCProfileAttributes _dcProfileAttributes;
        private ShimDataCompareSteps _compareSteps;
        private List<Code> _profileCodes;
        private bool _loadAttributesCalled;
        private bool _loadCheckBoxListsCalled;
        private XDocument _profileAttributes;
        private string _profileSelection;
        private RoutedEventArgs _routedEventArgs;
        private object[] _args;
 
        [Test]
        public void CheckBoxChecked_All_SetsProfileCodes()
        {
            // Arrange
            ArrangeControl(ProfileTypeAll);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _profileAttributes.ToString().ShouldBe(XDocument.Parse(AllProfileAttributes).ToString()),
                () => _loadAttributesCalled.ShouldBeTrue(),
                () => _loadCheckBoxListsCalled.ShouldBeTrue(),
                () => ((bool)_privateObject.GetFieldOrProperty("didSave")).ShouldBeTrue(),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpStandard)).Visibility.ShouldBe(Visibility
                    .Collapsed),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpPremium)).Visibility.ShouldBe(Visibility
                    .Collapsed),
                () => ((RadButton)_privateObject.GetFieldOrProperty(BtnSave)).Visibility
                    .ShouldBe(Visibility.Collapsed),
                () => _profileSelection.ShouldBe(ProfileTypeAll));
        }

        [Test]
        public void CheckBoxChecked_Custom_ProfileAttributesVisible()
        {
            // Arrange
            ArrangeControl(ProfileTypeCustom);

            // Act
            _privateObject.Invoke(CheckboxChecked, _args);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => _loadAttributesCalled.ShouldBeTrue(),
                () => _loadCheckBoxListsCalled.ShouldBeTrue(),
                () => ((bool)_privateObject.GetFieldOrProperty("didSave")).ShouldBeFalse(),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpStandard)).Visibility.ShouldBe(Visibility
                    .Visible),
                () => ((StackPanel)_privateObject.GetFieldOrProperty(SpPremium)).Visibility.ShouldBe(
                    Visibility.Visible),
                () => ((RadButton)_privateObject.GetFieldOrProperty(BtnSave)).Visibility.ShouldBe(Visibility.Visible),
                () => _profileSelection.ShouldBe(ProfileTypeCustom));
        }

        private void ArrangeControl(string textBlock)
        {
            ShimDCProfileAttributes.AllInstances.LoadAttributes = m => { _loadAttributesCalled = true; };
            ShimDCProfileAttributes.AllInstances.LoadCheckBoxLists = m => { _loadCheckBoxListsCalled = true; };
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


            CreateProfileAttributes();
            CreateArgsForCheckBoxCheck();
        }

        private void CreateProfileAttributes()
        {
            _compareSteps = new ShimDataCompareSteps();
            _profileCodes = new List<Code>();
            _compareSteps.profileSelectionSetString = profileSelection => _profileSelection = profileSelection;
            _compareSteps.profileCodesGet = () => _profileCodes;
            _compareSteps.profileAttributesSetXDocument = doc => _profileAttributes = doc;
            _dcProfileAttributes = new DCProfileAttributes(_compareSteps);

            ShimFrameworkElement.AllInstances.ParentGet = a => _dcProfileAttributes;

            _privateObject = new PrivateObject(_dcProfileAttributes);
            var codes = new List<Code>
            {
                new Code()
            };

            _privateObject.SetFieldOrProperty(ProfileStandards, codes);
            _privateObject.SetFieldOrProperty(ProfilePremiums, codes);
        }

        private void CreateArgsForCheckBoxCheck()
        {
            _routedEventArgs = new RoutedEventArgs();
            _sender = new CheckBox { IsChecked = true };
            _args = new object[2] { _sender, _routedEventArgs };
        }

        private void CheckBoxCheckInit()
        {
            _loadAttributesCalled = false;
            _loadCheckBoxListsCalled = false;
            _args = null;
            _sender = null;
            _privateObject = null;
        }
    }
}
