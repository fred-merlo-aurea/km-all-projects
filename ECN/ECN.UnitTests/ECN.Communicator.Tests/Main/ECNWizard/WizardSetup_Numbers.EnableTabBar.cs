using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    public partial class WizardSetupNumbersTest
    {
        [Test]
        [TestCaseSource(nameof(CasesGenerator))]
        public void EnableTabBar_DifferentBlastTypes_ShouldAddControl(
           string blastType,
           string expectedId,
           int completedStep,
           int stepIndex)
        {
            //Arrange
            InitTestEnableTabBar();
            _queryString.Add(BlastTypeName, blastType);
            var tabsCollectionTable = Get<HtmlTable>("tabsCollectionTable");
            var viewState = Get<StateBag>("ViewState");
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
            InitTestEnableTabBar();
            var completedStep = 1;
            var stepIndex = 0;
            var blastType = "some invalid string";
            _queryString.Add(BlastTypeName, blastType);
            var tabsCollectionTable = InitField<HtmlTable>("tabsCollectionTable");
            var viewState = Get<StateBag>("ViewState");
            viewState["StepIndex"] = stepIndex;

            //Act
            CallEnableTabBar(completedStep);

            //Assert
            tabsCollectionTable.ShouldSatisfyAllConditions(
                () => tabsCollectionTable.ShouldNotBeNull(),
                () => tabsCollectionTable.Rows.ShouldNotBeNull(),
                () => tabsCollectionTable.Rows.Count.ShouldBe(2));
        }

        private void InitTestEnableTabBar()
        {
            InitializeAllControls();
            var stateBag = new StateBag();
            stateBag["tabTypes"] = new ArrayList()
            {
               "CampaignInfo",
               "Content",
               "GroupInfo",
               "Preview",
               "BlastSchedule"
            };
            ShimStateBag.AllInstances.ItemSetStringObject = (bag, key, obj) =>
            {
                if (key == "tabTypes" && obj is ArrayList)
                {
                    var newTabTypesArray = obj as ArrayList;
                    var originalTabTypesArray = bag["tabTypes"] as ArrayList;
                    if (originalTabTypesArray != null && newTabTypesArray != null)
                    {
                        originalTabTypesArray.AddRange(newTabTypesArray);
                    }
                }
            };
            ShimControl.AllInstances.ViewStateGet = (control) => stateBag;
        }
        private void CallEnableTabBar(int completedStep)
        {
            _privateObject.Invoke(EnableTabBar, new object[] { completedStep });
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
    }
}
