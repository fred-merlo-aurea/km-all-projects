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
	public class ToolIndentTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolIndent = new Mock<ToolIndent>();

			toolIndent.Setup(x => x.Page).Returns(new Page());
			toolIndent.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolIndent.CallBase = true;

				// Act
				toolIndent.Object.RenderDesign(output);

				// Assert
				toolIndent.Object.ShouldSatisfyAllConditions(
					() => toolIndent.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolIndent.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolIndent = new Mock<ToolIndent>();

			toolIndent.Setup(x => x.Page).Returns(new Page());
			toolIndent.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolIndent.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolIndent.CallBase = true;
				var privateObject = new PrivateObject(toolIndent.Object, new PrivateType(typeof(ToolIndent)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolIndent.Object.ShouldSatisfyAllConditions(
					() => toolIndent.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolIndent.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolIndent.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
