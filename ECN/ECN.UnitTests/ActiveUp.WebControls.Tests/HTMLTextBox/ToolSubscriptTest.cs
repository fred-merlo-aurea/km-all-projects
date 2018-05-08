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
	public class ToolSubscriptTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSubscript = new Mock<ToolSubscript>();

			toolSubscript.Setup(x => x.Page).Returns(new Page());
			toolSubscript.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolSubscript.CallBase = true;

				// Act
				toolSubscript.Object.RenderDesign(output);

				// Assert
				toolSubscript.Object.ShouldSatisfyAllConditions(
					() => toolSubscript.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSubscript.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSubscript = new Mock<ToolSubscript>();

			toolSubscript.Setup(x => x.Page).Returns(new Page());
			toolSubscript.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolSubscript.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolSubscript.CallBase = true;
				var privateObject = new PrivateObject(toolSubscript.Object, new PrivateType(typeof(ToolSubscript)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolSubscript.Object.ShouldSatisfyAllConditions(
					() => toolSubscript.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSubscript.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolSubscript.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
