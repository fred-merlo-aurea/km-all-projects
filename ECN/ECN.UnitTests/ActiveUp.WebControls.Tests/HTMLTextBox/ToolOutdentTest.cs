﻿using System;
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
	public class ToolOutdentTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolOutdent = new Mock<ToolOutdent>();

			toolOutdent.Setup(x => x.Page).Returns(new Page());
			toolOutdent.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolOutdent.CallBase = true;

				// Act
				toolOutdent.Object.RenderDesign(output);

				// Assert
				toolOutdent.Object.ShouldSatisfyAllConditions(
					() => toolOutdent.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolOutdent.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolOutdent = new Mock<ToolOutdent>();

			toolOutdent.Setup(x => x.Page).Returns(new Page());
			toolOutdent.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolOutdent.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolOutdent.CallBase = true;
				var privateObject = new PrivateObject(toolOutdent.Object, new PrivateType(typeof(ToolOutdent)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolOutdent.Object.ShouldSatisfyAllConditions(
					() => toolOutdent.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolOutdent.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolOutdent.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}