using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using System.Windows.Controls.Fakes;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class DcMatchCriteriaTest
    {
        private const string BtnPreviousClickMethod = "btnPrevious_Click";
        private const string IsSaved = "isSaved";
        private const string ThisDcSteps = "thisDCSteps";
        private const string StepFourContainer = "StepFourContainer";
        private const string IsYesNo = "isYesNo";
        private const string RadioButtonYes = "rbYes";
        private const string RadioButtonNo = "rbNo";

        private PrivateObject _privateObject;
        private bool _step5ToStep4Called;
        private DCMatchCriteria _dcMatchCriteria;
        private ShimDataCompareSteps _compareSteps;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void BtnPreviousClick_YesNoTrueAndUserResponseYes_RbYesIsChecked()
        {
            // Arrange
            ArrangeControlForBtnPreivous();
            _privateObject.SetFieldOrProperty(IsYesNo, false);
            var radioButtonYes = new RadioButton();
            _privateObject.SetFieldOrProperty(RadioButtonYes, radioButtonYes);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.Yes;

            // Act
            _privateObject.Invoke(BtnPreviousClickMethod, GetArgs());

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => radioButtonYes.IsChecked.ShouldBe(true),
                () => ((bool) _privateObject.GetFieldOrProperty(IsYesNo)).ShouldBeTrue());
        }

        [Test]
        public void BtnPreviousClick_YesNoTrueAndUserResponseNo_RbNoIsChecked()
        {
            // Arrange
            ArrangeControlForBtnPreivous();
            _privateObject.SetFieldOrProperty(IsYesNo, false);
            var radioButtonNo = new RadioButton();
            _privateObject.SetFieldOrProperty(RadioButtonNo, radioButtonNo);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.No;

            // Act
            _privateObject.Invoke(BtnPreviousClickMethod, GetArgs());

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => radioButtonNo.IsChecked.ShouldBe(true),
                () => ((bool) _privateObject.GetFieldOrProperty(IsSaved)).ShouldBe(true),
                () => ((bool) _privateObject.GetFieldOrProperty(IsYesNo)).ShouldBeTrue());
        }

        [Test]
        public void BtnPreviousClick_YesNoIsTrueAndSaveIsFalseAndUserResponseYes_PreviousSucceeds()
        {
            // Arrange
            ArrangeControlForBtnPreivous();
            _privateObject.SetFieldOrProperty(IsYesNo, true);
            _privateObject.SetFieldOrProperty(IsSaved, false);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.Yes;
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
                ChildSetUIElement = (a) => { borderChildSet = true; }
            };
            ShimWPF.FindVisualChildrenOf1DependencyObject((a) => borderList);
            ShimDCDemoAttributes.ConstructorDataCompareSteps = (a, b) => { };

            // Act
            _privateObject.Invoke(BtnPreviousClickMethod, GetArgs());

            // Assert
            _step5ToStep4Called.ShouldSatisfyAllConditions(
                () => _step5ToStep4Called.ShouldBeTrue(),
                () => borderChildSet.ShouldBeTrue());

        }

        [Test]
        public void BtnPreviousClick_YesNoIsTrueAndSaveIsFalseAndUserResponseNo_PreviousFails()
        {
            // Arrange
            ArrangeControlForBtnPreivous();
            _privateObject.SetFieldOrProperty(IsYesNo, true);
            _privateObject.SetFieldOrProperty(IsSaved, false);
            ShimWPF.MessageResultStringMessageBoxButtonMessageBoxImageString = (a, b, c, f) => MessageBoxResult.No;

            // Act
            _privateObject.Invoke(BtnPreviousClickMethod, GetArgs());

            // Assert
            _step5ToStep4Called.ShouldBeFalse();
        }

        [Test]
        public void BtnPreviousClick_YesNoIsTrueAndSaveIsTrueAndUserResponseYes_PreviousSucceeds()
        {
            // Arrange
            ArrangeControlForBtnPreivous();
            _privateObject.SetFieldOrProperty(IsYesNo, true);
            _privateObject.SetFieldOrProperty(IsSaved, true);
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
                ChildSetUIElement = (a) => { borderChildSet = true; }
            };
            ShimWPF.FindVisualChildrenOf1DependencyObject(a => borderList);
            ShimDCDemoAttributes.ConstructorDataCompareSteps = (a, b) => { };

            // Act
            _privateObject.Invoke(BtnPreviousClickMethod, GetArgs());

            // Assert
            _step5ToStep4Called.ShouldSatisfyAllConditions(
                () => _step5ToStep4Called.ShouldBeTrue(),
                () => borderChildSet.ShouldBeTrue());

        }

        private void ArrangeControlForBtnPreivous()
        {
            ShimDCMatchCriteria.ConstructorDataCompareSteps = (a, b) => { };
            _compareSteps = new ShimDataCompareSteps
            {
                Step5ToStep4 = () => _step5ToStep4Called = true
            };

            _dcMatchCriteria = new DCMatchCriteria(_compareSteps);
            _privateObject = new PrivateObject(_dcMatchCriteria);
            _privateObject.SetFieldOrProperty(ThisDcSteps, _compareSteps.Instance);
        }

        private static object[] GetArgs()
        {
            return new object[2] {null, new RoutedEventArgs()};
        }
    }
}

