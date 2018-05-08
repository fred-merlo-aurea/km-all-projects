using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
	[TestFixture]
	public class ToolRuleTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolRule = new Mock<ToolRule>();

			toolRule.Setup(x => x.Page).Returns(new Page());
			toolRule.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

			using (ShimsContext.Create())
			{
				var count = 0;
				ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) => 
				{
					if (count == 0)
					{
						count++;
						return DummyImageUrl;
					}
					else
					{
						return DummyOverImageUrl;
					}
				};

				var output = new HtmlTextWriter(new StringWriter());
				toolRule.CallBase = true;

				// Act
				toolRule.Object.RenderDesign(output);

				// Assert
				toolRule.Object.ShouldSatisfyAllConditions(
					() => toolRule.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolRule.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolRule = new Mock<ToolRule>();

			toolRule.Setup(x => x.Page).Returns(new Page());
			toolRule.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolRule.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

			using (ShimsContext.Create())
			{
				var count = 0;
				ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (type, obj1, obj2) =>
				{
					if (count == 0)
					{
						count++;
						return DummyImageUrl;
					}
					else
					{
						return DummyOverImageUrl;
					}
				};

				toolRule.CallBase = true;
				var privateObject = new PrivateObject(toolRule.Object, new PrivateType(typeof(ToolRule)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolRule.Object.ShouldSatisfyAllConditions(
					() => toolRule.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolRule.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolRule.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
