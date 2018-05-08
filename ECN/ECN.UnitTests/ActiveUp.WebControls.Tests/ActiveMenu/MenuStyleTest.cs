using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveMenu
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class MenuStyleTest
	{
		[Test]
		public void ForeColor_DefaultValue()
		{
			// Arrange
			var menuStyle = new MenuStyle();

			// Act, Assert
			menuStyle.ForeColor.ShouldBe(Color.Empty);
		}

		[Test]
		public void ForeColor_SetAndGetValue()
		{
			// Arrange
			var menuStyle = new MenuStyle();

			// Act
			menuStyle.ForeColor = Color.Green;

			// Assert
			menuStyle.ForeColor.ShouldBe(Color.Green);
		}
	}
}
