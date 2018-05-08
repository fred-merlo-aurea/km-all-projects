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
	public class ToolUnderlineTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolUnderline = new Mock<ToolUnderline>();

			toolUnderline.Setup(x => x.Page).Returns(new Page());
			toolUnderline.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolUnderline.CallBase = true;

				// Act
				toolUnderline.Object.RenderDesign(output);

				// Assert
				toolUnderline.Object.ShouldSatisfyAllConditions(
					() => toolUnderline.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolUnderline.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolUnderline = new Mock<ToolUnderline>();

			toolUnderline.Setup(x => x.Page).Returns(new Page());
			toolUnderline.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolUnderline.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolUnderline.CallBase = true;
				var privateObject = new PrivateObject(toolUnderline.Object, new PrivateType(typeof(ToolUnderline)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolUnderline.Object.ShouldSatisfyAllConditions(
					() => toolUnderline.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolUnderline.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolUnderline.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
