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
	public class ToolUnorderedListTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolUnorderedList = new Mock<ToolUnorderedList>();

			toolUnorderedList.Setup(x => x.Page).Returns(new Page());
			toolUnorderedList.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolUnorderedList.CallBase = true;

				// Act
				toolUnorderedList.Object.RenderDesign(output);

				// Assert
				toolUnorderedList.Object.ShouldSatisfyAllConditions(
					() => toolUnorderedList.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolUnorderedList.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolUnorderedList = new Mock<ToolUnorderedList>();

			toolUnorderedList.Setup(x => x.Page).Returns(new Page());
			toolUnorderedList.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolUnorderedList.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolUnorderedList.CallBase = true;
				var privateObject = new PrivateObject(toolUnorderedList.Object, new PrivateType(typeof(ToolUnorderedList)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolUnorderedList.Object.ShouldSatisfyAllConditions(
					() => toolUnorderedList.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolUnorderedList.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolUnorderedList.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
