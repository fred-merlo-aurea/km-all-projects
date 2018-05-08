using System.Diagnostics.CodeAnalysis;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActiveAjax
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class AjaxPanelTest
	{
		[Test]
		public void ExternalScript_DefaultValue()
		{
			// Arrange
			var ajaxPanel = new AjaxPanel();

			// Act, Assert
			ajaxPanel.ExternalScript.ShouldBeEmpty();
		}

		[Test]
		public void ExternalScript_SetAndGetValue()
		{
			// Arrange
			var ajaxPanel = new AjaxPanel();

			// Act
			ajaxPanel.ExternalScript = "Test1";

			// Assert
			ajaxPanel.ExternalScript.ShouldBe("Test1");
		}
	}
}
