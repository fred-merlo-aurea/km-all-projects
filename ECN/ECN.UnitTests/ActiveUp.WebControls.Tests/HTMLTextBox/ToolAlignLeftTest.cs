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
	public class ToolAlignLeftTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignLeft = new Mock<ToolAlignLeft>();

			toolAlignLeft.Setup(x => x.Page).Returns(new Page());
			toolAlignLeft.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolAlignLeft.CallBase = true;

				// Act
				toolAlignLeft.Object.RenderDesign(output);

				// Assert
				toolAlignLeft.Object.ShouldSatisfyAllConditions(
					() => toolAlignLeft.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignLeft.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignLeft = new Mock<ToolAlignLeft>();

			toolAlignLeft.Setup(x => x.Page).Returns(new Page());
			toolAlignLeft.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolAlignLeft.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolAlignLeft.CallBase = true;
				var privateObject = new PrivateObject(toolAlignLeft.Object, new PrivateType(typeof(ToolAlignLeft)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolAlignLeft.Object.ShouldSatisfyAllConditions(
					() => toolAlignLeft.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignLeft.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolAlignLeft.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
