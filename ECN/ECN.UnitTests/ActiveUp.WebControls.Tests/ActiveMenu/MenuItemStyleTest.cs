using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveMenu
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class MenuItemStyleTest
	{
		[Test]
		public void ForeColor_DefaultValue()
		{
			// Arrange
			var menuItemStyle = new MenuItemStyle();

			// Act, Assert
			menuItemStyle.ForeColor.ShouldBe(Color.Empty);
		}

		[Test]
		public void ForeColor_SetAndGetValue()
		{
			// Arrange
			var menuItemStyle = new MenuItemStyle();

			// Act
			menuItemStyle.ForeColor = Color.Green;

			// Assert
			menuItemStyle.ForeColor.ShouldBe(Color.Green);
		}
	}
}
