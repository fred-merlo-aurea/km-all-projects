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
	public class ToolSuperscriptTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSuperscript = new Mock<ToolSuperscript>();

			toolSuperscript.Setup(x => x.Page).Returns(new Page());
			toolSuperscript.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolSuperscript.CallBase = true;

				// Act
				toolSuperscript.Object.RenderDesign(output);

				// Assert
				toolSuperscript.Object.ShouldSatisfyAllConditions(
					() => toolSuperscript.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSuperscript.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSuperscript = new Mock<ToolSuperscript>();

			toolSuperscript.Setup(x => x.Page).Returns(new Page());
			toolSuperscript.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolSuperscript.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolSuperscript.CallBase = true;
				var privateObject = new PrivateObject(toolSuperscript.Object, new PrivateType(typeof(ToolSuperscript)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolSuperscript.Object.ShouldSatisfyAllConditions(
					() => toolSuperscript.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSuperscript.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolSuperscript.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
