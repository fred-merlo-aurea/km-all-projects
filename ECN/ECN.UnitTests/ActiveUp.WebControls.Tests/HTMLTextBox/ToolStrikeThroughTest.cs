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
	public class ToolStrikeThroughTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolStrikeThrough = new Mock<ToolStrikeThrough>();

			toolStrikeThrough.Setup(x => x.Page).Returns(new Page());
			toolStrikeThrough.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolStrikeThrough.CallBase = true;

				// Act
				toolStrikeThrough.Object.RenderDesign(output);

				// Assert
				toolStrikeThrough.Object.ShouldSatisfyAllConditions(
					() => toolStrikeThrough.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolStrikeThrough.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolStrikeThrough = new Mock<ToolStrikeThrough>();

			toolStrikeThrough.Setup(x => x.Page).Returns(new Page());
			toolStrikeThrough.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolStrikeThrough.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolStrikeThrough.CallBase = true;
				var privateObject = new PrivateObject(toolStrikeThrough.Object, new PrivateType(typeof(ToolStrikeThrough)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolStrikeThrough.Object.ShouldSatisfyAllConditions(
					() => toolStrikeThrough.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolStrikeThrough.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolStrikeThrough.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
