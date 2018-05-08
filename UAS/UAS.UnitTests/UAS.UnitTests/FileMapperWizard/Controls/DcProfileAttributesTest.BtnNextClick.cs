using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Fakes;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class DcProfileAttributesTest
    {
        private const string StepFourContainer = "StepFourContainer";
        private const string DidSave = "didSave";
        private const string BtnNextClick = "btnNext_Click";
        private const string ThisDcSteps = "thisDCSteps";
        private bool _step3ToStep4Called;

        [Test]
        public void BtnNext_DidSaveIsTrue_NextSucceeds()
        {
            // Arrange
            ArrangeControlForBtnNext();
            var borderChildSet = false;
            var border = new Border
            {
                Name = StepFourContainer,
                Child = null
            };
            var borderList = new List<Border>
            {
                border
            };
            var parentDecorator = new ShimDecorator(border)
            {
                ChildSetUIElement = (a) =>
                {
                    borderChildSet = true;
                }
            };
            _privateObject.SetFieldOrProperty(DidSave, true);
            ShimWPF.FindVisualChildrenOf1DependencyObject((a) => borderList);

            // Act
            _privateObject.Invoke(BtnNextClick, GetArgs());

            // Assert
            _step3ToStep4Called.ShouldSatisfyAllConditions(
                () => _step3ToStep4Called.ShouldBeTrue(),
                () => borderChildSet.ShouldBeTrue());
        }

        [Test]
        public void BtnNext_DidSaveIsFalseAndMessageResultNo_NextFails()
        {
            // Arrange
            ArrangeControlForBtnNext();
            _privateObject.SetFieldOrProperty(DidSave, false);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.No;

            // Act
            _privateObject.Invoke(BtnNextClick, GetArgs());

            // Assert
            _step3ToStep4Called.ShouldSatisfyAllConditions(
                () => _step3ToStep4Called.ShouldBeFalse());
        }

        [Test]
        public void BtnNext_DidSaveIsFalseAndMessageResultYes_NextSucceeds()
        {
            // Arrange
            ArrangeControlForBtnNext();
            var borderChildSet = false;
            var border = new Border
            {
                Name = StepFourContainer,
                Child = null
            };
            var borderList = new List<Border>
            {
                border
            };
            new ShimDecorator(border)
            {
                ChildSetUIElement = (a) =>
                {
                    borderChildSet = true;
                }
            };

            _privateObject.SetFieldOrProperty(DidSave, false);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c,f) => MessageBoxResult.Yes;
            ShimWPF.FindVisualChildrenOf1DependencyObject((a) => borderList);

            // Act
            _privateObject.Invoke(BtnNextClick, GetArgs());

            // Assert
            _step3ToStep4Called.ShouldSatisfyAllConditions(
                () => _step3ToStep4Called.ShouldBeTrue(),
                () => borderChildSet.ShouldBeTrue());
        }

        private void ArrangeControlForBtnNext()
        {
            ShimDCDemoAttributes.ConstructorDataCompareSteps = (a, b) => { };
            ShimDCProfileAttributes.ConstructorDataCompareSteps = (a, b) => { };
            _compareSteps = new ShimDataCompareSteps
            {
                Step3ToStep4 = () => _step3ToStep4Called = true
            };

            _dcProfileAttributes = new DCProfileAttributes(_compareSteps);
            _privateObject = new PrivateObject(_dcProfileAttributes);
            _privateObject.SetFieldOrProperty(ThisDcSteps, _compareSteps.Instance);
        }

        private static object[] GetArgs()
        {
            return new object[2] {null, new RoutedEventArgs()};
        }
    }
}
