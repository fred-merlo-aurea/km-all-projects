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
	public class ToolItalicTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolItalic = new Mock<ToolItalic>();

			toolItalic.Setup(x => x.Page).Returns(new Page());
			toolItalic.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolItalic.CallBase = true;

				// Act
				toolItalic.Object.RenderDesign(output);

				// Assert
				toolItalic.Object.ShouldSatisfyAllConditions(
					() => toolItalic.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolItalic.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolItalic = new Mock<ToolItalic>();

			toolItalic.Setup(x => x.Page).Returns(new Page());
			toolItalic.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolItalic.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolItalic.CallBase = true;
				var privateObject = new PrivateObject(toolItalic.Object, new PrivateType(typeof(ToolItalic)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolItalic.Object.ShouldSatisfyAllConditions(
					() => toolItalic.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolItalic.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolItalic.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
