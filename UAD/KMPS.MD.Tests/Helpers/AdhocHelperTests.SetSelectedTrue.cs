using System;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using KMPS.MD.Helpers;
using KMPS.MD.Helpers.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    public partial class AdhocHelperTests
    {
        [Test]
        public void SetSelectedTrue_NullDropDownList_ThrowsArgumentNull()
        {
            // Arrange
            ListControl listControlNull = null;
            var value = string.Empty;

            //Act, Assert
            Should.Throw<ArgumentNullException>(() => AdhocHelper.SetSelectedTrue(listControlNull, value));
        }

        [Test]
        public void SetSelectedTrue_NullField_ThrowsArgumentNull()
        {
            // Arrange
            var listControl = new DropDownList();
            var value = string.Empty;
            var foundListItem = new ListItem();
            ShimListItemCollection.AllInstances.FindByValueString = (list, key) => foundListItem;

            //Act
            AdhocHelper.SetSelectedTrue(listControl, value);

            //Assert
            foundListItem.Selected.ShouldBeTrue();
        }
    }
}