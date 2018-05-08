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
	public class ToolCopyTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCopy = new Mock<ToolCopy>();

			toolCopy.Setup(x => x.Page).Returns(new Page());
			toolCopy.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolCopy.CallBase = true;

				// Act
				toolCopy.Object.RenderDesign(output);

				// Assert
				toolCopy.Object.ShouldSatisfyAllConditions(
					() => toolCopy.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCopy.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCopy = new Mock<ToolCopy>();

			toolCopy.Setup(x => x.Page).Returns(new Page());
			toolCopy.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolCopy.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolCopy.CallBase = true;
				var privateObject = new PrivateObject(toolCopy.Object, new PrivateType(typeof(ToolCopy)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolCopy.Object.ShouldSatisfyAllConditions(
					() => toolCopy.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCopy.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolCopy.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
