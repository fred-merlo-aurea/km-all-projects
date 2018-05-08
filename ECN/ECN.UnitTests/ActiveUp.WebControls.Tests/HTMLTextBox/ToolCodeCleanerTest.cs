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
	public class ToolCodeCleanerTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCodeCleaner = new Mock<ToolCodeCleaner>();

			toolCodeCleaner.Setup(x => x.Page).Returns(new Page());
			toolCodeCleaner.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolCodeCleaner.CallBase = true;

				// Act
				toolCodeCleaner.Object.RenderDesign(output);

				// Assert
				toolCodeCleaner.Object.ShouldSatisfyAllConditions(
					() => toolCodeCleaner.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCodeCleaner.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCodeCleaner = new Mock<ToolCodeCleaner>();

			toolCodeCleaner.Setup(x => x.Page).Returns(new Page());
			toolCodeCleaner.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolCodeCleaner.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolCodeCleaner.CallBase = true;
				var privateObject = new PrivateObject(toolCodeCleaner.Object, new PrivateType(typeof(ToolCodeCleaner)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolCodeCleaner.Object.ShouldSatisfyAllConditions(
					() => toolCodeCleaner.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCodeCleaner.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolCodeCleaner.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
