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
	public class ToolPrintTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolPrint = new Mock<ToolPrint>();

			toolPrint.Setup(x => x.Page).Returns(new Page());
			toolPrint.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolPrint.CallBase = true;

				// Act
				toolPrint.Object.RenderDesign(output);

				// Assert
				toolPrint.Object.ShouldSatisfyAllConditions(
					() => toolPrint.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolPrint.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolPrint = new Mock<ToolPrint>();

			toolPrint.Setup(x => x.Page).Returns(new Page());
			toolPrint.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolPrint.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolPrint.CallBase = true;
				var privateObject = new PrivateObject(toolPrint.Object, new PrivateType(typeof(ToolPrint)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolPrint.Object.ShouldSatisfyAllConditions(
					() => toolPrint.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolPrint.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolPrint.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
