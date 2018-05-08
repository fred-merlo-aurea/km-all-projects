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
	public class ToolOrderedListTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolOrderedList = new Mock<ToolOrderedList>();

			toolOrderedList.Setup(x => x.Page).Returns(new Page());
			toolOrderedList.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolOrderedList.CallBase = true;

				// Act
				toolOrderedList.Object.RenderDesign(output);

				// Assert
				toolOrderedList.Object.ShouldSatisfyAllConditions(
					() => toolOrderedList.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolOrderedList.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolOrderedList = new Mock<ToolOrderedList>();

			toolOrderedList.Setup(x => x.Page).Returns(new Page());
			toolOrderedList.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolOrderedList.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolOrderedList.CallBase = true;
				var privateObject = new PrivateObject(toolOrderedList.Object, new PrivateType(typeof(ToolOrderedList)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolOrderedList.Object.ShouldSatisfyAllConditions(
					() => toolOrderedList.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolOrderedList.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolOrderedList.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
