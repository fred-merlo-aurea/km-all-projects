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
	public class ToolSelectAllTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSelectAll = new Mock<ToolSelectAll>();

			toolSelectAll.Setup(x => x.Page).Returns(new Page());
			toolSelectAll.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolSelectAll.CallBase = true;

				// Act
				toolSelectAll.Object.RenderDesign(output);

				// Assert
				toolSelectAll.Object.ShouldSatisfyAllConditions(
					() => toolSelectAll.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSelectAll.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolSelectAll = new Mock<ToolSelectAll>();
			var dummyPage = new Page();

			toolSelectAll.Setup(x => x.Page).Returns(new Page());
			toolSelectAll.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolSelectAll.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolSelectAll.CallBase = true;
				var privateObject = new PrivateObject(toolSelectAll.Object, new PrivateType(typeof(ToolSelectAll)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolSelectAll.Object.ShouldSatisfyAllConditions(
					() => toolSelectAll.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolSelectAll.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolSelectAll.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
