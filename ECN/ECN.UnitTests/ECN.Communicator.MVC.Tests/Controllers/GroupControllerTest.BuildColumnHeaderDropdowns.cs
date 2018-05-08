using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string ignore = "Ignore";
        private const string boxName = "name";
        private const string colName = "test";
        private const int id = 1;

        [Test]
        public void BuildColumnHeaderDropdowns_PassColumnWhichExist_ReturnSelectWithSelectedTextAndHideIgnored()
        {
            // Arrange
            var field = new GroupDataFields()
            {
                ShortName = colName
            };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (grpId, user, getChild) => new List<GroupDataFields>() { field };

            // Act
            var result = _testObject.Invoke("buildColumnHeaderDropdowns", boxName, colName, id) as HtmlSelect;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Items.FindByText(ignore).Selected.ShouldBeFalse(),
                () => result.Items.FindByText($"user_{colName}").Selected.ShouldBeTrue());
        }

        [Test]
        public void BuildColumnHeaderDropdowns_PassColumnWhichNotExist_ReturnSelectSelectedIgnored()
        {
            // Arrange
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (grpId, user, getChild) => new List<GroupDataFields>();

            // Act
            var result = _testObject.Invoke("buildColumnHeaderDropdowns", boxName, colName, id) as HtmlSelect;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Items.FindByText(ignore).Selected.ShouldBeTrue(),
                () => result.Items.FindByText(colName).ShouldBeNull(),
                () => result.Items.FindByText($"user_{colName}").ShouldBeNull());
        }
    }
}