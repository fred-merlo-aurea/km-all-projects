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
	public class ToolPasteTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolPaste = new Mock<ToolPaste>();

			toolPaste.Setup(x => x.Page).Returns(new Page());
			toolPaste.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolPaste.CallBase = true;

				// Act
				toolPaste.Object.RenderDesign(output);

				// Assert
				toolPaste.Object.ShouldSatisfyAllConditions(
					() => toolPaste.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolPaste.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolPaste = new Mock<ToolPaste>();

			toolPaste.Setup(x => x.Page).Returns(new Page());
			toolPaste.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolPaste.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolPaste.CallBase = true;
				var privateObject = new PrivateObject(toolPaste.Object, new PrivateType(typeof(ToolPaste)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolPaste.Object.ShouldSatisfyAllConditions(
					() => toolPaste.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolPaste.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolPaste.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
