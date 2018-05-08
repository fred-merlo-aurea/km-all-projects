using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.controls;
using NUnit.Framework;
using Shouldly;

namespace Ecn.Controls.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TabBarHelpersTests
    {
        private const string ImageUrlTemplate = "image{0}.jpg";

        private IList<ImageButton> _tabs;

        [SetUp]
        public void SetUp()
        {
            _tabs = new List<ImageButton>();
        }

        [Test]
        [TestCase(-1, 1, 2)]
        [TestCase(0, 1, 2)]
        [TestCase(3, -1, 2)]
        [TestCase(3, 1, -1)]
        public void EnableTabBar_InvalidArguments_ThrowsException(
            int numberOfWizardSteps, 
            int currentWizardStepIndex,
            int completedWizardStepIndex)
        {
            // Arrange
            _tabs.Clear();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                TabBarHelpers.EnableTabBar(
                    numberOfWizardSteps,
                    currentWizardStepIndex,
                    completedWizardStepIndex,
                    GetTab,
                    GetCurrentWizardStepImageUrl,
                    GetUncompletedWizardStepImageUrl,
                    GetCompletedWizardStepImageUrl,
                    GetUncompletedWizardStepImageUrl,
                    GetUncompletedWizardStepImageUrl)
            );
        }

        [Test]
        public void EnableTabBar_ValidArguments_SetsCorrectTabProperties()
        {
            // Arrange
            _tabs.Clear();

            var numberOfWizardSteps = 3;
            var currentWizardStepIndex = 1;
            var completedWizardStepIndex = 2;

            // Act
            TabBarHelpers.EnableTabBar(
                numberOfWizardSteps,
                currentWizardStepIndex,
                completedWizardStepIndex,
                GetTab,
                GetCurrentWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl,
                GetCompletedWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl);
            var actualNumberOfTabs = _tabs.Count;

            // Assert
            actualNumberOfTabs.ShouldSatisfyAllConditions(
                () => actualNumberOfTabs.ShouldBe(numberOfWizardSteps),
                () => _tabs[0].ImageUrl.ShouldBe(GetCurrentWizardStepImageUrl(currentWizardStepIndex)),
                () => _tabs[0].Enabled.ShouldBe(false),
                () => _tabs[1].ImageUrl.ShouldBe(GetCompletedWizardStepImageUrl(completedWizardStepIndex)),
                () => _tabs[1].Enabled.ShouldBe(true),
                () => _tabs[2].ImageUrl.ShouldBe(GetUncompletedWizardStepImageUrl(numberOfWizardSteps)),
                () => _tabs[2].Enabled.ShouldBe(false));
        }

        [Test]
        public void EnableTabBar_WithSomeNullTabs_IgnoresNullTabs()
        {
            // Arrange
            _tabs.Clear();

            var numberOfWizardSteps = 5;
            var currentWizardStepIndex = 1;
            var completedWizardStepIndex = 2;

            // Act
            TabBarHelpers.EnableTabBar(
                numberOfWizardSteps,
                currentWizardStepIndex,
                completedWizardStepIndex,
                GetPotentiallyNullTab,
                GetCurrentWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl,
                GetCompletedWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl,
                GetUncompletedWizardStepImageUrl);
            var actualNumberOfTabs = _tabs.Count;

            // Assert
            actualNumberOfTabs.ShouldBe(numberOfWizardSteps - 1);
        }

        private ImageButton GetTab(int stepIndex)
        {
            var tab = new ImageButton
            {
                ID = stepIndex.ToString()
            };
            _tabs.Add(tab);

            return tab;
        }

        private ImageButton GetPotentiallyNullTab(int stepIndex)
        {
            if (stepIndex == 1)
            {
                return null;
            }

            return GetTab(stepIndex);
        }

        private static string GetCompletedWizardStepImageUrl(int stepIndex)
        {
            return string.Format(ImageUrlTemplate, stepIndex);
        }

        private static string GetUncompletedWizardStepImageUrl(int stepIndex)
        {
            return string.Format(ImageUrlTemplate, stepIndex);
        }

        private static string GetCurrentWizardStepImageUrl(int stepIndex)
        {
            return string.Format(ImageUrlTemplate, stepIndex);
        }
    }
}
