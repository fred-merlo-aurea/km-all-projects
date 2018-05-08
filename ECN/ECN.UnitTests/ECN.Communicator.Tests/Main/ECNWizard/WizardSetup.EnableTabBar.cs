using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard;
using ECN.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class WizardSetupEnableTabBar : PageHelper
    {
        private wizardSetup _wizardSetup;
        private PrivateObject _wizardSetupPrivate;

        [SetUp]
        public void Setup()
        {
            QueryString.Clear();
            _wizardSetup = new wizardSetup();
            _wizardSetupPrivate = new PrivateObject(_wizardSetup);
            InitializeAllControls(_wizardSetup);
        }

        [Test]
        [TestCaseSource(nameof(CasesGenerator))]
        public void EnableTabBar_DifferentCampaignItemTypes_ShouldAddControl(
            string campaignItemType,
            string expectedId,
            int completedStep,
            int stepIndex)
        {
            //Arrange
            QueryString.Add("CampaignItemType", campaignItemType);
            var tabsCollectionTable = Get<HtmlTable>(_wizardSetupPrivate, "tabsCollectionTable");
            var viewState = Get<StateBag>(_wizardSetupPrivate, "ViewState");
            viewState["StepIndex"] = stepIndex;
            //Act
            CallEnableTabBar(completedStep);
            //Assert
            tabsCollectionTable.ShouldSatisfyAllConditions(
                () => tabsCollectionTable.ShouldNotBeNull(),
                () => tabsCollectionTable.Rows.ShouldNotBeNull());
            var imageButtons = tabsCollectionTable.Rows
                .OfType<HtmlTableRow>()
                .SelectMany(row => row.Cells.OfType<HtmlTableCell>())
                .SelectMany(cell => cell.Controls.OfType<ImageButton>());
            imageButtons.ShouldSatisfyAllConditions(
                () => imageButtons.ShouldNotBeEmpty(),
                () => imageButtons.ShouldContain(image => image.ID.ToLower().Contains(expectedId)));
        }

        [Test]
        public void EnableTabBar_InvalidType_ShouldNotAddControl()
        {
            //Arrange
            var completedStep = 1;
            var stepIndex = 0;
            var campaignItemType = "some invalid string";
            QueryString.Add("CampaignItemType", campaignItemType);
            var tabsCollectionTable = Get<HtmlTable>(_wizardSetupPrivate, "tabsCollectionTable");
            var viewState = Get<StateBag>(_wizardSetupPrivate, "ViewState");
            viewState["StepIndex"] = stepIndex;
            //Act
            CallEnableTabBar(completedStep);
            //Assert
            tabsCollectionTable.ShouldSatisfyAllConditions(
                () => tabsCollectionTable.ShouldNotBeNull(),
                () => tabsCollectionTable.Rows.ShouldNotBeNull(),
                () => tabsCollectionTable.Rows.Count.ShouldBe(2));
        }

        private static IEnumerable<object[]> CasesGenerator()
        {
            var stringItems = new object[][]
            {
                new object[]
                {
                    "regular", "regular"
                },
                new object[]
                {
                    "sms", "sms"
                },
                new object[]
                {
                    "ab", "ab"
                },
                new object[]{
                    "champion", "champion"
                },
                new object[]
                {
                    "salesforce", "sf"
                },
                new object[]{
                    "regular", "regular"
                }
            };
            var numbersItems = new object[][]
            {
                new object[]{1, 0},
                new object[]{0, 1},
                new object[]{0, 0},
                new object[]{2, 0},
                new object[]{0, 2},
                new object[]{4, 0},
                new object[]{0, 4},
                new object[]{5, 0},
                new object[]{5, 0},
                new object[]{0, 5},

            };
            foreach (var stringsItem in stringItems)
            {
                foreach (var numbers in numbersItems)
                {
                    yield return stringsItem.Concat(numbers).ToArray();
                }
            }
        }

        private void CallEnableTabBar(int CompletedStep)
        {
            _wizardSetupPrivate.Invoke("EnableTabBar", new object[] { CompletedStep });
        }
    }
}
