using System.Diagnostics.CodeAnalysis;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.ActivePanel
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class PanelGroupTest
	{
		[Test]
		public void ExternalScript_DefaultValue()
		{
			// Arrange
			var panelGroup = new PanelGroup();

			// Act, Assert
			panelGroup.ExternalScript.ShouldBeEmpty();
		}

		[Test]
		public void ExternalScript_SetAndGetValue()
		{
			// Arrange
			var panelGroup = new PanelGroup();

			// Act
			panelGroup.ExternalScript = "Test1";

			// Assert
			panelGroup.ExternalScript.ShouldBe("Test1");
		}
	}
}
