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
	public class ToolBoldTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolBold = new Mock<ToolBold>();

			toolBold.Setup(x => x.Page).Returns(new Page());
			toolBold.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolBold.CallBase = true;

				// Act
				toolBold.Object.RenderDesign(output);

				// Assert
				toolBold.Object.ShouldSatisfyAllConditions(
					() => toolBold.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolBold.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolBold = new Mock<ToolBold>();

			toolBold.Setup(x => x.Page).Returns(new Page());
			toolBold.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolBold.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolBold.CallBase = true;
				var privateObject = new PrivateObject(toolBold.Object, new PrivateType(typeof(ToolBold)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolBold.Object.ShouldSatisfyAllConditions(
					() => toolBold.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolBold.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolBold.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
