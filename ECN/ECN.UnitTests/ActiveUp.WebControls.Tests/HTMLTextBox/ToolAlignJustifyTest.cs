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
	public class ToolAlignJustifyTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignJustify = new Mock<ToolAlignJustify>();

			toolAlignJustify.Setup(x => x.Page).Returns(new Page());
			toolAlignJustify.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolAlignJustify.CallBase = true;

				// Act
				toolAlignJustify.Object.RenderDesign(output);

				// Assert
				toolAlignJustify.Object.ShouldSatisfyAllConditions(
					() => toolAlignJustify.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignJustify.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolAlignJustify = new Mock<ToolAlignJustify>();

			toolAlignJustify.Setup(x => x.Page).Returns(new Page());
			toolAlignJustify.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolAlignJustify.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolAlignJustify.CallBase = true;
				var privateObject = new PrivateObject(toolAlignJustify.Object, new PrivateType(typeof(ToolAlignJustify)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolAlignJustify.Object.ShouldSatisfyAllConditions(
					() => toolAlignJustify.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolAlignJustify.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolAlignJustify.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
