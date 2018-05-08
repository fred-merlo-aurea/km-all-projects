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
	public class ToolAlignRightTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignRight = new Mock<ToolAlignRight>();

			toolAlignRight.Setup(x => x.Page).Returns(new Page());
			toolAlignRight.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolAlignRight.CallBase = true;

				// Act
				toolAlignRight.Object.RenderDesign(output);

				// Assert
				toolAlignRight.Object.ShouldSatisfyAllConditions(
					() => toolAlignRight.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignRight.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignRight = new Mock<ToolAlignRight>();

			toolAlignRight.Setup(x => x.Page).Returns(new Page());
			toolAlignRight.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolAlignRight.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolAlignRight.CallBase = true;
				var privateObject = new PrivateObject(toolAlignRight.Object, new PrivateType(typeof(ToolAlignRight)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolAlignRight.Object.ShouldSatisfyAllConditions(
					() => toolAlignRight.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignRight.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolAlignRight.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
