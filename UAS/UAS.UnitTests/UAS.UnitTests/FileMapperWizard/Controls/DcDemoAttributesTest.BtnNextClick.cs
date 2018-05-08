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
    public partial class DcDemoAttributesTest
    {
        private const string StepFiveContainer = "StepFiveContainer";
        private const string DidSave = "didSave";
        private const string BtnNextClick = "btnNext_Click";
        private const string ThisDcSteps = "thisDCSteps";
        private bool _step4ToStep5Called;

        [Test]
        public void BtnNext_DidSaveIsTrue_NextSucceeds()
        {
            // Arrange
            ArrangeControlForBtnNext();
            var borderChildSet = false;
            var border = new Border
            {
                Name = StepFiveContainer,
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
            _step4ToStep5Called.ShouldSatisfyAllConditions(
                () => _step4ToStep5Called.ShouldBeTrue(),
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
            _step4ToStep5Called.ShouldSatisfyAllConditions(
                () => _step4ToStep5Called.ShouldBeFalse());
        }

        [Test]
        public void BtnNext_DidSaveIsFalseAndMessageResultYes_NextSucceeds()
        {
            // Arrange
            ArrangeControlForBtnNext();
            var borderChildSet = false;
            var border = new Border
            {
                Name = StepFiveContainer,
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
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.Yes;
            ShimWPF.FindVisualChildrenOf1DependencyObject((a) => borderList);

            // Act
            _privateObject.Invoke(BtnNextClick, GetArgs());

            // Assert
            _step4ToStep5Called.ShouldSatisfyAllConditions(
                () => _step4ToStep5Called.ShouldBeTrue(),
                () => borderChildSet.ShouldBeTrue());
        }

        private void ArrangeControlForBtnNext()
        {
            ShimDCDemoAttributes.ConstructorDataCompareSteps = (a, b) => { };
            ShimDCMatchCriteria.ConstructorDataCompareSteps = (a, b) => { };
            _compareSteps = new ShimDataCompareSteps
            {
                Step4ToStep5 = () => _step4ToStep5Called = true
            };

            _dcDemoAttributes = new DCDemoAttributes(_compareSteps);
            _privateObject = new PrivateObject(_dcDemoAttributes);
            _privateObject.SetFieldOrProperty(ThisDcSteps, _compareSteps.Instance);
        }

        private static object[] GetArgs()
        {
            return new object[2] { null, new RoutedEventArgs() };
        }
    }
}
