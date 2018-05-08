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
	public class ToolCutTest
	{
		private const string DummyImageUrl = "dummyImageUrl.gif";
		private const string DummyOverImageUrl = "dummyOverImageUrl.gif";
		private const string DummyClientSideClick = "clientsideclick";
		private const string MethodOnPreRender = "OnPreRender";

		[Test]
		public void RenderDesign_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCut = new Mock<ToolCut>();

			toolCut.Setup(x => x.Page).Returns(new Page());
			toolCut.Setup(x => x.RenderControl(It.IsAny<HtmlTextWriter>()));

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
				toolCut.CallBase = true;

				// Act
				toolCut.Object.RenderDesign(output);

				// Assert
				toolCut.Object.ShouldSatisfyAllConditions(
					() => toolCut.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCut.Object.OverImageURL.ShouldBe(DummyOverImageUrl));
			}
		}

		[Test]
		public void OnPreRender_SetImageUrl_ImageUrlAssigned()
		{
			// Arrange
			var toolCut = new Mock<ToolCut>();

			toolCut.Setup(x => x.Page).Returns(new Page());
			toolCut.Setup(x => x.ClientSideClick).Returns(DummyClientSideClick);
			toolCut.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());

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

				toolCut.CallBase = true;
				var privateObject = new PrivateObject(toolCut.Object, new PrivateType(typeof(ToolCut)));

				// Act
				privateObject.Invoke(MethodOnPreRender, new EventArgs());

				// Assert
				toolCut.Object.ShouldSatisfyAllConditions(
					() => toolCut.Object.ImageURL.ShouldBe(DummyImageUrl),
					() => toolCut.Object.OverImageURL.ShouldBe(DummyOverImageUrl),
					() => toolCut.Object.ClientSideClick.ShouldBe(DummyClientSideClick));
			}
		}
	}
}
