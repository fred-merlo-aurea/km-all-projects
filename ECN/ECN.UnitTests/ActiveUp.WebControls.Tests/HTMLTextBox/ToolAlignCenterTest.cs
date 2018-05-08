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
	public class ToolAlignCenterTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignCenter = new Mock<ToolAlignCenter>();

			toolAlignCenter.Setup(x => x.Page).Returns(new Page());
			toolAlignCenter.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolAlignCenter.CallBase = true;

				// Act
				toolAlignCenter.Object.RenderDesign(output);

				// Assert
				toolAlignCenter.Object.ShouldSatisfyAllConditions(
					() => toolAlignCenter.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignCenter.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignCenter = new Mock<ToolAlignCenter>();

			toolAlignCenter.Setup(x => x.Page).Returns(new Page());
			toolAlignCenter.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolAlignCenter.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolAlignCenter.CallBase = true;
				var privateObject = new PrivateObject(toolAlignCenter.Object, new PrivateType(typeof(ToolAlignCenter)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolAlignCenter.Object.ShouldSatisfyAllConditions(
					() => toolAlignCenter.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignCenter.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolAlignCenter.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
